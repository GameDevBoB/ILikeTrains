using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour
{

    public static GameController instance;
    //[HideInInspector]
    //public GameObject[] enemies;
    //public GameObject enemyPrefab;
    public GameObject[] trapsPrefabs;
    public Train headCoach;
  

    private int bulletCount;
    public int[] trapsCosts;
    public int startResources = 10;
    [HideInInspector]
    public int totalResources;
    public bool isPaused = true;
    

    [HideInInspector]
    public bool trapIsBeingPlaced;
    public bool isPlaceable;
    //private int trapIndex = 0;
    private Vector3 screenPoint;
    private Vector3 offset;
    public GameObject selectedTrap;
    private int selectedTrapCost;
    //private GameObject previousTerrainHit;
    private RaycastHit hit;
    private LayerMask placeableLayer;
    
    private LayerMask unplaceableLayer;
    //private int enemyArrayCounter;
    // Use this for initialization
    void Awake()
    {
        Physics.queriesHitTriggers = false;
    }
    void Start()
    {
       
        instance = this;
        //enemyArrayCounter = 0;
        placeableLayer = 1 << LayerMask.NameToLayer("Placeable");
        unplaceableLayer = 1 << LayerMask.NameToLayer("Unplaceable");
        //SpawnEnemies();
        totalResources = startResources;
        
    }

    // Update is called once per frame
    void Update()
    {
        //trapIsBeingPlaced = true;
        if (!trapIsBeingPlaced && selectedTrap != null)
        {
            var mousePos = Input.mousePosition;
            screenPoint = Camera.main.ScreenToWorldPoint(mousePos);

            //selectedTrap.SetActive(true);
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.SphereCast(ray, 1.5f, out hit, 100.0f, placeableLayer | unplaceableLayer))
            {
                /*if(previousTerrainHit!=null)
                {
                    previousTerrainHit.SendMessage("BackToNormalColor");
                }*/
                //previousTerrainHit = hit.transform.gameObject;
                selectedTrap.SendMessage("ChangeColor", (hit.transform.gameObject.layer == LayerMask.NameToLayer("Placeable")) ? Color.green : Color.red);
                isPlaceable = (hit.transform.gameObject.layer == LayerMask.NameToLayer("Placeable"));
                MoveTrap();
            }
            if (Input.GetMouseButtonDown(0))
            {
                if (isPlaceable)
                    PlaceTrap();
                else
                {
                    Destroy(selectedTrap.gameObject);
                    /*if(previousTerrainHit != null)
                    	previousTerrainHit.SendMessage("BackToNormalColor");*/
                    GUIController.instance.ActivateInstanceButton();
                }
            }


        }
        //Debug.Log(Input.mousePosition);

    }

    /*void SpawnEnemies()
    {
        for (int i = 0; i < enemies.Length; i++)
        {
            enemies[i] = Instantiate(enemyPrefab, new Vector3(0, 0, 0), Quaternion.identity) as GameObject;
            enemyPrefab.SetActive(false);
        }
    }*/

    private void MoveTrap()
    {
        //selectedTrap.transform.position = hit.transform.position;
        /*float maxMovementX = hit.transform.position.x + (hit.transform.gameObject.GetComponent<Collider>().bounds.extents.x - selectedTrap.GetComponent<Collider>().bounds.extents.x);
		float minMovementX = hit.transform.position.x - (hit.transform.gameObject.GetComponent<Collider>().bounds.extents.x - selectedTrap.GetComponent<Collider>().bounds.extents.x);
		float maxMovementZ = hit.transform.position.z + (hit.transform.gameObject.GetComponent<Collider>().bounds.extents.z - selectedTrap.GetComponent<Collider>().bounds.extents.z);
		float minMovementZ = hit.transform.position.z - (hit.transform.gameObject.GetComponent<Collider>().bounds.extents.z - selectedTrap.GetComponent<Collider>().bounds.extents.z);
		if(screenPoint.x > minMovementX && screenPoint.x < maxMovementX && screenPoint.z > minMovementZ && screenPoint.z < maxMovementZ)*/
        selectedTrap.transform.position = new Vector3(screenPoint.x, hit.transform.position.y, screenPoint.z);
    }

    public void SelectTrap(int index)
    {

        trapIsBeingPlaced = false;
        isPlaceable = false;
        //trapIndex = index;
        if ((totalResources - trapsCosts[index]) >= 0)
        {
            selectedTrap = Instantiate(trapsPrefabs[index], Vector3.zero, trapsPrefabs[index].transform.rotation) as GameObject;
            //selectedTrap.SetActive(true);
            selectedTrapCost = trapsCosts[index];
            GUIController.instance.DeactivateInstanceButton();
        }
        //trapsInArray[index].transform.position = new Vector3(Camera.main.ScreenToWorldPoint(mousePos).x,0, Camera.main.ScreenToWorldPoint(mousePos).y);


    }

    /*public GameObject GetEnemy()
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
    }*/

    public void StartGame()
    {
        isPaused = false;
        GUIController.instance.StartGame();
    }

    public void PlaceTrap()
    {
        trapIsBeingPlaced = true;
        selectedTrap.SendMessage("BackToNormalColor");
        selectedTrap.layer = LayerMask.NameToLayer("Unplaceable");
        selectedTrap = null;
        //previousTerrainHit = null;
        //hit.transform.gameObject.SendMessage("SetUnplaceable");
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
