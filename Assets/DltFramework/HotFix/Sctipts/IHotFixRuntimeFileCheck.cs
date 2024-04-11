namespace HotFix
{
    public interface IHotFixRuntimeFileCheck
    {
        //下载开始下载
        public void HotFixRuntimeTableDownStart();

        //配置表下载完毕
        public void HotFixRuntimeTableDownOver();

        //开始本地检测
        public void HotFixRuntimeLocalFileCheck(int currentCount, int maxCount);

        //本地检测完毕
        public void HotFixRuntimeLocalFileCheckOver();
    }
}