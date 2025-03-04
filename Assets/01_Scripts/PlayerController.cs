using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("�̵�")]
    public float moveSpeed; ///�̵� �ӵ�
    private Vector2 curMovementInput; /// ���� �̵� �Է� �� ���� (WASD)

    [Header("����")]
    public Transform cameraContainer; /// ī�޶� ������ ������Ʈ (1,3��Ī ���밡��)
    public float minLook = -60f; ///ī�޶� �Ʒ��� �ִ� ȸ�� ��
    public float MaxLook = 60f ; ///�ִ밪 ���� �ִ� ȸ�� ��
    public float lookSensitvity = 2f; /// ���콺 ���� ����
    private float camCurX; /// ī�޶� x�� ȸ���� (�� �Ʒ� ���ѿ�)
    private Vector2 mouseDelta; ///���콺 �Է°� ����



    private Rigidbody _rigidbody; ///���� �̵��� ���� Rigidbody


    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>(); ///Rigidbody ������Ʈ
        _rigidbody.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
        ///����Ƽ �ν����Ϳ��� �������� ���������� �ڵ忡�� �ѹ� �� ����
    }

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked; ///���콺 Ŀ���� ȭ�� �߾ӿ� ����
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Move(); ///FixedYpdate()���� �̵��� ó��(���� ��� �̵�)
    }

    private void LateUpdate()
    {
        CameraLook(); ///ī�޶� ���� ���� ó��
        mouseDelta = Vector2.zero; /// �Է°� �ʱ�ȭ�Ǹ� ��鸲 ����
    }
    public void Move() 
    {
        /// �̵� ������ ī�޶� �������� ��� (WASD �Է¿� ���� ���� ����)
        Vector3 dir = Vector3.forward * curMovementInput.y + transform.right * curMovementInput.x;
        dir *= moveSpeed; ///�̵� �ӵ�

         /// ���� Y�� �ӵ��� �����ϸ鼭 �̵� (����, �߷� � ������ ���� �ʵ���)
        _rigidbody.velocity = new Vector3(dir.x, _rigidbody.velocity.y, dir.z);
    }

    void CameraLook()
    {
        camCurX += mouseDelta.y * lookSensitvity; /// ���콺 Y�� �Է��� �����Ͽ� ��/�Ʒ� ī�޶� ȸ��
        camCurX = Mathf.Clamp(camCurX, minLook, MaxLook); /// ��,�Ʒ� ȸ�� ������ min/max ������ �����Ͽ� ������ ������ ����
        cameraContainer.localEulerAngles = new Vector3(-camCurX, 0, 0);

        transform.eulerAngles += new Vector3(0, mouseDelta.x, lookSensitvity); /// ���콺 X�� �Է��� �������� ĳ���� ��ü�� Y�� ȸ�� ����
    }
    public void OnMove(InputAction.CallbackContext context)
    {
        if(context.phase == InputActionPhase.Performed)
        {
            /// �̵� �Է��� ������ ������ ���� (ex: WASD �Է�)
            curMovementInput = context.ReadValue<Vector2>();
        }
        else if (context.phase == InputActionPhase.Canceled)
        {
            /// �̵� �Է��� �����Ǹ� 0���� ����
            curMovementInput = Vector2.zero;
        }
    }
    
    public void OnLook(InputAction.CallbackContext context)
    {
        /// ���콺 �̵� ���� �����Ͽ� ī�޶� ȸ���� ���
        mouseDelta = context.ReadValue<Vector2>();
    }
}
