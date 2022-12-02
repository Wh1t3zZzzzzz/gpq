using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OilBlendSystem.Models.DataBaseModel;
using OilBlendSystem.Models.ConstructModel;
using OilSystem.ReturnClass;
using OilBlendSystem.BLL.Implementation;
using OilBlendSystem.BLL.Interface;
using Newtonsoft.Json;
namespace OilSystem.Controllers;
//using System.Data.Entity;
[ApiController]
[Route("[controller]")]
public class ProdOilConfigController : ControllerBase
{

    private readonly oilblendContext context;

    public ProdOilConfigController(oilblendContext _context)
    {
       context = _context;
    }

    [HttpGet]
    //public IEnumerable<TestTable> Get()//model里的名字
    public ApiModel Get()//model里的名字 多个数据用IEnumberable，单个数据不用
    {
        var list = context.Prodoilconfigs.ToList();
        List<Prodproperty> ResultList = new List<Prodproperty>();//列表，里面可以添加很多个对象
        
        for(int i = 0; i < list.Count; i++){
            Prodproperty result = new Prodproperty();//实体，可以理解为一个对象  
            result.ProdOilName = list[i].ProdOilName;
            result.CetHighLimit = list[i].CetHighLimit;
            result.CetLowLimit = list[i].CetLowLimit;
            result.D50HighLimit = list[i].D50HighLimit;
            result.D50LowLimit = list[i].D50LowLimit;
            result.PolHighLimit = list[i].PolHighLimit;
            result.PolLowLimit = list[i].PolLowLimit;
            result.DenHighLimit = list[i].DenHighLimit;
            result.DenLowLimit = list[i].DenLowLimit;
            ResultList.Add(result); 
        }   
        return new ApiModel()
        {
        code = 200,
        data = ResultList,
        msg = "查询成功"
        };

    }


    [HttpPut]
    public ApiModel Put(Prodproperty_index obj)
    {
        context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
        IProdOilConfig _ProdOilConfig = new ProdOilConfig(context);
        var list = _ProdOilConfig.GetAllProdOilConfigList().ToList();//需要把IEnumberable中遍历成List
        var list2 = context.Recipecalc3s.ToList();
        var list3 = context.Schemeverify2s.ToList();

        list[obj.index].ProdOilName = obj.ProdOilName;
        list2[obj.index].ProdOilName = obj.ProdOilName;
        list3[obj.index].ProdOilName = obj.ProdOilName;
        list[obj.index].CetHighLimit = obj.CetHighLimit;
        list[obj.index].CetLowLimit = obj.CetLowLimit;
        list[obj.index].D50HighLimit = obj.D50HighLimit;
        list[obj.index].D50LowLimit = obj.D50LowLimit;
        list[obj.index].PolHighLimit = obj.PolHighLimit;
        list[obj.index].PolLowLimit = obj.PolLowLimit;
        list[obj.index].DenHighLimit = obj.DenHighLimit;
        list[obj.index].DenLowLimit = obj.DenLowLimit;

        context.Prodoilconfigs.Update(list[obj.index]);
        context.Recipecalc3s.Update(list2[obj.index]);
        context.Schemeverify2s.Update(list3[obj.index]);
        context.SaveChanges();

        return new ApiModel()
        {
        code = 200,
        //data = JsonConvert.SerializeObject(list),
        data = list,
        msg = "查询成功"
        };
    }


}
