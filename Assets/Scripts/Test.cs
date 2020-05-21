using System;
using System.Collections.Generic;

public class Test
{
    /// <summary>
    /// 
    /// </summary>
    public string number { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public string op { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public string isSuccess { get; set; }
    /// <summary>
    /// 七步洗手
    /// </summary>
    public string zqda { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public string useTime { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public string opCount { get; set; }
    /// <summary>
    /// 秒
    /// </summary>
    public string unit { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public string score { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public string otherData { get; set; }
}

public class Root
{
    /// <summary>
    /// 
    /// </summary>
    public string pid { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public string createTime { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public DateTime updateTime { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public string useTime { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public string score { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public string status { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public List<Test> list { get; set; }
}