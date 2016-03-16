using UnityEngine;
using System.Collections;

[System.Serializable]
public class EnemyCollection : MonoBehaviour {
	
	public GameObject[] enemyPrefabs;

	private int counter;
	private float startDelay;
	private float spawnDelay;
	private bool imActive = false;


	void Start () {
		counter = 0;
	}
	

	void Update () {
		if (imActive && !GameController.instance.isPaused) {
			if (counter < enemyPrefabs.Length) {
				spawnEnemies ();
			}
			else
                //DESTROYS THE INSTANTIATED INSTANCE OF THE GAMEOBJECT ARRAY AFTER COMPLETING SPAWNIG
				Destroy(this.gameObject);
		}
	}

	public void Activate(float new_spawnDelay)
	{
		imActive = true;
		spawnDelay = new_spawnDelay;
	}

	private void spawnEnemies()
	{

        //INSTANTIATE THE OBJECT IN THE ARRAY CONSECUTIVELY AT ANY GIVEN SPAWN TIME
		if (((Time.time - startDelay) > spawnDelay) || startDelay == 0)
		{
			GameObject myEnemy;
			myEnemy = Instantiate(enemyPrefabs[counter], this.transform.position, enemyPrefabs[counter].transform.rotation) as GameObject;
            myEnemy.transform.position = transform.position;
            myEnemy.SetActive(true);
            myEnemy.SendMessage("Activate");
            startDelay = Time.time;
            counter++;
		}
	}
}
