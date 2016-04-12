using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Train : MonoBehaviour
{


    //HQ VARIABLES
    public float life;
    public float sprint;
    public float sprintCooldown;
    public float speed = 1.0F;
    public bool loop = false;
    public float slowDelay;
    public float actualLife;
    public bool isCoach;
    //

	//HQS and COACHES SPRITES
	public Sprite up;
	public Sprite upRight;
	public Sprite right;
	public Sprite downRight;
	public Sprite down;
	public Sprite downLeft;
	public Sprite left;
	public Sprite upLeft;
	private SpriteRenderer mySpriteRenderer;
    public ParticleSystem smokeParticle;
    public ParticleSystem smokeSpeedParticle;
	

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
    public float slowRatio = 0.5f;


    private float startTime;


    //SUPPORT VARIABLES 
    private int countWaypoints;
    private int healthUpgradeCounter;
    private int sprintUpgradeCounter;
    private int speedUpgradeCounter;
   
    private float slowTimer;
    private float startSprint;
    private bool trainIsSlowed;
    private bool trainIsSprinted;
    //

    private Vector3 initPos;

    void Awake()
    {
		if(!isCoach)
		mySpriteRenderer = transform.GetChild(2).GetComponent<SpriteRenderer> ();
		else
		mySpriteRenderer = transform.GetChild(3).GetComponent<SpriteRenderer> ();

    }

    void Start()
    {
      
        //SETTING INITIAL PARAMETERS
        initPos = transform.position;
        transform.position = initPos;
        healthUpgradeCounter = 0;
        sprintUpgradeCounter = 0;
        speedUpgradeCounter = 0;
        actualLife = life;
       


        countWaypoints = 0;
        GUIController.instance.healthSlider.value = actualLife;
        GUIController.instance.healthSlider.maxValue = actualLife;

		if (!isCoach) {
			SetSpeedGui ();
			SetSprintGui ();
			SetHealthGui ();
		}


        if (isCoach)
        {
            //GETTING THE PATHPOINT OF HQ GIVING THEM TO ALL ELEMENTS ATTACHED TO IT
            //for (int i = 0; i < trainToCopyFrom.GetComponent<Train>().waypoints.Length; i++)
            //{
            waypoints = GameController.instance.headCoach.waypoints;
            //}
            loop = GameController.instance.headCoach.loop;
        }
        //

    }


    void FixedUpdate()
    {

        CheckButtonInteractable();
        if (!GameController.instance.isPaused)
        {

            //CHECKING IF ITS HQ 
            if (!isCoach)
            {
                if (((Time.time - slowTimer) > slowDelay) || slowTimer == 0)
                {
                    trainIsSlowed = false;
                }
                if (((Time.time - startSprint) > sprint))
                {
                    trainIsSprinted = false;
                  
                }
                if ((Time.time - startSprint) > sprintCooldown + sprint)
                {
                    GUIController.instance.trainSprintButton.interactable = true;

                }
                if (Vector3.Distance(transform.position, waypoints[waypoints.Length - 1].GetChild(0).position) <= 0.01)
                    GameController.instance.WinGame();

                if (trainIsSprinted)
                {
                    smokeParticle.gameObject.SetActive(false);
                    smokeSpeedParticle.gameObject.SetActive(true);
                }
                else
                {
                    smokeParticle.gameObject.SetActive(true);
                    smokeSpeedParticle.gameObject.SetActive(false);
                }

                ChangeWaypoint();

                MoveTrain(1);

            }
            //
            //IF NOT WE MOVE THE COACHES BEHIND IT ON A CONVOY FORMATION
            else
            {
                //GETTING STATIC VALUES REFERENCES TO THE HQ INSTANCE
                loop = GameController.instance.headCoach.loop;
                speed = GameController.instance.headCoach.speed;
                trainIsSprinted = GameController.instance.headCoach.trainIsSprinted;
                trainIsSlowed = GameController.instance.headCoach.trainIsSlowed;
                sprintRatio = GameController.instance.headCoach.sprintRatio;

                //

                ChangeWaypoint();
                if (Vector3.Distance(frontPivot.position, rearPivot.position) >= maxDistance)
                {
                    //FORCING THE DISTANCE BETWEEN ELEMENTS TO A FIXED DISTANCE EVERY FRAME

                    MoveTrain(10);


                }

                //WE MOVE THE NEXT ELEMENT ONLY IF THAT DISTANCE IS EXCEEDED
                if (Vector3.Distance(frontPivot.position, rearPivot.position) >= distance)
                {
                    MoveTrain(1);
                }

                //
            }




        }


    }

    private void MoveTrain(float velocityMultiplier)
    {
        float newspeed = speed;
        if (trainIsSprinted)
            newspeed += (speed * sprintRatio);
        if (trainIsSlowed)
            newspeed -= (speed * slowRatio);
        transform.position = Vector3.MoveTowards(transform.position, waypoints[countWaypoints].GetChild(0).position, Time.deltaTime * newspeed * velocityMultiplier);


    }

    private void ChangeWaypoint()
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
			SpriteSwapper();


        }
    }

    public void Activate()
    {
        this.gameObject.SetActive(true);
    }

    public void GetDamage(int damage)
    {

        //WE SET THE HP VALUE OF THE HQ EVERY TIME IT GET DAMAGE DEACTIVATING IT IF THE HP IS EQUAL OR BELOW ZERO
        GameController.instance.headCoach.actualLife -= damage;
        GUIController.instance.healthSlider.value = GameController.instance.headCoach.actualLife;
        if (GameController.instance.headCoach.actualLife <= 0)
            //GameController.instance.LoseGame();
			GUIController.instance.GameOverView ();
        //

        GUIController.instance.isDamaged = true;
        GUIController.instance.FlashWhenTrainIsDamaged();

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
                GUIController.instance.healthSlider.maxValue = life;
                GUIController.instance.healthSlider.value = actualLife;
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

        GUIController.instance.trainSpeedUpgradeText.text = "Speed " + upgradeSpeedArray[speedUpgradeCounter].ToString();
		GUIController.instance.trainSpeedUpgradeButtonText.text =  "Cost " + upgradeCost[speedUpgradeCounter].ToString();
        //GUIController.instance.trainSpeedUpgradeButton.transform.GetChild(0).GetComponent<Text>().text = "Cost " + upgradeCost[speedUpgradeCounter];

    }
    private void SetHealthGui()
    {

        GUIController.instance.trainHealthUpgradeText.text = "Health " + upgradeHealthPointsArray[healthUpgradeCounter];
        GUIController.instance.trainHealthUpgradeButton.transform.GetChild(0).GetComponent<Text>().text = ("Cost " + upgradeCost[healthUpgradeCounter].ToString());
		Debug.Log (upgradeCost [healthUpgradeCounter]);

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

    void SetSlow(float input_slowRatio)
    {

        //SLOWING THE HQ

        trainIsSlowed = true;
        slowTimer = Time.time;
        slowRatio = input_slowRatio;
        //

    }

    public void SetSprint()
    {


        //SPRINTING THE HQ
        startSprint = Time.time;
        trainIsSprinted = true;
        GUIController.instance.trainSprintButton.interactable = false;


        //Debug.Log("ENTRO IN SPRINT " + startSprint);



    }
    //////////
   

	void SpriteSwapper()
	{
		float angle;
		angle = (transform.localRotation.eulerAngles.y>=0)? transform.localRotation.eulerAngles.y : 360 + transform.localRotation.eulerAngles.y;
	if(angle > 355f || angle < 5f )
	{
			mySpriteRenderer.sprite=up;
	}
		if(angle > 5f && angle < 85f )
	{
			mySpriteRenderer.sprite=upRight;
	}
		if(angle > 85f && angle < 95f )
	{
			mySpriteRenderer.sprite=right;
	}
		if(angle > 95f && angle < 175f )
	{
			mySpriteRenderer.sprite=downRight;
	}
		if(angle > 175f && angle < 185f )
	{
			mySpriteRenderer.sprite=down;
	}
		if(angle > 185f && angle < 265f )
	{
			mySpriteRenderer.sprite=downLeft;
	}
		if(angle > 265f && angle < 275f )
	{
			mySpriteRenderer.sprite=left;
	}
		if(angle > 275f && angle < 355f )
	{
			mySpriteRenderer.sprite=upLeft;
	}
	}

}