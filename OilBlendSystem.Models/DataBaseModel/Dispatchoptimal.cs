using System;
using System.Collections.Generic;
/// <summary>
/// 配方优化中的第一个组分油产量表格
/// </summary>
namespace OilBlendSystem.Models.DataBaseModel
{
    public partial class Dispatchoptimal
    {
        public int Id { get; set; }//数据库主键ID
        public string? Optimal_stage{ get; set; }//求解器状态位
        public float Optimal_obj { get; set; }//目标函数值


    }
}
