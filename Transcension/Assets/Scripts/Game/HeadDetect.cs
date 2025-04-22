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

    private void activateHead()
    {
        activateParticles(false);
        transform.parent.GameObject().GetComponent<Head>().activate(this);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            activateHead();
        }
    }
}
