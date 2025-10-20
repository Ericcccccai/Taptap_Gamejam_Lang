using UnityEngine;
using UnityEngine.UI; // 记得导入 UI 命名空间
using System.Collections;

public class BugNetController : MonoBehaviour
{
    private Collider2D bugInRange;

    // 下面的代码是用于与背包管理器交互的示例
    /*   private BagManager bagManager;
     private void Start()
    {
        bagManager = FindObjectOfType<BagManager>(); // 获取背包管理器
    } */
    
        [Header("抓虫失败设置")]
    [SerializeField, Range(0f, 100f)] private float failChance = 10f; // 10% 抓虫失败概率
    [SerializeField] private Sprite[] failImages; // 三张“收集错误”图片
    [SerializeField] private Image failImageUI;   // 画面中显示失败图片的 Image 组件
    [SerializeField] private float failDisplayTime = 1.5f; // 显示持续时间

    public Transform player;                 // 拖入玩家对象
     
     [Header("UI跟随设置")]
public Vector3 uiOffset = new Vector3(0, 2f, 0); // 图片在玩家头顶偏移


      private bool isShowingFail = false;

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
            // 抓虫失败概率判断
            float randomValue = Random.Range(0f, 100f);
            if (randomValue < failChance)
            {
                Destroy(bugInRange.gameObject);// 抓到虫子但失败，虫子消失
                Debug.Log("收集失败！随机展示错误图片。");
                StartCoroutine(ShowFailImage());
                return; // 不继续执行抓虫逻辑
            }
            else if (bugInRange != null)
            {
                Debug.Log("玩家按E抓到虫子：" + bugInRange.name);
                Destroy(bugInRange.gameObject); // 抓到虫子
                                                //bagManager.AddBug();
                bugInRange = null;
            }
        }
        else if (bugInRange == null && Input.GetKeyDown(KeyCode.F))
        {
            Debug.Log("玩家按没有抓到虫子");
            bugInRange = null; // 放弃抓虫子
        }
        
        // ----------------------
    // UI跟随玩家
    if (failImageUI != null && failImageUI.gameObject.activeSelf && player != null)
    {
        Vector3 worldPos = player.position + uiOffset; // 玩家世界位置 + 偏移
        Vector3 screenPos = Camera.main.WorldToScreenPoint(worldPos); // 转换为屏幕坐标
        failImageUI.transform.position = screenPos; // 设置UI位置
    }
    }
        
        private IEnumerator ShowFailImage()
    {
        if (isShowingFail || failImageUI == null || failImages.Length == 0)
            yield break;

        isShowingFail = true;

        // 随机选择一张失败图片
        int randomIndex = Random.Range(0, failImages.Length);
        failImageUI.sprite = failImages[randomIndex];
        failImageUI.gameObject.SetActive(true);

        yield return new WaitForSeconds(failDisplayTime);

        failImageUI.gameObject.SetActive(false);
        isShowingFail = false;
    }
    
}
