using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour
{

    public static GameController instance;
    [HideInInspector]
    public GameObject[] enemies;
    public GameObject enemyPrefab;
    public GameObject[] trapsPrefabs;
    public int[] trapsCosts;
    public int startResources = 10;
    [HideInInspector]
    public int totalResources;
    public bool isPaused = true;
    private int enemyArrayCounter;
    private bool trapIsBeingPlaced;
    public bool isPlaceable;
    private int trapIndex=0;
    private Vector3 screenPoint;
    private Vector3 offset;
    private GameObject selectedTrap;
    private int selectedTrapCost;
    private GameObject previousTerrainHit;
    private RaycastHit hit;
    private LayerMask placeableLayer;
    private LayerMask unplaceableLayer;
    // Use this for initialization
    void Awake()
    {
        Physics.queriesHitTriggers = false;
    }
    void Start()
    {
        instance = this;
        enemyArrayCounter = 0;
        placeableLayer = 1 << LayerMask.NameToLayer("Placeable");
        unplaceableLayer = 1 << LayerMask.NameToLayer("Unplaceable");
        SpawnEnemies();
        totalResources = startResources;
    }

    // Update is called once per frame
    void Update()
    {
        //trapIsBeingPlaced = true;
        if (!trapIsBeingPlaced && selectedTrap!= null)
        {
            var mousePos = Input.mousePosition;
            screenPoint = Camera.main.ScreenToWorldPoint(mousePos);

            //selectedTrap.SetActive(true);
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit, 100.0f, placeableLayer | unplaceableLayer))
            {
                if(previousTerrainHit!=null)
                {
                    previousTerrainHit.SendMessage("BackToNormalColor");
                }
                previousTerrainHit = hit.transform.gameObject;
                previousTerrainHit.SendMessage("ChangeColor");
                selectedTrap.transform.position = hit.transform.position;
            }
            if (Input.GetMouseButtonDown(0))
            {
                if (isPlaceable)
                    PlaceTrap();
                else
                {
                    Destroy(selectedTrap.gameObject);
                    previousTerrainHit.SendMessage("BackToNormalColor");
                    GUIController.instance.ActivateInstanceButton();
                }
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
        isPlaceable = false;
        //trapIndex = index;
        if ((totalResources - trapsCosts[index]) >= 0)
        {
            selectedTrap = Instantiate(trapsPrefabs[index], new Vector3(0, 0, 0), Quaternion.identity) as GameObject;
            //selectedTrap.SetActive(true);
            selectedTrapCost = trapsCosts[index];
            GUIController.instance.DeactivateInstanceButton();
        }
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
        previousTerrainHit.SendMessage("BackToNormalColor");
        previousTerrainHit = null;
        hit.transform.gameObject.SendMessage("SetUnplaceable");
        GUIController.instance.ActivateInstanceButton();
        totalResources -= selectedTrapCost;
        /*var mousePos = Input.mousePosition;
        screenPoint = Camera.main.ScreenToWorldPoint(mousePos);

        selectedTrap.SetActive(true);
        selectedTrap.transform.position = new Vector3(screenPoint.x, 0, screenPoint.z); */


        /*var screenPos = Input.mousePosition;
        screenPos.z = 20;
        var worldPos = Camera.main.ScreenToWorldPoint(screenPos);
        var newInstance = Instantiate(trapsPrefab[index], worldPos, Quaternion.identity);*/
    }

    public void UpdateResources(int earning)
    {
        totalResources += earning;
    }
}
