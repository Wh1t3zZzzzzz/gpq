using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OilBlendSystem.Models.Gas.DataBaseModel;
using OilBlendSystem.Models.Gas.ConstructModel;
using OilBlendSystem.Models;
using OilSystem.ReturnClass;
using OilBlendSystem.BLL.Implementation.Gas;
using OilBlendSystem.BLL.Interface.Gas;

namespace OilSystem.Controllers;
//using System.Data.Entity;
[ApiController]
[Route("[controller]")]
public class ProdOilConfigGasController : ControllerBase
{

    private readonly oilblendContext context;

    public ProdOilConfigGasController(oilblendContext _context)
    {
       context = _context;
    }

    [HttpGet]
    //public IEnumerable<TestTable> Get()//model里的名字
    public ApiModel Get()//model里的名字 多个数据用IEnumberable，单个数据不用
    {
        var list = context.Prodoilconfig_gases.ToList();
        List<GasProdproperty> ResultList = new List<GasProdproperty>();//列表，里面可以添加很多个对象
        
        for(int i = 0; i < list.Count; i++){
            GasProdproperty result = new GasProdproperty();//实体，可以理解为一个对象  
            result.ProdOilName = list[i].ProdOilName;
            result.ronHighLimit = list[i].ronHighLimit;
            result.ronLowLimit = list[i].ronLowLimit;
            result.t50HighLimit = list[i].t50HighLimit;
            result.t50LowLimit = list[i].t50LowLimit;
            result.sufHighLimit = list[i].sufHighLimit;
            result.sufLowLimit = list[i].sufLowLimit;
            result.denHighLimit = list[i].denHighLimit;
            result.denLowLimit = list[i].denLowLimit;
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
    public ApiModel Put(GasProdproperty_index obj)
    {
        context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
        IProdOilConfig _ProdOilConfig = new ProdOilConfig(context);
        var list = _ProdOilConfig.GetAllProdOilConfigList().ToList();//需要把IEnumberable中遍历成List
        var list2 = context.Recipecalc3_gases.ToList();
        var list3 = context.Schemeverify2_gases.ToList();

        // if(40 <= obj.ronLowLimit && obj.ronLowLimit <= obj.ronHighLimit && obj.ronHighLimit <= 70 
        // && 200 <= obj.t50LowLimit && obj.t50LowLimit <= obj.t50HighLimit  && obj.t50HighLimit <= 300
        // && 0 < obj.sufLowLimit && obj.sufLowLimit <= obj.sufHighLimit  && obj.sufHighLimit <= 7
        // && 700 <= obj.denLowLimit && obj.denLowLimit <= obj.denHighLimit  && obj.denHighLimit <= 900){
        list[obj.index].ProdOilName = obj.ProdOilName;
        list2[obj.index].ProdOilName = obj.ProdOilName;
        list3[obj.index].ProdOilName = obj.ProdOilName;

        list[obj.index].ronHighLimit = obj.ronHighLimit;
        list[obj.index].ronLowLimit = obj.ronLowLimit;
        list[obj.index].t50HighLimit = obj.t50HighLimit;
        list[obj.index].t50LowLimit = obj.t50LowLimit;
        list[obj.index].sufHighLimit = obj.sufHighLimit;
        list[obj.index].sufLowLimit = obj.sufLowLimit;
        list[obj.index].denHighLimit = obj.denHighLimit;
        list[obj.index].denLowLimit = obj.denLowLimit;

        context.Prodoilconfig_gases.Update(list[obj.index]);
        context.Recipecalc3_gases.Update(list2[obj.index]);
        context.Schemeverify2_gases.Update(list3[obj.index]);
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
