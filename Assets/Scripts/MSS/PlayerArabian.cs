using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MetalSlugStars
{
    public class PlayerArabian : MonoBehaviour
    {
        public float WalkSpeed { get; set; }
        public float JumpHeight { get; set; }
        public bool IsGround { get; set; }
        /// <summary>
        /// 若角色朝左时Scale为1，则IsReverse=true
        /// </summary>
        public bool IsReverse { get; set; }
        public bool IsDead { get; set; }
        /// <summary>
        /// 禁止左右移动
        /// 在攻击，死亡等时候会用上
        /// </summary>
        public bool CanMove { get; set; }
        public bool CanJump { get; set; }

        public string PlayerIndex;

        public Transform groundCheckPos;
        private Animator animator;
        private Rigidbody2D rg2d;
        private Collider2D collider2d;
        private Vector3 velocity;
        private AnimatorStateInfo stateInfo;
        private string aniAttack = "Attack";
        private string aniWalk = "Walk";
        private string aniJump = "Jump";
        private string aniDeath = "Death";
        private float h;
        private float v;
        private float scaleX;
        private int groundLayerIndex;

        private void Awake()
        {
            WalkSpeed = 3.5f;
            JumpHeight = 13.5f;
            IsReverse = true;
            CanMove = true;
            CanJump = true;
            scaleX = transform.localScale.x;
            groundLayerIndex = LayerMask.GetMask("Ground");
        }

        private void Start()
        {
            groundCheckPos = transform.GetChild(1).transform;
            animator = GetComponent<Animator>();
            rg2d = GetComponent<Rigidbody2D>();
            collider2d = GetComponent<Collider2D>();
        }

        private void FixedUpdate()
        {
            stateInfo = animator.GetCurrentAnimatorStateInfo(0);
            GroundCheck();
        }

        private void Update()
        {
            // 恢复移动状态
            if (stateInfo.IsName("Idle") || stateInfo.IsName("Walk"))
            {
                CanMove = true;
                CanJump = true;
            }
            else if (stateInfo.IsName("Attack"))
            {
                CanMove = false;
            }

            MoveInput();
            JumpInput();
            AttackInput();
        }

        private void GroundCheck()
        {
            //地面检测
            IsGround = Physics2D.Raycast( groundCheckPos.position, Vector2.down,
                0.1f, groundLayerIndex);
            //RaycastHit2D ray = Physics2D.Raycast(transform.position + new Vector3(0, -1.6f, 0), Vector2.down,
               // 0.15f, groundLayerIndex);
        }

        private void MoveInput()
        {
            h = Input.GetAxisRaw("Horizontal"+PlayerIndex);
            v = Input.GetAxisRaw("Vertical" + PlayerIndex);
            velocity = rg2d.velocity;
            if (CanMove)
            {
                //rg2d.MovePosition(transform.position+ new Vector3(h, 0, 0) * WalkSpeed * Time.deltaTime);
                rg2d.velocity = new Vector2(h * WalkSpeed , velocity.y);
            }
            // TODO:根据是否离地切换collider为trigger
            if (IsGround)
            {
                animator.SetFloat(aniWalk, Mathf.Abs(h));
                collider2d.isTrigger = false;
            }
            else
            {
                collider2d.isTrigger = true;
            }
            FaceToward();
        }

        private void FaceToward()
        {
            // 往右走x为正，左走为负，IsReverse则相反
            if (rg2d.velocity.x > 0.05f)
            {
                if (IsReverse)
                    scaleX = -Mathf.Abs(scaleX);
                else
                    scaleX = Mathf.Abs(scaleX);
            }
            else if (rg2d.velocity.x < -0.05f)
            {
                if (IsReverse)
                    scaleX = Mathf.Abs(scaleX);
                else
                    scaleX = -Mathf.Abs(scaleX);
            }

            transform.localScale = new Vector3(scaleX, transform.localScale.y, transform.localScale.z);
        }

        private void JumpInput()
        {
            // 不允许触发攻击后触发跳跃
            if (CanJump && IsGround && Input.GetButtonDown("Jump" + PlayerIndex))
            {
                rg2d.velocity = new Vector2(velocity.x, JumpHeight);
                CanJump = false;
            }
            // 注意按下Jump后有一段时间依然没有离地
            if (IsGround)
                animator.SetBool(aniJump, false);
            else
                animator.SetBool(aniJump, true);
        }

        private void AttackInput()
        {
            // 不允许触发跳跃后触发攻击
            if (CanMove && CanJump && IsGround && Input.GetButtonDown("Attack" + PlayerIndex))
            {
                animator.SetTrigger(aniAttack);
                //CanMove = false;
                CanJump = false;
            }

        }

        public void Dead()
        {
            CanMove = false;
            CanJump = false;
            IsDead = true;
            animator.SetTrigger(aniDeath);
        }

    }
}