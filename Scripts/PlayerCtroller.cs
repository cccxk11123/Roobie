using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCtroller : MonoBehaviour
{
    private Rigidbody2D rb;
    private BoxCollider2D coll;
    public LayerMask groundLayer;//�������

    [Header("�ƶ�����")]
    public float Speed;
    public float crouchSpeeddivision;

    [Header("��Ծ����")]
    public float jumpForce;//������Ծ��
    public float jumpHoldForce;//�������ӳɵ���
    public float jumpHoldDuration;//������Ծ��ʱ��
    public float crouchJumpBoost;//�¶���Ծ����ӳ�
    public float hangingJumpForce;//������

    float jumpTime;//��Ծʱ��
    [Header("״̬")]
    public bool isCrouch;
    public bool isOnGround;
    public bool isJump;
    public bool isHead;//�ж�ͷ���Ƿ�������
    public bool isHanging;

    [Header("�������")]
    public float footOffset = 0.4f;//�ŵ�λ��
    public float headDistance = 0.5f;
    public float groundDistance = 0.2f;//�����ľ���
    float playerHeight;
    public float eyeHeight = 1.5f;
    public float reachOffset = 0.7f;//ͷ��û�����嵫ͷ��������
    public float grabDistance = 0.4f;//��ǽ���ж�����

    public float xVelocity;
    float faceDirction;

    //��������
    bool jumpPressed;//���ΰ���Ծ
    bool jumpHeld;//������Ծ
    bool crouchHeld;//�����¶�

    //������ײ��ߴ�
    Vector2 colliderStandSize;
    Vector2 colliderStandOffset;
    Vector2 colliderCrouchSize;
    Vector2 colliderCrouchOffset;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        coll = GetComponent<BoxCollider2D>();
        //��������
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
            return;//������Ϸֱ�ӷ���
        }

        if (Input.GetButtonDown("Jump")) jumpPressed = true;

        jumpHeld = Input.GetButton("Jump");//�ú����������
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
        if (isHanging)//ֹͣ������ת
            return;
        if (crouchHeld && !isCrouch && isOnGround)//�ڵ��水ס�¶���û�д����¶�״̬
            Crouch();
        else if (!crouchHeld && isCrouch && !isHead)//û�а��¶׼��Ҵ����¶�״̬
            StandUp();
        else if (!isOnGround && isCrouch)//���з�ֹ�¶�
            StandUp();

        faceDirction = Input.GetAxisRaw("Horizontal");//����
        xVelocity = Input.GetAxis("Horizontal");//��Χֵ

        if (isCrouch)//�¶��ٶȸı�
            xVelocity /= crouchSpeeddivision;
        rb.velocity = new Vector2(xVelocity * Speed, rb.velocity.y);//���������ٶ�

        if (faceDirction != 0)
            transform.localScale = new Vector3(faceDirction, 1, 1);
    }
    void MidAirMovement()
    {
        if(isHanging)
        {
            //��������״̬
            if(jumpPressed)
            {
                //��Ծ����ԭrb����
                rb.bodyType = RigidbodyType2D.Dynamic;
                rb.velocity = new Vector2(rb.velocity.x, hangingJumpForce);
                isHanging = false;
            }
            if(crouchHeld)
            {
                //�¶ף���ԭrb����
                rb.bodyType = RigidbodyType2D.Dynamic;
                isHanging = false;
            }
        }

        if(!isHead)
        {//ͷ��û������
            if (jumpPressed && isOnGround && !isJump)
            {
                //�¶���
                if (isCrouch & isOnGround)
                {
                    StandUp();
                    rb.AddForce(new Vector2(0f, crouchJumpBoost), ForceMode2D.Impulse);//ʩ��һ����������ģʽΪ����
                }

                isOnGround = false;
                isJump = true;
                jumpPressed = false;

                //��Ծ��ȴʱ��
                jumpTime = Time.time + jumpHoldDuration;
                
                rb.AddForce(new Vector2(0f, jumpForce), ForceMode2D.Impulse);
                //������Ч
                AudioManager.playJumpAudio();
            }
            else if (isJump)
            {
                if (jumpHeld)
                    rb.AddForce(new Vector2(0f, jumpHoldForce), ForceMode2D.Impulse);
                //��ֹ������Ծ
                if (jumpTime < Time.time)
                    isJump = false;
            }
        }
    }
    void Crouch()
    {
        //�ı�״̬
        isCrouch = true;
        //������ײ���С
        coll.size = colliderCrouchSize;
        coll.offset = colliderCrouchOffset;
    }
    void StandUp()
    {
        //�ı�״̬
        isCrouch = false;
        //������ײ���С
        coll.size = colliderStandSize;
        coll.offset = colliderStandOffset;
    }

    void PhysicsCheck()
    {
        //���߷������,Դ�㡢���򡢷�Χ������ͼ��
        Vector2 pos = transform.position;
        RaycastHit2D leftCheck = Raycast(new Vector2(-footOffset, 0f), Vector2.down, groundDistance, groundLayer);//���ҽ�
        RaycastHit2D rightCheck = Raycast(new Vector2(footOffset, 0f), Vector2.down, groundDistance, groundLayer);
        RaycastHit2D headCheck = Raycast(new Vector2(0f, coll.size.y), Vector2.up, headDistance, groundLayer);
        
        if (leftCheck || rightCheck) isOnGround = true;
        else isOnGround = false;
        isHead = headCheck;

        playerHeight = coll.size.y;
        float direction = transform.localScale.x;//���߷����ķ��������ﳯ��һ��
        Vector2 grabDir = new Vector2(direction, 0);//���߷���
        RaycastHit2D playerHeightCheck = Raycast(new Vector2(footOffset * direction, playerHeight), grabDir, grabDistance, groundLayer);//ͷ��
        RaycastHit2D eyeCheck = Raycast(new Vector2(footOffset * direction, eyeHeight), grabDir, grabDistance, groundLayer);//�۾�
        RaycastHit2D reachCheck = Raycast(new Vector2(reachOffset * direction, playerHeight), Vector2.down, grabDistance, groundLayer);

        if (!isOnGround && rb.velocity.y < 0f && reachCheck && eyeCheck && !playerHeightCheck)
        {
            Vector3 playerPos = transform.position;//��ȡ����λ��
            //����ǽ���ϵ��������λ�ã�ͷ������ǽ�棬���ӽ���ǽ��
            playerPos.x += (eyeCheck.distance - 0.05f) * direction;
            playerPos.y -= reachCheck.distance;
            transform.position = playerPos;
            //��ֹ��ǽ����
            rb.bodyType = RigidbodyType2D.Static;
            isHanging = true;
        }
    }

    //������д�������š��ҽš�ͷ��
    RaycastHit2D Raycast(Vector2 offset, Vector2 dirction, float distance, LayerMask layer)
    {
        Vector2 pos = transform.position;//posΪ����λ�ã�offsetΪƫ��λ��
        RaycastHit2D hit = Physics2D.Raycast(pos + offset, dirction, distance, layer);
        Color color = hit ? Color.red : Color.green;
        Debug.DrawRay(pos + offset, dirction, color, distance);//���������
        return hit;//��Χ�����
    }
}