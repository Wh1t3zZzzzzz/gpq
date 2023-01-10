namespace OilBlendSystem.Models.Diesel.ConstructModel
{
    public class Recipecalc_3_1
    {
        //场景3
        //组分油参调流量高低限设置表格
        public string? ComOilName { get; set; }//组分油名称
        public float AutoFlowHigh { get; set; }//车柴参调流量上限
        public float AutoFlowLow { get; set; }//车柴参调流量下限
        public float ExpFlowHigh { get; set; }//出柴参调流量上限
        public float ExpFlowLow { get; set; }//出柴参调流量下限
        public float Prod1FlowHigh { get; set; }//备用成品油1参调流量上限
        public float Prod1FlowLow { get; set; }//备用成品油1参调流量下限
        public float Prod2FlowHigh { get; set; }//备用成品油2参调流量上限
        public float Prod2FlowLow { get; set; }//备用成品油2参调流量下限

    }
}