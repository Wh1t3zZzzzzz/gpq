using Microsoft.AspNetCore.Mvc;
using OilBlendSystem.Models.Diesel.DataBaseModel;
using OilBlendSystem.Models.Diesel.ConstructModel;
using OilBlendSystem.Models;
using OilSystem.ReturnClass;
using OilBlendSystem.BLL.Implementation.Diesel;
using OilBlendSystem.BLL.Interface.Diesel;

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

    [HttpPut("Put/ComOilProductLimit")]
    //配方优化场景1组分油产量范围设置表格——修改保存功能
    public ApiModel Put1(Recipecalc_1_1_index obj)//model里的名字 多个数据用IEnumberable，单个数据不用
    {
        var ComOilProductLimitList = context.Recipecalc1s.ToList();
        var list1 = context.Schemeverify1s.ToList();
        var list2 = context.Compoilconfigs.ToList();

        // if(0 <= obj.ComOilProductLow && obj.ComOilProductLow <= obj.ComOilProductHigh && obj.ComOilProductHigh <= 9999999999){
        ComOilProductLimitList[obj.index].ComOilName = obj.ComOilName;
        list1[obj.index].ComOilName = obj.ComOilName;
        list2[obj.index].ComOilName = obj.ComOilName;
        ComOilProductLimitList[obj.index].ComOilProductHigh = obj.ComOilProductHigh;
        ComOilProductLimitList[obj.index].ComOilProductLow = obj.ComOilProductLow;

        context.Recipecalc1s.Update(ComOilProductLimitList[obj.index]);
        context.Schemeverify1s.Update(list1[obj.index]);
        context.Compoilconfigs.Update(list2[obj.index]);
        context.SaveChanges();
    
        return new ApiModel()
        {
        code = 200,
        //data = JsonConvert.SerializeObject(list),
        data = ComOilProductLimitList,
        msg = "查询成功"
        };            
        // }else{
        //     return new ApiModel(){
        //         code = 500,
        //         //data = JsonConvert.SerializeObject(list),
        //         data = null,
        //         msg = @"组分油产量范围设置需遵循以下条件: 
        //         1) 组分油产量低限大于等于0
        //         2) 组分油产量低限小于等于高限
        //         3) 组分油产量高限小于等于999999999"
        //     };         
        // }
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

    [HttpPut("Put/OptimizeObj")]
    //配方优化场景1优化目标设置表格——修改保存功能
    public ApiModel Put2(Recipecalc_1_2_index obj)//model里的名字 多个数据用IEnumberable，单个数据不用
    {
        var OptimizeObjList = context.Recipecalc2s.Where(m => m.Apply == 1).ToList();
        OptimizeObjList[obj.index].Weight = obj.Weight;
        context.Recipecalc2s.Update(OptimizeObjList[obj.index]);//是按经过where查询后的list顺序更改的
        context.SaveChanges();   
        return new ApiModel()
        {
        code = 200,
        //data = JsonConvert.SerializeObject(list),
        data = OptimizeObjList,
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

    [HttpPut("Put/ProdOilProductLimit")]
    //配方优化场景1成品油产量限制设置表格（上限）——修改保存功能
    public ApiModel Put3(Recipecalc_1_3_index obj)//model里的名字 多个数据用IEnumberable，单个数据不用
    {
        var ProdOilProductLimitList = context.Recipecalc3s.Where(m => m.Apply == 1).ToList();
        var list1 = context.Schemeverify2s.ToList();
        var list2 = context.Prodoilconfigs.ToList();

        ProdOilProductLimitList[obj.index].ProdOilName = obj.ProdOilName;
        list1[obj.index].ProdOilName = obj.ProdOilName;
        list2[obj.index].ProdOilName = obj.ProdOilName;
        ProdOilProductLimitList[obj.index].ProdOilProduct = obj.ProdOilProduct;

        context.Recipecalc3s.Update(ProdOilProductLimitList[obj.index]);
        context.Schemeverify2s.Update(list1[obj.index]);
        context.Prodoilconfigs.Update(list2[obj.index]);
        context.SaveChanges();
    
        return new ApiModel()
        {
        code = 200,
        //data = JsonConvert.SerializeObject(list),
        data = ProdOilProductLimitList,
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

    [HttpPut("Put/ComOilTankVol")]
    //配方优化场景1组分油罐容设置——修改保存功能
    public ApiModel Put4(Recipecalc_1_4_index obj)//model里的名字 多个数据用IEnumberable，单个数据不用
    {
        var ComOilTankVolList = context.Recipecalc1s.ToList();
        var list1 = context.Schemeverify1s.ToList();
        var list2 = context.Compoilconfigs.ToList();

        ComOilTankVolList[obj.index].ComOilName = obj.ComOilName;
        list1[obj.index].ComOilName = obj.ComOilName;
        list2[obj.index].ComOilName = obj.ComOilName;
        ComOilTankVolList[obj.index].IniVolume = obj.IniVolume;
        ComOilTankVolList[obj.index].VolumeHigh = obj.HighVolume;
        ComOilTankVolList[obj.index].VolumeLow = obj.LowVolume;

        context.Recipecalc1s.Update(ComOilTankVolList[obj.index]);
        context.Schemeverify1s.Update(list1[obj.index]);
        context.Compoilconfigs.Update(list2[obj.index]);
        context.SaveChanges();
    
        return new ApiModel()
        {
        code = 200,
        //data = JsonConvert.SerializeObject(list),
        data = ComOilTankVolList,
        msg = "查询成功"
        };

    }

    [HttpGet("Res/ComOilProductSug")]
    //配方优化场景1计算结果：组分油产量分配（建议产量）
    public ApiModel Res1()//model里的名字 多个数据用IEnumberable，单个数据不用
    {
        IRecipeCalc _RecipeCalc = new RecipeCalc(context);
        var list = _RecipeCalc.GetRecipecalc_1Res_ComOilSugProduct().ToList();
        int status = (int)_RecipeCalc.GetRecipe1()[_RecipeCalc.GetRecipe1().Length - 1];
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
