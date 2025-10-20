using UnityEngine;

public class BugNetController : MonoBehaviour
{
    private Collider2D bugInRange;

    // 下面的代码是用于与背包管理器交互的示例
    /*   private BagManager bagManager;
     private void Start()
    {
        bagManager = FindObjectOfType<BagManager>(); // 获取背包管理器
    } */
    

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Bug"))
        {
            bugInRange = other;
            Debug.Log("虫子进入抓网范围");
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Bug") && other == bugInRange)
        {
            bugInRange = null;
            Debug.Log("虫子离开抓网范围");
        }
    }

    private void Update()
    {
        if (bugInRange != null && Input.GetKeyDown(KeyCode.F))
        {
            Debug.Log("玩家按E抓到虫子：" + bugInRange.name);
            Destroy(bugInRange.gameObject); // 抓到虫子
            //bagManager.AddBug();
            bugInRange = null;
        }else if (bugInRange == null && Input.GetKeyDown(KeyCode.F))
        {
            Debug.Log("玩家按没有抓到虫子");
            bugInRange = null; // 放弃抓虫子
        }
    }
}
