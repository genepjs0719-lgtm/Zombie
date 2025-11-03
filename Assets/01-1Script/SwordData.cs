using UnityEngine;

// 파일명: SwordData.cs
// 유니티 에디터 메뉴에 생성 옵션을 추가합니다.
[CreateAssetMenu(menuName = "Scriptable/SwordData", fileName = "Sword Data")]
public class SwordData : ScriptableObject
{
    // [참조]: Sword.cs의 swordAudioPlayer.PlayOneShot(swordData.swingClip)에서 사용
    public AudioClip swingClip;

    // [참조]: Sword.cs의 피해 처리 로직에서 사용 (현재는 미구현)
    public float damage = 25f;

    // [참조]: Sword.cs의 Attack() 메서드 내에서 쿨타임 체크에 사용
    public float timeBetAttack = 0.5f; // 공격을 연속으로 할 수 있는 최소 시간 

    // [참조]: Sword.cs의 AttackRoutine() 코루틴에서 쿨타임 계산에 사용
    public float coolTime = 1.0f; // 공격 후 재사용 대기 시간 
}