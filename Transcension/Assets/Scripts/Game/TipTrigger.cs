using UnityEngine;

public class TipTrigger : MonoBehaviour
{
    [SerializeField] private UIControl uiControl;
    [SerializeField] private UIControl.TipType tipType;

    private void OnTriggerEnter2D(Collider2D col)
    {
        // When player collides with a tip, it shows the tip and destroys itself.
        if (col.CompareTag("Player"))
        {
            uiControl.showTip(tipType);
            Destroy(gameObject);
        }
    }
}
