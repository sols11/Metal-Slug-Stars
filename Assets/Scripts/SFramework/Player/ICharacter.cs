using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

namespace SFramework
{
    /// <summary>
    /// 游戏中任何活的角色
    /// TODO:包含默认的基本属性，可根据需要修改
    /// </summary>
    public abstract class ICharacter
	{
		protected int maxHP=10;   // 保持一个底线值是为了避免一开始HP就为0
		protected int currentHP=10;
        protected int maxSP = 100;
        protected int currentSP = 100;
        protected bool isDead = false;			// 是否已挂
		//动画状态机
		public Animator animator;
		protected AnimatorStateInfo stateInfo;
	    protected Collider2D collider2d;
	    protected Vector3 velocity;
	    protected float scaleX;
	    protected int groundLayerIndex;
	    protected Transform groundCheckPos;

        public string Name { get; set; }
		public float MoveSpeed { get; set; }
        /// <summary>
        /// set时将CurrentHP回复满
        /// </summary>
        public int MaxHP { get { return maxHP; } set { maxHP = value < 0 ? 0 : value;
                CurrentHP = MaxHP;
            } }
		public virtual int CurrentHP
        {
            get { return currentHP; }
            set
            {
                currentHP = value >= MaxHP ? MaxHP : value;
                if (currentHP <= 0)
                {
                    currentHP = 0;
                    Dead();
                }
                HPpercent = currentHP * 1.0f / MaxHP;
            }
        }
        public float HPpercent { get; protected set; }
        public int MaxSP { get { return maxSP; } set { maxSP = value < 0 ? 0 : value;
                CurrentSP = MaxSP;
            } }
        public virtual int CurrentSP
        {
            get { return currentSP; }
            set
            {
                currentSP = value >= MaxSP ? MaxSP : value;
                currentSP = value < 0 ? 0 : currentSP;
                SPpercent = currentSP * 1.0f / MaxSP;
            }
        }
        public float SPpercent { get; protected set; }
        // 属性
	    public float WalkSpeed { get; set; }
	    public float JumpHeight { get; set; }
	    public bool IsGround { get; set; }
	    /// <summary>
	    /// 若角色朝左时Scale为1，则IsReverse=true
	    /// </summary>
	    public bool IsReverse { get; set; }
	    public bool IsDead { get { return isDead; } }

        //设置对应物体
        public GameObject GameObjectInScene { get; set; }
        public Rigidbody2D Rg2d { get; set; }

        //构造函数
        public ICharacter(GameObject gameObject)
        {
            GameObjectInScene = gameObject;
            groundLayerIndex = LayerMask.GetMask("Ground");
            if (GameObjectInScene != null)
            {
                animator = GameObjectInScene.GetComponent<Animator>();
                Rg2d = GameObjectInScene.GetComponent<Rigidbody2D>();
                collider2d = GameObjectInScene.GetComponent<Collider2D>();
                // 固定放第2个
                groundCheckPos = GameObjectInScene.transform.GetChild(1).transform;
                scaleX = GameObjectInScene.transform.localScale.x;
            }
        }

        public virtual void Initialize() { }
		public virtual void Release() { }
		public virtual void Update() { }

	    public virtual void FixedUpdate()
	    {
	        stateInfo = animator.GetCurrentAnimatorStateInfo(0);
	        GroundCheck();
        }

        // 方法
        public virtual void Dead()
		{
			if (isDead)
				return;
			isDead = true;
		}

	    protected void GroundCheck()
	    {
	        //地面检测
	        IsGround = Physics2D.Raycast(groundCheckPos.position, Vector2.down,
	            0.1f, groundLayerIndex);
	        //RaycastHit2D ray = Physics2D.Raycast(transform.position + new Vector3(0, -1.6f, 0), Vector2.down,
	        // 0.15f, groundLayerIndex);
	    }

	    protected void FaceToward()
	    {
	        // 往右走x为正，左走为负，IsReverse则相反
	        if (Rg2d.velocity.x > 0.05f)
	        {
	            if (IsReverse)
	                scaleX = -Mathf.Abs(scaleX);
	            else
	                scaleX = Mathf.Abs(scaleX);
	        }
	        else if (Rg2d.velocity.x < -0.05f)
	        {
	            if (IsReverse)
	                scaleX = Mathf.Abs(scaleX);
	            else
	                scaleX = -Mathf.Abs(scaleX);
	        }

	        GameObjectInScene.transform.localScale = new Vector3(scaleX, GameObjectInScene.transform.localScale.y, GameObjectInScene.transform.localScale.z);
	    }
    }
}
