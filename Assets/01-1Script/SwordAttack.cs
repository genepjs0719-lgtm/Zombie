using UnityEngine;
using System.Collections; // 코루틴 사용을 위해 필요

// IDamageable 인터페이스는 총 스크립트와 동일하게 다른 스크립트에 정의되어 있다고 가정합니다.

public class SwordAttack : MonoBehaviour
{
    // ====== 1. 인스펙터에서 설정할 변수 (총의 fireTransform, gunData에 해당) ======

    [Header("Sword Components")]
    // 검의 끝점 위치 (공격 시작 위치)
    public Transform swordTipTransform;
    // 검의 데이터 (공격력 등)를 담는 스크립터블 오브젝트 (GunData에 해당)
    public SwordData swordData;

    [Header("Effect & Animation")]
    // 검 휘두르기 효과 (총의 muzzleFlashEffect에 해당)
    public ParticleSystem swingEffect;
    // 검 궤적을 그리는 LineRenderer (총의 bulletLineRenderer에 해당)
    public LineRenderer attackLineRenderer;
    // 검 소리 재생을 위한 AudioSource (총의 gunAudioPlayer에 해당)
    public AudioSource swordAudioPlayer;

    [Header("Stats")]
    // 검의 사거리 (총의 fireDistance에 해당)
    public float attackRange = 2f;
    // 검의 현재 내구도 (총의 magAmmo에 해당)
    public int durability = 100;

    // ====== 2. 상태 관리 (총의 State.Empty에 해당) ======
    private enum State { Ready, Broken }
    private State state = State.Ready;

    // ====== 3. 공격 메서드 (총의 Shot() 메서드에 해당) ======

    // 이 메서드는 외부(예: PlayerInput 스크립트)에서 호출됩니다.
    public void Attack()
    {
        if (state == State.Broken)
        {
            Debug.Log("검이 부러져 사용할 수 없습니다.");
            return;
        }

        RaycastHit hit;
        Vector3 hitPosition = Vector3.zero;

        // Raycast를 사용하여 검의 공격 범위(사거리)를 감지합니다.
        // fireTransform.position -> swordTipTransform.position으로 변경
        if (Physics.Raycast(swordTipTransform.position, swordTipTransform.forward, out hit, attackRange))
        {
            // 충돌한 물체에서 IDamageable 컴포넌트를 찾습니다.
            IDamageable target = hit.collider.GetComponent<IDamageable>();

            if (target != null)
            {
                // 충돌한 타겟에게 피해를 줍니다.
                // gunData.damage -> swordData.damage로 변경
                target.OnDamage(swordData.damage, hit.point, hit.normal);
            }

            // 검이 맞은 위치를 저장합니다. (총알이 맞은 위치와 동일)
            hitPosition = hit.point;
        }
        else
        {
            // 아무것도 맞지 않았을 경우, 검의 최대 사거리 끝 지점을 hitPosition으로 설정합니다.
            hitPosition = swordTipTransform.position + swordTipTransform.forward * attackRange;
        }

        // 공격 효과 코루틴을 실행합니다. (ShotEffect -> AttackEffect로 변경)
        StartCoroutine(AttackEffect(hitPosition));

        // 내구도를 감소시킵니다. (magAmmo -> durability로 변경)
        durability--;

        // 내구도가 0 이하가 되면 검이 부러진 상태로 변경합니다.
        if (durability <= 0)
        {
            state = State.Broken;
            Debug.Log("검이 부러졌습니다!");
        }
    }

    // ====== 4. 공격 효과 코루틴 (총의 ShotEffect 코루틴에 해당) ======

    // 검을 휘두르는 시각/청각 효과를 처리합니다.
    private IEnumerator AttackEffect(Vector3 hitPosition)
    {
        // 1. 시각/청각 효과 재생
        // 총구 화염 -> 검 휘두르는 효과 재생
        swingEffect.Play();

        // 탄피 배출 이펙트 제거 (검에는 탄피가 없으므로)

        // 총 소리 -> 검 휘두르는 소리 재생 (OneShot은 짧은 효과음에 유용)
        swordAudioPlayer.PlayOneShot(swordData.swingClip);

        // 2. 궤적 설정 및 표시
        attackLineRenderer.SetPosition(0, swordTipTransform.position); // 검 끝에서 시작
        attackLineRenderer.SetPosition(1, hitPosition); // 맞은 지점 또는 사거리 끝

        // 라인 렌더러를 활성화하여 검의 궤적(선)을 그림
        attackLineRenderer.enabled = true;

        // 3. 잠시 대기 (궤적을 짧게 보여주기 위함)
        // 0.1초로 늘려 검의 휘두르는 시간을 표현할 수 있습니다.
        yield return new WaitForSeconds(0.1f);

        // 4. 궤적을 지움
        attackLineRenderer.enabled = false;
    }
}