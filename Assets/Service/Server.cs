using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net.Sockets;
using System.Net;
using System;
using System.Text;
using BXB.Core;
using LitJson;
using System.Threading.Tasks;
using UnityEditor;

public class Server : MiSingleton<Server>
{
    [SerializeField] string ip = "127.0.0.1";
    [SerializeField] int port = 1000;


    Socket socket;
    IPEndPoint iep;

    public async Task LinkServer()
    {
        await AsyncDefaule();
        int nowNumber = 0;
        int linkMaxNumerOfTimes = 5;
        while (nowNumber < linkMaxNumerOfTimes)
        {
            nowNumber++;
            try
            {
                socket = new Socket(AddressFamily.InterNetwork, socketType: SocketType.Stream, ProtocolType.Tcp);
                iep = new IPEndPoint(IPAddress.Parse(ip), port);
                socket.Connect(iep);

                Log(Color.white, $"Link server defeated - <color=#00FF00>{nowNumber}</color> / {linkMaxNumerOfTimes}");
                break;
            }
            catch (Exception exp)
            {
                Log(Color.white, $"Link server defeated - <color=#FF0000>{nowNumber}</color> / {linkMaxNumerOfTimes}");
                await Task.Delay(1000);
            }
        }
        if (nowNumber >= 5) EditorApplication.ExecuteMenuItem("Edit/Play");
    }

    public async Task SendData(JsonData data)
    {
        await AsyncDefaule();
        try
        {
            //ProjectManager.Instance.TryGetSettingAssets<SystemStringAsset>(ProjectManager.AssetTypes.SystemStringAsset,out SystemStringAsset asset);
            //JsonData jsonData = new JsonData();
            //jsonData["name"] = "Õı“ª";
            //jsonData["age"] = 10;
            //jsonData["anchievement"] = "10/10/10/10/10/10";
            //jsonData["time"] = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:ffff");
            byte[] jdBytes = Encoding.UTF8.GetBytes(data.ToJson());
            socket.Send(jdBytes);

            jdBytes = new byte[1024];
            var length = socket.Receive(jdBytes);

            var serverDataJsonText = Encoding.UTF8.GetString(jdBytes, 0, length);
            var serverData = JsonMapper.ToObject<JsonData>(new JsonReader(serverDataJsonText));
            Log(Color.cyan, serverData.ToJson());
        }
        catch (Exception exp)
        {

        }
    }
}
