using UnityEngine;
using System.Collections;
using UnityEngine.UI;


public class CreditsScroller : MonoBehaviour {

    public GameObject creditContainer;
    public float speed;
    private Vector3 initialPosition;
 
    
	// Use this for initialization
	void Start () {

        initialPosition = creditContainer.transform.position;
        
	
	}
	
	// Update is called once per frame
	void Update () {

       // text.text = creditContainer.transform.position.y.ToString();

        creditContainer.transform.Translate(Vector3.up * Time.deltaTime * speed);
        
           
	}

    public void BackToMain()
    {
        Application.LoadLevel(0);
    }

    void OnTriggerExit(Collider col)
    {
            
        if (col.gameObject.tag=="BoundExit")
            creditContainer.transform.position = initialPosition;
    }
}

