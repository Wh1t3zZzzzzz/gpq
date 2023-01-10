namespace OilBlendSystem.Models.Gas.ConstructModel
{
    public class GasRecipecalc_1_1_index
    {
        //场景1
        //组分油参调范围设置表格
        public int index { get; set; }//行的索引
        public string? ComOilName { get; set; }//组分油名称
        public float ComOilProductHigh { get; set; }//组分油参调范围（产量）上限
        public float ComOilProductLow { get; set; }//组分油参调范围（产量）下限

    }
}