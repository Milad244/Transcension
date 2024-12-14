using TMPro;
using UnityEngine;

public class UIControl : MonoBehaviour
{
    [SerializeField] private TextMeshPro attackCharge;

    public void updateCharge(float charge)
    {
        attackCharge.SetText(charge.ToString("0.00"));
    }
}
