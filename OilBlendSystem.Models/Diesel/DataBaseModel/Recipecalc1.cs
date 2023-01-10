using System;
using System.Collections.Generic;
/// <summary>
/// 配方优化中的第一个组分油产量表格
/// </summary>
namespace OilBlendSystem.Models.Diesel.DataBaseModel
{
    public partial class Recipecalc1
    {
        public int Id { get; set; }//数据库主键ID
        public string? ComOilName { get; set; }//组分油名称
        public float ComOilProductHigh { get; set; }//组分油产量
        public float ComOilProductLow { get; set; }//
        public float AutoFlowHigh { get; set; }//
        public float AutoFlowLow { get; set; }//
        public float ExpFlowHigh { get; set; }//
        public float ExpFlowLow { get; set; }//
        public float Prod1FlowHigh { get; set; }//
        public float Prod1FlowLow { get; set; }//
        public float Prod2FlowHigh { get; set; }//
        public float Prod2FlowLow { get; set; }//
        public float IniVolume { get; set; }//
        public float VolumeHigh { get; set; }//
        public float VolumeLow { get; set; }//

    }
}
