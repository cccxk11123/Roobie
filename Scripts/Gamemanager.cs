using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Gamemanager : MonoBehaviour
{
    static Gamemanager instance;//����ֱ�ӷ���
    List<Orb> orbs;//�ռ������������¼������Ӻ�ɾ��

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
        gameTime += Time.deltaTime;//��Ϸʱ��
        UIManager.UpdatetimeUI(instance.gameTime);//����
    }
    public static void RegisterDoor(Door door)
    {
        instance.lockedDoor = door;
    }
    public static void RegisterSceneFader(SceneFader obj)
    {//ע�᳡��fader
        instance.fader = obj;
    }
    public static void RegisterOrb(Orb orb)
    {
        if (instance == null) return;
        if(!instance.orbs.Contains(orb))
        {//�������в���������
            instance.orbs.Add(orb);
        }
        UIManager.UpdateOrbUI(instance.orbs.Count);
    }
    //����ñ���
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

    //�������������ת�����������¼��س���
    public static void playerDied()
    {
        instance.fader.FadeOut();
        instance.deathNum++;
        UIManager.UpdatedeathUI(instance.deathNum);
        instance.Invoke("RestartScene", 1.5f);
    }
    void RestartScene()
    {
        instance.orbs.Clear();//�����������
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    //ʯ�ſ���
    public static bool DoorOpen()
    {
        if (instance.orbs.Count == 0)
            return true;
        return false;
    }
    //��һ�ʤ
    public static void PlayerWon()
    {
        instance.gameIsOver = true;
        UIManager.DisplayGameOver();
        AudioManager.playerWin();
    }
    //��Ϸ����
    public static bool GameOver()
    {
        return instance.gameIsOver;
    }
}