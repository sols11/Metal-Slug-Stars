using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SFramework
{
    /// <summary>
    /// 角色控制系统
    /// 负责角色的创建，管理，删除
    /// </summary>
	public class PlayerMgr : IGameMgr
	{
        private List<IPlayer> playersInScene;
        public IPlayer CurrentPlayer { get; private set; } //切换场景时不要消除引用
        public bool CanInput { get; set; }

        public PlayerMgr(GameMainProgram gameMain):base(gameMain)
		{
		    playersInScene = new List<IPlayer>();
        }

        public override void Initialize()
        {
            if (CurrentPlayer != null)
                CurrentPlayer.Initialize();
        }

	    public override void Release()
	    {
	        foreach (IPlayer p in playersInScene)
	        {
	            if (p != null)
	                p.Release();
	        }
	        playersInScene.Clear();
	    }

	    public override void Update()
	    {
	        foreach (IPlayer p in playersInScene)
	            p.Update();
	    }

	    public override void FixedUpdate()
	    {
	        foreach (IPlayer p in playersInScene)
	            p.FixedUpdate();
        }

	    public void AddPlayer(IPlayer player)
	    {
	        if (player != null)
	        {
	            playersInScene.Add(player);
	            player.Initialize();
	        }
	    }

	    private void RemovePlayer(IPlayer player)
	    {
	        if (player != null)
	        {
	            playersInScene.Remove(player);
	            player.Release();
	        }
	    }

    }
}
