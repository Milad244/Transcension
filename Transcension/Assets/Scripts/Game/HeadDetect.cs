using Unity.VisualScripting;
using UnityEngine;

public class HeadDetect : MonoBehaviour
{
    ParticleSystem particles;

    private void Awake()
    {
       particles = GetComponent<ParticleSystem>();
       activateParticles(true);
    }

    /// <summary>
    /// Plays the particles with an initial emission if true, stops the particles if false.
    /// </summary>
    /// <param name="b">True to activate particles, false to deactivate particles.</param>
    public void activateParticles(bool b)
    {
        if (b)
        {
            particles.Play();
            particles.Emit(50);
        }
        else
        {
            particles.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
        }
    }

    /// <summary>
    /// Deactivates the particles and activates the parent head trap. 
    /// </summary>
    public void activateHead()
    {
        activateParticles(false);
        getParentHead().activate(this);
    }

    /// <summary>
    /// Gets the parent head trap Head script.
    /// </summary>
    /// <returns>The parent head trap Head script.</returns>
    public Head getParentHead()
    {
        return transform.parent.GameObject().GetComponent<Head>();
    }
}
