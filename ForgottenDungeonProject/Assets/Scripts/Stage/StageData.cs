using UnityEngine;

[CreateAssetMenu(fileName = "StageData", menuName = "ForgottenDungeon/Stage Data")]
public class StageData : ScriptableObject
{
    [Header("=== 기본 정보 ===")]

    [Tooltip("단계 번호")]
    public int stageNumber;

    [Tooltip("제한 시간 (초 단위)")]
    public float timeLimit;

    [Tooltip("난이도 수치 (1~10 권장)")]
    [Range(1, 10)]
    public int difficulty;

    [Tooltip("이상현상 발생 확률 (0~1)")]
    [Range(0f, 1f)]
    public float anomalyProbability;
}