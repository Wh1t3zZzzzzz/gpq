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
public class RecipeCalc_2Controller : ControllerBase
{

    private readonly oilblendContext context;

    public RecipeCalc_2Controller(oilblendContext _context)
    {
       context = _context;
    }

    [HttpGet("Set/RecipeLimit")]
    //配方优化场景2配方上下限设置表格
    public ApiModel Set1()//model里的名字 多个数据用IEnumberable，单个数据不用
    {
        var RecipeLimitList = context.Recipecalc1s.ToList();
        List<Recipecalc_2_1> ResultList = new List<Recipecalc_2_1>();//列表，里面可以添加很多个对象
        for(int i = 0; i < RecipeLimitList.Count; i++){
            Recipecalc_2_1 result = new Recipecalc_2_1();//实体，可以理解为一个对象  
            result.ComOilName = RecipeLimitList[i].ComOilName;
            result.AutoRecipeHigh = RecipeLimitList[i].AutoFlowHigh;
            result.AutoRecipeLow = RecipeLimitList[i].AutoFlowLow;
            result.ExpRecipeHigh = RecipeLimitList[i].ExpFlowHigh;
            result.ExpRecipeLow = RecipeLimitList[i].ExpFlowLow;
            result.Prod1RecipeHigh = RecipeLimitList[i].Prod1FlowHigh;
            result.Prod1RecipeLow = RecipeLimitList[i].Prod1FlowLow;
            result.Prod2RecipeHigh = RecipeLimitList[i].Prod2FlowHigh;
            result.Prod2RecipeLow = RecipeLimitList[i].Prod2FlowLow;
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

    [HttpGet("Set/TotalFlow")]
    //配方优化场景2参调总流量设置表格
    public ApiModel Set2()//model里的名字 多个数据用IEnumberable，单个数据不用
    {

        var TotalFlowList = context.Recipecalc3s.Where(m => m.Apply == 1).ToList();
        List<Recipecalc_2_2> ResultList = new List<Recipecalc_2_2>();//列表，里面可以添加很多个对象
        for(int i = 0; i < TotalFlowList.Count; i++){
            Recipecalc_2_2 result = new Recipecalc_2_2();//实体，可以理解为一个对象  
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

    [HttpGet("Set/ComOilFlow")]
    //配方优化场景2组分油参调流量表格
    public ApiModel Set3()//model里的名字 多个数据用IEnumberable，单个数据不用
    {
        IRecipeCalc _RecipeCalc = new RecipeCalc(context);
        var CompOilConstraint = _RecipeCalc.GetRecipeCalc1().ToList();//场景二配方的高低限，转化为流量高低限，场景二用到
        var ProdOilProduct = _RecipeCalc.GetRecipeCalc3().Where(m => m.Apply == 1).ToList();//成品油调合总量

        List<Recipecalc_2_3> ResultList = new List<Recipecalc_2_3>();//新建一个List用来append的,返回的是list形式
        for(int i = 0; i < CompOilConstraint.Count; i++){     
            Recipecalc_2_3 Result = new Recipecalc_2_3();//
            Result.ComOilName = CompOilConstraint[i].ComOilName;
            for(int j = 0; j < ProdOilProduct.Count; j++){
                if(ProdOilProduct[j].Id == 1){
                    Result.AutoFlowLow = CompOilConstraint[i].AutoFlowLow / 100 * ProdOilProduct[j].TotalFlow;
                    Result.AutoFlowHigh = CompOilConstraint[i].AutoFlowHigh / 100 * ProdOilProduct[j].TotalFlow;
                }

                if(ProdOilProduct[j].Id == 2){
                    Result.ExpFlowLow = CompOilConstraint[i].ExpFlowLow / 100 * ProdOilProduct[j].TotalFlow;
                    Result.ExpFlowHigh = CompOilConstraint[i].ExpFlowHigh / 100 * ProdOilProduct[j].TotalFlow;
                }

                if(ProdOilProduct[j].Id == 3){
                    Result.Prod1FlowLow = CompOilConstraint[i].Prod1FlowLow / 100 * ProdOilProduct[j].TotalFlow;
                    Result.Prod1FlowHigh = CompOilConstraint[i].Prod1FlowHigh / 100 * ProdOilProduct[j].TotalFlow;
                }

                if(ProdOilProduct[j].Id == 4){
                    Result.Prod2FlowLow = CompOilConstraint[i].Prod2FlowLow / 100 * ProdOilProduct[j].TotalFlow;
                    Result.Prod2FlowHigh = CompOilConstraint[i].Prod2FlowHigh / 100 * ProdOilProduct[j].TotalFlow;
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

        var OptimizeObjList = context.Recipecalc2_2s.Where(m => m.Apply == 1).ToList();
        List<Recipecalc_2_4> ResultList = new List<Recipecalc_2_4>();//列表，里面可以添加很多个对象
        for(int i = 0; i < OptimizeObjList.Count; i++){
            Recipecalc_2_4 result = new Recipecalc_2_4();//实体，可以理解为一个对象  
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

    [HttpGet("Res/Product")]
    //配方优化场景2计算结果：成品油质量产量
    public ApiModel Res1()//model里的名字 多个数据用IEnumberable，单个数据不用
    {
        IRecipeCalc _RecipeCalc = new RecipeCalc(context);
        var list = _RecipeCalc.GetRecipecalc_2Res_ProdOilProduct().ToList();
        return new ApiModel()
        {
        code = 200,
        //data = JsonConvert.SerializeObject(list),
        data = list,
        msg = "查询成功"
        };

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
