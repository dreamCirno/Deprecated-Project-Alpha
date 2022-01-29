using System.Runtime.InteropServices;
using XLua;

namespace CirnoFramework.Runtime.Lua {
    public partial class Lua {
        private const string LUADLL = "xlua";

        [DllImport(LUADLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern int luaopen_rapidjson(System.IntPtr L);


        [MonoPInvokeCallback(typeof(XLua.LuaDLL.lua_CSFunction))]
        public static int LoadRapidJson(System.IntPtr L) {
            return luaopen_rapidjson(L);
        }

        [DllImport(LUADLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern int luaopen_lpeg(System.IntPtr L);

        [MonoPInvokeCallback(typeof(XLua.LuaDLL.lua_CSFunction))]
        public static int LoadLpeg(System.IntPtr L) {
            return luaopen_lpeg(L);
        }


        [DllImport(LUADLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern int luaopen_pb(System.IntPtr L);

        [MonoPInvokeCallback(typeof(XLua.LuaDLL.lua_CSFunction))]
        public static int LoadPb(System.IntPtr L) {
            return luaopen_pb(L);
        }
    }
}