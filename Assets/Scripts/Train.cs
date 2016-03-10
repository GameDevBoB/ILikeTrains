using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Train : MonoBehaviour
{
    //HQ VARIABLES
    public float life;
    public float sprint;
    public float speed = 1.0F;
    public bool loop = false;
    public float slowDelay;
    public float actualLife;
    public bool isCoach;
    //

    //HQ PATHPOINTS
    public Transform[] waypoints;
    //

    //ARRAYS OF UPGRADE VAULES
    public float[] upgradeHealthPointsArray = new float[5];
    public float[] upgradeSprintArray = new float[5];
    public float[] upgradeSpeedArray = new float[5];
    public int[] upgradeCost = new int[5];
    //

    //JOIN POINT THORUGH HQ ELEMENTS LIKE COACHES
    public Transform frontPivot;
    public Transform rearPivot;
    //

    //VARIABLES THAT INDICATES THE DISTANCE BETWEEN ELEMENTS
    public float maxDistance;
    public float distance;
    //


    public float rotationSpeed;
    public float sprintRatio = 0.5f;
    public GameObject trainToCopyFrom;


    private float startTime;
    private float journeyLength;

    //SUPPORT VARIABLES 
    private int countWaypoints;
    private int healthUpgradeCounter;
    private int sprintUpgradeCounter;
    private int speedUpgradeCounter;
    private float initialSpeed;
    private float slowTimer;
    private float sprintTimer;
    private bool trainIsSlowed;
    //

    private Vector3 initPos;

    void Awake()
    {

        if (isCoach)
        {
            //GETTING THE PATHPOINT OF HQ GIVING THEM TO ALL ELEMENTS ATTACHED TO IT
            for (int i = 0; i < trainToCopyFrom.GetComponent<Train>().waypoints.Length; i++)
            {
                waypoints[i] = trainToCopyFrom.GetComponent<Train>().waypoints[i];
            }
            loop = trainToCopyFrom.GetComponent<Train>().loop;
        }

    }

    void Start()
    {

        //SETTING INITIAL PARAMETERS
        initPos = transform.position;
        transform.position = initPos;
        initialSpeed = speed;
        healthUpgradeCounter = 0;
        sprintUpgradeCounter = 0;
        speedUpgradeCounter = 0;
        actualLife = life;
        countWaypoints = 0;
        GUIController.instance.healthSlider.value = actualLife;
        SetSpeedGui();
        SetSprintGui();
        SetHealthGui();
        //

    }


    void FixedUpdate()
    {

        //GETTING STATIC VALUES REFERENCES TO THE HQ INSTANCE
        loop = GameController.instance.headCoach.loop;
        speed = GameController.instance.headCoach.speed;
        //
       
        CheckButtonInteractable();
        if (!GameController.instance.isPaused)
        {
            
            //CHECKING IF ITS HQ 
            if (!isCoach)
            {
                if (Vector3.Distance(transform.position, waypoints[countWaypoints].GetChild(0).position) <= 0.01)
                {
                    if ((countWaypoints < (waypoints.Length - 1)))
                    {
                        countWaypoints++;
                    }
                    else if (loop)
                    {

                        countWaypoints = 0;
                        //transform.position = waypoints[0].GetChild(0).position;
                        transform.position = initPos;

                    }
                    transform.LookAt(waypoints[countWaypoints].GetChild(0).position);
                }
                transform.position = Vector3.MoveTowards(transform.position, waypoints[countWaypoints].GetChild(0).position, Time.deltaTime * speed);
            }
            //
            //IF NOT WE MOVE THE COACHES BEHIND IT ON A CONVOY FORMATION
            else
            {
                if (Vector3.Distance(frontPivot.position, rearPivot.position) >= maxDistance)
                {
                    //FORCING THE DISTANCE BETWEEN ELEMENTS TO A FIXED DISTANCE EVERY FRAME
                    transform.position = Vector3.MoveTowards(transform.position, waypoints[countWaypoints].GetChild(0).position, Time.deltaTime * speed * 2);
                    //

                }

                //WE MOVE THE NEXT ELEMENT ONLY IF THAT DISTANCE IS EXCEEDED
                if (Vector3.Distance(frontPivot.position, rearPivot.position) >= distance)
                {
                    if (Vector3.Distance(transform.position, waypoints[countWaypoints].GetChild(0).position) <= 0.01)
                    {
                        if ((countWaypoints < (waypoints.Length - 1)))
                        {
                            countWaypoints++;
                        }
                        else if (loop)
                        {

                            countWaypoints = 0;
                            //transform.position = waypoints[0].GetChild(0).position;
                            transform.position = initPos;

                        }

                        transform.LookAt(waypoints[countWaypoints].GetChild(0).position);
                    }
                    transform.position = Vector3.MoveTowards(transform.position, waypoints[countWaypoints].GetChild(0).position, Time.deltaTime * speed);
                }

                //
            }
        }


    }

    public void Activate()
    {
        this.gameObject.SetActive(true);
    }

    public void GetDamage(int damage)
    {

        //WE SET THE HP VALUE OF THE HQ EVERY TIME IT GET DAMAGE DEACTIVATING IT IF THE HP IS EQUAL OR BELOW ZERO
        actualLife -= damage;
        GUIController.instance.healthSlider.value = actualLife;
        if (actualLife <= 0)
            transform.parent.gameObject.SetActive(false);
        //

    }

    public void Upgrade(int myUpgrade)
    {
        //GETTING THE UPGRADE TYPE INCREMENTING THE VALUE GOT FROM THE ARRAY OF VALUES RELATIVE
        //TO THE SELECTED UPGRADE
        switch (myUpgrade)
        {
            //SPEED UPGRADE
            case 0:
                speed = upgradeSpeedArray[speedUpgradeCounter];
                GameController.instance.UpdateResources(-upgradeCost[speedUpgradeCounter]);
                speedUpgradeCounter++;
                if (speedUpgradeCounter < upgradeSpeedArray.Length)
                {
                    SetSpeedGui();
                }
                break;
            //

            //HP UPGRADE
            case 1:
                float lifeDifference;
                lifeDifference = life - actualLife;
                life = actualLife = upgradeHealthPointsArray[healthUpgradeCounter];
                actualLife -= lifeDifference;
                GameController.instance.UpdateResources(-upgradeCost[healthUpgradeCounter]);
                healthUpgradeCounter++;
                if (healthUpgradeCounter < upgradeHealthPointsArray.Length)
                {
                    SetHealthGui();
                }
                break;
            //

            //SPRINT UPGRADEs
            case 2:
                sprint = upgradeSprintArray[sprintUpgradeCounter];
                GameController.instance.UpdateResources(-upgradeCost[sprintUpgradeCounter]);
                sprintUpgradeCounter++;
                if (sprintUpgradeCounter < upgradeSprintArray.Length)
                {
                    SetSprintGui();
                }
                break;
        }
        //
    }


    void OnMouseOver()
    {
/*
        //ACTIVATE THE CANVAS RELATIVE TO THE HQ UPGRADE
        if (Input.GetMouseButtonDown(1))
        {
            if (!GUIController.instance.upgradeTrainCanvas.gameObject.activeSelf)
            {
                GUIController.instance.upgradeTrainCanvas.gameObject.SetActive(true);
            }
            else
            {
                GUIController.instance.upgradeTrainCanvas.gameObject.SetActive(false);
            }
        }
        //
        */
    }
    //SETTING THE GUI AND THE BUTTONS RELATIVE TO THE UPGRADE SELECTED
    private void SetSpeedGui()
    {

        GUIController.instance.trainSpeedUpgradeText.text = "Speed " + upgradeSpeedArray[speedUpgradeCounter];
        GUIController.instance.trainSpeedUpgradeButton.transform.GetChild(0).GetComponent<Text>().text = "Cost " + upgradeCost[speedUpgradeCounter];

    }
    private void SetHealthGui()
    {

        GUIController.instance.trainHealthUpgradeText.text = "Health " + upgradeHealthPointsArray[healthUpgradeCounter];
        GUIController.instance.trainHealthUpgradeButton.transform.GetChild(0).GetComponent<Text>().text = "Cost " + upgradeCost[healthUpgradeCounter];

    }
    private void SetSprintGui()
    {

        GUIController.instance.trainSprintUpgradeText.text = "Sprint " + upgradeSprintArray[sprintUpgradeCounter];
        GUIController.instance.trainSprintUpgradeButton.transform.GetChild(0).GetComponent<Text>().text = "Cost " + upgradeCost[sprintUpgradeCounter];

    }
    //


    //CHECKING IF WE HAVE ENOUGH RESOURCES TO SPEND ON MORE UPGRADES
    //IN CASE WE DONT HAVE ENOUGH RESOURCES WE DEACIVATE THE BUTTONS OF THAT UPGRADE
    private void CheckButtonInteractable()
    {
        if (GUIController.instance.upgradeTrainCanvas.gameObject.activeSelf)
        {
            if (speedUpgradeCounter < upgradeSpeedArray.Length && GameController.instance.totalResources >= upgradeCost[speedUpgradeCounter])
            {
                GUIController.instance.trainSpeedUpgradeButton.interactable = true;
            }
            else
            {
                GUIController.instance.trainSpeedUpgradeButton.interactable = false;
            }
            if (healthUpgradeCounter < upgradeHealthPointsArray.Length && GameController.instance.totalResources >= upgradeCost[healthUpgradeCounter])
            {
                GUIController.instance.trainHealthUpgradeButton.interactable = true;
            }
            else
            {
                GUIController.instance.trainHealthUpgradeButton.interactable = false;
            }
            if (sprintUpgradeCounter < upgradeSprintArray.Length && GameController.instance.totalResources >= upgradeCost[sprintUpgradeCounter])
            {
                GUIController.instance.trainSprintUpgradeButton.interactable = true;
            }
            else
            {
                GUIController.instance.trainSprintUpgradeButton.interactable = false;
            }
        }
    }

    void SetSlow(int speedRate)
    {

        //SLOWING THE HQ
        if (((Time.time - slowTimer) > slowDelay) || slowTimer == 0)
        {
            speed = speedRate;
            slowTimer = Time.time;

        }
        else
        {
            speed = initialSpeed;
        }
        //

    }

    public void SetSprint()
    {


        //SPRINTING THE HQ

       

        //

    }
    //////////


}