using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[DisallowMultipleComponent]
public class AnomalyObject : MonoBehaviour
{
    [Header("===== 이상현상 사용 여부 (체크된 것 중 랜덤 1개 적용) =====")]

    [Tooltip("체크 시 이상현상 발생 시 완전 사라짐")]
    [SerializeField] private bool useDisappear;

    [Tooltip("체크 시 크기 증가")]
    [SerializeField] private bool useScaleUp;

    [Tooltip("체크 시 크기 감소")]
    [SerializeField] private bool useScaleDown;

    [Tooltip("체크 시 사라졌다 나타나기 반복")]
    [SerializeField] private bool useBlink;

    [Header("===== 세부 수치 =====")]

    [Tooltip("크기 증가 배율")]
    [SerializeField] private float scaleUpMultiplier = 2f;

    [Tooltip("크기 감소 배율")]
    [SerializeField] private float scaleDownMultiplier = 0.5f;

    [Tooltip("Blink 반복 간격(초)")]
    [SerializeField] private float blinkInterval = 2f;

    private bool defaultActiveState;
    private Vector3 defaultScale;

    private Renderer[] renderers;
    private Collider[] colliders;

    private Coroutine blinkCoroutine;

    private void Awake()
    {
        defaultActiveState = gameObject.activeSelf;
        defaultScale = transform.localScale;

        renderers = GetComponentsInChildren<Renderer>(true);
        colliders = GetComponentsInChildren<Collider>(true);
    }

    // ===============================
    // 기본 상태 복구
    // ===============================
    public void ResetToDefault()
    {
        gameObject.SetActive(defaultActiveState);
        transform.localScale = defaultScale;

        if (blinkCoroutine != null)
        {
            StopCoroutine(blinkCoroutine);
            blinkCoroutine = null;
        }

        SetVisible(true);

        Debug.Log($"[Reset] {gameObject.name} → 기본 상태 복구 완료");
    }

    // ===============================
    // 이상현상 적용 (랜덤 1개)
    // ===============================
    public void ApplyAnomaly(bool hasAnomaly)
    {
        if (!hasAnomaly)
        {
            ResetToDefault();
            return;
        }

        Debug.Log($"[이상현상 선택 시작] {gameObject.name}");

        List<System.Action> anomalyActions = new List<System.Action>();

        if (useScaleUp)
        {
            anomalyActions.Add(() =>
            {
                transform.localScale = defaultScale * scaleUpMultiplier;
                Debug.Log($"{gameObject.name} → ScaleUp 적용");
            });
        }

        if (useScaleDown)
        {
            anomalyActions.Add(() =>
            {
                transform.localScale = defaultScale * scaleDownMultiplier;
                Debug.Log($"{gameObject.name} → ScaleDown 적용");
            });
        }

        if (useBlink)
        {
            anomalyActions.Add(() =>
            {
                blinkCoroutine = StartCoroutine(BlinkRoutine());
                Debug.Log($"{gameObject.name} → Blink 적용 시작");
            });
        }

        if (useDisappear)
        {
            anomalyActions.Add(() =>
            {
                gameObject.SetActive(false);
                Debug.Log($"{gameObject.name} → Disappear 적용");
            });
        }

        if (anomalyActions.Count == 0)
        {
            Debug.LogWarning($"{gameObject.name} → 설정된 이상현상이 없습니다.");
            return;
        }

        int randomIndex = Random.Range(0, anomalyActions.Count);
        anomalyActions[randomIndex].Invoke();
    }

    // ===============================
    // Blink 코루틴
    // ===============================
    private IEnumerator BlinkRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(blinkInterval);

            SetVisible(false);
            Debug.Log($"{gameObject.name} → 사라짐");

            yield return new WaitForSeconds(1f);

            SetVisible(true);
            Debug.Log($"{gameObject.name} → 다시 나타남");
        }
    }

    private void SetVisible(bool visible)
    {
        foreach (var r in renderers)
            r.enabled = visible;

        foreach (var c in colliders)
            c.enabled = visible;
    }
}