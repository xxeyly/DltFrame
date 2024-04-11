namespace Aot
{
    public interface IHotFixViewAndHotFixCode
    {
        //需要更新
        public void HotFixViewAndHotFixCodeLocalIsUpdate(bool localIsUpdate);

//需要下载
        public void HotFixViewAndHotFixCodeIsDown(bool down);

//下载速度
        public void HotFixViewAndHotFixCodeDownSpeed(float downSpeed);

//当前下载量
        public void HotFixViewAndHotFixCodeDownloadValue(double currentDownValue, double totalDownValue);
    }
}