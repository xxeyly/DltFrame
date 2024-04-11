namespace HotFix
{
    public interface IHotFixRuntimeFileDown
    {
        //开始下载
        public void HotFixRuntimeDownStart();

        //下载速度
        public void HotFixRuntimeDownSpeed(float downSpeed);

        //下载量
        public void HotFixRuntimeDownloadValue(double current, double total);

        //开始结束
        public void HotFixRuntimeDownOver();
    }
}