using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("�̵� ����")]
    public float moveSpeed = 5f; // �̵� �ӵ�
    private Vector2 curMovementInput; // ���� �̵� �Է� ��

    [Header("���� ����")]
    public float jumpForce = 5f; // ���� ��
    public float groundCheckDistance = 0.3f;  // �ٴ� üũ �Ÿ�
    public LayerMask groundLayer;  // �ٴ� ���̾� ����
    private bool isGrounded = false; // �ٴ� üũ
    private bool isJumping = false; // ���� ������ üũ

    [Header("���� ����")]
    public Transform cameraContainer; // ī�޶� �����̳�
    public float minLook = -60f;
    public float maxLook = 60f;
    public float lookSensitivity = 2f;
    private float camCurX;
    private Vector2 mouseDelta;

    private Rigidbody _rigidbody;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _rigidbody.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
        _rigidbody.useGravity = true; // �߷� Ȱ��ȭ
    }

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    void FixedUpdate()
    {
        Move(); // ���� ��� �̵� ó��
        CheckGrounded(); // �ٴ� ����
        ApplyGravity(); // �߷� ����
    }

    private void LateUpdate()
    {
        CameraLook();
        mouseDelta = Vector2.zero; // �Է°� �ʱ�ȭ
    }

    public void Move()
    {
        Vector3 forward = cameraContainer.forward;
        Vector3 right = cameraContainer.right;

        forward.y = 0; // Y�� �����Ͽ� ȸ�� �� �̵��� Ʋ������ ���� �ذ�
        right.y = 0;

        Vector3 moveDirection = (forward * curMovementInput.y + right * curMovementInput.x).normalized;

        _rigidbody.velocity = new Vector3(moveDirection.x * moveSpeed, _rigidbody.velocity.y, moveDirection.z * moveSpeed);
    }

    void CameraLook()
    {
        camCurX += mouseDelta.y * lookSensitivity;
        camCurX = Mathf.Clamp(camCurX, minLook, maxLook);
        cameraContainer.localEulerAngles = new Vector3(-camCurX, 0, 0);

        Quaternion targetRotation = Quaternion.Euler(0, transform.eulerAngles.y + mouseDelta.x * lookSensitivity, 0);
        transform.rotation = targetRotation;
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            curMovementInput = context.ReadValue<Vector2>();
        }
        else if (context.phase == InputActionPhase.Canceled)
        {
            curMovementInput = Vector2.zero;
        }
    }

    public void OnLook(InputAction.CallbackContext context)
    {
        mouseDelta = context.ReadValue<Vector2>();
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started && isGrounded)
        {
            isJumping = true;
            _rigidbody.velocity = new Vector3(_rigidbody.velocity.x, 0, _rigidbody.velocity.z); // ���� Y�ӵ� �ʱ�ȭ
            _rigidbody.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }

    void CheckGrounded()
    {
        float sphereRadius = 0.3f; // ���� �ݰ�
        float sphereDistance = 1.1f; // ������ �ִ� �Ÿ�
        Vector3 sphereOrigin = transform.position + Vector3.up * 0.1f; // ĳ���� ��¦ ������ ����

        isGrounded = Physics.SphereCast(sphereOrigin, sphereRadius, Vector3.down, out _, sphereDistance, groundLayer);

        if (isGrounded && isJumping)
        {
            isJumping = false;
            _rigidbody.velocity = new Vector3(_rigidbody.velocity.x, 0, _rigidbody.velocity.z); // ���� �� �ӵ� �ʱ�ȭ
        }
    }

    void ApplyGravity()
    {
        if (!isGrounded)
        {
            _rigidbody.velocity += Vector3.down * 5f * Time.deltaTime; // �߰� �߷� ����
        }
    }
}
