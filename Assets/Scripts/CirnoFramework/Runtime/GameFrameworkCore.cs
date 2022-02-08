using System;
using System.Collections.Generic;
using System.Linq;
using CirnoFramework.Runtime.Base;
using CirnoFramework.Runtime.Utility;

namespace CirnoFramework.Runtime {
    public static class GameFrameworkCore {
        #region 属性

        private static readonly Dictionary<int, IGameFrameworkModule> _allGameModules =
            new Dictionary<int, IGameFrameworkModule>();

        private static readonly List<IUpdatable> _updatableGameModules = new List<IUpdatable>();

        private static readonly List<ILateUpdatable> _lateUpdatableGameModules = new List<ILateUpdatable>();

        private static readonly List<IFixedUpdatable> _fixedUpdatablesGameModules = new List<IFixedUpdatable>();

        #endregion

        #region 外部接口

        /// <summary>
        /// 初始化
        /// </summary>
        public static void Init() {
            var orderResult = _allGameModules.OrderBy(
                x => x.Value.Priority);
            foreach (var item in orderResult) {
                var module = item.Value;
                Log.Info($"[Priority: {module.Priority}] - {module.GetType().Name} Start Init.");
                module.OnInit();
                Log.Info($"[Priority: {module.Priority}] - {module.GetType().Name} Initialized.");
            }
        }

        /// <summary>
        /// 渲染帧
        /// </summary>
        public static void Update() {
            foreach (var module in _updatableGameModules) {
                module.OnUpdate();
            }
        }

        public static void LateUpdate() {
            foreach (var module in _lateUpdatableGameModules) {
                module.OnLateUpdate();
            }
        }

        /// <summary>
        /// 固定帧
        /// </summary>
        public static void FixedUpdate() {
            foreach (var module in _fixedUpdatablesGameModules) {
                module.OnFixedUpdate();
            }
        }

        /// <summary>
        /// 获取模块
        /// </summary>
        /// <typeparam name="T">游戏模块</typeparam>
        /// <returns></returns>
        public static T GetModule<T>() where T : IGameFrameworkModule, new() {
            return (T) GetModule(typeof(T));
        }

        /// <summary>
        /// 关闭游戏的所有模块
        /// </summary>
        public static void ShutDown() {
            var orderResult = _allGameModules.OrderBy(x => x.Value.Priority);
            foreach (var item in orderResult) {
                var module = item.Value;
                module.OnClose();
                Log.Info($"[Priority: {module.Priority}] - {module.GetType().Name} OnClose.");
            }

            _updatableGameModules.Clear();
            _fixedUpdatablesGameModules.Clear();
            _allGameModules.Clear();
        }

        #endregion

        #region 内部函数

        private static IGameFrameworkModule GetModule(Type type) {
            int hashCode = type.GetHashCode();
            if (_allGameModules.TryGetValue(hashCode, out var module))
                return module;
            module = CreateModule(type);
            return module;
        }

        private static IGameFrameworkModule CreateModule(Type type) {
            int hashCode = type.GetHashCode();
            IGameFrameworkModule module = (IGameFrameworkModule) Activator.CreateInstance(type);
            _allGameModules[hashCode] = module;
            //整理含 IUpdatable 的模块
            if (module is IUpdatable update)
                _updatableGameModules.Add(update);
            //整理含 IFixedUpdatable 的模块
            if (module is IFixedUpdatable fixedUpdate)
                _fixedUpdatablesGameModules.Add(fixedUpdate);
            return module;
        }

        #endregion
    }
}