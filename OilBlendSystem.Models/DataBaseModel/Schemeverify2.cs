using System;
using System.Collections.Generic;
/// <summary>
/// 方案验证中的第二个罐底油信息配置表格
/// </summary>
namespace OilBlendSystem.Models.DataBaseModel
{
    public partial class Schemeverify2
    {
        public int Id { get; set; }//数据库主键ID
        public string? ProdOilName { get; set; }//成品油名称
        public float BottomVolume { get; set; }//罐底油体积
        public float CetVol { get; set; }//十六烷值
        public float D50Vol { get; set; }//50%回收温度
        public float PolVol { get; set; }//多芳烃含量
        public float DenVol { get; set; }// density 密度
        public float TotalBlendVol { get; set; }// 调合总量（质量）
        public float TotalBlendVol2 { get; set; }// 调合总量（质量）
        public float TotalBlendVol3 { get; set; }// 调合总量（质量）
        public float BottomMass { get; set; }//罐底油体积
        public float CetMass { get; set; }//十六烷值
        public float D50Mass { get; set; }//50%回收温度
        public float PolMass { get; set; }//多芳烃含量
        public float DenMass { get; set; }// density 密度
        public float TotalBlendMass { get; set; }// 调合总量（质量）
        public float TotalBlendMass2 { get; set; }// 调合总量（质量）
        public float TotalBlendMass3 { get; set; }// 调合总量（质量）
    }
}
