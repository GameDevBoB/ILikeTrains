using UnityEngine;
using System.Collections;

public class SpawnPoint : MonoBehaviour {
    public int enemyNumber;
    public float cooldown;
    public float distanceTrigger;
    public float spawnDelay;

    private float startCooldown;
    private float startDelay;
    private int counter;

    void Awake()
    {
        GetComponent<SphereCollider>().radius = distanceTrigger;
    }

	// Use this for initialization
	void Start () {
        counter = 0;
	}
	
	// Update is called once per frame
	void Update () {
        
	}

    private void spawnEnemies()
    {
        GameObject myEnemy;
        if (((Time.time - startDelay) > spawnDelay) || startDelay == 0)
        {
            myEnemy = GameController.instance.GetEnemy();
            myEnemy.transform.position = transform.position;
            myEnemy.SetActive(true);
            myEnemy.SendMessage("Activate");
            startDelay = Time.time;
            counter++;
        }
    }

    void OnTriggerStay(Collider col)
    {
        if (col.gameObject.tag.Equals("Train"))
        {
            if (!GameController.instance.isPaused)
            {
                if (((Time.time - startCooldown) > cooldown) || startCooldown == 0)
                {
                    if (counter < enemyNumber)
                    {
                        spawnEnemies();
                    }
                    else
                    {
                        counter = 0;
                        startCooldown = Time.time;
                    }
                }
            }
        }
    }
}
