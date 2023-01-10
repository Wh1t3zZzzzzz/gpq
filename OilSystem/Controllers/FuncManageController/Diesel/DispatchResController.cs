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
public class DispatchResController : ControllerBase
{
    //智能决策模块设置控制器

    private readonly oilblendContext context;

    public DispatchResController(oilblendContext _context)
    {
       context = _context;
    }

    [HttpGet("Res/Obj")]
    public ApiModel GetObj()//model里的名字 多个数据用IEnumberable，单个数据不用
    {       
        IDispatch _Dispatch = new Dispatch(context);
        // var list = context.Properties.ToList();
        var list = _Dispatch.GetDispatch_decsCalc().ToList();
        if(list[0].objValue != 0){
            return new ApiModel(){
                code = 200,
                //data = JsonConvert.SerializeObject(list),
                data = list,
                msg = "计算成功"
            };
        }else{
            return new ApiModel(){
                code = 407,
                data = list,
                msg = "计算失败"
            };
        }
    }

    [HttpGet("Res/ComFlowInfo_divProd")]
    //ComFlowInfo_divProd1 + ComFlowInfo_divProd2 + ComFlowInfo_divProd3 + ComFlowInfo_divProd4
    public ApiModel GetProd()//model里的名字 多个数据用IEnumberable，单个数据不用
    {       
        IDispatch _Dispatch = new Dispatch(context);
        // var list = context.Properties.ToList();
        var list1 = _Dispatch.GetDispatch_decsScheme_comFlowInfo_divProd1().ToList();
        var list2 = _Dispatch.GetDispatch_decsScheme_comFlowInfo_divProd2().ToList();
        var list3 = _Dispatch.GetDispatch_decsScheme_comFlowInfo_divProd3().ToList();
        var list4 = _Dispatch.GetDispatch_decsScheme_comFlowInfo_divProd4().ToList();
        List<Dispatch_decsScheme_comFlowInfo_divProd> ResultList = new List<Dispatch_decsScheme_comFlowInfo_divProd>();//列表，里面可以添加很多个对象  
        for(int i = 0; i < list1.Count; i++){//组分油个数
            Dispatch_decsScheme_comFlowInfo_divProd result = new Dispatch_decsScheme_comFlowInfo_divProd();//实体，可以理解为一个对象  
            result.ComOilName = list1[i].ComOilName;//随便取一个列表的组分油名称，因为肯定是一致的
            result.comFlowT1 = list1[i].comFlowT1 + list2[i].comFlowT1 + list3[i].comFlowT1 + list4[i].comFlowT1;
            result.comFlowT2 = list1[i].comFlowT2 + list2[i].comFlowT2 + list3[i].comFlowT2 + list4[i].comFlowT2;
            result.comFlowT3 = list1[i].comFlowT3 + list2[i].comFlowT3 + list3[i].comFlowT3 + list4[i].comFlowT3;
            result.comFlowT4 = list1[i].comFlowT4 + list2[i].comFlowT4 + list3[i].comFlowT4 + list4[i].comFlowT4;
            result.comFlowT5 = list1[i].comFlowT5 + list2[i].comFlowT5 + list3[i].comFlowT5 + list4[i].comFlowT5;
            result.comFlowT6 = list1[i].comFlowT6 + list2[i].comFlowT6 + list3[i].comFlowT6 + list4[i].comFlowT6;
            result.comFlowT7 = list1[i].comFlowT7 + list2[i].comFlowT7 + list3[i].comFlowT7 + list4[i].comFlowT7;
            ResultList.Add(result); 
        }   
        return new ApiModel(){
            code = 200,
            //data = JsonConvert.SerializeObject(list),
            data = ResultList,
            msg = "查询成功"
        };
    }

    [HttpGet("Res/ComFlowInfo_divProd1")]
    public ApiModel GetProd1()//model里的名字 多个数据用IEnumberable，单个数据不用
    {       
        IDispatch _Dispatch = new Dispatch(context);
        // var list = context.Properties.ToList();
        var list = _Dispatch.GetDispatch_decsScheme_comFlowInfo_divProd1().ToList();
        return new ApiModel()
        {
        code = 200,
        //data = JsonConvert.SerializeObject(list),
        data = list,
        msg = "查询成功"
        };
    }

    [HttpGet("Res/ComFlowInfo_divProd2")]
    public ApiModel GetProd2()//model里的名字 多个数据用IEnumberable，单个数据不用
    {       
        IDispatch _Dispatch = new Dispatch(context);
        // var list = context.Properties.ToList();
        var list = _Dispatch.GetDispatch_decsScheme_comFlowInfo_divProd2().ToList();
        return new ApiModel()
        {
        code = 200,
        //data = JsonConvert.SerializeObject(list),
        data = list,
        msg = "查询成功"
        };

    }

    [HttpGet("Res/ComFlowInfo_divProd3")]
    public ApiModel GetProd3()//model里的名字 多个数据用IEnumberable，单个数据不用
    {       
        IDispatch _Dispatch = new Dispatch(context);
        // var list = context.Properties.ToList();
        var list = _Dispatch.GetDispatch_decsScheme_comFlowInfo_divProd3().ToList();
        return new ApiModel()
        {
        code = 200,
        //data = JsonConvert.SerializeObject(list),
        data = list,
        msg = "查询成功"
        };

    }

    [HttpGet("Res/ComFlowInfo_divProd4")]
    public ApiModel GetProd4()//model里的名字 多个数据用IEnumberable，单个数据不用
    {       
        IDispatch _Dispatch = new Dispatch(context);
        // var list = context.Properties.ToList();
        var list = _Dispatch.GetDispatch_decsScheme_comFlowInfo_divProd4().ToList();
        return new ApiModel()
        {
        code = 200,
        //data = JsonConvert.SerializeObject(list),
        data = list,
        msg = "查询成功"
        };

    }

    [HttpGet("Res/ComFlowInfo_divT1")]
    public ApiModel GetT1()//model里的名字 多个数据用IEnumberable，单个数据不用
    {       
        IDispatch _Dispatch = new Dispatch(context);
        // var list = context.Properties.ToList();
        var list = _Dispatch.GetDispatch_decsScheme_comFlowInfo_divT1().ToList();
        return new ApiModel()
        {
        code = 200,
        //data = JsonConvert.SerializeObject(list),
        data = list,
        msg = "查询成功"
        };

    }

    [HttpGet("Res/ComFlowInfo_divT2")]
    public ApiModel GetT2()//model里的名字 多个数据用IEnumberable，单个数据不用
    {       
        IDispatch _Dispatch = new Dispatch(context);
        // var list = context.Properties.ToList();
        var list = _Dispatch.GetDispatch_decsScheme_comFlowInfo_divT2().ToList();
        return new ApiModel()
        {
        code = 200,
        //data = JsonConvert.SerializeObject(list),
        data = list,
        msg = "查询成功"
        };

    }

    [HttpGet("Res/ComFlowInfo_divT3")]
    public ApiModel GetT3()//model里的名字 多个数据用IEnumberable，单个数据不用
    {       
        IDispatch _Dispatch = new Dispatch(context);
        // var list = context.Properties.ToList();
        var list = _Dispatch.GetDispatch_decsScheme_comFlowInfo_divT3().ToList();
        return new ApiModel()
        {
        code = 200,
        //data = JsonConvert.SerializeObject(list),
        data = list,
        msg = "查询成功"
        };

    }

    [HttpGet("Res/ComFlowInfo_divT4")]
    public ApiModel GetT4()//model里的名字 多个数据用IEnumberable，单个数据不用
    {       
        IDispatch _Dispatch = new Dispatch(context);
        // var list = context.Properties.ToList();
        var list = _Dispatch.GetDispatch_decsScheme_comFlowInfo_divT4().ToList();
        return new ApiModel()
        {
        code = 200,
        //data = JsonConvert.SerializeObject(list),
        data = list,
        msg = "查询成功"
        };

    }

    [HttpGet("Res/ComFlowInfo_divT5")]
    public ApiModel GetT5()//model里的名字 多个数据用IEnumberable，单个数据不用
    {       
        IDispatch _Dispatch = new Dispatch(context);
        // var list = context.Properties.ToList();
        var list = _Dispatch.GetDispatch_decsScheme_comFlowInfo_divT5().ToList();
        return new ApiModel()
        {
        code = 200,
        //data = JsonConvert.SerializeObject(list),
        data = list,
        msg = "查询成功"
        };

    }

    [HttpGet("Res/ComFlowInfo_divT6")]
    public ApiModel GetT6()//model里的名字 多个数据用IEnumberable，单个数据不用
    {       
        IDispatch _Dispatch = new Dispatch(context);
        // var list = context.Properties.ToList();
        var list = _Dispatch.GetDispatch_decsScheme_comFlowInfo_divT6().ToList();
        return new ApiModel()
        {
        code = 200,
        //data = JsonConvert.SerializeObject(list),
        data = list,
        msg = "查询成功"
        };

    }
    
    [HttpGet("Res/ComFlowInfo_divT7")]
    public ApiModel GetT7()//model里的名字 多个数据用IEnumberable，单个数据不用
    {       
        IDispatch _Dispatch = new Dispatch(context);
        // var list = context.Properties.ToList();
        var list = _Dispatch.GetDispatch_decsScheme_comFlowInfo_divT7().ToList();
        return new ApiModel()
        {
        code = 200,
        //data = JsonConvert.SerializeObject(list),
        data = list,
        msg = "查询成功"
        };

    }

    [HttpGet("Res/ComInv")]
    public ApiModel GetComInv()//model里的名字 多个数据用IEnumberable，单个数据不用
    {       
        IDispatch _Dispatch = new Dispatch(context);
        // var list = context.Properties.ToList();
        var list = _Dispatch.GetDispatch_decsScheme_invInfo_comOil().ToList();
        return new ApiModel()
        {
        code = 200,
        //data = JsonConvert.SerializeObject(list),
        data = list,
        msg = "查询成功"
        };

    }

    [HttpGet("Res/ProdInv")]
    public ApiModel GetProdInv()//model里的名字 多个数据用IEnumberable，单个数据不用
    {       
        IDispatch _Dispatch = new Dispatch(context);
        // var list = context.Properties.ToList();
        var list = _Dispatch.GetDispatch_decsScheme_invInfo_prodOil().ToList();
        return new ApiModel()
        {
        code = 200,
        //data = JsonConvert.SerializeObject(list),
        data = list,
        msg = "查询成功"
        };

    }

    [HttpGet("Res/Prod1Info")]
    public ApiModel GetProd1Info()//model里的名字 多个数据用IEnumberable，单个数据不用
    {       
        IDispatch _Dispatch = new Dispatch(context);
        // var list = context.Properties.ToList();
        var list = _Dispatch.GetDispatch_decsScheme_prod1Info().ToList();
        return new ApiModel()
        {
        code = 200,
        //data = JsonConvert.SerializeObject(list),
        data = list,
        msg = "查询成功"
        };

    }

    [HttpGet("Res/Prod2Info")]
    public ApiModel GetProd2Info()//model里的名字 多个数据用IEnumberable，单个数据不用
    {       
        IDispatch _Dispatch = new Dispatch(context);
        // var list = context.Properties.ToList();
        var list = _Dispatch.GetDispatch_decsScheme_prod2Info().ToList();
        return new ApiModel()
        {
        code = 200,
        //data = JsonConvert.SerializeObject(list),
        data = list,
        msg = "查询成功"
        };

    }

    [HttpGet("Res/Prod3Info")]
    public ApiModel GetProd3Info()//model里的名字 多个数据用IEnumberable，单个数据不用
    {       
        IDispatch _Dispatch = new Dispatch(context);
        // var list = context.Properties.ToList();
        var list = _Dispatch.GetDispatch_decsScheme_prod3Info().ToList();
        return new ApiModel()
        {
        code = 200,
        //data = JsonConvert.SerializeObject(list),
        data = list,
        msg = "查询成功"
        };

    }
    [HttpGet("Res/Prod4Info")]
    public ApiModel GetProd4Info()//model里的名字 多个数据用IEnumberable，单个数据不用
    {       
        IDispatch _Dispatch = new Dispatch(context);
        // var list = context.Properties.ToList();
        var list = _Dispatch.GetDispatch_decsScheme_prod4Info().ToList();
        return new ApiModel()
        {
        code = 200,
        //data = JsonConvert.SerializeObject(list),
        data = list,
        msg = "查询成功"
        };

    }

}
