public class RequestData
{
    public RequestCode requestCode;
    public string data;

    public RequestData(RequestCode requestCode, string data)
    {
        this.requestCode = requestCode;
        this.data = data;
    }
}