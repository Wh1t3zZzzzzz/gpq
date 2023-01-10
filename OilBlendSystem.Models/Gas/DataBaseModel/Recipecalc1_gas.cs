using System;
using System.Collections.Generic;
/// <summary>
/// 配方优化中的第一个组分油产量表格
/// </summary>
namespace OilBlendSystem.Models.Gas.DataBaseModel
{
    public partial class Recipecalc1_gas
    {
        public int Id { get; set; }//数据库主键ID
        public string? ComOilName { get; set; }//组分油名称
        public float ComOilProductHigh { get; set; }//组分油产量
        public float ComOilProductLow { get; set; }//
        public float gas92FlowHigh { get; set; }//
        public float gas92FlowLow { get; set; }//
        public float gas95FlowHigh { get; set; }//
        public float gas95FlowLow { get; set; }//
        public float gas98FlowHigh { get; set; }//
        public float gas98FlowLow { get; set; }//
        public float gasSelfFlowHigh { get; set; }//
        public float gasSelfFlowLow { get; set; }//
        public float IniVolume { get; set; }//
        public float VolumeHigh { get; set; }//
        public float VolumeLow { get; set; }//

    }
}
