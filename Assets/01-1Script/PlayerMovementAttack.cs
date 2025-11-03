using UnityEngine;

// 파일명: PlayerMovementAttack.cs
// PlayerInputSword 타입에 의존 (에러 방지 핵심 수정)
public class PlayerMovementAttack : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float rotateSpeed = 180f;

    // [중요]: PlayerInputSword 타입을 정확히 참조
    private PlayerInputSword playerInput;
    private Rigidbody playerRigidbody;
    private Animator playerAnimator;

    private void Start()
    {
        // [중요]: GetComponent<PlayerInputSword>()를 정확히 호출
        playerInput = GetComponent<PlayerInputSword>();
        playerRigidbody = GetComponent<Rigidbody>();
        playerAnimator = GetComponent<Animator>();

        if (playerInput == null) Debug.LogError("PlayerInputSword 컴포넌트 누락");
    }

    private void Update()
    {
        playerAnimator.SetFloat("Move", playerInput.move);

        if (playerInput.attack)
        {
            playerAnimator.SetTrigger("Attack");
        }
    }

    private void FixedUpdate()
    {
        Rotate();
        Move();
    }

    private void Move()
    {
        Vector3 moveDistance =
             playerInput.move * transform.forward * moveSpeed * Time.deltaTime;
        playerRigidbody.MovePosition(playerRigidbody.position + moveDistance);
    }

    private void Rotate()
    {
        float turn = playerInput.rotate * rotateSpeed * Time.deltaTime;
        playerRigidbody.rotation = playerRigidbody.rotation * Quaternion.Euler(0, turn, 0f);
    }
}