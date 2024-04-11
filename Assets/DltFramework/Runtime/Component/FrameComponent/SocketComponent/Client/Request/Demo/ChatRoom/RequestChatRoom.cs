using DltFramework;
using Google.Protobuf;
using UnityEngine;

public class RequestChatRoom
{
    [AddRequestCode(RequestCode.ChatRoom, RequestType.Client)]
    public void OnChatRoom(string data)
    {
        ListenerFrameComponent.Instance.chatRoom.AddChatRoom(data);
      
    }
}