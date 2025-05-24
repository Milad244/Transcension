using UnityEngine;

public class Fireball : MonoBehaviour
{
    private BoxCollider2D boxCollider;
    private Animator anim;
    private Vector3 velocity;
    private bool hit;
    private float lifetime;
    private float maxLife = 10f;
   

    private void Awake()
    {
        boxCollider = GetComponent<BoxCollider2D>();
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        if (hit) return;

        // moves the fireball forward based on how fast it's going. Always moves right, direction (rotation) happens in setFireball method.
        transform.Translate(Vector3.right * velocity.magnitude * Time.deltaTime);

        // disabling fireball after X seconds
        lifetime += Time.deltaTime;
        if (lifetime > maxLife)
            gameObject.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Fireball") // do not explode against fireballs
            return;

        hit = true;
        boxCollider.enabled = false;
        anim.SetTrigger("explode"); // causing fireball explode animation
    }

    /// <summary>
    /// Activates fireball with a given velocity and rotates itself appropriatly for that velocity.
    /// </summary>
    /// <param name="velocity">The new velocity of the fireball.</param>
    public void setFireball(Vector3 velocity)
    {
        lifetime = 0;
        gameObject.SetActive(true);
        hit = false;
        boxCollider.enabled = true;

        this.velocity = velocity;

        // points the fireball in the direction it's flying.
        // uses velocity to calculate angle and rotate the fireball to match.
        float angle = Mathf.Atan2(velocity.y, velocity.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }

    private void deactivate() // Called after explode anim is finished
    {
        gameObject.SetActive(false);
    }
}
