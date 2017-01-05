using System;
using UnityEngine;
using System.Collections.Generic;
using NsTcpClient;
//using LuaInterface;
using System.Runtime.InteropServices;

    public class NetworkManager :Singleton<NetworkManager>
    {
        private ClientSocket m_clientSocket = new ClientSocket();
        //private ICRC mCrc = new Crc32();

        void Awake()
        {
            m_clientSocket.AddStateEvent(OnSocketStateEvt);
            m_clientSocket.AddPacketDistributeEvent(OnPacketDistributeEvt);
        }

        //每帧太快的话，可以减小频率
        void Update()
        {
            m_clientSocket.OnUpdate();
        }

        public void Init()
        {
           // CallMethod("Start");
        }

        ///// <summary>
        ///// 执行Lua方法
        ///// </summary>
        //public object[] CallMethod(string func, params object[] args)
        //{
        //    return Util.CallMethod("Network", func, args);
        //}

        /// <summary>
        /// 响应socket连接状态
        /// </summary>
        /// <param name="state"></param>
        private void OnSocketStateEvt(eClientState state)
        {
            switch (state)
            {
                case eClientState.eCLIENT_STATE_NONE:
                    Debug.Log("None");
                    break;
                case eClientState.eClient_STATE_CONNECTING:
                    Debug.Log("connecting");
                    break;
                case eClientState.eClient_STATE_CONNECTED:
                    Debug.Log("connected");
                   // CallMethod("OnConnect");
                    break;
                case eClientState.eClient_STATE_CONNECT_FAIL:
                    Debug.Log("connect fail");
                   // CallMethod("OnConnectFailed");
                    break;
                case eClientState.eClient_STATE_ABORT:
                    Debug.Log("abort");
                   // CallMethod("OnDisconnect");
                    break;
                case eClientState.eClient_STATE_DISCONNECT:
                    Debug.Log("disconnect");
                   // CallMethod("OnDisconnect");
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// 消息分发
        /// </summary>
        /// <param name="packet"></param>
        private void OnPacketDistributeEvt(GamePacket packet)
        {
            //ByteBuffer buffer = new ByteBuffer();
            //buffer.WriteBytes(packet.data);
            //KeyValuePair<int, ByteBuffer> _event = new KeyValuePair<int, ByteBuffer>(packet.GetMsgId(), buffer);
            //facade.SendMessageCommand(NotiConst.DISPATCH_MESSAGE, _event);
        }

        /// <summary>
        /// 发送连接请求
        /// </summary>
        public void SendConnect()
        {
            m_clientSocket.Connect(AppConst.SocketAddress, AppConst.SocketPort);
        }

        /*
        /// <summary>
        /// 发送SOCKET消息
        /// </summary>
        public void SendMessage(ByteBuffer buffer)
        {
            //if (buffer != null)
            //{
            //    ushort id = buffer.ReadShort();
            //    Debug.LogError("id:" + id);
            //}
            //else
            //{
            //    Debug.LogError("buffer null");
            //}
            //byte[] data = buffer.ToBytes();
            //Debug.LogError("net work send!!" + data.Length);
            //ByteBuffer buf = new ByteBuffer(data);
            //ushort protoId = buf.ReadShort();
            //Debug.LogError("protoId:" + protoId);
            //byte[] data2 = buf.ToBytes();
            //Debug.LogError("datasize:" + data2.Length);
            // m_clientSocket.Send(data2, protoId);
        }
        */

        //public void SendMessage(string str_protoId, ByteBuffer buffer)
        //{
        //    byte[] data = buffer.ToBytes();
        //    int protoId = int.Parse(str_protoId.Trim());
        //    string str = String.Format("发送：ProtoId：{0}，数据内容字节数(不含包头)：{1}",protoId,data.Length);
        //    Debug.Log(str);
        //    m_clientSocket.Send_QiPai(protoId,data);
        //}

        public void Unload()
        {
           // CallMethod("Unload");
        }

        public void OnDestroy()
        {
            Unload();
            m_clientSocket.DisConnect();
            m_clientSocket.RemoveStateEvent(OnSocketStateEvt);
            m_clientSocket.RemovePacketDistributeEvent(OnPacketDistributeEvt);
            Debug.Log("~NetworkManager was destroy");
        }
    }
