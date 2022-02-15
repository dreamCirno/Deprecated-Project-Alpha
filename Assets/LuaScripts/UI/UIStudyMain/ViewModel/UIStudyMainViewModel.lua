--[[
-- added by wsh @ 2017-12-01
-- UILogin视图层
-- 注意：
-- 1、成员变量最好预先在__init函数声明，提高代码可读性
-- 2、OnEnable函数每次在窗口打开时调用，直接刷新
-- 3、组件命名参考代码规范
--]]


local UIStudyMainViewModel = BaseClass("UIStudyMainViewModel", UIBaseViewModel)
local base = UIBaseViewModel

local function OnCreate(self)
    self.hp_text = BindableProperty.New(0)
    self.timer_action = function(self)
        self.hp_text.Value = self.hp_text.Value + 1
    end
    self.timer = TimerManager:GetInstance():GetTimer(1, self.timer_action, self)
    -- 启动定时器
    self.timer:Start()

    self.test_button_text = BindableProperty.New(0)
    self.test_button = {
        OnClick = function()
            UIManager:GetInstance():CloseWindow(UIWindowNames.UIStudyMain)
            UIManager:GetInstance():OpenWindow(UIWindowNames.UIAnotherTest)
            UIManager:GetInstance():OpenWindow(UIWindowNames.UIAnotherTest2)
        end
    }

    self.bg_image = BindableProperty.New("Sample")
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

UIStudyMainViewModel.OnCreate = OnCreate
UIStudyMainViewModel.OnEnable = OnEnable
UIStudyMainViewModel.OnDisable = OnDisable
UIStudyMainViewModel.OnDistroy = OnDistroy

return UIStudyMainViewModel