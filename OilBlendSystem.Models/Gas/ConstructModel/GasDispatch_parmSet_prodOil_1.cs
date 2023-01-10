namespace OilBlendSystem.Models.Gas.ConstructModel
{
    public class GasDispatch_parmSet_prodOil_1
    {
        //智能决策——参数设置——成品油设置
        //成品油罐容
        public string? ProdOilName { get; set; }//成品油名称
        public float iniVolume { get; set; }//成品油初始罐容
        public float lowVolume { get; set; }//成品油罐容低限
        public float highVolume { get; set; }//成品油罐容高限


    }
}