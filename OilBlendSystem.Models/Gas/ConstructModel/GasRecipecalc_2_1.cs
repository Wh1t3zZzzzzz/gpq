namespace OilBlendSystem.Models.Gas.ConstructModel
{
    public class GasRecipecalc_2_1
    {
        //场景2
        //配方上下限设置表格
        public string? ComOilName { get; set; }//组分油名称
        public float gas92RecipeHigh { get; set; }//车柴配方上限
        public float gas92RecipeLow { get; set; }//车柴配方下限
        public float gas95RecipeHigh { get; set; }//出柴配方上限
        public float gas95RecipeLow { get; set; }//出柴配方下限
        public float gas98RecipeHigh { get; set; }//备用成品油1配方上限
        public float gas98RecipeLow { get; set; }//备用成品油1配方下限
        public float gasSelfRecipeHigh { get; set; }//备用成品油2配方上限
        public float gasSelfRecipeLow { get; set; }//备用成品油2配方下限

    }
}