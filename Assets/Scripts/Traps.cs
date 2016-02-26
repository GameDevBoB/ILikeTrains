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


	void OnMouseEnter()
	{
        if (gameObject.CompareTag("Trap"))
        {
            Debug.Log("ENTRATO");
            if (Input.GetMouseButtonDown(0))
                this.GetComponent<Collider>().enabled = true;
        }
		
}
    void OnMouseExit()
    {
        if (gameObject.CompareTag("Trap"))
        {
            Debug.Log("USCITO");
            if (Input.GetMouseButtonDown(0))
                this.GetComponent<Collider>().enabled = true;
        }

    }


    void OnTriggerEnter(Collider col){
		if (col.gameObject.tag == "Enemy") {

		}
	}

}
