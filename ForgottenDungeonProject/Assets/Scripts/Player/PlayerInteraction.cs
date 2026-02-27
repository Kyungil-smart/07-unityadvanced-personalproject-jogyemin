using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteraction : MonoBehaviour
{
    [Header("입력 설정")]
    [Tooltip("G키 상호작용 Input Action 연결")]
    [SerializeField] private InputActionReference interactAction;

    [Header("감지 설정")]
    [Tooltip("문 감지 최대 거리")]
    [SerializeField] private float interactDistance = 3f;

    [Tooltip("플레이어 카메라")]
    [SerializeField] private Camera playerCamera;

    private StageDoorTrigger currentDoor;

    private void OnEnable()
    {
        if (interactAction != null)
            interactAction.action.Enable();
    }

    private void OnDisable()
    {
        if (interactAction != null)
            interactAction.action.Disable();
    }

    private void Update()
    {
        DetectDoor();

        if (currentDoor != null &&
            interactAction.action.WasPressedThisFrame())
        {
            GameManager.Instance.EvaluateChoice(
                currentDoor.IsAnomalyDoor);
        }
    }

    private void DetectDoor()
    {
        currentDoor = null;

        Ray ray = new Ray(
            playerCamera.transform.position,
            playerCamera.transform.forward);

        if (Physics.Raycast(ray, out RaycastHit hit, interactDistance))
        {
            StageDoorTrigger door =
                hit.collider.GetComponent<StageDoorTrigger>();

            if (door != null)
            {
                currentDoor = door;
                Debug.Log("문 감지됨 - G를 눌러 선택");
            }
        }
    }
}