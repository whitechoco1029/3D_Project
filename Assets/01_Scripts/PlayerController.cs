using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("이동 설정")]
    public float moveSpeed = 5f; // 이동 속도
    private Vector2 curMovementInput; // 현재 이동 입력 값

    [Header("점프 설정")]
    public float jumpForce = 5f; // 점프 힘
    public float groundCheckDistance = 0.3f;  // 바닥 체크 거리
    public LayerMask groundLayer;  // 바닥 레이어 지정
    private bool isGrounded = false; // 바닥 체크
    private bool isJumping = false; // 점프 중인지 체크

    [Header("시점 설정")]
    public Transform cameraContainer; // 카메라 컨테이너
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
        _rigidbody.useGravity = true; // 중력 활성화
    }

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    void FixedUpdate()
    {
        Move(); // 물리 기반 이동 처리
        CheckGrounded(); // 바닥 감지
        ApplyGravity(); // 중력 보정
    }

    private void LateUpdate()
    {
        CameraLook();
        mouseDelta = Vector2.zero; // 입력값 초기화
    }

    public void Move()
    {
        Vector3 forward = cameraContainer.forward;
        Vector3 right = cameraContainer.right;

        forward.y = 0; // Y축 제거하여 회전 시 이동이 틀어지는 문제 해결
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
            _rigidbody.velocity = new Vector3(_rigidbody.velocity.x, 0, _rigidbody.velocity.z); // 기존 Y속도 초기화
            _rigidbody.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }

    void CheckGrounded()
    {
        float sphereRadius = 0.3f; // 감지 반경
        float sphereDistance = 1.1f; // 감지할 최대 거리
        Vector3 sphereOrigin = transform.position + Vector3.up * 0.1f; // 캐릭터 살짝 위에서 시작

        isGrounded = Physics.SphereCast(sphereOrigin, sphereRadius, Vector3.down, out _, sphereDistance, groundLayer);

        if (isGrounded && isJumping)
        {
            isJumping = false;
            _rigidbody.velocity = new Vector3(_rigidbody.velocity.x, 0, _rigidbody.velocity.z); // 착지 시 속도 초기화
        }
    }

    void ApplyGravity()
    {
        if (!isGrounded)
        {
            _rigidbody.velocity += Vector3.down * 5f * Time.deltaTime; // 추가 중력 적용
        }
    }
}
