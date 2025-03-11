using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("�̵� ����")]
    public float moveSpeed = 5f; // �̵� �ӵ�
    private Vector2 curMovementInput; // ���� �̵� �Է� �� ����

    [Header("���� ����")]
    public float jumpForce = 120f; // ���� ��
    public float groundCheckDistance = 1.1f; // �ٴ� üũ �Ÿ�
    public LayerMask groundLayer; // �ٴ� ���� ���̾�
    private bool isGrounded = false; // �ٴ� üũ

    [Header("���� ����")]
    public Transform cameraContainer; // ī�޶� �����̳� (1��Ī/3��Ī ����)
    public float minLook = -60f;
    public float maxLook = 60f;
    public float lookSensitivity = 0.1f;
    private float camCurX;
    private Vector2 mouseDelta;

    [Header("�κ��丮 ����")]
    public GameObject inventoryUI; // �κ��丮 UI �г� (SetActive�� Ȱ��ȭ/��Ȱ��ȭ)
    private bool isInventoryOpen = false;

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
    }

    private void LateUpdate()
    {
        CameraLook();
        mouseDelta = Vector2.zero; // �Է°� �ʱ�ȭ
    }

    public void Move()
    {
        // ���� ī�޶��� ���� ������ �������� �̵� ���� ����
        Vector3 forward = cameraContainer.forward;
        Vector3 right = cameraContainer.right;

        // ��/�Ʒ� ������ �����Ͽ� ���� �̵��� �����ϵ��� ����
        forward.y = 0;
        right.y = 0;

        // �̵� ���� ��� (ī�޶� ������ �������� �̵�)
        Vector3 moveDirection = (forward * curMovementInput.y + right * curMovementInput.x).normalized;

        // ���� �̵� �ӵ� ����
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
            _rigidbody.velocity = new Vector3(_rigidbody.velocity.x, 0, _rigidbody.velocity.z); // ���� Y�� �ӵ� �ʱ�ȭ
            _rigidbody.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }

    public void OnToggleInventory(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started) // Ű�� ������ �� ����
        {
            isInventoryOpen = !isInventoryOpen;
            inventoryUI.SetActive(isInventoryOpen);
        }
    }

    public void OnInteract(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started)
        {
            Collider[] hitColliders = Physics.OverlapSphere(transform.position, 2f); // ��ó ������ ����
            foreach (Collider col in hitColliders)
            {
                ItemPickup item = col.GetComponent<ItemPickup>();
                if (item != null)
                {
                    item.OnInteract(context);
                    break; // ù ��° ������ �����۸� ��ȣ�ۿ�
                }
            }
        }
    }

    void CheckGrounded()
    {
        float sphereRadius = 0.3f; // ���� �ݰ�
        float sphereDistance = 1.1f; // ������ �ִ� �Ÿ�
        Vector3 sphereOrigin = transform.position + Vector3.up * 0.1f; // ĳ���� ��¦ ������ ����

        isGrounded = Physics.SphereCast(sphereOrigin, sphereRadius, Vector3.down, out _, sphereDistance, groundLayer);
    }
}
