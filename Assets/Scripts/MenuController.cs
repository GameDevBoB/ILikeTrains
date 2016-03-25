using UnityEngine;
using System.Collections;

public class MenuController : MonoBehaviour {
    public UISlider myslider;
    private bool load = false;
	public Texture2D cursorTexture;
	private Vector2 hotSpot = Vector2.zero;
	private string sceneToLoad;

    void Start()
    {
		Cursor.SetCursor(cursorTexture, hotSpot, CursorMode.Auto);
        myslider.value = 0;
        load = false;
    }

    void Update()
    {
		//cursorTexture = camera.main.ScreenToViewportPoint(Input.mousePosition);
        if(load)
        {
            myslider.value += 0.01f;
            if(myslider.value >= 1)
               Application.LoadLevel(sceneToLoad);
        }
    }

	public void LoadLevelScene(){

		sceneToLoad = UIButton.current.name;

	}
	public void LoadMainGame(){
		//Application.LoadLevel ("Denis");
	}
	public void ExitGame(){
		Application.Quit ();
	}

    public void Load()
    {
        load = true;
    }

}
