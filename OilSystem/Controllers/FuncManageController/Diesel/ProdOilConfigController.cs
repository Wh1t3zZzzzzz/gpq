using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OilBlendSystem.Models.Diesel.DataBaseModel;
using OilBlendSystem.Models.Diesel.ConstructModel;
using OilBlendSystem.Models;
using OilSystem.ReturnClass;
using OilBlendSystem.BLL.Implementation.Diesel;
using OilBlendSystem.BLL.Interface.Diesel;

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

        // if(40 <= obj.CetLowLimit && obj.CetLowLimit <= obj.CetHighLimit && obj.CetHighLimit <= 70 
        // && 200 <= obj.D50LowLimit && obj.D50LowLimit <= obj.D50HighLimit  && obj.D50HighLimit <= 300
        // && 0 < obj.PolLowLimit && obj.PolLowLimit <= obj.PolHighLimit  && obj.PolHighLimit <= 7
        // && 700 <= obj.DenLowLimit && obj.DenLowLimit <= obj.DenHighLimit  && obj.DenHighLimit <= 900){
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
        msg = "修改成功"
        };            
        // }else{
        //     return new ApiModel(){
        //         code = 500,
        //         //data = JsonConvert.SerializeObject(list),
        //         data = null,
        //         msg = @"成品油属性值高低限应满足以下条件: 
        //         1) 属性值低限小于等于高限
        //         2) 十六烷值指数: [40,70] 
        //         3) 50%回收温度(℃): [200,300] 
        //         4) 多环芳烃含量(wt%): (0,7] 
        //         5) 密度(kg/m³): [700,900]"
        //     };        
        // }


    }


}
