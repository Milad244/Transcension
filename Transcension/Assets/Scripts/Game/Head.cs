using System.Collections;
using UnityEngine;

public class Head : MonoBehaviour
{
    private bool activated;
    private Vector3 oriPos;
    private Animator anim;
    private HeadDetect headDetect;
    [SerializeField] private float speed;
    private Coroutine deactivateRoutine;
    private PlayerMovement playerMovement;
    private void Awake()
    {
        anim = GetComponent<Animator>();
        oriPos = transform.position;
        playerMovement = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>();
    }

    /// <summary>
    /// Deactivates the head trap actively chasing the player and returns it to its sleeping state.
    /// </summary>
    private IEnumerator deactivate()
    {
        activated = false;
        yield return new WaitForSeconds(0.5f);// delay after killing you 
        // Travelling back to its original position until close enough to snap back to its original position exactly.
        while (Vector3.Distance(transform.position, oriPos) > 1f)
        {
            Vector3 direction = oriPos - transform.position;
            direction.Normalize();
            Vector3 v = direction * speed * Time.deltaTime;
            transform.position += v;

            yield return null;
        }

        transform.position = oriPos;

        // Going back to deactivated state.
        anim.SetTrigger("deactivate");
        if (headDetect)
        {
            headDetect.activateParticles(true);
        }
        else
        {
            Debug.LogError("HeadDetect script not found");
        }

        activated = false;
        deactivateRoutine = null;
    }

    /// <summary>
    /// Starts the deactivation coroutine for the Head trap unless its currently deactivating.
    /// </summary>
    public void startDeactivate() //called from playerResources when player dies/revives while head trap is activated
    {
        if (deactivateRoutine == null)
        {
            deactivateRoutine = StartCoroutine(deactivate());
        }
    }

    /// <summary>
    /// When head trap is not activated, not being deactivated, and the player is not dying, 
    /// starts the head trap activate animation which then activates the head trap at the end of its animation. 
    /// </summary>
    /// <param name="script">The HeadDetect script of the child HeadDetect of the head trap.</param>
    public void activate(HeadDetect script)
    {
        if (activated || deactivateRoutine != null || playerMovement.dying) return;

        headDetect = script;
        anim.SetTrigger("activate"); //onActivateFinished() called from the triggered animation
    }

    public void onActivateFinished()
    {
        activated = true;
    }

    private void Update()
    {
        if (activated) // runs toward the player
        {
            Vector3 playPos = GameObject.FindGameObjectWithTag("Player").transform.position;
            // Vector3 direction = playPos - transform.position;
            // direction.Normalize();
            // Vector3 v = direction * speed * Time.deltaTime;
            // transform.position += v;
            transform.position = Vector3.MoveTowards(transform.position, playPos, speed * Time.deltaTime); // Replaces above to avoid it tweaking when very close to player
        }
    }
}
