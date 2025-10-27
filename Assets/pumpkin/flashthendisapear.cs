using UnityEngine;
using System.Collections;
using UnityEngine.Events;
using meshrenderer = UnityEngine.Renderer;

[RequireComponent(typeof(Renderer))]
public class flashthendisapear : MonoBehaviour
{
    [Header("触发条件：Y 高度范围")]
    public float yMin = 0f;
    public float yMax = 2f;
    public bool checkEveryFrame = true; // true: 实时检测；false: 外部调用 TriggerNow()

    [Header("闪烁参数")]
    public Color flashColor = Color.white;
    public float flashOnIntensity = 6f; // 亮的时候强度
    public float flashOffIntensity = 0f; // 灭的时候强度(通常 0)
    public int flashCount = 2;         // 闪烁次数
    public float flashOnTime = 0.08f;  // 亮起时间
    public float flashOffTime = 0.06f; // 熄灭时间(两次之间)
    public bool useAllMaterials = true; // 对所有材质槽开启 emission

    [Header("消失方式")]
    public HideMode hideMode = HideMode.DisableRenderer;
    public float vanishDelay = 0.0f; // 闪完后延迟再隐藏/销毁

    [Header("烟花特效（粒子）")]
    public GameObject fireworkPrefab; // 粒子预制体（必填）
    public Vector3 fireworkOffset = Vector3.zero;
    public float fireworkDelay = 0.0f;   // 闪完后延迟多久放烟花
    public float fireworkAutoDestroy = 3f; // 几秒后自动销毁实例（<=0 不销毁）

    [Header("复位选项")]
    public bool retriggerAllowed = false; // 再次进入范围→再超出时是否可再次触发
    public float rearmDelay = 0.2f;       // 回到范围内后，多少秒后允许再次触发

    private Renderer rend;
    private Material[] mats;
    private bool isRunning = false;
    private bool armed = true; // 是否可触发

    public enum HideMode { DisableRenderer, SetInactive, DestroyGameObject }

    void Awake()
    {
        rend = GetComponent<Renderer>();
        // 使用实例材质，避免影响共享材质
        mats = useAllMaterials ? rend.materials : new Material[] { rend.material };
    }

    void Update()
    {
        if (!checkEveryFrame || isRunning || !armed) return;

        float y = transform.position.y;
        bool outOfRange = (y < yMin || y > yMax);

        if (outOfRange)
        {
            StartCoroutine(FlashVanishFireworkRoutine());
        }
    }

    /// <summary>
    /// 外部可调用强制触发（例如 UnityEvent / Interact）
    /// </summary>
    public void TriggerNow()
    {
        if (!isRunning && armed)
            StartCoroutine(FlashVanishFireworkRoutine());
    }

    IEnumerator FlashVanishFireworkRoutine()
    {
        isRunning = true;
        armed = false;

        // 闪烁
        for (int i = 0; i < flashCount; i++)
        {
            SetEmission(flashColor, flashOnIntensity);
            if (flashOnTime > 0f) yield return new WaitForSeconds(flashOnTime);

            SetEmission(flashColor, flashOffIntensity);
            if (i < flashCount - 1 && flashOffTime > 0f) yield return new WaitForSeconds(flashOffTime);
        }

        // 闪完可选延迟
        if (vanishDelay > 0f) yield return new WaitForSeconds(vanishDelay);

        // 先放烟花还是先消失：多数时候先消失再放烟花更干净
        // 你也可以把 SpawnFirework 放到下面 Hide 之后
        if (fireworkDelay > 0f) yield return new WaitForSeconds(fireworkDelay);
        SpawnFirework();

        // 隐藏 / 失活 / 销毁
        HideSelf();

        // 如果销毁了就不需要后续逻辑
        if (hideMode == HideMode.DestroyGameObject)
            yield break;

        // 如果允许再次触发：等待回臂
        if (retriggerAllowed)
        {
            yield return new WaitForSeconds(rearmDelay);
            armed = true;
            isRunning = false;
        }
    }

    void SetEmission(Color color, float intensity)
    {
        Color final = color * Mathf.LinearToGammaSpace(Mathf.Max(0f, intensity));
        foreach (var m in mats)
        {
            if (intensity > 0f)
            {
                m.EnableKeyword("_EMISSION");
                m.SetColor("_EmissionColor", final);
            }
            else
            {
                // 设黑 + 关闭关键字
                m.SetColor("_EmissionColor", Color.black);
                m.DisableKeyword("_EMISSION");
            }
        }
    }

    void HideSelf()
    {
        switch (hideMode)
        {
            case HideMode.DisableRenderer:
                rend.enabled = false;
                break;
            case HideMode.SetInactive:
                gameObject.SetActive(false);
                break;
            case HideMode.DestroyGameObject:
                Destroy(gameObject);
                break;
        }
        isRunning = false;
    }

    void SpawnFirework()
    {
        if (fireworkPrefab == null) return;
        Vector3 pos = transform.position + fireworkOffset;
        Quaternion rot = Quaternion.identity;
        var go = Instantiate(fireworkPrefab, pos, rot);

        if (fireworkAutoDestroy > 0f)
            Destroy(go, fireworkAutoDestroy);
    }

    // 可选：当对象重新启用时，重置渲染与材质 emission
    void OnEnable()
    {
        // 若选择 SetInactive 隐藏后重新启用，需要把 Renderer 打开
        if (rend != null) rend.enabled = true;

        // 清空 emission 残留
        if (mats != null) SetEmission(flashColor, 0f);

        if (retriggerAllowed)
        {
            armed = true;
            isRunning = false;
        }
    }
}
