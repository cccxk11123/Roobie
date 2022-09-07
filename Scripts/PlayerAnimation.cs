using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    Animator anim;
    PlayerCtroller movement;
    Rigidbody2D rb;

    int groundID;
    int hangingID;
    int crouchID;
    int speedID;
    int jumpID;
    int fallID;
    void Start()
    {
        anim = GetComponent<Animator>();
        movement = GetComponentInParent<PlayerCtroller>();//获取父级的脚本
        rb = GetComponentInParent<Rigidbody2D>();

        speedID = Animator.StringToHash("speed");
        groundID = Animator.StringToHash("isOnGround");//获取编号来设置动画
        hangingID = Animator.StringToHash("isHanging");
        crouchID = Animator.StringToHash("isCrouching");
        jumpID = Animator.StringToHash("isJumping");
        fallID = Animator.StringToHash("verticalVelocity");
    }
    void Update()
    {
        anim.SetFloat(speedID, Mathf.Abs(movement.xVelocity));
        //anim.SetBool("isOnGround", movement.isOnGround);
        anim.SetBool(groundID, movement.isOnGround);//字符形变量传递可能会出现问题
        anim.SetBool(hangingID, movement.isHanging);
        anim.SetBool(crouchID, movement.isCrouch);
        anim.SetBool(jumpID, movement.isJump);
        anim.SetFloat(fallID, rb.velocity.y);
    }
    public void StepAudio()
    {
        AudioManager.playFootstepAudio();
    }
    public void CrouchStepAudio()
    {
        AudioManager.playCrouchAudio();
    }
}