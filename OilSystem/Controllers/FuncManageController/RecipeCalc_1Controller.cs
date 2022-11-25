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
public class RecipeCalc_1Controller : ControllerBase
{

    private readonly oilblendContext context;

    public RecipeCalc_1Controller(oilblendContext _context)
    {
       context = _context;
    }

    [HttpGet("Set/ComOilProductLimit")]
    //配方优化场景1组分油产量范围设置表格
    public ApiModel Set1()//model里的名字 多个数据用IEnumberable，单个数据不用
    {
        var ComOilProductLimitList = context.Recipecalc1s.ToList();//
        List<Recipecalc_1_1> ResultList = new List<Recipecalc_1_1>();//列表，里面可以添加很多个对象
        for(int i = 0; i < ComOilProductLimitList.Count; i++){
            Recipecalc_1_1 result = new Recipecalc_1_1();//实体，可以理解为一个对象  
            result.ComOilName = ComOilProductLimitList[i].ComOilName;
            result.ComOilProductHigh = ComOilProductLimitList[i].ComOilProductHigh;
            result.ComOilProductLow = ComOilProductLimitList[i].ComOilProductLow;
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
    //配方优化场景1优化目标设置表格
    public ApiModel Set2()//model里的名字 多个数据用IEnumberable，单个数据不用
    {
        var OptimizeObjList = context.Recipecalc2s.Where(m => m.Apply == 1).ToList();
        List<Recipecalc_1_2> ResultList = new List<Recipecalc_1_2>();//列表，里面可以添加很多个对象
        for(int i = 0; i < OptimizeObjList.Count; i++){
            Recipecalc_1_2 result = new Recipecalc_1_2();//实体，可以理解为一个对象  
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

    [HttpGet("Set/ProdOilProductLimit")]
    //配方优化场景1成品油产量限制设置表格（上限）
    public ApiModel Set3()//model里的名字 多个数据用IEnumberable，单个数据不用
    {
        var ProdOilProductLimitList = context.Recipecalc3s.Where(m => m.Apply == 1).ToList();
        List<Recipecalc_1_3> ResultList = new List<Recipecalc_1_3>();//列表，里面可以添加很多个对象
        for(int i = 0; i < ProdOilProductLimitList.Count; i++){
            Recipecalc_1_3 result = new Recipecalc_1_3();//实体，可以理解为一个对象  
            result.ProdOilName = ProdOilProductLimitList[i].ProdOilName;
            result.ProdOilProduct = ProdOilProductLimitList[i].ProdOilProduct;
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

    [HttpGet("Set/ComOilTankVol")]
    //配方优化场景1组分油罐容设置
    public ApiModel Set4()//model里的名字 多个数据用IEnumberable，单个数据不用
    {
        var ComOilTankVolList = context.Recipecalc1s.ToList();
        List<Recipecalc_1_4> ResultList = new List<Recipecalc_1_4>();//列表，里面可以添加很多个对象
        for(int i = 0; i < ComOilTankVolList.Count; i++){
            Recipecalc_1_4 result = new Recipecalc_1_4();//实体，可以理解为一个对象  
            result.ComOilName = ComOilTankVolList[i].ComOilName;
            result.IniVolume = ComOilTankVolList[i].IniVolume;
            result.HighVolume = ComOilTankVolList[i].VolumeHigh;
            result.LowVolume = ComOilTankVolList[i].VolumeLow;
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

    [HttpGet("Res/ComOilProductSug")]
    //配方优化场景1计算结果：组分油产量分配（建议产量）
    public ApiModel Res1()//model里的名字 多个数据用IEnumberable，单个数据不用
    {
        IRecipeCalc _RecipeCalc = new RecipeCalc(context);
        int status = (int)_RecipeCalc.GetRecipe1()[_RecipeCalc.GetRecipe1().Length - 1];
        var list = _RecipeCalc.GetRecipecalc_1Res_ComOilSugProduct().ToList();
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

    [HttpGet("Res/Product")]
    //配方优化场景1计算结果：成品油质量产量 
    public ApiModel Res2()//model里的名字 多个数据用IEnumberable，单个数据不用
    {
        IRecipeCalc _RecipeCalc = new RecipeCalc(context);
        var list = _RecipeCalc.GetRecipecalc_1Res_ProdOilProduct().ToList();
        return new ApiModel()
        {
        code = 200,
        //data = JsonConvert.SerializeObject(list),
        data = list,
        msg = "查询成功"
        };

    }

    [HttpGet("Res/Recipe")]
    //配方优化场景1计算结果：成品油优化配方
    public ApiModel Res3()//model里的名字 多个数据用IEnumberable，单个数据不用
    {
        IRecipeCalc _RecipeCalc = new RecipeCalc(context);
        var list = _RecipeCalc.GetRecipecalc_1Res_Recipe().ToList();
        return new ApiModel()
        {
        code = 200,
        //data = JsonConvert.SerializeObject(list),
        data = list,
        msg = "查询成功"
        };

    }

    [HttpGet("Res/Property")]
    //配方优化场景1计算结果：成品油属性
    public ApiModel Res4()//model里的名字 多个数据用IEnumberable，单个数据不用
    {
        IRecipeCalc _RecipeCalc = new RecipeCalc(context);
        var list = _RecipeCalc.GetRecipecalc_1Res_Property().ToList();
        return new ApiModel()
        {
        code = 200,
        //data = JsonConvert.SerializeObject(list),
        data = list,
        msg = "查询成功"
        };

    }


}
