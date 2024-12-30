using TMPro;
using UnityEngine;

public class UIControl : MonoBehaviour
{
    [SerializeField] private TextMeshPro attackCharge;
    [SerializeField] private TextMeshPro dialogueText;

    public void updateCharge(float charge)
    {
        attackCharge.SetText(charge.ToString("0.00"));
    }

    public void writeDialogue(string dialogue)
    {
        dialogueText.SetText(dialogue);
    }

    public void stopDialogue()
    {
        dialogueText.SetText("");
    }
}
