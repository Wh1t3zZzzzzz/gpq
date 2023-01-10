/// <summary>
/// 方案配置中的组分油配置
/// </summary>
namespace OilBlendSystem.Models.Diesel.DataBaseModel
{
    public partial class Compoilconfig
    {
        public int Id { get; set; }//数据库主键ID
        //下面是组分油属性
        public string? ComOilName { get; set; }//组分油名称 ComOil = Component Oil
        public float Cet { get; set; }//十六烷值指数
        public float D50 { get; set; }//50%回收温度
        public float Pol { get; set; }//多芳烃含量
        public float Den { get; set; }//密度
        public float Price { get; set; }//价格
        //下面是组分油高低限的组分油参调流量高低限（配方优化）表格
        public float AutoLow1 { get; set; }//车柴低限 Auto = automobile（汽车）代表车用柴油 Exp = export 代表出口柴油
        public float AutoHigh1 { get; set; }//车柴高限
        public float ExpLow1 { get; set; }//出柴低限
        public float ExpHigh1 { get; set; }//出柴高限
        public float Prod1Low1 { get; set; }//低限
        public float Prod1High1 { get; set; }//高限
        public float Prod2Low1 { get; set; }//低限
        public float Prod2High1 { get; set; }//高限
        //下面是组分油参调流量高低限（智能决策）表格
        public float AutoLow2 { get; set; }//车柴低限
        public float AutoHigh2 { get; set; }//车柴高限
        public float ExpLow2 { get; set; }//出柴低限
        public float ExpHigh2 { get; set; }//出柴高限
        public float Prod1Low2 { get; set; }//低限
        public float Prod1High2 { get; set; }//高限
        public float Prod2Low2 { get; set; }//低限
        public float Prod2High2 { get; set; }//高限
        //下面是组分油罐容
        public float IniVolume { get; set; }//初始罐容
        public float HighVolume { get; set; }//罐容高限
        public float LowVolume { get; set; }//罐容低限
        //下面是组分油计划产量
        public float PlanProduct1 { get; set; }//第一天的组分油计划产量
        public float PlanProduct2 { get; set; }//第二天的组分油计划产量
        public float PlanProduct3 { get; set; }
        public float PlanProduct4 { get; set; }
        public float PlanProduct5 { get; set; }
        public float PlanProduct6 { get; set; }
        public float PlanProduct7 { get; set; }
    }
}
