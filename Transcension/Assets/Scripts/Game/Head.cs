using UnityEngine;

public class Head : MonoBehaviour
{
    private bool activated;
    private Vector3 restPos;
    private Animator anim;
    private void Awake()
    {
        anim = GetComponent<Animator>();
        restPos = transform.localPosition;
        createDetect();
    }

    private void createDetect()
    {
        // Create a trigger collider
    }

    private void deactivate()
    {
        transform.localPosition = restPos;
        activated = false;
        anim.SetTrigger("deactivate");
    }

    public void activate()
    {
        if (!activated)
        {
            activated = true;
            anim.SetTrigger("activate");
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            // Get direction for head killing animation
            collision.GetComponent<PlayerResources>().die();
        }
    }
}
