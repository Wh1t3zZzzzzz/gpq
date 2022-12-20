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
public class SchemeVerify_1MassController : ControllerBase
{

    private readonly oilblendContext context;

    public SchemeVerify_1MassController(oilblendContext _context)
    {
       context = _context;
    }

    [HttpGet("Set/ProdOilProduct")]
    //质量
    //方案验证场景1成品油产量分配表格
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

    [HttpPut("Put/ProdOilProduct")]
    //质量
    //方案验证场景1成品油产量分配表格——修改保存功能
    public ApiModel Put1(SchemeVerify_1_1_index obj)//model里的名字 多个数据用IEnumberable，单个数据不用
    {
        var ProdOilProductList = context.Schemeverify1s.ToList();
        var list1 = context.Recipecalc1s.ToList();
        var list2 = context.Compoilconfigs.ToList();

        // if(0 <= obj.AutoProduct && obj.AutoProduct <= 9999999999 && 0 <= obj.ExpProduct && obj.ExpProduct <= 9999999999 
        // && 0 <= obj.Prod1Product && obj.Prod1Product <= 9999999999 && 0 <= obj.Prod2Product && obj.Prod2Product <= 9999999999){
        ProdOilProductList[obj.index].ComOilName = obj.ComOilName;
        list1[obj.index].ComOilName = obj.ComOilName;
        list2[obj.index].ComOilName = obj.ComOilName;
        ProdOilProductList[obj.index].AutoQualityProduct = obj.AutoProduct;
        ProdOilProductList[obj.index].ExpQualityProduct = obj.ExpProduct;
        ProdOilProductList[obj.index].Prod1QualityProduct = obj.Prod1Product;
        ProdOilProductList[obj.index].Prod2QualityProduct = obj.Prod2Product;

        context.Schemeverify1s.Update(ProdOilProductList[obj.index]);
        context.Recipecalc1s.Update(list1[obj.index]);
        context.Compoilconfigs.Update(list2[obj.index]);
        context.SaveChanges();
    
        return new ApiModel()
        {
        code = 200,
        //data = JsonConvert.SerializeObject(list),
        data = ProdOilProductList,
        msg = "修改成功"
        };
        // }else{
        //     return new ApiModel(){
        //         code = 500,
        //         //data = JsonConvert.SerializeObject(list),
        //         data = null,
        //         msg = @"成品油天产量超出限制: [0,9999999999]"
        //     };           
        // }
    }

    [HttpGet("Set/BottomInfo")]
    //方案验证场景1罐底油信息配置表格
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

    [HttpPut("Put/BottomInfo")]
    //质量
    //方案验证场景1罐底油信息配置表格——修改保存功能
    public ApiModel Put2(SchemeVerify_1_2_index obj)//model里的名字 多个数据用IEnumberable，单个数据不用
    {
        var BottomInfoList = context.Schemeverify2s.ToList();
        var list1 = context.Recipecalc3s.ToList();
        var list2 = context.Prodoilconfigs.ToList();

        // if(0 <= obj.BottomCapacity && obj.BottomCapacity <= 999999 
        // && 40 <= obj.BottomCET && obj.BottomCET <= 70 
        // && 200 <= obj.BottomD50 && obj.BottomD50 <= 300 
        // && 0 < obj.BottomPOL && obj.BottomPOL <= 7 
        // && 700 <= obj.BottomDEN && obj.BottomDEN <= 900){
        BottomInfoList[obj.index].ProdOilName = obj.ProdOilName;
        list1[obj.index].ProdOilName = obj.ProdOilName;
        list2[obj.index].ProdOilName = obj.ProdOilName;
        BottomInfoList[obj.index].BottomMass = obj.BottomCapacity;
        BottomInfoList[obj.index].CetMass = obj.BottomCET;
        BottomInfoList[obj.index].D50Mass = obj.BottomD50;
        BottomInfoList[obj.index].PolMass = obj.BottomPOL;
        BottomInfoList[obj.index].DenMass = obj.BottomDEN;

        context.Schemeverify2s.Update(BottomInfoList[obj.index]);
        context.Recipecalc3s.Update(list1[obj.index]);
        context.Prodoilconfigs.Update(list2[obj.index]);
        context.SaveChanges();
    
        return new ApiModel()
        {
        code = 200,
        //data = JsonConvert.SerializeObject(list),
        data = BottomInfoList,
        msg = "修改成功"
        };
        // }else{
        //     return new ApiModel(){
        //         code = 500,
        //         //data = JsonConvert.SerializeObject(list),
        //         data = null,
        //         msg = @"罐底油信息配置需遵循以下条件: 
        //         1) 罐底油体积/质量: [0,999999]
        //         2) 十六烷值指数: [40,70] 
        //         3) 50%回收温度(℃): [200,300] 
        //         4) 多环芳烃含量(wt%): (0,7] 
        //         5) 密度(kg/m³): [700,900]"
        //     };            
        // }
    }

    [HttpGet("Set/TotalBlend")]
    //方案验证场景1调合总量设置表格（含罐底油）
    public ApiModel Set3()//model里的名字 多个数据用IEnumberable，单个数据不用
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

    [HttpPut("Put/TotalBlend")]
    //质量
    //方案验证场景1调合总量设置表格（含罐底油）——修改保存功能
    public ApiModel Put3(SchemeVerify_1_3_index obj)//model里的名字 多个数据用IEnumberable，单个数据不用
    {
        var TotalBlendList = context.Schemeverify2s.ToList();
        var list1 = context.Recipecalc3s.ToList();
        var list2 = context.Prodoilconfigs.ToList();
        var list3 = context.Schemeverify2s.ToList();

        if(list3[obj.index].BottomMass < obj.ProdTotalBlend){
        TotalBlendList[obj.index].ProdOilName = obj.ProdOilName;
        list1[obj.index].ProdOilName = obj.ProdOilName;
        list2[obj.index].ProdOilName = obj.ProdOilName;
        TotalBlendList[obj.index].TotalBlendMass = obj.ProdTotalBlend;

        context.Schemeverify2s.Update(TotalBlendList[obj.index]);
        context.Recipecalc3s.Update(list1[obj.index]);
        context.Prodoilconfigs.Update(list2[obj.index]);
        context.SaveChanges();
    
        return new ApiModel()
        {
        code = 200,
        //data = JsonConvert.SerializeObject(list),
        data = TotalBlendList,
        msg = "修改成功"
        };
        }else{
            return new ApiModel(){
                code = 401,
                //data = JsonConvert.SerializeObject(list),
                data = null,
                msg = @"成品油调合总量应大于罐底油体积/质量"
            };               
        }
    }

    [HttpGet("Res/Time")]
    //方案验证场景1计算结果：调合时间
    public ApiModel Res1()//model里的名字 多个数据用IEnumberable，单个数据不用
    {
        ISchemeVerify _SchemeVerify = new SchemeVerify(context);
        var list = _SchemeVerify.GetSchemeverifyResult1_mass_Time().ToList();
        return new ApiModel()
        {
        code = 200,
        //data = JsonConvert.SerializeObject(list),
        data = list,
        msg = "查询成功"
        };
    }

    [HttpGet("Res/Product")]
    //方案验证场景1计算结果：成品油产量
    public ApiModel Res2()//model里的名字 多个数据用IEnumberable，单个数据不用
    {
        ISchemeVerify _SchemeVerify = new SchemeVerify(context);
        var list = _SchemeVerify.GetSchemeverifyResult1_mass_Product().ToList();
        return new ApiModel()
        {
        code = 200,
        //data = JsonConvert.SerializeObject(list),
        data = list,
        msg = "查询成功"
        };

    }

    [HttpGet("Res/Property")]
    //方案验证场景1计算结果：成品油属性
    public ApiModel Res3()//model里的名字 多个数据用IEnumberable，单个数据不用
    {
        ISchemeVerify _SchemeVerify = new SchemeVerify(context);
        var list = _SchemeVerify.GetSchemeverifyResult1_mass_Property().ToList();
        return new ApiModel()
        {
        code = 200,
        //data = JsonConvert.SerializeObject(list),
        data = list,
        msg = "查询成功"
        };

    }


}
