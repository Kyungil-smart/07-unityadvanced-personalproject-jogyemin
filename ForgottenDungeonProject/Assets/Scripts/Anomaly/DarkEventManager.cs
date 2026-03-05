using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class DarkEventManager : MonoBehaviour
{
    public static DarkEventManager Instance;

    [Header("===== UI 참조 =====")]

    [Tooltip("화면을 어둡게 만드는 검은 이미지")]
    [SerializeField] private Image darkScreen;

    [Tooltip("점프스케어 이미지")]
    [SerializeField] private Image scareImage;


    [Header("===== 연출 설정 =====")]

    [Tooltip("화면이 어두워지는 시간")]
    [SerializeField] private float fadeToDarkTime = 2f;

    [Tooltip("점프스케어 이미지 유지 시간")]
    [SerializeField] private float scareDuration = 0.3f;

    [Tooltip("화면이 다시 밝아지는 시간")]
    [SerializeField] private float fadeToBrightTime = 2f;


    [Header("===== 사운드 =====")]

    [Tooltip("점프스케어 사운드")]
    [SerializeField] private AudioSource scareSound;


    private void Awake()
    {
        Instance = this;
    }


    private void Start()
    {
        if (darkScreen != null)
        {
            Color c = darkScreen.color;
            c.a = 0f;
            darkScreen.color = c;
        }

        if (scareImage != null)
            scareImage.gameObject.SetActive(false);
    }


    public void StartDarkEvent()
    {
        StartCoroutine(DarkEventCoroutine());
    }


    IEnumerator DarkEventCoroutine()
    {
        Debug.Log("암전 이벤트 시작");

        float t = 0;

        // 화면 어두워짐
        while (t < fadeToDarkTime)
        {
            t += Time.deltaTime;

            float alpha = Mathf.Lerp(0, 1, t / fadeToDarkTime);

            Color c = darkScreen.color;
            c.a = alpha;
            darkScreen.color = c;

            yield return null;
        }


        // 점프스케어 등장
        if (scareImage != null)
            scareImage.gameObject.SetActive(true);

        if (scareSound != null)
            scareSound.Play();

        yield return new WaitForSeconds(scareDuration);


        // 점프스케어 제거
        if (scareImage != null)
            scareImage.gameObject.SetActive(false);


        // 화면 밝아짐
        t = 0;

        while (t < fadeToBrightTime)
        {
            t += Time.deltaTime;

            float alpha = Mathf.Lerp(1, 0, t / fadeToBrightTime);

            Color c = darkScreen.color;
            c.a = alpha;
            darkScreen.color = c;

            yield return null;
        }

        Debug.Log("암전 이벤트 종료");
    }
}