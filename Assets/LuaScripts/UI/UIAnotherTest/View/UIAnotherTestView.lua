--[[
-- added by wsh @ 2018-02-26
-- UITestMain视图层
--]]

local UIAnotherTestView = BaseClass("UIAnotherTestView", UIBaseView)
local base = UIBaseView

-- 各个组件路径
local test_button_path = "ContentRoot/TestButton"

local function OnCreate(self)
    base.OnCreate(self)
    -- 初始化各个组件
    self.test_button = self:AddComponent(UIButton, test_button_path, self.Binder, "test_button")

    -- 调用父类Bind所有属性
    base.BindAll(self)
end

local function OnEnable(self)
    base.OnEnable(self)
end

local function OnDestroy(self)
    base.OnDestroy(self)
end

UIAnotherTestView.OnCreate = OnCreate
UIAnotherTestView.OnEnable = OnEnable
UIAnotherTestView.OnDestroy = OnDestroy

return UIAnotherTestView