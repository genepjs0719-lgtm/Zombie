using System.Collections;
using UnityEngine;
using UnityEngine.AI;

// 파일명: Sword.cs
public class Sword : MonoBehaviour
{
    public enum State
    {
        Ready,      // 공격 준비 완료
        Attacking,  // 공격 중 (휘두르는 중)
        Cooling     // 재사용 대기 중 (쿨타임)
    }

    // C# 변수: 검의 상태 및 데이터
    public State state { get; private set; }

    public Transform attackPivot; // 공격의 시작점 
    private AudioSource swordAudioPlayer;

    public SwordData swordData; // 검의 공격력, 쿨타임 등의 데이터 (ScriptableObject)

    private float lastAttackTime;

    // C# 메서드: 컴포넌트 초기화
    private void Awake()
    {
        swordAudioPlayer = GetComponent<AudioSource>();
    }

    // C# 메서드: 컴포넌트 활성화 시 초기화
    private void OnEnable()
    {
        state = State.Ready;
        lastAttackTime = 0;
    }

    // 공격 시도 (PlayerMovementAttack에서 호출됨)
    public void Attack()
    {
        // 공격 가능 조건: 준비 상태이고 쿨타임이 지났는지 확인
        if (state == State.Ready && Time.time >= lastAttackTime + swordData.timeBetAttack)
        {
            lastAttackTime = Time.time;
            Swing(); // 실제 휘두르기 동작 시작
        }
    }

    // 실제 휘두르기 동작 처리
    private void Swing()
    {
        state = State.Attacking;

        // 1. 공격 이펙트와 소리 재생
        swordAudioPlayer.PlayOneShot(swordData.swingClip);
        // swingEffect.Play(); // 휘두르기 이펙트 재생 (옵션)

        // 2. [추가 필요]: 여기에 충돌 감지 로직 (OverlapSphere 등)이 들어갑니다.

        // 3. 코루틴을 사용하여 공격 시간 동안 대기 및 쿨타임 처리
        StartCoroutine(AttackRoutine());
    }

    // 공격 동작과 쿨타임 처리를 진행
    private IEnumerator AttackRoutine()
    {
        float attackDuration = 0.5f; // 공격 애니메이션 시간과 일치시키는 것이 좋음

        // 공격하는 동안 잠시 대기
        yield return new WaitForSeconds(attackDuration);

        // 1. 공격이 끝났으므로 상태를 Cooling (쿨타임)으로 전환
        state = State.Cooling;

        // 2. 쿨타임 소요 시간만큼 처리 쉬기
        yield return new WaitForSeconds(swordData.coolTime - attackDuration);

        // 3. 쿨타임이 끝나면 발사 준비된 상태로 변경
        state = State.Ready;
    }
}