using UnityEngine;

public class AddForce : MonoBehaviour
{
    public float force ;
    public Rigidbody whowillbeaffected ;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnCollisionEnter(Collision collision)
    {
        Debug.Log("hi????");
        whowillbeaffected.AddTorque(0,0,force);        
    }
}
