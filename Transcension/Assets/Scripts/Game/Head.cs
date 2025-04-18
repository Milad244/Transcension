using Unity.VisualScripting;
using UnityEngine;

public class Head : MonoBehaviour
{
    private bool activated;
    private Vector3 oriPos;
    private Animator anim;
    private HeadDetect headDetect;
    [SerializeField] private float speed;
    private void Awake()
    {
        anim = GetComponent<Animator>();
        oriPos = transform.localPosition;
    }

    private void deactivate()
    {
        transform.localPosition = oriPos;
        activated = false;
        anim.SetTrigger("deactivate");
        if (headDetect)
        {
            headDetect.activateParticles(true);
        } else {
            Debug.LogError("HeadDetect script not found");
        }
    }

    public void activate(HeadDetect script)
    {
        headDetect = script;
        if (!activated)
        {
            activated = true;
            anim.SetTrigger("activate");
        }
    }

    private void Update()
    {
        if (activated)
        {
            Vector3 playPos = GameObject.FindGameObjectWithTag("Player").transform.position;
            Debug.Log(playPos);
            Vector3 direction = playPos - transform.position;
            direction.Normalize();
            Vector3 v = direction * speed * Time.deltaTime;
            transform.position += v;
        }
    }

  void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            // Get direction for head killing animation
            collision.GetComponent<PlayerResources>().die();
            deactivate();
        }
    }
}
