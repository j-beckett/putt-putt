using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneController : MonoBehaviour
{

    [SerializeField] private GameObject enemyPrefab;
    private GameObject enemy;
    private Vector3 spawnPoint = new Vector3(0, 0, 5);

    private int numEnemies = 5;
    private GameObject[] enemyArr;


    [SerializeField] private GameObject iguanaPrefab;
    private GameObject iguana;
    private GameObject[] iguanaArr;
    private int numIguanas = 13;
    private Vector3 iguanaSpawnPoint = new Vector3(-5, 0, -13);

    [SerializeField] private UIController ui;

    private int score = 0;

    void Awake()
    {
        Messenger.AddListener(GameEvent.ENEMY_DEAD, this.OnEnemyDead);
        Messenger<int>.AddListener(GameEvent.DIFFICULTY_CHANGED, OnDifficultyChanged);
    }
    void OnDestroy()
    {
        Messenger.RemoveListener(GameEvent.ENEMY_DEAD, this.OnEnemyDead);
        Messenger<int>.RemoveListener(GameEvent.DIFFICULTY_CHANGED, OnDifficultyChanged);
    }
    void OnEnemyDead()
    {
        score++;
        ui.UpdateScore(score);
    }

    private void OnDifficultyChanged(int newDifficulty)
    {
        Debug.Log("Scene.OnDifficultyChanged(" + newDifficulty + ")");
        for (int i = 0; i < enemyArr.Length; i++)
        {
            WanderingAI ai = enemyArr[i].GetComponent<WanderingAI>();
            ai.SetDifficulty(newDifficulty);
        }
    }

    int getDifficulty() {

        return PlayerPrefs.GetInt("difficulty", 1);
    }

        // Start is called before the first frame update
        void Start()
    {
        enemyArr = new GameObject[numEnemies];
        for (int i = 0; i < numEnemies; i++)
        {
            enemyArr[i] = null;
        }


        iguanaArr = new GameObject[numIguanas];
        for (int i = 0; i < numIguanas; i++)
        {
            iguanaArr[i] = null;
        }

        for (int i = 0; i < numIguanas; i++)
        {
            if (iguanaArr[i] == null)
            {
                iguana = Instantiate(iguanaPrefab) as GameObject;
                iguana.transform.position = iguanaSpawnPoint;
                float angle = Random.Range(0, 360);
                iguana.transform.Rotate(0, angle, 0);

                iguanaArr[i] = iguana;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {

        for (int i = 0; i < numEnemies; i++)
        {
            if (enemyArr[i] == null)
            {
                enemy = Instantiate(enemyPrefab) as GameObject;
                enemy.transform.position = spawnPoint;
                float angle = Random.Range(0, 360);
                enemy.transform.Rotate(0, angle, 0);
                
                enemyArr[i] = enemy;
                WanderingAI ai = enemyArr[i].GetComponent<WanderingAI>();
                ai.SetDifficulty(getDifficulty());
            }
        }
    }
}
