using UnityEngine;

public class Fireball : MonoBehaviour
{
    [SerializeField] private float speed;
    private float direction;
    private bool hit;
    private float lifetime;

    private BoxCollider2D boxCollider;
    private Animator anim;

    private void Awake()
    {
        boxCollider = GetComponent<BoxCollider2D>();
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        if (hit) return;
        float movementSpeed = speed * Time.deltaTime * direction;
        transform.Translate(movementSpeed, 0, 0);

        lifetime += Time.deltaTime;
        if (lifetime > 3)gameObject.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision) // make fireballs not 
    {
        if (collision.tag == "Player" || collision.tag == "Fireball")
            return;

        hit = true;
        boxCollider.enabled = false;
        anim.SetTrigger("explode");
    }

    public void setDirection(float direction)
    {
        lifetime = 0;
        this.direction = direction;
        gameObject.SetActive(true);
        hit = false;
        boxCollider.enabled = true;

        float localScaleX = transform.localScale.x;
        if (Mathf.Sign(localScaleX) != direction)
            localScaleX = -localScaleX;

        transform.localScale = new Vector3(localScaleX, transform.localScale.y, transform.localScale.z);
    }

    private void deactivate()
    {
        gameObject.SetActive(false);
    }
}
