public class UdpRequestData
{
    public int frameIndex;
    public string data;

    public UdpRequestData(int frameIndex, string data)
    {
        this.frameIndex = frameIndex;
        this.data = data;
    }
}