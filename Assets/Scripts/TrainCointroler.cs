using UnityEngine;

public class TrainControl : MonoBehaviour
{
    [Header("Train Control")]
    public float Startspeed = 0f;
    public float acceleration = 2f;
    public float deceleration = 1f;
    public float maxSpeed = 10f;
    public float minSpeed = 0f;
    private float finalSpeed;

    [Header("Coal")]
    
    public float burnTime = 3f;
    private float TotalburnTime = 0f;

    [Header("Wheels")]
    public HingeJoint[] wheels;
    
 
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        finalSpeed = Startspeed;
    }

    public void BurnCoal( float coalcount )
    {
        coalcount = coalcount + coalcount;
        TotalburnTime = coalcount * burnTime;
        Debug.Log("TotalburnTime: " + TotalburnTime);
    }

    void WheelMove()
    {
        
       
        foreach (HingeJoint wheel in wheels)
        {
            JointMotor motor= wheel.motor;
            motor.targetVelocity = finalSpeed * (-100f);
            wheel.motor = motor;
        }

    }
    
   void accelerate()
    {
        if (finalSpeed < maxSpeed)
        {
            finalSpeed += acceleration * Time.deltaTime;
        }
        else
        {
            finalSpeed = maxSpeed;
        }
        WheelMove();    
    }

    void decelerate()
    {
        if (finalSpeed > minSpeed)
        {
            finalSpeed -= deceleration * Time.deltaTime;
        }
        else
        {
            finalSpeed = minSpeed;
        }
    }
    

    // Update is called once per frame
    void Update()
    {
    
        WheelMove();
        TotalburnTime -= Time.deltaTime;
        transform.Translate(Vector3.left * finalSpeed * Time.deltaTime);
        
        if (TotalburnTime >= 0f ) 
            {
                accelerate();
            }
            else if (TotalburnTime <= 0f )
            {
                decelerate();
            }
            else
            {
                finalSpeed = minSpeed;
            }

      
    }
}

