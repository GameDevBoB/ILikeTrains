using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour
{

    public static GameController instance;
    [HideInInspector]
    public GameObject[] enemies;
    public GameObject enemy;
    public GameObject[] traps;
    [HideInInspector]
    public GameObject[] trapsInArray;
    public bool isPaused = true;
    private int enemyArrayCounter;
    // Use this for initialization
    void Start()
    {
        instance = this;
        enemyArrayCounter = 0;
        SpawnEnemies();
        InstantiateTraps();

    }

    // Update is called once per frame
    void Update()
    {

    }

    void SpawnEnemies()
    {
        for (int i = 0; i < enemies.Length; i++)
        {
            enemies[i] = Instantiate(enemy, new Vector3(0, 0, 0), Quaternion.identity) as GameObject;
            enemy.SetActive(false);
        }
    }

    void InstantiateTraps()
    {
        for (int i = 0; i < traps.Length; i++)
        {
            trapsInArray[i] = Instantiate(traps[i], new Vector3(0, 0, 0), Quaternion.identity) as GameObject;
            trapsInArray[i].SetActive(false);
        }
    }

    public void PlaceTraps(int index)
    {

        var mousePos = Input.mousePosition;
        trapsInArray[index].transform.position = Camera.main.ScreenToWorldPoint(mousePos);
    }

    public void GetEnemy()
    {
        enemies[enemyArrayCounter].SetActive(true);
        enemyArrayCounter++;
    }
}
