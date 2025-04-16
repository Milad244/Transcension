using Unity.VisualScripting;
using UnityEngine;

public class HeadDetect : MonoBehaviour
{

    private void activateHead()
    {
        transform.parent.GameObject().GetComponent<Head>().activate();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            activateHead();
        }
    }
}
