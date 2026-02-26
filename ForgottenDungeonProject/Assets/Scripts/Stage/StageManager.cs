using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class StageManager : MonoBehaviour
{
    [Header("=== 현재 단계 ===")]
    [Tooltip("현재 진행 단계")]
    public int currentStage = 1;

    [Tooltip("최대 단계 수")]
    public int maxStage = 8;

    [Header("=== Stage 데이터 목록 ===")]
    [Tooltip("StageData ScriptableObject 리스트 (비어있으면 오류 발생)")]
    public List<StageData> stageDataList;

    private StageData currentStageData;

    void Start()
    {
        if (stageDataList == null || stageDataList.Count == 0)
        {
            Debug.LogError("StageDataList가 비어 있습니다. Inspector에서 연결하세요.");
            return;
        }

        LoadStage(currentStage);
    }

    public void LoadStage(int stageNumber)
    {
        if (stageDataList == null)
        {
            Debug.LogError("StageDataList가 null입니다.");
            return;
        }

        currentStage = stageNumber;

        currentStageData = stageDataList.Find(s => s != null && s.stageNumber == stageNumber);

        if (currentStageData == null)
        {
            Debug.LogError("해당 StageData를 찾을 수 없습니다: " + stageNumber);
            return;
        }

        Debug.Log("현재 단계: " + currentStage);
    }

    public void LoadNextStage()
    {
        currentStage++;

        if (currentStage > maxStage)
        {
            EndGame();
            return;
        }

        LoadStage(currentStage);
    }

    public void ResetGame()
    {
        currentStage = 1;
        LoadStage(currentStage);
    }

    void EndGame()
    {
        Debug.Log("게임 클리어");
        SceneManager.LoadScene("EndingScene");
    }

    public StageData GetCurrentStageData()
    {
        return currentStageData;
    }
}