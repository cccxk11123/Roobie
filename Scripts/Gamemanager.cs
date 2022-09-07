using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Gamemanager : MonoBehaviour
{
    static Gamemanager instance;//方便直接访问
    List<Orb> orbs;//收集宝珠用链表记录方便添加和删除

    SceneFader fader;
    Door lockedDoor;

    int deathNum;
    float gameTime;
    bool gameIsOver;

    private void Awake()
    {
        if(instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;

        orbs = new List<Orb>();
        DontDestroyOnLoad(this);
    }
    private void Update()
    {
        //orbNum = instance.orbs.Count;
        if (gameIsOver)
            return;
        gameTime += Time.deltaTime;//游戏时间
        UIManager.UpdatetimeUI(instance.gameTime);//更新
    }
    public static void RegisterDoor(Door door)
    {
        instance.lockedDoor = door;
    }
    public static void RegisterSceneFader(SceneFader obj)
    {//注册场景fader
        instance.fader = obj;
    }
    public static void RegisterOrb(Orb orb)
    {
        if (instance == null) return;
        if(!instance.orbs.Contains(orb))
        {//若场景中不包含宝珠
            instance.orbs.Add(orb);
        }
        UIManager.UpdateOrbUI(instance.orbs.Count);
    }
    //玩家拿宝珠
    public static void playerGrabbedOrb(Orb orb)
    {
        if (!instance.orbs.Contains(orb))
            return;
        instance.orbs.Remove(orb);
        if(instance.orbs.Count == 0)
        {
            instance.lockedDoor.Open();
        }
        UIManager.UpdateOrbUI(instance.orbs.Count);
    }

    //玩家死亡，加载转场动画并重新加载场景
    public static void playerDied()
    {
        instance.fader.FadeOut();
        instance.deathNum++;
        UIManager.UpdatedeathUI(instance.deathNum);
        instance.Invoke("RestartScene", 1.5f);
    }
    void RestartScene()
    {
        instance.orbs.Clear();//清除宝珠数量
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    //石门开启
    public static bool DoorOpen()
    {
        if (instance.orbs.Count == 0)
            return true;
        return false;
    }
    //玩家获胜
    public static void PlayerWon()
    {
        instance.gameIsOver = true;
        UIManager.DisplayGameOver();
        AudioManager.playerWin();
    }
    //游戏结束
    public static bool GameOver()
    {
        return instance.gameIsOver;
    }
}