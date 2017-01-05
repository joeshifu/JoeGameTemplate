local transform;
local gameObject;

LoginPanelView = {};
local this = LoginPanelView;


function LoginPanelView.Start(obj)
	gameObject = obj;
	transform = obj.transform;
 	local userNameGo = transform:FindChild("Image_loginBg/account/InputField_account").gameObject;
 	local passWordGo = transform:FindChild("Image_loginBg/password/InputField_password").gameObject;
    this.userName =userNameGo:GetComponent("InputField");
    this.passWord =passWordGo:GetComponent("InputField");
    this.accountLoginBtn = transform:FindChild("Image_loginBg/Button_accountLogin").gameObject;
    this.visitorLoginBtn = transform:FindChild("Image_loginBg/Button_visitorLogin").gameObject;
    this.registerBtn = transform:FindChild("Image_loginBg/Button_register").gameObject;
    this.moreLoginWayBtn = transform:FindChild("Image_loginBg/Button_moreLoginWay").gameObject;
    this.thirdLoginGo = transform:FindChild("Image_loginBg/Image_thirdLoginBg").gameObject;
    this.thirdLoginMaskBtn = transform:FindChild("Image_loginBg/Image_thirdLoginBg/Button_mask").gameObject
    this.wechatLoginBtn = transform:FindChild("Image_loginBg/Image_thirdLoginBg/Button_wechatLogin").gameObject;
end

