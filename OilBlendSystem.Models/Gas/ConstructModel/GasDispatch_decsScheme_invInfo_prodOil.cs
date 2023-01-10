namespace OilBlendSystem.Models.Gas.ConstructModel
{
    public class GasDispatch_decsScheme_invInfo_prodOil
    {
        //智能决策——决策方案——库存信息（按油品划分——>组分油还是成品油）
        //成品油库存信息
        public string? ProdOilName { get; set; }//成品油名称
        public float volumeT1 { get; set; }//成品油第一天库存
        public float volumeT2 { get; set; }
        public float volumeT3 { get; set; }
        public float volumeT4 { get; set; }
        public float volumeT5 { get; set; }
        public float volumeT6 { get; set; }
        public float volumeT7 { get; set; }


    }
}