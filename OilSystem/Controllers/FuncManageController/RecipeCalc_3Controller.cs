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


    [HttpGet("Res/Product")]
    //配方优化场景3计算结果：成品油质量产量
    public ApiModel Res1()//model里的名字 多个数据用IEnumberable，单个数据不用
    {
        IRecipeCalc _RecipeCalc = new RecipeCalc(context);
        var list = _RecipeCalc.GetRecipecalc_3Res_ProdOilProduct().ToList();
        return new ApiModel()
        {
        code = 200,
        //data = JsonConvert.SerializeObject(list),
        data = list,
        msg = "查询成功"
        };

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
