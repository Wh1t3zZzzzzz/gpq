namespace OilBlendSystem.Models.Diesel.ConstructModel
{
    public class Recipecalc_1_3_index
    {
        //场景1
        //成品油产量限制（上限）表格
        public int index { get; set; }//行的索引
        public string? ProdOilName { get; set; }//成品油名称
        public float ProdOilProduct { get; set; }//成品油产量上限

    }
}