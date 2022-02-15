using System;
using CirnoFramework.Runtime;
using CirnoFramework.Runtime.Utility;
using GameFramework;
using UnityEngine;

/// <summary>
/// 游戏入口。
/// </summary>
public partial class GameCore : MonoBehaviour {
    private void Awake() {
        DontDestroyOnLoad(gameObject);
        InitLogHelper();
    }

    private void Start() {
        InitBuiltinComponents();
        InitCustomComponents();
    }

    private void Update() {
        GameFrameworkCore.Update();

        if (Input.GetKeyDown(KeyCode.K)) {
            XLua.SafeDoString($"UIManager:GetInstance():OpenWindow(UIWindowNames.UIStudyMain)");
        }

        if (Input.GetKeyDown(KeyCode.L)) {
            Resource.DestroyAll();
        }

        if (Input.GetKeyDown(KeyCode.Keypad1)) {
            XLua.SafeDoString("UIManager:GetInstance():OpenWindow(UIWindowNames.UIStudyMain)");
        }

        if (Input.GetKeyDown(KeyCode.Keypad2)) {
            XLua.SafeDoString("UIManager:GetInstance():OpenWindow(UIWindowNames.UIAnotherTest)");
        }

        if (Input.GetKeyDown(KeyCode.Keypad3)) {
            XLua.SafeDoString("UIManager:GetInstance():OpenWindow(UIWindowNames.UIAnotherTest2)");
        }

        if (Input.GetKeyDown(KeyCode.Keypad4)) {
            XLua.SafeDoString("UIManager:GetInstance():CloseWindow(UIWindowNames.UIStudyMain)");
        }

        if (Input.GetKeyDown(KeyCode.Keypad5)) {
            XLua.SafeDoString("UIManager:GetInstance():CloseWindow(UIWindowNames.UIAnotherTest)");
        }

        if (Input.GetKeyDown(KeyCode.Keypad6)) {
            XLua.SafeDoString("UIManager:GetInstance():CloseWindow(UIWindowNames.UIAnotherTest2)");
        }
    }

    private void LateUpdate() {
        GameFrameworkCore.LateUpdate();
    }

    private void FixedUpdate() {
        GameFrameworkCore.FixedUpdate();
    }

    private void OnDestroy() {
        GameFrameworkCore.ShutDown();
        Log.Info($"{nameof(GameFrameworkCore)} ShutDown.");
    }

    private void InitLogHelper() {
        var logHelperType = typeof(DefaultLogHelper);

        GameFrameworkLog.ILogHelper logHelper =
            (GameFrameworkLog.ILogHelper) Activator.CreateInstance(logHelperType);

        GameFrameworkLog.SetLogHelper(logHelper);
    }
}