using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChangeTrigger : MonoBehaviour
{
    [SerializeField] private string nextSceneName; // 在 Inspector 填目标场景名字

    // 当有物体进入触发器时自动调用
    private void OnTriggerEnter2D(Collider2D other)
    {
        // 如果碰到的物体是玩家
        if (other.CompareTag("Player"))
        {
            Debug.Log("玩家碰到边界，切换场景中..."+nextSceneName);
            SceneManager.LoadScene(nextSceneName); // 切换场景
        }
    }
}
