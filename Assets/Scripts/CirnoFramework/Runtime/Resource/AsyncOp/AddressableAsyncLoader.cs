using System;
using System.Collections.Generic;
using CirnoFramework.Runtime.Utility;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using XLua;
using Object = UnityEngine.Object;

namespace CirnoFramework.Runtime.Resource.AsyncOp {
    [LuaCallCSharp]
    public class AddressableAsyncLoader : BaseAssetAsyncLoader {
        private static readonly Queue<AddressableAsyncLoader> Pool = new Queue<AddressableAsyncLoader>();
        private static int _sequence = 0;

        public int Sequence { get; protected set; }
        public Type AssetType { get; protected set; }
        public string AddressPath { get; protected set; }

        protected bool IsOver = false;

        private dynamic _handle;

        public static AddressableAsyncLoader Get() {
            return Pool.Count > 0 ? Pool.Dequeue() : new AddressableAsyncLoader(++_sequence);
        }

        public static void Recycle(AddressableAsyncLoader creator) {
            Pool.Enqueue(creator);
        }

        public AddressableAsyncLoader(int sequence) {
            Sequence = sequence;
        }

        public void Init<T>(string addressPath, Type assetType) where T : Object {
            Asset = null;
            IsOver = false;
            AssetType = assetType;
            AddressPath = addressPath;

            _handle = Addressables.LoadAssetAsync<T>(addressPath);
        }

        public override void Update() {
            if (isDone) return;

            _handle.Status = "123";

            //if (_handle.Status == AsyncOperationStatus.Succeeded) {
            //    Asset = _handle.Result as Object;
            //    IsOver = true;
            //}

            //if (_handle.Status == AsyncOperationStatus.Failed) {
            //    Log.Error($"Load asset:{AddressPath} error: {_handle.Status}");
            //    IsOver = true;
            //}
        }

        protected override bool IsDone() {
            return IsOver;
        }

        protected override float Progress() {
            return isDone ? 1.0f : 0.0f;
        }

        public override void Dispose() {
            Recycle(this);
        }
    }
}