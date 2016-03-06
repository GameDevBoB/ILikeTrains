using UnityEngine;
using System.Collections;

public class Coach : MonoBehaviour
{

    public Transform frontPivot;
    public Transform rearPivot;
    public float maxDistance;
    public Transform[] waypoints;
    private int countWaypoints;
    public float speed;


    // Use this for initialization
    void Start()
    {
        transform.position = waypoints[0].GetChild(0).position;
        countWaypoints = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (!GameController.instance.isPaused)
        {

            if (Vector3.Distance(transform.position, waypoints[countWaypoints].GetChild(0).position) <= 0.01)
            {
                if ((countWaypoints < (waypoints.Length - 1)))
                {
                    countWaypoints++;
                }
                else
                {

                    countWaypoints = 0;
                    transform.position = waypoints[0].GetChild(0).position;

                }
                transform.LookAt(waypoints[countWaypoints].GetChild(0).position);
            }
            if (Vector3.Distance(frontPivot.position, rearPivot.position) >= maxDistance)
            {
                transform.position = Vector3.MoveTowards(transform.position, waypoints[countWaypoints].GetChild(0).position, Time.deltaTime * speed);
            }
            /*TEST
            if (Vector3.Distance(frontPivot.position, rearPivot.position) < maxDistance)
            {
                transform.position = rearPivot.position;
            }
            TEST*/
        }
    }
    public void GetDamage(int damage)
    {
       
    }
}
