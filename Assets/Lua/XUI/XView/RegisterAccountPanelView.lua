local transform;
local gameObject;

RegisterAccountPanelView = {};
local this = RegisterAccountPanelView;


function RegisterAccountPanelView.Start(obj)
	gameObject = obj;
	transform = obj.transform;

 	local registerGo = transform:FindChild("Image_bg/register/InputField_account").gameObject;
 	local passWordGo = transform:FindChild("Image_bg/password/InputField_password").gameObject;
    local confirmGo = transform:FindChild("Image_bg/confirm/InputField_confirm").gameObject;
    this.registerInput = registerGo:GetComponent("InputField");
    this.passwordInput =passWordGo:GetComponent("InputField");
    this.confirmInput =confirmGo:GetComponent("InputField");

    this.registerSoonBtn = transform:FindChild("Image_bg/Button_registerSoon").gameObject;

    local readToggleGo = transform:FindChild("Image_bg/Toggle").gameObject;
    this.readToggle = readToggleGo:GetComponent("Toggle");

    this.returnBtn = transform:FindChild("Button_return").gameObject;
end

