namespace OilBlendSystem.Models.Gas.ConstructModel
{
    public class GasProdproperty_index
    {
        //成品油设置表格——带index索引
        public int index { get; set; }//行的索引
        public string? ProdOilName { get; set; }//成品油名称
        public float ronLowLimit { get; set; }//十六烷值低限
        public float ronHighLimit { get; set; }//十六烷值高限
        public float t50LowLimit { get; set; }//50%回收温度低限
        public float t50HighLimit { get; set; }//50%回收温度高限
        public float sufLowLimit { get; set; }//多芳烃含量低限
        public float sufHighLimit { get; set; }//多芳烃含量高限
        public float denLowLimit { get; set; }//密度低限
        public float denHighLimit { get; set; }//密度高限
    }
}