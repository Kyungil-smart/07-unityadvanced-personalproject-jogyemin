using UnityEngine;
using UnityEngine.InputSystem;

[DisallowMultipleComponent]
public class PlayerController : MonoBehaviour
{
    [Header("필수 연결")]
    [Tooltip("1인칭 카메라(플레이어 자식). 여기의 회전(X축: 상하)을 제어합니다.")]
    [SerializeField] private Camera playerCamera;

    [Header("이동 설정")]
    [Tooltip("일반 이동 속도 (WASD).")]
    [SerializeField] private float walkSpeed = 3.5f;

    [Tooltip("달리기 이동 속도 (Shift 누르는 동안).")]
    [SerializeField] private float runSpeed = 6.0f;

    [Tooltip("중력 값(기본 -9.81). CharacterController는 Rigidbody가 아니므로 중력은 직접 적용합니다.")]
    [SerializeField] private float gravity = -9.81f;

    [Tooltip("바닥에 붙이기 위한 작은 하강값. 경사/계단에서 '뜸' 현상을 줄입니다.")]
    [SerializeField] private float groundedStickForce = -2.0f;

    [Header("시점(마우스) 설정")]
    [Tooltip("마우스 좌우/상하 회전 민감도. 값이 클수록 더 빠르게 회전합니다.")]
    [SerializeField] private float lookSensitivity = 0.08f;

    [Tooltip("상하 시야 제한(위). 예: 80이면 위로 80도까지.")]
    [SerializeField] private float pitchMax = 80f;

    [Tooltip("상하 시야 제한(아래). 예: -80이면 아래로 80도까지.")]
    [SerializeField] private float pitchMin = -80f;

    [Header("Input Actions (PlayerInputActions 에셋의 액션을 드래그해서 연결)")]
    [Tooltip("Move(Vector2): WASD 이동 입력 액션을 연결하세요.")]
    [SerializeField] private InputActionReference moveAction;

    [Tooltip("Look(Vector2): 마우스 Delta 입력 액션을 연결하세요.")]
    [SerializeField] private InputActionReference lookAction;

    [Tooltip("Run(Button): Left Shift 입력 액션을 연결하세요.")]
    [SerializeField] private InputActionReference runAction;

    private CharacterController _cc;
    private float _verticalVelocity;
    private float _pitch; // 카메라 상하 회전 누적값

    private void Awake()
    {
        _cc = GetComponent<CharacterController>();

        if (playerCamera == null)
        {
            Debug.LogError("[FirstPersonController] Player Camera가 연결되지 않았습니다. PlayerCamera를 드래그해서 넣어주세요.");
        }
    }

    private void OnEnable()
    {
        // 액션 활성화 (PlayerInput이 있어도, 안전하게 직접 Enable 해둡니다)
        if (moveAction != null) moveAction.action.Enable();
        if (lookAction != null) lookAction.action.Enable();
        if (runAction != null) runAction.action.Enable();

        // 1인칭 기본: 마우스 커서 잠금
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void OnDisable()
    {
        if (moveAction != null) moveAction.action.Disable();
        if (lookAction != null) lookAction.action.Disable();
        if (runAction != null) runAction.action.Disable();

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    private void Update()
    {
        HandleLook();   // 마우스 시점
        HandleMove();   // WASD + Shift 달리기 + 중력
    }

    private void HandleLook()
    {
        if (lookAction == null || playerCamera == null) return;

        Vector2 look = lookAction.action.ReadValue<Vector2>();

        // 마우스 좌우: 플레이어(몸통) Y 회전
        float yaw = look.x * lookSensitivity;
        transform.Rotate(0f, yaw, 0f);

        // 마우스 상하: 카메라 X 회전(피치)
        _pitch -= look.y * lookSensitivity;
        _pitch = Mathf.Clamp(_pitch, pitchMin, pitchMax);

        playerCamera.transform.localRotation = Quaternion.Euler(_pitch, 0f, 0f);
    }

    private void HandleMove()
    {
        if (moveAction == null) return;

        Vector2 moveInput = moveAction.action.ReadValue<Vector2>();
        bool isRunning = (runAction != null) && runAction.action.IsPressed();

        float speed = isRunning ? runSpeed : walkSpeed;

        // 방향: 플레이어 기준 앞/오른쪽
        Vector3 move = (transform.right * moveInput.x + transform.forward * moveInput.y);
        move = Vector3.ClampMagnitude(move, 1f);

        // 바닥 체크: CharacterController.isGrounded 사용
        if (_cc.isGrounded)
        {
            // 바닥에 붙는 힘 (약간 아래로)
            if (_verticalVelocity < 0f)
                _verticalVelocity = groundedStickForce;
        }
        else
        {
            // 공중이면 중력 적용
            _verticalVelocity += gravity * Time.deltaTime;
        }

        Vector3 velocity = move * speed;
        velocity.y = _verticalVelocity;

        _cc.Move(velocity * Time.deltaTime);
    }
}