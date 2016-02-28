using UnityEngine;
using System.Collections;

public class Train : MonoBehaviour {

    public int life;
	public Transform startMarker;
	public Transform endMarker;
	public float speed = 1.0F;

	private float startTime;
    private int actualLife;
	//private bool SetActive (bool value);
	private float journeyLength;


	void Start() {
        actualLife = life;
        transform.position = startMarker.position;
		journeyLength = Vector3.Distance(startMarker.position, endMarker.position);

	}


	void Update() {
        if (!GameController.instance.isPaused)
        {
            if(startTime == 0)
                startTime = Time.time;
            float distCovered = (Time.time - startTime) * speed;
            float fracJourney = distCovered / journeyLength;
            transform.position = Vector3.Lerp(startMarker.position, endMarker.position, fracJourney);
        }

	}

    public void Activate()
    {
        this.gameObject.SetActive(true);
    }
	
    public void GetDamage(int damage)
    {
        actualLife -= damage;
        if (actualLife <= 0)
            this.gameObject.SetActive(false);
    }

}