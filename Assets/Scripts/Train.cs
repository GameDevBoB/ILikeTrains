using UnityEngine;
using System.Collections;

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



    private float startTime;
    private float actualLife;
    //private bool SetActive (bool value);
    private float journeyLength;
    private int countWaypoints;
    private int healthUpgradeCounter;
    private int sprintUpgradeCounter;
    private int speedUpgradeCounter;
    private int upgradeCostCounter;
    private float initialSpeed;
    private float slowTimer;
    private bool trainIsSlowed;

    void Start()
    {
        initialSpeed = speed;
        healthUpgradeCounter = 0;
        sprintUpgradeCounter = 0;
        speedUpgradeCounter = 0;
        actualLife = life;
        transform.position = waypoints[0].GetChild(0).position;
        countWaypoints = 0;
        //journeyLength = Vector3.Distance(startMarker.position, endMarker.position);

    }


    void Update()
    {
        if (!GameController.instance.isPaused)
        {
            /*if(startTime == 0)
                startTime = Time.time;
            float distCovered = (Time.time - startTime) * speed;
            float fracJourney = distCovered / journeyLength;
            transform.position = Vector3.Lerp(startMarker.position, endMarker.position, fracJourney);*/
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

    }

    public void Activate()
    {
        this.gameObject.SetActive(true);
    }

    public void GetDamage(int damage)
    {
        actualLife -= damage;
        if (actualLife <= 0)
            this.gameObject.SetActive(false);
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
                    GUIController.instance.trainSpeedUpgradeText.text = "Speed " + upgradeSpeedArray[speedUpgradeCounter];
                    GUIController.instance.trainSpeedUpgradeText.transform.GetChild(0).GetComponent<GUIText>().text = "Costo " + upgradeCost[speedUpgradeCounter];
                }
                break;
            case 1:
                life = upgradeHealthPointsArray[healthUpgradeCounter];
                GameController.instance.UpdateResources(-upgradeCost[healthUpgradeCounter]);
                healthUpgradeCounter++;
                if (healthUpgradeCounter < upgradeHealthPointsArray.Length)
                {
                    GUIController.instance.trainHealthUpgradeText.text = "Health " + upgradeHealthPointsArray[healthUpgradeCounter];
                    GUIController.instance.trainHealthUpgradeText.transform.GetChild(0).GetComponent<GUIText>().text = "Costo " + upgradeCost[healthUpgradeCounter];
                }
                break;
            case 2:
                sprint = upgradeSprintArray[sprintUpgradeCounter];
                GameController.instance.UpdateResources(-upgradeCost[sprintUpgradeCounter]);
                sprintUpgradeCounter++;
                if (sprintUpgradeCounter < upgradeSprintArray.Length)
                {
                    GUIController.instance.trainSprintUpgradeText.text = "Sprint " + upgradeSprintArray[sprintUpgradeCounter];
                    GUIController.instance.trainHealthUpgradeText.transform.GetChild(0).GetComponent<GUIText>().text = "Costo " + upgradeCost[sprintUpgradeCounter];
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
}