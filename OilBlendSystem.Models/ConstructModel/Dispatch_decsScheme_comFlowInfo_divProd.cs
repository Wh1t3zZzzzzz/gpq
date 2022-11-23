namespace OilBlendSystem.Models.ConstructModel
{
    public class Dispatch_decsScheme_comFlowInfo_divProd
    {
        //智能决策——决策方案——参调流量信息（按成品油划分）
        //第n个成品油的七天组分油参调流量数据
        public string? ComOilName { get; set; }//组分油名称
        public float comFlowT1 { get; set; }//当前成品油第一天的组分油参调流量
        public float comFlowT2 { get; set; }
        public float comFlowT3 { get; set; }
        public float comFlowT4 { get; set; }
        public float comFlowT5 { get; set; }
        public float comFlowT6 { get; set; }
        public float comFlowT7 { get; set; }

    }
}