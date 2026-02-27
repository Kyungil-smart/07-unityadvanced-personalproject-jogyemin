using UnityEngine;
using UnityEngine.SceneManagement;

[DisallowMultipleComponent]
public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("스테이지 설정")]

    [Tooltip("총 스테이지 수 (고정 8)")]
    [SerializeField] private int maxStage = 8;

    [Tooltip("현재 진행 중인 스테이지 번호")]
    [SerializeField] private int currentStage = 1;

    [Tooltip("이번 스테이지에 이상현상이 존재하는지 여부")]
    [SerializeField] private bool hasAnomaly;

    public int CurrentStage => currentStage;
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

    private void Start()
    {
        StartStage();
    }

    public void StartStage()
    {
        hasAnomaly = Random.value > 0.5f;

        Debug.Log("=== 스테이지 시작 ===");
        Debug.Log("현재 스테이지: " + currentStage);
        Debug.Log("이상현상 존재 여부: " + hasAnomaly);
    }

    public void EvaluateChoice(bool playerChoseAnomalyDoor)
    {
        bool success =
            (hasAnomaly && playerChoseAnomalyDoor) ||
            (!hasAnomaly && !playerChoseAnomalyDoor);

        if (success)
        {
            currentStage++;

            if (currentStage > maxStage)
            {
                Debug.Log("게임 클리어!");
                currentStage = 1; // 클리어 후 초기화
                ReloadStage();
                return;
            }

            Debug.Log("성공! 다음 스테이지: " + currentStage);
        }
        else
        {
            Debug.Log("실패! 스테이지 1로 리셋");
            currentStage = 1;
        }

        ReloadStage();
    }

    private void ReloadStage()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}