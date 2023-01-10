namespace OilBlendSystem.Models.Gas.ConstructModel
{
    public class GasRecipecalc_3_1_index
    {
        //场景3
        //组分油参调流量高低限设置表格
        public int index { get; set; }//行的索引
        public string? ComOilName { get; set; }//组分油名称
        public float gas92FlowHigh { get; set; }//车柴参调流量上限
        public float gas92FlowLow { get; set; }//车柴参调流量下限
        public float gas95FlowHigh { get; set; }//出柴参调流量上限
        public float gas95FlowLow { get; set; }//出柴参调流量下限
        public float gas98FlowHigh { get; set; }//备用成品油1参调流量上限
        public float gas98FlowLow { get; set; }//备用成品油1参调流量下限
        public float gasSelfFlowHigh { get; set; }//备用成品油2参调流量上限
        public float gasSelfFlowLow { get; set; }//备用成品油2参调流量下限

    }
}