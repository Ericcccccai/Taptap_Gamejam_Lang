using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class BugData
{
    public string id;            // unique internal name
    public string displayName;   // shown to player
    public Sprite icon;          // image for gallery
    [HideInInspector] public bool collected;  // saved state
}

public class BugCollectionManager : MonoBehaviour
{
    public static BugCollectionManager I { get; private set; }

    [Header("Bug Database")]
    public List<BugData> allBugs = new List<BugData>();

    private const string SAVE_KEY_PREFIX = "BUG_";

    void Awake()
    {
        // Singleton setup
        if (I != null && I != this)
        {
            Destroy(gameObject);
            return;
        }
        I = this;
        DontDestroyOnLoad(gameObject);
        LoadProgress();
    }

    /// <summary>
    /// Mark a bug as collected if it wasn't before, save progress, and show a notification.
    /// </summary>
    public void CollectBug(string bugID)
    {
        BugData bug = allBugs.Find(b => b.id == bugID);
        if (bug == null)
        {
            Debug.LogWarning($"[BugCollectionManager] Bug ID '{bugID}' not found!");
            return;
        }

        if (!bug.collected)
        {
            bug.collected = true;
            SaveProgress();
            Debug.Log($"Collected new bug: {bug.displayName}");
            if (NotificationUI.I != null)
                NotificationUI.I.ShowMessage($"🪲 New bug collected: {bug.displayName}!");
        }
    }

    /// <summary>
    /// Check if a bug has been collected.
    /// </summary>
    public bool IsCollected(string bugID)
    {
        BugData bug = allBugs.Find(b => b.id == bugID);
        return bug != null && bug.collected;
    }

    /// <summary>
    /// Save collection progress using PlayerPrefs.
    /// </summary>
    public void SaveProgress()
    {
        foreach (var bug in allBugs)
        {
            PlayerPrefs.SetInt(SAVE_KEY_PREFIX + bug.id, bug.collected ? 1 : 0);
        }
        PlayerPrefs.Save();
    }

    /// <summary>
    /// Load collection progress.
    /// </summary>
    public void LoadProgress()
    {
        foreach (var bug in allBugs)
        {
            bug.collected = PlayerPrefs.GetInt(SAVE_KEY_PREFIX + bug.id, 0) == 1;
        }
    }

    /// <summary>
    /// Clear all saved collection data (for testing/debug).
    /// </summary>
    public void ResetCollection()
    {
        foreach (var bug in allBugs)
        {
            bug.collected = false;
            PlayerPrefs.DeleteKey(SAVE_KEY_PREFIX + bug.id);
        }
        PlayerPrefs.Save();
    }
}
