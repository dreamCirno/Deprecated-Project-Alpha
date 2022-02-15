--[[
-- added by wsh @ 2018-02-26
-- UITestMain视图层
--]]

local UIStudyMainView = BaseClass("UIStudyMainView", UIBaseView)
local base = UIBaseView

-- 各个组件路径
local hp_text_path = "ContentRoot/LoadingDesc"
local test_button_text_path = "ContentRoot/TestButton/Text"

local test_button_path = "ContentRoot/TestButton"
local test_uieffect2_path = "ContentRoot/TestButton"

local bg_image_path = "BgRoot/BG"

local function OnCreate(self)
    base.OnCreate(self)
    -- 初始化各个组件
    self.hp_text = self:AddComponent(UIText, hp_text_path, self.Binder, "hp_text")
    self.test_button_text = self:AddComponent(UIText, test_button_text_path, self.Binder, "test_button_text")

    self.test_button = self:AddComponent(UIButton, test_button_path, self.Binder, "test_button")

    local effect2_config = EffectConfig.UITaskFinish
    self.test_effect2 = self:AddComponent(UIEffect, test_uieffect2_path, 1, effect2_config)

    self.bg_image = self:AddComponent(UIImage, bg_image_path, AtlasConfig.Sample, self.Binder, "bg_image")

    -- 调用父类Bind所有属性
    base.BindAll(self)
end

local function OnEnable(self)
    base.OnEnable(self)
end

local function OnDestroy(self)
    base.OnDestroy(self)
end

UIStudyMainView.OnCreate = OnCreate
UIStudyMainView.OnEnable = OnEnable
UIStudyMainView.OnDestroy = OnDestroy

return UIStudyMainView