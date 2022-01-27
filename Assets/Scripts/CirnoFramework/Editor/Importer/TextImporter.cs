using System.IO;
using UnityEngine;

namespace CirnoFramework.Editor.Importer {
    [UnityEditor.AssetImporters.ScriptedImporter(1, new string[] {"lua", "pb", "proto"})]
    public class TextImporter : UnityEditor.AssetImporters.ScriptedImporter {
        public override void OnImportAsset(UnityEditor.AssetImporters.AssetImportContext ctx) {
            var asset = new TextAsset(File.ReadAllText(ctx.assetPath));
            ctx.AddObjectToAsset("MainText", asset);
            ctx.SetMainObject(asset);
        }
    }
}