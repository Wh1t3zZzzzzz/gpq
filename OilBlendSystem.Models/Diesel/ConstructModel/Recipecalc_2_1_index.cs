namespace OilBlendSystem.Models.Diesel.ConstructModel
{
    public class Recipecalc_2_1_index
    {
        //场景2
        //配方上下限设置表格
        public int index { get; set; }//行的索引
        public string? ComOilName { get; set; }//组分油名称
        public float AutoRecipeHigh { get; set; }//车柴配方上限
        public float AutoRecipeLow { get; set; }//车柴配方下限
        public float ExpRecipeHigh { get; set; }//出柴配方上限
        public float ExpRecipeLow { get; set; }//出柴配方下限
        public float Prod1RecipeHigh { get; set; }//备用成品油1配方上限
        public float Prod1RecipeLow { get; set; }//备用成品油1配方下限
        public float Prod2RecipeHigh { get; set; }//备用成品油2配方上限
        public float Prod2RecipeLow { get; set; }//备用成品油2配方下限

    }
}