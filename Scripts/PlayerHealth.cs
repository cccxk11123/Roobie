using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    public GameObject deathVFXPrefab;
    public GameObject posVFXPrefab;
    int trapsLayer;
    private bool firstDeath = true;//��ֹ��������+2
    void Start()
    {
        trapsLayer = LayerMask.NameToLayer("Traps");//��ȡ�㼶���
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.layer == trapsLayer && firstDeath)
        {
            firstDeath = false;

            Instantiate(deathVFXPrefab, transform.position, transform.rotation);//�ͷ�����Ч��
            Instantiate(posVFXPrefab, transform.position, Quaternion.Euler(0,0,Random.Range(-45,90)));
            gameObject.SetActive(false);

            AudioManager.playDeathAudio();

            Gamemanager.playerDied();
            //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);//���¼��ص�ǰ����
        }
    }
}
