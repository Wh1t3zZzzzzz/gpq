namespace OilBlendSystem.Models.Diesel.ConstructModel
{
    public class Dispatch_decsScheme_comFlowInfo_divT
    {
        //智能决策——决策方案——参调流量信息（按周期划分）
        //第n天的四个成品油的组分油参调流量数据
        public string? ComOilName { get; set; }//组分油名称
        public float prod1ComFlow { get; set; }//第n天，第一个成品油的组分油参调流量
        public float prod2ComFlow { get; set; }
        public float prod3ComFlow { get; set; }
        public float prod4ComFlow { get; set; }


    }
}