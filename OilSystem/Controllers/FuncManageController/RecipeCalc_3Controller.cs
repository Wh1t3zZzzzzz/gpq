using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OilBlendSystem.Models.DataBaseModel;
using OilBlendSystem.Models.ConstructModel;
using OilSystem.ReturnClass;
using Newtonsoft.Json;
using OilBlendSystem.BLL.Implementation;
using OilBlendSystem.BLL.Interface;

namespace OilSystem.Controllers;
//using System.Data.Entity;
[ApiController]
[Route("[controller]")]
public class RecipeCalc_3Controller : ControllerBase
{

    private readonly oilblendContext context;

    public RecipeCalc_3Controller(oilblendContext _context)
    {
       context = _context;
    }

    [HttpGet("Set/ComOilFlowLimit")]
    //配方优化场景3组分油参调流量高低限设置表格
    public ApiModel Set1()//model里的名字 多个数据用IEnumberable，单个数据不用
    {
        var ComOilFlowLimitList = context.Compoilconfigs.ToList();
        List<Recipecalc_3_1> ResultList = new List<Recipecalc_3_1>();//列表，里面可以添加很多个对象
        for(int i = 0; i < ComOilFlowLimitList.Count; i++){
            Recipecalc_3_1 result = new Recipecalc_3_1();//实体，可以理解为一个对象  
            result.ComOilName = ComOilFlowLimitList[i].ComOilName;
            result.AutoFlowHigh = ComOilFlowLimitList[i].AutoHigh1;
            result.AutoFlowLow = ComOilFlowLimitList[i].AutoLow1;
            result.ExpFlowHigh = ComOilFlowLimitList[i].ExpHigh1;
            result.ExpFlowLow = ComOilFlowLimitList[i].ExpLow1;
            result.Prod1FlowHigh = ComOilFlowLimitList[i].Prod1High1;
            result.Prod1FlowLow = ComOilFlowLimitList[i].Prod1Low1;
            result.Prod2FlowHigh = ComOilFlowLimitList[i].Prod2High1;
            result.Prod2FlowLow = ComOilFlowLimitList[i].Prod2Low1;
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

    [HttpPut("Put/ComOilFlowLimit")]
    //配方优化场景3组分油参调流量高低限设置表格——修改保存功能
    public ApiModel Put1(Recipecalc_3_1_index obj)//model里的名字 多个数据用IEnumberable，单个数据不用
    {
        var ComOilFlowLimitList = context.Compoilconfigs.ToList();
        var list1 = context.Recipecalc1s.ToList();
        var list2 = context.Schemeverify1s.ToList();

        ComOilFlowLimitList[obj.index].ComOilName = obj.ComOilName;
        list1[obj.index].ComOilName = obj.ComOilName;
        list2[obj.index].ComOilName = obj.ComOilName;
        ComOilFlowLimitList[obj.index].AutoHigh1 = obj.AutoFlowHigh;
        ComOilFlowLimitList[obj.index].AutoLow1 = obj.AutoFlowLow;
        ComOilFlowLimitList[obj.index].ExpHigh1 = obj.ExpFlowHigh;
        ComOilFlowLimitList[obj.index].ExpLow1 = obj.ExpFlowLow;
        ComOilFlowLimitList[obj.index].Prod1High1 = obj.Prod1FlowHigh;
        ComOilFlowLimitList[obj.index].Prod1Low1 = obj.Prod1FlowLow;
        ComOilFlowLimitList[obj.index].Prod2High1 = obj.Prod2FlowHigh;
        ComOilFlowLimitList[obj.index].Prod2Low1 = obj.Prod2FlowLow;

        context.Compoilconfigs.Update(ComOilFlowLimitList[obj.index]);
        context.Recipecalc1s.Update(list1[obj.index]);
        context.Schemeverify1s.Update(list2[obj.index]);
        context.SaveChanges();
    
        return new ApiModel()
        {
        code = 200,
        //data = JsonConvert.SerializeObject(list),
        data = ComOilFlowLimitList,
        msg = "查询成功"
        };
    }

    [HttpGet("Set/OptimizeObj")]
    //配方优化场景3优化目标设置设置表格
    public ApiModel Set2()//model里的名字 多个数据用IEnumberable，单个数据不用
    {
        var OptimizeObjList = context.Recipecalc2_3s.Where(m => m.Apply == 1).ToList();
        List<Recipecalc_3_2> ResultList = new List<Recipecalc_3_2>();//列表，里面可以添加很多个对象
        for(int i = 0; i < OptimizeObjList.Count; i++){
            Recipecalc_3_2 result = new Recipecalc_3_2();//实体，可以理解为一个对象  
            result.Weight = OptimizeObjList[i].Weight;
            result.WeightName = OptimizeObjList[i].WeightName;
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
    //配方优化场景3优化目标设置设置表格——修改保存功能
    public ApiModel Put2(Recipecalc_3_2_index obj)//model里的名字 多个数据用IEnumberable，单个数据不用
    {
        var OptimizeObjList = context.Recipecalc2_3s.Where(m => m.Apply == 1).ToList();
        OptimizeObjList[obj.index].Weight = obj.Weight;
        context.Recipecalc2_3s.Update(OptimizeObjList[obj.index]);//是按经过where查询后的list顺序更改的
        context.SaveChanges();   
        return new ApiModel()
        {
        code = 200,
        //data = JsonConvert.SerializeObject(list),
        data = OptimizeObjList,
        msg = "查询成功"
        };
    }

    [HttpGet("Set/ProdTotal")]
    //配方优化场景3成品油产量限制（成品油参调总量设置表格）设置表格
    public ApiModel Set3()//model里的名字 多个数据用IEnumberable，单个数据不用
    {
        var ProdTotalList = context.Recipecalc3s.Where(m => m.Apply == 1).ToList();
        List<Recipecalc_3_3> ResultList = new List<Recipecalc_3_3>();//列表，里面可以添加很多个对象
        for(int i = 0; i < ProdTotalList.Count; i++){
            Recipecalc_3_3 result = new Recipecalc_3_3();//实体，可以理解为一个对象  
            result.ProdOilName = ProdTotalList[i].ProdOilName;
            result.ProdTotalFlow = ProdTotalList[i].TotalFlow2;
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

    [HttpPut("Put/ProdTotal")]
    //配方优化场景2参调总流量设置表格——修改保存功能
    public ApiModel Put3(Recipecalc_3_3_index obj)//model里的名字 多个数据用IEnumberable，单个数据不用
    {
        var ProdTotalList = context.Recipecalc3s.Where(m => m.Apply == 1).ToList();
        var list1 = context.Schemeverify2s.ToList();
        var list2 = context.Prodoilconfigs.ToList();

        ProdTotalList[obj.index].ProdOilName = obj.ProdOilName;
        list1[obj.index].ProdOilName = obj.ProdOilName;
        list2[obj.index].ProdOilName = obj.ProdOilName;
        ProdTotalList[obj.index].TotalFlow2 = obj.ProdTotalFlow;

        context.Recipecalc3s.Update(ProdTotalList[obj.index]);
        context.Schemeverify2s.Update(list1[obj.index]);
        context.Prodoilconfigs.Update(list2[obj.index]);
        context.SaveChanges();
    
        return new ApiModel()
        {
        code = 200,
        //data = JsonConvert.SerializeObject(list),
        data = ProdTotalList,
        msg = "查询成功"
        };
    }

    [HttpGet("Res/Product")]
    //配方优化场景3计算结果：成品油质量产量
    public ApiModel Res1()//model里的名字 多个数据用IEnumberable，单个数据不用
    {
        IRecipeCalc _RecipeCalc = new RecipeCalc(context);
        var list = _RecipeCalc.GetRecipecalc_3Res_ProdOilProduct().ToList();
        int status = (int)_RecipeCalc.GetRecipe3()[_RecipeCalc.GetRecipe3().Length - 1];
        if(status == 0 || status == 1){//得到最优解或者次优解
            return new ApiModel(){
                code = 200,
                //data = JsonConvert.SerializeObject(list),
                data = list,
                msg = "求解成功"
            };
        }else{//计算失败
            return new ApiModel(){
            code = 500,
            //data = JsonConvert.SerializeObject(list),
            data = list,
            msg = "求解失败"
            };       
        }
    }

    [HttpGet("Res/Recipe")]
    //配方优化场景3计算结果：成品油优化配方
    public ApiModel Res2()//model里的名字 多个数据用IEnumberable，单个数据不用
    {
        IRecipeCalc _RecipeCalc = new RecipeCalc(context);
        var list = _RecipeCalc.GetRecipecalc_3Res_Recipe().ToList();
        return new ApiModel()
        {
        code = 200,
        //data = JsonConvert.SerializeObject(list),
        data = list,
        msg = "查询成功"
        };

    }

    [HttpGet("Res/Property")]
    //配方优化场景3计算结果：成品油属性
    public ApiModel Res3()//model里的名字 多个数据用IEnumberable，单个数据不用
    {
        IRecipeCalc _RecipeCalc = new RecipeCalc(context);
        var list = _RecipeCalc.GetRecipecalc_3Res_Property().ToList();
        return new ApiModel()
        {
        code = 200,
        //data = JsonConvert.SerializeObject(list),
        data = list,
        msg = "查询成功"
        };

    }


}
