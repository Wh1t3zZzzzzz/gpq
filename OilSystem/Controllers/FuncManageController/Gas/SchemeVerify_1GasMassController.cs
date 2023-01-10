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
public class SchemeVerify_1GasMassController : ControllerBase
{

    private readonly oilblendContext context;

    public SchemeVerify_1GasMassController(oilblendContext _context)
    {
       context = _context;
    }

    [HttpGet("Set/ProdOilProduct")]
    //质量
    //方案验证场景1成品油产量分配表格
    public ApiModel Set1()//model里的名字 多个数据用IEnumberable，单个数据不用
    {
        var ProdOilProductList = context.Schemeverify1_gases.ToList();
        List<GasSchemeVerify_1_1> ResultList = new List<GasSchemeVerify_1_1>();//列表，里面可以添加很多个对象
        for(int i = 0; i < ProdOilProductList.Count; i++){
            GasSchemeVerify_1_1 result = new GasSchemeVerify_1_1();//实体，可以理解为一个对象  
            result.ComOilName = ProdOilProductList[i].ComOilName;
            result.gas92Product = ProdOilProductList[i].gas92QualityProduct;
            result.gas95Product = ProdOilProductList[i].gas95QualityProduct;
            result.gas98Product = ProdOilProductList[i].gas98QualityProduct;
            result.gasSelfProduct = ProdOilProductList[i].gasSelfQualityProduct;
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
    public ApiModel Put1(GasSchemeVerify_1_1_index obj)//model里的名字 多个数据用IEnumberable，单个数据不用
    {
        var ProdOilProductList = context.Schemeverify1_gases.ToList();
        var list1 = context.Recipecalc1_gases.ToList();
        var list2 = context.Compoilconfig_gases.ToList();

        // if(0 <= obj.gas92Product && obj.gas92Product <= 9999999999 && 0 <= obj.gas95Product && obj.gas95Product <= 9999999999 
        // && 0 <= obj.gas98Product && obj.gas98Product <= 9999999999 && 0 <= obj.gasSelfProduct && obj.gasSelfProduct <= 9999999999){
        ProdOilProductList[obj.index].ComOilName = obj.ComOilName;
        list1[obj.index].ComOilName = obj.ComOilName;
        list2[obj.index].ComOilName = obj.ComOilName;
        ProdOilProductList[obj.index].gas92QualityProduct = obj.gas92Product;
        ProdOilProductList[obj.index].gas95QualityProduct = obj.gas95Product;
        ProdOilProductList[obj.index].gas98QualityProduct = obj.gas98Product;
        ProdOilProductList[obj.index].gasSelfQualityProduct = obj.gasSelfProduct;

        context.Schemeverify1_gases.Update(ProdOilProductList[obj.index]);
        context.Recipecalc1_gases.Update(list1[obj.index]);
        context.Compoilconfig_gases.Update(list2[obj.index]);
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
        var BottomInfoList = context.Schemeverify2_gases.ToList();
        List<GasSchemeVerify_1_2> ResultList = new List<GasSchemeVerify_1_2>();//列表，里面可以添加很多个对象
        for(int i = 0; i < BottomInfoList.Count; i++){
            GasSchemeVerify_1_2 result = new GasSchemeVerify_1_2();//实体，可以理解为一个对象  
            result.ProdOilName = BottomInfoList[i].ProdOilName;
            result.BottomCapacity = BottomInfoList[i].BottomMass;
            result.Bottomron = BottomInfoList[i].ronMass;
            result.Bottomt50 = BottomInfoList[i].t50Mass;
            result.Bottomsuf = BottomInfoList[i].sufMass;
            result.Bottomden = BottomInfoList[i].denMass;
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
    public ApiModel Put2(GasSchemeVerify_1_2_index obj)//model里的名字 多个数据用IEnumberable，单个数据不用
    {
        var BottomInfoList = context.Schemeverify2_gases.ToList();
        var list1 = context.Recipecalc3_gases.ToList();
        var list2 = context.Prodoilconfig_gases.ToList();

        // if(0 <= obj.BottomCapacity && obj.BottomCapacity <= 999999 
        // && 40 <= obj.Bottomron && obj.Bottomron <= 70 
        // && 200 <= obj.Bottomt50 && obj.Bottomt50 <= 300 
        // && 0 < obj.Bottomsuf && obj.Bottomsuf <= 7 
        // && 700 <= obj.Bottomden && obj.Bottomden <= 900){
        BottomInfoList[obj.index].ProdOilName = obj.ProdOilName;
        list1[obj.index].ProdOilName = obj.ProdOilName;
        list2[obj.index].ProdOilName = obj.ProdOilName;
        BottomInfoList[obj.index].BottomMass = obj.BottomCapacity;
        BottomInfoList[obj.index].ronMass = obj.Bottomron;
        BottomInfoList[obj.index].t50Mass = obj.Bottomt50;
        BottomInfoList[obj.index].sufMass = obj.Bottomsuf;
        BottomInfoList[obj.index].denMass = obj.Bottomden;

        context.Schemeverify2_gases.Update(BottomInfoList[obj.index]);
        context.Recipecalc3_gases.Update(list1[obj.index]);
        context.Prodoilconfig_gases.Update(list2[obj.index]);
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

        var TotalBlendList = context.Schemeverify2_gases.ToList();
        List<GasSchemeVerify_1_3> ResultList = new List<GasSchemeVerify_1_3>();//列表，里面可以添加很多个对象
        for(int i = 0; i < TotalBlendList.Count; i++){
            GasSchemeVerify_1_3 result = new GasSchemeVerify_1_3();//实体，可以理解为一个对象  
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
    public ApiModel Put3(GasSchemeVerify_1_3_index obj)//model里的名字 多个数据用IEnumberable，单个数据不用
    {
        var TotalBlendList = context.Schemeverify2_gases.ToList();
        var list1 = context.Recipecalc3_gases.ToList();
        var list2 = context.Prodoilconfig_gases.ToList();
        var list3 = context.Schemeverify2_gases.ToList();

        if(list3[obj.index].BottomMass < obj.ProdTotalBlend){
        TotalBlendList[obj.index].ProdOilName = obj.ProdOilName;
        list1[obj.index].ProdOilName = obj.ProdOilName;
        list2[obj.index].ProdOilName = obj.ProdOilName;
        TotalBlendList[obj.index].TotalBlendMass = obj.ProdTotalBlend;

        context.Schemeverify2_gases.Update(TotalBlendList[obj.index]);
        context.Recipecalc3_gases.Update(list1[obj.index]);
        context.Prodoilconfig_gases.Update(list2[obj.index]);
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
