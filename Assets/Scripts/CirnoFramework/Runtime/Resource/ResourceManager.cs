using CirnoFramework.Runtime.Base;
using CirnoFramework.Runtime.Resource.Base;
using CirnoFramework.Runtime.Resource.GameObjectPool.Base;
using CirnoFramework.Runtime.Resource.Impl.Addressable;
using UnityEngine;

namespace CirnoFramework.Runtime.Resource {
    public class ResourceManager : IGameFrameworkModule, IUpdatable {
        public int Priority => 0;

        #region Property

        /// <summary>
        /// 资源助手
        /// </summary>
        public IAssetsHelper Asset { get; private set; }

        /// <summary>
        /// 资源版本信息
        /// </summary>
        public ResourceVersion Version { get; private set; }

        private IGameObjectPoolHelper _gameObjectPoolHelper;

        #endregion

        public ResourceManager() {
        }

        #region Public interface

        public void SetResourceHelper(IAssetsHelper resourceHelper) {
            Asset?.Clear();
            Asset = resourceHelper;
        }

        /// <summary>
        /// 设置对象池管理器的
        /// </summary>
        /// <param name="helper"></param>
        public void SetGameObjectPoolHelper(IGameObjectPoolHelper helper) {
            _gameObjectPoolHelper = helper;
        }

        /// <summary>
        /// 加载预设信息
        /// </summary>
        /// <param name="assetBundleName"></param>
        /// <param name="assetName"></param>
        /// <param name="prefabInfo"></param>
        public void AddPrefab(string assetBundleName, string assetName, PoolPrefabInfo prefabInfo) {
            _gameObjectPoolHelper.AddPrefab(assetBundleName, assetName, prefabInfo);
        }

        /// <summary>
        /// 生成物体
        /// </summary>
        /// <param name="assetName"></param>
        /// <returns></returns>
        public GameObject Spawn(string assetName) {
            return _gameObjectPoolHelper.Spawn(assetName);
        }

        /// <summary>
        /// 是否包含预设
        /// </summary>
        /// <param name="assetName"></param>
        /// <returns></returns>
        public bool HasPrefab(string assetName) {
            return _gameObjectPoolHelper.HasPrefab(assetName);
        }

        /// <summary>
        /// 销毁物体
        /// </summary>
        /// <param name="go"></param>
        /// <param name="isDestroy"></param>
        public void Despawn(GameObject go, bool isDestroy = false) {
            _gameObjectPoolHelper.Despawn(go, isDestroy);
        }

        /// <summary>
        /// 关闭所有物体
        /// </summary>
        public void DespawnAll() {
            _gameObjectPoolHelper.DespawnAll();
        }

        /// <summary>
        /// 销毁所有物体
        /// </summary>
        public void DestroyAll() {
            _gameObjectPoolHelper.DestroyAll();
        }

        /// <summary>
        /// 关闭预设的所有物体
        /// </summary>
        /// <param name="assetName"></param>
        public void DespawnPrefab(string assetName) {
            _gameObjectPoolHelper.DespawnPrefab(assetName);
        }

        #endregion

        #region Implement

        public void OnInit() {
            SetResourceHelper(new AddressableAssetsHelper());
            Version = new AddressableVersion();
        }

        public void OnUpdate() {
            Asset?.OnUpdate();
        }

        public void OnClose() {
            Asset?.Clear();
            _gameObjectPoolHelper?.DestroyAll();
        }

        public void OnDispose() {
        }

        #endregion
    }
}