using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D body;
    private Animator anim;
    private BoxCollider2D boxCollider;

    [SerializeField] private GameObject cameraHolder;
    private CameraController cameraController;
    [SerializeField] private Transform spawn1;
    [SerializeField] private Transform spawn2;
    [SerializeField] private Transform transcend1;
    private Dictionary<GameObject, Transform> transcendSpawnMap;
    private Dictionary<GameObject, float> transcendFloorLimitMap;
    
    public float speed;
    public float jumpPower;
    public float ceilingPushPower;
    public float default_gravity;
    public float wallJumpCD;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private LayerMask climbWallLayer;
    [SerializeField] private LayerMask pushCeilingLayer;

    private float horizontalInput;
    private float wallJumpCDTimer;
    private bool dying;
    private Vector3 revivePos;



    private void Awake()
    {
        body = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        boxCollider = GetComponent<BoxCollider2D>();
        body.gravityScale = default_gravity;

        cameraController = cameraHolder.GetComponent<CameraController>();

        transcendSpawnMap = new Dictionary<GameObject, Transform>
        {
            { transcend1.gameObject, spawn2 }
        };

        transcendFloorLimitMap = new Dictionary<GameObject, float>
        {
            { transcend1.gameObject, 50.4f}
        };

        revivePos = AdjustSpawnPosition(spawn1.position);
        transform.localPosition = revivePos;
        cameraController.changeFloorLimit(3);
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
        transform.localPosition = revivePos;
        anim.SetTrigger("returnIdle");
        dying = false;
    }

    private Vector3 AdjustSpawnPosition(Vector3 originalPosition)
    {
        return new Vector3(originalPosition.x, originalPosition.y + 2.228477f - 1, originalPosition.z);
    }

    public void transcend(GameObject transcendLevel)
    {
        if (transcendSpawnMap.TryGetValue(transcendLevel, out Transform spawnTransform))
        {
            revivePos = AdjustSpawnPosition(spawnTransform.position);
            transform.localPosition = revivePos;

            if (transcendFloorLimitMap.TryGetValue(transcendLevel, out float floorLimit))
            {
                cameraController.changeFloorLimit(floorLimit);
            } else
            {
                Debug.LogWarning("Transcend level not mapped to a floor limit.");
            }
        }
        else
        {
            Debug.LogWarning("Transcend level not mapped to a spawn point.");
        }
    }
}
