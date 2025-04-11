using UnityEngine;

public class MoveSpike : MonoBehaviour
{
    private Animator anim;
    private Rigidbody2D rb;
    [SerializeField] int range;
    [SerializeField] int speed;

    private float minRange;
    private float maxRange;

    private bool goingLeft;

    private void Awake()
  {
    anim = GetComponent<Animator>();
    rb = GetComponent<Rigidbody2D>();

    oscillate();
  }

  private void oscillate()
  {
    anim.SetBool("on", true);
    minRange = transform.position.x - range;
    maxRange = transform.position.x + range;
    goingLeft = true;
  }

  private void Update()
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

  private void OnTriggerEnter2D(Collider2D col)
  {
    if (col.tag == "Player") {
        col.GetComponent<PlayerResources>().die();
    }
  }
}
