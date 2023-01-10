using OilBlendSystem.Models.Gas.DataBaseModel;
using OilBlendSystem.Models.Gas.ConstructModel;

namespace OilBlendSystem.BLL.Interface.Gas
{
    public interface IRecipeCalc
    {
        IEnumerable<Recipecalc1_gas> GetRecipeCalc1();
        IEnumerable<Recipecalc2_gas> GetRecipeCalc2();
        IEnumerable<Recipecalc2_2_gas> GetRecipeCalc2_2();
        IEnumerable<Recipecalc2_3_gas> GetRecipeCalc2_3();
        IEnumerable<Recipecalc3_gas> GetRecipeCalc3();
        double[] GetRecipe1();
        double[] GetRecipe2();
        double[] GetRecipe3();
        IEnumerable<GasRecipecalc_1Res_1> GetRecipecalc_1Res_ComOilSugProduct();//场景1 计算结果：组分油产量分配（组分油建议产量）
        IEnumerable<GasRecipecalc_1Res_2> GetRecipecalc_1Res_ProdOilProduct();//场景1 计算结果：成品油质量产量
        IEnumerable<GasRecipecalc_1Res_3> GetRecipecalc_1Res_Recipe();//场景1 计算结果：成品油优化配方
        IEnumerable<GasRecipecalc_1Res_4> GetRecipecalc_1Res_Property();//场景1 计算结果：成品油属性
        IEnumerable<GasRecipecalc_2Res_1> GetRecipecalc_2Res_ProdOilProduct();//场景2 计算结果：成品油质量产量
        IEnumerable<GasRecipecalc_2Res_2> GetRecipecalc_2Res_Recipe();//场景2 计算结果：成品油优化配方
        IEnumerable<GasRecipecalc_2Res_3> GetRecipecalc_2Res_Property();//场景2 计算结果：成品油属性
        IEnumerable<GasRecipecalc_3Res_1> GetRecipecalc_3Res_ProdOilProduct();//场景3 计算结果：成品油质量产量
        IEnumerable<GasRecipecalc_3Res_2> GetRecipecalc_3Res_Recipe();//场景3 计算结果：成品油优化配方
        IEnumerable<GasRecipecalc_3Res_3> GetRecipecalc_3Res_Property();//场景3 计算结果：成品油属性

    }
}