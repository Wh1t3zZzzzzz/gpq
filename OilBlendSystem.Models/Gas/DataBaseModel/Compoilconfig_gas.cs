/// <summary>
/// 方案配置中的组分油配置
/// </summary>
namespace OilBlendSystem.Models.Gas.DataBaseModel
{
    public partial class Compoilconfig_gas
    {
        public int Id { get; set; }//数据库主键ID
        //下面是组分油属性
        public string? ComOilName { get; set; }//组分油名称 ComOil = Component Oil
        public float ron { get; set; }//十六烷值指数
        public float t50 { get; set; }//50%回收温度
        public float suf { get; set; }//多芳烃含量
        public float den { get; set; }//密度
        public float Price { get; set; }//价格
        //下面是组分油高低限的组分油参调流量高低限（配方优化）表格
        public float gas92Low1 { get; set; }//车柴低限 gas92 = gas92mobile（汽车）代表车用柴油 gas95 = gas95ort 代表出口柴油
        public float gas92High1 { get; set; }//车柴高限
        public float gas95Low1 { get; set; }//出柴低限
        public float gas95High1 { get; set; }//出柴高限
        public float gas98Low1 { get; set; }//低限
        public float gas98High1 { get; set; }//高限
        public float gasSelfLow1 { get; set; }//低限
        public float gasSelfHigh1 { get; set; }//高限
        //下面是组分油参调流量高低限（智能决策）表格
        public float gas92Low2 { get; set; }//车柴低限
        public float gas92High2 { get; set; }//车柴高限
        public float gas95Low2 { get; set; }//出柴低限
        public float gas95High2 { get; set; }//出柴高限
        public float gas98Low2 { get; set; }//低限
        public float gas98High2 { get; set; }//高限
        public float gasSelfLow2 { get; set; }//低限
        public float gasSelfHigh2 { get; set; }//高限
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
