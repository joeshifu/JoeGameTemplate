require "Common/define"
require "Common/protocal"
require "Common/functions"
Event = require 'events'
require "3rd/pbc/protobuf"

local transform;
local gameObject;

LoginPanelCtrl = {};
local this = LoginPanelCtrl;

--构建函数--
function LoginPanelCtrl.New()
	return this;
end

function LoginPanelCtrl.Awake(xpage)
    --logWarn('LoginPanelCtrl Awake--->>>'..'xpage name:'..xpage.m_pageName);
    --xpage.m_pageType = EPageType.Normal;
    --xpage.m_pageMode = EPageMode.DoNothing;
    xpage.m_pageType = EPageType.PopUp;
    xpage.m_pageMode = EPageMode.HideOtherAndNeedBack
    EventDispatcher.AddEventListener("test", this.OnEventTest);
    --EventDispatcher.AddEventListener<int>("testint", this.OnEventTestInt);
end

function LoginPanelCtrl.Start()
    log('LoginPanelCtrl Start--->>>');
    Event.AddListener(Protocal.LoginResponse, this.OnLoginResponse); 
    EventTriggerListener.Get(LoginPanelView.accountLoginBtn.gameObject):AddClick(LoginPanelView.accountLoginBtn,this.OnAccountLoginBtnClick);
    EventTriggerListener.Get(LoginPanelView.visitorLoginBtn.gameObject):AddClick(LoginPanelView.visitorLoginBtn,this.OnVisitorLoginBtnClick);
    EventTriggerListener.Get(LoginPanelView.registerBtn.gameObject):AddClick(LoginPanelView.registerBtn,this.OnRegisterBtnClick);
    EventTriggerListener.Get(LoginPanelView.moreLoginWayBtn.gameObject):AddClick(LoginPanelView.moreLoginWayBtn,this.OnMoreLoginWayBtnClick);
    EventTriggerListener.Get(LoginPanelView.wechatLoginBtn.gameObject):AddClick(LoginPanelView.wechatLoginBtn,this.OnWechatLoginBtnClick);
    EventTriggerListener.Get(LoginPanelView.thirdLoginMaskBtn.gameObject):AddClick(LoginPanelView.thirdLoginMaskBtn,this.OnThirdLoginMaskBtnClick);
end

function LoginPanelCtrl.Rest()
    log('LoginPanelCtrl Rest--->>>');
    LoginPanelView.userName.text = "";
    LoginPanelView.passWord.text = "";
end

function LoginPanelCtrl.Hide()
    log('LoginPanelCtrl Hide--->>>');
end

function LoginPanelCtrl.Destroy()
    log('LoginPanelCtrl Destroy--->>>');
    Event.RemoveListener(Protocal.LoginResponse);
    EventTriggerListener.Get(LoginPanelView.accountLoginBtn.gameObject):RemoveClick(LoginPanelView.accountLoginBtn.gameObject);
    EventTriggerListener.Get(LoginPanelView.visitorLoginBtn.gameObject):RemoveClick(LoginPanelView.visitorLoginBtn,this.OnVisitorLoginBtnClick);
    EventTriggerListener.Get(LoginPanelView.registerBtn.gameObject):RemoveClick(LoginPanelView.registerBtn,this.OnRegisterBtnClick);
    EventTriggerListener.Get(LoginPanelView.moreLoginWayBtn.gameObject):RemoveClick(LoginPanelView.moreLoginWayBtn,this.OnMoreLoginWayBtnClick);
    EventTriggerListener.Get(LoginPanelView.wechatLoginBtn.gameObject):RemoveClick(LoginPanelView.wechatLoginBtn,this.OnWechatLoginBtnClick);
    EventTriggerListener.Get(LoginPanelView.thirdLoginMaskBtn.gameObject):RemoveClick(LoginPanelView.thirdLoginMaskBtn,this.OnThirdLoginMaskBtnClick);
    --TODO
end
---------------------------------------------------------------------------------------
--账号登录按钮点击--
function LoginPanelCtrl.OnAccountLoginBtnClick(go)   
	 log("accountLoginBtn clicked.............")
    --this.SendLogin(LoginPanelView.userName.text,LoginPanelView.passWord.text);
    log("account:"..LoginPanelView.userName.text.."_passWord:"..LoginPanelView.passWord.text);

    --test  go to mainscene
end

--游客登录按钮点击--
function LoginPanelCtrl.OnVisitorLoginBtnClick(go)
    log("visitorLoginBtn clicked.............")
    xpageMgr:ShowPage(true,"UI/Prefab/GameHallPanel");
end

--注册按钮点击--
function LoginPanelCtrl.OnRegisterBtnClick( go )
	 --log("reginsterBtn clicked.............")
    xpageMgr:ShowPage(true,"UI/Prefab/RegisterAccountPanel");
end

--更多登录方式按钮点击--
function LoginPanelCtrl.OnMoreLoginWayBtnClick( go )
	LoginPanelView.thirdLoginGo:SetActive(true)
end

--微信登录按钮点击--
function LoginPanelCtrl.OnWechatLoginBtnClick( go )
	log("wechatLoginBtn clicked.............")
end

--点击空白处关闭第三方登陆按钮--
function LoginPanelCtrl.OnThirdLoginMaskBtnClick( go )
	LoginPanelView.thirdLoginGo:SetActive(false)
end

function LoginPanelCtrl.OnEventTest(  )
	
	log("OnEventTest........................................")
end

--function LoginPanelCtrl.OnEventTestInt( i )
	--log("OnEventTestInt......"..i)
--end
----------------------------------------------------------------------------------------
--发送登陆--
function LoginPanelCtrl.SendLogin(_userName,_passWord)
    log(_userName.."|".._passWord);
    local path = Util.DataPath.."lua/3rd/pbc/gt_base.pb";
    log('io.open--->>>'..path);

    local addr = io.open(path, "rb")
    local buffer = addr:read "*a"
    addr:close()
    protobuf.register(buffer)

    local _LoginRequest = {
        accounts = "joe2",
        password = "4297f44b13955235245b2497399d7a93",
        sessionid = "123456",
        uid = "10000",
        dwPlazaVersion = 101515267,
        szMachineID = "b1a6afedf9cbc767ac8ff04fe997655a",
        dwLogonType = 3,
        dwClientVersion = 5003,
        dwClientIP = "28551360",       
    }
    local code = protobuf.encode("gt_msg.LoginRequest", _LoginRequest)

    local buffer = ByteBuffer.New();
    buffer:WriteBuffer(code);
    networkMgr:SendMessage(Protocal.LoginRequest,buffer);
end

--登陆返回--
function LoginPanelCtrl.OnLoginResponse(buffer)
    local data = buffer:ToLuaByteBuffer();
    local path = Util.DataPath.."lua/3rd/pbc/gt_base.pb";
    local addr = io.open(path, "rb")
    local buffer = addr:read "*a"
    addr:close()
    protobuf.register(buffer)
    local decode = protobuf.decode("gt_msg.LoginResponse",data)
    log('LoginPanelCtrl.OnLoginResponse--->>>'..decode.szDescribeString);

    --    
    --for _,v in ipairs(decode.phone) do
      --  print("\t"..v.number, v.type)
    --end
end

