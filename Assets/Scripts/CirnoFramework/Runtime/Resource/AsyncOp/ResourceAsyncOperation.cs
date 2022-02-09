using System;
using System.Collections;
using UnityEngine;

namespace CirnoFramework.Runtime.Resource.AsyncOp {
    /// <summary>
    /// 功能：异步操作抽象基类，继承自IEnumerator接口，支持迭代，主要是为了让异步操作能够适用于协程操作
    /// 注意：提供对协程操作的支持，但是异步操作的进行不依赖于协程，可以在Update等函数中查看进度值
    /// </summary>
    public abstract class ResourceAsyncOperation : IEnumerator, IDisposable {
        public object Current {
            get { return null; }
        }

        public bool isDone {
            get { return IsDone(); }
        }

        public float progress {
            get { return Progress(); }
        }

        public abstract void Update();

        public bool MoveNext() {
            return !IsDone();
        }

        public void Reset() {
        }

        protected abstract bool IsDone();

        protected abstract float Progress();

        public virtual void Dispose() {
        }
    }

    public abstract class BaseAssetBundleAsyncLoader : ResourceAsyncOperation {
        public string AssetBundleName { get; protected set; }

        public AssetBundle AssetBundle { get; protected set; }

        public override void Dispose() {
            AssetBundleName = null;
            AssetBundle = null;
        }
    }

    public abstract class BaseAssetAsyncLoader : ResourceAsyncOperation {
        public UnityEngine.Object Asset { get; protected set; }

        public override void Dispose() {
            Asset = null;
        }
    }
}