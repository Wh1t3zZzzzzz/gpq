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
public class MenuListController : ControllerBase
{
    private readonly oilblendContext context;

    public MenuListController(oilblendContext _context)
    {
       context = _context;
    }

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
