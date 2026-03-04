using UnityEngine;
using TMPro;
using System.Collections;

public class ClearUIController : MonoBehaviour
{
    [Header("클리어 UI")]
    [Tooltip("클리어 텍스트")]
    [SerializeField] private TextMeshProUGUI clearText;

    [Tooltip("게임 종료 버튼")]
    [SerializeField] private GameObject quitButton;

    [Header("연출 설정")]
    [Tooltip("버튼 등장 대기 시간")]
    [SerializeField] private float buttonDelay = 3f;

    void Awake()
    {
        if (quitButton != null)
            quitButton.SetActive(false);
    }

    public void ShowClearUI()
    {
        StartCoroutine(ClearSequence());
    }

    IEnumerator ClearSequence()
    {
        clearText.text = "GAME CLEAR";

        yield return new WaitForSeconds(buttonDelay);

        if (quitButton != null)
            quitButton.SetActive(true);
    }
}