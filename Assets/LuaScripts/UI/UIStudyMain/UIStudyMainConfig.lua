--[[
-- added by wsh @ 2018-02-26
-- UIStudyMain模块窗口配置，要使用还需要导出到UI.Config.UIConfig.lua
--]]

-- 窗口配置
local UIStudyMain = {
    Name = UIWindowNames.UIStudyMain,
    Layer = UILayers.BackgroundLayer,
    View = require "UI.UIStudyMain.View.UIStudyMainView",
    ViewModel = require "UI.UIStudyMain.ViewModel.UIStudyMainViewModel",
    PrefabPath = "Prefabs/UILoading.prefab",
}

return {
    -- 配置
    UIStudyMain = UIStudyMain,
}