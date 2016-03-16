using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour
{

    public static GameController instance;
    //DATA STRUCTURE FOR TRAPS VARIABLES
    //ARRAY OF DIFFEREN TRAPS
    public GameObject[] trapsPrefabs;
    //TRAP GAMEOBJECT CURRENTLY SELECTED
    public GameObject selectedTrap;
    public int[] trapsCosts;
    //

    public Train headCoach;
    public int startResources = 10;
    [HideInInspector]
    public int totalResources;
    //TRAPS RUNTIME STATUS 
    [HideInInspector]
    public bool trapIsBeingPlaced;
    public bool isPlaceable;
    //

    public bool isPaused = true;
    public int enemyCount;

    //MOUSE POSITION VARIABLES
    private Vector3 screenPoint;
    //
    private Vector3 offset;
    private int selectedTrapCost;
    private int bulletCount;
    private RaycastHit hit;
    //TRAPS POSITION LAYER NEEDED FOR RAYCASTS AND BEHAVIORS
    private LayerMask placeableLayer;
    private LayerMask unplaceableLayer;
    private int lastTrap;

    //


    //private int trapIndex = 0;
    //private GameObject previousTerrainHit;
    //private int enemyArrayCounter;

    void Awake()
    {
        Physics.queriesHitTriggers = false;
        instance = this;
    }
    void Start()
    {
        enemyCount = 0;
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

            //MOUSE POSITION LETTING PLAYER TO PLACE TRAP ON SCENE
            var mousePos = Input.mousePosition;
            screenPoint = Camera.main.ScreenToWorldPoint(mousePos);

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.SphereCast(ray, 1.5f, out hit, 100.0f, placeableLayer | unplaceableLayer))
            {
                //WE CHANGE THE COLOR OF THE TRAP DEPENDING ON WICH LAYER IT IS BEING PLACED
                selectedTrap.SendMessage("ChangeColor", (hit.transform.gameObject.layer == LayerMask.NameToLayer("Placeable")) ? Color.green : Color.red);
                isPlaceable = (hit.transform.gameObject.layer == LayerMask.NameToLayer("Placeable"));
                //AFTER SELECTING TRAP WE CAN MOVE IT CALLING THE METHOD
                MoveTrap();
                //
            }
            if (Input.GetMouseButtonDown(0))
            {
                //AT LEFT CLICK WE PLACE THE TRAP ON MOUSE POSITION
                if (isPlaceable)
                    PlaceTrap();
                else
                {
                    DeselectTrap();
                }
            }


        }
    }

    private void MoveTrap()
    {
        //TEST
        //selectedTrap.transform.position = hit.transform.position;
        /*float maxMovementX = hit.transform.position.x + (hit.transform.gameObject.GetComponent<Collider>().bounds.extents.x - selectedTrap.GetComponent<Collider>().bounds.extents.x);
		float minMovementX = hit.transform.position.x - (hit.transform.gameObject.GetComponent<Collider>().bounds.extents.x - selectedTrap.GetComponent<Collider>().bounds.extents.x);
		float maxMovementZ = hit.transform.position.z + (hit.transform.gameObject.GetComponent<Collider>().bounds.extents.z - selectedTrap.GetComponent<Collider>().bounds.extents.z);
		float minMovementZ = hit.transform.position.z - (hit.transform.gameObject.GetComponent<Collider>().bounds.extents.z - selectedTrap.GetComponent<Collider>().bounds.extents.z);
		if(screenPoint.x > minMovementX && screenPoint.x < maxMovementX && screenPoint.z > minMovementZ && screenPoint.z < maxMovementZ)*/
        //TEST

        //WE MOVE THE TRAP AROUND
        selectedTrap.transform.position = new Vector3(screenPoint.x, hit.transform.position.y, screenPoint.z);
        if (Input.GetMouseButtonDown(1))
        {
            DeselectTrap();
            trapIsBeingPlaced = true;
            GUIController.instance.startButton.interactable = true;
        }
    }

    public void SelectTrap(int index)
    {
        lastTrap = index;
        trapIsBeingPlaced = false;
        isPlaceable = false;
        //WE SELECT THE NEXT TRAP ONLY IF WE CAN AFFORD IT
        if ((totalResources - trapsCosts[index]) >= 0)
        //
        {
            //WE INSTANTIATE THE SELECTED TRAP
            selectedTrap = Instantiate(trapsPrefabs[index], new Vector3(0, -1, 0), trapsPrefabs[index].transform.rotation) as GameObject;
            selectedTrapCost = trapsCosts[index];
            GUIController.instance.DeactivateTrapsButton();
        }
        GUIController.instance.startButton.interactable = false;
    }


    public void StartGame()
    {
        DeselectTrap();
        isPaused = false;
        GUIController.instance.StartGame();
    }

    public void PlaceTrap()
    {
        //PUT THE TRAP AT MOUSEPOINT CHANGING ITS LAYER SO WE DO NOT RISK TO "STACK" THEM ON TOP OF EACH OTHER
        trapIsBeingPlaced = true;
        selectedTrap.SendMessage("BackToNormalColor");
        selectedTrap.layer = LayerMask.NameToLayer("Unplaceable");
        selectedTrap = null;
        GUIController.instance.ActivateTrapsButton();
        totalResources -= selectedTrapCost;
        SelectTrap(lastTrap);

    }

    public void UpdateResources(int earning)
    {
        //EARNNG COULD BE POSITIVE OR NEGATIVE DEPENDING ON WE ARE SPENDING RESOURCES TO BUY STUFF OR WE KILLING ENEMIES
        totalResources += earning;
    }

    void DeselectTrap()
    {
        Destroy(selectedTrap.gameObject);
        GUIController.instance.startButton.interactable = true;
        GUIController.instance.ActivateTrapsButton();
    }

    public void StopGame()
    {
        isPaused = true;
        Time.timeScale = 0;
    }

    public void TotalEnemyKilled()
    {
        enemyCount++;
    }
}
