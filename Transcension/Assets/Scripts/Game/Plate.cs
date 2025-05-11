using System;
using System.Collections;
using UnityEngine;

public class Plate : MonoBehaviour
{
    [SerializeField] private bool enable;
    [SerializeField] private GameObject objToChange;
    [SerializeField] private float enableTime;

    private Coroutine plateCoroutine;
    private Vector3 oriPlatePos;

  private void Awake()
  {
        oriPlatePos = transform.localPosition;
  }
  private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.tag == "Player") {
            if (plateCoroutine != null)
                StopCoroutine(plateCoroutine);

            plateCoroutine = StartCoroutine(activateObj());
        }
    }

    private IEnumerator activateObj()
    {
        updateState(enable);
        Collider2D player = Physics2D.OverlapBox(transform.position, GetComponent<Collider2D>().bounds.size, 0, LayerMask.GetMask("Player"));
        while (player) {
            player = Physics2D.OverlapBox(transform.position, GetComponent<Collider2D>().bounds.size, 0, LayerMask.GetMask("Player"));
            yield return new WaitForSeconds(0.01f); //so game doesn't crash :)
        }
        yield return new WaitForSeconds(enableTime);
        updateState(!enable);
    }

    private void updateState(bool enable_)
    {
        objToChange.SetActive(enable_);
        if (enable == enable_)
        {
            transform.localPosition = new Vector3(oriPlatePos.x, oriPlatePos.y-0.1f, oriPlatePos.z);
        } else {
            transform.localPosition = oriPlatePos;
        }
    }
}
