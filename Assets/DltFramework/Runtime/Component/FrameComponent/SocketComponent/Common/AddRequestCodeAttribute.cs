using System;

[AttributeUsage(AttributeTargets.All, AllowMultiple = false, Inherited = true)]
public class AddRequestCodeAttribute : Attribute
{
    public RequestCode RequestCode { get; set; }
    public RequestType RequestType { get; set; }

    public AddRequestCodeAttribute(RequestCode requestCode, RequestType requestType)
    {
        RequestCode = requestCode;
        RequestType = requestType;
    }
}