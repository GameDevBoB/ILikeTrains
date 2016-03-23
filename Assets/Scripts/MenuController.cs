using UnityEngine;
using System.Collections;

public class MenuController : MonoBehaviour {
    public UISlider myslider;
    private bool load = false;

    void Start()
    {
        myslider.value = 0;
        load = false;
    }

    void Update()
    {
        if(load)
        {
            myslider.value += 0.005f;
            if(myslider.value >= 1)
                Application.LoadLevel("Denis");
        }
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
