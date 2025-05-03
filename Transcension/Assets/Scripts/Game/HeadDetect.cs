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

    public void activateParticles(bool b)
    {
        if (b)
        {
            particles.Play();
            particles.Emit(50);
        } else {
            particles.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
        }
    }

    public void activateHead()
    {
        activateParticles(false);
        getParentHead().activate(this);
    }

    public Head getParentHead() {
        return transform.parent.GameObject().GetComponent<Head>();
    }
}
