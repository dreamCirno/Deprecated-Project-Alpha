using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CirnoFramework.Runtime.Resource.Base;
using CirnoFramework.Runtime.Utility;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace CirnoFramework.Runtime.Resource.Impl.Addressable {
    public class AddressableVersion : ResourceVersion {
        public override async Task<bool> Initialize() {
            // 初始化 Addressables
            var initHandle = Addressables.InitializeAsync();
            await initHandle.Task;
            Log.Info($"{nameof(Addressables.InitializeAsync)}: {initHandle.Status}");
            // 检测 Catalog 是否存在更新？
            var checkHandle = Addressables.CheckForCatalogUpdates(false);
            await checkHandle.Task;
            if (checkHandle.Status == AsyncOperationStatus.Succeeded) {
                var catalogList = checkHandle.Result;
                if (catalogList.Count > 0) {
                    // 需要更新 Catalog
                    var updateCatalogHandle = Addressables.UpdateCatalogs(catalogList, false);
                    Log.Info("正在更新 Catalog...");
                    await updateCatalogHandle.Task;
                    if (updateCatalogHandle.Status == AsyncOperationStatus.Succeeded) {
                        Log.Info("更新 Catalog 完毕。");
                        return true;
                    }
                    else {
                        Log.Error($"{nameof(Addressables.UpdateCatalogs)}: Failed");
                        return false;
                    }
                }
                else {
                    // 不需要更新 Catalog
                    Log.Info("当前 Catalog 是最新版本。");
                    return true;
                }
            }
            else {
                Log.Error($"{nameof(Addressables.CheckForCatalogUpdates)}: Failed");
                return false;
            }
        }

        public override async void CheckUpdate(Action<bool> needUpdate) {
            var keys = new HashSet<object>();
            foreach (var locator in Addressables.ResourceLocators) {
                foreach (var key in locator.Keys) {
                    keys.Add(key);
                }
            }

            var getDownloadSizeHandle = Addressables.GetDownloadSizeAsync(keys);
            await getDownloadSizeHandle.Task;
            if (getDownloadSizeHandle.Status == AsyncOperationStatus.Succeeded) {
                var downloadSize = getDownloadSizeHandle.Result;
                var isNeedUpdateResource = downloadSize > 0;
                needUpdate?.Invoke(isNeedUpdateResource);
            }
            else {
                Log.Error($"{nameof(Addressables.GetDownloadSizeAsync)}: Failed");
            }
        }

        public override async void UpdateResource(Action<float, double, double, float> callback,
            Action downloadComplete,
            Action<string, string> errorCallback, string label = null) {
            // TODO：可优化
            var keys = new HashSet<object>();
            foreach (var locator in Addressables.ResourceLocators) {
                foreach (var key in locator.Keys) {
                    keys.Add(key);
                }
            }

            var downloadDependenciesHandle = Addressables.DownloadDependenciesAsync(keys, Addressables.MergeMode.Union);
            var downloadStartTime = Time.realtimeSinceStartup;
            while (!downloadDependenciesHandle.IsDone) {
                var downloadStatus = downloadDependenciesHandle.GetDownloadStatus();
                var totalKbSize = downloadStatus.TotalBytes / 1024.0f;
                var downloadKbSize = downloadStatus.DownloadedBytes / 1024.0f;
                var percentage = downloadStatus.Percent;
                var useTime = Time.realtimeSinceStartup - downloadStartTime;
                var downloadSpeed = downloadKbSize / useTime;
                var remainingTime = (totalKbSize - downloadKbSize) / downloadSpeed;
                callback?.Invoke(percentage, totalKbSize, downloadSpeed,
                    float.IsInfinity(remainingTime) ? 0 : remainingTime);
                await Task.Delay(1000);
            }

            await downloadDependenciesHandle.Task;
            if (downloadDependenciesHandle.Status == AsyncOperationStatus.Succeeded) {
                callback?.Invoke(1, 0, 0, 0);
                downloadComplete?.Invoke();
            }
            else {
                errorCallback?.Invoke(downloadDependenciesHandle.OperationException.Message,
                    downloadDependenciesHandle.OperationException.ToString());
            }
        }
    }
}