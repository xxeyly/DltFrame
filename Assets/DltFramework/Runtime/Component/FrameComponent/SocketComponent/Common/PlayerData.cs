using System;

[Serializable]
public class PlayerData
{
    public int id;
    /*//前
    public bool front;
    //后
    public bool back;
    //左
    public bool left;
    //右
    public bool right;*/

    public string x;
    public string z;

    public PlayerData()
    {
        x = "0";
        z = "0";
    }
}