using UnityEngine;
using TMPro;

public class TimerManager : MonoBehaviour
{
    [Header("===== UI 참조 =====")]
    [Tooltip("중앙 상단에 표시될 남은 시간 텍스트")]
    public TextMeshProUGUI timerText;

    [Header("===== 시간 설정 =====")]
    [Tooltip("제한 시간 (분 단위)")]
    [Range(1, 60)]
    public int limitMinutes = 10;

    private float remainingTime;
    private bool isRunning = true;

    void Start()
    {
        remainingTime = limitMinutes * 60f;
        UpdateUI();
    }

    void Update()
    {
        if (!isRunning) return;

        if (remainingTime > 0)
        {
            remainingTime -= Time.deltaTime;
            UpdateUI();
        }
        else
        {
            remainingTime = 0;
            isRunning = false;

            // 시간 끝나면 스테이지 초기화
            if (StageManager.Instance != null)
                StageManager.Instance.SetStage(1);
        }
    }

    void UpdateUI()
    {
        if (timerText == null) return;

        int min = Mathf.FloorToInt(remainingTime / 60f);
        int sec = Mathf.FloorToInt(remainingTime % 60f);

        timerText.text = $"{min:00}:{sec:00}";
    }
}