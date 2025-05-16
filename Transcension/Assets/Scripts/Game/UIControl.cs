using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIControl : MonoBehaviour
{
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
        { TipType.Transcending, "Press 'F' to Transcend" },
    };

    public HashSet<string> finishedTips = new HashSet<string>();
    private string currentTip;

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

    public void transcendTipActive(bool active)
    {
        if (active)
        {
            tipText.gameObject.SetActive(true);
            tipText.SetText(tips[TipType.Transcending]);
        } else {
            if (this != null)
                Invoke(nameof(disableTipIfInGameScene), 0.1f); //Delay so if in mind scene it won't give error
        }
    }

    private void disableTipIfInGameScene()
    {
        if (this != null && SceneManager.GetActiveScene().name == "Game")
        {
            tipText.gameObject.SetActive(false);
        }
    }
}
