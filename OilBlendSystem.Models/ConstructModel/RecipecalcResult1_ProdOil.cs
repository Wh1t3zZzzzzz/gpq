using System;
using System.Collections.Generic;

namespace OilBlendSystem.Models.ConstructModel
{
    public partial class RecipecalcResult1_ProdOil
    {
        public int Id { get; set; }
        public string? ComOilName { get; set; }//
        public string? ProdOilName { get; set; }//
        //public float VolumeSum { get; set; }//
        public float CETLow { get; set; }//
        public float CETHigh { get; set; }//
        public float D50Low { get; set; }//
        public float D50High { get; set; }//
        public float POLLow { get; set; }//
        public float POLHigh { get; set; }//
        public float DENLow { get; set; }//
        public float DENHigh { get; set; }//
        public float CET { get; set; }//十六烷值
        public float D50 { get; set; }//50%回收温度
        public float POL { get; set; }//多芳烃含量
        public float DEN { get; set; }//密度
    }
}
