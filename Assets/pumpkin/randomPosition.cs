using UnityEngine;

public class randomPosition : MonoBehaviour
{
    [Header("随机范围（世界/本地坐标）")]
    public Vector2 xRange = new Vector2(-1f, 1f);
    public Vector2 yRange = new Vector2( 0f,  2f);
    public Vector2 zRange = new Vector2(-1f, 1f);

    [Tooltip("true: 在本地空间随机（相对父物体）；false: 在世界空间随机")]
    public bool useLocalSpace = false;

    [Tooltip("true: 在启动时记录一个基准点，随机位置在该点的范围内偏移")]
    public bool relativeToStart = false;

    [Header("可选：启动/激活时自动随机")]
    public bool randomizeOnStart = true;   // 第一次创建后
    public bool randomizeOnEnable = true;  // 每次 SetActive(true) 时

    private Vector3 startAnchor; // 记录启动时基准点

    void Awake()
    {
        // 记录基准点
        startAnchor = useLocalSpace ? transform.localPosition : transform.position;
    }

    void Start()
    {
        if (randomizeOnStart)
            RandomizeNow();
    }

    void OnEnable()
    {
        if (randomizeOnEnable)
            RandomizeNow();
    }

    /// <summary>
    /// 供按钮/事件/脚本调用：立刻随机一个新位置
    /// </summary>
    public void RandomizeNow()
    {
        // 生成随机偏移
        float rx = Random.Range(xRange.x, xRange.y);
        float ry = Random.Range(yRange.x, yRange.y);
        float rz = Random.Range(zRange.x, zRange.y);
        Vector3 rand = new Vector3(rx, ry, rz);

        if (relativeToStart)
        {
            // 以启动时位置为锚点
            if (useLocalSpace)
                transform.localPosition = startAnchor + rand;
            else
                transform.position = startAnchor + rand;
        }
        else
        {
            // 直接在区间内取值（非偏移）
            if (useLocalSpace)
                transform.localPosition = rand;
            else
                transform.position = rand;
        }
    }

    // 场景里画出大致的范围辅助（仅编辑可见）
    void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(0f, 0.6f, 1f, 0.25f);

        // 估算中心与尺寸（只画世界空间的可视框，便于理解）
        Vector3 center = new Vector3(
            (xRange.x + xRange.y) * 0.5f,
            (yRange.x + yRange.y) * 0.5f,
            (zRange.x + zRange.y) * 0.5f
        );
        Vector3 size = new Vector3(
            Mathf.Abs(xRange.y - xRange.x),
            Mathf.Abs(yRange.y - yRange.x),
            Mathf.Abs(zRange.y - zRange.x)
        );

        // 若相对启动位置，则以当前（或记录的）锚点为中心
        Vector3 anchor = Application.isPlaying
            ? (useLocalSpace ? transform.parent.TransformPoint(startAnchor) : startAnchor)
            : transform.position; // 编辑态下用当前位置预览

        Gizmos.matrix = Matrix4x4.identity;
        Gizmos.DrawCube(relativeToStart ? anchor + center : center, size);
        Gizmos.color = new Color(0f, 0.6f, 1f, 0.9f);
        Gizmos.DrawWireCube(relativeToStart ? anchor + center : center, size);
    }
}
