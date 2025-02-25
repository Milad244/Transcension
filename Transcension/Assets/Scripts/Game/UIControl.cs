using System.Collections;
using TMPro;
using UnityEngine;

public class UIControl : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI attackCharge;
    [SerializeField] private GameObject transitionPanel;
    [SerializeField] private TextMeshProUGUI transitionText;

    public void updateCharge(float charge)
    {
        attackCharge.SetText(charge.ToString("0.00"));
    }

    public void mindTransition()
    {   
        transitionPanel.SetActive(true);
        transitionText.SetText("");
    }
}
