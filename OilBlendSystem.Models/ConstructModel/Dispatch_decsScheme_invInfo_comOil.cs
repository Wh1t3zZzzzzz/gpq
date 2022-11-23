namespace OilBlendSystem.Models.ConstructModel
{
    public class Dispatch_decsScheme_invInfo_comOil
    {
        //智能决策——决策方案——库存信息（按油品划分——>组分油还是成品油）
        //组分油库存信息
        public string? ComOilName { get; set; }//组分油名称 ComOil = Component Oil
        public float volumeT1 { get; set; }//组分油第一天库存
        public float volumeT2 { get; set; }
        public float volumeT3 { get; set; }
        public float volumeT4 { get; set; }
        public float volumeT5 { get; set; }
        public float volumeT6 { get; set; }
        public float volumeT7 { get; set; }


    }
}