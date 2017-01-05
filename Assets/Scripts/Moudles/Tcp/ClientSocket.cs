using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Threading;
using System.Runtime.CompilerServices;
using System.Net;

namespace NsTcpClient
{
    /*
     *  4字节+  8字节+   2字节  + 2字节   +  msglen
        “GTV1” 时间戳  msgid    msglen     data
     */

    [StructLayoutAttribute(LayoutKind.Sequential, Pack = 1)]
    public struct GamePackHeader_QiPai
    {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        public byte[] desc;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
        public byte[] timeStamp;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
        public byte[] msgId;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
        public byte[] msgLength;
    }

    public class GamePacket
    {
        public GamePackHeader_QiPai header;
        public byte[] data = null;

        public int GetMsgId()
        {
            return (header.msgId[0] << 8) + header.msgId[1];
        }
    }

    public class ClientSocket
    {
        public static readonly int cSocketConnWaitTime = 2000;
        private ITcpClient mTcpClient = null;
        private LinkedList<GamePacket> mPacketList = new LinkedList<GamePacket>();
        private byte[] mRecvBuffer = new byte[TcpClient.MAX_TCP_CLIENT_BUFF_SIZE];
        private int mRecvSize = 0;
        private bool mConnecting = false;
        private bool mAbort = false;
        private System.Timers.Timer mTimer = null;

        #region SocketState
        private event OnSocketStateEvent mStateEvents;
        public delegate void OnSocketStateEvent(eClientState state);
        public void AddStateEvent(OnSocketStateEvent evt)
        {
            if (evt == null)
                return;

            mStateEvents += evt;
        }

        public void RemoveStateEvent(OnSocketStateEvent evt)
        {
            if (evt == null)
                return;

            mStateEvents -= evt;
        }

        public eClientState GetState()
        {
            if (mTcpClient == null)
                return eClientState.eCLIENT_STATE_NONE;
            return mTcpClient.GetState();
        }
        #endregion

        #region PacketDistribute
        public delegate void OnPacketDistributeEvent(GamePacket packet);
        private event OnPacketDistributeEvent mPacketDistribute;
        public void AddPacketDistributeEvent(OnPacketDistributeEvent evt)
        {
            if (evt == null)
                return;

            mPacketDistribute += evt;
        }

        public void RemovePacketDistributeEvent(OnPacketDistributeEvent evt)
        {
            if (evt == null)
                return;

            mPacketDistribute -= evt;
        }
        #endregion

        public ClientSocket()
        {
            
        }

        private bool canUpdate = false;
        public void OnUpdate()
        {
            if (canUpdate)
            {
                Execute_QiPai();
            }
        }

        public bool Connect(string ip, int port)
        {
            DisConnect();
            mTcpClient = new TcpClient();
            bool ret = mTcpClient.Connect(ip, port, cSocketConnWaitTime);
            if (ret)
            {
                mConnecting = true;
                canUpdate = true;
            }
            else
            {
                mTcpClient.Release();
                mTcpClient = null;
            }

            return ret;
        }

        public void DisConnect()
        {
            canUpdate = false;
            if (mPacketList != null)
                mPacketList.Clear();
            if (mTcpClient != null)
            {
                mTcpClient.Release();
                mTcpClient = null;
            }

            mRecvSize = 0;
            mConnecting = false;
            mAbort = false;
        }

        private void ProcessPackets()
        {
            LinkedListNode<GamePacket> node = mPacketList.First;
            while (node != null)
            {
                GamePacket packet = node.Value;
                LinkedListNode<GamePacket> next = node.Next;
                mPacketList.Remove(node);

                if (packet != null)
                {
                    ProcessPacket(packet);
                }

                node = next;
            }
        }

        private void ProcessPacket(GamePacket packet)
        {
            if (mPacketDistribute != null)
                mPacketDistribute(packet);
        }

        ///////////////////////////////////////////

        #region 棋牌
        public void Send_QiPai(int _msgId, byte[] _dataBytes)
        {
            _dataBytes = codeData(_dataBytes);

            GamePackHeader_QiPai header = new GamePackHeader_QiPai();
            byte[] mMagic = System.Text.Encoding.Default.GetBytes("GTV1");
            header.desc = mMagic;
            header.msgId = new byte[2];
            header.msgId[0] = (byte)(_msgId >> 8);
            header.msgId[1] = (byte)(_msgId >> 0);
            header.msgLength = new byte[2];
            header.msgLength[0] = (byte)(_dataBytes.Length >> 8);
            header.msgLength[1] = (byte)(_dataBytes.Length >> 0);
            int headerSize = Marshal.SizeOf(header);
            byte[] finalBytes = new byte[headerSize + _dataBytes.Length];
            IntPtr pStruct = Marshal.AllocHGlobal(headerSize);
            try
            {
                Marshal.StructureToPtr(header, pStruct, false);
                Marshal.Copy(pStruct, finalBytes, 0, headerSize);
            }
            finally
            {
                Marshal.FreeHGlobal(pStruct);
            }
            Buffer.BlockCopy(_dataBytes, 0, finalBytes, headerSize, _dataBytes.Length);
            mTcpClient.Send(finalBytes);

        }

        private byte[] codeData(byte[] data)
        {
            byte[] key = { 69, 123, 132, 104, 67, 95, 33, 74, 120, 131, 61, 101, 55, 101, 69, 44 };

            for (int i = 0; i < data.Length; i++)
            {
                data[i] ^= key[(i) % key.Length];
            }
            return data;
        }

        public bool Execute_QiPai()
        {

            if (mTcpClient == null)
            {
                canUpdate = false;
                return false;
            }

            if (mConnecting)
            {
                eClientState state = mTcpClient.GetState();
                if ((state == eClientState.eClient_STATE_CONNECTING) ||
                    (state == eClientState.eCLIENT_STATE_NONE))
                    return true;

                if ((state == eClientState.eClient_STATE_CONNECT_FAIL) ||
                    (state == eClientState.eClient_STATE_ABORT))
                {
                    mConnecting = false;
                    mAbort = false;
                    mTcpClient.Release();
                    mTcpClient = null;

                    // Call Event Error
                    if (mStateEvents != null)
                    {
                        mStateEvents(state);
                    }

                    return false;
                }
                else if (state == eClientState.eClient_STATE_CONNECTED)
                {
                    mConnecting = false;
                    mAbort = false;

                    // Call Event Success
                    if (mStateEvents != null)
                    {
                        mStateEvents(state);
                    }

                    return true;
                }

                mConnecting = false;
            }

            if (mTcpClient.HasReadData())
            {
                int recvsize = mTcpClient.GetReadData(mRecvBuffer, mRecvSize);
                if (recvsize > 0)
                {
                    mRecvSize += recvsize;
                    int recvBufSz = mRecvSize;
                    int i = 0;
                    GamePackHeader_QiPai header = new GamePackHeader_QiPai();
                    int headerSize = Marshal.SizeOf(header);
                    IntPtr headerBuffer = Marshal.AllocHGlobal(headerSize);
                    try
                    {
                        //可能粘包，用循环来切包
                        while (recvBufSz - i >= headerSize)
                        {
                            Marshal.Copy(mRecvBuffer, i, headerBuffer, headerSize);
                            header = (GamePackHeader_QiPai)Marshal.PtrToStructure(headerBuffer, typeof(GamePackHeader_QiPai));
                            string descStr = System.Text.Encoding.Default.GetString(header.desc);

                            if (descStr == "GTV1")
                            {
                                
                            }

                            int msgId = (header.msgId[0] << 8) + header.msgId[1];
                            int msgLength = (header.msgLength[0] << 8) + header.msgLength[1];

                            //断包，break掉，继续接收
                            if ((recvBufSz - i) < (msgLength + headerSize))
                                break;

                            byte[] dataBytes = new byte[msgLength];
                            Buffer.BlockCopy(mRecvBuffer, i + headerSize, dataBytes, 0, msgLength);

                            dataBytes = codeData(dataBytes);

                            GamePacket packet = new GamePacket();
                            packet.header = header;
                            packet.data = dataBytes;

                            LinkedListNode<GamePacket> node = new LinkedListNode<GamePacket>(packet);
                            mPacketList.AddLast(node);

                            i += headerSize + msgLength;//当前包已处理完，偏移量移动，开始处理下一个包

                        }
                    }
                    finally
                    {
                        Marshal.FreeHGlobal(headerBuffer);
                    }

                    recvBufSz -= i;
                    mRecvSize = recvBufSz;
                    if (recvBufSz > 0)
                        Buffer.BlockCopy(mRecvBuffer, i, mRecvBuffer, 0, recvBufSz); //剩余的数据往前移

                    ProcessPackets();
                    return true;
                }
            }
            else
            {
                if (!mAbort)
                {
                    eClientState state = mTcpClient.GetState();
                    if (state == eClientState.eClient_STATE_ABORT)
                    {
                        mAbort = true;

                        // Call Event Abort
                        if (mStateEvents != null)
                        {
                            mStateEvents(state);
                        }

                        return false;
                    }
                }
            }
            
            return true;
        }
        #endregion


    }

}
