using System.Collections;
using UnityEngine;

public class FireTrap : MonoBehaviour
{
    [Header ("Firetrap Timers")]
    [SerializeField] private float activationDelay;
    [SerializeField] private float activeTime;
    private Animator anim;
    private SpriteRenderer spriteRend;

    private bool triggered;
    private bool active;

    private void Awake()
    {
      anim = GetComponent<Animator>();
      spriteRend = GetComponent<SpriteRenderer>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
      if (collision.tag == "Player") {
          if (!triggered) { // If firetrap not activated, then activates it.
              StartCoroutine(activateFireTrap());
          }

          if (active) // If firetrap is active and player steps on it, then die.
              collision.GetComponent<PlayerResources>().die();
      }
    }

    /// <summary>
    /// Activates the firetrap, causing it to go red. 
    /// After an activation delay, the trap will be animated as active and will kill the player if player is in its collider.
    /// Finally, it will deactivate and return to its normal color after the active time period has passed.
    /// </summary>
    private IEnumerator activateFireTrap()
    {
      triggered = true;
      spriteRend.color = Color.red;

      yield return new WaitForSeconds(activationDelay);
      spriteRend.color = Color.white;
      active = true;
      anim.SetBool("activated", true);

      Collider2D player = Physics2D.OverlapBox(transform.position, GetComponent<Collider2D>().bounds.size, 0, LayerMask.GetMask("Player"));
      if (player)
      {
        player.GetComponent<PlayerResources>().die();
      }

      yield return new WaitForSeconds(activeTime);
      active = false;
      triggered = false;
      anim.SetBool("activated", false);
    }
}
