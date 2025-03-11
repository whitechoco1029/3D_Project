프로젝트 개요
개발 언어: C#
엔진: Unity (현재 3D 개발 진행 중)


주요 기능
1. 플레이어 컨트롤
Rigidbody와 Quaternion을 활용한 물리 기반 이동 및 회전 시스템
Input System을 활용하여 키보드/마우스 입력 처리
CameraLook() 함수 적용으로 마우스를 활용한 부드러운 카메라 회전
Jump() 기능을 추가하여 중력과 지형에 영향을 받는 점프 시스템 구현
2. 인벤토리 시스템
SetActive()를 활용한 인벤토리 UI 열기/닫기 (Tab 키)
InventoryManager를 싱글톤 패턴(Instance)으로 구현하여 전역 관리 가능
ItemData (ScriptableObject)를 이용한 확장 가능한 아이템 데이터 관리
아이템 습득 후 UI 업데이트 및 리스트 관리
*미구현*
3. 아이템 시스템
BaseItem 클래스를 만들어 다형성 적용 (WeaponItem, ConsumableItem 등 상속)
ItemPickup 스크립트에서 Trigger Collider를 활용하여 습득 가능
OnInteract()를 InputAction.CallbackContext에 적용하여 인터랙션 시스템 개선
Physics.Raycast()를 활용한 필드 내 아이템 감지 및 습득
*미구현*
4. 오브젝트 물리 시스템
Rigidbody.constraints = FreezeRotationX | FreezeRotationZ를 적용하여 불필요한 회전 방지
Collision Detection을 Continuous Dynamic으로 설정하여 더 정밀한 충돌 감지
Capsule Collider와 Mesh Collider를 조합하여 환경과의 자연스러운 충돌 구현
5. UI 시스템
Canvas → InventoryUI를 활용한 인벤토리 창
ItemSlot 프리팹을 동적으로 생성하여 아이템 UI 자동 추가
ItemSlot.cs에서 SetItem() 함수를 통해 아이템 정보 UI에 반영
체력 시스템(Scrollbar.size 기반)과 연동하여 체력바 업데이트 기능 구현
개발 과정
1. 초기 개발

Unity 2D 기반으로 시작하여 점차 3D로 전환
플래피 버드, 도둑질(스텔스) 미니게임 등 미니게임 개발 경험 포함
2. Unity 3D 기반으로 전환

Rigidbody 기반의 이동 시스템 구현
카메라 이동 및 회전 기능 추가
Quaternion을 활용하여 Gimbal Lock 문제 해결
3. 아이템 및 인벤토리 시스템 구축

ScriptableObject를 활용하여 아이템을 확장 가능하도록 설계
InventoryManager를 싱글톤 패턴으로 관리하여 아이템 데이터 일관성 유지
BaseItem을 만들고, WeaponItem, ConsumableItem 등을 상속하여 객체 지향적 구조 적용
4. UI 및 인터랙션 시스템 추가

E 키를 누르면 아이템 습득 가능
Tab 키로 인벤토리 UI 열고 닫기
스크롤바를 활용한 체력 UI 시스템 구축
OnInteract()를 CallbackContext에 적용하여 더 정밀한 키 입력 처리
실행 방법
1. 플레이어 조작
키 입력	기능
WASD	플레이어 이동
마우스 이동	시점 회전
Space	점프
E	아이템 습득
Tab	인벤토리 열기/닫기
3. 아이템 습득 및 인벤토리 UI 연동
아이템(AK-47 프리팹 등)을 필드에 배치
플레이어가 가까이 가서 E 키를 누르면 아이템 획득
Tab 키를 눌러 인벤토리 확인
무기는 장착, 소모품은 즉시 사용 가능
