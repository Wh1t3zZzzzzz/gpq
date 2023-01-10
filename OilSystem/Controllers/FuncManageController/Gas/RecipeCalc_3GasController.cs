using Microsoft.AspNetCore.Mvc;
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
public class RecipeCalc_3GasController : ControllerBase
{

    private readonly oilblendContext context;

    public RecipeCalc_3GasController(oilblendContext _context)
    {
       context = _context;
    }

    [HttpGet("Set/ComOilFlowLimit")]
    //配方优化场景3组分油参调流量高低限设置表格
    public ApiModel Set1()//model里的名字 多个数据用IEnumberable，单个数据不用
    {
        var ComOilFlowLimitList = context.Compoilconfig_gases.ToList();
        List<GasRecipecalc_3_1> ResultList = new List<GasRecipecalc_3_1>();//列表，里面可以添加很多个对象
        for(int i = 0; i < ComOilFlowLimitList.Count; i++){
            GasRecipecalc_3_1 result = new GasRecipecalc_3_1();//实体，可以理解为一个对象  
            result.ComOilName = ComOilFlowLimitList[i].ComOilName;
            result.gas92FlowHigh = ComOilFlowLimitList[i].gas92High1;
            result.gas92FlowLow = ComOilFlowLimitList[i].gas92Low1;
            result.gas95FlowHigh = ComOilFlowLimitList[i].gas95High1;
            result.gas95FlowLow = ComOilFlowLimitList[i].gas95Low1;
            result.gas98FlowHigh = ComOilFlowLimitList[i].gas98High1;
            result.gas98FlowLow = ComOilFlowLimitList[i].gas98Low1;
            result.gasSelfFlowHigh = ComOilFlowLimitList[i].gasSelfHigh1;
            result.gasSelfFlowLow = ComOilFlowLimitList[i].gasSelfLow1;
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
    public ApiModel Put1(GasRecipecalc_3_1_index obj)//model里的名字 多个数据用IEnumberable，单个数据不用
    {
        var ComOilFlowLimitList = context.Compoilconfig_gases.ToList();
        var list1 = context.Recipecalc1_gases.ToList();
        var list2 = context.Schemeverify1_gases.ToList();

        ComOilFlowLimitList[obj.index].ComOilName = obj.ComOilName;
        list1[obj.index].ComOilName = obj.ComOilName;
        list2[obj.index].ComOilName = obj.ComOilName;
        ComOilFlowLimitList[obj.index].gas92High1 = obj.gas92FlowHigh;
        ComOilFlowLimitList[obj.index].gas92Low1 = obj.gas92FlowLow;
        ComOilFlowLimitList[obj.index].gas95High1 = obj.gas95FlowHigh;
        ComOilFlowLimitList[obj.index].gas95Low1 = obj.gas95FlowLow;
        ComOilFlowLimitList[obj.index].gas98High1 = obj.gas98FlowHigh;
        ComOilFlowLimitList[obj.index].gas98Low1 = obj.gas98FlowLow;
        ComOilFlowLimitList[obj.index].gasSelfHigh1 = obj.gasSelfFlowHigh;
        ComOilFlowLimitList[obj.index].gasSelfLow1 = obj.gasSelfFlowLow;

        context.Compoilconfig_gases.Update(ComOilFlowLimitList[obj.index]);
        context.Recipecalc1_gases.Update(list1[obj.index]);
        context.Schemeverify1_gases.Update(list2[obj.index]);
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
        var OptimizeObjList = context.Recipecalc2_3_gases.Where(m => m.Apply == 1).ToList();
        List<GasRecipecalc_3_2> ResultList = new List<GasRecipecalc_3_2>();//列表，里面可以添加很多个对象
        for(int i = 0; i < OptimizeObjList.Count; i++){
            GasRecipecalc_3_2 result = new GasRecipecalc_3_2();//实体，可以理解为一个对象  
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
    public ApiModel Put2(GasRecipecalc_3_2_index obj)//model里的名字 多个数据用IEnumberable，单个数据不用
    {
        var OptimizeObjList = context.Recipecalc2_3_gases.Where(m => m.Apply == 1).ToList();
        OptimizeObjList[obj.index].Weight = obj.Weight;
        context.Recipecalc2_3_gases.Update(OptimizeObjList[obj.index]);//是按经过where查询后的list顺序更改的
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
        var ProdTotalList = context.Recipecalc3_gases.Where(m => m.Apply == 1).ToList();
        List<GasRecipecalc_3_3> ResultList = new List<GasRecipecalc_3_3>();//列表，里面可以添加很多个对象
        for(int i = 0; i < ProdTotalList.Count; i++){
            GasRecipecalc_3_3 result = new GasRecipecalc_3_3();//实体，可以理解为一个对象  
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
    public ApiModel Put3(GasRecipecalc_3_3_index obj)//model里的名字 多个数据用IEnumberable，单个数据不用
    {
        var ProdTotalList = context.Recipecalc3_gases.Where(m => m.Apply == 1).ToList();
        var list1 = context.Schemeverify2_gases.ToList();
        var list2 = context.Prodoilconfig_gases.ToList();

        ProdTotalList[obj.index].ProdOilName = obj.ProdOilName;
        list1[obj.index].ProdOilName = obj.ProdOilName;
        list2[obj.index].ProdOilName = obj.ProdOilName;
        ProdTotalList[obj.index].TotalFlow2 = obj.ProdTotalFlow;

        context.Recipecalc3_gases.Update(ProdTotalList[obj.index]);
        context.Schemeverify2_gases.Update(list1[obj.index]);
        context.Prodoilconfig_gases.Update(list2[obj.index]);
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
            code = 407,
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
