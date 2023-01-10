namespace OilBlendSystem.Models.Diesel.ConstructModel
{
    public class Recipecalc_2_3
    {
        //场景2
        //参调流量表格
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