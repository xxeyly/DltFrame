using System;

[AttributeUsage(AttributeTargets.All, AllowMultiple = false, Inherited = true)]
public class AddRequestCodeAttribute : Attribute
{
    public int RequestCode { get; set; }
    public RequestType RequestType { get; set; }

    public AddRequestCodeAttribute(int requestCode, RequestType requestType)
    {
        RequestCode = requestCode;
        RequestType = requestType;
    }
}