namespace OilBlendSystem.Models.ConstructModel
{
    public class Recipecalc_3Res_3
    {
        //场景3
        //计算结果：成品油属性
        public string? ProdOilName { get; set; }//成品油名称
        public string? ProdCET {get; set; }//成品油十六烷值  
        public float CETLowLimit {get; set; }//成品油十六烷值低限
        public float CETHighLimit {get; set; }//成品油十六烷值高限
        public string? ProdD50 {get; set; }//50%回收温度
        public float D50LowLimit { get; set; }
        public float D50HighLimit { get; set; }
        public string? ProdPOL {get; set; }//多芳烃含量
        public float POLLowLimit { get; set; }
        public float POLHighLimit { get; set; }
        public string? ProdDEN {get; set; }//密度
        public float DENLowLimit {get; set; }
        public float DENHighLimit {get; set; }

    }
}