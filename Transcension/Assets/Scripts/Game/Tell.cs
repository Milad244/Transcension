using UnityEngine;

public class Tell : MonoBehaviour
{
    [SerializeField] private Transform objToTellTo;

    private void OnTriggerEnter2D(Collider2D col)
        {
            // When player collides with the teleporter, teleport player to the set object.
            if (col.tag == "Player")
            {
                col.gameObject.GetComponent<PlayerMovement>().tellTo(objToTellTo);
            }
        }
}
