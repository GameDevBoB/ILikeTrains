using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour
{

    public static GameController instance;
    [HideInInspector]
    public GameObject[] enemies;
    public GameObject enemyPrefab;
    public GameObject[] trapsPrefab;
    [HideInInspector]
    public GameObject[] trapsInArray;
    public bool isPaused = true;
    private int enemyArrayCounter;
    private bool trapIsBeingPlaced;
    public bool isPlaceable;
    private int trapIndex;
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
        if (trapIsBeingPlaced)
        {
            SelectTrap(trapIndex);
            PlaceTrap();
            
            
        }
        
    }

    void SpawnEnemies()
    {
        for (int i = 0; i < enemies.Length; i++)
        {
            enemies[i] = Instantiate(enemyPrefab, new Vector3(0, 0, 0), Quaternion.identity) as GameObject;
            enemyPrefab.SetActive(false);
        }
    }

    void InstantiateTraps()
    {
        for (int i = 0; i < trapsPrefab.Length; i++)
        {
            trapsInArray[i] = Instantiate(trapsPrefab[i], new Vector3(0, 0, 0), Quaternion.identity) as GameObject;
            trapsInArray[i].SetActive(false);
        }
    }

    public void SelectTrap(int index)
    {
        trapIsBeingPlaced = true;
        trapIndex = index;
        var mousePos = Input.mousePosition;
        trapsInArray[index].transform.position = new Vector3(Camera.main.ScreenToWorldPoint(mousePos).x,0, Camera.main.ScreenToWorldPoint(mousePos).y);

    }

    public GameObject GetEnemy()
    {
        //enemies[enemyArrayCounter].SetActive(true);
        int actualCounter = enemyArrayCounter;
        enemyArrayCounter++;
        if (enemyArrayCounter <= enemies.Length)
        {
            enemyArrayCounter++;
            
        }
        else
        {
            enemyArrayCounter = 0;
        }
        return enemies[actualCounter];
    }

    public void PlaceTrap()
    {
        if (Input.GetMouseButton(0) && isPlaceable)
        {
            trapIsBeingPlaced = false;
        }
    }
}
