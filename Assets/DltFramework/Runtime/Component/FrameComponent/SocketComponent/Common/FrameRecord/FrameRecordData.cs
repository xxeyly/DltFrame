using System;

[Serializable]
public class FrameRecordData
{
    public int id;
    public bool create;
    public bool exit;
    public bool w, a, s, d;

    public static bool IsEmptyFrame(FrameRecordData frameRecordData)
    {
        return !frameRecordData.create &&
               !frameRecordData.exit &&
               !frameRecordData.w &&
               !frameRecordData.a &&
               !frameRecordData.s &&
               !frameRecordData.d;
    }
}