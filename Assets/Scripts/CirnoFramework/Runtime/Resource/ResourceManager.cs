using CirnoFramework.Runtime.Base;
using CirnoFramework.Runtime.Resource.Base;
using CirnoFramework.Runtime.Resource.Impl.Addressable;

namespace CirnoFramework.Runtime.Resource {
    public class ResourceManager : IGameFrameworkModule, IUpdatable {
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

        #region Public interface

        public void SetResourceHelper(IAssetsHelper resourceHelper) {
            Asset?.Clear();
            Asset = resourceHelper;
        }

        #endregion

        #region Implement

        public void OnInit() {
            Asset = new AddressableAssetsHelper();
            Version = new AddressableVersion();
        }

        public void OnUpdate() {
            
        }

        public void OnClose() {
            Asset?.Clear();
        }

        #endregion
    }
}