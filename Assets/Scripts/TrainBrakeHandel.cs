using UnityEngine;

public class TrainBrakeHandel : MonoBehaviour
{
    public TrainControl trainControler;
    public float originaldeceleration;
    public float newdeceleration;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        originaldeceleration = trainControler.deceleration;
    }

    
    public void invokeBrake()
    {
        trainControler.deceleration = newdeceleration;
        Debug.Log("Brake invoked");
    }
    public void cancelBrake()
    {
        trainControler.deceleration = originaldeceleration;
        Debug.Log("Brake cancelled");
    }
    
    
}
