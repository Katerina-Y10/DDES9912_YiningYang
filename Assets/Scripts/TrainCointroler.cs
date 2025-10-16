using UnityEngine;

public class TrainControl : MonoBehaviour
{
    [Header("Train Control")]
    public float Startspeed = -2f;
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



    

//check if the train ready to start
    public bool isStarted = false;

 
// Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        finalSpeed = Startspeed;
        
    }

//it's a time counting down function
    public void BurnCoal( float coalcount )
    {
        coalcount = coalcount + coalcount;
        TotalburnTime = coalcount * burnTime;
        Debug.Log("TotalburnTime: " + TotalburnTime);
    }

// make a fake motor to move the wheels
    void WheelMove()
    {
        
       
        foreach (HingeJoint wheel in wheels)
        {
            JointMotor motor= wheel.motor;
            motor.targetVelocity = finalSpeed * (-50f);
            wheel.motor = motor;
        }

    }
    
    //accelerate the train
   void accelerate()
    {
        if (finalSpeed == minSpeed && isStarted == false)
        {
            isStarted = true;
            GetComponent<AudioSource>().Play();
        
        }

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

    //decelerate the train
    void decelerate()
    {
        if (finalSpeed > minSpeed)
        {
            finalSpeed -= deceleration * Time.deltaTime;
        }
        else
        {
            finalSpeed = minSpeed;
            isStarted = false;
        }
      
    }
    

    // Update is called once per frame
    //if time is still counting down, accelerate the train
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

