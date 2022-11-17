using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OilBlendSystem.Models.DataBaseModel;
using OilBlendSystem.Models.ConstructModel;
using OilSystem.ReturnClass;
using OilBlendSystem.BLL.Implementation;
using OilBlendSystem.BLL.Interface;
using Newtonsoft.Json;
namespace OilSystem.Controllers;
//using System.Data.Entity;
[ApiController]
[Route("[controller]")]
public class ProdOilConfigController : ControllerBase
{

    private readonly oilblendContext context;

    public ProdOilConfigController(oilblendContext _context)
    {
       context = _context;
    }

    [HttpGet]
    //public IEnumerable<TestTable> Get()//model里的名字
    public ApiModel Get()//model里的名字 多个数据用IEnumberable，单个数据不用
    {
        // return Enumerable.Range(1, 5).Select(index => new WeatherForecast
        // {
        //     Date = DateTime.Now.AddDays(index),
        //     TemperatureC = Random.Shared.Next(-20, 55),
        //     Summary = Summaries[Random.Shared.Next(Summaries.Length)]
        // })
        // .ToArray();
        //using oilblendContext context = new();
        var list = context.Prodoilconfigs.ToList();
        return new ApiModel()
        {
        code = 200,
        //data = JsonConvert.SerializeObject(list),
        data = list,
        msg = "查询成功"
        };
        //using oilblendContext context = new();//context上下文紧接着的是model中的名字（数据库名字+s）
        //retun context.Menulists.Where(m=>m.Id == 1).ToList();//自动表名后加s
        //context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;//用到了microsoft.entity包
        //var menulist = context.Menulists.ToList();//firstordefult 如果没有值，则返回默认值
        //return context.Compoilconfigs.ToList();
        //asnotracking关闭跟踪
        //return context.TestTables.Single(m=>m.Id == 1);//只有一个值用single
        //index.Index = number;
        //context.Menulists.Update(menulist);//把赋予的对象放到update里,这句其实可以不写
        // context.TestTables.Add(new TestTable{
        //     PartName = "Leo",
        //     Point = 10,
        // });
        //throw new Exception();
        //context.SaveChanges();//跟踪原理,不需要写update，如果取消更新则需要加上update
        //查询的时候用asnotracking，可以降低内存消耗，减少不必要的跟踪，二分写八分读

       //return menulist;

    }


    [HttpPut]
    public ApiModel Put(Prodproperty obj)
    {
        //oilblendContext context = new();
        context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
        IProdOilConfig _ProdOilConfig = new ProdOilConfig(context);
        //List<Compoilconfig> list3 = new List<Compoilconfig>();
        var list = _ProdOilConfig.GetAllProdOilConfigList().ToList();//需要把IEnumberable中遍历成List
        //var list = _CompOilConfig.GetAllCompOilConfigList();
        //var list = context.Compoilconfigs.ToList();
        //for(int i = 0; i < list.Count; i++)
        //foreach (var item in list)
        //{
            list[obj.Index].ProdOilName = obj.ProdOilName;
            list[obj.Index].CetHighLimit = obj.CetHighLimit;
            list[obj.Index].CetLowLimit = obj.CetLowLimit;
            list[obj.Index].D50HighLimit = obj.D50HighLimit;
            list[obj.Index].D50LowLimit = obj.D50LowLimit;
            list[obj.Index].PolHighLimit = obj.PolHighLimit;
            list[obj.Index].PolLowLimit = obj.PolLowLimit;
            list[obj.Index].DenHighLimit = obj.DenHighLimit;
            list[obj.Index].DenLowLimit = obj.DenLowLimit;
            context.Prodoilconfigs.Update(list[obj.Index]);
            context.SaveChanges();
        //}
        //list[0].Cet = obj.Cet;
        //obj.Cet = 16;
        //list[0].Cet = cet;
        //list[index].Cet = cet;

        return new ApiModel()
        {
        code = 200,
        //data = JsonConvert.SerializeObject(list),
        data = list,
        msg = "查询成功"
        };
    }


}
