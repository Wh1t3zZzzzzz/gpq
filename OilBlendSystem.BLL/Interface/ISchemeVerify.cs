using OilBlendSystem.Models.DataBaseModel;
using OilBlendSystem.Models.ConstructModel;

namespace OilBlendSystem.BLL.Interface
{
    public interface ISchemeVerify
    {
        IEnumerable<Schemeverify1> GetSchemeVerify1();
        IEnumerable<Schemeverify2> GetSchemeVerify2();
        IEnumerable<SchemeVerify_1Res_1> GetSchemeverifyResult1_mass_Time();//场景1质量 调合时间
        IEnumerable<SchemeVerify_1Res_1> GetSchemeverifyResult1_vol_Time();//场景1体积 调合时间
        IEnumerable<SchemeVerify_1Res_2> GetSchemeverifyResult1_mass_Product();//场景1质量 成品油产量
        IEnumerable<SchemeVerify_1Res_2> GetSchemeverifyResult1_vol_Product();//场景1体积 成品油产量
        IEnumerable<SchemeVerify_1Res_3> GetSchemeverifyResult1_mass_Property();//场景1质量 成品油属性
        IEnumerable<SchemeVerify_1Res_3> GetSchemeverifyResult1_vol_Property();//场景1体积 成品油属性
        IEnumerable<SchemeVerify_2Res_1> GetSchemeverifyResult2_mass_Product();//场景2质量 成品油产量
        IEnumerable<SchemeVerify_2Res_1> GetSchemeverifyResult2_vol_Product();//场景2体积 成品油产量
        IEnumerable<SchemeVerify_2Res_2> GetSchemeverifyResult2_mass_Property();//场景2质量 成品油属性
        IEnumerable<SchemeVerify_2Res_2> GetSchemeverifyResult2_vol_Property();//场景2体积 成品油属性
        IEnumerable<SchemeVerify_3Res_1> GetSchemeverifyResult3_mass_Time();//场景3质量 调合时间
        IEnumerable<SchemeVerify_3Res_1> GetSchemeverifyResult3_vol_Time();//场景3体积 调合时间
        IEnumerable<SchemeVerify_3Res_2> GetSchemeverifyResult3_mass_Product();//场景3质量 成品油产量
        IEnumerable<SchemeVerify_3Res_2> GetSchemeverifyResult3_vol_Product();//场景3体积 成品油产量
        IEnumerable<SchemeVerify_3Res_3> GetSchemeverifyResult3_mass_Property();//场景3质量 成品油属性
        IEnumerable<SchemeVerify_3Res_3> GetSchemeverifyResult3_vol_Property();//场景3体积 成品油属性


    }
}