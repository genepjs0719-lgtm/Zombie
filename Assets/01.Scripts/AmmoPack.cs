using UnityEngine;

// 주어진 검(Sword) 오브젝트를 사용해 공격 동작을 처리하고
// IK를 사용해 캐릭터 양손이 검의 손잡이에 위치하도록 조정합니다.
public class PlayerAttacker : MonoBehaviour
{
    // C# 변수: 검 컴포넌트 및 트랜스폼 연결
    public Sword sword; // 사용할 검 컴포넌트 (위에서 수정한 Sword.cs)

    public Transform swordPivot; // 검 배치의 기준점
    public Transform leftHandMount; // 검의 왼손잡이, 왼손이 위치할 지점
    public Transform rightHandMount; // 검의 오른손잡이, 오른손이 위치할 지점

    private PlayerInput playerInput; // 플레이어의 입력
    private Animator playerAnimator; // 애니메이터 컴포넌트

    // C# 메서드: 초기화 (Awake나 Start에서 컴포넌트 초기화)
    private void Start()
    {
        // 사용할 컴포넌트들을 가져오기
        playerInput = GetComponent<PlayerInput>();
        playerAnimator = GetComponent<Animator>();

        // [안정성 강화] 필수 컴포넌트 연결 확인
        if (sword == null)
        {
            Debug.LogError("PlayerAttacker에 Sword 컴포넌트가 연결되지 않았습니다. 인스펙터에서 연결해주세요.");
        }
    }

    // C# 메서드: 활성화/비활성화 시 검 오브젝트 제어 (유지)
    private void OnEnable()
    {
        // [안정성 추가] null 체크 후 활성화
        if (sword != null)
        {
            sword.gameObject.SetActive(true);
        }
    }

    private void OnDisable()
    {
        // [안정성 추가] null 체크 후 비활성화
        if (sword != null)
        {
            sword.gameObject.SetActive(false);
        }
    }

    // C# 메서드: 매 프레임마다 입력 감지 및 공격 로직
    private void Update()
    {
        // [안정성 강화] sword가 null이 아니며, 입력이 감지되었을 때만 로직 실행
        if (sword == null) return;

        // 'fire' 입력을 감지하고 검의 Attack 메서드를 호출
        if (playerInput.fire)
        {
            // Attack() 메서드를 호출하여 공격 시도를 검 컴포넌트에 위임합니다.
            sword.Attack();

            // 공격 애니메이션 트리거는 Attack() 호출 후,
            // 실제 상태가 Attacking으로 바뀌었을 때 (Ready가 아닐 때) 실행하는 것이 더 정확합니다.
            // (혹은 Attack() 메서드 내부에서 직접 애니메이터를 제어하는 것도 좋은 방법입니다.)
            if (sword.state == Sword.State.Attacking)
            {
                playerAnimator.SetTrigger("Attack");
            }
        }
    }

    // C# 메서드: 애니메이터의 IK 갱신
    // OnAnimatorIK는 약속된 메서드 이름입니다.
    private void OnAnimatorIK(int layerIndex)
    {
        // [안정성 강화] 필수 변수가 null인지 먼저 확인
        if (playerAnimator == null || swordPivot == null || leftHandMount == null || rightHandMount == null)
        {
            return;
        }

        // 1. 검의 기준점 (Pivot) 위치를 캐릭터 팔꿈치 힌트 위치에 맞춥니다.
        swordPivot.position = playerAnimator.GetIKHintPosition(AvatarIKHint.RightElbow);

        // 2. 왼손 위치 IK 설정
        // [수정] Position과 Rotation 가중치를 명확히 설정 (원본 코드의 중복 오류 수정)
        playerAnimator.SetIKPositionWeight(AvatarIKGoal.LeftHand, 1.0f);  // 100% 위치 강제 적용
        playerAnimator.SetIKRotationWeight(AvatarIKGoal.LeftHand, 1.0f);  // 100% 회전 강제 적용

        // 손의 최종 위치와 회전을 검 손잡이에 맞춥니다.
        playerAnimator.SetIKPosition(AvatarIKGoal.LeftHand, leftHandMount.position);
        playerAnimator.SetIKRotation(AvatarIKGoal.LeftHand, leftHandMount.rotation);

        // 3. 오른손 위치 IK 설정
        // [수정] Position과 Rotation 가중치를 명확히 설정 (원본 코드의 중복 오류 수정)
        playerAnimator.SetIKPositionWeight(AvatarIKGoal.RightHand, 1.0f); // 100% 위치 강제 적용
        playerAnimator.SetIKRotationWeight(AvatarIKGoal.RightHand, 1.0f); // 100% 회전 강제 적용

        // 손의 최종 위치와 회전을 검 손잡이에 맞춥니다.
        playerAnimator.SetIKPosition(AvatarIKGoal.RightHand, rightHandMount.position);
        playerAnimator.SetIKRotation(AvatarIKGoal.RightHand, rightHandMount.rotation);
    }
}