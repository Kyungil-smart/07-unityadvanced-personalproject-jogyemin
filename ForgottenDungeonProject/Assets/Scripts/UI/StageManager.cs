using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

[DisallowMultipleComponent]
public class StageManager : MonoBehaviour
{
    public static StageManager Instance;

    [Header("UI 참조")]
    [Tooltip("우측 상단에 표시될 Stage 텍스트")]
    public TextMeshProUGUI stageText;

    private int currentStage = 1;   // 🔥 기본값 1

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
        if (stageText == null)
            stageText = FindStageText();

        UpdateUI();
    }

    private TextMeshProUGUI FindStageText()
    {
        TextMeshProUGUI[] allTexts =
            FindObjectsOfType<TextMeshProUGUI>(true);

        foreach (var txt in allTexts)
        {
            if (txt.name == "StageText")
                return txt;
        }

        Debug.LogWarning("StageText를 찾지 못했습니다.");
        return null;
    }

    private void UpdateUI()
    {
        if (stageText == null) return;
        stageText.text = "Stage " + currentStage;
    }

    public void CorrectAnswer()
    {
        currentStage++;
        UpdateUI();
    }

    public void WrongAnswer()
    {
        currentStage = 1;   // 0이 아니라 1
        UpdateUI();
    }

    public void SetStage(int value)
    {
        currentStage = Mathf.Max(1, value);   // 최소 1 보장
        UpdateUI();
    }

    public int GetStage()
    {
        return currentStage;
    }
}
