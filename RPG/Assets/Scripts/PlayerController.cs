using UnityEngine;
using UnityEngine.UI;   // 引用介面 API
using Fungus;           // 引用蘑菇 API

public enum NPCState
{
    鐵塊, 鐵塊不足, 給予鑰匙, 打敗魔王
}

public class PlayerController : MonoBehaviour
{
    [Header("走路速度")]
    public float speed;
    [Header("旋轉速度")]
    public float turn;
    [Header("剛體")]
    public Rigidbody rig;
    [Header("動畫控制器")]
    public Animator ani;
    [Header("村民流程圖")]
    public Flowchart npc;
    [Header("村民目前狀態")]
    public NPCState npcState;
    [Header("任務介面")]
    public Text textMission;
    [Header("鐵礦數量")]
    public int metalCount;

    private GameObject obj;     // 儲存觸碰到的物件

    private void Move()
    {
        // 如果 目前動畫的資訊 名稱為"攻擊" 就跳出
        if (ani.GetCurrentAnimatorStateInfo(0).IsName("攻擊"))
        {
            return;
        }

        // Vertical   垂直：上：1、下：-1、沒按：0
        // Horizontal 水平：右：1、左：-1、沒按：0
        float v = Input.GetAxisRaw("Vertical");
        float h = Input.GetAxisRaw("Horizontal");
        // Mouse X 滑鼠水平：左：負，右：正、沒按：0
        float mX = Input.GetAxisRaw("Mouse X");

        //Debug.Log("玩家垂直：" + v);
        //Debug.Log("玩家水平：" + h);
        //Debug.Log("滑鼠水平：" + mX);

        // 剛體 推力((角色前方 * 垂直 + 角色右方 * 水平) * 速度)
        rig.AddForce((transform.forward * v + transform.right * h) * speed);
        // 變形 旋轉(X，Y，Z)
        transform.Rotate(0, mX * turn, 0);
    }

    private void AnimationControl()
    {
        if (Mathf.Abs(Input.GetAxisRaw("Vertical")) == 1 || Mathf.Abs(Input.GetAxisRaw("Horizontal")) == 1)
        {
            ani.SetBool("跑步開關", true);
        }
        else if(Input.GetAxisRaw("Vertical") == 0 || Input.GetAxisRaw("Horizontal") == 0)
        {
            ani.SetBool("跑步開關", false);
        }

        // 如果 目前動畫的資訊 名稱為"攻擊" 就重新設定 "攻擊開關"
        if (ani.GetCurrentAnimatorStateInfo(0).IsName("攻擊"))
        {
            ani.ResetTrigger("攻擊開關");
        }

        // 按下左鍵 播放攻擊動畫
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            ani.SetTrigger("攻擊開關");
        }
    }

    private void Update()
    {
        Move();
        AnimationControl();
    }

    private void Start()
    {
        Cursor.visible = false; // 指標 顯示 = 不要顯示 (true 為顯示)
    }

    private void OnTriggerStay(Collider other)
    {
        //Debug.Log("觸發到的物件：" + other.name);

        if (other.name == "村民")
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                //Debug.Log("觸發對話!!!");

                switch (npcState)
                {
                    case NPCState.鐵塊:
                        npc.SendFungusMessage("鐵塊");
                        //npcState = NPCState.鐵塊不足;
                        break;
                    case NPCState.鐵塊不足:
                        npc.SendFungusMessage("鐵塊不足");
                        break;
                    case NPCState.給予鑰匙:
                        npc.SendFungusMessage("給予鑰匙");
                        break;
                    case NPCState.打敗魔王:
                        npc.SendFungusMessage("打敗魔王");
                        break;
                }
            }
        }

        if (other.tag == "鐵礦")
        {
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                //Destroy(other.gameObject);
                obj = other.gameObject;                     // 物件 = 觸碰.遊戲物件
                Invoke("DelayDestroyMetal", 1);             // 延遲一秒呼叫方法：DelayDestroyMetal
            }
        }
    }

    private void DelayDestroyMetal()
    {
        if (obj == null) return;
        metalCount++;                                               // 數量遞增
        textMission.text = "取得鐵礦：" + metalCount + " / 5";      // 更新介面
        Destroy(obj);                                               // 刪除(物件)
    }

    /// <summary>
    /// 改變 NPC 村民狀態，0 鐵塊，1 鐵塊不足，2 給予鑰匙，3 打敗魔王
    /// </summary>
    /// <param name="npcStateIndex">編號</param>
    public void ChangeNPCState(int npcStateIndex)
    {
        npcState = (NPCState)npcStateIndex;
    }
}
