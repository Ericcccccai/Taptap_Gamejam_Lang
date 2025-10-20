using UnityEngine;

public class PlayerSpawnManager : MonoBehaviour
{
    public static string lastSpawnPointName;

    void Start()
    {
        if (!string.IsNullOrEmpty(lastSpawnPointName))
        {
            GameObject spawnPoint = GameObject.Find(lastSpawnPointName);
            if (spawnPoint != null)
            {
                transform.position = spawnPoint.transform.position;
                Debug.Log("玩家出生在：" + lastSpawnPointName);
            }
            else
            {
                Debug.LogWarning("找不到出生点：" + lastSpawnPointName);
            }
        }
    }
}
