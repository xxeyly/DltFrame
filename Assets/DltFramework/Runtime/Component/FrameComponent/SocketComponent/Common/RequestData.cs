public class RequestData
{
    public int requestCode;
    public byte[] data;

    public RequestData(int requestCode, byte[] data)
    {
        this.requestCode = requestCode;
        this.data = data;
    }
}