using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
public class CompOilConfigGasController : ControllerBase
{
    private readonly oilblendContext context;

    public CompOilConfigGasController(oilblendContext _context)
    {
       context = _context;
    }

    [HttpGet]
    public ApiModel Get()//model里的名字 多个数据用IEnumberable，单个数据不用
    {
        ICompOilConfig _CompOilConfig = new CompOilConfig(context);
        var list = _CompOilConfig.GetAllCompOilConfigList().ToList();
        List<GasComproperty> ResultList = new List<GasComproperty>();//列表，里面可以添加很多个对象
        for(int i = 0; i < list.Count; i++){
            GasComproperty result = new GasComproperty();//实体，可以理解为一个对象  
            result.ComOilName = list[i].ComOilName;
            result.ron = list[i].ron;
            result.t50 = list[i].t50;
            result.suf = list[i].suf;
            result.den = list[i].den;
            result.Price = list[i].Price;
            ResultList.Add(result); 
        }   
        return new ApiModel()
        {
        code = 200,
        data = ResultList,
        msg = "查询成功"
        };

    }

    [HttpPut]
    public ApiModel Put(GasComproperty_index obj)//前端新增时调用post
    {
        context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
        ICompOilConfig _CompOilConfig = new CompOilConfig(context);  
        if(obj.action == "add"){    
            //增加操作
            Compoilconfig_gas comp = new Compoilconfig_gas();
            Recipecalc1_gas recipecalc1 = new Recipecalc1_gas();
            Schemeverify1_gas schemeverify1 = new Schemeverify1_gas();
            context.Compoilconfig_gases.Add(comp);
            context.Recipecalc1_gases.Add(recipecalc1);
            context.Schemeverify1_gases.Add(schemeverify1);
            context.SaveChanges();

            //更改保存操作
            var list = context.Compoilconfig_gases.ToList();//增加行过后的表格数据
            var list2 = context.Recipecalc1_gases.ToList();//recipecalc1表格
            var list3 = context.Schemeverify1_gases.ToList();//schemeverify1表格

            // if(40 <= obj.ron && obj.ron <= 70 
            // && 200 <= obj.t50 && obj.t50 <= 300 
            // && 0 < obj.suf && obj.suf <= 7 
            // && 700 <= obj.den && obj.den <= 900 
            // && 0 < obj.Price && obj.Price < 999999999){
            list[obj.index].ComOilName = obj.ComOilName;
            list2[obj.index].ComOilName = obj.ComOilName;
            list3[obj.index].ComOilName = obj.ComOilName;
            list[obj.index].ron = obj.ron;
            list[obj.index].t50 = obj.t50;
            list[obj.index].suf = obj.suf;
            list[obj.index].den = obj.den;
            list[obj.index].Price = obj.Price;
            context.Compoilconfig_gases.Update(list[obj.index]);
            context.Recipecalc1_gases.Update(list2[obj.index]);
            context.Schemeverify1_gases.Update(list3[obj.index]);
            context.SaveChanges();
            var list1 = context.Compoilconfig_gases.ToList();//增加行并且修改后的表格数据         
            return new ApiModel()
            {
            code = 200,
            //data = JsonConvert.SerializeObject(list),
            data = null,
            msg = "增加成功"
            };
            // }else{
            //     return new ApiModel(){
            //         code = 500,
            //         //data = JsonConvert.SerializeObject(list),
            //         data = null,
            //         msg = @"组分油属性值应尽量满足: 
            //         十六烷值指数: [40,70]
            //         50%回收温度(℃): [200,300] 
            //         多环芳烃含量(wt%): (0,7] 
            //         密度(kg/m³): [700,900]"
            //     };
            // }
        }else{
            //更改保存操作
            var list = context.Compoilconfig_gases.ToList();//打印最新的表格数据
            var list2 = context.Recipecalc1_gases.ToList();//增加行过后的表格数据
            var list3 = context.Schemeverify1_gases.ToList();//增加行过后的表格数据

            // if(40 <= obj.ron && obj.ron <= 70 
            // && 200 <= obj.t50 && obj.t50 <= 300 
            // && 0 < obj.suf && obj.suf <= 7 
            // && 700 <= obj.den && obj.den <= 900
            // && 0 < obj.Price && obj.Price < 999999999){
            list[obj.index].ComOilName = obj.ComOilName;
            list2[obj.index].ComOilName = obj.ComOilName;
            list3[obj.index].ComOilName = obj.ComOilName;
            list[obj.index].ron = obj.ron;
            list[obj.index].t50 = obj.t50;
            list[obj.index].suf = obj.suf;
            list[obj.index].den = obj.den;
            list[obj.index].Price = obj.Price;
            
            context.Compoilconfig_gases.Update(list[obj.index]);
            context.Recipecalc1_gases.Update(list2[obj.index]);
            context.Schemeverify1_gases.Update(list3[obj.index]);
            context.SaveChanges();
            var list1 = _CompOilConfig.GetAllCompOilConfigList().ToList();
            return new ApiModel()
            {
            code = 200,
            //data = JsonConvert.SerializeObject(list),
            data = list1,
            msg = "修改成功"
            };                    
            // }else{
            //     return new ApiModel(){
            //         code = 500,
            //         //data = JsonConvert.SerializeObject(list),
            //         data = null,
            //         msg = @"组分油属性值应尽量满足: 
            //         十六烷值指数: [40,70] 
            //         50%回收温度(℃): [200,300] 
            //         多环芳烃含量(wt%): (0,7] 
            //         密度(kg/m³): [700,900]"
            //     };
            // }
        }     

    }


    [HttpDelete]
    public ApiModel Delete(GasIndexNumber obj)//IndexNumber obj
    {
        //增加时，要同时把三个表格里的组分油都增加，删除时同理
        //oilblendContext context = new();
        context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
        ICompOilConfig _CompOilConfig = new CompOilConfig(context);
        //List<Compoilconfig> list3 = new List<Compoilconfig>();
        var list = _CompOilConfig.GetAllCompOilConfigList().ToList();//需要把IEnumberable中遍历成List
        var list2 = context.Recipecalc1_gases.ToList();
        var list3 = context.Schemeverify1_gases.ToList();
        if(list.Count == 2){
            return new ApiModel()
            {
            code = 400,
            //data = JsonConvert.SerializeObject(list),
            data = null,
            msg = "组分油个数不允许小于两个"
            };        
        }else{
            context.Compoilconfig_gases.Remove(list[obj.index]);
            context.Recipecalc1_gases.Remove(list2[obj.index]);
            context.Schemeverify1_gases.Remove(list3[obj.index]);
            context.SaveChanges();
            var list1 = _CompOilConfig.GetAllCompOilConfigList().ToList();
            return new ApiModel()
            {
            code = 200,
            //data = JsonConvert.SerializeObject(list),
            data = null,
            msg = "删除成功"
            };            
        }
    }


}
