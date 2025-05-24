using UnityEngine;

public class MoveSpike : MonoBehaviour
{
    private Animator anim;
    private Rigidbody2D rb;
    [SerializeField] float range;
    [SerializeField] float speed;
    [SerializeField] bool isHorizontal;

    private float minRange;
    private float maxRange;

    private bool goingLeft;
    private bool goingDown;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();

        if (isHorizontal) {
          oscillateH();
        } else {
          oscillateV();
        }
    }

    /// <summary>
    /// Sets up object to oscillate horizontally in a set horizontal range. Object starts going left.
    /// </summary>
    private void oscillateH()
    {
        rb.constraints = RigidbodyConstraints2D.FreezePositionY;
        anim.SetBool("on", true);
        minRange = transform.position.x - range;
        maxRange = transform.position.x + range;
        goingLeft = true;
    }

    /// <summary>
    /// Sets up object to oscillate vertically in a set vertical range. Object starts going down.
    /// </summary>
    private void oscillateV()
    {
        rb.constraints = RigidbodyConstraints2D.FreezePositionX;
        anim.SetBool("on", true);
        minRange = transform.position.y - range;
        maxRange = transform.position.y + range;
        goingDown = true;
    }

    /// <summary>
    /// Moves horizontally either left or right, and switches direction once hitting the end of its range.
    /// </summary>
    private void moveH()
    {
        float trapX = transform.position.x;

        if (trapX < minRange){
          goingLeft = false;
        } 
        if (trapX > maxRange) {
          goingLeft = true;
        }

        if (goingLeft)
        {
          rb.linearVelocityX = -speed;
        } else if (!goingLeft) {
          rb.linearVelocityX = speed;
        }
    }

    /// <summary>
    /// Moves vertically either up or down, and switches direction once hitting the end of its range.
    /// </summary>
    private void moveV()
    {
        float trapY = transform.position.y;

        if (trapY < minRange){
          goingDown = false;
        } 
        if (trapY > maxRange) {
          goingDown = true;
        }

        if (goingDown)
        {
          rb.linearVelocityY = -speed;
        } else if (!goingDown) {
          rb.linearVelocityY = speed;
        }
    }

    private void Update()
    {
        if (isHorizontal) {
          moveH();
        } else {
          moveV();
        }
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.tag == "Player") {
            col.GetComponent<PlayerResources>().die();
        }
    }
}
