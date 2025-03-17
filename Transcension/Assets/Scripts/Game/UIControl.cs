using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIControl : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI attackCharge;
    [SerializeField] private GameObject transitionPanel;
    [SerializeField] private TextMeshProUGUI transitionText;
    [SerializeField] private TextMeshProUGUI tipText;

    public enum TipType
    {
        Movement,
        Jumping,
        Transcending
    }
    private Dictionary<TipType, string> tips = new Dictionary<TipType, string>
    {
        { TipType.Movement, "Press 'A' and 'D' to move!" },
        { TipType.Jumping, "Press 'Space' to jump!" },
        { TipType.Transcending, "Press 'F' to transcend" },
    };

    public HashSet<string> finishedTips = new HashSet<string>();
    private string currentTip;

    public void updateCharge(float charge)
    {
        attackCharge.SetText(charge.ToString("0.00"));
    }

    public void mindTransition()
    {   
        transitionPanel.SetActive(true);
        transitionText.SetText("");
    }

    public void showTip(TipType tipType) {

        string tipMessage = tips[tipType];

        if (tipMessage == currentTip) {
            closeTip();
        } else if (!finishedTips.Contains(tipMessage)){
            currentTip = tipMessage;
            tipText.gameObject.SetActive(true);
            tipText.SetText(tipMessage);
        }
    }

    public void closeTip() {
        if (!string.IsNullOrEmpty(currentTip))
        {
            finishedTips.Add(currentTip);
            currentTip = null;
        }
        tipText.gameObject.SetActive(false);
    }
}
