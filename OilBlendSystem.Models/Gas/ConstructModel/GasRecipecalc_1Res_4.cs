namespace OilBlendSystem.Models.Gas.ConstructModel
{
    public class GasRecipecalc_1Res_4
    {
        //场景1
        //计算结果：成品油属性
        public string? ProdOilName { get; set; }//成品油名称
        public string? Prodron {get; set; }//成品油十六烷值 用string是为了输出46.0这样的数据
        public float ronLowLimit {get; set; }//成品油十六烷值低限
        public float ronHighLimit {get; set; }//成品油十六烷值高限
        public string? Prodt50 {get; set; }//50%回收温度
        public float t50LowLimit { get; set; }
        public float t50HighLimit { get; set; }
        public string? Prodsuf {get; set; }//多芳烃含量
        public float sufLowLimit { get; set; }
        public float sufHighLimit { get; set; }
        public string? Prodden {get; set; }//密度
        public float denLowLimit {get; set; }
        public float denHighLimit {get; set; }

    }
}