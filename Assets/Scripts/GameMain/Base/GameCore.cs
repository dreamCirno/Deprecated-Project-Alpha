using UnityEngine;

namespace DefaultNamespace {
    /// <summary>
    /// 游戏入口。
    /// </summary>
    public partial class GameCore : MonoBehaviour {
        private void Start() {
            InitBuiltinComponents();
            InitCustomComponents();
        }
    }
}