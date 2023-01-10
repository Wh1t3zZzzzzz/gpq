namespace OilBlendSystem.Models.Diesel.ConstructModel
{
    public class Dispatch_parmSet_prodOil_2
    {
        //智能决策——参数设置——成品油设置
        //成品油需求
        public string? ProdOilName { get; set; }//成品油名称
        public float prodDemandLowT1 { get; set; }//成品油第一天需求低限
        public float prodDemandHighT1 { get; set; }//成品油第一天需求高限
        public float prodDemandLowT2 { get; set; }//成品油第二天需求低限
        public float prodDemandHighT2 { get; set; }//成品油第二天需求高限
        public float prodDemandLowT3 { get; set; }//成品油第三天需求低限
        public float prodDemandHighT3 { get; set; }//成品油第三天需求高限
        public float prodDemandLowT4 { get; set; }//成品油第四天需求低限
        public float prodDemandHighT4 { get; set; }//成品油第四天需求高限
        public float prodDemandLowT5 { get; set; }//成品油第五天需求低限
        public float prodDemandHighT5 { get; set; }//成品油第五天需求高限
        public float prodDemandLowT6 { get; set; }//成品油第六天需求低限
        public float prodDemandHighT6 { get; set; }//成品油第六天需求高限
        public float prodDemandLowT7 { get; set; }//成品油第七天需求低限
        public float prodDemandHighT7 { get; set; }//成品油第七天需求高限
    }
}