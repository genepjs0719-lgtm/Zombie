using UnityEngine;

// 파일명: PlayerInputSword.cs
public class PlayerInputSword : MonoBehaviour
{
    // ====== 1. 인스펙터에서 설정할 입력축/버튼 이름 ======
    public string moveAxisName = "Vertical";
    public string rotateAxisName = "Horizontal";
    public string attackButtonName = "Fire1";
    public string skillButtonName = "Reload";

    // ====== 2. 다른 스크립트에서 읽을 수 있는 입력 값 ======
    public float move { get; private set; }
    public float rotate { get; private set; }
    public bool attack { get; private set; }
    public bool skill { get; private set; }

    // ====== 3. 매 프레임 사용자 입력을 감지 (Update) ======
    private void Update()
    {
        // GameManager가 존재하고 게임오버 상태에서는 사용자 입력을 감지하지 않는다 (가정)
        // if (GameManager.instance != null && GameManager.instance.isGameover) { ... }

        move = Input.GetAxis(moveAxisName);
        rotate = Input.GetAxis(rotateAxisName);

        attack = Input.GetButton(attackButtonName);
        skill = Input.GetButtonDown(skillButtonName);
    }
}