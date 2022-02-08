using System;
using System.Collections.Generic;
using System.Diagnostics;
using CirnoFramework.Runtime.Utility;
using UnityEngine;
using XLua;

namespace CirnoFramework.Runtime.Lua {
    [Hotfix]
    public class LuaUpdater : MonoSingleton<LuaUpdater> {
        Action<float, float> luaUpdate = null;
        Action luaLateUpdate = null;
        Action<float> luaFixedUpdate = null;

#if UNITY_EDITOR
#pragma warning disable 0414
        // added by wsh @ 2017-12-29
        [SerializeField] long updateElapsedMilliseconds = 0;
        [SerializeField] long lateUpdateElapsedMilliseconds = 0;
        [SerializeField] long fixedUpdateElapsedMilliseconds = 0;
#pragma warning restore 0414
        Stopwatch sw = new Stopwatch();
#endif

        public void OnInit(LuaEnv luaEnv) {
#if UNITY_EDITOR
            sw.Start();
#endif
            Restart(luaEnv);
        }

        public void Restart(LuaEnv luaEnv) {
            luaUpdate = luaEnv.Global.Get<Action<float, float>>("Update");
            luaLateUpdate = luaEnv.Global.Get<Action>("LateUpdate");
            luaFixedUpdate = luaEnv.Global.Get<Action<float>>("FixedUpdate");
        }

        void Update() {
            if (luaUpdate != null) {
#if UNITY_EDITOR
                var start = sw.ElapsedMilliseconds;
#endif
                try {
                    luaUpdate(Time.deltaTime, Time.unscaledDeltaTime);
                }
                catch (Exception ex) {
                    Log.Error("luaUpdate err : " + ex.Message + "\n" + ex.StackTrace);
                }
#if UNITY_EDITOR
                updateElapsedMilliseconds = sw.ElapsedMilliseconds - start;
#endif
            }
        }

        void LateUpdate() {
            if (luaLateUpdate != null) {
#if UNITY_EDITOR
                var start = sw.ElapsedMilliseconds;
#endif
                try {
                    luaLateUpdate();
                }
                catch (Exception ex) {
                    Log.Error("luaLateUpdate err : " + ex.Message + "\n" + ex.StackTrace);
                }
#if UNITY_EDITOR
                lateUpdateElapsedMilliseconds = sw.ElapsedMilliseconds - start;
#endif
            }
        }

        void FixedUpdate() {
            if (luaFixedUpdate != null) {
#if UNITY_EDITOR
                var start = sw.ElapsedMilliseconds;
#endif
                try {
                    luaFixedUpdate(Time.fixedDeltaTime);
                }
                catch (Exception ex) {
                    Log.Error("luaFixedUpdate err : " + ex.Message + "\n" + ex.StackTrace);
                }
#if UNITY_EDITOR
                fixedUpdateElapsedMilliseconds = sw.ElapsedMilliseconds - start;
#endif
            }
        }

        public void OnDispose() {
            luaUpdate = null;
            luaLateUpdate = null;
            luaFixedUpdate = null;
        }

        void OnDestroy() {
            OnDispose();
        }
    }
}