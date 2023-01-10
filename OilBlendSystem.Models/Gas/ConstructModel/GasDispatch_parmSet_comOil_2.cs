namespace OilBlendSystem.Models.Gas.ConstructModel
{
    public class GasDispatch_parmSet_comOil_2
    {
        //智能决策——参数设置——组分油设置
        //组分油罐容
        public string? ComOilName { get; set; }//组分油名称 ComOil = Component Oil
        public float iniVolume { get; set; }//组分油初始罐容
        public float lowVolume { get; set; }//组分油罐容低限
        public float highVolume { get; set; }//组分油罐容高限

    }
}