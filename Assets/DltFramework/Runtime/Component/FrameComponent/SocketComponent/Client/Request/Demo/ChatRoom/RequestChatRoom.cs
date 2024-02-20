using DltFramework;

public class RequestChatRoom
{
    [AddRequestCode(RequestCode.ChatRoom, RequestType.Client)]
    public void OnChatRoom(string data)
    {
        ListenerFrameComponent.Instance.chatRoom.AddChatRoom(data);
    }
}