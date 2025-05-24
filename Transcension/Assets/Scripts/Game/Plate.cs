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
        // Activates plate when player steps on it.
        if (col.tag == "Player")
        {
            if (plateCoroutine != null)
                StopCoroutine(plateCoroutine);

            plateCoroutine = StartCoroutine(activateObj());
        }
    }

    /// <summary>
    /// Activates plate while player is on it. After player gets off plate, it will deactivate after a set enableTime.
    /// </summary>
    private IEnumerator activateObj()
    {
        updateState(enable);
        Collider2D player = Physics2D.OverlapBox(transform.position, GetComponent<Collider2D>().bounds.size, 0, LayerMask.GetMask("Player"));
        while (player)
        {
            player = Physics2D.OverlapBox(transform.position, GetComponent<Collider2D>().bounds.size, 0, LayerMask.GetMask("Player"));
            yield return null; // so game doesn't crash :)
        }
        yield return new WaitForSeconds(enableTime);
        updateState(!enable);
    }

    /// <summary>
    /// Updates the plate's visual state. If true, lowers the plate to show activation. If false, raises it back to its original position.
    /// </summary>
    /// <param name="enable_">True when the plate is activated, false when deactivated.</param>
    private void updateState(bool enable_)
    {
        objToChange.SetActive(enable_);
        if (enable == enable_)
        {
            transform.localPosition = new Vector3(oriPlatePos.x, oriPlatePos.y - 0.1f, oriPlatePos.z);
        }
        else
        {
            transform.localPosition = oriPlatePos;
        }
    }
}
