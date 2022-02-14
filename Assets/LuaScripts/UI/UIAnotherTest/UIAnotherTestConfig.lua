--[[
-- added by wsh @ 2018-02-26
-- UITestMain模块窗口配置，要使用还需要导出到UI.Config.UIConfig.lua
--]]

-- 窗口配置
local UIAnotherTest = {
    Name = UIWindowNames.UIAnotherTest,
    Layer = UILayers.TopLayer,
    View = nil,
    ViewModel = nil,
    PrefabPath = "UI/Prefabs/View/UITestMain.prefab",
}

return {
    -- 配置
    UITestMain = UITestMain,
}