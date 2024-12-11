using TMPro;
using UnityEngine;

public class UIControl : MonoBehaviour
{
    [SerializeField] private TextMeshPro attackCharge;

    public void updateCharge(float charge)
    {
        if (charge > 1)
            charge = 1;

        attackCharge.SetText(charge.ToString("0.00"));
    }
}
