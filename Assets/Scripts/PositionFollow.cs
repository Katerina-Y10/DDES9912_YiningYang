using UnityEngine;

public class PositionFollow : MonoBehaviour
{
    public Transform target;
    public Vector3 offset;
    public bool spinFollow;
    public bool positionFollow;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(positionFollow)
        {
            transform.position = target.position + offset;
        }
        if(spinFollow)
        {
            transform.rotation = target.rotation;
        }
    }
}
