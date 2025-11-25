using UnityEngine;

public class FireChangeWithSpeed : MonoBehaviour
{
    public ParticleSystem particleSystem;
    public TrainControl trainControler;
    public float changeRate;
    public float targetburnTime;
    public float emissionRate;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        emissionRate = particleSystem.emissionRate;
    }

    // Update is called once per frame
    void Update()
    {
        targetburnTime = trainControler.TotalburnTime;
        if ( targetburnTime > 0 ) {
            float newEmissionRate = changeRate * targetburnTime;
            particleSystem.emissionRate = newEmissionRate + emissionRate;
        }
        else {
            particleSystem.emissionRate = emissionRate;
        }
    }
}
