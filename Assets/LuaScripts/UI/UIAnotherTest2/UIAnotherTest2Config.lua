--[[
-- added by wsh @ 2018-02-26
-- UIStudyMain模块窗口配置，要使用还需要导出到UI.Config.UIConfig.lua
--]]

-- 窗口配置
local UIAnotherTest2 = {
    Name = UIWindowNames.UIAnotherTest2,
    Layer = UILayers.BackgroundLayer,
    View = require "UI.UIAnotherTest2.View.UIAnotherTest2View",
    ViewModel = require "UI.UIAnotherTest2.ViewModel.UIAnotherTest2ViewModel",
    PrefabPath = "Prefabs/UIAnotherTestView1.prefab",
}

return {
    -- 配置
    UIAnotherTest2 = UIAnotherTest2,
}