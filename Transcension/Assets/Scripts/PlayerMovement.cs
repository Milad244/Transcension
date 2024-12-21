using UnityEngine;
using UnityEngine.UIElements;

public class PlayerMovement : MonoBehaviour
{
    public float speed;
    public float jumpPower;
    public float ceilingPushPower;
    public float default_gravity;
    public float wallJumpCD;
    [SerializeField] private Transform spawn1;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private LayerMask climbWallLayer;
    [SerializeField] private LayerMask pushCeilingLayer;
    private Rigidbody2D body;
    private Animator anim;
    private BoxCollider2D boxCollider;
    private float horizontalInput;
    private float wallJumpCDTimer;
    private bool dying;
    Vector3 spawn1Pos;



    private void Awake()
    {
        body = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        boxCollider = GetComponent<BoxCollider2D>();
        body.gravityScale = default_gravity;

        
        spawn1Pos = new Vector3(spawn1.position.x, spawn1.position.y + 2.228477f - 1);
        // Make array for spawns

        transform.localPosition = spawn1Pos; // Can edit the glow for each room
    }

    private void Update()
    {
        if (dying)
            return;

        // horizontal movement
        horizontalInput = Input.GetAxis("Horizontal");

        // flipping character
        if (horizontalInput > 0.01f)
            transform.localScale = Vector3.one;
        else if (horizontalInput < -0.01f)
            transform.localScale = new Vector3(-1, 1, 1);

        // animation params
        anim.SetBool("run", horizontalInput != 0);
        anim.SetBool("grounded", isGrounded());

        // applying movement
        if (wallJumpCDTimer <= 0)
        {
            // horizontal movement
            body.linearVelocity = new Vector2(horizontalInput * speed, body.linearVelocity.y);

            if (onWall() && !isGrounded()) // on wall
            {
                body.gravityScale = 0;
                body.linearVelocity = Vector2.zero;
            }
            else
                body.gravityScale = default_gravity; // not on wall

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
        if (isGrounded()) // regular jump
        {
            body.linearVelocity = new Vector2(body.linearVelocity.x, jumpPower);
            anim.SetTrigger("jump");
        }
        else if (onWall() && !isGrounded()) // wall jump
        {
            if (horizontalInput == 0)
            {
                body.linearVelocity = new Vector2(-Mathf.Sign(transform.localScale.x) * 10, 0);
                transform.localScale = new Vector3(-Mathf.Sign(transform.localScale.x), transform.localScale.y, transform.localScale.z);
            }
            else
                body.linearVelocity = new Vector2(-Mathf.Sign(transform.localScale.x) * 3, 6);

            wallJumpCDTimer = wallJumpCD;
        }
    }

    private void ceilingPush()
    {
        body.linearVelocity = new Vector2(body.linearVelocity.x, -ceilingPushPower);
    }

    private bool isGrounded()
    {
        RaycastHit2D rayCastHit = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0, Vector2.down, 0.1f, groundLayer);
        return rayCastHit.collider != null;
    }


    private bool onWall() // Prob only works if the collider is on the actual climb wall
    {
        RaycastHit2D rayCastHit = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0, new Vector2(transform.localScale.x, 0), 0.1f, climbWallLayer);
        if (rayCastHit.collider == null)
            return false;
            
        GameObject climbWall = rayCastHit.transform.gameObject;
        if (climbWall.transform.rotation.y == 1 && Mathf.Sign(transform.localScale.x) == -1)
            return false;
        else if (climbWall.transform.rotation.y == 0 && Mathf.Sign(transform.localScale.x) == 1)
            return false;

        return true;
    }

    private bool onPushCeiling()
    {
        RaycastHit2D rayCastHit = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0, Vector2.up, 0.1f, pushCeilingLayer);
        return rayCastHit.collider != null;
    }

    public bool canAttack()
    {
        return !onWall() && !dying;
    }

    public void dieMovement()
    {
        dying = true;
        body.linearVelocity = new Vector2(0, 0);
        anim.SetTrigger("die");
    }

    public void reviveMovement()
    {
        transform.localPosition = spawn1Pos;
        anim.SetTrigger("returnIdle");
        dying = false;
    }
}
