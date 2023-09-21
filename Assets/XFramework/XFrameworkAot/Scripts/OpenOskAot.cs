using System.Runtime.InteropServices;

public class OpenOskAot
{
    /// <summary>
    /// 调用外部应用
    /// </summary>
    /// <param name="exeName">带后缀的exe名称，如osk.exe</param>
    /// <param name="parameters">传给exeName的参数，不需要的话可留空</param>
    /// <param name="workDirector">exeName的工作目录，即exe所在的全路径，如D:/somePath</param>
    /// <param name="showWindow">是否显示exe窗口</param>
    /// <returns></returns>
    [DllImport("UniCaller", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public static extern int runExe(string exeName, string parameters, string workDirector, bool showWindow);
}
