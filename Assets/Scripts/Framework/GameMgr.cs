using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Framework
{
    /// <summary>
    /// 游戏入口类
    /// 单例模式
    /// 外观模式
    /// 管理整个游戏下所有的Mgr
    /// </summary>
    public class GameMgr : MonoBehaviour
    {
        // 单例模式
        private static GameMgr _instance;

        public static GameMgr Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = FindObjectOfType<GameMgr>();

                    if (_instance == null)
                        _instance = new GameObject("GameLoop").AddComponent<GameMgr>();
                }
                return _instance;
            }
        }

        /// <summary>
        /// 一般每个场景下都会放一个GameMgr，切换场景若造成重复则会自动销毁
        /// </summary>
        private void Awake()
        {
            if (_instance == null)
                _instance = GetComponent<GameMgr>();
            else if (_instance != GetComponent<GameMgr>())
            {
                Debug.LogWarningFormat("There is more than one {0} in the scene，auto inactive the copy one.", typeof(GameMgr).ToString());
                Destroy(gameObject);
                return;
            }
            DontDestroyOnLoad(gameObject);
        }

        private void Start()
        {
            
        }

        private void FixedUpdate()
        {
            
        }

        private void Update()
        {
            
        }

    }
}