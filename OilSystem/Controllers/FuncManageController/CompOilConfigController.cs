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
public class CompOilConfigController : ControllerBase
{

    private readonly oilblendContext context;

    public CompOilConfigController(oilblendContext _context)
    {
       context = _context;
    }

    [HttpGet]
    public ApiModel Get()//model里的名字 多个数据用IEnumberable，单个数据不用
    {
        ICompOilConfig _CompOilConfig = new CompOilConfig(context);
        var list = _CompOilConfig.GetAllCompOilConfigList().ToList();
        List<Comproperty> ResultList = new List<Comproperty>();//列表，里面可以添加很多个对象
        for(int i = 0; i < list.Count; i++){
            Comproperty result = new Comproperty();//实体，可以理解为一个对象  
            result.ComOilName = list[i].ComOilName;
            result.Cet = list[i].Cet;
            result.D50 = list[i].D50;
            result.Pol = list[i].Pol;
            result.Den = list[i].Den;
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
    public ApiModel Put(Comproperty_index obj)//前端新增时调用post
    {
        context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
        ICompOilConfig _CompOilConfig = new CompOilConfig(context);  
        if(obj.action == "add"){    
            //增加操作
            Compoilconfig comp = new Compoilconfig();
            Recipecalc1 recipecalc1 = new Recipecalc1();
            Schemeverify1 schemeverify1 = new Schemeverify1();
            context.Compoilconfigs.Add(comp);
            context.Recipecalc1s.Add(recipecalc1);
            context.Schemeverify1s.Add(schemeverify1);
            context.SaveChanges();

            //更改保存操作
            var list = context.Compoilconfigs.ToList();//增加行过后的表格数据
            var list2 = context.Recipecalc1s.ToList();//recipecalc1表格
            var list3 = context.Schemeverify1s.ToList();//schemeverify1表格

            if(40 <= obj.Cet && obj.Cet <= 70 
            && 200 <= obj.D50 && obj.D50 <= 300 
            && 0 < obj.Pol && obj.Pol <= 7 
            && 700 <= obj.Den && obj.Den <= 900 
            && 0 < obj.Price && obj.Price < 999999999){
                list[obj.index].ComOilName = obj.ComOilName;
                list2[obj.index].ComOilName = obj.ComOilName;
                list3[obj.index].ComOilName = obj.ComOilName;
                list[obj.index].Cet = obj.Cet;
                list[obj.index].D50 = obj.D50;
                list[obj.index].Pol = obj.Pol;
                list[obj.index].Den = obj.Den;
                list[obj.index].Price = obj.Price;
                context.Compoilconfigs.Update(list[obj.index]);
                context.Recipecalc1s.Update(list2[obj.index]);
                context.Schemeverify1s.Update(list3[obj.index]);
                context.SaveChanges();
                var list1 = context.Compoilconfigs.ToList();//增加行并且修改后的表格数据         
                return new ApiModel()
                {
                code = 200,
                //data = JsonConvert.SerializeObject(list),
                data = null,
                msg = "增加成功"
                };
            }else{
                return new ApiModel(){
                    code = 500,
                    //data = JsonConvert.SerializeObject(list),
                    data = null,
                    msg = @"组分油属性值应尽量满足: 
                    十六烷值指数: [40,70]
                    50%回收温度(℃): [200,300] 
                    多环芳烃含量(wt%): (0,7] 
                    密度(kg/m³): [700,900]"
                };
            }
        }else{
            //更改保存操作
            var list = context.Compoilconfigs.ToList();//打印最新的表格数据
            var list2 = context.Recipecalc1s.ToList();//增加行过后的表格数据
            var list3 = context.Schemeverify1s.ToList();//增加行过后的表格数据

            if(40 <= obj.Cet && obj.Cet <= 70 
            && 200 <= obj.D50 && obj.D50 <= 300 
            && 0 < obj.Pol && obj.Pol <= 7 
            && 700 <= obj.Den && obj.Den <= 900
            && 0 < obj.Price && obj.Price < 999999999){
                list[obj.index].ComOilName = obj.ComOilName;
                list2[obj.index].ComOilName = obj.ComOilName;
                list3[obj.index].ComOilName = obj.ComOilName;
                list[obj.index].Cet = obj.Cet;
                list[obj.index].D50 = obj.D50;
                list[obj.index].Pol = obj.Pol;
                list[obj.index].Den = obj.Den;
                list[obj.index].Price = obj.Price;
                
                context.Compoilconfigs.Update(list[obj.index]);
                context.Recipecalc1s.Update(list2[obj.index]);
                context.Schemeverify1s.Update(list3[obj.index]);
                context.SaveChanges();
                var list1 = _CompOilConfig.GetAllCompOilConfigList().ToList();
                return new ApiModel()
                {
                code = 200,
                //data = JsonConvert.SerializeObject(list),
                data = list1,
                msg = "修改成功"
                };                    
            }else{
                return new ApiModel(){
                    code = 500,
                    //data = JsonConvert.SerializeObject(list),
                    data = null,
                    msg = @"组分油属性值应尽量满足: 
                    十六烷值指数: [40,70] 
                    50%回收温度(℃): [200,300] 
                    多环芳烃含量(wt%): (0,7] 
                    密度(kg/m³): [700,900]"
                };
            }
        }     

    }


    [HttpDelete]
    public ApiModel Delete(IndexNumber obj)//IndexNumber obj
    {
        //增加时，要同时把三个表格里的组分油都增加，删除时同理
        //oilblendContext context = new();
        context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
        ICompOilConfig _CompOilConfig = new CompOilConfig(context);
        //List<Compoilconfig> list3 = new List<Compoilconfig>();
        var list = _CompOilConfig.GetAllCompOilConfigList().ToList();//需要把IEnumberable中遍历成List
        var list2 = context.Recipecalc1s.ToList();
        var list3 = context.Schemeverify1s.ToList();
        if(list.Count == 2){
            return new ApiModel()
            {
            code = 500,
            //data = JsonConvert.SerializeObject(list),
            data = null,
            msg = "组分油个数不允许小于两个"
            };        
        }else{
            context.Compoilconfigs.Remove(list[obj.index]);
            context.Recipecalc1s.Remove(list2[obj.index]);
            context.Schemeverify1s.Remove(list3[obj.index]);
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
