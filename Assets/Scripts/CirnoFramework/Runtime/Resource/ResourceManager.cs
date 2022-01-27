using CirnoFramework.Runtime.Base;
using CirnoFramework.Runtime.Resource.Base;

namespace CirnoFramework.Runtime.Resource {
    public class ResourceManager : IGameFrameworkModule {
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

        #endregion

        public ResourceManager() {
            
        }

        public void OnInit() {

        }

        public void OnClose() {
        }
    }
}