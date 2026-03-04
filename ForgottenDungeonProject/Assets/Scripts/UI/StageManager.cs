using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

[DisallowMultipleComponent]
public class StageManager : MonoBehaviour
{
    public static StageManager Instance;

    [Header("===== UI 설정 =====")]
    [Tooltip("Hierarchy에 있는 'StageText' 오브젝트를 자동으로 찾습니다.")]
    [SerializeField] private TextMeshProUGUI stageText;

    [Header("===== 현재 스테이지 =====")]
    [Tooltip("현재 진행 중인 스테이지 번호")]
    [SerializeField] private int currentStage = 0;

    [Header("===== 최대 스테이지 =====")]
    [Tooltip("최대 스테이지 번호 (예: 8)")]
    [SerializeField] private int maxStage = 8;

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
        // 🔥 정확히 StageText 이름으로 찾기
        stageText = FindStageTextByName();

        UpdateUI();
    }

    private TextMeshProUGUI FindStageTextByName()
    {
        TextMeshProUGUI[] allTexts =
            Object.FindObjectsByType<TextMeshProUGUI>(
                FindObjectsInactive.Include,
                FindObjectsSortMode.None);

        foreach (var txt in allTexts)
        {
            if (txt.name == "StageText")
                return txt;
        }

        Debug.LogWarning("StageText를 찾지 못했습니다. 이름을 정확히 확인하세요.");
        return null;
    }

    private void UpdateUI()
    {
        if (stageText != null)
        {
            stageText.text = "Stage " + currentStage;
        }
    }

    public void CorrectAnswer()
    {
        if (currentStage < maxStage)
        {
            currentStage++;
        }

        UpdateUI();
    }

    public void SetStage(int value)
    {
        currentStage = Mathf.Clamp(value, 0, maxStage);
        UpdateUI();
    }

    public int GetStage()
    {
        return currentStage;
    }
}