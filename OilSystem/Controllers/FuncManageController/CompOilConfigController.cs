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
            Compoilconfig comp = new Compoilconfig();
            context.Compoilconfigs.Add(comp);
            context.SaveChanges();
            var list = context.Compoilconfigs.ToList();//增加行过后的表格数据
            list[obj.index].ComOilName = obj.ComOilName;
            // list[obj.index].Cet = obj.Cet;
            // list[obj.index].D50 = obj.D50;
            // list[obj.index].Pol = obj.Pol;
            // list[obj.index].Den = obj.Den;
            // list[obj.index].Price = obj.Price;
            context.Compoilconfigs.Update(list[obj.index]);
            context.SaveChanges();
            var list1 = context.Compoilconfigs.ToList();//增加行并且修改后的表格数据         
            return new ApiModel()
            {
            code = 200,
            //data = JsonConvert.SerializeObject(list),
            data = list1,
            msg = "增加成功"
            };
        }else{
            var list = context.Compoilconfigs.ToList();//打印最新的表格数据
            list[obj.index].ComOilName = obj.ComOilName;
            // list[obj.index].Cet = obj.Cet;
            // list[obj.index].D50 = obj.D50;
            // list[obj.index].Pol = obj.Pol;
            // list[obj.index].Den = obj.Den;
            // list[obj.index].Price = obj.Price;
            context.Compoilconfigs.Update(list[obj.index]);
            context.SaveChanges();
            var list1 = _CompOilConfig.GetAllCompOilConfigList().ToList();
            return new ApiModel()
            {
            code = 200,
            //data = JsonConvert.SerializeObject(list),
            data = list1,
            msg = "查询成功"
            };            
        }     

    }


    [HttpDelete]
    public ApiModel Delete(IndexNumber obj)//IndexNumber obj
    {
        //oilblendContext context = new();
        context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
        ICompOilConfig _CompOilConfig = new CompOilConfig(context);
        //List<Compoilconfig> list3 = new List<Compoilconfig>();
        var list = _CompOilConfig.GetAllCompOilConfigList().ToList();//需要把IEnumberable中遍历成List
        context.Compoilconfigs.Remove(list[obj.index]);
        context.SaveChanges();
        var list1 = _CompOilConfig.GetAllCompOilConfigList().ToList();
        return new ApiModel()
        {
        code = 200,
        //data = JsonConvert.SerializeObject(list),
        data = list1,
        msg = "查询成功"
        };
    }


}
