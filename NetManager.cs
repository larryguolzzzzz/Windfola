using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
using UnityEngine.UI;
using System;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;


public class NetManager : NetworkManager
{

    //get the instance
    private static NetManager _instance;
    public static NetManager GetInstance
    {
        get
        {
            if (_instance == null)
            {
                //_instance = new NetManager();
                _instance = FindObjectOfType<NetManager>();
            }
            return _instance;
        }
    }
    //start of the game, should use start() not awake()
    private void Start()
    {

        _instance = this;

        autoCreatePlayer = false;
        //register
        NetworkServer.RegisterHandler(MsgType.Highest + 1, OnMsgArrive);


        InitialRegist();


        //TryHost();

    }

    bool hasConnected = false;

    void InitialRegist()
    {

        onClientConnect += ((NetworkConnection conn) => waitToConnect.Add(new Pear(conn)));

        RegistNet("connected it", (object ob) =>
        {
            if (hasConnected) return;
            myNetId = (int)ob;
            SendToServer("RemoveWait", ob);
            if (onConnectToServer_Stable != null)
                onConnectToServer_Stable.Invoke((int)ob);
        });
        //pear
        RegistNet("RemoveWait", (object ob) =>
        {
            Pear p = waitToConnect.Find(x => x.conn.connectionId == (int)ob);
            if (p == null)
                return;

            pears.Add(p);
            waitToConnect.Remove(p);
        });


    }

    public int myNetId = -1;


    #region tools

    public string GetIP()
    {
        NetworkInterface[] adapters = NetworkInterface.GetAllNetworkInterfaces(); ;
        foreach (NetworkInterface adapter in adapters)
        {
            if (adapter.Supports(NetworkInterfaceComponent.IPv4))
            {
                UnicastIPAddressInformationCollection uniCast = adapter.GetIPProperties().UnicastAddresses;
                if (uniCast.Count > 0)
                {
                    foreach (UnicastIPAddressInformation uni in uniCast)
                    {

                        if (uni.Address.AddressFamily == AddressFamily.InterNetwork)
                        {
                            return uni.Address.ToString();
                        }
                    }
                }
            }
        }
        return "no ip…";

    }

    public bool logOut = true;
    void log(object ob) { if (logOut) Debug.Log(ob); }

    #endregion
    #region syncTools

    public static byte[] SerializeObj(object obj)
    {
        MemoryStream ms = new MemoryStream();
        BinaryFormatter bf = new BinaryFormatter();
        bf.Serialize(ms, obj);
        byte[] data = ms.ToArray();
        return data;
    }


    public static T DeSerializeObj<T>(byte[] data)
    {
        MemoryStream ms1 = new MemoryStream(data);
        BinaryFormatter bf1 = new BinaryFormatter();
        T type = (T)bf1.Deserialize(ms1);
        return type;
    }


    #endregion
    public static bool host = false;
    public void TryConnectIp(string ip)
    {
        networkAddress = ip;
        networkPort = 7777;
        hasConnected = false;
        StartClient();
        host = false;
    }

    public void TryConnectIp(Text ip)
    {
        TryConnectIp(ip.text);
    }




    public void TryHost()
    {
        waitToConnect.Clear();

        networkPort = 7777;
        StartHost();
        host = true;

        NetworkServer.RegisterHandler(MsgType.Highest + 1, OnMsgArrive);

    }

    int cd = 0;
    List<Pear> waitToConnect = new List<Pear>();
    private void FixedUpdate()
    {
        //wait actions
        if (waitToConnect.Count > 0)
        {
            if (cd < 1)
            {
                foreach (var p in waitToConnect)
                {
                    SendToPear(p, "connected it", p.conn.connectionId);
                }
                cd = 5;
            }
            else { cd--; }
        }
    }

    //user is connecting
    public override void OnServerConnect(NetworkConnection conn)
    {
        base.OnServerConnect(conn);
        log("player in：" + conn.address + " id:" + conn.connectionId);
        if(onClientConnect!=null)
            onClientConnect.Invoke(conn);
    }

    public delegate void Del_connect_id(int id);
    public delegate void Del_connect(NetworkConnection conn);

    public event Del_connect_id onConnectToServer_Stable;//connect to server

    public event Del_connect onConnectToServer;//connecting to server

    public event Del_connect onClientConnect;//player is connected

    public event Del_connect onDisconnect;//disconnected from server

    public event Del_connect onPlayerLeft;//disconnected

    public delegate void MsgCallBack(string type, object msg);



    // called when connected to a server
    public override void OnClientConnect(NetworkConnection conn)
    {
        base.OnClientConnect(conn);
        log("connected to server：" + conn.address + " NetId：" + conn.connectionId);
        if(onConnectToServer!=null)
        onConnectToServer.Invoke(conn);


        client.RegisterHandler(MsgType.Highest + 1, OnMsgArrive);

    }


    public List<Pear> pears = new List<Pear>();

    public class Pear
    {
        public Pear(NetworkConnection _conn) { conn = _conn; }
        public NetworkConnection conn;
    }



    /// <summary>
    /// sentoserver
    /// </summary>
    /// <param name="_type"></param>
    /// <param name="_msg"></param>
    /// send to server
    public void SendToServer(string _type, object _msg = null)
    {
        client.Send(MsgType.Highest + 1,
            new MsgBase { bytes = SerializeObj(new MsgPacket { msg = _msg, type = _type }) }
            );
    }
    //server send to all players
    public void SendToAllPears(string _type, object _msg = null)
    {
        if (logOut) { Debug.Log("server to ALL" + " type：" + _type + " msg: " + _msg); }

        foreach (var p in pears)
        {
            SendToPear(p, _type, _msg);
        }
    }


    public void SendToPear(Pear p, string _type, object _msg)
    {
        SendToNetId(p.conn.connectionId, _type, _msg);
    }

    public void SendToNetId(int netId, string _type, object _msg)
    {
        log("server to " + netId + " type：" + _type + " msg: " + _msg);
        NetworkServer.SendToClient(netId, MsgType.Highest + 1,
            new MsgBase { bytes = SerializeObj(new MsgPacket { msg = _msg, type = _type }) }
            );
    }



    //convert msg
    void OnMsgArrive(NetworkMessage msg)
    {
        MsgPacket packet = DeSerializeObj<MsgPacket>(msg.ReadMessage<MsgBase>().bytes);
        ReceiveMsg(packet.type, packet.msg);
    }

    [Serializable]
    class MsgBase : MessageBase
    {
        public byte[] bytes;
    }
    [Serializable]
    class MsgPacket
    {
        public string type;

        public object msg;
    }



    public delegate void d_netCallBack(object ob);
    Dictionary<string, d_netCallBack> netCallBacks = new Dictionary<string, d_netCallBack>();
    void ReceiveMsg(string type, object msg)
    {
        log("get：" + type + "   info：" + msg);

        d_netCallBack callback;
        if (netCallBacks.TryGetValue(type, out callback))
        { callback(msg); }
        else { log("nofound：" + type); }
    }

    public void RegistNet(string type, d_netCallBack callback)
    {
        netCallBacks.Add(type, callback);
    }

    //player disconnect
    public override void OnClientDisconnect(NetworkConnection conn)
    {
        StopClient();
        if(onDisconnect!=null)
        onDisconnect.Invoke(conn);
    }
    //player leave
    public override void OnServerDisconnect(NetworkConnection conn)
    {
        NetworkServer.DestroyPlayersForConnection(conn);
        if(onPlayerLeft!=null)
        onPlayerLeft.Invoke(conn);
        waitToConnect.Remove(waitToConnect.Find(x => x.conn.connectionId == conn.connectionId));
        log(conn.connectionId + " disconnected");
    }













}
