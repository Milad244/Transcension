using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIControl : MonoBehaviour
{
    [SerializeField] private GameObject transitionPanel;
    [SerializeField] private TextMeshProUGUI transitionText;
    [SerializeField] private TextMeshProUGUI tipText;
    [SerializeField] private Image bossHealthFill;
    [SerializeField] private GameObject bossUI;
    private GlobalSceneManager globalSceneManager;

    public enum TipType
    {
        Movement,
        Jumping,
        Transcending
    }
    private Dictionary<TipType, string> tips;

    public HashSet<string> finishedTips = new HashSet<string>();
    private string currentTip;

    private void Awake()
    {
        globalSceneManager = GameObject.Find("GlobalManager").GetComponent<GlobalSceneManager>();
        tips = new Dictionary<TipType, string>
        {
            { TipType.Movement, "Press 'A' and 'D' to move!" },
            { TipType.Jumping, "Press '" + globalSceneManager.cleanKeyCode(globalSceneManager.keyBinds[GlobalSceneManager.Binds.Jump]) + "' to jump!"},
            { TipType.Transcending, "Press '"+ globalSceneManager.cleanKeyCode(globalSceneManager.keyBinds[GlobalSceneManager.Binds.Tran]) + "' to Transcend" },
        };
    }

    /// <summary>
    /// Activates a black panel for transitioning into the mind.
    /// </summary>
    public void mindTransition()
    {
        transitionPanel.SetActive(true);
        transitionText.SetText("");
    }

    /// <summary>
    /// Gets a tip message from a given tipType. If the tip is currently showing, it will close the tip. If the tip hasn't been shown before, it will show the tip.
    /// </summary>
    /// <param name="tipType">The type of tip.</param>
    public void showTip(TipType tipType)
    {

        string tipMessage = tips[tipType];

        if (tipMessage == currentTip)
        {
            closeTip();
        }
        else if (!finishedTips.Contains(tipMessage))
        {
            currentTip = tipMessage;
            tipText.gameObject.SetActive(true);
            tipText.SetText(tipMessage);
        }
    }

    /// <summary>
    /// Closes the currently shown tip and adds it to the finishedTips hashset.
    /// </summary>
    public void closeTip()
    {
        if (!string.IsNullOrEmpty(currentTip))
        {
            finishedTips.Add(currentTip);
            currentTip = null;
        }
        tipText.gameObject.SetActive(false);
    }

    /// <summary>
    /// Shows the transcend tip when true. Otherwise, disables it.
    /// </summary>
    /// <param name="active">True to show the tip, false to disable it.</param>
    public void transcendTipActive(bool active)
    {
        if (active)
        {
            tipText.gameObject.SetActive(true);
            tipText.SetText(tips[TipType.Transcending]);
        }
        else
        {
            if (this != null)
                Invoke(nameof(disableTipIfInGameScene), 0.1f); //Delay so if in mind scene it won't give error
        }
    }

    /// <summary>
    /// Closes the transcend tip if in the game scene.
    /// </summary>
    private void disableTipIfInGameScene()
    {
        if (this != null && SceneManager.GetActiveScene().name == "Game")
        {
            tipText.gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// Smoothly animates the boss fight's health bar from an initial value to a final value.
    /// </summary>
    /// <param name="from">Boss's intial health.</param>
    /// <param name="to">Boss's final health.</param>
    public IEnumerator updateHealthBar(float from, float to)
    {
        bossUI.SetActive(true);

        RectTransform rectTransform = bossHealthFill.GetComponent<RectTransform>();
        float elapsed = 0f;
        float duration = 1f;

        while (elapsed < duration)
        {
            float t = elapsed / duration;
            float currentWidth = Mathf.Lerp(from, to, t);
            Vector2 changingSize = new Vector2(currentWidth, rectTransform.sizeDelta.y);
            rectTransform.sizeDelta = changingSize;
            elapsed += Time.deltaTime;
            yield return null;
        }
        Vector2 finalWidth = new Vector2(to, rectTransform.sizeDelta.y);
        rectTransform.sizeDelta = finalWidth;
        yield return new WaitForSeconds(2);
        bossUI.SetActive(false);
    }
}
