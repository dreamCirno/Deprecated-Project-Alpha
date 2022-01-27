using System;
using CirnoFramework.Runtime;
using CirnoFramework.Runtime.Utility;
using GameFramework;
using UnityEngine;

namespace DefaultNamespace {
    /// <summary>
    /// 游戏入口。
    /// </summary>
    public partial class GameCore : MonoBehaviour {
        private void Awake() {
            InitLogHelper();
        }

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
            GameFrameworkCore.ShutDown();
            Log.Info($"{nameof(GameFrameworkCore)}: ShutDown.");
        }

        private void InitLogHelper() {
            var logHelperType = typeof(DefaultLogHelper);

            GameFrameworkLog.ILogHelper logHelper =
                (GameFrameworkLog.ILogHelper) Activator.CreateInstance(logHelperType);

            GameFrameworkLog.SetLogHelper(logHelper);
        }
    }
}