using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// 메인 메뉴 UI 제어 스크립트
/// - 시작 버튼
/// - 튜토리얼 열기/닫기
/// - 게임 종료
/// </summary>
public class MainMenuUI : MonoBehaviour
{
    [Header("===== 튜토리얼 패널 =====")]
    [Tooltip("Hierarchy의 TutorialPanel 오브젝트를 드래그하세요.")]
    public GameObject tutorialPanel;

    [Header("===== 스토리 캔버스 =====")]
    [Tooltip("Hierarchy의 StoryCanvas 오브젝트를 드래그하세요.")]
    public GameObject storyCanvas;

    [Header("===== 이동할 씬 이름 =====")]
    [Tooltip("스토리가 끝난 후 이동할 게임 씬 이름입니다.")]
    public string gameSceneName = "Game";

    private void Start()
    {
        // 마우스 커서 보이게 설정
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        // 시작 시 튜토리얼 숨김
        if (tutorialPanel != null)
            tutorialPanel.SetActive(false);

        // 시작 시 스토리 화면 숨김
        if (storyCanvas != null)
            storyCanvas.SetActive(false);
    }

    /// <summary>
    /// 게임 시작 버튼 클릭
    /// </summary>
    public void OnClickStartGame()
    {
        if (storyCanvas != null)
        {
            // 스토리 화면 활성화
            storyCanvas.SetActive(true);

            // 메인 메뉴 패널 비활성화
            this.gameObject.SetActive(false);
        }
        else
        {
            // 스토리 캔버스가 연결되지 않았으면 바로 게임 씬 이동
            SceneManager.LoadScene(gameSceneName);
        }
    }

    /// <summary>
    /// 튜토리얼 열기
    /// </summary>
    public void OnClickOpenTutorial()
    {
        if (tutorialPanel != null)
            tutorialPanel.SetActive(true);
    }

    /// <summary>
    /// 튜토리얼 닫기
    /// </summary>
    public void OnClickCloseTutorial()
    {
        if (tutorialPanel != null)
            tutorialPanel.SetActive(false);
    }

    /// <summary>
    /// 게임 종료
    /// </summary>
    public void OnClickQuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}