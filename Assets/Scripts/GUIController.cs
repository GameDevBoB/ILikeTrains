﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class GUIController : MonoBehaviour
{
    public static GUIController instance;
    //PLANNINGPHASE CANVAS WITH ELEMENTS
    public Canvas planningCanvas;
    public Button startButton;
    public Button instantiateDynamiteButton;
    public Button instantiateTeslaButton;
    public Button instantiateBarrelButton;
    public Button instantiateTarButton;
    
    //
	/*LEVEL TRANSITION VARIABLES
	public string currentLevelString;
	public string nextLevelString;
	*/

    //HQ UPGRADE CANVAS WITH ELEMENTS
    public Canvas upgradeTrainCanvas;
    public Button upgradeTrainButton;
    public Text phaseText;
    public Text ResourcesText;
    public Text trainSpeedUpgradeText;
    public Text trainHealthUpgradeText;
    public Text trainSprintUpgradeText;
    public Slider healthSlider;
    public Button trainSpeedUpgradeButton;
	public Text trainSpeedUpgradeButtonText;
    public Button trainHealthUpgradeButton;
    public Button trainSprintUpgradeButton;
    //

    //UI ELEMENTS FOR UPGRADES

    public Canvas upgradeCanvas;
    //public Text damageUpgradeText;
    //public Text radiusUpgradeText;
    //public Text cooldownUpgradeText;
	//public Button damageUpgradeButton;
    //public Button radiusUpgradeButton;
    //public Button cooldownUpgradeButton;
	//public GameObject PanelUpgrade;
	public Text upgradeButtonText;
	//public Button activateUpgradeButton;
    [HideInInspector]
    public GameObject canvasOpener;

    //FLASHSCREEN VARIABLES
    public Image errorImage;
    public float flashSpeed = 1f;
    public Color flashColour = new Color(1f, 0f, 0f, 0.1f);
    [HideInInspector]
    public bool isDamaged;

	//CANVAS WinPauseEndLEVEL
	public Image pauseView;
	public Button pausePlay;
	public Button pauseReset;
	public Button pauseMenu;
	public Image gameOverView;
	public Button goMenu;
	public Button goRestart;
	public Image completeLevelView;
	public Button menuWin;
	public Button resetWin;
	public Button nextLevel;
	public Image star1;
	public Image star2;
	public Image star3;

    //ACTIONPHASE BUTTONS
    public Button trainSprintButton;
    public Button pauseButton;


    void Awake()
    {
        instance = this;
    }

    // Use this for initialization
    void Start()
    {
		
        phaseText.text = "PLANNING PHASE";
        instantiateDynamiteButton.transform.GetChild(0).GetComponent<Text>().text = "$" + GameController.instance.trapsCosts[0].ToString();
        instantiateTeslaButton.transform.GetChild(0).GetComponent<Text>().text = "$" + GameController.instance.trapsCosts[1].ToString();
        instantiateTarButton.transform.GetChild(0).GetComponent<Text>().text = "$" + GameController.instance.trapsCosts[3].ToString();
        instantiateBarrelButton.transform.GetChild(0).GetComponent<Text>().text = "$" + GameController.instance.trapsCosts[2].ToString();

    }

    // Update is called once per frame
    void Update()
    {
		if (Input.GetMouseButton(0)) {
			RaycastHit hit;
			Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
			if (!Physics.Raycast (ray, out hit, 100.0f ,1 << LayerMask.NameToLayer("Trap")| 1 << LayerMask.NameToLayer("UI"))) {
				if (upgradeCanvas.gameObject.activeSelf) {
					upgradeCanvas.gameObject.SetActive(false);

				}
			}
		}
        ResourcesText.text = "Resources: $" + GameController.instance.totalResources.ToString();
        FlashWhenTrainIsDamaged();
    }

    public void StartGame()
    {
        //THE FIRST CANVAS ACTIVE AT THE BEGINNING OF THE GAME
        pauseButton.gameObject.SetActive(true);
        phaseText.text = "ACTION PHASE";
        trainSprintButton.gameObject.SetActive(true);
        //if (upgradeTrainCanvas.gameObject.activeSelf)
        //     upgradeTrainCanvas.gameObject.SetActive(false);
        planningCanvas.gameObject.SetActive(false);
    }

    //ACTIVATON OF BUTTON THAT CAN BE PRESSED
    public void ActivateTrapsButton()
    {

        instantiateDynamiteButton.interactable = true;
        instantiateTeslaButton.interactable = true;
        instantiateBarrelButton.interactable = true;
        instantiateTarButton.interactable = true;
    }

    public void DeactivateTrapsButton()
    {
        instantiateDynamiteButton.interactable = false;
        instantiateTeslaButton.interactable = false;
        instantiateBarrelButton.interactable = false;
        instantiateTarButton.interactable = false;


}
    //CANVAS HQ UPGRADE ACTIVATION/DEACTIVATION
    public void ActivateTrainUpgradeCanvas()
    {
        upgradeTrainCanvas.gameObject.SetActive(true);
        upgradeTrainButton.interactable = false;
    }

    public void DeactivateTrainUpgradeCanvas()
    {
        upgradeTrainCanvas.gameObject.SetActive(false);
        upgradeTrainButton.interactable = true;
    }

    public void CloseUpgrade()
    {
		//PanelUpgrade.SetActive (false);
		//activateUpgradeButton.gameObject.SetActive (true);
        upgradeCanvas.gameObject.SetActive(false);

    }
	/*
    public void SetCanvasElements(trapType inputMyType, float inputDamage, float inputRange, float inputCooldown,
        float inputCostDamage, float inputCostRange, float inputCostCooldown)
    {
        switch (inputMyType)
        {
            case trapType.Dynamite:
                damageUpgradeText.text = "Damage " + inputDamage;
                break;
            case trapType.MinaTesla:
                damageUpgradeText.text = "Slow " + inputDamage;
                break;
        }
        radiusUpgradeText.text = "Range " + inputRange;
        cooldownUpgradeText.text = "Cooldown " + inputCooldown;
        radiusUpgradeButton.transform.GetChild(0).GetComponent<Text>().text = "Cost " + inputCostRange;
        cooldownUpgradeButton.transform.GetChild(0).GetComponent<Text>().text = "Cost " + inputCostCooldown;
        damageUpgradeButton.transform.GetChild(0).GetComponent<Text>().text = "Cost " + inputCostDamage;


    }
*/
	public void SetCanvasElements(float inputCost){

		upgradeButtonText.text = ("UPGRADE: $"+inputCost.ToString());

	}

    public void SetCanvasOpener(GameObject myOpener)
    {
        canvasOpener = myOpener;
        /*EventTrigger trigger =  activateUpgradeButton.GetComponent<EventTrigger>();
        EventTrigger.Entry entry = new EventTrigger.Entry();
        EventTrigger.Entry click = new EventTrigger.Entry();
        EventTrigger.Entry exit = new EventTrigger.Entry();
        entry.eventID = EventTriggerType.PointerEnter;
        click.eventID = EventTriggerType.PointerClick;
        exit.eventID = EventTriggerType.PointerExit;
        entry.callback.AddListener((eventData) => { myOpener.GetComponent<Traps>().ShowPreviewRange(); });
        click.callback.AddListener((eventData) => { myOpener.GetComponent<Traps>().ShowPreviewRange(); });
        exit.callback.AddListener((eventData) => { myOpener.GetComponent<Traps>().HidePreviewRange(); });
        trigger.triggers.Clear();
        trigger.triggers.Add(entry);
        trigger.triggers.Add(click);
        trigger.triggers.Add(exit);*/
        // .delegates.Add(entry);
    }

	public void ActivatePanelUpgrade()
	{
		//PanelUpgrade.SetActive (true);
		//activateUpgradeButton.gameObject.SetActive (false);
	}

    public void FlashWhenTrainIsDamaged()
    {
        if (isDamaged)
        {
            errorImage.color = flashColour;

        }
        else
        {
            errorImage.color = Color.Lerp(errorImage.color, Color.clear, flashSpeed * Time.deltaTime);
        }
        isDamaged = false;
    }
	//CANVAS ENDLEVEL
	void ViewOnStart()
	{
		gameOverView.gameObject.SetActive(false);
		completeLevelView.gameObject.SetActive(false);
	}

	public void PauseView()
	{
		//Debug.Log("stronzo");
		pauseView.gameObject.SetActive(true);
		Time.timeScale=0;


	}
	public void UnPaused()
	{
			//Debug.Log("stronzo++");
			pauseView.gameObject.SetActive(false);
			Time.timeScale=1;
		
	}

	public void GameOverView()
	{
		gameOverView.gameObject.SetActive(true);
		GameController.instance.isPaused = true;
        Time.timeScale = 0;
    }

	public void CompleteLevel()
	{
		completeLevelView.gameObject.SetActive(true);
		//GameController.instance.isPaused = true;

		float lifePercentage = GameController.instance.headCoach.actualLife / GameController.instance.headCoach.life * 100;
		if (lifePercentage >= 25f)
		{
			// attiva prima stellina
			star1.gameObject.SetActive(true);
		}
		if (lifePercentage >= 50f)
		{
			// attiva seconda stellina
			star2.gameObject.SetActive(true);
		}
		if (lifePercentage >= 75f)
		{
			// attiva terza stellina
			star3.gameObject.SetActive(true);
		}
        Time.timeScale = 0;

	}
	//BUTTON ENDLEVEL
	// prima di utilizzarli bisogna definirli bene i numeri sono casuali 
	public void RestartLevel()
	{
       
        Application.LoadLevel(Application.loadedLevel);  //bisogna definire il currentLevel
        Time.timeScale = 1;

    }
	public void GoToMenu()
	{
		Application.LoadLevel(0); 
	}
	public void NextLevel()
	{
		Application.LoadLevel(Application.loadedLevel+1);
	}
}
