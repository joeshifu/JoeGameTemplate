
require "Common/define"
require "Common/protocal"
require "Common/functions"
Event = require 'events'

require "3rd/pblua/login_pb"
require "3rd/pbc/protobuf"

local sproto = require "3rd/sproto/sproto"
local core = require "sproto.core"
local print_r = require "3rd/sproto/print_r"

Network = {};
local this = Network;

local transform;
local gameObject;


function Network.Start() 
    logWarn("Network.Start!!"); 
    Event.AddListener(Protocal.HHResponse, this.OnHHResponse); 
end

--Socket消息分发--
function Network.OnSocket(key, data)
    Event.Brocast(tostring(key), data);
end

--当连接建立时--
function Network.OnConnect() 
    logWarn("Game Server connected!!");

    --开定时器，发心跳包 TODO
    this.SendHeart(); 

    --coroutine.start(this.test_coroutine);
end

--连接服务器失败--
function Network.OnConnectFailed()
     logWarn("connected failed !!");
end

--连接中断，或者被踢掉--
function Network.OnDisconnect() 
    logError("OnDisconnect------->>>>");
end

--卸载网络监听--
function Network.Unload()
    Event.RemoveListener(Protocal.HHResponse);
    logWarn('Unload Network...');
end

--------------------------------------------------------------
--发送心跳包--
function Network.SendHeart()
    local path = Util.DataPath.."lua/3rd/pbc/gt_base.pb";
    log('io.open--->>>'..path);

    local addr = io.open(path, "rb")
    local buffer = addr:read "*a"
    addr:close()
    protobuf.register(buffer)

    local hhrequest = {
       
    }
    local code = protobuf.encode("gt_msg.HHRequest", hhrequest)

    local buffer = ByteBuffer.New();
    buffer:WriteBuffer(code);
    networkMgr:SendMessage(Protocal.HHRequest,buffer);
end

--接收心跳回包--
function Network.OnHHResponse(buffer)
    local data = buffer:ToLuaByteBuffer();
    local path = Util.DataPath.."lua/3rd/pbc/gt_base.pb";
    local addr = io.open(path, "rb")
    local buffer = addr:read "*a"
    addr:close()
    protobuf.register(buffer)
    local decode = protobuf.decode("gt_msg.HHResponse" , data)
    print(decode.serverTimeNow)
end

---------------------------------------------------------------------------------
--测试协同--
function Network.test_coroutine()    
    logWarn("1111");
    coroutine.wait(1);  
    logWarn("2222");
    
    local www = WWW("http://bbs.ulua.org/readme.txt");
    coroutine.www(www);
    logWarn(www.text);      
end
--------------------------------------------------------------------------------
