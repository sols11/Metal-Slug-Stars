using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SFramework;
using UnityEngine;

namespace MetalSlugStars
{
    public class PlayerArabian:IPlayer
    {
        public override void Initialize()
        {
            base.Initialize();
            WalkSpeed = 3.5f;
            JumpHeight = 13.5f;
            IsReverse = true;
        }

        public PlayerArabian(GameObject gameObject):base(gameObject)
        {
            
        }

        public override void Update()
        {
            base.Update();
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

        private void MoveInput()
        {
            h = Input.GetAxisRaw("Horizontal" + PlayerIndex);
            v = Input.GetAxisRaw("Vertical" + PlayerIndex);
            velocity = Rg2d.velocity;
            if (CanMove)
            {
                //rg2d.MovePosition(transform.position+ new Vector3(h, 0, 0) * WalkSpeed * Time.deltaTime);
                Rg2d.velocity = new Vector2(h * WalkSpeed, velocity.y);
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


        private void JumpInput()
        {
            // 不允许触发攻击后触发跳跃
            if (CanJump && IsGround && Input.GetButtonDown("Jump" + PlayerIndex))
            {
                Rg2d.velocity = new Vector2(velocity.x, JumpHeight);
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
    }
}
