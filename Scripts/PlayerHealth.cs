using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    public GameObject deathVFXPrefab;
    public GameObject posVFXPrefab;
    int trapsLayer;
    private bool firstDeath = true;//防止死亡次数+2
    void Start()
    {
        trapsLayer = LayerMask.NameToLayer("Traps");//获取层级编号
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.layer == trapsLayer && firstDeath)
        {
            firstDeath = false;

            Instantiate(deathVFXPrefab, transform.position, transform.rotation);//释放烟雾效果
            Instantiate(posVFXPrefab, transform.position, Quaternion.Euler(0,0,Random.Range(-45,90)));
            gameObject.SetActive(false);

            AudioManager.playDeathAudio();

            Gamemanager.playerDied();
            //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);//重新加载当前场景
        }
    }
}
