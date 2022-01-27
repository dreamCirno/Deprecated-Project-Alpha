using CirnoFramework.Runtime.Base;

namespace CirnoFramework.Runtime.XLua {
    public class XLuaManager : IGameFrameworkModule, IUpdatable, IFixedUpdatable {
        public int Priority => 1;

        public void OnInit() {
        }

        public void OnClose() {
        }

        public void OnUpdate() {
        }

        public void OnFixedUpdate() {
        }
    }
}