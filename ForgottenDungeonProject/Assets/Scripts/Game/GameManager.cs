using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.UI;
using TMPro;

[DisallowMultipleComponent]
public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("===== 스테이지 설정 =====")]
    [Tooltip("최종 스테이지 번호 (8이면 8단계에서 클리어)")]
    [SerializeField] private int maxStage = 8;

    [Header("===== 클리어 연출 UI =====")]
    [Tooltip("검은 화면과 텍스트를 포함하는 ClearCanvas")]
    [SerializeField] private GameObject clearCanvas;

    [Tooltip("전체 화면을 덮는 검은색 Image")]
    [SerializeField] private Image blackImage;

    [Tooltip("GAME CLEAR 텍스트")]
    [SerializeField] private TextMeshProUGUI clearText;

    [Tooltip("페이드 연출 시간(초)")]
    [SerializeField] private float fadeDuration = 3f;

    [Header("===== 게임 상태 =====")]
    [SerializeField] private bool hasAnomaly;

    [Tooltip("씬의 AnomalySystem 자동 연결")]
    [SerializeField] private AnomalySystem anomalySystem;

    private bool isGameCleared = false;

    public bool HasAnomaly => hasAnomaly;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        anomalySystem = Object.FindFirstObjectByType<AnomalySystem>(FindObjectsInactive.Include);

        if (clearCanvas == null)
        {
            GameObject canvasObj = GameObject.Find("ClearCanvas");
            if (canvasObj != null)
                clearCanvas = canvasObj;
        }

        if (blackImage == null)
        {
            Image img = Object.FindFirstObjectByType<Image>(FindObjectsInactive.Include);
            if (img != null && img.gameObject.name == "BlackImage")
                blackImage = img;
        }

        if (clearText == null)
        {
            TextMeshProUGUI[] texts = Object.FindObjectsByType<TextMeshProUGUI>(FindObjectsInactive.Include, FindObjectsSortMode.None);

            foreach (var t in texts)
            {
                if (t.name == "ClearText")
                {
                    clearText = t;
                    break;
                }
            }
        }

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

        StartStage();
    }

    public void StartStage()
    {
        if (isGameCleared) return;

        int stage = StageManager.Instance.GetStage();

        if (stage == 0)
        {
            hasAnomaly = false;

            Debug.Log("=== Stage 0 (관찰 단계) ===");

            anomalySystem?.ResetAll();
            return;
        }

        hasAnomaly = Random.value > 0.5f;

        Debug.Log("=== Stage " + stage + " 시작 ===");
        Debug.Log("이상현상 존재 여부: " + (hasAnomaly ? "있음" : "없음"));

        anomalySystem?.ApplyStageAnomaly(hasAnomaly);
    }

    public void EvaluateChoice(bool playerChoseAnomalyDoor)
    {
        if (isGameCleared) return;

        bool success =
            (hasAnomaly && playerChoseAnomalyDoor) ||
            (!hasAnomaly && !playerChoseAnomalyDoor);

        int currentStage = StageManager.Instance.GetStage();

        if (success)
        {
            if (currentStage == maxStage)
            {
                StartCoroutine(ClearSequence());
                return;
            }

            StageManager.Instance.CorrectAnswer();
            ReloadStage();
        }
        else
        {
            StageManager.Instance.SetStage(0);
            ReloadStage();
        }
    }

    IEnumerator ClearSequence()
    {
        isGameCleared = true;

        PlayerController player = Object.FindFirstObjectByType<PlayerController>();
        if (player != null) player.enabled = false;

        PlayerInteraction interaction = Object.FindFirstObjectByType<PlayerInteraction>();
        if (interaction != null) interaction.enabled = false;

        CharacterController cc = Object.FindFirstObjectByType<CharacterController>();
        if (cc != null) cc.enabled = false;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        if (clearCanvas != null)
            clearCanvas.SetActive(true);

        if (clearText != null)
            clearText.gameObject.SetActive(false);

        float t = 0f;

        while (t < fadeDuration)
        {
            t += Time.deltaTime;

            if (blackImage != null)
            {
                float alpha = Mathf.Lerp(0f, 1f, t / fadeDuration);
                Color c = blackImage.color;
                c.a = alpha;
                blackImage.color = c;
            }

            yield return null;
        }

        if (blackImage != null)
            blackImage.color = new Color(0, 0, 0, 1);

        yield return new WaitForSeconds(1f);

        if (clearText != null)
            clearText.gameObject.SetActive(true);

        // ===== 여기서 버튼 등장 연출 호출 =====
        ClearUIController clearUI = Object.FindFirstObjectByType<ClearUIController>();
        if (clearUI != null)
        {
            clearUI.ShowClearUI();
        }
    }

    void ReloadStage()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}