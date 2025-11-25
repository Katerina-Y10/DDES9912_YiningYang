using UnityEngine;

public class VFXchangeWithSpeed : MonoBehaviour
{
    public ParticleSystem particleSystem;
    public TrainControl trainControler;
    public float changeRate;
    public float targetSpeed;
    public Vector3 forceOverLifetime;
    
 
    void Update()
    {
       targetSpeed = trainControler.finalSpeed;

    // 计算你想要的力
       float forceValue = changeRate * targetSpeed;
       float oldForceValuez = forceOverLifetime.z;  
     

    // 修改 forceOverLifetime（正确用法）
       var fol = particleSystem.forceOverLifetime;
       fol.space = ParticleSystemSimulationSpace.World;

       fol.z = forceValue + oldForceValuez; 
     
       
    }
}
