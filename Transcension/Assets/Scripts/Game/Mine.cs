using UnityEngine;

public class Mine : MonoBehaviour
{
    [SerializeField] private float lifeSpan;
    private float lifeTime;

    private void Update()
    {
        lifeTime += Time.deltaTime;

        if (lifeTime >= lifeSpan)
            Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            PlayerResources playerResources = col.gameObject.GetComponent<PlayerResources>();

            if (playerResources != null)
            {
                playerResources.die();
                Destroy(gameObject);
            }
            else
            {
                Debug.LogWarning("PlayerResources component not found on the GameObject.");
            }
        }
    }
}
