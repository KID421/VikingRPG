using UnityEngine;
using UnityEngine.SceneManagement;  // 引用 場景管理 API

public class IconManager : MonoBehaviour
{
    [Header("圖示")]
    public RectTransform Icon;
    [Header("編號")]
    public int Index;
    [Header("座標")]
    public Vector2[] Position = { new Vector2(365, -50), new Vector2(365, -130), new Vector2(465, -200) };
    [Header("音效")]
    public AudioSource AS;
    public AudioClip SoundSelect, SoundEnter;

    private void ControlIcon(int number)
    {
        AS.PlayOneShot(SoundSelect, 1.5f);

        Index += number;                            // 編號累加傳入的數字

        if (Index == 3) Index = 0;
        else if (Index == -1) Index = 2;

        Icon.anchoredPosition = Position[Index];    // 圖示 的 座標 = 陣列資料[編號]
    }

    private void Enter()
    {
        AS.PlayOneShot(SoundEnter, 1.5f);

        if (Index == 0)
        {
            //GameStart();                  // 直接執行方法
            Invoke("GameStart", 1.5f);      // 延遲執行方法 ("方法名稱", 延遲時間)
        }
        else if (Index == 1)
        {
            //GameQuit();
            Invoke("GameQuit", 1.5f);
        }
        else if (Index == 2)
        {
            //GameSetting();
            Invoke("GameSetting", 1.5f);
        }
    }

    private void GameStart()
    {
        SceneManager.LoadScene("遊戲場景");
    }

    private void GameQuit()
    {
        Application.Quit();
    }

    private void GameSetting()
    {

    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))         // 按下 S 或者 下 控制圖示 - 傳入 +1 往下跑
        {
            ControlIcon(1);
        }
        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))           // 按下 W 或者 上 控制圖示 - 傳入 -1 往上跑
        {
            ControlIcon(-1);
        }

        if (Input.GetKeyDown(KeyCode.Return))   // Enter 鍵 Return
        {
            Enter();
        }
    }
}
