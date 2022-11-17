using System;
using System.Collections.Generic;
/// <summary>
/// 配方优化中的第一个组分油产量表格
/// </summary>
namespace OilBlendSystem.Models.DataBaseModel
{
    public partial class Dispatchcomflow
    {
        public int Id { get; set; }//数据库主键ID
        public int ProdIns { get; set; }//成品油标识 0代表第一个 1代表第二个
        public float ComFlowT1 { get; set; }//组分油参调流量
        public float ComFlowT2 { get; set; }//
        public float ComFlowT3 { get; set; }//
        public float ComFlowT4 { get; set; }//
        public float ComFlowT5 { get; set; }//
        public float ComFlowT6 { get; set; }//
        public float ComFlowT7 { get; set; }//

    }
}
