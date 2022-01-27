namespace CirnoFramework.Runtime.Base {
    /// <summary>
    /// 游戏框架模块
    /// </summary>
    public interface IGameFrameworkModule {
        int Priority { get; }
        void OnInit();
        void OnClose();
    }
}