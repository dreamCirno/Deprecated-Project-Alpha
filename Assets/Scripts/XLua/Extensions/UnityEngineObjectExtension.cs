namespace XLua.Extensions {
    [LuaCallCSharp]
    [ReflectionUse]
    public static class UnityEngineObjectExtension {
        public static bool IsNull(this UnityEngine.Object o) {
            return o == null;
        }
    }
}