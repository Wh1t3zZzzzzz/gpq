using System;
using System.Collections.Generic;

namespace OilBlendSystem.Models.ConstructModel
{
    public partial class RecipecalcResult2Calc
    {
        public int Id { get; set; }
        public string? ComOilName { get; set; }//
        //public float VolumeSum { get; set; }//
        public float AutoFlowLow { get; set; }//车柴低限
        public float AutoFlowHigh { get; set; }//车柴高限
        public float ExpFlowLow { get; set; }//出柴低限
        public float ExpFlowHigh { get; set; }//出柴高限
    }
}
