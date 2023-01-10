namespace OilBlendSystem.Models.Gas.ConstructModel
{
    public class GasDispatch_parmSet_comOil_1
    {
        //智能决策——参数设置——组分油设置
        //组分油参调流量上下限
        public string? comOilName { get; set; }//组分油名称 ComOil = Component Oil
        public float gas92LowLimit { get; set; }//第一个成品油低限
        public float gas92HighLimit { get; set; }//第一个成品油高限
        public float gas95LowLimit { get; set; }//第二个成品油低限
        public float gas95HighLimit { get; set; }//第二个成品油高限
        public float gas98LowLimit { get; set; }//第三个成品油低限
        public float gas98HighLimit { get; set; }//第三个成品油高限
        public float gasSelfLowLimit { get; set; }//第四个成品油低限
        public float gasSelfHighLimit { get; set; }//第四个成品油高限




    }
}