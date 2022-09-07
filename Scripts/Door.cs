using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    Animator anim;
    int openID;
    private void Start()
    {
        anim = GetComponentInParent<Animator>();
        openID = Animator.StringToHash("Open");
        Gamemanager.RegisterDoor(this);//����gamemanger���þ�������ע��
    }
    public void Open()
    {
        anim.SetTrigger(openID);
        AudioManager.playDoorOpen();
    }
}