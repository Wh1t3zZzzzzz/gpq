using OilBlendSystem.Models.Gas.DataBaseModel;
using OilBlendSystem.Models.Gas.ConstructModel;

namespace OilBlendSystem.BLL.Interface.Gas
{
    public interface ICompOilConfig
    {
        IEnumerable<Compoilconfig_gas> GetAllCompOilConfigList();
        // var list = context.Recipecalc1s.ToList();
        // List<COMPONENTOIL> GetCompOilMessageList2();
        // List<DISPATCHRESULT> GetDispatchResult();
       // void UpdateCompOilConfig(Compoilconfig CompOilConfigList);
        // void SaveCompOilConfig(COMPONENTOIL CompOilMessage);

        // void DeleteCompOilConfigByID(int ID);

        // void UpdateCompOilConfig(COMPONENTOIL CompOilMessage);

        // List<COMPONENTOIL> GetAllCompOilList();

        // void SaveCompOilConfig_ID(COMPOIL_ID CompOilMessage_ID);// 智能决策下保存组分油库存

        // void UpdateCompOilConfig_ID(COMPOIL_ID CompOilMessage_ID); // 智能决策下更新组分油库存
        
    }
}