using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bounce : MonoBehaviour
{
    public Vector3 startPosition;
    public Vector3 Bouncedirection;
    public float Bounceoffset;
    public float BounceSpeed;
    private float sinValue;
    public float StartAngle = 0;
   
    // Start is called before the first frame update
    void Start()
    {
        startPosition = transform.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
       
        StartAngle += BounceSpeed;
        if (StartAngle > 360)
        {
            StartAngle = 0;
            StartAngle += BounceSpeed;
        }
    
        sinValue = Mathf.Sin(StartAngle * Mathf.Deg2Rad);

        transform.localPosition = startPosition + sinValue * Bouncedirection * Bounceoffset;

    }
}
