using UnityEngine;

public class RotateFixedAngle : MonoBehaviour
{
    public float x = 0; 
    public float y = 0;
    public float z = 0;
    Vector3 oldEulerAngles;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
 

    public void RotateAngle()
    {
        oldEulerAngles = transform.localEulerAngles;   // 保存当前旋转角度
        transform.localEulerAngles = new Vector3(x, y, z); // 旋转角度换成新的旋转角度(最终旋转角度)
    }

    public void Rotateback()
    {
        transform.localEulerAngles = oldEulerAngles;
        Debug.Log("Rotateback: " + oldEulerAngles);
    }   

}
