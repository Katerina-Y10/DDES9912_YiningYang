using UnityEngine;

public class TrainBrakeHandel : MonoBehaviour
{
    public TrainControl trainControler;
    public float originaldeceleration;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        originaldeceleration = trainControler.deceleration;
    }

    
    public void invokeBrake(){
        if ( trainControler.isStarted ) {
            trainControler.deceleration = 20f;
        }
    }
    public void cancelBrake()
        {
            trainControler.deceleration = originaldeceleration;
        }
    
}
