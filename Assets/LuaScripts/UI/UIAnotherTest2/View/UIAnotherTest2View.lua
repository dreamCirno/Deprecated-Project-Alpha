--[[
-- added by wsh @ 2018-02-26
-- UITestMain视图层
--]]

local UIAnotherTest2View = BaseClass("UIAnotherTest2View", UIBaseView)
local base = UIBaseView

local function OnCreate(self)
    base.OnCreate(self)

    -- 调用父类Bind所有属性
    base.BindAll(self)
end

local function OnEnable(self)
    base.OnEnable(self)
end

local function OnDestroy(self)
    base.OnDestroy(self)
end

UIAnotherTest2View.OnCreate = OnCreate
UIAnotherTest2View.OnEnable = OnEnable
UIAnotherTest2View.OnDestroy = OnDestroy

return UIAnotherTest2View