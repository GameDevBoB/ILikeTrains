using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour
{

    public static GameController instance;
    [HideInInspector]
    public GameObject[] enemies;
    public GameObject enemyPrefab;
<<<<<<< HEAD
=======
<<<<<<< HEAD
>>>>>>> origin/master
    public GameObject[] trapsPrefabs;
    public int[] trapsCosts;
    public int startResources = 10;
    [HideInInspector]
    public int totalResources;
<<<<<<< HEAD
=======
=======
    public GameObject[] trapsPrefab;
>>>>>>> origin/master
>>>>>>> origin/master
    public bool isPaused = true;
    private int enemyArrayCounter;
    private bool trapIsBeingPlaced;
    public bool isPlaceable;
<<<<<<< HEAD
=======
<<<<<<< HEAD
=======
<<<<<<< HEAD
>>>>>>> origin/master
>>>>>>> origin/master
    private int trapIndex=0;
    private Vector3 screenPoint;
    private Vector3 offset;
    private GameObject selectedTrap;
<<<<<<< HEAD
    private int selectedTrapCost;
=======
<<<<<<< HEAD
    private int selectedTrapCost;
=======
>>>>>>> origin/master
>>>>>>> origin/master
    private GameObject previousTerrainHit;
    private RaycastHit hit;
    private LayerMask placeableLayer;
    private LayerMask unplaceableLayer;
<<<<<<< HEAD
=======
<<<<<<< HEAD
=======
=======
    private Vector3 screenPoint;
    private Vector3 offset;
    private GameObject selectedTrap;
>>>>>>> origin/master
>>>>>>> origin/master
>>>>>>> origin/master
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
<<<<<<< HEAD
        totalResources = startResources;
=======
<<<<<<< HEAD
        totalResources = startResources;
=======

>>>>>>> origin/master
>>>>>>> origin/master
    }

    // Update is called once per frame
    void Update()
    {
<<<<<<< HEAD
=======
<<<<<<< HEAD
=======
<<<<<<< HEAD
=======
        isPlaceable = true;
>>>>>>> origin/master
>>>>>>> origin/master
>>>>>>> origin/master
        //trapIsBeingPlaced = true;
        if (!trapIsBeingPlaced && selectedTrap!= null)
        {
            var mousePos = Input.mousePosition;
            screenPoint = Camera.main.ScreenToWorldPoint(mousePos);

            //selectedTrap.SetActive(true);
<<<<<<< HEAD
=======
<<<<<<< HEAD
=======
<<<<<<< HEAD
>>>>>>> origin/master
>>>>>>> origin/master
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
<<<<<<< HEAD
=======
<<<<<<< HEAD
=======
=======
            selectedTrap.transform.position = new Vector3(screenPoint.x, 0, screenPoint.z);
            if (Input.GetMouseButtonDown(0) && isPlaceable)
            {
                PlaceTrap();
>>>>>>> origin/master
>>>>>>> origin/master
>>>>>>> origin/master
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
<<<<<<< HEAD
=======
<<<<<<< HEAD
>>>>>>> origin/master
        isPlaceable = false;
        //trapIndex = index;
        if ((totalResources - trapsCosts[index]) >= 0)
        {
            selectedTrap = Instantiate(trapsPrefabs[index], new Vector3(0, 0, 0), Quaternion.identity) as GameObject;
            //selectedTrap.SetActive(true);
            selectedTrapCost = trapsCosts[index];
            GUIController.instance.DeactivateInstanceButton();
        }
<<<<<<< HEAD
=======
=======
<<<<<<< HEAD
        isPlaceable = false;
=======
>>>>>>> origin/master
        //trapIndex = index;
        selectedTrap = Instantiate(trapsPrefab[index], new Vector3(0, 0, 0), Quaternion.identity) as GameObject;
        selectedTrap.SetActive(true);
        GUIController.instance.DeactivateInstanceButton();
>>>>>>> origin/master
>>>>>>> origin/master
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
<<<<<<< HEAD
=======
<<<<<<< HEAD
>>>>>>> origin/master
        previousTerrainHit.SendMessage("BackToNormalColor");
        previousTerrainHit = null;
        hit.transform.gameObject.SendMessage("SetUnplaceable");
        GUIController.instance.ActivateInstanceButton();
        totalResources -= selectedTrapCost;
<<<<<<< HEAD
=======
=======
<<<<<<< HEAD
        previousTerrainHit.SendMessage("BackToNormalColor");
        previousTerrainHit = null;
        hit.transform.gameObject.SendMessage("SetUnplaceable");
=======
>>>>>>> origin/master
        GUIController.instance.ActivateInstanceButton();
>>>>>>> origin/master
>>>>>>> origin/master
        /*var mousePos = Input.mousePosition;
        screenPoint = Camera.main.ScreenToWorldPoint(mousePos);

        selectedTrap.SetActive(true);
        selectedTrap.transform.position = new Vector3(screenPoint.x, 0, screenPoint.z); */


        /*var screenPos = Input.mousePosition;
        screenPos.z = 20;
        var worldPos = Camera.main.ScreenToWorldPoint(screenPos);
        var newInstance = Instantiate(trapsPrefab[index], worldPos, Quaternion.identity);*/
<<<<<<< HEAD
=======
<<<<<<< HEAD
>>>>>>> origin/master
    }

    public void UpdateResources(int earning)
    {
        totalResources += earning;
<<<<<<< HEAD
=======
=======
>>>>>>> origin/master
>>>>>>> origin/master
    }
}
