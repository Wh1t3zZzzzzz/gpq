using System;
using System.Collections.Generic;
/// <summary>
/// 方案配置中的成品油配置
/// </summary>
namespace OilBlendSystem.Models.Gas.DataBaseModel
{
    public partial class Prodoilconfig_gas
    {
        public int Id { get; set; }//数据库主键ID
        public int Apply { get; set; }//成品油启用标志位

        //下面是成品油指标
        public string? ProdOilName { get; set; }//成品油名称
        public float ronLowLimit { get; set; }//十六烷值低限
        public float ronHighLimit { get; set; }//十六烷值高限
        public float t50LowLimit { get; set; }//50%回收温度低限
        public float t50HighLimit { get; set; }//50%回收温度高限
        public float sufLowLimit { get; set; }//多芳烃含量低限
        public float sufHighLimit { get; set; }//多芳烃含量高限
        public float denLowLimit { get; set; }//密度低限
        public float denHighLimit { get; set; }//密度高限
        //下面是成品油罐容
        public float IniVolume { get; set; }//成品油罐容
        public float ProdVolumeLowLimit { get; set; }//成品油罐容低限 Prod = ProdOil
        public float ProdVolumeHighLimit { get; set; }//成品油罐容高限
        //下面是成品油需求
        public float Demand1LowLimit { get; set; }//第一天的成品油需求低限
        public float Demand1HighLimit { get; set; }//第一天的成品油需求高限
        public float Demand2LowLimit { get; set; }//第二天的成品油需求低限
        public float Demand2HighLimit { get; set; }//第二天的成品油需求高限
        public float Demand3LowLimit { get; set; }
        public float Demand3HighLimit { get; set; }
        public float Demand4LowLimit { get; set; }
        public float Demand4HighLimit { get; set; }
        public float Demand5LowLimit { get; set; }
        public float Demand5HighLimit { get; set; }
        public float Demand6LowLimit { get; set; }
        public float Demand6HighLimit { get; set; }
        public float Demand7LowLimit { get; set; }
        public float Demand7HighLimit { get; set; }
    }
}
