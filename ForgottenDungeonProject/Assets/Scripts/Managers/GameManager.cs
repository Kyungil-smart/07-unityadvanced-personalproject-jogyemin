using UnityEngine;
using UnityEngine.SceneManagement;

[DisallowMultipleComponent]
public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("스테이지 설정")]
    [Tooltip("최종 스테이지 번호 (8이면 8에서 클리어)")]
    [SerializeField] private int maxStage = 8;

    [Tooltip("이번 스테이지에 이상현상이 존재하는지 여부 (읽기 전용)")]
    [SerializeField] private bool hasAnomaly;

    [SerializeField] private AnomalySystem anomalySystem;

    public bool HasAnomaly => hasAnomaly;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
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
        // 🔥 새 씬에서 AnomalySystem 다시 찾기
        anomalySystem = FindObjectOfType<AnomalySystem>();

        if (anomalySystem == null)
            Debug.LogWarning("AnomalySystem을 찾지 못했습니다.");

        StartStage();
    }

    // ==============================
    // 스테이지 시작
    // ==============================
    public void StartStage()
    {
        int stage = StageManager.Instance.GetStage();

        // 🔵 Stage 1 = 관찰 단계
        if (stage == 1)
        {
            hasAnomaly = false;

            Debug.Log("=== Stage 1 (관찰 단계) ===");

            if (anomalySystem != null)
                anomalySystem.ResetAll();   // 🔥 이상현상 적용하지 않음

            return;
        }

        // 🔴 Stage 2 이상부터 랜덤
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
        int stage = StageManager.Instance.GetStage();

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
                Debug.Log("게임 클리어");
                StageManager.Instance.SetStage(1);
                ReloadStage();
                return;
            }
        }
        else
        {
            Debug.Log("실패 → Stage 1로 리셋");
            StageManager.Instance.SetStage(1);
        }

        ReloadStage();
    }

    private void ReloadStage()
    {
        SceneManager.LoadScene(
            SceneManager.GetActiveScene().name);
    }
}
