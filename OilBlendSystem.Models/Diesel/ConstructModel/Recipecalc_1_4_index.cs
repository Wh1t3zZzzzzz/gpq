namespace OilBlendSystem.Models.Diesel.ConstructModel
{
    public class Recipecalc_1_4_index
    {
        //场景1
        //组分油罐容设置表格
        public int index { get; set; }//行的索引
        public string? ComOilName { get; set; }//组分油名称
        public float IniVolume { get; set; }//组分油初始罐容
        public float HighVolume { get; set; }//组分油罐容高限
        public float LowVolume { get; set; }//组分油罐容低限

    }
}