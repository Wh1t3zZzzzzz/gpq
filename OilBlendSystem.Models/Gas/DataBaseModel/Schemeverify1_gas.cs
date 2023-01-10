using System;
using System.Collections.Generic;
/// <summary>
/// 方案验证中的第一个成品油产量设置表格
/// </summary>
namespace OilBlendSystem.Models.Gas.DataBaseModel
{
    public partial class Schemeverify1_gas
    {
        public int Id { get; set; }//数据库主键ID
        public string? ComOilName { get; set; }//组分油名称
        public float gas92QualityProduct { get; set; }//车用柴油质量产量
        public float gas95QualityProduct { get; set; }//出口柴油质量产量
        public float gas98QualityProduct { get; set; }//备用成品油1质量产量
        public float gasSelfQualityProduct { get; set; }//备用成品油2质量产量
        public float gas92FlowPercentMass { get; set; }//车用柴油
        public float gas95FlowPercentMass { get; set; }//出口柴油
        public float gas98FlowPercentMass { get; set; }//
        public float gasSelfFlowPercentMass { get; set; }//
        public float gas92FlowMass { get; set; }//车柴质量流量
        public float gas95FlowMass { get; set; }//出口柴油
        public float gas98FlowMass { get; set; }//出口柴油
        public float gasSelfFlowMass { get; set; }//出口柴油
        public float gas92VolumeProduct { get; set; }//
        public float gas95VolumeProduct { get; set; }//
        public float gas98VolumeProduct { get; set; }//
        public float gasSelfVolumeProduct { get; set; }//
        public float gas92FlowPercentVol { get; set; }//出口柴油质量产量
        public float gas95FlowPercentVol { get; set; }//出口柴油质量产量
        public float gas98FlowPercentVol { get; set; }//出口柴油质量产量
        public float gasSelfFlowPercentVol { get; set; }//出口柴油质量产量
        public float gas92FlowVol { get; set; }//车柴体积流量
        public float gas95FlowVol { get; set; }//出口柴油质量产量
        public float gas98FlowVol { get; set; }//出口柴油质量产量
        public float gasSelfFlowVol { get; set; }//出口柴油质量产量

    }
}
