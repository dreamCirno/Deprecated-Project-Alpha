using System;
using System.Threading.Tasks;

namespace CirnoFramework.Runtime.Resource.Base {
    public abstract class ResourceVersion {
        public T Cast<T>() where T : ResourceVersion {
            return this as T;
        }

        public abstract Task<bool> Initialize();

        public abstract void CheckUpdate(Action<bool> needUpdate);

        public abstract void UpdateResource(Action<float, double, double, float> callback, Action downloadComplete,
            Action<string, string> errorCallback, string label = null);
    }
}