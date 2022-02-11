using System;
using System.Collections.Generic;
using CirnoFramework.Runtime.Resource.GameObjectPool.Base;
using CirnoFramework.Runtime.Utility;
using UnityEngine;

namespace CirnoFramework.Runtime.Resource.GameObjectPool {
    public class GameObjectPoolHelper : MonoBehaviour, IGameObjectPoolHelper {
        /// <summary>
        /// 对象池名称
        /// </summary>
        public string PoolName { get; set; }

        /// <summary>
        /// 对象池所有的预设
        /// </summary>
        private readonly Dictionary<string, PoolPrefabInfo> _prefabs = new Dictionary<string, PoolPrefabInfo>();

        /// <summary>
        /// 已经生成并显示物体
        /// </summary>
        private readonly Dictionary<string, List<GameObject>> _spawneds = new Dictionary<string, List<GameObject>>();

        /// <summary>
        /// 已经生成未显示物体
        /// </summary>
        private readonly Dictionary<string, Queue<GameObject>>
            _despawneds = new Dictionary<string, Queue<GameObject>>();

        public void AddPrefab(string assetBundleName, string assetName, PoolPrefabInfo prefabInfo) {
            if (_prefabs.ContainsKey(assetName)) {
                Log.Debug($"已经存在资源：{assetName}");
                return;
            }

            if (prefabInfo.Prefab == null) {
                // 根据 assetName，直接从 ResourceManager 里面加载
                prefabInfo.Prefab = GameFrameworkCore.GetModule<ResourceManager>().Asset
                    .LoadAsset<GameObject>(assetName);
                if (prefabInfo.Prefab == null) {
                    Log.Debug($"无法找到预设资源：{assetName} is null");
                    return;
                }
            }

            _prefabs[assetName] = prefabInfo;
            _spawneds[assetName] = new List<GameObject>();

            Initialization(assetName, prefabInfo);
        }

        public void AddPrefabAsync(string assetBundleName, string assetName, PoolPrefabInfo prefabInfo,
            Action callback) {
            if (_prefabs.ContainsKey(assetName)) {
                Log.Debug($"已经存在资源：{assetName}");
                return;
            }

            if (prefabInfo.Prefab == null) {
                // 根据 assetName，直接从 ResourceManager 里面加载
                GameFrameworkCore.GetModule<ResourceManager>().Asset
                    .LoadAsset<GameObject>(assetName, o => {
                        prefabInfo.Prefab = o;
                        if (prefabInfo.Prefab == null) {
                            Log.Debug($"无法找到预设资源：{assetName} is null");
                            return;
                        }

                        _prefabs[assetName] = prefabInfo;
                        _spawneds[assetName] = new List<GameObject>();

                        Initialization(assetName, prefabInfo);

                        callback?.Invoke();
                    });
            }
        }

        public bool HasPrefab(string assetName) {
            return _prefabs.ContainsKey(assetName);
        }

        private void Initialization(string assetName, PoolPrefabInfo prefabInfo) {
            var objects = new Queue<GameObject>();
            for (var i = 0; i < prefabInfo.PreloadAmount; i++) {
                var obj = Instantiate(prefabInfo.Prefab, transform, true);
                obj.SetActive(false);
                objects.Enqueue(obj);
            }

            _despawneds[assetName] = objects;
        }

        /// <summary>
        /// 生成 GameObject
        /// </summary>
        /// <param name="assetName"></param>
        /// <returns></returns>
        public GameObject Spawn(string assetName) {
            if (!_despawneds.ContainsKey(assetName)) {
                // 在没有添加预设的时候，默认添加一个预设
                AddPrefab("", assetName, new PoolPrefabInfo {
                    PreloadAmount = 1
                });
            }

            GameObject gameObject;
            Queue<GameObject> queueGos = _despawneds[assetName];
            if (queueGos.Count > 0) {
                gameObject = queueGos.Dequeue();
                gameObject.SetActive(true);
            }
            else {
                gameObject = Instantiate(_prefabs[assetName].Prefab, transform, true);
            }

            _spawneds[assetName].Add(gameObject);

            return gameObject;
        }

        /// <summary>
        /// 异步生成 GameObject
        /// </summary>
        /// <param name="assetName"></param>
        /// <param name="callback"></param>
        public void SpawnAsync(string assetName, Action<GameObject> callback) {
            if (!_despawneds.ContainsKey(assetName)) {
                // 在没有添加预设的时候，默认添加一个预设
                AddPrefabAsync("", assetName, new PoolPrefabInfo {
                    PreloadAmount = 1
                }, () => {
                    GameObject gameObject;
                    Queue<GameObject> queueGos = _despawneds[assetName];
                    if (queueGos.Count > 0) {
                        gameObject = queueGos.Dequeue();
                        gameObject.SetActive(true);
                    }
                    else {
                        gameObject = Instantiate(_prefabs[assetName].Prefab, transform, true);
                    }

                    _spawneds[assetName].Add(gameObject);

                    callback?.Invoke(gameObject);
                });
            }
        }

        /// <summary>
        /// 回收 GameObject（默认不销毁）
        /// </summary>
        /// <param name="go">GameObject</param>
        /// <param name="isDestroy">需要被 Destroy 吗？</param>
        public void Despawn(GameObject go, bool isDestroy = false) {
            foreach (var item in _spawneds) {
                if (item.Value.Contains(go)) {
                    if (isDestroy) {
                        item.Value.Remove(go);
                        Destroy(go);
                    }
                    else {
                        Queue<GameObject> _queueObjs = _despawneds[item.Key];
                        if ((_prefabs[item.Key].MaxAmount >= 0)
                            && (item.Value.Count + _queueObjs.Count) > _prefabs[item.Key].MaxAmount) {
                            item.Value.Remove(go);
                            Destroy(go);
                        }
                        else {
                            item.Value.Remove(go);
                            go.SetActive(false);
                            _despawneds[item.Key].Enqueue(go);
                        }
                    }

                    return;
                }
            }
        }

        /// <summary>
        /// 回收所有正在使用的 GameObject
        /// </summary>
        public void DespawnAll() {
            foreach (var item in _spawneds) {
                for (int i = item.Value.Count - 1; i >= 0; i--) {
                    var go = item.Value[i];
                    item.Value.Remove(go);
                    go.SetActive(false);
                    _despawneds[item.Key].Enqueue(go);
                }
            }
        }

        /// <summary>
        /// 销毁所有对象池的 GameObject
        /// </summary>
        public void DestroyAll() {
            foreach (var item in _spawneds) {
                for (int i = item.Value.Count - 1; i >= 0; i--) {
                    var go = item.Value[i];
                    item.Value.Remove(go);
                    Destroy(go);
                }
            }

            foreach (var item in _despawneds.Values) {
                while (item.Count > 0) {
                    Destroy(item.Dequeue());
                }
            }
        }

        /// <summary>
        /// 回收名为 assetName 所有的 GameObject
        /// </summary>
        /// <param name="assetName"></param>
        public void DespawnPrefab(string assetName) {
            if (_spawneds.ContainsKey(assetName)) {
                var objs = _spawneds[assetName];
                for (int i = objs.Count - 1; i >= 0; i--) {
                    var go = objs[i];
                    objs.Remove(go);
                    go.SetActive(false);
                    _despawneds[assetName].Enqueue(go);
                }
            }
            else {
                Log.Error($"DespawnPrefab [{assetName}] failed.");
            }
        }
    }
}