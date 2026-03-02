using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.UI;
using TMPro;

[DisallowMultipleComponent]
public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("스테이지 설정")]
    [Tooltip("최종 스테이지 번호 (8이면 클리어)")]
    [SerializeField] private int maxStage = 8;

    [Header("클리어 연출 UI (GameManager 자식으로 배치)")]
    [Tooltip("검은 화면을 포함하는 Canvas")]
    [SerializeField] private GameObject clearCanvas;

    [Tooltip("전체 화면을 덮는 검은 Image")]
    [SerializeField] private Image blackImage;

    [Tooltip("GAME CLEAR 텍스트")]
    [SerializeField] private TextMeshProUGUI clearText;

    [Tooltip("페이드 아웃 시간(초)")]
    [SerializeField] private float fadeDuration = 3f;

    [Header("게임 상태")]
    [SerializeField] private bool hasAnomaly;
    [SerializeField] private AnomalySystem anomalySystem;

    private bool isGameCleared = false;

    public bool HasAnomaly => hasAnomaly;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            // 시작 시 클리어 UI 초기화
            if (clearCanvas != null)
                clearCanvas.SetActive(false);

            if (blackImage != null)
            {
                Color c = blackImage.color;
                c.a = 0f;
                blackImage.color = c;
            }

            if (clearText != null)
                clearText.gameObject.SetActive(false);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        anomalySystem = FindObjectOfType<AnomalySystem>(true);

        if (anomalySystem == null)
            Debug.LogWarning("AnomalySystem을 찾지 못했습니다.");

        StartStage();
    }

    // ==============================
    // 스테이지 시작
    // ==============================
    public void StartStage()
    {
        if (isGameCleared) return;

        int stage = StageManager.Instance.GetStage();

        if (stage == 1)
        {
            hasAnomaly = false;
            Debug.Log("=== Stage 1 (관찰 단계) ===");

            if (anomalySystem != null)
                anomalySystem.ResetAll();

            return;
        }

        hasAnomaly = Random.value > 0.5f;

        Debug.Log("=== Stage " + stage + " 시작 ===");
        Debug.Log("이상현상 존재 여부: " + (hasAnomaly ? "있음" : "없음"));

        if (anomalySystem != null)
            anomalySystem.ApplyStageAnomaly(hasAnomaly);
    }

    // ==============================
    // 플레이어 선택 평가
    // ==============================
    public void EvaluateChoice(bool playerChoseAnomalyDoor)
    {
        if (isGameCleared) return;

        bool success =
            (hasAnomaly && playerChoseAnomalyDoor) ||
            (!hasAnomaly && !playerChoseAnomalyDoor);

        if (success)
        {
            StageManager.Instance.CorrectAnswer();
            int nextStage = StageManager.Instance.GetStage();

            Debug.Log("성공 → 다음 스테이지: " + nextStage);

            if (nextStage > maxStage)
            {
                StartCoroutine(ClearSequence());
                return;
            }
        }
        else
        {
            Debug.Log("실패 → Stage 1로 리셋");
            StageManager.Instance.SetStage(1);
            ReloadStage();
            return;
        }

        ReloadStage();
    }

    // ==============================
    // 클리어 연출
    // ==============================
    private IEnumerator ClearSequence()
    {
        isGameCleared = true;

        Debug.Log("게임 클리어 연출 시작");

        clearCanvas.SetActive(true);
        clearText.gameObject.SetActive(false);

        float t = 0f;

        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            float alpha = Mathf.Lerp(0f, 1f, t / fadeDuration);

            Color c = blackImage.color;
            c.a = alpha;
            blackImage.color = c;

            yield return null;
        }

        // 완전 검정 고정
        Color final = blackImage.color;
        final.a = 1f;
        blackImage.color = final;

        yield return new WaitForSeconds(1f);

        clearText.gameObject.SetActive(true);

        Debug.Log("GAME CLEAR 표시 완료");
    }

    private void ReloadStage()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}