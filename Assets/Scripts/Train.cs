using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Train : MonoBehaviour
{

    public float life;
    public float sprint;
    public Transform[] waypoints;
    public float speed = 1.0F;
    public float[] upgradeHealthPointsArray = new float[5];
    public float[] upgradeSprintArray = new float[5];
    public float[] upgradeSpeedArray = new float[5];
    public int[] upgradeCost = new int[5];
    public bool loop = false;
    public float slowDelay;
    public float actualLife;
    public Transform frontPivot;
    public Transform rearPivot;
    public float maxDistance;
    public float distance;
    public bool isCoach;
    public float rotationSpeed;
    




    private float startTime;
    private float journeyLength;
    private int countWaypoints;
    private int healthUpgradeCounter;
    private int sprintUpgradeCounter;
    private int speedUpgradeCounter;
    private float initialSpeed;
    private float slowTimer;
    private bool trainIsSlowed;
    //private Transform startPosition;


    void Start()
    {
        initialSpeed = speed;
        healthUpgradeCounter = 0;
        sprintUpgradeCounter = 0;
        speedUpgradeCounter = 0;
        actualLife = life;
        //startPosition.position =  new Vector3(waypoints[0].GetChild(0).position.x, waypoints[0].GetChild(0).position.y+.5f, waypoints[0].GetChild(0).position.z);
        //transform.position = startPosition.position;
        transform.position = waypoints[0].GetChild(0).position;
        countWaypoints = 0;
        SetSpeedGui();
        SetSprintGui();
        SetHealthGui();
        //journeyLength = Vector3.Distance(startMarker.position, endMarker.position);

    }


    void Update()
    {
        CheckButtonInteractable();
        if (!GameController.instance.isPaused)
        {

            /*if(startTime == 0)
                startTime = Time.time;
            float distCovered = (Time.time - startTime) * speed;
            float fracJourney = distCovered / journeyLength;
            transform.position = Vector3.Lerp(startMarker.position, endMarker.position, fracJourney);*/
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
                        transform.position = waypoints[0].GetChild(0).position;

                    }
                    transform.LookAt(waypoints[countWaypoints].GetChild(0).position);
                }
                transform.position = Vector3.MoveTowards(transform.position, waypoints[countWaypoints].GetChild(0).position, Time.deltaTime * speed);
            }
            else
            {
                if (Vector3.Distance(transform.position, waypoints[countWaypoints].GetChild(0).position) <= 0.01)
                {
                    if ((countWaypoints < (waypoints.Length - 1)))
                    {
                        countWaypoints++;
                    }
                    else
                    {

                        countWaypoints = 0;
                        transform.position = waypoints[0].GetChild(0).position;

                    }
                   
                   transform.LookAt(waypoints[countWaypoints].GetChild(0).position);
                }
                if (Vector3.Distance(frontPivot.position, rearPivot.position) >= maxDistance)
                {
                    transform.position = Vector3.MoveTowards(transform.position, waypoints[countWaypoints].GetChild(0).position, Time.deltaTime * GameController.instance.headCoach.speed * 10);
                }
                if (Vector3.Distance(frontPivot.position, rearPivot.position) >= distance)
                {
                    transform.position = Vector3.MoveTowards(transform.position, waypoints[countWaypoints].GetChild(0).position, Time.deltaTime * GameController.instance.headCoach.speed);
                }
            }
        }
       

    }

    public void Activate()
    {
        this.gameObject.SetActive(true);
    }

    public void GetDamage(int damage)
    {
        actualLife -= damage;
        if (actualLife <= 0)
           transform.parent.gameObject.SetActive(false);
    }

    public void Upgrade(int myUpgrade)
    {
        switch (myUpgrade)
        {
            case 0:
                speed = upgradeSpeedArray[speedUpgradeCounter];
                GameController.instance.UpdateResources(-upgradeCost[speedUpgradeCounter]);
                speedUpgradeCounter++;
                if (speedUpgradeCounter < upgradeSpeedArray.Length)
                {
                    SetSpeedGui();
                }
                break;
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
    }
    void SetSlow(int speedRate)
    {
        if (((Time.time - slowTimer) > slowDelay) || slowTimer == 0)
        {
            speed = speedRate;
            slowTimer = Time.time;

        }
        else
        {
            speed = initialSpeed;
        }
    }

    void OnMouseOver()
    {
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
    }
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
}