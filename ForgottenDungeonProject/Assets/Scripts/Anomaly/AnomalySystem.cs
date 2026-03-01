using UnityEngine;

public class AnomalySystem : MonoBehaviour
{
    [Header("이상현상 오브젝트 목록")]
    [Tooltip("이 목록 중에서 랜덤으로 하나만 이상현상이 적용됩니다.")]
    [SerializeField] private AnomalyObject[] anomalyObjects;

    // ==============================
    // 관찰 단계 (Stage 1)
    // ==============================
    public void ResetAll()
    {
        foreach (var obj in anomalyObjects)
        {
            obj.ResetToDefault();
        }

        Debug.Log("관찰 단계: 이상현상 없음 (기본 상태 유지)");
    }

    // ==============================
    // 스테이지 이상현상 적용
    // ==============================
    public void ApplyStageAnomaly(bool hasAnomaly)
    {
        // 먼저 전부 기본 상태로 초기화
        foreach (var obj in anomalyObjects)
        {
            obj.ResetToDefault();
        }

        if (!hasAnomaly)
        {
            Debug.Log("현재 스테이지 이상현상: 없음");
            return;
        }

        if (anomalyObjects.Length == 0)
        {
            Debug.LogWarning("AnomalyObject가 등록되지 않았습니다.");
            return;
        }

        // 🔥 랜덤으로 하나 선택
        int randomIndex = Random.Range(0, anomalyObjects.Length);
        AnomalyObject selected = anomalyObjects[randomIndex];

        Debug.Log("이상현상 발생 오브젝트: " + selected.name);

        // 🔥 선택된 하나만 이상현상 적용
        selected.ApplyAnomaly(true);

        Debug.Log("현재 스테이지 이상현상: 있음 (1개만 적용)");
    }
}