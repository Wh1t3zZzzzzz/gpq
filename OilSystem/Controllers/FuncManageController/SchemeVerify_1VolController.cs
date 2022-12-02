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
public class SchemeVerify_1VolController : ControllerBase
{

    private readonly oilblendContext context;

    public SchemeVerify_1VolController(oilblendContext _context)
    {
       context = _context;
    }

    [HttpGet("Set/ProdOilProduct")]
    //体积
    //方案验证场景1成品油产量分配表格
    public ApiModel Set1()//model里的名字 多个数据用IEnumberable，单个数据不用
    {
        var ProdOilProductList = context.Schemeverify1s.ToList();
        List<SchemeVerify_1_1> ResultList = new List<SchemeVerify_1_1>();//列表，里面可以添加很多个对象
        for(int i = 0; i < ProdOilProductList.Count; i++){
            SchemeVerify_1_1 result = new SchemeVerify_1_1();//实体，可以理解为一个对象  
            result.ComOilName = ProdOilProductList[i].ComOilName;
            result.AutoProduct = ProdOilProductList[i].AutoVolumeProduct;
            result.ExpProduct = ProdOilProductList[i].ExpVolumeProduct;
            result.Prod1Product = ProdOilProductList[i].Prod1VolumeProduct;
            result.Prod2Product = ProdOilProductList[i].Prod2VolumeProduct;
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
    //体积
    //方案验证场景1成品油产量分配表格——修改保存功能
    public ApiModel Put1(SchemeVerify_1_1_index obj)//model里的名字 多个数据用IEnumberable，单个数据不用
    {
        var ProdOilProductList = context.Schemeverify1s.ToList();
        var list1 = context.Recipecalc1s.ToList();
        var list2 = context.Compoilconfigs.ToList();

        ProdOilProductList[obj.index].ComOilName = obj.ComOilName;
        list1[obj.index].ComOilName = obj.ComOilName;
        list2[obj.index].ComOilName = obj.ComOilName;
        ProdOilProductList[obj.index].AutoVolumeProduct = obj.AutoProduct;
        ProdOilProductList[obj.index].ExpVolumeProduct = obj.ExpProduct;
        ProdOilProductList[obj.index].Prod1VolumeProduct = obj.Prod1Product;
        ProdOilProductList[obj.index].Prod2VolumeProduct = obj.Prod2Product;

        context.Schemeverify1s.Update(ProdOilProductList[obj.index]);
        context.Recipecalc1s.Update(list1[obj.index]);
        context.Compoilconfigs.Update(list2[obj.index]);
        context.SaveChanges();
    
        return new ApiModel()
        {
        code = 200,
        //data = JsonConvert.SerializeObject(list),
        data = ProdOilProductList,
        msg = "查询成功"
        };

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
            result.BottomCapacity = BottomInfoList[i].BottomVolume;
            result.BottomCET = BottomInfoList[i].CetVol;
            result.BottomD50 = BottomInfoList[i].D50Vol;
            result.BottomPOL = BottomInfoList[i].PolVol;
            result.BottomDEN = BottomInfoList[i].DenVol;
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
    //体积
    //方案验证场景1罐底油信息配置表格——修改保存功能
    public ApiModel Put2(SchemeVerify_1_2_index obj)//model里的名字 多个数据用IEnumberable，单个数据不用
    {
        var BottomInfoList = context.Schemeverify2s.ToList();
        var list1 = context.Recipecalc3s.ToList();
        var list2 = context.Prodoilconfigs.ToList();

        BottomInfoList[obj.index].ProdOilName = obj.ProdOilName;
        list1[obj.index].ProdOilName = obj.ProdOilName;
        list2[obj.index].ProdOilName = obj.ProdOilName;
        BottomInfoList[obj.index].BottomVolume = obj.BottomCapacity;
        BottomInfoList[obj.index].CetVol = obj.BottomCET;
        BottomInfoList[obj.index].D50Vol = obj.BottomD50;
        BottomInfoList[obj.index].PolVol = obj.BottomPOL;
        BottomInfoList[obj.index].DenVol = obj.BottomDEN;

        context.Schemeverify2s.Update(BottomInfoList[obj.index]);
        context.Recipecalc3s.Update(list1[obj.index]);
        context.Prodoilconfigs.Update(list2[obj.index]);
        context.SaveChanges();
    
        return new ApiModel()
        {
        code = 200,
        //data = JsonConvert.SerializeObject(list),
        data = BottomInfoList,
        msg = "查询成功"
        };

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
            result.ProdTotalBlend = TotalBlendList[i].TotalBlendVol;
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
    //体积
    //方案验证场景1调合总量设置表格（含罐底油）——修改保存功能
    public ApiModel Put3(SchemeVerify_1_3_index obj)//model里的名字 多个数据用IEnumberable，单个数据不用
    {
        var TotalBlendList = context.Schemeverify2s.ToList();
        var list1 = context.Recipecalc3s.ToList();
        var list2 = context.Prodoilconfigs.ToList();

        TotalBlendList[obj.index].ProdOilName = obj.ProdOilName;
        list1[obj.index].ProdOilName = obj.ProdOilName;
        list2[obj.index].ProdOilName = obj.ProdOilName;
        TotalBlendList[obj.index].TotalBlendVol = obj.ProdTotalBlend;

        context.Schemeverify2s.Update(TotalBlendList[obj.index]);
        context.Recipecalc3s.Update(list1[obj.index]);
        context.Prodoilconfigs.Update(list2[obj.index]);
        context.SaveChanges();
    
        return new ApiModel()
        {
        code = 200,
        //data = JsonConvert.SerializeObject(list),
        data = TotalBlendList,
        msg = "查询成功"
        };

    }

    [HttpGet("Res/Time")]
    //方案验证场景1计算结果：调合时间
    public ApiModel Res1()//model里的名字 多个数据用IEnumberable，单个数据不用
    {
         ISchemeVerify _SchemeVerify = new SchemeVerify(context);
        var list = _SchemeVerify.GetSchemeverifyResult1_vol_Time().ToList();
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
        var list = _SchemeVerify.GetSchemeverifyResult1_vol_Product().ToList();
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
        var list = _SchemeVerify.GetSchemeverifyResult1_vol_Property().ToList();
        return new ApiModel()
        {
        code = 200,
        //data = JsonConvert.SerializeObject(list),
        data = list,
        msg = "查询成功"
        };

    }

}







