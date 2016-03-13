using UnityEngine;
using System.Collections;

public class MenuController : MonoBehaviour {

	public void LoadSelectionLevelMenu(){
		Application.LoadLevel ("LevelSelection");
	}
	public void ExitGame(){
		Application.Quit ();
	}
}
