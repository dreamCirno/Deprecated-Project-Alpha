using System;
using System.Collections.Generic;
using CirnoFramework.Runtime.Resource.Base;
using CirnoFramework.Runtime.Utility;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;
using Object = UnityEngine.Object;

namespace CirnoFramework.Runtime.Resource.Impl.Addressable {
    public class AddressableAssetsHelper : IAssetsHelper {
        public bool IsProgressRunning { get; private set; }
        public List<string> AllAssetPaths { get; }

        private Dictionary<string, AsyncOperationHandle<SceneInstance>> _sceneInstanceAsync =
            new Dictionary<string, AsyncOperationHandle<SceneInstance>>();

        private Dictionary<string, Object> _objectAsync = new Dictionary<string, Object>();

        // 逻辑层正在等待的 asset 加载异步句柄
        private List<AsyncOperationHandle> _processingAssetAsyncList = new List<AsyncOperationHandle>();

        public void SetResource(Action callback) {
            callback?.Invoke();
        }

        public void Preload(Action<float> progressCallback) {
            progressCallback?.Invoke(1f);
        }

        public void LoadAssetBundle(string assetBundleName, Action<AssetBundle> callback) {
            throw new NotImplementedException();
        }

        public void LoadAsset<T>(string assetName, Action<T> callback) where T : Object {
            var handle = Addressables.LoadAssetAsync<T>(assetName);
            _processingAssetAsyncList.Add(handle);
            handle.Completed += (handle) => {
                var @object = handle.Result;
                CheckAsset(assetName, @object);
                callback?.Invoke(@object);
            };
        }

        public T LoadAsset<T>(string assetName) where T : Object {
            var handle = Addressables.LoadAssetAsync<T>(assetName);
            var @object = handle.WaitForCompletion();
            CheckAsset(assetName, @object);
            return @object;
        }

        public T[] FindAssets<T>(List<string> tags) where T : Object {
            var handle = Addressables.LoadAssetsAsync<T>(tags, (tObject) => { }, Addressables.MergeMode.Union);
            var resultList = handle.WaitForCompletion();
            T[] result = new T[resultList.Count];
            resultList.CopyTo(result, 0);
            return result;
        }

        public void FindAssets<T>(List<string> tags, Action<T[]> callback) where T : Object {
            var handle = Addressables.LoadAssetsAsync<T>(tags, (tObject) => { }, Addressables.MergeMode.Union);
            handle.Completed += (resultHandle) => {
                var resultList = resultHandle.Result;
                T[] result = new T[resultList.Count];
                resultList.CopyTo(result, 0);
                callback?.Invoke(result);
            };
        }

        public void UnloadAsset(string assetName) {
            if (_objectAsync.ContainsKey(assetName)) {
                var @object = _objectAsync[assetName];
                _objectAsync.Remove(assetName);
                if (@object != null) {
                    Addressables.Release(@object);
                }
            }
        }

        public void UnloadAssetBundle(string assetBundleName, bool unload = false) {
            throw new NotImplementedException();
        }

        public void LoadSceneAsync(string sceneName, LoadSceneMode mode, Action<AsyncOperation> callback) {
            if (!_sceneInstanceAsync.ContainsKey(sceneName)) {
                var handle = Addressables.LoadSceneAsync(sceneName, mode);
                _sceneInstanceAsync.Add(sceneName, handle);
                handle.Completed += (sceneHandle) => { callback?.Invoke(sceneHandle.Result.ActivateAsync()); };
            }
        }

        public AsyncOperation UnloadSceneAsync(string sceneName) {
            if (_sceneInstanceAsync.ContainsKey(sceneName)) {
                var handle = _sceneInstanceAsync[sceneName];
                _sceneInstanceAsync.Remove(sceneName);
                var unloadHandle = Addressables.UnloadSceneAsync(handle);
                return unloadHandle.Result.ActivateAsync();
            }

            return null;
        }

        public void Clear() {
            foreach (var item in _objectAsync) {
                Addressables.Release(item.Value);
            }

            _objectAsync.Clear();

            foreach (var item in _sceneInstanceAsync) {
                Addressables.UnloadSceneAsync(item.Value);
            }

            _sceneInstanceAsync.Clear();
        }

        private void CheckAsset(string assetName, Object @object) {
            if (!_objectAsync.ContainsKey(assetName)) {
                _objectAsync.Add(assetName, @object);
            }
        }

        public void OnUpdate() {
            OnProcessingAssetAsyncLoader();
        }

        void OnProcessingAssetAsyncLoader() {
            for (int i = _processingAssetAsyncList.Count - 1; i >= 0; i--) {
                var asyncOp = _processingAssetAsyncList[i];
                if (asyncOp.IsDone) {
                    _processingAssetAsyncList.RemoveAt(i);
                }
            }
        }
    }
}