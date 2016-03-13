using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GUIController : MonoBehaviour
{
    public static GUIController instance;
    //PLANNINGPHATE CANVAS WITH ELEMENTS
    public Canvas planningCanvas;
    public Button startButton;
    public Button instantiateButton;
    //

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
    public Button trainHealthUpgradeButton;
    public Button trainSprintUpgradeButton;
	//

	//CANVAS ENDLEVEL
	public Canvas gameOverView;
	public Button goMenu;
	public Button goRestart;
	public Canvas completeLevelView;
	public Button menuWin;
	public Button resetWin;
	public Button nextLevel;
	public Image stars;
	public Text finalLifeText;
	public Text finalEnemyText;
	public Text finalResourceText;
    //

    public Button trainSprintButton;
    void Awake()
    {
        instance = this;
    }

    // Use this for initialization
    void Start()
    {
        phaseText.text = "PLANNING PHASE";
		ViewOnStart();

    }

    // Update is called once per frame
    void Update()
    {
        ResourcesText.text = "Resources: " + GameController.instance.totalResources.ToString();
		finalResourceText.text = "Risorse" + GameController.instance.totalResources.ToString();
		finalLifeText.text = "Vita" + Train.instance.actualLife.ToString();
		//finalEnemyText.text = "Nemici Uccisi" +    SERVE UN CONTATORE DEI NEMICI UCCISI

    }

    public void StartGame()
    {
        //THE FIRST CANVAS ACTIVE AT THE BEGINNING OF THE GAME
        phaseText.text = "ACTION PHASE";
        //if (upgradeTrainCanvas.gameObject.activeSelf)
       //     upgradeTrainCanvas.gameObject.SetActive(false);
       
        planningCanvas.gameObject.SetActive(false);
    }

    //ACTIVATON OF BUTTON THAT CAN BE PRESSED
    public void ActivateInstanceButton()
    {

        instantiateButton.interactable = true;
    }

    public void DeactivateInstanceButton()
    {
        instantiateButton.interactable = false;
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
	//CANVAS ENDLEVEL
	void ViewOnStart()
	{
		gameOverView.gameObject.SetActive(false);
		completeLevelView.gameObject.SetActive(false);
	}

	public void GameOverView()
	{
		gameOverView.gameObject.SetActive(true);
		GameController.instance.isPaused=true;
	}

	public void CompleteLevel()
	{
		completeLevelView.gameObject.SetActive(true);
		GameController.instance.isPaused=true;
	}

	//BUTTON ENDLEVEL
	// prima di utilizzarli bisogna definirli bene i numeri sono casuali 
	public void RestartLevel ()
	{
		Application.LoadLevel (1);
	}
	public void GoToMenu()
	{
		Application.LoadLevel (0);  // il menu è una scena?
	}
	public void NextLevel()
	{
		Application.LoadLevel (2);
	}
}
