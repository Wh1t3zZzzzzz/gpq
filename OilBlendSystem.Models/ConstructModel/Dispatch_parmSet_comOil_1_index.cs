namespace OilBlendSystem.Models.ConstructModel
{
    public class Dispatch_parmSet_comOil_1_index
    {
        //智能决策——参数设置——组分油设置
        //组分油参调流量上下限
        public int index { get; set; }//行的索引
        public string? comOilName { get; set; }//组分油名称 ComOil = Component Oil
        public float prod1LowLimit { get; set; }//第一个成品油低限
        public float prod1HighLimit { get; set; }//第一个成品油高限
        public float prod2LowLimit { get; set; }//第二个成品油低限
        public float prod2HighLimit { get; set; }//第二个成品油高限
        public float prod3LowLimit { get; set; }//第三个成品油低限
        public float prod3HighLimit { get; set; }//第三个成品油高限
        public float prod4LowLimit { get; set; }//第四个成品油低限
        public float prod4HighLimit { get; set; }//第四个成品油高限




    }
}