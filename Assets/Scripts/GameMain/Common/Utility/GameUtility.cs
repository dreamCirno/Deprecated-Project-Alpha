using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using CirnoFramework.Runtime.Utility;
using UnityEngine;
using UnityEngine.EventSystems;
using XLua;

/// <summary>
/// 功能：通用静态方法
/// </summary>
[Hotfix]
public class GameUtility {
    public const string AssetsFolderName = "Assets";

    public static string FormatToUnityPath(string path) {
        return path.Replace("\\", "/");
    }

    public static string FormatToSysFilePath(string path) {
        return path.Replace("/", "\\");
    }

    /// <summary>
    /// 获取当前平台
    /// </summary>
    /// <returns></returns>
    public static string GetPlatform() {
        switch (Application.platform) {
            case RuntimePlatform.Android:
                return "Android";
            case RuntimePlatform.IPhonePlayer:
                return "IOS";
            default:
                return "Windows";
        }
    }

    /// <summary>
    /// 是否是移动端
    /// </summary>
    /// <returns></returns>
    public static bool IsMobile() {
        return Application.isMobilePlatform;
    }

    /// <summary>
    /// 获取当前网络
    /// </summary>
    /// <returns>0：无网，1：移动网，2：wifi</returns>
    public static int GetInternet() {
        return (int) Application.internetReachability;
    }

    /// <summary>
    /// 手机永不休眠
    /// </summary>
    public static void NeverSleep() {
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
    }

    /// <summary>
    /// 获取时间戳
    /// </summary>
    /// <returns></returns>
    public static string GetTimeStamp() {
        return new DateTimeOffset(DateTime.UtcNow).ToUnixTimeSeconds().ToString();
    }

    /// <summary>
    /// 判断手指是否点击UI，用于UI遮挡
    /// </summary>
    public static bool IsPointerOverGameObject() {
#if UNITY_EDITOR
        return EventSystem.current.IsPointerOverGameObject();
#elif UNITY_ANDROID || UNITY_IOS
        return EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId);
#endif
    }

    /// <summary>
    /// 获取手机点击的UI，用于判断指定UI遮挡
    /// </summary>
    public static bool GetPointerOverGameObject(out GameObject pressObject) {
        pressObject = null;

        if (IsPointerOverGameObject()) {
            PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
            eventDataCurrentPosition.position = new Vector2(
#if UNITY_EDITOR
                Input.mousePosition.x, Input.mousePosition.y
#elif UNITY_ANDROID || UNITY_IOS
           Input.touchCount > 0 ? Input.GetTouch(0).position.x : 0, Input.touchCount > 0 ? Input.GetTouch(0).position.y : 0
#endif
            );

            List<RaycastResult> results = new List<RaycastResult>();
            EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
            if (results.Count > 0) {
                pressObject = results[0].gameObject;
                return true;
            }
        }

        return false;
    }

    /// <summary>
    /// 递归查找子节点
    /// </summary>
    /// <param name="parent">父节点</param>
    /// <param name="name">子节点名字</param>
    /// <returns>子节点Transform</returns>
    public static Transform FindChildRecursion(Transform parent, string name) {
        foreach (Transform child in parent) {
            if (child.name == name) {
                return child;
            }
            else {
                Transform ret = FindChildRecursion(child, name);
                if (ret != null)
                    return ret;
            }
        }

        return null;
    }

    public static string FullPathToAssetPath(string full_path) {
        full_path = FormatToUnityPath(full_path);
        if (!full_path.StartsWith(Application.dataPath)) {
            return null;
        }

        string ret_path = full_path.Replace(Application.dataPath, "");
        return AssetsFolderName + ret_path;
    }

    public static string GetFileExtension(string path) {
        return Path.GetExtension(path).ToLower();
    }

    public static string[] GetSpecifyFilesInFolder(string path, string[] extensions = null, bool exclude = false) {
        if (string.IsNullOrEmpty(path)) {
            return null;
        }

        if (extensions == null) {
            return Directory.GetFiles(path, "*.*", SearchOption.AllDirectories);
        }
        else if (exclude) {
            return Directory.GetFiles(path, "*.*", SearchOption.AllDirectories)
                .Where(f => !extensions.Contains(GetFileExtension(f))).ToArray();
        }
        else {
            return Directory.GetFiles(path, "*.*", SearchOption.AllDirectories)
                .Where(f => extensions.Contains(GetFileExtension(f))).ToArray();
        }
    }

    public static string[] GetSpecifyFilesInFolder(string path, string pattern) {
        if (string.IsNullOrEmpty(path)) {
            return null;
        }

        return Directory.GetFiles(path, pattern, SearchOption.AllDirectories);
    }

    public static string[] GetAllFilesInFolder(string path) {
        return GetSpecifyFilesInFolder(path);
    }

    public static string[] GetAllDirsInFolder(string path) {
        return Directory.GetDirectories(path, "*", SearchOption.AllDirectories);
    }

    public static void CheckFileAndCreateDirWhenNeeded(string filePath) {
        if (string.IsNullOrEmpty(filePath)) {
            return;
        }

        FileInfo file_info = new FileInfo(filePath);
        DirectoryInfo dir_info = file_info.Directory;
        if (!dir_info.Exists) {
            Directory.CreateDirectory(dir_info.FullName);
        }
    }

    public static void CheckDirAndCreateWhenNeeded(string folderPath) {
        if (string.IsNullOrEmpty(folderPath)) {
            return;
        }

        if (!Directory.Exists(folderPath)) {
            Directory.CreateDirectory(folderPath);
        }
    }

    public static bool SafeWriteAllBytes(string outFile, byte[] outBytes) {
        try {
            if (string.IsNullOrEmpty(outFile)) {
                return false;
            }

            CheckFileAndCreateDirWhenNeeded(outFile);
            if (File.Exists(outFile)) {
                File.SetAttributes(outFile, FileAttributes.Normal);
            }

            File.WriteAllBytes(outFile, outBytes);
            return true;
        }
        catch (System.Exception ex) {
            Log.Error($"SafeWriteAllBytes failed! path = {outFile} with err = {ex.Message}");
            return false;
        }
    }

    public static bool SafeWriteAllLines(string outFile, string[] outLines) {
        try {
            if (string.IsNullOrEmpty(outFile)) {
                return false;
            }

            CheckFileAndCreateDirWhenNeeded(outFile);
            if (File.Exists(outFile)) {
                File.SetAttributes(outFile, FileAttributes.Normal);
            }

            File.WriteAllLines(outFile, outLines);
            return true;
        }
        catch (System.Exception ex) {
            Log.Error($"SafeWriteAllLines failed! path = {outFile} with err = {ex.Message}");
            return false;
        }
    }

    public static bool SafeWriteAllText(string outFile, string text) {
        try {
            if (string.IsNullOrEmpty(outFile)) {
                return false;
            }

            CheckFileAndCreateDirWhenNeeded(outFile);
            if (File.Exists(outFile)) {
                File.SetAttributes(outFile, FileAttributes.Normal);
            }

            File.WriteAllText(outFile, text);
            return true;
        }
        catch (System.Exception ex) {
            Log.Error(
                $"SafeWriteAllText failed! path = {outFile} with err = {ex.Message}");
            return false;
        }
    }

    public static byte[] SafeReadAllBytes(string inFile) {
        try {
            if (string.IsNullOrEmpty(inFile)) {
                return null;
            }

            if (!File.Exists(inFile)) {
                return null;
            }

            File.SetAttributes(inFile, FileAttributes.Normal);
            return File.ReadAllBytes(inFile);
        }
        catch (System.Exception ex) {
            Log.Error($"SafeReadAllBytes failed! path = {inFile} with err = {ex.Message}");
            return null;
        }
    }

    public static string[] SafeReadAllLines(string inFile) {
        try {
            if (string.IsNullOrEmpty(inFile)) {
                return null;
            }

            if (!File.Exists(inFile)) {
                return null;
            }

            File.SetAttributes(inFile, FileAttributes.Normal);
            return File.ReadAllLines(inFile);
        }
        catch (System.Exception ex) {
            Log.Error($"SafeReadAllLines failed! path = {inFile} with err = {ex.Message}");
            return null;
        }
    }

    public static string SafeReadAllText(string inFile) {
        try {
            if (string.IsNullOrEmpty(inFile)) {
                return null;
            }

            if (!File.Exists(inFile)) {
                return null;
            }

            File.SetAttributes(inFile, FileAttributes.Normal);
            return File.ReadAllText(inFile);
        }
        catch (System.Exception ex) {
            Log.Error($"SafeReadAllText failed! path = {inFile} with err = {ex.Message}");
            return null;
        }
    }

    public static void DeleteDirectory(string dirPath) {
        string[] files = Directory.GetFiles(dirPath);
        string[] dirs = Directory.GetDirectories(dirPath);

        foreach (string file in files) {
            File.SetAttributes(file, FileAttributes.Normal);
            File.Delete(file);
        }

        foreach (string dir in dirs) {
            DeleteDirectory(dir);
        }

        Directory.Delete(dirPath, false);
    }

    public static bool SafeClearDir(string folderPath) {
        try {
            if (string.IsNullOrEmpty(folderPath)) {
                return true;
            }

            if (Directory.Exists(folderPath)) {
                DeleteDirectory(folderPath);
            }

            Directory.CreateDirectory(folderPath);
            return true;
        }
        catch (System.Exception ex) {
            Log.Error($"SafeClearDir failed! path = {folderPath} with err = {ex.Message}");
            return false;
        }
    }

    public static bool SafeDeleteDir(string folderPath) {
        try {
            if (string.IsNullOrEmpty(folderPath)) {
                return true;
            }

            if (Directory.Exists(folderPath)) {
                DeleteDirectory(folderPath);
            }

            return true;
        }
        catch (System.Exception ex) {
            Log.Error($"SafeDeleteDir failed! path = {folderPath} with err: {ex.Message}");
            return false;
        }
    }

    public static bool SafeDeleteFile(string filePath) {
        try {
            if (string.IsNullOrEmpty(filePath)) {
                return true;
            }

            if (!File.Exists(filePath)) {
                return true;
            }

            File.SetAttributes(filePath, FileAttributes.Normal);
            File.Delete(filePath);
            return true;
        }
        catch (System.Exception ex) {
            Log.Error($"SafeDeleteFile failed! path = {filePath} with err: {ex.Message}");
            return false;
        }
    }

    public static bool SafeRenameFile(string sourceFileName, string destFileName) {
        try {
            if (string.IsNullOrEmpty(sourceFileName)) {
                return false;
            }

            if (!File.Exists(sourceFileName)) {
                return true;
            }

            SafeDeleteFile(destFileName);
            File.SetAttributes(sourceFileName, FileAttributes.Normal);
            File.Move(sourceFileName, destFileName);
            return true;
        }
        catch (System.Exception ex) {
            Log.Error($"SafeRenameFile failed! path = {sourceFileName} with err: {ex.Message}");
            return false;
        }
    }

    public static bool SafeCopyFile(string fromFile, string toFile) {
        try {
            if (string.IsNullOrEmpty(fromFile)) {
                return false;
            }

            if (!File.Exists(fromFile)) {
                return false;
            }

            CheckFileAndCreateDirWhenNeeded(toFile);
            SafeDeleteFile(toFile);
            File.Copy(fromFile, toFile, true);
            return true;
        }
        catch (System.Exception ex) {
            Log.Error($"SafeCopyFile failed! formFile = {fromFile}, toFile = {toFile}, with err = {ex.Message}");
            return false;
        }
    }
}