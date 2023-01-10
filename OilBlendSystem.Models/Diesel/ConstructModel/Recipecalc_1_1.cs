namespace OilBlendSystem.Models.Diesel.ConstructModel
{
    public class Recipecalc_1_1
    {
        //场景1
        //组分油参调范围设置表格
        public string? ComOilName { get; set; }//组分油名称
        public float ComOilProductHigh { get; set; }//组分油参调范围（产量）上限
        public float ComOilProductLow { get; set; }//组分油参调范围（产量）下限

    }
}