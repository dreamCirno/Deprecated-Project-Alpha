using CirnoFramework.Runtime.Procedure;
using CirnoFramework.Runtime.Utility;
using GameFramework.Fsm;
using GameFramework.Procedure;
using ProcedureBase = GameMain.Procedure.Base.ProcedureBase;

namespace GameMain.Procedure.Impl {
    [Procedure(ProcedureType.Start)]
    public class ProcedureResource : ProcedureBase {
        private IFsm<IProcedureManager> _procedureOwner;

        protected override void OnEnter(IFsm<IProcedureManager> procedureOwner) {
            base.OnEnter(procedureOwner);

            _procedureOwner = procedureOwner;

            InitializeAndUpdate();
        }

        public async void InitializeAndUpdate() {
            var version = GameCore.Resource.Version;
            var versionTask = version.Initialize();
            await versionTask;
            var isVersionInitialized = versionTask.Result;
            Log.Info($"{(isVersionInitialized ? "版本初始化完成" : "版本初始化失败")}。");
            if (isVersionInitialized) {
                version.CheckUpdate(needUpdate => {
                    Log.Info($"{(needUpdate ? "需要更新资源" : "不需要更新资源，当前已是最新版本")}。");
                    if (needUpdate) {
                        version.UpdateResource(OnResourceUpdateCallback, OnDownloadComplete, OnDownloadError);
                    }
                    else {
                        OnEnterLaunch();
                    }
                });
            }
            else {
                Log.Info("尝试重新初始化 ResourceVersion");
                InitializeAndUpdate();
            }
        }

        #region 事件回调

        /// <summary>
        /// 资源下载的回调
        /// </summary>
        /// <param name="progress">当前进度 0.0-1.0</param>
        /// <param name="speed">下载速度 KB/s</param>
        /// <param name="size">文件大小 KB</param>
        /// <param name="remainingTime">剩余时间 s</param>
        /// 下载回调[进度(0-1)，大小(KB),速度(KB/S),剩余时间(s)]
        private void OnResourceUpdateCallback(float progress, double size, double speed, float remainingTime) {
            Log.Info($"进度：{progress} 下载大小：{size} 下载速度：{speed} 剩余时间：{remainingTime}");
        }

        /// <summary>
        /// 资源下载完成
        /// </summary>
        private void OnDownloadComplete() {
            Log.Info("资源下载完成！转至游戏入口。");
        }

        /// <summary>
        /// 下载错误
        /// </summary>
        /// <param name="localPath"></param>
        /// <param name="error"></param>
        private void OnDownloadError(string localPath, string error) {
            Log.Error($"资源下载失败！ {localPath} {error}");

            InitializeAndUpdate();
        }

        private void OnEnterLaunch() {
            Log.Info("进入 Launch 场景。");
            ChangeState<ProcedureLaunch>(_procedureOwner);
        }

        #endregion
    }
}