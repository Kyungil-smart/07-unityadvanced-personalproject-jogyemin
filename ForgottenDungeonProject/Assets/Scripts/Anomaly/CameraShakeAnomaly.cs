using UnityEngine;
using System.Collections;

[DisallowMultipleComponent]
public class CameraShakeAnomaly : MonoBehaviour
{
    public static CameraShakeAnomaly Instance;

    [Header("===== 흔들릴 카메라 =====")]
    [Tooltip("플레이어 카메라 (PlayerCamera)")]
    [SerializeField] private Transform targetCamera;

    [Header("===== 지진 설정 =====")]

    [Tooltip("지진 지속 시간")]
    [SerializeField] private float duration = 4f;

    [Tooltip("흔들림 강도")]
    [SerializeField] private float magnitude = 0.08f;

    [Tooltip("지진 속도")]
    [SerializeField] private float speed = 20f;

    private Vector3 originalPos;

    void Awake()
    {
        Instance = this;

        if (targetCamera != null)
            originalPos = targetCamera.localPosition;
    }

    public void StartEarthquake()
    {
        StopAllCoroutines();
        StartCoroutine(ShakeRoutine());
    }

    IEnumerator ShakeRoutine()
    {
        float elapsed = 0f;

        while (elapsed < duration)
        {
            float x = (Mathf.PerlinNoise(Time.time * speed, 0f) - 0.5f) * magnitude;
            float y = (Mathf.PerlinNoise(0f, Time.time * speed) - 0.5f) * magnitude;

            targetCamera.localPosition = originalPos + new Vector3(x, y, 0);

            elapsed += Time.deltaTime;
            yield return null;
        }

        targetCamera.localPosition = originalPos;
    }
}