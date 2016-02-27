using UnityEngine;
using System.Collections;

public class Train : MonoBehaviour {

	public Transform startMarker;
	public Transform endMarker;
	public float speed = 1.0F;

	private float startTime;
	//private bool SetActive (bool value);
	private float journeyLength;


	void Start() {

		startTime = Time.time;
		journeyLength = Vector3.Distance(startMarker.position, endMarker.position);

	}


	void Update() {

		float distCovered = (Time.time - startTime) * speed;
		float fracJourney = distCovered / journeyLength;
		transform.position = Vector3.Lerp(startMarker.position, endMarker.position, fracJourney);

	}

    public void Activate()
    {
        this.gameObject.SetActive(true);
    }
	
}