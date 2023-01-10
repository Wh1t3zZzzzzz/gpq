using System;
using System.Collections.Generic;
/// <summary>
/// 配方优化中的第二个多目标权值设置表格
/// </summary>
namespace OilBlendSystem.Models.Diesel.DataBaseModel
{
    public partial class Recipecalc2
    {
        public int Id { get; set; }//数据库主键ID
        public string? WeightName { get; set; }//权值对应的名字
        public float Weight { get; set; }//权值对应的值
        public int ProdOilStatus { get; set; }//成品油标志位
        public int Apply { get; set; }//成品油启用标志

    }
}
