using UnityEngine;
using System.Collections;

[System.Serializable]
public class EnemyCollection : MonoBehaviour {
	
	public GameObject[] enemyPrefabs;

	private int counter;
	private float startDelay;
	private float spawnDelay;
	private bool imActive = false;

	// Use this for initialization
	void Start () {
		counter = 0;
	}
	
	// Update is called once per frame
	void Update () {
		if (imActive) {
			if (counter < enemyPrefabs.Length) {
				spawnEnemies ();
			}
			else
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
