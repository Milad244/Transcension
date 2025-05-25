using System.Collections;
using UnityEngine;

public class Speed : MonoBehaviour
{
    [SerializeField] private float speedBoost;
    [SerializeField] private float speedBoostDuration;
    private Coroutine speedCoroutine;

  private void OnTriggerEnter2D(Collider2D col)
    {
        // Activates speed when player steps on it.
        if (col.tag == "Player")
        {
            if (speedCoroutine != null)
                StopCoroutine(speedCoroutine);

            speedCoroutine = StartCoroutine(activateSpeed(col));
        }
    }

    /// <summary>
    /// Activates a speed boost for the player. The boost starts when the player enters and lasts for a set duration after the player leaves the trigger area.
    /// </summary>
    /// <param name="col">The player's collider.</param>
    private IEnumerator activateSpeed(Collider2D col)
    {
        col.GetComponent<PlayerMovement>().setSpeedBoost(speedBoost);

        Collider2D player = Physics2D.OverlapBox(transform.position, GetComponent<Collider2D>().bounds.size, 0, LayerMask.GetMask("Player"));
        while (player)
        {
            player = Physics2D.OverlapBox(transform.position, GetComponent<Collider2D>().bounds.size, 0, LayerMask.GetMask("Player"));
            yield return null;
        }
        yield return new WaitForSeconds(speedBoostDuration);

        col.GetComponent<PlayerMovement>().setSpeedDefault();
    }
}
