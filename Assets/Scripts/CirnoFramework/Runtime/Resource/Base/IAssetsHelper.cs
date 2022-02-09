using System;
using System.Collections.Generic;
using CirnoFramework.Runtime.Base;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace CirnoFramework.Runtime.Resource.Base {
    public interface IAssetsHelper : IUpdatable {
        bool IsRunning { get; }

        /// <summary>
        /// 所有的资源路径 全小写
        /// </summary>
        List<string> AllAssetPaths { get; }

        /// <summary>
        /// 设置资源的路径,默认是为只读路径:Application.streamingAssetsPath;
        /// </summary>
        /// <param name="path"></param>
        void SetResource(Action callback);

        /// <summary>
        /// 预加载回调
        /// </summary>
        /// <param name="progressCallback"></param>
        void Preload(Action<float> progressCallback);

        /// <summary>
        /// 加载 AssetBundle
        /// </summary>
        /// <param name="assetBundleName"></param>
        /// <param name="callback"></param>
        /// <returns></returns>
        void LoadAssetBundle(string assetBundleName, Action<AssetBundle> callback);

        /// <summary>
        /// 同步加载资源
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="assetName"></param>
        T LoadAsset<T>(string assetName) where T : UnityEngine.Object;
        
        /// <summary>
        /// 异步加载资源
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="assetName"></param>
        /// <param name="callback"></param>
        /// <returns></returns>
        void LoadAsset<T>(string assetName, Action<T> callback) where T : UnityEngine.Object;

        /// <summary>
        /// 同步加载多个资源
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="tags"></param>
        /// <returns></returns>
        T[] LoadAssetsAsync<T>(IEnumerable<string> tags) where T : UnityEngine.Object;

        /// <summary>
        /// 异步加载多个资源
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="tags"></param>
        /// <param name="callback"></param>
        void LoadAssetsAsync<T>(IEnumerable<string> tags, Action<T[]> callback) where T : UnityEngine.Object;

        /// <summary>
        /// 卸载资源 -- 取消掉资源计数
        /// </summary>
        /// <param name="assetName"></param>
        void UnloadAsset(string assetName);

        /// <summary>
        /// 卸载资源 -- 主要为卸载 AssetBundle
        /// </summary>
        /// <param name="assetBundleName">资源名称</param>
        /// <param name="unload">是否卸载调所有资源</param>
        void UnloadAssetBundle(string assetBundleName, bool unload = false);

        /// <summary>
        /// 异步加载场景
        /// </summary>
        /// <param name="sceneName"></param>
        /// <param name="mode"></param>
        /// <param name="callback"></param>
        void LoadSceneAsync(string sceneName, LoadSceneMode mode, Action<AsyncOperation> callback);

        /// <summary>
        /// 卸载场景
        /// </summary>
        /// <param name="sceneName"></param>
        AsyncOperation UnloadSceneAsync(string sceneName);

        /// <summary>
        /// 清理资源
        /// </summary>
        void Clear();
    }
}