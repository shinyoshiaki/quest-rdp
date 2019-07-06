using UnityEngine;
using WebRTC;
using WebSocketSharp;
using UniRx;
using System;

public class Connect : MonoBehaviour
{
    WebSocket ws;
    Signaling signaling;

    public delegate void IOnRemoteVideo(int id,
      IntPtr dataY, IntPtr dataU, IntPtr dataV, IntPtr dataA,
      int strideY, int strideU, int strideV, int strideA,
      uint width, uint height);

    public IOnRemoteVideo OnRemoteVideo;

    bool connected = false;

    public void StartConnect(string ipAddress)
    {

#if UNITY_EDITOR
#elif UNITY_ANDROID
        AndroidJavaClass playerClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        AndroidJavaObject activity = playerClass.GetStatic<AndroidJavaObject>("currentActivity");
        AndroidJavaClass utilityClass = new AndroidJavaClass("org.webrtc.UnityUtility");
        utilityClass.CallStatic("InitializePeerConncectionFactory", new object[1] { activity });
#endif

        Observable.Timer(TimeSpan.FromSeconds(2)).Subscribe(_ => Join());

        Debug.Log("start");
        ws = new WebSocket("ws://" + ipAddress + ":8080");

        ws.OnMessage += (_, e) => OnMessage(e.Data);

        ws.Connect();

        signaling = new Signaling(ipAddress);
        signaling.OnConnectMethod += OnConnet;
        signaling.OnDataMethod += OnData;
        signaling.OnSdpMethod += OnSdp;
        signaling.OnRemoteVideo += OnI420RemoteFrameReady;
    }

    void OnConnet(string str)
    {
        Debug.Log("connect");
        signaling.peer.SendDataViaDataChannel("test from unity");
        connected = true;
    }

    public void Send(string str)
    {
        if (connected)
            signaling.peer.SendDataViaDataChannel(str);
    }

    void OnData(string s)
    {
        Debug.Log("data " + s);
    }

    void OnSdp(string s)
    {
        Debug.Log("sendsdp " + s);
        ws.Send(s);
    }

    class Action
    {
        public string type;
        public string payload;
    }

    public void Join()
    {
        Debug.Log("join");
        var data = new Action();
        data.type = "join";
        data.payload = "join";
        var json = JsonUtility.ToJson(data);
        ws.Send(json);
    }


    class OnMessageS
    {
        public string type;
        public string payload;
    }
    void OnMessage(string s)
    {
        Debug.Log("onmessage " + s);
        var data = JsonUtility.FromJson<OnMessageS>(s);
        Debug.Log(data.type);
        if (data.type == "offer" || data.type == "answer" || data.type == "ice")
        {
            signaling.SetSdp(data.payload);
        }
    }

    void OnI420RemoteFrameReady(int id,
       IntPtr dataY, IntPtr dataU, IntPtr dataV, IntPtr dataA,
       int strideY, int strideU, int strideV, int strideA,
       uint width, uint height)
    {
        OnRemoteVideo(id, dataY, dataU, dataV, dataA, strideY, strideU, strideV, strideA, width, height);
    }


    void OnDestroy()
    {
        Debug.Log("OnDestroy1");
        signaling.Close();
    }
}
