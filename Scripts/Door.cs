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
        Gamemanager.RegisterDoor(this);//想让gamemanger调用就先向其注册
    }
    public void Open()
    {
        anim.SetTrigger(openID);
        AudioManager.playDoorOpen();
    }
}