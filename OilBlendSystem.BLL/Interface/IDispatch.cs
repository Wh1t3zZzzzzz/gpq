using OilBlendSystem.Models.DataBaseModel;
using OilBlendSystem.Models.ConstructModel;

namespace OilBlendSystem.BLL.Interface
{
    public interface IDispatch
    {
        void RunPythonScript();
        void RunIOFromtxt();
        IEnumerable<Dispatch_decsCalc> GetDispatch_decsCalc();//求解结果
        IEnumerable<Dispatch_decsScheme_comFlowInfo_divProd> GetDispatch_decsScheme_comFlowInfo_divProd1();//第一个成品油的组分油参调流量（七天）
        IEnumerable<Dispatch_decsScheme_comFlowInfo_divProd> GetDispatch_decsScheme_comFlowInfo_divProd2();//第二个成品油的组分油参调流量
        IEnumerable<Dispatch_decsScheme_comFlowInfo_divProd> GetDispatch_decsScheme_comFlowInfo_divProd3();//第三个成品油的组分油参调流量
        IEnumerable<Dispatch_decsScheme_comFlowInfo_divProd> GetDispatch_decsScheme_comFlowInfo_divProd4();//第四个成品油的组分油参调流量
        IEnumerable<Dispatch_decsScheme_comFlowInfo_divT> GetDispatch_decsScheme_comFlowInfo_divT1();//第一天的组分油参调流量（四个成品油）
        IEnumerable<Dispatch_decsScheme_comFlowInfo_divT> GetDispatch_decsScheme_comFlowInfo_divT2();//第二天的组分油参调流量（四个成品油）
        IEnumerable<Dispatch_decsScheme_comFlowInfo_divT> GetDispatch_decsScheme_comFlowInfo_divT3();//第三天的组分油参调流量（四个成品油）
        IEnumerable<Dispatch_decsScheme_comFlowInfo_divT> GetDispatch_decsScheme_comFlowInfo_divT4();//第四天的组分油参调流量（四个成品油）
        IEnumerable<Dispatch_decsScheme_comFlowInfo_divT> GetDispatch_decsScheme_comFlowInfo_divT5();//第五天的组分油参调流量（四个成品油）
        IEnumerable<Dispatch_decsScheme_comFlowInfo_divT> GetDispatch_decsScheme_comFlowInfo_divT6();//第六天的组分油参调流量（四个成品油）
        IEnumerable<Dispatch_decsScheme_comFlowInfo_divT> GetDispatch_decsScheme_comFlowInfo_divT7();//第七天的组分油参调流量（四个成品油）
        IEnumerable<Dispatch_decsScheme_invInfo_comOil> GetDispatch_decsScheme_invInfo_comOil();//组分油库存信息
        IEnumerable<Dispatch_decsScheme_invInfo_prodOil> GetDispatch_decsScheme_invInfo_prodOil();//成品油库存信息
        IEnumerable<Dispatch_decsScheme_prodInfo> GetDispatch_decsScheme_prod1Info();//第一个成品油的属性信息
        IEnumerable<Dispatch_decsScheme_prodInfo> GetDispatch_decsScheme_prod2Info();//第二个成品油的属性信息
        IEnumerable<Dispatch_decsScheme_prodInfo> GetDispatch_decsScheme_prod3Info();//第三个成品油的属性信息
        IEnumerable<Dispatch_decsScheme_prodInfo> GetDispatch_decsScheme_prod4Info();//第四个成品油的属性信息

        
    }
}