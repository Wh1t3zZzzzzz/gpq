using System;
using System.Collections.Generic;
/// <summary>
/// 配方优化中的第三个成品油产量限制表格
/// </summary>
namespace OilBlendSystem.Models.Diesel.DataBaseModel
{
    public partial class Recipecalc3
    {
        public int Id { get; set; }//数据库主键ID
        public string? ProdOilName { get; set; }//成品油名称
        public float ProdOilProduct { get; set; }//产品产量高限
        public float ProdOilProductLow { get; set; }//产品产量低限
        public float TotalFlow { get; set; }//
        public float TotalFlow2 { get; set; }//
        public int Apply { get; set; }//成品油启用标志位
    }
}
