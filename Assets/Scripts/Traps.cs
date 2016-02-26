using UnityEngine;
using System.Collections;

public class Traps : MonoBehaviour {

	public int ColliderRay=10;

	void Awake() {
		GetComponent<SphereCollider>().radius = ColliderRay;
		this.GetComponent<Collider>().enabled = true;
	}
	

	void Update () {
		
	}


	void OnMouseOver()
	{
		Debug.Log ("Ciao");
		if (Input.GetMouseButtonDown (0))
			this.GetComponent<Collider> ().enabled = true;
		
}


	void OnTriggerEnter(Collider col){
		if (col.gameObject.tag == "Enemy") {

		}
	}

}
