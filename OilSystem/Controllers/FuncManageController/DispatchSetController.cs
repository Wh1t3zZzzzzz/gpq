using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OilBlendSystem.Models.DataBaseModel;
using OilBlendSystem.Models.ConstructModel;
using OilSystem.ReturnClass;
using Newtonsoft.Json;
namespace OilSystem.Controllers;
using OilBlendSystem.BLL.Implementation;
using OilBlendSystem.BLL.Interface;

//using System.Data.Entity;
[ApiController]
[Route("[controller]")]
public class DispatchSetController : ControllerBase
{
    //智能决策模块求解控制器

    private readonly oilblendContext context;

    public DispatchSetController(oilblendContext _context)
    {
       context = _context;
    }

    [HttpGet("Set/comFlowLimit")]
    //组分油参调流量上下限
    public ApiModel Set1()//model里的名字 多个数据用IEnumberable，单个数据不用
    {   
        var comFlowLimitList = context.Compoilconfigs.ToList();
        List<Dispatch_parmSet_comOil_1> ResultList = new List<Dispatch_parmSet_comOil_1>();//列表，里面可以添加很多个对象
        for(int i = 0; i < comFlowLimitList.Count; i++){
            Dispatch_parmSet_comOil_1 result = new Dispatch_parmSet_comOil_1();//实体，可以理解为一个对象  
            result.comOilName = comFlowLimitList[i].ComOilName;
            result.prod1HighLimit = comFlowLimitList[i].AutoHigh2;
            result.prod1LowLimit = comFlowLimitList[i].AutoLow2;
            result.prod2HighLimit = comFlowLimitList[i].ExpHigh2;
            result.prod2LowLimit = comFlowLimitList[i].ExpLow2;
            result.prod3HighLimit = comFlowLimitList[i].Prod1High2;
            result.prod3LowLimit = comFlowLimitList[i].Prod1Low2;
            result.prod4HighLimit = comFlowLimitList[i].Prod2High2;
            result.prod4LowLimit = comFlowLimitList[i].Prod2Low2;
            ResultList.Add(result);
        }    

        return new ApiModel()
        {
        code = 200,
        //data = JsonConvert.SerializeObject(list),
        data = ResultList,
        msg = "查询成功"
        };
    }

    [HttpGet("Set/comInvLimit")]
    //组分油罐容上下限
    public ApiModel Set2()//model里的名字 多个数据用IEnumberable，单个数据不用
    {       
        var comInvLimitList = context.Compoilconfigs.ToList();
        List<Dispatch_parmSet_comOil_2> ResultList = new List<Dispatch_parmSet_comOil_2>();//列表，里面可以添加很多个对象
        for(int i = 0; i < comInvLimitList.Count; i++){
            Dispatch_parmSet_comOil_2 result = new Dispatch_parmSet_comOil_2();//实体，可以理解为一个对象  
            result.ComOilName = comInvLimitList[i].ComOilName;
            result.iniVolume = comInvLimitList[i].IniVolume;
            result.lowVolume = comInvLimitList[i].LowVolume;
            result.highVolume = comInvLimitList[i].HighVolume;
            ResultList.Add(result);
        }    

        return new ApiModel()
        {
        code = 200,
        //data = JsonConvert.SerializeObject(list),
        data = ResultList,
        msg = "查询成功"
        };
    }

    [HttpGet("Set/comPlanProduct")]
    //组分油计划产量
    public ApiModel Set3()//model里的名字 多个数据用IEnumberable，单个数据不用
    {       
        var comPlanProductList = context.Compoilconfigs.ToList();
        List<Dispatch_parmSet_comOil_3> ResultList = new List<Dispatch_parmSet_comOil_3>();//列表，里面可以添加很多个对象
        for(int i = 0; i < comPlanProductList.Count; i++){
            Dispatch_parmSet_comOil_3 result = new Dispatch_parmSet_comOil_3();//实体，可以理解为一个对象  
            result.ComOilName = comPlanProductList[i].ComOilName;
            result.comPlanProduct1 = comPlanProductList[i].PlanProduct1;
            result.comPlanProduct2 = comPlanProductList[i].PlanProduct2;
            result.comPlanProduct3 = comPlanProductList[i].PlanProduct3;
            result.comPlanProduct4 = comPlanProductList[i].PlanProduct4;
            result.comPlanProduct5 = comPlanProductList[i].PlanProduct5;
            result.comPlanProduct6 = comPlanProductList[i].PlanProduct6;
            result.comPlanProduct7 = comPlanProductList[i].PlanProduct7;
            ResultList.Add(result);
        }    

        return new ApiModel()
        {
        code = 200,
        //data = JsonConvert.SerializeObject(list),
        data = ResultList,
        msg = "查询成功"
        };
    }

    [HttpGet("Set/prodInvLimit")]
    //成品油罐容上下限
    public ApiModel Set4()//model里的名字 多个数据用IEnumberable，单个数据不用
    {       
        var prodInvLimitList = context.Prodoilconfigs.ToList();
        List<Dispatch_parmSet_prodOil_1> ResultList = new List<Dispatch_parmSet_prodOil_1>();//列表，里面可以添加很多个对象
        for(int i = 0; i < prodInvLimitList.Count; i++){
            Dispatch_parmSet_prodOil_1 result = new Dispatch_parmSet_prodOil_1();//实体，可以理解为一个对象  
            result.ProdOilName = prodInvLimitList[i].ProdOilName;
            result.iniVolume = prodInvLimitList[i].IniVolume;
            result.lowVolume = prodInvLimitList[i].ProdVolumeLowLimit;
            result.highVolume = prodInvLimitList[i].ProdVolumeHighLimit;
            ResultList.Add(result);
        }    

        return new ApiModel()
        {
        code = 200,
        //data = JsonConvert.SerializeObject(list),
        data = ResultList,
        msg = "查询成功"
        };
    }

    [HttpGet("Set/prodDemand")]
    //成品油需求
    public ApiModel Set5()//model里的名字 多个数据用IEnumberable，单个数据不用
    {       
        var prodDemandList = context.Prodoilconfigs.ToList();
        List<Dispatch_parmSet_prodOil_2> ResultList = new List<Dispatch_parmSet_prodOil_2>();//列表，里面可以添加很多个对象
        for(int i = 0; i < prodDemandList.Count; i++){
            Dispatch_parmSet_prodOil_2 result = new Dispatch_parmSet_prodOil_2();//实体，可以理解为一个对象  
            result.ProdOilName = prodDemandList[i].ProdOilName;
            result.prodDemandHighT1 = prodDemandList[i].Demand1HighLimit;
            result.prodDemandLowT1 = prodDemandList[i].Demand1LowLimit;
            result.prodDemandHighT2 = prodDemandList[i].Demand2HighLimit;
            result.prodDemandLowT2 = prodDemandList[i].Demand2LowLimit;
            result.prodDemandHighT3 = prodDemandList[i].Demand3HighLimit;
            result.prodDemandLowT3 = prodDemandList[i].Demand3LowLimit;
            result.prodDemandHighT4 = prodDemandList[i].Demand4HighLimit;
            result.prodDemandLowT4 = prodDemandList[i].Demand4LowLimit;
            result.prodDemandHighT5 = prodDemandList[i].Demand5HighLimit;
            result.prodDemandLowT5 = prodDemandList[i].Demand5LowLimit;
            result.prodDemandHighT6 = prodDemandList[i].Demand6HighLimit;
            result.prodDemandLowT6 = prodDemandList[i].Demand6LowLimit;
            result.prodDemandHighT7 = prodDemandList[i].Demand7HighLimit;
            result.prodDemandLowT7 = prodDemandList[i].Demand7LowLimit;
            ResultList.Add(result);
        }    

        return new ApiModel()
        {
        code = 200,
        //data = JsonConvert.SerializeObject(list),
        data = ResultList,
        msg = "查询成功"
        };
    }

    [HttpGet("Set/OptimizeObj")]
    //优化目标设置
    public ApiModel Set6()//model里的名字 多个数据用IEnumberable，单个数据不用
    {       
        var OptimizeObjList = context.Dispatchweights.ToList();
        List<Dispatch_parmSet_obj> ResultList = new List<Dispatch_parmSet_obj>();//列表，里面可以添加很多个对象
        for(int i = 0; i < OptimizeObjList.Count; i++){
            Dispatch_parmSet_obj result = new Dispatch_parmSet_obj();//实体，可以理解为一个对象  
            result.weightName = OptimizeObjList[i].WeightName;
            result.weight = OptimizeObjList[i].Weight;
            ResultList.Add(result);
        }    

        return new ApiModel()
        {
        code = 200,
        //data = JsonConvert.SerializeObject(list),
        data = ResultList,
        msg = "查询成功"
        };
    }
}
