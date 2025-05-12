using System.Collections;
using UnityEngine;

public class Speed : MonoBehaviour
{
    [SerializeField] private float speedBoost;
    [SerializeField] private float speedBoostDuration;
    private Coroutine speedCoroutine;

  private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.tag == "Player") {
            if (speedCoroutine != null)
                StopCoroutine(speedCoroutine);

            speedCoroutine = StartCoroutine(activateSpeed(col));
        }
    }

    private IEnumerator activateSpeed(Collider2D col)
    {
        col.GetComponent<PlayerMovement>().setSpeedBoost(speedBoost);

        Collider2D player = Physics2D.OverlapBox(transform.position, GetComponent<Collider2D>().bounds.size, 0, LayerMask.GetMask("Player"));
        while (player) {
            player = Physics2D.OverlapBox(transform.position, GetComponent<Collider2D>().bounds.size, 0, LayerMask.GetMask("Player"));
            yield return new WaitForSeconds(0.01f); //so game doesn't crash :)
        }
        yield return new WaitForSeconds(speedBoostDuration);

        col.GetComponent<PlayerMovement>().setSpeedDefault();
    }
}
