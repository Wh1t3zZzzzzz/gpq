namespace OilBlendSystem.Models.ConstructModel
{
    public class Prodproperty
    {
        public int Index { get; set; }
        public string? ProdOilName { get; set; }//成品油名称
        public float CetLowLimit { get; set; }//十六烷值低限
        public float CetHighLimit { get; set; }//十六烷值高限
        public float D50LowLimit { get; set; }//50%回收温度低限
        public float D50HighLimit { get; set; }//50%回收温度高限
        public float PolLowLimit { get; set; }//多芳烃含量低限
        public float PolHighLimit { get; set; }//多芳烃含量高限
        public float DenLowLimit { get; set; }//密度低限
        public float DenHighLimit { get; set; }//密度高限
    }
}