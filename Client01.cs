using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

public class Client01 : MonoBehaviour {

    //定义ip、端口 字段...
    //本地第一次修改客户端代码
    private string ip = "192.168.0.103";
    private int port = 33333;
    private Socket socket;

    void Start () {
        try
        {
            socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            IPAddress address = IPAddress.Parse(ip);
            IPEndPoint point = new IPEndPoint(address, port);
            //连接服务器端口
            socket.Connect(point);
        }
        catch 
        {
            print("服务器无法连接");
        }
        //开启子线程，接受服务器信息
        Thread receiveThread = new Thread(ReceiveThread);
        receiveThread.Start();
    }
	
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            SendMessages("AAA");
        }
        if (Input.GetKeyDown(KeyCode.B))
        {
            SendMessages("BBBBBB");
        }
        if (Input.GetKeyDown(KeyCode.C))
        {
            SendMessages("CCCCCCCCCC");
        }
    }

    void OnDestroy()
    {
        socket.Close();
        print("客户端已下线");
    }

    /// <summary>
    /// 客户端给服务器端发信息
    /// </summary>
    private void SendMessages(string str)
    {
        byte[] message = Encoding.UTF8.GetBytes(str);
        socket.Send(message);
    }

    /// <summary>
    /// 子线程，接受服务器信息
    /// </summary>
    private void ReceiveThread()
    {
        while (true)
        {
            byte[] messages = new byte[1024];
            int length = socket.Receive(messages);
            if (length == 0)
            {
                print("服务器端已下线...");
                break;
            }
            print(Encoding.UTF8.GetString(messages));
        }
    }
}
