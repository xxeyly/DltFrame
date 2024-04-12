using System;

[Serializable]
public class ServerRoomPlayerReadyState
{
    public int roomId;
    public string token;
    public bool ready;
}