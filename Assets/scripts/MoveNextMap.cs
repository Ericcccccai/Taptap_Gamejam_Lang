using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChangeTrigger : MonoBehaviour
{
    [SerializeField] private string nextSceneName; // 在 Inspector 填目标场景名字
    [SerializeField] private string targetSpawnPoint;  // 目标场景中的出生点名字

    [SerializeField] private string bugSceneName = "bugScene"; // 隐藏场景名
    [SerializeField, Range(0f, 100f)] private float bugSceneChance = 5f; // 百分比几率（默认5%）

    // 当有物体进入触发器时自动调用
    private void OnTriggerEnter2D(Collider2D other)
    
    {
        // 如果碰到的物体是玩家
        if (other.CompareTag("Player"))
        {
            // 生成一个0到100之间的随机数
            float randomValue = Random.Range(0f, 100f);
            // 判断是否进入隐藏场景
            if (randomValue < bugSceneChance)
            {

                Debug.Log("触发隐藏场景！" + bugSceneName);
                PlayerSpawnManager.lastSpawnPointName = "BugSpawn"; // 假设隐藏场景没有出生点
                SceneManager.LoadScene(bugSceneName);
            }else
            {

                Debug.Log("玩家碰到边界，切换场景中..." + nextSceneName);

                // 记录下一次出生点
                PlayerSpawnManager.lastSpawnPointName = targetSpawnPoint;
                SceneManager.LoadScene(nextSceneName); // 切换场景
            }
             //SceneManager.LoadScene(1);
        }
    }
}
