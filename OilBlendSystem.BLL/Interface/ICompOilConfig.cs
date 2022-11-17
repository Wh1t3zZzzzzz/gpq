using OilBlendSystem.Models.DataBaseModel;
using OilBlendSystem.Models.ConstructModel;

namespace OilBlendSystem.BLL.Interface
{
    public interface ICompOilConfig
    {
        IEnumerable<Compoilconfig> GetAllCompOilConfigList();
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