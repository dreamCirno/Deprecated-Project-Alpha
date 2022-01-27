using CirnoFramework.Runtime;
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

        private void Update() {
            GameFrameworkCore.Update();
        }

        private void FixedUpdate() {
            GameFrameworkCore.FixedUpdate();
        }

        private void OnDestroy() {
            Debug.Log($"{nameof(GameFrameworkCore)}: ShutDown.");
            GameFrameworkCore.ShutDown();
        }
    }
}