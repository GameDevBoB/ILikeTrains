using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour
{

    public static GameController instance;
    [HideInInspector]
    public GameObject[] enemies;
    public GameObject enemyPrefab;
    public GameObject[] trapsPrefab;
    public bool isPaused = true;
    private int enemyArrayCounter;
    private bool trapIsBeingPlaced;
    public bool isPlaceable;
    private int trapIndex=0;
    private Vector3 screenPoint;
    private Vector3 offset;
    private GameObject selectedTrap;
    // Use this for initialization
    void Awake()
    {
        Physics.queriesHitTriggers = false;
    }
    void Start()
    {
        instance = this;
        enemyArrayCounter = 0;
        SpawnEnemies();

    }

    // Update is called once per frame
    void Update()
    {
        isPlaceable = true;
        //trapIsBeingPlaced = true;
        if (!trapIsBeingPlaced && selectedTrap!= null)
        {
            var mousePos = Input.mousePosition;
            screenPoint = Camera.main.ScreenToWorldPoint(mousePos);

            //selectedTrap.SetActive(true);
            selectedTrap.transform.position = new Vector3(screenPoint.x, 0, screenPoint.z);
            if (Input.GetMouseButtonDown(0) && isPlaceable)
            {
                PlaceTrap();
            }
            
            
        }
        //Debug.Log(Input.mousePosition);
        
    }

    void SpawnEnemies()
    {
        for (int i = 0; i < enemies.Length; i++)
        {
            enemies[i] = Instantiate(enemyPrefab, new Vector3(0, 0, 0), Quaternion.identity) as GameObject;
            enemyPrefab.SetActive(false);
        }
    }


    public void SelectTrap(int index)
    {
        
        trapIsBeingPlaced = false;
        //trapIndex = index;
        selectedTrap = Instantiate(trapsPrefab[index], new Vector3(0, 0, 0), Quaternion.identity) as GameObject;
        selectedTrap.SetActive(true);
        GUIController.instance.DeactivateInstanceButton();
        //trapsInArray[index].transform.position = new Vector3(Camera.main.ScreenToWorldPoint(mousePos).x,0, Camera.main.ScreenToWorldPoint(mousePos).y);
        
       
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

    public void StartGame()
    {
        isPaused = false;
        GUIController.instance.StartGame();
    }

    public void PlaceTrap()
    {
        trapIsBeingPlaced = true;
        selectedTrap = null;
        GUIController.instance.ActivateInstanceButton();
        /*var mousePos = Input.mousePosition;
        screenPoint = Camera.main.ScreenToWorldPoint(mousePos);

        selectedTrap.SetActive(true);
        selectedTrap.transform.position = new Vector3(screenPoint.x, 0, screenPoint.z); */


        /*var screenPos = Input.mousePosition;
        screenPos.z = 20;
        var worldPos = Camera.main.ScreenToWorldPoint(screenPos);
        var newInstance = Instantiate(trapsPrefab[index], worldPos, Quaternion.identity);*/
    }
}
