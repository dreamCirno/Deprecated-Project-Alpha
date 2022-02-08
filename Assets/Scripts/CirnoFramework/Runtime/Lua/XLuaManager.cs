using CirnoFramework.Runtime.Base;
using CirnoFramework.Runtime.Resource;
using CirnoFramework.Runtime.Utility;
using UnityEngine;
using XLua;

namespace CirnoFramework.Runtime.Lua {
    public class XLuaManager : IGameFrameworkModule, IUpdatable, IFixedUpdatable {
        private const string CommonMainScriptName = "Common.Main";
        private const string GameMainScriptName = "GameMain";
        private const string HotfixMainScriptName = "XLua.HotfixMain";

        public LuaEnv LuaEnv { get; private set; }

        public int Priority => 2;

        public void OnInit() {
            InitializeLuaEnv();
            LoadScript(CommonMainScriptName);
        }

        public void OnClose() {
        }

        public void OnRestart() {
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
            Log.Info($"SafeDoString: {scriptContent}");
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


        public void StartHotfix(bool restart = false) {
            if (LuaEnv == null) {
                return;
            }

            if (restart) {
                StopHotfix();
                ReloadScript(HotfixMainScriptName);
            }
            else {
                LoadScript(HotfixMainScriptName);
            }

            SafeDoString("HotfixMain.Start()");
        }

        public void StopHotfix() {
            SafeDoString("HotfixMain.Stop()");
        }

        public void StartGame() {
            LoadScript(GameMainScriptName);
            SafeDoString($"{GameMainScriptName}.Start()");
        }

        #endregion

        private void InitializeLuaEnv() {
            LuaEnv = new LuaEnv();
            LuaEnv.AddLoader(XLuaLoader);
            // 注册 ProtoBuffer Lua
            LuaEnv.AddBuildin("pb", Lua.LoadPb);
            // 注册 RapidJson
            LuaEnv.AddBuildin("rapidjson", Lua.LoadRapidJson);
        }

        /// <summary>
        /// 自定义 Lua Loader
        /// </summary>
        /// <param name="filepath"></param>
        /// <returns></returns>
        private static byte[] XLuaLoader(ref string filepath) {
            filepath = filepath.Replace('.', '/');
            var virtualPath = $"Assets/LuaScripts/{filepath}.lua";
            var luaTextAsset = GameFrameworkCore.GetModule<ResourceManager>().Asset
                .LoadAsset<TextAsset>(virtualPath);

            if (luaTextAsset == null) {
                Log.Error($"Failed to load Lua script [{virtualPath}], you should preload Lua script first.");
                return null;
            }
            else {
                Log.Debug($"Lua script loaded [{virtualPath}].");
                return luaTextAsset.bytes;
            }
        }
    }
}