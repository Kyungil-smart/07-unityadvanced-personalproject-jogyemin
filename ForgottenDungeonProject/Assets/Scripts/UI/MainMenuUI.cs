using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuUI : MonoBehaviour
{
    [Header("===== 튜토리얼 패널 =====")]
    [Tooltip("Hierarchy의 TutorialPanel을 드래그하세요")]
    public GameObject tutorialPanel;

    [Header("===== 이동할 씬 이름 =====")]
    public string gameSceneName = "Game";

    void Start()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        if (tutorialPanel != null)
            tutorialPanel.SetActive(false);
    }

    // 게임 시작
    public void OnClickStartGame()
    {
        SceneManager.LoadScene(gameSceneName);
    }

    // 튜토리얼 열기
    public void OnClickOpenTutorial()
    {
        if (tutorialPanel != null)
            tutorialPanel.SetActive(true);
    }

    // 튜토리얼 닫기
    public void OnClickCloseTutorial()
    {
        if (tutorialPanel != null)
            tutorialPanel.SetActive(false);
    }

    // 게임 종료
    public void OnClickQuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}