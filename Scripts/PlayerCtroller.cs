using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCtroller : MonoBehaviour
{
    private Rigidbody2D rb;
    private BoxCollider2D coll;
    public LayerMask groundLayer;//检测陷阱

    [Header("移动参数")]
    public float Speed;
    public float crouchSpeeddivision;

    [Header("跳跃参数")]
    public float jumpForce;//基本跳跃力
    public float jumpHoldForce;//蓄力跳加成的力
    public float jumpHoldDuration;//长按跳跃的时间
    public float crouchJumpBoost;//下蹲跳跃额外加成
    public float hangingJumpForce;//悬挂力

    float jumpTime;//跳跃时间
    [Header("状态")]
    public bool isCrouch;
    public bool isOnGround;
    public bool isJump;
    public bool isHead;//判断头顶是否有物体
    public bool isHanging;

    [Header("环境检测")]
    public float footOffset = 0.4f;//脚的位置
    public float headDistance = 0.5f;
    public float groundDistance = 0.2f;//与地面的距离
    float playerHeight;
    public float eyeHeight = 1.5f;
    public float reachOffset = 0.7f;//头顶没有物体但头下有物体
    public float grabDistance = 0.4f;//与墙的判定距离

    public float xVelocity;
    float faceDirction;

    //按键设置
    bool jumpPressed;//单次按跳跃
    bool jumpHeld;//长按跳跃
    bool crouchHeld;//长按下蹲

    //保存碰撞体尺寸
    Vector2 colliderStandSize;
    Vector2 colliderStandOffset;
    Vector2 colliderCrouchSize;
    Vector2 colliderCrouchOffset;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        coll = GetComponent<BoxCollider2D>();
        //保存数据
        playerHeight = coll.size.y;
        colliderStandSize = coll.size;
        colliderStandOffset = coll.offset;
        colliderCrouchSize = new Vector2(coll.size.x, coll.size.y * 0.5f);
        colliderCrouchOffset = new Vector2(coll.offset.x, coll.offset.y * 0.5f);
    }
    private void Update()
    {
        if (Gamemanager.GameOver())
        {
            rb.bodyType = RigidbodyType2D.Static;
            return;//结束游戏直接返回
        }

        if (Input.GetButtonDown("Jump")) jumpPressed = true;

        jumpHeld = Input.GetButton("Jump");//该函数连续检测
        crouchHeld = Input.GetButton("Crouch");
    }
    private void FixedUpdate()
    {
        Movement();
        PhysicsCheck();
        MidAirMovement();
    }
    
    void Movement()
    {
        if (isHanging)//停止左右旋转
            return;
        if (crouchHeld && !isCrouch && isOnGround)//在地面按住下蹲且没有处于下蹲状态
            Crouch();
        else if (!crouchHeld && isCrouch && !isHead)//没有按下蹲键且处于下蹲状态
            StandUp();
        else if (!isOnGround && isCrouch)//空中防止下蹲
            StandUp();

        faceDirction = Input.GetAxisRaw("Horizontal");//朝向
        xVelocity = Input.GetAxis("Horizontal");//范围值

        if (isCrouch)//下蹲速度改变
            xVelocity /= crouchSpeeddivision;
        rb.velocity = new Vector2(xVelocity * Speed, rb.velocity.y);//立即更改速度

        if (faceDirction != 0)
            transform.localScale = new Vector3(faceDirction, 1, 1);
    }
    void MidAirMovement()
    {
        if(isHanging)
        {
            //处于悬挂状态
            if(jumpPressed)
            {
                //跳跃，还原rb类型
                rb.bodyType = RigidbodyType2D.Dynamic;
                rb.velocity = new Vector2(rb.velocity.x, hangingJumpForce);
                isHanging = false;
            }
            if(crouchHeld)
            {
                //下蹲，还原rb类型
                rb.bodyType = RigidbodyType2D.Dynamic;
                isHanging = false;
            }
        }

        if(!isHead)
        {//头上没有物体
            if (jumpPressed && isOnGround && !isJump)
            {
                //下蹲跳
                if (isCrouch & isOnGround)
                {
                    StandUp();
                    rb.AddForce(new Vector2(0f, crouchJumpBoost), ForceMode2D.Impulse);//施加一个力，力的模式为冲力
                }

                isOnGround = false;
                isJump = true;
                jumpPressed = false;

                //跳跃冷却时间
                jumpTime = Time.time + jumpHoldDuration;
                
                rb.AddForce(new Vector2(0f, jumpForce), ForceMode2D.Impulse);
                //播放音效
                AudioManager.playJumpAudio();
            }
            else if (isJump)
            {
                if (jumpHeld)
                    rb.AddForce(new Vector2(0f, jumpHoldForce), ForceMode2D.Impulse);
                //防止连续跳跃
                if (jumpTime < Time.time)
                    isJump = false;
            }
        }
    }
    void Crouch()
    {
        //改变状态
        isCrouch = true;
        //调整碰撞体大小
        coll.size = colliderCrouchSize;
        coll.offset = colliderCrouchOffset;
    }
    void StandUp()
    {
        //改变状态
        isCrouch = false;
        //调整碰撞体大小
        coll.size = colliderStandSize;
        coll.offset = colliderStandOffset;
    }

    void PhysicsCheck()
    {
        //射线方法检测,源点、方向、范围、检测的图层
        Vector2 pos = transform.position;
        RaycastHit2D leftCheck = Raycast(new Vector2(-footOffset, 0f), Vector2.down, groundDistance, groundLayer);//左右脚
        RaycastHit2D rightCheck = Raycast(new Vector2(footOffset, 0f), Vector2.down, groundDistance, groundLayer);
        RaycastHit2D headCheck = Raycast(new Vector2(0f, coll.size.y), Vector2.up, headDistance, groundLayer);
        
        if (leftCheck || rightCheck) isOnGround = true;
        else isOnGround = false;
        isHead = headCheck;

        playerHeight = coll.size.y;
        float direction = transform.localScale.x;//射线发出的方向与人物朝向一致
        Vector2 grabDir = new Vector2(direction, 0);//射线方向
        RaycastHit2D playerHeightCheck = Raycast(new Vector2(footOffset * direction, playerHeight), grabDir, grabDistance, groundLayer);//头部
        RaycastHit2D eyeCheck = Raycast(new Vector2(footOffset * direction, eyeHeight), grabDir, grabDistance, groundLayer);//眼睛
        RaycastHit2D reachCheck = Raycast(new Vector2(reachOffset * direction, playerHeight), Vector2.down, grabDistance, groundLayer);

        if (!isOnGround && rb.velocity.y < 0f && reachCheck && eyeCheck && !playerHeightCheck)
        {
            Vector3 playerPos = transform.position;//获取人物位置
            //挂在墙壁上调整人物的位置，头不超过墙面，身子紧贴墙壁
            playerPos.x += (eyeCheck.distance - 0.05f) * direction;
            playerPos.y -= reachCheck.distance;
            transform.position = playerPos;
            //静止在墙面上
            rb.bodyType = RigidbodyType2D.Static;
            isHanging = true;
        }
    }

    //方法重写来检测左脚、右脚、头顶
    RaycastHit2D Raycast(Vector2 offset, Vector2 dirction, float distance, LayerMask layer)
    {
        Vector2 pos = transform.position;//pos为基本位置，offset为偏移位置
        RaycastHit2D hit = Physics2D.Raycast(pos + offset, dirction, distance, layer);
        Color color = hit ? Color.red : Color.green;
        Debug.DrawRay(pos + offset, dirction, color, distance);//画出检测线
        return hit;//范围检测结果
    }
}