using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("이동")]
    public float moveSpeed; ///이동 속도
    private Vector2 curMovementInput; /// 현재 이동 입력 값 저장 (WASD)

    [Header("시점")]
    public Transform cameraContainer; /// 카메라에 부착된 오브젝트 (1,3인칭 적용가능)
    public float minLook = -60f; ///카메라 아랫쪽 최대 회전 값
    public float MaxLook = 60f ; ///최대값 위쪽 최대 회전 값
    public float lookSensitvity = 2f; /// 마우스 감도 설정
    private float camCurX; /// 카메라 x축 회전값 (위 아래 제한용)
    private Vector2 mouseDelta; ///마우스 입력값 저장



    private Rigidbody _rigidbody; ///물리 이동을 위한 Rigidbody


    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>(); ///Rigidbody 컴포넌트
        _rigidbody.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
        ///유니티 인스펙터에서 설정에서 셋팅했지만 코드에서 한번 더 적용
    }

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked; ///마우스 커서를 화면 중앙에 고정
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Move(); ///FixedYpdate()에서 이동을 처리(물리 기반 이동)
    }

    private void LateUpdate()
    {
        CameraLook(); ///카메라 시점 조작 처리
        mouseDelta = Vector2.zero; /// 입력값 초기화되며 흔들림 방지
    }
    public void Move() 
    {
        /// 이동 방향을 카메라 기준으로 계산 (WASD 입력에 따라 방향 결정)
        Vector3 dir = Vector3.forward * curMovementInput.y + transform.right * curMovementInput.x;
        dir *= moveSpeed; ///이동 속도

         /// 기존 Y축 속도를 유지하면서 이동 (점프, 중력 등에 영향을 받지 않도록)
        _rigidbody.velocity = new Vector3(dir.x, _rigidbody.velocity.y, dir.z);
    }

    void CameraLook()
    {
        camCurX += mouseDelta.y * lookSensitvity; /// 마우스 Y축 입력을 누적하여 위/아래 카메라 회전
        camCurX = Mathf.Clamp(camCurX, minLook, MaxLook); /// 위,아래 회전 각도를 min/max 범위로 제한하여 과도한 움직임 방지
        cameraContainer.localEulerAngles = new Vector3(-camCurX, 0, 0);

        transform.eulerAngles += new Vector3(0, mouseDelta.x, lookSensitvity); /// 마우스 X축 입력을 기준으로 캐릭터 자체의 Y축 회전 적용
    }
    public void OnMove(InputAction.CallbackContext context)
    {
        if(context.phase == InputActionPhase.Performed)
        {
            /// 이동 입력을 받으면 변수에 저장 (ex: WASD 입력)
            curMovementInput = context.ReadValue<Vector2>();
        }
        else if (context.phase == InputActionPhase.Canceled)
        {
            /// 이동 입력이 해제되면 0으로 설정
            curMovementInput = Vector2.zero;
        }
    }
    
    public void OnLook(InputAction.CallbackContext context)
    {
        /// 마우스 이동 값을 저장하여 카메라 회전에 사용
        mouseDelta = context.ReadValue<Vector2>();
    }
}
