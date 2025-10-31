using System.Collections;
using UnityEngine;
using UnityEngine.AI; // (예시로 사용하지 않을 수 있지만, using은 위에 모아둡니다.)

// [핵심 수정 1] MonoBehaviour 상속: 게임 오브젝트에 컴포넌트로 붙일 수 있게 함
public class Sword : MonoBehaviour
{
    // [핵심 수정 2] 검의 상태 정의: 준비됨, 공격 중, 대기 중
    public enum State
    {
        Ready,     // 공격 준비 완료
        Attacking, // 공격 중 (휘두르는 중)
        Cooling    // 재사용 대기 중 (쿨타임)
    }

    // C# 변수: 검의 상태 및 데이터
    public State state { get; private set; } // 현재 검의 상태

    public Transform attackPivot; // 공격의 시작점 또는 검의 중심

    // [수정] 총의 이펙트 대신 검의 휘두르기/피격 이펙트로 변경
    // public ParticleSystem swingEffect; // 검을 휘두를 때의 이펙트 (옵션)
    // public ParticleSystem hitEffect;  // 적을 때렸을 때의 피격 이펙트 (옵션)

    private AudioSource swordAudioPlayer; // 검 소리 재생기 (휘두르는 소리 등)

    public SwordData swordData; // 검의 공격력, 쿨타임 등의 데이터 (ScriptableObject)

    // private float attackRange = 2f; // 공격 사정거리 (충돌 처리 방식으로 구현 시 필요)

    private float lastAttackTime; // 검을 마지막으로 휘두른 시점

    // C# 메서드: 컴포넌트 초기화
    private void Awake()
    {
        swordAudioPlayer = GetComponent<AudioSource>();
        // [삭제] bulletLineRenderer 관련 코드는 검에 필요 없으므로 삭제
    }

    // C# 메서드: 컴포넌트 활성화 시 초기화
    private void OnEnable()
    {
        state = State.Ready;
        lastAttackTime = 0;
        // [삭제] 탄약 초기화 코드는 검에 필요 없으므로 삭제
    }

    // 공격 시도 (Fire 대신 Attack으로 이름 변경)
    public void Attack()
    {
        // [수정] 공격 가능 조건: 준비 상태이고 쿨타임이 지났는지 확인
        if (state == State.Ready && Time.time >= lastAttackTime + swordData.timeBetAttack)
        {
            lastAttackTime = Time.time;
            Swing(); // 실제 휘두르기 동작 시작
        }
    }

    // 실제 휘두르기 동작 처리 (Shot 대신 Swing으로 이름 변경)
    private void Swing()
    {
        // 1. 상태를 Attacking으로 전환
        state = State.Attacking;

        // [수정] Raycast 대신, 공격 동작을 처리하고 충돌을 확인하는 로직 시작
        // (실제 게임에서는 애니메이션과 콜라이더를 사용하거나, Physics.OverlapSphere 등을 사용)

        // 예시: 간단한 충돌 감지 로직 (SphereCast 또는 OverlapSphere 사용 가능)
        // Physics.OverlapSphere(attackPivot.position, attackRange) 등으로 범위 내 적을 찾음

        // 2. 공격 이펙트와 소리 재생
        swordAudioPlayer.PlayOneShot(swordData.swingClip); // 휘두르는 소리
        // swingEffect.Play(); // 휘두르기 이펙트 재생

        // [수정] 코루틴을 사용하여 공격 시간 동안 대기 및 충돌 처리
        StartCoroutine(AttackRoutine());

        // [삭제] 탄약 감소 코드는 검에 필요 없으므로 삭제
    }


    // 공격 동작과 쿨타임 처리를 진행
    private IEnumerator AttackRoutine()
    {
        // 공격 애니메이션이 끝날 때까지 대기하는 시간 (예: 0.5초)
        float attackDuration = 0.5f;

        // 공격하는 동안 잠시 대기
        yield return new WaitForSeconds(attackDuration);

        // 1. 공격이 끝났으므로 상태를 Cooling (쿨타임)으로 전환
        state = State.Cooling;

        // 2. 쿨타임 소요 시간만큼 처리 쉬기
        yield return new WaitForSeconds(swordData.coolTime - attackDuration);

        // 3. 쿨타임이 끝나면 발사 준비된 상태로 변경
        state = State.Ready;
    }

    // [핵심 삭제 3] 재장전 관련 코드는 모두 삭제 (검에 필요 없음)
    // public bool Reload() { ... }
    // private IEnumerator ReloadRoutine() { ... }
}