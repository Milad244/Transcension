using System.Collections;
using UnityEngine;

public class Plate : MonoBehaviour
{
    [SerializeField] private GameObject objToEnable;
    [SerializeField] private float enableTime;
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.tag == "Player") {
            StopCoroutine(activateObj());
            StartCoroutine(activateObj());
        }
    }

    private IEnumerator activateObj()
    {
        objToEnable.SetActive(true);
        Collider2D player = Physics2D.OverlapBox(transform.position, GetComponent<Collider2D>().bounds.size, 0, LayerMask.GetMask("Player"));
        while (player) {
            player = Physics2D.OverlapBox(transform.position, GetComponent<Collider2D>().bounds.size, 0, LayerMask.GetMask("Player"));
            yield return new WaitForSeconds(0.1f); //so game doesn't crash :)
        }
        yield return new WaitForSeconds(enableTime);
        objToEnable.SetActive(false);
    }
}
