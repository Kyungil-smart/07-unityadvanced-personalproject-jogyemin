using UnityEngine;

public class GameQuitButton : MonoBehaviour
{
    [Header("===== 버튼 설정 =====")]
    [Tooltip("게임 종료 버튼을 눌렀을 때 게임을 종료합니다.")]
    [SerializeField] private bool useQuit = true;

    public void QuitGame()
    {
        Debug.Log("게임 종료 버튼 클릭");

        if (useQuit)
        {
            Application.Quit();
        }

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}