require "Common/define"
require "Common/protocal"
require "Common/functions"
Event = require 'events'
require "3rd/pbc/protobuf"

local transform;
local gameObject;

GameHallPanelCtrl = {};
local this = GameHallPanelCtrl;

--构建函数--
function GameHallPanelCtrl.New()
	return this;
end

function GameHallPanelCtrl.Awake(xpage)
    xpage.m_pageType = EPageType.PopUp;
    xpage.m_pageMode = EPageMode.HideOtherAndNeedBack
end

function GameHallPanelCtrl.Start()
    log('GameHallPanelCtrl Start--->>>');
end

function GameHallPanelCtrl.Rest()
    log('GameHallPanelCtrl Rest--->>>');
end

function GameHallPanelCtrl.Hide()
    log('GameHallPanelCtrl Hide--->>>');
end

function GameHallPanelCtrl.Destroy()
    log('GameHallPanelCtrl Destroy--->>>');
end
---------------------------------------------------------------------------------------
