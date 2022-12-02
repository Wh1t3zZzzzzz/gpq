namespace OilBlendSystem.Models.ConstructModel
{
    public class Dispatch_parmSet_comOil_3_index
    {
        //智能决策——参数设置——组分油设置
        //组分油计划产量
        public int index { get; set; }//行的索引
        public string? ComOilName { get; set; }//组分油名称 ComOil = Component Oil
        public float comPlanProduct1 { get; set; }//第一天的组分油计划产量
        public float comPlanProduct2 { get; set; }   
        public float comPlanProduct3 { get; set; } 
        public float comPlanProduct4 { get; set; } 
        public float comPlanProduct5 { get; set; } 
        public float comPlanProduct6 { get; set; } 
        public float comPlanProduct7 { get; set; }  


    }
}