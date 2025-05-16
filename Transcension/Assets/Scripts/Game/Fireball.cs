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

        // fireball flying
        transform.Translate(Vector3.right * velocity.magnitude * Time.deltaTime);

        // ending fireball after X seconds
        lifetime += Time.deltaTime;
        if (lifetime > maxLife)gameObject.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Fireball")
            return;

        hit = true;
        boxCollider.enabled = false;
        anim.SetTrigger("explode");
    }

    public void setFireball(Vector3 velocity)
    {
        lifetime = 0;
        gameObject.SetActive(true);
        hit = false;
        boxCollider.enabled = true;

        this.velocity = velocity;

        float angle = Mathf.Atan2(velocity.y, velocity.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }

    private void deactivate() // Called after explode anim is finished
    {
        gameObject.SetActive(false);
    }
}
