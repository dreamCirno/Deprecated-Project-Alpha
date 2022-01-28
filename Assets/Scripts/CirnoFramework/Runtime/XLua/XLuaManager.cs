using System.Collections.Generic;
using CirnoFramework.Runtime.Base;
using CirnoFramework.Runtime.Resource;
using CirnoFramework.Runtime.Utility;
using UnityEngine;
using XLua;

namespace CirnoFramework.Runtime.XLua {
    public class XLuaManager : IGameFrameworkModule, IUpdatable, IFixedUpdatable {
        public LuaEnv LuaEnv { get; private set; }

        public int Priority => 1;

        public void OnInit() {
            InitializeLuaEnv();
            Startup();
        }

        public void OnClose() {
        }

        public void OnUpdate() {
        }

        public void OnFixedUpdate() {
        }

        #region Public interface

        /// <summary>
        /// 载入 Lua 脚本
        /// </summary>
        /// <param name="scriptName">脚本名称</param>
        void LoadScript(string scriptName) {
            SafeDoString($"require '{scriptName}'");
        }

        /// <summary>
        /// 以安全的方式调用 DoString
        /// </summary>
        /// <param name="scriptContent">脚本内容</param>
        public void SafeDoString(string scriptContent) {
            try {
                LuaEnv?.DoString(scriptContent);
            }
            catch (System.Exception ex) {
                Log.Error($"xLua Manager exception : {ex.Message}\n {ex.StackTrace}");
            }
        }

        /// <summary>
        /// 重新加载 Lua 脚本
        /// </summary>
        /// <param name="scriptName">脚本名称</param>
        public void ReloadScript(string scriptName) {
            SafeDoString($"package.loaded['{scriptName}'] = nil]");
            LoadScript(scriptName);
        }

        #endregion

        private void InitializeLuaEnv() {
            LuaEnv = new LuaEnv();
            LuaEnv.AddLoader(XLuaLoader);
        }

        private void Startup() {
            LoadScript("GameMain");
        }

        /// <summary>
        /// 自定义 Lua Loader
        /// </summary>
        /// <param name="filepath"></param>
        /// <returns></returns>
        private static byte[] XLuaLoader(ref string filepath) {
            var luaTextAsset = GameFrameworkCore.GetModule<ResourceManager>().Asset
                .LoadAsset<TextAsset>(filepath);

            if (luaTextAsset == null) {
                Log.Error($"Failed to load Lua script [{filepath}], you should preload Lua script first.");
                return null;
            }
            else {
                return luaTextAsset.bytes;
            }
        }
    }
}