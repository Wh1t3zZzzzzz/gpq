using System;
using System.Collections.Generic;
/// <summary>
/// 配方优化中的第一个组分油产量表格
/// </summary>
namespace OilBlendSystem.Models.DataBaseModel
{
    public partial class Dispatchprodvol
    {
        public int Id { get; set; }//数据库主键ID
        public float ProdVolT1 { get; set; }//成品油库存
        public float ProdVolT2 { get; set; }//
        public float ProdVolT3 { get; set; }//
        public float ProdVolT4 { get; set; }//
        public float ProdVolT5 { get; set; }//
        public float ProdVolT6 { get; set; }//
        public float ProdVolT7 { get; set; }//

    }
}
