using UnityEngine;

public class StageDoorTrigger : MonoBehaviour
{
    [Header("문 설정")]

    [Tooltip("체크하면 '이상현상 있음(입구)' 문")]
    [SerializeField] private bool isAnomalyDoor;

    public bool IsAnomalyDoor => isAnomalyDoor;
}