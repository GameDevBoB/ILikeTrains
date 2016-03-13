using UnityEngine;
using System.Collections;

public class MenuController : MonoBehaviour {

	public void LoadSelectionLevelMenu(){
		Application.LoadLevel ("Denis");
	}
	public void ExitGame(){
		Application.Quit ();
	}
}
