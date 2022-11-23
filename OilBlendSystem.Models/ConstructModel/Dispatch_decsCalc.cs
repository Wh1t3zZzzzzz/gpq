namespace OilBlendSystem.Models.ConstructModel
{
    public class Dispatch_decsCalc
    {
        //智能决策——决策计算
        public string? status { get; set; }//求解标志（求解成功 or 失败）
        public float objValue { get; set; }//目标函数值

    }
}