using UnityEngine;

// 유니티 에디터 메뉴에 생성 옵션을 추가합니다.
// [CreateAssetMenu(menuName = "Scriptable/SwordData", fileName = "Sword Data")]
[CreateAssetMenu(menuName = "Scriptable/SwordData", fileName = "Sword Data")]
public class SwordData : ScriptableObject
{
    // [수정] 발사 소리 대신, 검을 '휘두르는' 소리
    public AudioClip swingClip; // 휘두르는 소리

    // [삭제] 재장전 소리는 검에 필요 없으므로 삭제
    // public AudioClip reloadClip; 

    // C# 변수: 검의 공격력 (그대로 유지)
    public float damage = 25f; // 공격력 (float 타입)

    // [삭제] 탄약 관련 변수는 검에 필요 없으므로 삭제
    // public int startAmmoRemain = 100;
    // public int magCapacity = 25; 

    // [수정] 공격 속도와 쿨타임으로 변경
    // timeBetFire (발사 간격) 대신, timeBetAttack (공격 간격)으로 사용
    public float timeBetAttack = 0.5f; // 공격을 연속으로 할 수 있는 최소 시간 (공격 애니메이션 길이)

    // [추가] 공격 후 재사용 대기 시간 (Cool Time)
    // 이 시간 동안은 다시 공격할 수 없습니다.
    public float coolTime = 1.0f; // 공격 후 재사용 대기 시간 (float 타입)

    // [삭제] 재장전 시간은 검에 필요 없으므로 삭제
    // public float reloadTime = 1.8f; 
}