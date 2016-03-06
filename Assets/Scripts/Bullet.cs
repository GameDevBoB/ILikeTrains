using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour
{

 
    private float duration = 3;
    public float shootPower = 200;
    private float activationTime;
    public float bulletDamage = 2f;
    private GameObject target;

   


    private Rigidbody rb;
 

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
       
    }
    // Use this for initialization
    void Start()
    {
        target = GameObject.FindWithTag("Train");


    }

    // Update is called once per frame
    void Update()
    {
        if ((Time.time - activationTime) > duration)
        {
            HideMe();
        }

    }
   
    public void HideMe()
    {
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        this.gameObject.SetActive(false);
    }

    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == "Train")
        {

            col.gameObject.SendMessage("GetDamage", bulletDamage);
            Deactivate();
        }

    }

    private void Deactivate()
    {
        gameObject.SetActive(false);
    }

   
}
