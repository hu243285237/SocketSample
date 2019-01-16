using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonFunction : MonoBehaviour
{
    private ClientSocket myClientSocket;

    public static string textString;

    public Text text;
    public InputField inputField;

    //------------------------------------------------------------------------

    /// <summary>
    /// 链接服务器按钮
    /// </summary>
    public void _ConnectServerButton()
    {
        myClientSocket = new ClientSocket();
        myClientSocket.ConnectServer("127.0.0.1", 8088);
    }

    //------------------------------------------------------------------------

    /// <summary>
    /// 发送数据给服务器按钮
    /// </summary>
    public void _TransmitButton()
    {
        string data = inputField.text;
        myClientSocket.SendMessage(data);
    }

    //------------------------------------------------------------------------

    private void Update()
    {
        text.text = textString;
    }

    //------------------------------------------------------------------------
}
