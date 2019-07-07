using System;
using UnityEngine;
using UniRx;
public class WebRtcVideoPlayer : MonoBehaviour
{

    private Texture2D tex;

    public Connect connect;

    FramePacket framePacket;

    void Start()
    {
        tex = new Texture2D(2, 2);
        tex.SetPixel(0, 0, Color.blue);
        tex.SetPixel(1, 1, Color.blue);
        tex.Apply();
        GetComponent<Renderer>().material.mainTexture = tex;
        connect.OnRemoteVideo += OnI420RemoteFrameReady;

        Observable.Interval(TimeSpan.FromMilliseconds(1000 / 30)).Subscribe(l =>
       {
           ProcessFrameBuffer(framePacket);
       }).AddTo(this);
    }

    private void ProcessFrameBuffer(FramePacket packet)
    {
        if (packet == null)
        {
            return;
        }

        if (tex == null || (tex.width != packet.width || tex.height != packet.height))
        {
            Debug.Log("Create Texture. width:" + packet.width + " height:" + packet.height);
            tex = new Texture2D(packet.width, packet.height, TextureFormat.RGBA32, false);
        }
        //Debug.Log("Received Packet. " + packet.ToString());
        tex.LoadRawTextureData(packet.Buffer);

        tex.Apply();
        GetComponent<Renderer>().material.mainTexture = tex;
        framePacket = null;
    }

    public void OnI420RemoteFrameReady(int id,
     IntPtr dataY, IntPtr dataU, IntPtr dataV, IntPtr dataA,
     int strideY, int strideU, int strideV, int strideA,
     uint width, uint height)
    {
        FramePacket packet = GetNewBuffer((int)(width * height * 4));
        if (packet == null)
        {
            Debug.LogError("OnI420RemoteFrameReady: FramePacket is null!");
            return;
        }
        CopyYuvToBuffer(dataY, dataU, dataV, strideY, strideU, strideV, width, height, packet.Buffer);
        packet.width = (int)width;
        packet.height = (int)height;
        framePacket = packet;
    }

    private FramePacket GetNewBuffer(int neededSize)
    {
        FramePacket packet = new FramePacket((int)(neededSize * 1.2));
        return packet;
    }

    void CopyYuvToBuffer(IntPtr dataY, IntPtr dataU, IntPtr dataV,
      int strideY, int strideU, int strideV,
      uint width, uint height, byte[] buffer)
    {
        unsafe
        {
            byte* ptrY = (byte*)dataY.ToPointer();
            byte* ptrU = (byte*)dataU.ToPointer();
            byte* ptrV = (byte*)dataV.ToPointer();
            int srcOffsetY = 0;
            int srcOffsetU = 0;
            int srcOffsetV = 0;
            int destOffset = 0;
            for (int i = 0; i < height; i++)
            {
                srcOffsetY = i * strideY;
                srcOffsetU = (i / 2) * strideU;
                srcOffsetV = (i / 2) * strideV;
                destOffset = i * (int)width * 4;
                for (int j = 0; j < width; j += 2)
                {
                    {
                        byte y = ptrY[srcOffsetY];
                        byte u = ptrU[srcOffsetU];
                        byte v = ptrV[srcOffsetV];
                        srcOffsetY++;
                        srcOffsetU++;
                        srcOffsetV++;
                        destOffset += 4;
                        buffer[destOffset] = y;
                        buffer[destOffset + 1] = u;
                        buffer[destOffset + 2] = v;
                        buffer[destOffset + 3] = 0xff;

                        // use same u, v values
                        byte y2 = ptrY[srcOffsetY];
                        srcOffsetY++;
                        destOffset += 4;
                        buffer[destOffset] = y2;
                        buffer[destOffset + 1] = u;
                        buffer[destOffset + 2] = v;
                        buffer[destOffset + 3] = 0xff;
                    }
                }
            }
        }
    }
}
