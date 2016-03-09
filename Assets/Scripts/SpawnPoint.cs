using UnityEngine;
using System.Collections;

public class SpawnPoint : MonoBehaviour {
    //ENEMY SPAWN RATIO
    public float cooldown;
    //
    //VARIABLE THT ASSIGNS THE RADIUS OF COLLIDER ON INSPECTOR
    public float distanceTrigger;
    public float spawnDelay;
	public GameObject[] enemyCollectionPrefabs;

    private float startCooldown;

    void Awake()
    {
        GetComponent<SphereCollider>().radius = distanceTrigger;
    }


	void Start () {

	}
	

	void Update () {
        
	}

    /*private void spawnEnemies()
    {
        GameObject myEnemy;
        if (((Time.time - startDelay) > spawnDelay) || startDelay == 0)
        {
            //myEnemy = GameController.instance.GetEnemy();
			myEnemy = Instantiate();
            myEnemy.transform.position = transform.position;
            myEnemy.SetActive(true);
            myEnemy.SendMessage("Activate");
            startDelay = Time.time;
            counter++;
        }
    } */

    void OnTriggerStay(Collider col)
    {
        if (col.gameObject.tag.Equals("Train"))
        {
            if (!GameController.instance.isPaused)
            {
                if (((Time.time - startCooldown) > cooldown) || startCooldown == 0)
                {
                    SpawnCollection();
                }
            }
        }
    }
    //SPAWN A COLLECTION OF ENEMIES RANDOMLY THROUGH AN ARRAY OF ENEMIES
    void SpawnCollection()
    {
        GameObject enemyCollection = Instantiate(enemyCollectionPrefabs[Random.Range(0, enemyCollectionPrefabs.Length)], gameObject.transform.position, gameObject.transform.rotation) as GameObject;
        enemyCollection.SendMessage("Activate", spawnDelay);
        startCooldown = Time.time;
    }
}
