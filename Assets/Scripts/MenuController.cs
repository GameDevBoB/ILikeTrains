using UnityEngine;
using System.Collections;

public class MenuController : MonoBehaviour {

	public void LoadMainGame(){
		Application.LoadLevel ("Denis");
	}
	public void ExitGame(){
		Application.Quit ();
	}
	public void LoadSelectionLevelMenu(){
		Application.LoadLevel ("LevelSelection");
	}

}
