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
public class MenuListController : ControllerBase
{
    private readonly oilblendContext context;

    public MenuListController(oilblendContext _context)
    {
       context = _context;
    }
    //private readonly IProdOilConfig _prod;

    // private readonly ILogger<SchemeVerify_2Controller> _logger;

    // public SchemeVerify_2Controller(ILogger<SchemeVerify_2Controller> logger)
    // {
    //     _logger = logger;
    // }

    [HttpGet]
    //ID ParentID Name   List<TreeChildren>

    public ApiModel Get()//model里的名字 多个数据用IEnumberable，单个数据不用
    {

        //using oilblendContext context = new();
        IMenuList _MenuList = new MenuList(context);
        var list = _MenuList.GetTreeViewMenuList();
        // var list = context.Menulists.ToList();
        return new ApiModel()
        {
        code = 200,
        //data = JsonConvert.SerializeObject(list),
        data = list,
        msg = "查询成功"
        };

    }


}
