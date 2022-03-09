using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnemyStates { alive, dead };

public class WanderingAI : MonoBehaviour
{
    private float enemySpeed = 1f; 
    private float obstacleRange = 5.0f;
    private float sphereRadius = 0.75f;

    private EnemyStates state;

    [SerializeField]
    private GameObject laserbeamPrefab;
    private GameObject laserbeam;

    public float fireRate = 2.0f; 
    private float nextFire = 0.0f;

    public void ChangeState(EnemyStates state) {
        this.state = state; 
    }

    // Start is called before the first frame update
    void Start()
    {
        state = EnemyStates.alive;
    }

    // Update is called once per frame
    void Update()
    {
        if (state == EnemyStates.alive)
        {
            //move enemy
            transform.Translate(Vector3.forward * enemySpeed * Time.deltaTime);
            // generate Ray
            Ray ray = new Ray(transform.position, transform.forward);
            // Spherecast and determine if Enemy needs to turn

            RaycastHit hit;

            if (Physics.SphereCast(ray, sphereRadius, out hit))
            {
                GameObject hitObject = hit.transform.gameObject;
                if (hitObject.GetComponent<PlayerCharacter>())
                { // Spherecast hit Player, fire laser!
                    if (laserbeam == null && Time.time > nextFire)
                    {
                        nextFire = Time.time + fireRate;
                        laserbeam = Instantiate(laserbeamPrefab) as GameObject; 
                        laserbeam.transform.position = transform.TransformPoint(0, 1.5f, 1.5f); 
                        laserbeam.transform.rotation = transform.rotation;

                    }


                } else if (hit.distance < obstacleRange) { // Must've hit wall or other object, turn
                    float turnAngle = Random.Range(-110, 110); transform.Rotate(0, turnAngle, 0);
                }
            }

        }
    }

    private void OnDrawGizmos() //it doesn't work?
    {
        Gizmos.color = Color.red;//set color

        //determine the range vector (at enemy)
        Vector3 rangeTest = transform.position + transform.forward * obstacleRange;

        //line to represent range vector
        Debug.DrawLine(transform.position, rangeTest);

        //draw a wire spehere at the point on the end of the range vector
        Gizmos.DrawWireSphere(rangeTest, sphereRadius);
    }


}

