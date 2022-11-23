namespace OilBlendSystem.Models.ConstructModel
{
    public class Dispatch_decsScheme_prodInfo
    {
        //智能决策——决策方案——成品信息
        //成品油的属性信息
        public string? PropertyName { get; set; }//属性名称
        public float valueT1 { get; set; }//第一天的属性值
        public float valueT2 { get; set; }
        public float valueT3 { get; set; }
        public float valueT4 { get; set; }
        public float valueT5 { get; set; }
        public float valueT6 { get; set; }
        public float valueT7 { get; set; }
        public float valueLow { get; set; }//属性值低限
        public float valueHigh { get; set; }//属性值高限      

    }
}