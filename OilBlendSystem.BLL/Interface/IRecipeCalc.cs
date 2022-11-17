using OilBlendSystem.Models.DataBaseModel;
using OilBlendSystem.Models.ConstructModel;

namespace OilBlendSystem.BLL.Interface
{
    public interface IRecipeCalc
    {
        IEnumerable<Recipecalc1> GetRecipeCalc1();
        IEnumerable<Recipecalc2> GetRecipeCalc2();
        IEnumerable<Recipecalc3> GetRecipeCalc3();
        double[] GetRecipe1();
        double[] GetRecipe2();
        double[] GetRecipe3();
        IEnumerable<Recipecalc_1Res_1> GetRecipecalc_1Res_ComOilSugProduct();//场景1 计算结果：组分油产量分配（组分油建议产量）
        IEnumerable<Recipecalc_1Res_2> GetRecipecalc_1Res_ProdOilProduct();//场景1 计算结果：成品油质量产量
        IEnumerable<Recipecalc_1Res_3> GetRecipecalc_1Res_Recipe();//场景1 计算结果：成品油优化配方
        IEnumerable<Recipecalc_1Res_4> GetRecipecalc_1Res_Property();//场景1 计算结果：成品油属性
        IEnumerable<Recipecalc_2Res_1> GetRecipecalc_2Res_ProdOilProduct();//场景2 计算结果：成品油质量产量
        IEnumerable<Recipecalc_2Res_2> GetRecipecalc_2Res_Recipe();//场景2 计算结果：成品油优化配方
        IEnumerable<Recipecalc_2Res_3> GetRecipecalc_2Res_Property();//场景2 计算结果：成品油属性
        IEnumerable<Recipecalc_3Res_1> GetRecipecalc_3Res_ProdOilProduct();//场景3 计算结果：成品油质量产量
        IEnumerable<Recipecalc_3Res_2> GetRecipecalc_3Res_Recipe();//场景3 计算结果：成品油优化配方
        IEnumerable<Recipecalc_3Res_3> GetRecipecalc_3Res_Property();//场景3 计算结果：成品油属性

    }
}