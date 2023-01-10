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
public class RecipeCalc_2GasController : ControllerBase
{

    private readonly oilblendContext context;

    public RecipeCalc_2GasController(oilblendContext _context)
    {
       context = _context;
    }

    [HttpGet("Set/RecipeLimit")]
    //配方优化场景2配方上下限设置表格
    public ApiModel Set1()//model里的名字 多个数据用IEnumberable，单个数据不用
    {
        var RecipeLimitList = context.Recipecalc1_gases.ToList();
        List<GasRecipecalc_2_1> ResultList = new List<GasRecipecalc_2_1>();//列表，里面可以添加很多个对象
        for(int i = 0; i < RecipeLimitList.Count; i++){
            GasRecipecalc_2_1 result = new GasRecipecalc_2_1();//实体，可以理解为一个对象  
            result.ComOilName = RecipeLimitList[i].ComOilName;
            result.gas92RecipeHigh = RecipeLimitList[i].gas92FlowHigh;
            result.gas92RecipeLow = RecipeLimitList[i].gas92FlowLow;
            result.gas95RecipeHigh = RecipeLimitList[i].gas95FlowHigh;
            result.gas95RecipeLow = RecipeLimitList[i].gas95FlowLow;
            result.gas98RecipeHigh = RecipeLimitList[i].gas98FlowHigh;
            result.gas98RecipeLow = RecipeLimitList[i].gas98FlowLow;
            result.gasSelfRecipeHigh = RecipeLimitList[i].gasSelfFlowHigh;
            result.gasSelfRecipeLow = RecipeLimitList[i].gasSelfFlowLow;
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

    [HttpPut("Put/RecipeLimit")]
    //配方优化场景2配方上下限设置表格——修改保存功能
    public ApiModel Put1(GasRecipecalc_2_1_index obj)//model里的名字 多个数据用IEnumberable，单个数据不用
    {
        var RecipeLimitList = context.Recipecalc1_gases.ToList();
        var list1 = context.Schemeverify1_gases.ToList();
        var list2 = context.Compoilconfig_gases.ToList();

        RecipeLimitList[obj.index].ComOilName = obj.ComOilName;
        list1[obj.index].ComOilName = obj.ComOilName;
        list2[obj.index].ComOilName = obj.ComOilName;
        RecipeLimitList[obj.index].gas92FlowHigh = obj.gas92RecipeHigh;
        RecipeLimitList[obj.index].gas92FlowLow = obj.gas92RecipeLow;
        RecipeLimitList[obj.index].gas95FlowHigh = obj.gas95RecipeHigh;
        RecipeLimitList[obj.index].gas95FlowLow = obj.gas95RecipeLow;
        RecipeLimitList[obj.index].gas98FlowHigh = obj.gas98RecipeHigh;
        RecipeLimitList[obj.index].gas98FlowLow = obj.gas98RecipeLow;
        RecipeLimitList[obj.index].gasSelfFlowHigh = obj.gasSelfRecipeHigh;
        RecipeLimitList[obj.index].gasSelfFlowLow = obj.gasSelfRecipeLow;

        context.Recipecalc1_gases.Update(RecipeLimitList[obj.index]);
        context.Schemeverify1_gases.Update(list1[obj.index]);
        context.Compoilconfig_gases.Update(list2[obj.index]);
        context.SaveChanges();
    
        return new ApiModel()
        {
        code = 200,
        //data = JsonConvert.SerializeObject(list),
        data = RecipeLimitList,
        msg = "查询成功"
        };

    }

    [HttpGet("Set/TotalFlow")]
    //配方优化场景2参调总流量设置表格
    public ApiModel Set2()//model里的名字 多个数据用IEnumberable，单个数据不用
    {
        var TotalFlowList = context.Recipecalc3_gases.Where(m => m.Apply == 1).ToList();
        List<GasRecipecalc_2_2> ResultList = new List<GasRecipecalc_2_2>();//列表，里面可以添加很多个对象
        for(int i = 0; i < TotalFlowList.Count; i++){
            GasRecipecalc_2_2 result = new GasRecipecalc_2_2();//实体，可以理解为一个对象  
            result.ProdOilName = TotalFlowList[i].ProdOilName;
            result.ProdTotalFlow = TotalFlowList[i].TotalFlow;
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

    [HttpPut("Put/TotalFlow")]
    //配方优化场景2参调总流量设置表格——修改保存功能
    public ApiModel Put2(GasRecipecalc_2_2_index obj)//model里的名字 多个数据用IEnumberable，单个数据不用
    {
        var TotalFlowList = context.Recipecalc3_gases.Where(m => m.Apply == 1).ToList();
        var list1 = context.Schemeverify2_gases.ToList();
        var list2 = context.Prodoilconfig_gases.ToList();

        TotalFlowList[obj.index].ProdOilName = obj.ProdOilName;
        list1[obj.index].ProdOilName = obj.ProdOilName;
        list2[obj.index].ProdOilName = obj.ProdOilName;
        TotalFlowList[obj.index].TotalFlow = obj.ProdTotalFlow;

        context.Recipecalc3_gases.Update(TotalFlowList[obj.index]);
        context.Schemeverify2_gases.Update(list1[obj.index]);
        context.Prodoilconfig_gases.Update(list2[obj.index]);
        context.SaveChanges();
    
        return new ApiModel()
        {
        code = 200,
        //data = JsonConvert.SerializeObject(list),
        data = TotalFlowList,
        msg = "查询成功"
        };
    }

    [HttpGet("Set/ComOilFlow")]
    //配方优化场景2组分油参调流量表格
    public ApiModel Set3()//model里的名字 多个数据用IEnumberable，单个数据不用
    {
        IRecipeCalc _RecipeCalc = new RecipeCalc(context);
        var CompOilConstraint = _RecipeCalc.GetRecipeCalc1().ToList();//场景二配方的高低限，转化为流量高低限，场景二用到
        var ProdOilProduct = _RecipeCalc.GetRecipeCalc3().Where(m => m.Apply == 1).ToList();//成品油调合总量

        List<GasRecipecalc_2_3> ResultList = new List<GasRecipecalc_2_3>();//新建一个List用来append的,返回的是list形式
        for(int i = 0; i < CompOilConstraint.Count; i++){     
            GasRecipecalc_2_3 Result = new GasRecipecalc_2_3();//
            Result.ComOilName = CompOilConstraint[i].ComOilName;
            for(int j = 0; j < ProdOilProduct.Count; j++){
                if(ProdOilProduct[j].Id == 1){
                    Result.gas92FlowLow = CompOilConstraint[i].gas92FlowLow / 100 * ProdOilProduct[j].TotalFlow;
                    Result.gas92FlowHigh = CompOilConstraint[i].gas92FlowHigh / 100 * ProdOilProduct[j].TotalFlow;
                }

                if(ProdOilProduct[j].Id == 2){
                    Result.gas95FlowLow = CompOilConstraint[i].gas95FlowLow / 100 * ProdOilProduct[j].TotalFlow;
                    Result.gas95FlowHigh = CompOilConstraint[i].gas95FlowHigh / 100 * ProdOilProduct[j].TotalFlow;
                }

                if(ProdOilProduct[j].Id == 3){
                    Result.gas98FlowLow = CompOilConstraint[i].gas98FlowLow / 100 * ProdOilProduct[j].TotalFlow;
                    Result.gas98FlowHigh = CompOilConstraint[i].gas98FlowHigh / 100 * ProdOilProduct[j].TotalFlow;
                }

                if(ProdOilProduct[j].Id == 4){
                    Result.gasSelfFlowLow = CompOilConstraint[i].gasSelfFlowLow / 100 * ProdOilProduct[j].TotalFlow;
                    Result.gasSelfFlowHigh = CompOilConstraint[i].gasSelfFlowHigh / 100 * ProdOilProduct[j].TotalFlow;
                }
            }
            ResultList.Add(Result);
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
    //配方优化场景2优化目标设置
    public ApiModel Set4()//model里的名字 多个数据用IEnumberable，单个数据不用
    {

        var OptimizeObjList = context.Recipecalc2_2_gases.Where(m => m.Apply == 1).ToList();
        List<GasRecipecalc_2_4> ResultList = new List<GasRecipecalc_2_4>();//列表，里面可以添加很多个对象
        for(int i = 0; i < OptimizeObjList.Count; i++){
            GasRecipecalc_2_4 result = new GasRecipecalc_2_4();//实体，可以理解为一个对象  
            result.WeightName = OptimizeObjList[i].WeightName;
            result.Weight = OptimizeObjList[i].Weight;
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
    //配方优化场景2优化目标设置——修改保存功能
    public ApiModel Put3(GasRecipecalc_2_4_index obj)//model里的名字 多个数据用IEnumberable，单个数据不用
    {
        var OptimizeObjList = context.Recipecalc2_2_gases.Where(m => m.Apply == 1).ToList();
        OptimizeObjList[obj.index].Weight = obj.Weight;
        context.Recipecalc2_2_gases.Update(OptimizeObjList[obj.index]);//是按经过where查询后的list顺序更改的
        context.SaveChanges();   
        return new ApiModel()
        {
        code = 200,
        //data = JsonConvert.SerializeObject(list),
        data = OptimizeObjList,
        msg = "查询成功"
        };
    }

    [HttpGet("Res/Product")]
    //配方优化场景2计算结果：成品油质量产量
    public ApiModel Res1()//model里的名字 多个数据用IEnumberable，单个数据不用
    {
        IRecipeCalc _RecipeCalc = new RecipeCalc(context);
        var list = _RecipeCalc.GetRecipecalc_2Res_ProdOilProduct().ToList();
        int status = (int)_RecipeCalc.GetRecipe2()[_RecipeCalc.GetRecipe2().Length - 1];
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
    //配方优化场景2计算结果：成品油优化配方
    public ApiModel Res2()//model里的名字 多个数据用IEnumberable，单个数据不用
    {
        IRecipeCalc _RecipeCalc = new RecipeCalc(context);
        var list = _RecipeCalc.GetRecipecalc_2Res_Recipe().ToList();
        return new ApiModel()
        {
        code = 200,
        //data = JsonConvert.SerializeObject(list),
        data = list,
        msg = "查询成功"
        };

    }

    [HttpGet("Res/Property")]
    //配方优化场景2计算结果：成品油属性
    public ApiModel Res3()//model里的名字 多个数据用IEnumberable，单个数据不用
    {
        IRecipeCalc _RecipeCalc = new RecipeCalc(context);
        var list = _RecipeCalc.GetRecipecalc_2Res_Property().ToList();
        return new ApiModel()
        {
        code = 200,
        //data = JsonConvert.SerializeObject(list),
        data = list,
        msg = "查询成功"
        };

    }


}
