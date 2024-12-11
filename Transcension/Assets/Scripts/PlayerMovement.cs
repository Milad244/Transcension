using System.Numerics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;

public class PlayerMovement : MonoBehaviour
{
    public float speed;
    public float jumpPower;
    public float ceilingPushPower;
    public float default_gravity;
    public float wallJumpCD;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private LayerMask climbWallLayer;
    [SerializeField] private LayerMask pushCeilingLayer;
    private Rigidbody2D body;
    private Animator anim;
    private BoxCollider2D boxCollider;
    private float horizontalInput;
    private float wallJumpCDTimer;



    private void Awake()
    {
        body = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        boxCollider = GetComponent<BoxCollider2D>();
        body.gravityScale = default_gravity;
    }

    private void Update()
    {
        // horizontal movement
        horizontalInput = Input.GetAxis("Horizontal");

        // flipping character
        if (horizontalInput > 0.01f)
            transform.localScale = UnityEngine.Vector3.one;
        else if (horizontalInput < -0.01f)
            transform.localScale = new UnityEngine.Vector3(-1, 1, 1);

        // animation params
        anim.SetBool("run", horizontalInput != 0);
        anim.SetBool("grounded", isGrounded());

        // applying movement
        if (wallJumpCDTimer <= 0)
        {
            // horizontal movement
            body.linearVelocity = new UnityEngine.Vector2(horizontalInput * speed, body.linearVelocity.y);

            // jump movement
            if (onWall() && !isGrounded())
            {
                body.gravityScale = 0;
                body.linearVelocity = UnityEngine.Vector2.zero;
            }
            else
                body.gravityScale = default_gravity;

            if(onPushCeiling())
                ceilingPush();

            if (Input.GetKey(KeyCode.Space))
                Jump();
        }
        else
            wallJumpCDTimer -= Time.deltaTime;
    }

    private void Jump()
    {
        if (isGrounded())
        {
            body.linearVelocity = new UnityEngine.Vector2(body.linearVelocity.x, jumpPower);
            anim.SetTrigger("jump");
        }
        else if (onWall() && !isGrounded())
        {
            if (horizontalInput == 0)
            {
                body.linearVelocity = new UnityEngine.Vector2(-Mathf.Sign(transform.localScale.x) * 10, 0);
                transform.localScale = new UnityEngine.Vector3(-Mathf.Sign(transform.localScale.x), transform.localScale.y, transform.localScale.z);
            }
            else
                body.linearVelocity = new UnityEngine.Vector2(-Mathf.Sign(transform.localScale.x) * 3, 6);

            wallJumpCDTimer = wallJumpCD;
        }
    }

    private void ceilingPush()
    {
        body.linearVelocity = new UnityEngine.Vector2(body.linearVelocity.x, -ceilingPushPower);
    }

    private bool isGrounded()
    {
        RaycastHit2D rayCastHit = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0, UnityEngine.Vector2.down, 0.1f, groundLayer);
        return rayCastHit.collider != null;
    }

    private bool onWall()
    {
        RaycastHit2D rayCastHit = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0, new UnityEngine.Vector2(transform.localScale.x, 0), 0.1f, climbWallLayer);
        return rayCastHit.collider != null;
    }

    private bool onPushCeiling()
    {
        RaycastHit2D rayCastHit = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0, UnityEngine.Vector2.up, 0.1f, pushCeilingLayer);
        return rayCastHit.collider != null;
    }

    public bool canAttack()
    {
        return !onWall();
    }
}
