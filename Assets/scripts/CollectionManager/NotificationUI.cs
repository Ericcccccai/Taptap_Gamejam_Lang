using UnityEngine;
using TMPro;
using System.Collections;

public class NotificationUI : MonoBehaviour
{
    public static NotificationUI I { get; private set; }

    [Header("UI References")]
    public CanvasGroup canvasGroup;
    public TextMeshProUGUI messageText;

    [Header("Timing")]
    public float fadeSpeed = 3f;
    public float displayDuration = 2f;

    Coroutine activeRoutine;

    void Awake()
    {
        if (I != null && I != this)
        {
            Destroy(gameObject);
            return;
        }
        I = this;
        DontDestroyOnLoad(gameObject);
        canvasGroup.alpha = 0f;
    }

    /// <summary>
    /// Display a temporary notification message.
    /// </summary>
    public void ShowMessage(string message)
    {
        if (activeRoutine != null)
            StopCoroutine(activeRoutine);

        activeRoutine = StartCoroutine(ShowRoutine(message));
    }

    IEnumerator ShowRoutine(string message)
    {
        messageText.text = message;
        yield return FadeTo(1f);
        yield return new WaitForSeconds(displayDuration);
        yield return FadeTo(0f);
    }

    IEnumerator FadeTo(float targetAlpha)
    {
        while (Mathf.Abs(canvasGroup.alpha - targetAlpha) > 0.01f)
        {
            canvasGroup.alpha = Mathf.MoveTowards(canvasGroup.alpha, targetAlpha, Time.deltaTime * fadeSpeed);
            yield return null;
        }
        canvasGroup.alpha = targetAlpha;
    }
}
