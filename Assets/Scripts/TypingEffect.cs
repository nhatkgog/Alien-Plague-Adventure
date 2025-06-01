using System.Collections;
using TMPro;
using UnityEngine;

public class TypingEffect : MonoBehaviour
{
    public TextMeshProUGUI textUI;
    private string fullText;
    public float delay = 0.01f;
    private Coroutine typingCoroutine;

    void OnEnable()
    {
        fullText = textUI.text;
        textUI.text = "";
        typingCoroutine = StartCoroutine(ShowText());
    }

    IEnumerator ShowText()
    {
        textUI.text = "";
        foreach(var c in fullText)
        {
            textUI.text += c;
            yield return new WaitForSeconds(delay);
        }
        typingCoroutine = null;
    }

    public void SkipOrFinish()
    {
        if (typingCoroutine != null)
        {
            StopCoroutine(typingCoroutine);
            textUI.text = fullText;
            typingCoroutine = null;
        }
    }
}
