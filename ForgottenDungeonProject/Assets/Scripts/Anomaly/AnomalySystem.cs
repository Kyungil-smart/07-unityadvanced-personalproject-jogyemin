using UnityEngine;
using System.Collections;

public class AnomalySystem : MonoBehaviour
{
    [Header("이상현상 오브젝트 목록")]
    [SerializeField] private AnomalyObject[] anomalyObjects;

    [Header("지진 이상현상")]
    [SerializeField] private CameraShakeAnomaly earthquake;

    [Header("암전 점프스케어 이벤트")]
    [Tooltip("암전 + 점프스케어 이벤트 매니저")]
    [SerializeField] private DarkEventManager darkEvent;

    [Header("지진 등장 시간 설정")]
    [Tooltip("지진 최소 등장 시간")]
    [SerializeField] private float earthquakeDelayMin = 3f;

    [Tooltip("지진 최대 등장 시간")]
    [SerializeField] private float earthquakeDelayMax = 12f;

    public void ResetAll()
    {
        foreach (var obj in anomalyObjects)
        {
            obj.ResetToDefault();
        }

        Debug.Log("관찰 단계: 이상현상 없음 (기본 상태 유지)");
    }

    public void ApplyStageAnomaly(bool hasAnomaly)
    {
        foreach (var obj in anomalyObjects)
        {
            obj.ResetToDefault();
        }

        if (!hasAnomaly)
        {
            Debug.Log("현재 스테이지 이상현상: 없음");
            return;
        }

        // 오브젝트 + 지진 + 암전 이벤트
        int totalAnomalyCount = anomalyObjects.Length + 2;

        int randomIndex = Random.Range(0, totalAnomalyCount);

        // 1️⃣ 오브젝트 이상현상
        if (randomIndex < anomalyObjects.Length)
        {
            AnomalyObject selected = anomalyObjects[randomIndex];

            Debug.Log("이상현상 발생 오브젝트: " + selected.name);

            selected.ApplyAnomaly(true);
        }

        // 2️⃣ 지진
        else if (randomIndex == anomalyObjects.Length)
        {
            Debug.Log("이상현상 발생: 지진 예정");

            StartCoroutine(StartEarthquakeDelayed());
        }

        // 3️⃣ 암전 점프스케어
        else
        {
            Debug.Log("이상현상 발생: 암전 점프스케어");

            if (darkEvent != null)
                darkEvent.StartDarkEvent();
        }
    }

    IEnumerator StartEarthquakeDelayed()
    {
        float delay = Random.Range(earthquakeDelayMin, earthquakeDelayMax);

        Debug.Log("지진 발생 예정 시간: " + delay + "초");

        yield return new WaitForSeconds(delay);

        if (earthquake != null)
            earthquake.StartEarthquake();
    }
}