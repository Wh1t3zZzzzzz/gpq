using System;
using System.Collections.Generic;
/// <summary>
/// 方案验证中的第一个成品油产量设置表格
/// </summary>
namespace OilBlendSystem.Models.DataBaseModel
{
    public partial class Schemeverify1
    {
        public int Id { get; set; }//数据库主键ID
        public string? ComOilName { get; set; }//组分油名称
        public float AutoQualityProduct { get; set; }//车用柴油质量产量
        public float ExpQualityProduct { get; set; }//出口柴油质量产量
        public float Prod1QualityProduct { get; set; }//备用成品油1质量产量
        public float Prod2QualityProduct { get; set; }//备用成品油2质量产量
        public float AutoFlowPercentMass { get; set; }//车用柴油
        public float ExpFlowPercentMass { get; set; }//出口柴油
        public float Prod1FlowPercentMass { get; set; }//
        public float Prod2FlowPercentMass { get; set; }//
        public float AutoFlowMass { get; set; }//车柴质量流量
        public float ExpFlowMass { get; set; }//出口柴油
        public float Prod1FlowMass { get; set; }//出口柴油
        public float Prod2FlowMass { get; set; }//出口柴油
        public float AutoVolumeProduct { get; set; }//
        public float ExpVolumeProduct { get; set; }//
        public float Prod1VolumeProduct { get; set; }//
        public float Prod2VolumeProduct { get; set; }//
        public float AutoFlowPercentVol { get; set; }//出口柴油质量产量
        public float ExpFlowPercentVol { get; set; }//出口柴油质量产量
        public float Prod1FlowPercentVol { get; set; }//出口柴油质量产量
        public float Prod2FlowPercentVol { get; set; }//出口柴油质量产量
        public float AutoFlowVol { get; set; }//车柴体积流量
        public float ExpFlowVol { get; set; }//出口柴油质量产量
        public float Prod1FlowVol { get; set; }//出口柴油质量产量
        public float Prod2FlowVol { get; set; }//出口柴油质量产量

    }
}
