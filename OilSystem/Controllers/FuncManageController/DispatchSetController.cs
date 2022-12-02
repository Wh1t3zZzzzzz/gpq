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

    [HttpPut("Put/comFlowLimit")]
    //组分油参调流量上下限——修改保存功能
    public ApiModel Put1(Dispatch_parmSet_comOil_1_index obj)//model里的名字 多个数据用IEnumberable，单个数据不用
    {
        var comFlowLimitList = context.Compoilconfigs.ToList();
        var list1 = context.Recipecalc1s.ToList();
        var list2 = context.Schemeverify1s.ToList();

        comFlowLimitList[obj.index].ComOilName = obj.comOilName;
        list1[obj.index].ComOilName = obj.comOilName;
        list2[obj.index].ComOilName = obj.comOilName;
        comFlowLimitList[obj.index].AutoHigh2 = obj.prod1HighLimit;
        comFlowLimitList[obj.index].AutoLow2 = obj.prod1LowLimit;
        comFlowLimitList[obj.index].ExpHigh2 = obj.prod2HighLimit;
        comFlowLimitList[obj.index].ExpLow2 = obj.prod2LowLimit;
        comFlowLimitList[obj.index].Prod1High2 = obj.prod3HighLimit;
        comFlowLimitList[obj.index].Prod1Low2 = obj.prod3LowLimit;
        comFlowLimitList[obj.index].Prod2High2 = obj.prod4HighLimit;
        comFlowLimitList[obj.index].Prod2Low2 = obj.prod4LowLimit;

        context.Compoilconfigs.Update(comFlowLimitList[obj.index]);
        context.Recipecalc1s.Update(list1[obj.index]);
        context.Schemeverify1s.Update(list2[obj.index]);
        context.SaveChanges();
    
        return new ApiModel()
        {
        code = 200,
        //data = JsonConvert.SerializeObject(list),
        data = comFlowLimitList,
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

    [HttpPut("Put/comInvLimit")]
    //组分油罐容上下限——修改保存功能
    public ApiModel Put2(Dispatch_parmSet_comOil_2_index obj)//model里的名字 多个数据用IEnumberable，单个数据不用
    {
        var comInvLimitList = context.Compoilconfigs.ToList();
        var list1 = context.Recipecalc1s.ToList();
        var list2 = context.Schemeverify1s.ToList();

        comInvLimitList[obj.index].ComOilName = obj.ComOilName;
        list1[obj.index].ComOilName = obj.ComOilName;
        list2[obj.index].ComOilName = obj.ComOilName;
        comInvLimitList[obj.index].IniVolume = obj.iniVolume;
        comInvLimitList[obj.index].LowVolume = obj.lowVolume;
        comInvLimitList[obj.index].HighVolume = obj.highVolume;

        context.Compoilconfigs.Update(comInvLimitList[obj.index]);
        context.Recipecalc1s.Update(list1[obj.index]);
        context.Schemeverify1s.Update(list2[obj.index]);
        context.SaveChanges();
    
        return new ApiModel()
        {
        code = 200,
        //data = JsonConvert.SerializeObject(list),
        data = comInvLimitList,
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

    [HttpPut("Put/comPlanProduct")]
    //组分油计划产量——修改保存功能
    public ApiModel Put3(Dispatch_parmSet_comOil_3_index obj)//model里的名字 多个数据用IEnumberable，单个数据不用
    {
        var comPlanProductList = context.Compoilconfigs.ToList();
        var list1 = context.Recipecalc1s.ToList();
        var list2 = context.Schemeverify1s.ToList();

        comPlanProductList[obj.index].ComOilName = obj.ComOilName;
        list1[obj.index].ComOilName = obj.ComOilName;
        list2[obj.index].ComOilName = obj.ComOilName;
        comPlanProductList[obj.index].PlanProduct1 = obj.comPlanProduct1;
        comPlanProductList[obj.index].PlanProduct2 = obj.comPlanProduct2;
        comPlanProductList[obj.index].PlanProduct3 = obj.comPlanProduct3;
        comPlanProductList[obj.index].PlanProduct4 = obj.comPlanProduct4;
        comPlanProductList[obj.index].PlanProduct5 = obj.comPlanProduct5;
        comPlanProductList[obj.index].PlanProduct6 = obj.comPlanProduct6;
        comPlanProductList[obj.index].PlanProduct7 = obj.comPlanProduct7;

        context.Compoilconfigs.Update(comPlanProductList[obj.index]);
        context.Recipecalc1s.Update(list1[obj.index]);
        context.Schemeverify1s.Update(list2[obj.index]);
        context.SaveChanges();
    
        return new ApiModel()
        {
        code = 200,
        //data = JsonConvert.SerializeObject(list),
        data = comPlanProductList,
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

    [HttpPut("Put/prodInvLimit")]
    //成品油罐容上下限——修改保存功能
    public ApiModel Put4(Dispatch_parmSet_prodOil_1_index obj)//model里的名字 多个数据用IEnumberable，单个数据不用
    {
        var prodInvLimitList = context.Prodoilconfigs.ToList();
        var list1 = context.Recipecalc3s.ToList();
        var list2 = context.Schemeverify2s.ToList();

        prodInvLimitList[obj.index].ProdOilName = obj.ProdOilName;
        list1[obj.index].ProdOilName = obj.ProdOilName;
        list2[obj.index].ProdOilName = obj.ProdOilName;
        prodInvLimitList[obj.index].IniVolume = obj.iniVolume;
        prodInvLimitList[obj.index].ProdVolumeLowLimit = obj.lowVolume;
        prodInvLimitList[obj.index].ProdVolumeHighLimit = obj.highVolume;

        context.Prodoilconfigs.Update(prodInvLimitList[obj.index]);
        context.Recipecalc3s.Update(list1[obj.index]);
        context.Schemeverify2s.Update(list2[obj.index]);
        context.SaveChanges();
    
        return new ApiModel()
        {
        code = 200,
        //data = JsonConvert.SerializeObject(list),
        data = prodInvLimitList,
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

    [HttpPut("Put/prodDemand")]
    //成品油需求——修改保存功能
    public ApiModel Put5(Dispatch_parmSet_prodOil_2_index obj)//model里的名字 多个数据用IEnumberable，单个数据不用
    {
        var prodDemandList = context.Prodoilconfigs.ToList();
        var list1 = context.Recipecalc3s.ToList();
        var list2 = context.Schemeverify2s.ToList();

        prodDemandList[obj.index].ProdOilName = obj.ProdOilName;
        list1[obj.index].ProdOilName = obj.ProdOilName;
        list2[obj.index].ProdOilName = obj.ProdOilName;
        prodDemandList[obj.index].Demand1HighLimit = obj.prodDemandHighT1;
        prodDemandList[obj.index].Demand2HighLimit = obj.prodDemandHighT2;
        prodDemandList[obj.index].Demand3HighLimit = obj.prodDemandHighT3;
        prodDemandList[obj.index].Demand4HighLimit = obj.prodDemandHighT4;
        prodDemandList[obj.index].Demand5HighLimit = obj.prodDemandHighT5;
        prodDemandList[obj.index].Demand6HighLimit = obj.prodDemandHighT6;
        prodDemandList[obj.index].Demand7HighLimit = obj.prodDemandHighT7;
        prodDemandList[obj.index].Demand1LowLimit = obj.prodDemandLowT1;
        prodDemandList[obj.index].Demand2LowLimit = obj.prodDemandLowT2;
        prodDemandList[obj.index].Demand3LowLimit = obj.prodDemandLowT3;
        prodDemandList[obj.index].Demand4LowLimit = obj.prodDemandLowT4;
        prodDemandList[obj.index].Demand5LowLimit = obj.prodDemandLowT5;
        prodDemandList[obj.index].Demand6LowLimit = obj.prodDemandLowT6;
        prodDemandList[obj.index].Demand7LowLimit = obj.prodDemandLowT7;

        context.Prodoilconfigs.Update(prodDemandList[obj.index]);
        context.Recipecalc3s.Update(list1[obj.index]);
        context.Schemeverify2s.Update(list2[obj.index]);
        context.SaveChanges();
    
        return new ApiModel()
        {
        code = 200,
        //data = JsonConvert.SerializeObject(list),
        data = prodDemandList,
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

    [HttpPut("Put/OptimizeObj")]
    //优化目标设置——修改保存功能
    public ApiModel Put6(Dispatch_parmSet_obj_index obj)//model里的名字 多个数据用IEnumberable，单个数据不用
    {
        var OptimizeObjList = context.Dispatchweights.ToList();
        if(obj.index == 0){
            string time = Convert.ToString(obj.weight);
            if(time.Contains(".")){
                return new ApiModel()
                {
                code = 500,
                //data = JsonConvert.SerializeObject(list),
                data = null,
                msg = "周期不允许是小数, 取值范围(1~7)"
                };
            }
            else if(obj.weight > 7 || obj.weight < 1){
                return new ApiModel()
                {
                code = 501,
                //data = JsonConvert.SerializeObject(list),
                data = null,
                msg = "超出限制，周期的取值范围是(1~7)"
                };
            }
        }
        if(obj.index == 5){
            string time = Convert.ToString(obj.weight);
            if(time.Contains(".")){
                return new ApiModel()
                {
                code = 500,
                //data = JsonConvert.SerializeObject(list),
                data = null,
                msg = "成品油个数不允许是小数, 取值范围(1~4)"
                };
            }
            else if(obj.weight > 4 || obj.weight < 1){
                return new ApiModel()
                {
                code = 501,
                //data = JsonConvert.SerializeObject(list),
                data = null,
                msg = "超出限制，成品油个数的取值范围是(1~4)"
                };
            }
        }
        OptimizeObjList[obj.index].Weight = obj.weight;
        context.Dispatchweights.Update(OptimizeObjList[obj.index]);//是按经过where查询后的list顺序更改的
        context.SaveChanges();   
        return new ApiModel()
        {
        code = 200,
        //data = JsonConvert.SerializeObject(list),
        data = OptimizeObjList,
        msg = "查询成功"
        };
    }

}
