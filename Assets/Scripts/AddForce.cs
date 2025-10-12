using UnityEngine;

public class AddForce : MonoBehaviour
{
    public float force ;
    public Rigidbody subject ;
    public bool addForceX;
    public bool addForceY;
    public bool addForceZ;
  
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
         subject.AddTorque(addForceX ? force : 0,addForceY ? force : 0,addForceZ ? force : 0);
    }

    /*void OnCollisionEnter(Collision collision)
    {
        Debug.Log("hi????");
        //subject.AddTorque(0,0,force);    
       // subject.AddForce(addForceX ? force : 0,addForceY ? force : 0,addForceZ ? force : 0);    
    }*/

 
}
