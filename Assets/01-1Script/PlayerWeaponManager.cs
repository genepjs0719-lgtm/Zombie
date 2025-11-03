using UnityEngine;

// 파일명: PlayerWeaponManager.cs
public class PlayerWeaponManager : MonoBehaviour
{
    // 인스펙터에 연결할 모든 무기 스크립트 (총 스크립트는 삭제하지 않고 비활성화 상태로 둡니다.)
    public MonoBehaviour currentWeapon; // 현재 활성화된 무기
    public Sword swordWeapon;          // 검 스크립트 컴포넌트 (Sword.cs)

    // 총 스크립트 (예시: GunShooter.cs)도 여기에 연결할 수 있습니다.
    // public MonoBehaviour gunWeapon; 

    // 무기 교체 입력을 받기 위해 PlayerInputSword 참조
    private PlayerInputSword playerInput;

    private void Start()
    {
        playerInput = GetComponent<PlayerInputSword>();

        // 1. 초기 무기를 검으로 설정 (총 스크립트는 비활성화 상태라고 가정)
        SwitchToSword();
    }

    private void Update()
    {
        // 2. 무기 교체 입력 감지 (예: Q 키)
        if (Input.GetKeyDown(KeyCode.Q))
        {
            // 현재는 검만 사용하므로, 검 활성화 코드를 다시 호출하거나 다른 무기 로직을 추가
            // (만약 총이 있다면, 총 <-> 검 전환 로직이 들어갑니다.)
            SwitchToSword(); // 검만 있는 경우
        }
    }

    // 검으로 무기를 교체하는 메서드
    public void SwitchToSword()
    {
        // 1. 모든 무기를 비활성화 (총이 있다면 총을 비활성화)
        // if (gunWeapon != null) gunWeapon.enabled = false;

        // 2. 검 무기를 활성화
        if (swordWeapon != null)
        {
            swordWeapon.enabled = true;
            currentWeapon = swordWeapon;
            Debug.Log("무기가 검으로 교체되었습니다.");
        }
    }

    // 이 스크립트를 통해 PlayerMovementAttack이 Attack()을 호출하도록 할 수 있습니다.
}