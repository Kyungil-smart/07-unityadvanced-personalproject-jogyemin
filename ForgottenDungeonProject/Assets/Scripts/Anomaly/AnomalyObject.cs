using UnityEngine;

public enum AnomalyType
{
    Disappear,
    Appear
}

[DisallowMultipleComponent]
public class AnomalyObject : MonoBehaviour
{
    [Header("이상현상 설정")]
    [Tooltip("Disappear = 이상현상 시 사라짐\nAppear = 이상현상 시 생성됨")]
    [SerializeField] private AnomalyType anomalyType;

    private bool defaultActiveState;

    private void Awake()
    {
        // 🔥 Awake에서 기본 상태 저장 (씬 로드 직후 즉시 실행됨)
        defaultActiveState = gameObject.activeSelf;
    }

    public void ResetToDefault()
    {
        gameObject.SetActive(defaultActiveState);
    }

    public void ApplyAnomaly(bool hasAnomaly)
    {
        if (!hasAnomaly)
        {
            ResetToDefault();
            return;
        }

        if (anomalyType == AnomalyType.Disappear)
        {
            gameObject.SetActive(false);
        }
        else
        {
            gameObject.SetActive(true);
        }
    }
}