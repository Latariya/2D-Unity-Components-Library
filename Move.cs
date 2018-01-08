using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]

public class Move : MonoBehaviour
{
    [SerializeField] private MoveAxis moveAxis;

    [SerializeField] private float moveSpeed;
    [SerializeField] private float jumpValue;

    [SerializeField] private bool multipleJumps;
    [SerializeField] private int multipleJumpsCount;

    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private Transform groundCheckers;

    private bool onGround;
    private Rigidbody2D rb;

    private float initialGravity;
    private int jumpCounter;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        initialGravity = rb.gravityScale;
    }

    private void FixedUpdate()
    {
        MoveProcess();
        JumpProcess();
    }

    private void MoveProcess()
    {
        Vector2 moveVector = GetMoveVector(moveAxis);
        rb.velocity = moveVector;
    }

    private Vector2 GetMoveVector(MoveAxis axis)
    {
        Vector2 resultInputVector = Vector2.zero;
        float yInput = Input.GetAxisRaw("Vertical");
        float xInput = Input.GetAxisRaw("Horizontal");

        switch (axis)
        {
            case MoveAxis.Horizontal:
                CheckGround();
                rb.gravityScale = initialGravity;
                resultInputVector = new Vector2(xInput * moveSpeed, rb.velocity.y);
                break;
            case MoveAxis.Free:
                onGround = false;
                rb.gravityScale = 0f;
                resultInputVector = new Vector2(xInput, yInput) * moveSpeed;
                break;
            default:
                break;
        }

        return resultInputVector;
    }

    private void CheckGround()
    {
        foreach (Transform item in groundCheckers)
        {
            onGround = Physics2D.OverlapCircle(item.position, .03f, groundLayer);
            if (onGround) return;
        }
    }

    private void JumpProcess()
    {
        bool isSpacePressed = Input.GetKeyDown(KeyCode.Space);
        bool canJump = isSpacePressed && onGround;
        bool canMultipleJumps = multipleJumps && jumpCounter < multipleJumpsCount && !onGround && isSpacePressed;
        Vector2 jumpVector = GetJumpVector();

        if (canJump || canMultipleJumps)
        {
            jumpCounter++;
            rb.velocity = Vector2.right * rb.velocity.x;
            rb.AddForce(jumpVector);
        }   
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground")) jumpCounter = 0;
    }

    private Vector2 GetJumpVector ()
    {
        Vector2 jumpVector = Vector2.up * jumpValue;
        return jumpVector;
    }

    public enum MoveAxis
    {
        Horizontal,
        Free
    }
}
