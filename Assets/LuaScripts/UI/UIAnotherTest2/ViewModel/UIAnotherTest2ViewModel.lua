--[[
-- added by wsh @ 2017-12-01
-- UILogin视图层
-- 注意：
-- 1、成员变量最好预先在__init函数声明，提高代码可读性
-- 2、OnEnable函数每次在窗口打开时调用，直接刷新
-- 3、组件命名参考代码规范
--]]


local UIAnotherTest2ViewModel = BaseClass("UIAnotherTest2ViewModel", UIBaseViewModel)
local base = UIBaseViewModel

local function OnCreate(self)

end

-- 打开
local function OnEnable(self)
    base.OnEnable(self)
end

-- 关闭
local function OnDisable(self)
    base.OnDisable(self)
    -- 清理成员变量
end

-- 销毁
local function OnDistroy(self)
    base.OnDistroy(self)
    -- 清理成员变量
end

UIAnotherTest2ViewModel.OnCreate = OnCreate
UIAnotherTest2ViewModel.OnEnable = OnEnable
UIAnotherTest2ViewModel.OnDisable = OnDisable
UIAnotherTest2ViewModel.OnDistroy = OnDistroy

return UIAnotherTest2ViewModel