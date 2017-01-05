require "Common/define"
require "Common/protocal"
require "Common/functions"
Event = require 'events'
require "3rd/pbc/protobuf"

local transform;
local gameObject;

RegisterAccountPanelCtrl = {};
local this = RegisterAccountPanelCtrl;

--构建函数--
function RegisterAccountPanelCtrl.New()
	return this;
end

function RegisterAccountPanelCtrl.Awake(xpage)
    --xpage.m_pageType = EPageType.Normal;
    --xpage.m_pageMode = EPageMode.DoNothing;
    xpage.m_pageType = EPageType.PopUp;
    xpage.m_pageMode = EPageMode.HideOtherAndNeedBack
end

function RegisterAccountPanelCtrl.Start()
    log('RegisterAccountPanelCtrl Start--->>>');
   -- Event.AddListener(Protocal.LoginResponse, this.OnLoginResponse); 
    EventTriggerListener.Get(RegisterAccountPanelView.returnBtn.gameObject):AddClick(RegisterAccountPanelView.returnBtn,this.OnReturnBtnClick);
    EventTriggerListener.Get(RegisterAccountPanelView.registerSoonBtn.gameObject):AddClick(RegisterAccountPanelView.registerSoonBtn,this.OnRegisterSoonBtnClick);

end

function RegisterAccountPanelCtrl.Rest()
    log('RegisterAccountPanelCtrl Rest--->>>');
    RegisterAccountPanelView.registerInput.text = "";
    RegisterAccountPanelView.passwordInput.text = "";
    RegisterAccountPanelView.confirmInput.text = "";
end

function RegisterAccountPanelCtrl.Hide()
    log('RegisterAccountPanelCtrl Hide--->>>');
end

function RegisterAccountPanelCtrl.Destroy()
    log('RegisterAccountPanelCtrl Destroy--->>>');
    --Event.RemoveListener(Protocal.LoginResponse);
    EventTriggerListener.Get(RegisterAccountPanelView.returnBtn.gameObject):RemoveClick(RegisterAccountPanelView.returnBtn,this.OnReturnBtnClick);
    EventTriggerListener.Get(RegisterAccountPanelView.registerSoonBtn.gameObject):RemoveClick(RegisterAccountPanelView.registerSoonBtn,this.OnRegisterSoonBtnClick);
end
---------------------------------------------------------------------------------------
--后退按钮点击--
function RegisterAccountPanelCtrl.OnReturnBtnClick( go )
    xpageMgr:ShowPage(true,"UI/Prefab/LoginPanel");
end

function RegisterAccountPanelCtrl.OnRegisterSoonBtnClick( go )
    log("OnRegisterSoonBtnClick click...............")
end