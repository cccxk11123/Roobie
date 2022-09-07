using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    //获取全部文本变量
    static UIManager instance;
    public TextMeshProUGUI orbText, timeText, deathText, gameOverText;

    void Awake()
    {
        if(instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(this);
    }
    public static void UpdateOrbUI(int orbCount)
    {
        instance.orbText.text = orbCount.ToString();
    }
    public static void UpdatedeathUI(int deathNum)
    {
        instance.deathText.text = deathNum.ToString();
    }
    public static void UpdatetimeUI(float time)
    {
        int mm = (int)(time / 60);
        float ss = time % 60;
        instance.timeText.text = mm.ToString("00") + ":" + ss.ToString("00");
    }
    public static void DisplayGameOver()
    {
        instance.gameOverText.enabled = true;
    }
}