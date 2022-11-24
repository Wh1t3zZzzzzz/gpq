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
    //public IEnumerable<TestTable> Get()//model里的名字
    public ApiModel Get()//model里的名字 多个数据用IEnumberable，单个数据不用
    {

        ICompOilConfig _CompOilConfig = new CompOilConfig(context);
        var list = _CompOilConfig.GetAllCompOilConfigList().ToList();
        //list[0].ComOilName = System.Environment.CurrentDirectory;

        return new ApiModel()
        {
        code = 200,
        //data = JsonConvert.SerializeObject(list),
        data = list,
        msg = "查询成功"
        };
    

       //return menulist;

    }


    [HttpPut]
    public ApiModel Put(Comproperty obj)//前端新增时调用post
    {
        context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
        ICompOilConfig _CompOilConfig = new CompOilConfig(context);
        if(obj.action == "edit"){        
            var list = context.Compoilconfigs.Where(m=>m.Id == obj.ID).ToList();
            list[0].ComOilName = obj.ComOilName;
            list[0].Cet = obj.Cet;
            list[0].D50 = obj.D50;
            list[0].Pol = obj.Pol;
            list[0].Den = obj.Den;
            list[0].Price = obj.Price;
            context.Compoilconfigs.Update(list[0]);
            var list1 = _CompOilConfig.GetAllCompOilConfigList().ToList();
            //context.SaveChanges();
            //context.Compoilconfigs.Update(list[0]);
            context.SaveChanges();
            return new ApiModel()
            {
            code = 200,
            //data = JsonConvert.SerializeObject(list),
            data = list1,
            msg = "查询成功"
            };
        }else{
            //List<Compoilconfig> list3 = new List<Compoilconfig>();
            var list = _CompOilConfig.GetAllCompOilConfigList().ToList();//需要把IEnumberable中遍历成List
            int index = list.Count;
            Compoilconfig comp = new Compoilconfig();
            context.Compoilconfigs.Add(comp);
            context.SaveChanges();
            var list1 = context.Compoilconfigs.ToList();
            list1[index].ComOilName = obj.ComOilName;
            list1[index].Cet = obj.Cet;
            list1[index].D50 = obj.D50;
            list1[index].Pol = obj.Pol;
            list1[index].Den = obj.Den;
            list1[index].Price = obj.Price;
            context.Compoilconfigs.Update(list1[index]);
            context.SaveChanges();
            var list2 = _CompOilConfig.GetAllCompOilConfigList().ToList();
            //context.SaveChanges();
            //context.Compoilconfigs.Update(list[0]);
            return new ApiModel()
            {
            code = 200,
            //data = JsonConvert.SerializeObject(list),
            data = list2,
            msg = "查询成功"
            };
        }

    }

    [HttpDelete]
    //[FromBody]Compoilconfig obj
    //public ApiModel Put(float cet, int index)//model里的名字 多个数据用IEnumberable，单个数据不用
    //public ApiModel Post(Compoilconfig obj)
    public ApiModel Delete(IndexNumber obj)//IndexNumber obj
    {
        //oilblendContext context = new();
        context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
        ICompOilConfig _CompOilConfig = new CompOilConfig(context);
        //List<Compoilconfig> list3 = new List<Compoilconfig>();
        var list = _CompOilConfig.GetAllCompOilConfigList().ToList();//需要把IEnumberable中遍历成List
        context.Compoilconfigs.Remove(list[obj.Index]);
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
