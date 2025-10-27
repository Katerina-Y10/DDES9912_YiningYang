using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class GazeBob : MonoBehaviour
{
    [Header("检测设置")]
    public Camera cam;
    public float detectDistance = 100f;
    public string targetTag = "TargetA";
    public Transform targetA;

    [Header("跳动参数")]
    public Vector2 startOffsetRange = new Vector2(-1f, 1f);  // ✅ 随机起始位置区间 (Y 轴)
    public float bobHeight = 0.3f;       // 漂浮幅度
    public float bobSpeed = 2.0f;        // 漂浮速度
    public float floatForce = 10f;       // 悬浮力
    public float tiltAngle = 5f;         // 小角度摆动
    public float lookRotateSpeed = 5f;   // 朝向相机的旋转速度

    private Rigidbody rb;
    private Vector3 basePos;
    private Quaternion baseRot;
    private float t;
    private bool isGazing;
    private float randomStartOffsetY;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        if (cam == null) cam = Camera.main;

        basePos = transform.position;
        baseRot = transform.rotation;

        // ✅ 一开始随机给一个位置偏移
        randomStartOffsetY = Random.Range(startOffsetRange.x, startOffsetRange.y);
        transform.position += Vector3.up * randomStartOffsetY;
    }

    void FixedUpdate()
    {
        if (cam == null) return;

        bool gazingNow = false;
        Ray ray = new Ray(cam.transform.position, cam.transform.forward);
        if (Physics.Raycast(ray, out RaycastHit hit, detectDistance))
        {
            if (targetA != null)
                gazingNow = (hit.transform == targetA || hit.transform.IsChildOf(targetA));
            else if (!string.IsNullOrEmpty(targetTag))
                gazingNow = hit.collider.CompareTag(targetTag);
        }

        if (gazingNow)
        {
            rb.useGravity = false;
            rb.linearVelocity = Vector3.zero;
            ApplyFloatMotion();
            FaceCamera();
        }
        else
        {
            rb.useGravity = true;
        }

        isGazing = gazingNow;
    }

    void ApplyFloatMotion()
    {
        t += Time.fixedDeltaTime * bobSpeed;
        float upOffset = Mathf.Sin(t) * bobHeight;
        Vector3 targetPos = basePos + Vector3.up * (upOffset + randomStartOffsetY);

        Vector3 move = (targetPos - transform.position);
        rb.AddForce(move * floatForce, ForceMode.Acceleration);

        float tiltZ = Mathf.Sin(t * 0.9f) * tiltAngle;
        float tiltX = Mathf.Cos(t * 0.7f) * tiltAngle * 0.5f;
        Quaternion targetRot = baseRot * Quaternion.Euler(tiltX, 0f, tiltZ);
        rb.MoveRotation(Quaternion.Slerp(rb.rotation, targetRot, Time.fixedDeltaTime * 3f));
    }

    void FaceCamera()
    {
        Vector3 dir = cam.transform.position - transform.position;
        dir.y = 0f; // 如果不希望上下仰视
        if (dir.sqrMagnitude > 0.001f)
        {
            Quaternion lookRot = Quaternion.LookRotation(dir.normalized, Vector3.up);
            rb.MoveRotation(Quaternion.Slerp(rb.rotation, lookRot, Time.fixedDeltaTime * lookRotateSpeed));
        }
    }

    void OnDrawGizmosSelected()
    {
        if (cam != null)
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawRay(cam.transform.position, cam.transform.forward * detectDistance);
        }
    }
}
