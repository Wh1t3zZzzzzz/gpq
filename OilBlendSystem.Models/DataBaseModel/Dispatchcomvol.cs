using System;
using System.Collections.Generic;
/// <summary>
/// 配方优化中的第一个组分油产量表格
/// </summary>
namespace OilBlendSystem.Models.DataBaseModel
{
    public partial class Dispatchcomvol
    {
        public int Id { get; set; }//数据库主键ID
        public int ComIns { get; set; }//无实际意义
        public float ComVolT1 { get; set; }//组分油库存
        public float ComVolT2 { get; set; }//
        public float ComVolT3 { get; set; }//
        public float ComVolT4 { get; set; }//
        public float ComVolT5 { get; set; }//
        public float ComVolT6 { get; set; }//
        public float ComVolT7 { get; set; }//

    }
}
