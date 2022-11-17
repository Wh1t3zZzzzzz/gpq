using System;
using System.Collections.Generic;
/// <summary>
/// 配方优化中的第一个组分油产量表格
/// </summary>
namespace OilBlendSystem.Models.DataBaseModel
{
    public partial class Dispatchprodpick
    {
        public int Id { get; set; }//数据库主键ID
        public float ProdPickT1 { get; set; }//成品油提货量
        public float ProdPickT2 { get; set; }//
        public float ProdPickT3 { get; set; }//
        public float ProdPickT4 { get; set; }//
        public float ProdPickT5 { get; set; }//
        public float ProdPickT6 { get; set; }//
        public float ProdPickT7 { get; set; }//

    }
}
