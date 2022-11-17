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
        var ProdOilProductList = context.Schemeverify1s.ToList();
        List<SchemeVerify_1_1> ResultList = new List<SchemeVerify_1_1>();//列表，里面可以添加很多个对象
        for(int i = 0; i < ProdOilProductList.Count; i++){
            SchemeVerify_1_1 result = new SchemeVerify_1_1();//实体，可以理解为一个对象  
            result.ComOilName = ProdOilProductList[i].ComOilName;
            result.AutoProduct = ProdOilProductList[i].AutoQualityProduct;
            result.ExpProduct = ProdOilProductList[i].ExpQualityProduct;
            result.Prod1Product = ProdOilProductList[i].Prod1QualityProduct;
            result.Prod2Product = ProdOilProductList[i].Prod2QualityProduct;
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

        var BottomInfoList = context.Schemeverify2s.ToList();
        List<SchemeVerify_1_2> ResultList = new List<SchemeVerify_1_2>();//列表，里面可以添加很多个对象
        for(int i = 0; i < BottomInfoList.Count; i++){
            SchemeVerify_1_2 result = new SchemeVerify_1_2();//实体，可以理解为一个对象  
            result.ProdOilName = BottomInfoList[i].ProdOilName;
            result.BottomCapacity = BottomInfoList[i].BottomMass;
            result.BottomCET = BottomInfoList[i].CetMass;
            result.BottomD50 = BottomInfoList[i].D50Mass;
            result.BottomPOL = BottomInfoList[i].PolMass;
            result.BottomDEN = BottomInfoList[i].DenMass;
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
        IProdOilConfig _ProdOilConfig = new ProdOilConfig(context);
        IProperty _Property = new PropertyApply(context);
        IRecipeCalc _RecipeCalc = new RecipeCalc(context);
        ICompOilConfig _CompOilConfig = new CompOilConfig(context);

        var CompOilList = _CompOilConfig.GetAllCompOilConfigList().ToList();//组分油参调流量的高低限，场景三按流量优化会用到
        var ProdOilList = _ProdOilConfig.GetAllProdOilConfigList().ToList();
        var PropertyList = _Property.GetAllPropertyList().ToList();
        var CompOilConstraint = _RecipeCalc.GetRecipeCalc1().ToList();//场景二配方的高低限，转化为流量高低限，场景二用到
        var Weight = _RecipeCalc.GetRecipeCalc2().ToList();//权值
        var ProdOilProduct = _RecipeCalc.GetRecipeCalc3().ToList();//成品油总量

        List<Recipecalc_2_3> ResultList = new List<Recipecalc_2_3>();//新建一个List用来append的,返回的是list形式

        for(int i = 0; i < 8; i++){     

            Recipecalc_2_3 Result = new Recipecalc_2_3();//

            Result.AutoFlowLow = CompOilConstraint[i].AutoFlowLow / 100 * ProdOilProduct[0].TotalFlow;//车柴 0 2 4 6 8
            Result.ExpFlowLow  = CompOilConstraint[i].ExpFlowLow / 100 * ProdOilProduct[1].TotalFlow;//出柴 1 3 5 7 
            Result.AutoFlowHigh = CompOilConstraint[i].AutoFlowHigh / 100 * ProdOilProduct[0].TotalFlow;
            Result.ExpFlowHigh  = CompOilConstraint[i].ExpFlowHigh / 100 * ProdOilProduct[1].TotalFlow;
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

        var TotalBlendList = context.Schemeverify2s.ToList();
        List<SchemeVerify_1_3> ResultList = new List<SchemeVerify_1_3>();//列表，里面可以添加很多个对象
        for(int i = 0; i < TotalBlendList.Count; i++){
            SchemeVerify_1_3 result = new SchemeVerify_1_3();//实体，可以理解为一个对象  
            result.ProdOilName = TotalBlendList[i].ProdOilName;
            result.ProdTotalBlend = TotalBlendList[i].TotalBlendMass;
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
