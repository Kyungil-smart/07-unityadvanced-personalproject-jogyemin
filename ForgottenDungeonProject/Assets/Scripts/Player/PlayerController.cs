using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    [Header("=== 이동 설정 ===")]
    [Tooltip("기본 이동 속도")]
    public float moveSpeed = 4f;

    [Tooltip("달리기 속도")]
    public float runSpeed = 7f;

    [Tooltip("중력")]
    public float gravity = -9.81f;

    [Tooltip("지면 고정 힘")]
    public float groundStickForce = -2f;

    [Header("=== 마우스 설정 ===")]
    [Tooltip("마우스 감도 (0.05 ~ 0.15 추천)")]
    public float mouseSensitivity = 0.08f;

    [Tooltip("상하 시점 제한")]
    public float verticalClamp = 80f;

    private CharacterController controller;
    private Transform cameraPivot;

    private InputAction moveAction;
    private InputAction lookAction;
    private InputAction runAction;

    private float yVelocity;
    private float xRotation;

    void Awake()
    {
        controller = GetComponent<CharacterController>();
        cameraPivot = transform.Find("CameraPivot");

        moveAction = new InputAction("Move", InputActionType.Value);
        moveAction.AddCompositeBinding("2DVector")
            .With("Up", "<Keyboard>/w")
            .With("Down", "<Keyboard>/s")
            .With("Left", "<Keyboard>/a")
            .With("Right", "<Keyboard>/d");

        lookAction = new InputAction("Look", InputActionType.Value);
        lookAction.AddBinding("<Mouse>/delta");

        runAction = new InputAction("Run", InputActionType.Button);
        runAction.AddBinding("<Keyboard>/leftShift");
    }

    void OnEnable()
    {
        moveAction.Enable();
        lookAction.Enable();
        runAction.Enable();
    }

    void OnDisable()
    {
        moveAction.Disable();
        lookAction.Disable();
        runAction.Disable();
    }

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        HandleLook();
        HandleMove();
    }

    void HandleLook()
    {
        Vector2 look = lookAction.ReadValue<Vector2>();

        float mouseX = look.x * mouseSensitivity;
        float mouseY = look.y * mouseSensitivity;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -verticalClamp, verticalClamp);

        if (cameraPivot != null)
            cameraPivot.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

        transform.Rotate(Vector3.up * mouseX);
    }

    void HandleMove()
    {
        Vector2 input = moveAction.ReadValue<Vector2>();
        bool running = runAction.IsPressed();

        float speed = running ? runSpeed : moveSpeed;

        Vector3 move = transform.right * input.x + transform.forward * input.y;

        if (controller.isGrounded && yVelocity < 0)
            yVelocity = groundStickForce;

        yVelocity += gravity * Time.deltaTime;

        Vector3 velocity = move * speed;
        velocity.y = yVelocity;

        controller.Move(velocity * Time.deltaTime);
    }
}