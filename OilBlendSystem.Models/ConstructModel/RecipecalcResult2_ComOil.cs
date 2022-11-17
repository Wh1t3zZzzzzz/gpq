﻿using System;
using System.Collections.Generic;

namespace OilBlendSystem.Models.ConstructModel
{
    public partial class RecipecalcResult2_ComOil
    {
        public int Id { get; set; }
        public string? ComOilName { get; set; }//
        public string? ProdOilName { get; set; }//
        public float AutoQualitySum { get; set; }//车柴产量分配总值
        public float ExpQualitySum { get; set; }//车柴产量分配总值
        public float AutoQualityProduct { get; set; }//车柴质量产量
        public float ExpQualityProduct { get; set; }//出柴质量产量
        public float AutoMassRecipe { get; set; }//质量配方
        public float ExpMassRecipe { get; set; }//质量配方

    }
}
