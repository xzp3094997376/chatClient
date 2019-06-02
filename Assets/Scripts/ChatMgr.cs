using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Net;
using UnityEngine;
using System.Text;
using UnityEngine.UI;
using System.Threading;

public class ChatMgr : MonoBehaviour {

    public Socket clientSocket;
    public InputField txtInput;
    Thread t;
    public Text label;
    public string message = string.Empty;
    private void OnEnable()
    {
        ConnectServer();
    }
    private void Update()
    {
        if (!string.IsNullOrEmpty(message))
        {
            label.text += "\n" + message;
            message = string.Empty;
        }
    }
    // Use this for initializat
    private void ConnectServer()
    {
        clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        clientSocket.Connect(new IPEndPoint(IPAddress.Parse("192.168.137.1"), 8888));
        t = new Thread(ReceiveMsg);
        t.Start();
    }

    byte[] data = new byte[1024];
    void ReceiveMsg()
    {
        while (true)
        {
            if (!clientSocket.Connected)
            {
                break;
            }
            int length= clientSocket.Receive(data);           
            message = Encoding.UTF8.GetString(data, 0, length);
            Debug.Log(message);
        }
    }   
    void SendMsg(string Msg)
    {
        byte[] data = Encoding.UTF8.GetBytes(Msg);
        clientSocket.Send(data);
    }                 
    public void OSendBtnClick()
    {
        string txt = txtInput.text;
        SendMsg(txt);
        txtInput.text = "";
    }
    private void OnDestroy()
    {
        clientSocket.Shutdown(SocketShutdown.Both);
        clientSocket.Close();
    }
}
