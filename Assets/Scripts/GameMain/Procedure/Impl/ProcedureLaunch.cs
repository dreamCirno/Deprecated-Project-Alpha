using CirnoFramework.Runtime.Resource.GameObjectPool;
using CirnoFramework.Runtime.Utility;
using GameFramework.Fsm;
using GameFramework.Procedure;
using UnityEngine;
using ProcedureBase = GameMain.Procedure.Base.ProcedureBase;

namespace GameMain.Procedure.Impl {
    public class ProcedureLaunch : ProcedureBase {
        protected override void OnEnter(IFsm<IProcedureManager> procedureOwner) {
            base.OnEnter(procedureOwner);

            InitGameObjectPool();
            StartupXLua();

            Log.Info("ProcedureLaunch OnEnter");
        }

        private void InitGameObjectPool() {
            var gameObjectPoolHelper = new GameObject("GameObject PoolHelper");
            Object.DontDestroyOnLoad(gameObjectPoolHelper);
            GameCore.Resource.SetGameObjectPoolHelper(gameObjectPoolHelper.AddComponent<GameObjectPoolHelper>());
        }

        private void StartupXLua() {
            GameCore.XLua.StartHotfix();
            GameCore.XLua.StartGame();
        }
    }
}