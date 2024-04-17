using System;
using System.IO;
using Google.Protobuf;

public class ProtobufTool
{
    public static byte[] SerializeToByteArray<T>(T message) where T : IMessage<T>
    {
        return message.ToByteArray();
    }

    public static T DeserializeFromByteArray<T>(byte[] data) where T : class, IMessage<T>, new()
    {
        IMessage message = new T();
        try
        {
            return message.Descriptor.Parser.ParseFrom(data) as T;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
    
}