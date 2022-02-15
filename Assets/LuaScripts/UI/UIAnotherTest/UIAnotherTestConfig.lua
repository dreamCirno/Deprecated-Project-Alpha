--[[
-- added by wsh @ 2018-02-26
-- UIAnotherTest模块窗口配置，要使用还需要导出到UI.Config.UIConfig.lua
--]]

-- 窗口配置
local UIAnotherTest = {
    Name = UIWindowNames.UIAnotherTest,
    Layer = UILayers.BackgroundLayer,
    View = require "UI.UIAnotherTest.View.UIAnotherTestView",
    ViewModel = require "UI.UIAnotherTest.ViewModel.UIAnotherTestViewModel",
    PrefabPath = "Prefabs/UIAnotherTestView.prefab",
}

return {
    -- 配置
    UIAnotherTest = UIAnotherTest,
}