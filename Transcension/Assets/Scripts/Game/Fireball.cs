using UnityEngine;

public class Fireball : MonoBehaviour
{
    [SerializeField] private float speed;
    private BoxCollider2D boxCollider;
    private Animator anim;
    private float direction;
    private bool hit;
    private float lifetime;
    private float charge;
   

    private void Awake()
    {
        boxCollider = GetComponent<BoxCollider2D>();
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        if (hit) return;

        // fireball flying
        float movementSpeed = speed * Time.deltaTime * direction;
        transform.Translate(movementSpeed, 0, 0);

        // ending fireball after 3 sec
        lifetime += Time.deltaTime;
        if (lifetime > 3)gameObject.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player" || collision.tag == "Fireball" || collision.tag == "Dialogue")
            return;

        hit = true;
        boxCollider.enabled = false;
        anim.SetTrigger("explode");
    }

    public void setFireball(float direction, float charge)
    {
        lifetime = 0;
        gameObject.SetActive(true);
        hit = false;
        boxCollider.enabled = true;

        this.direction = direction;

        if (charge > 1)
            this.charge = 1;
        else if (charge < 0.5f)
            this.charge = 0.5f;
        else
            this.charge = charge;

        float absCharge = this.charge;
        if (Mathf.Sign(this.charge) != direction)
            this.charge = -this.charge;

        transform.localScale = new Vector3(this.charge, absCharge, absCharge);
    }

    private void deactivate() // Called after explode anim is finished
    {
        gameObject.SetActive(false);
    }
}
