using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class StartMenuController : MonoBehaviour {
    public CanvasRenderer mainPanel;
    public CanvasRenderer levelPanel;
    public CanvasRenderer loadingPanel;
    public Slider trainSlider;
    public Text sliderText;

    private bool isLoading;
    private int levelToLoad;
    // Use this for initialization
    void Start()
    {
        isLoading = false;
    }
	
	// Update is called once per frame
	void Update () {
        if(isLoading && trainSlider.value < 1)
        {
            trainSlider.value += 0.01f;
            sliderText.text = ((int)(trainSlider.value * 100)).ToString() + "%";
        }
        else if(isLoading && trainSlider.value == 1)
        {
            Application.LoadLevel(levelToLoad);
        }
    }

    public void OpenLevel()
    {
        levelPanel.gameObject.SetActive(true);
        mainPanel.gameObject.SetActive(false);
    }

    public void OpenMain()
    {
        mainPanel.gameObject.SetActive(true);
        levelPanel.gameObject.SetActive(false);
    }

    public void OpenLoading()
    {
        loadingPanel.gameObject.SetActive(true);
        levelPanel.gameObject.SetActive(false);
    }

    public void SetLevelToLoad(int levelIndex)
    {
        trainSlider.value = 0;
        isLoading = true;
        levelToLoad = levelIndex;
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
