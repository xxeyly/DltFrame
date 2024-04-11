using System.Collections.Generic;

public interface IRequestClientRoomGetRoom
{
    void OnGetRoom(List<ServerRoomData> serverRoomData);
}