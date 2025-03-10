using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("이동 설정")]
    public float moveSpeed = 5f; // 이동 속도
    private Vector2 curMovementInput; // 현재 이동 입력 값 저장

    [Header("점프 설정")]
    public float jumpForce = 120f; // 점프 힘
    public float groundCheckDistance = 1.1f; // 바닥 체크 거리
    public LayerMask groundLayer; // 바닥 감지 레이어
    private bool isGrounded = false; // 바닥 체크

    [Header("시점 설정")]
    public Transform cameraContainer; // 카메라 컨테이너 (1인칭/3인칭 지원)
    public float minLook = -60f;
    public float maxLook = 60f;
    public float lookSensitivity = 0.1f;
    private float camCurX;
    private Vector2 mouseDelta;

    [Header("인벤토리 설정")]
    public GameObject inventoryUI; // 인벤토리 UI 패널 (SetActive로 활성화/비활성화)
    private bool isInventoryOpen = false;

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
    }

    private void LateUpdate()
    {
        CameraLook();
        mouseDelta = Vector2.zero; // 입력값 초기화
    }

    public void Move()
    {
        // 현재 카메라의 정면 방향을 기준으로 이동 방향 설정
        Vector3 forward = cameraContainer.forward;
        Vector3 right = cameraContainer.right;

        // 위/아래 방향을 제거하여 수평 이동만 가능하도록 설정
        forward.y = 0;
        right.y = 0;

        // 이동 벡터 계산 (카메라 방향을 기준으로 이동)
        Vector3 moveDirection = (forward * curMovementInput.y + right * curMovementInput.x).normalized;

        // 최종 이동 속도 적용
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
            _rigidbody.velocity = new Vector3(_rigidbody.velocity.x, 0, _rigidbody.velocity.z); // 기존 Y축 속도 초기화
            _rigidbody.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }

    public void OnToggleInventory(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started) // 키가 눌렸을 때 실행
        {
            isInventoryOpen = !isInventoryOpen;
            inventoryUI.SetActive(isInventoryOpen);
        }
    }

    void CheckGrounded()
    {
        float sphereRadius = 0.3f; // 감지 반경
        float sphereDistance = 1.1f; // 감지할 최대 거리
        Vector3 sphereOrigin = transform.position + Vector3.up * 0.1f; // 캐릭터 살짝 위에서 시작

        isGrounded = Physics.SphereCast(sphereOrigin, sphereRadius, Vector3.down, out _, sphereDistance, groundLayer);
    }
}
