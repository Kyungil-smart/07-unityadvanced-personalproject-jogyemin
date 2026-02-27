using UnityEngine;

public class AnomalyController : MonoBehaviour
{
    [Tooltip("이상현상으로 활성/비활성 될 오브젝트들")]
    [SerializeField] private GameObject[] anomalyObjects;

    private void Start()
    {
        bool active = GameManager.Instance.HasAnomaly;

        foreach (var obj in anomalyObjects)
        {
            obj.SetActive(active);
        }
    }
}