using System;

[AttributeUsage(AttributeTargets.All, AllowMultiple = false, Inherited = true)]
public class AddFrameDataLogicAttribute : Attribute
{
    public string FrameDataType { get; set; }

    public AddFrameDataLogicAttribute(string frameDataType)
    {
        FrameDataType = frameDataType;
    }
}