using OilBlendSystem.Models.Gas.DataBaseModel;
using OilBlendSystem.Models.Gas.ConstructModel;
using OilBlendSystem.Models;
using Microsoft.EntityFrameworkCore;
using OilBlendSystem.BLL.Interface.Gas;

namespace OilBlendSystem.BLL.Implementation.Gas
{
    public partial class RecipeCalc : IRecipeCalc
    {        
        private readonly oilblendContext context;//带问号是可以为空

        public RecipeCalc(oilblendContext _context)
        {
           //oilblendContext _context = new();
           context = _context;
        }

        public IEnumerable<Recipecalc1_gas> GetRecipeCalc1()
        {
            context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
            return context.Recipecalc1_gases.ToList();
        }
        public IEnumerable<Recipecalc2_gas> GetRecipeCalc2()
        {//配方优化第一个场景的权值设置表格
            context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
            return context.Recipecalc2_gases.ToList();
        }
        public IEnumerable<Recipecalc2_2_gas> GetRecipeCalc2_2()
        {//配方优化第二个场景的权值设置表格
            context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
            return context.Recipecalc2_2_gases.ToList();
        }
        public IEnumerable<Recipecalc2_3_gas> GetRecipeCalc2_3()
        {//配方优化第三个场景的权值设置表格
            context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
            return context.Recipecalc2_3_gases.ToList();
        }
        public IEnumerable<Recipecalc3_gas> GetRecipeCalc3()
        {
            context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
            return context.Recipecalc3_gases.ToList();
        }
        public double[] GetRecipe1()//场景1的lpsolve求解配方
        {
            IProdOilConfig _ProdOilConfig = new ProdOilConfig(context);
            IProperty _Property = new PropertyApply(context);
            IRecipeCalc _RecipeCalc = new RecipeCalc(context);
            ICompOilConfig _CompOilConfig = new CompOilConfig(context);
            
            //context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;

            var CompOilList = _CompOilConfig.GetAllCompOilConfigList().ToList();
            var ProdOilList = _ProdOilConfig.GetAllProdOilConfigList().Where(m=>m.Apply == 1).ToList();
            var PropertyList = _Property.GetAllPropertyList().ToList();
            var CompOilConstraint = _RecipeCalc.GetRecipeCalc1().ToList();//组分油产量 罐容高低限 配方高低限 约束
            var Weight = _RecipeCalc.GetRecipeCalc2().Where(m=>m.Apply == 1).ToList();//多目权值,三个场景的
            var ProdOilProduct = _RecipeCalc.GetRecipeCalc3().Where(m=>m.Apply == 1).ToList();//成品油产量限制

            int ComOilNum = CompOilList.Count;//组分油个数
            int ProdOilNum = ProdOilProduct.Count;//成品油个数

            //变量约束定义
            //变量个数应为 成品油启用个数 x 组分油个数 + 组分油个数（组分油建议产量）
            //int iNum = 24;//2X8是组分油1（车柴 出柴）组分油2（车柴 出柴） +8 （组分油产量）
            int iNum_Uncontain = ProdOilNum * ComOilNum;
            int iNum = ProdOilNum * ComOilNum + ComOilNum;
            double[] Calc = new double[iNum + 1];//加的这个1是求解标志位

            #region 目标函数定义

            // 目标函数定义
            float[] f = new float[iNum];
            for(int i = 0; i < ComOilNum; i++){//成品油的目标函数(产量最大 + 十六烷值 + 多芳烃)
                for(int j = 0; j < ProdOilNum; j++){
                    f[ProdOilNum * i + j] = -Weight[j].Weight * CompOilList[i].den / 1000 * 10000 + Weight[ProdOilNum + j].Weight * (CompOilList[i].ron - ProdOilList[j].ronLowLimit) * 1000 + Weight[ProdOilNum * 2 + j].Weight * (CompOilList[i].suf - ProdOilList[j].sufLowLimit) * 1000 ; 
                }
            }
            // f[0] = -Weight[0].Weight * CompOilList[0].den / 1000 * 10000 ;//质量产量乘以10000，质量指标乘以1000
            // f[1] = -Weight[3].Weight * CompOilList[0].den / 1000 * 10000 + Weight[1].Weight * (CompOilList[0].ron - ProdOilList[1].ronLowLimit) * 1000 - Weight[2].Weight * (CompOilList[0].suf - ProdOilList[1].sufHighLimit) * 1000 ;
            // //-CompOilList[0].den / 1000 * 10000;  一个向上卡边一个向下卡边
            // f[2] = -Weight[0].Weight * CompOilList[1].den / 1000 * 10000 ;
            // f[3] = -Weight[3].Weight * CompOilList[1].den / 1000 * 10000 + Weight[1].Weight * (CompOilList[1].ron - ProdOilList[1].ronLowLimit) * 1000 - Weight[2].Weight * (CompOilList[1].suf - ProdOilList[1].sufHighLimit) * 1000 ;
            // //-CompOilList[1].den / 1000 * 10000;
            // f[4] = -Weight[0].Weight * CompOilList[2].den / 1000 * 10000 ;
            // f[5] = -Weight[3].Weight * CompOilList[2].den / 1000 * 10000 + Weight[1].Weight * (CompOilList[2].ron - ProdOilList[1].ronLowLimit) * 1000 - Weight[2].Weight * (CompOilList[2].suf - ProdOilList[1].sufHighLimit) * 1000 ;
            // //-CompOilList[2].den / 1000 * 10000;
            // f[6] = -Weight[0].Weight * CompOilList[3].den / 1000 * 10000 ;
            // f[7] = -Weight[3].Weight * CompOilList[3].den / 1000 * 10000 + Weight[1].Weight * (CompOilList[3].ron - ProdOilList[1].ronLowLimit) * 1000 - Weight[2].Weight * (CompOilList[3].suf - ProdOilList[1].sufHighLimit) * 1000 ;
            // //-CompOilList[3].den / 1000 * 10000;
            // f[8] = -Weight[0].Weight * CompOilList[4].den / 1000 * 10000 ;
            // f[9] = -Weight[3].Weight * CompOilList[4].den / 1000 * 10000 + Weight[1].Weight * (CompOilList[4].ron - ProdOilList[1].ronLowLimit) * 1000 - Weight[2].Weight * (CompOilList[4].suf - ProdOilList[1].sufHighLimit) * 1000 ;

            // f[10] = -Weight[0].Weight * CompOilList[5].den / 1000 * 10000 ;
            // f[11] = -Weight[3].Weight * CompOilList[5].den / 1000 * 10000 + Weight[1].Weight * (CompOilList[5].ron - ProdOilList[1].ronLowLimit) * 1000 - Weight[2].Weight * (CompOilList[5].suf - ProdOilList[1].sufHighLimit) * 1000 ;

            // f[12] = -Weight[0].Weight * CompOilList[6].den / 1000 * 10000 ;
            // f[13] = -Weight[3].Weight * CompOilList[6].den / 1000 * 10000 + Weight[1].Weight * (CompOilList[6].ron - ProdOilList[1].ronLowLimit) * 1000 - Weight[2].Weight * (CompOilList[6].suf - ProdOilList[1].sufHighLimit) * 1000 ;

            // f[14] = -Weight[0].Weight * CompOilList[7].den / 1000 * 10000 ;
            // f[15] = -Weight[3].Weight * CompOilList[7].den / 1000 * 10000 + Weight[1].Weight * (CompOilList[7].ron - ProdOilList[1].ronLowLimit) * 1000 - Weight[2].Weight * (CompOilList[7].suf - ProdOilList[1].sufHighLimit) * 1000 ;
            
            for(int i = 0; i < ComOilNum; i++){
                f[i + ProdOilNum * ComOilNum] = CompOilList[i].Price;//每吨的价格
            }

            #endregion

            //组分油产量上下限

            Double[] LB = new Double[iNum];// 变量低限
            Double[] UB = new Double[iNum];// 高限

            for(int i = iNum_Uncontain; i < iNum; i++){
                LB[i] = CompOilConstraint[(i - iNum_Uncontain)].ComOilProductLow;/// CompOilList[(i - iNum_Uncontain)].den * 1000
                UB[i] = CompOilConstraint[(i - iNum_Uncontain)].ComOilProductHigh;
            }

            #region 流量上下限          
            // LB[0] = CompOilList[0].gas92Low1 / CompOilList[0].den * 1000;
            // LB[1] = CompOilList[0].gas95Low1 / CompOilList[0].den * 1000;

            // LB[2] = CompOilList[1].gas92Low1 / CompOilList[1].den * 1000;
            // LB[3] = CompOilList[1].gas95Low1 / CompOilList[1].den * 1000;

            // LB[4] = CompOilList[2].gas92Low1 / CompOilList[2].den * 1000;
            // LB[5] = CompOilList[2].gas95Low1 / CompOilList[2].den * 1000;

            // LB[6] = CompOilList[3].gas92Low1 / CompOilList[3].den * 1000;
            // LB[7] = CompOilList[3].gas95Low1 / CompOilList[3].den * 1000;

            // LB[8] = CompOilList[4].gas92Low1 / CompOilList[4].den * 1000;
            // LB[9] = CompOilList[4].gas95Low1 / CompOilList[4].den * 1000;

            // LB[10] = CompOilList[5].gas92Low1 / CompOilList[5].den * 1000;
            // LB[11] = CompOilList[5].gas95Low1 / CompOilList[5].den * 1000;

            // LB[12] = CompOilList[6].gas92Low1 / CompOilList[6].den * 1000;
            // LB[13] = CompOilList[6].gas95Low1 / CompOilList[6].den * 1000;

            // LB[14] = CompOilList[7].gas92Low1 / CompOilList[7].den * 1000;
            // LB[15] = CompOilList[7].gas95Low1 / CompOilList[7].den * 1000;

            // UB[0] = CompOilList[0].gas92High1 / CompOilList[0].den * 1000; 
            // UB[1] = CompOilList[0].gas95High1 / CompOilList[0].den * 1000;

            // UB[2] = CompOilList[1].gas92High1 / CompOilList[1].den * 1000;
            // UB[3] = CompOilList[1].gas95High1 / CompOilList[1].den * 1000;

            // UB[4] = CompOilList[2].gas92High1 / CompOilList[2].den * 1000;
            // UB[5] = CompOilList[2].gas95High1 / CompOilList[2].den * 1000;

            // UB[6] = CompOilList[3].gas92High1 / CompOilList[3].den * 1000;
            // UB[7] = CompOilList[3].gas95High1 / CompOilList[3].den * 1000;

            // UB[8] = CompOilList[4].gas92High1 / CompOilList[4].den * 1000;
            // UB[9] = CompOilList[4].gas95High1 / CompOilList[4].den * 1000;

            // UB[10] = CompOilList[5].gas92High1 / CompOilList[5].den * 1000;
            // UB[11] = CompOilList[5].gas95High1 / CompOilList[5].den * 1000;

            // UB[12] = CompOilList[6].gas92High1 / CompOilList[6].den * 1000;
            // UB[13] = CompOilList[6].gas95High1 / CompOilList[6].den * 1000;

            // UB[14] = CompOilList[7].gas92High1 / CompOilList[7].den * 1000;
            // UB[15] = CompOilList[7].gas95High1 / CompOilList[7].den * 1000;
            
            #endregion

            #region 成品油不等式约束
            // 成品油产量不等式约束

            Double[,] Prod_le = new Double[ProdOilNum, iNum_Uncontain];//2行16列矩阵 带变量和系数的计算式
            Double[] Prod_rle = new Double[ProdOilNum];

            for(int i = 0; i < ProdOilNum; i++){
                Prod_rle[i] = ProdOilProduct[i].ProdOilProduct;//成品油产量限制 
            }

            for(int i = 0; i < ProdOilNum; i++){
                for(int j = 0; j < ComOilNum; j++){
                    Prod_le[i, ProdOilNum * j + i] = CompOilList[j].den / 1000;//组分油密度，t/m³ 成品油产量限制
                }
            }

            // P1产量不等式约束 车用柴油

            // Prod_le[0, 2] = CompOilList[1].den / 1000;
            // Prod_le[0, 4] = CompOilList[2].den / 1000;
            // Prod_le[0, 6] = CompOilList[3].den / 1000;
            // Prod_le[0, 8] = CompOilList[4].den / 1000;
            // Prod_le[0, 10] = CompOilList[5].den / 1000;
            // Prod_le[0, 12] = CompOilList[6].den / 1000;
            // Prod_le[0, 14] = CompOilList[7].den / 1000;

            // // P2产量不等式约束 出口柴油
            // Prod_le[1, 1] = CompOilList[0].den / 1000;
            // Prod_le[1, 3] = CompOilList[1].den / 1000;
            // Prod_le[1, 5] = CompOilList[2].den / 1000;
            // Prod_le[1, 7] = CompOilList[3].den / 1000;
            // Prod_le[1, 9] = CompOilList[4].den / 1000;
            // Prod_le[1, 11] = CompOilList[5].den / 1000;
            // Prod_le[1, 13] = CompOilList[6].den / 1000;
            // Prod_le[1, 15] = CompOilList[7].den / 1000;

            #endregion

            #region 组分油不等式约束

            // 组分油不等式约束 有罐子

            Double[,] Comp_le_up = new Double[ComOilNum, iNum];
            Double[,] Comp_le_low = new Double[ComOilNum, iNum];
            Double[] Comp_rle_low = new Double[ComOilNum];// 低限
            Double[] Comp_rle_up = new Double[ComOilNum];// 高限

            for( int i = 0; i < ComOilNum; i++)
            {
                Comp_rle_low[i] = CompOilConstraint[i].VolumeLow - CompOilConstraint[i].IniVolume;           
                Comp_rle_up[i] = CompOilConstraint[i].VolumeHigh - CompOilConstraint[i].IniVolume;
            }
            //下限约束
            for(int i = 0; i < ComOilNum; i++){
                for(int j = 0; j < ProdOilNum; j++){
                    Comp_le_low[i, ProdOilNum * i + j] = - CompOilList[i].den / 1000;
                }
                Comp_le_low[i, iNum_Uncontain + i] = 1;
            }
            //上限约束
            for(int i = 0; i < ComOilNum; i++){
                Comp_le_up[i, iNum_Uncontain + i] = 1;
            }           

            // // F2产量等式约束 第二个组分油
            // Comp_le[1, 2] = CompOilList[1].den / 1000;第一个组分油的车柴变量
            // Comp_le[1, 3] = CompOilList[1].den / 1000;第一个组分油的出柴变量
            // Comp_le[1, 17] = CompOilList[1].den / 1000; 第一个组分油产量变量

            // // F3产量等式约束
            // Comp_le[2, 4] = CompOilList[2].den / 1000;
            // Comp_le[2, 5] = CompOilList[2].den / 1000;
            // Comp_le[2, 18] = CompOilList[2].den / 1000;

            // // F4产量等式约束
            // Comp_le[3, 6] = CompOilList[3].den / 1000;
            // Comp_le[3, 7] = CompOilList[3].den / 1000;
            // Comp_le[3, 19] = CompOilList[3].den / 1000;

            // // F5产量等式约束
            // Comp_le[4, 8] = CompOilList[4].den / 1000;
            // Comp_le[4, 9] = CompOilList[4].den / 1000;
            // Comp_le[4, 20] = CompOilList[4].den / 1000;

            // // F6产量等式约束
            // Comp_le[5, 10] = CompOilList[5].den / 1000;
            // Comp_le[5, 11] = CompOilList[5].den / 1000;
            // Comp_le[5, 21] = CompOilList[5].den / 1000;

            // // F7产量等式约束
            // Comp_le[6, 12] = CompOilList[6].den / 1000;
            // Comp_le[6, 13] = CompOilList[6].den / 1000;
            // Comp_le[6, 22] = CompOilList[6].den / 1000;

            // // F8产量等式约束 第八个组分油
            // Comp_le[7, 14] = CompOilList[7].den / 1000;
            // Comp_le[7, 15] = CompOilList[7].den / 1000;
            // Comp_le[7, 23] = CompOilList[7].den / 1000;

            #endregion

            #region 属性高限约束

            // 属性不等式约束 八个组分油 8行16列
            Double[,] Ale = new Double[4 * ProdOilNum, iNum_Uncontain];//这个的行应该是成品油个数乘以属性个数
            Double[] ble = new Double[4 * ProdOilNum];

            #region 十六烷值指数高限

            for(int i = 0; i < ProdOilNum; i++){
                for(int j = 0; j < ComOilNum; j++){
                    Ale[i, ProdOilNum * j + i] = (CompOilList[j].ron - ProdOilList[i].ronHighLimit) * PropertyList[0].Apply;
                }
            }

            // 车柴十六烷值指数高限 八个组分油

            // Ale[0, 2] = (CompOilList[1].ron - ProdOilList[0].ronHighLimit) * PropertyList[0].Apply;
            // Ale[0, 4] = (CompOilList[2].ron - ProdOilList[0].ronHighLimit) * PropertyList[0].Apply;
            // Ale[0, 6] = (CompOilList[3].ron - ProdOilList[0].ronHighLimit) * PropertyList[0].Apply;
            // Ale[0, 8] = (CompOilList[4].ron - ProdOilList[0].ronHighLimit) * PropertyList[0].Apply;
            // Ale[0, 10] = (CompOilList[5].ron - ProdOilList[0].ronHighLimit) * PropertyList[0].Apply;
            // Ale[0, 12] = (CompOilList[6].ron - ProdOilList[0].ronHighLimit) * PropertyList[0].Apply;
            // Ale[0, 14] = (CompOilList[7].ron - ProdOilList[0].ronHighLimit) * PropertyList[0].Apply;


            // //出柴十六烷值指数高限 
            // Ale[1, 1] = (CompOilList[0].ron - ProdOilList[1].ronHighLimit) * PropertyList[0].Apply;
            // Ale[1, 3] = (CompOilList[1].ron - ProdOilList[1].ronHighLimit) * PropertyList[0].Apply;
            // Ale[1, 5] = (CompOilList[2].ron - ProdOilList[1].ronHighLimit) * PropertyList[0].Apply;
            // Ale[1, 7] = (CompOilList[3].ron - ProdOilList[1].ronHighLimit) * PropertyList[0].Apply;
            // Ale[1, 9] = (CompOilList[4].ron - ProdOilList[1].ronHighLimit) * PropertyList[0].Apply;
            // Ale[1, 11] = (CompOilList[5].ron - ProdOilList[1].ronHighLimit) * PropertyList[0].Apply;
            // Ale[1, 13] = (CompOilList[6].ron - ProdOilList[1].ronHighLimit) * PropertyList[0].Apply;
            // Ale[1, 15] = (CompOilList[7].ron - ProdOilList[1].ronHighLimit) * PropertyList[0].Apply;

            #endregion

            #region 馏程高限

            for(int i = 0; i < ProdOilNum; i++){
                for(int j = 0; j < ComOilNum; j++){
                    Ale[ProdOilNum + i, ProdOilNum * j + i] = (CompOilList[j].t50 - ProdOilList[i].t50HighLimit) * PropertyList[1].Apply;
                }
            }

            // Ale[2, 0] = (CompOilList[0].t50 - ProdOilList[0].t50HighLimit) * PropertyList[1].Apply;
            // Ale[2, 2] = (CompOilList[1].t50 - ProdOilList[0].t50HighLimit) * PropertyList[1].Apply;
            // Ale[2, 4] = (CompOilList[2].t50 - ProdOilList[0].t50HighLimit) * PropertyList[1].Apply;
            // Ale[2, 6] = (CompOilList[3].t50 - ProdOilList[0].t50HighLimit) * PropertyList[1].Apply;
            // Ale[2, 8] = (CompOilList[4].t50 - ProdOilList[0].t50HighLimit) * PropertyList[1].Apply;
            // Ale[2, 10] = (CompOilList[5].t50 - ProdOilList[0].t50HighLimit) * PropertyList[1].Apply;
            // Ale[2, 12] = (CompOilList[6].t50 - ProdOilList[0].t50HighLimit) * PropertyList[1].Apply;
            // Ale[2, 14] = (CompOilList[7].t50 - ProdOilList[0].t50HighLimit) * PropertyList[1].Apply;


            // //出口柴油馏程高限
            // Ale[3, 1] = (CompOilList[0].t50 - ProdOilList[1].t50HighLimit) * PropertyList[1].Apply;
            // Ale[3, 3] = (CompOilList[1].t50 - ProdOilList[1].t50HighLimit) * PropertyList[1].Apply;
            // Ale[3, 5] = (CompOilList[2].t50 - ProdOilList[1].t50HighLimit) * PropertyList[1].Apply;
            // Ale[3, 7] = (CompOilList[3].t50 - ProdOilList[1].t50HighLimit) * PropertyList[1].Apply;
            // Ale[3, 9] = (CompOilList[4].t50 - ProdOilList[1].t50HighLimit) * PropertyList[1].Apply;
            // Ale[3, 11] = (CompOilList[5].t50 - ProdOilList[1].t50HighLimit) * PropertyList[1].Apply;
            // Ale[3, 13] = (CompOilList[6].t50 - ProdOilList[1].t50HighLimit) * PropertyList[1].Apply;
            // Ale[3, 15] = (CompOilList[7].t50 - ProdOilList[1].t50HighLimit) * PropertyList[1].Apply;

            #endregion

            #region 多环芳烃含量高限

            for(int i = 0; i < ProdOilNum; i++){
                for(int j = 0; j < ComOilNum; j++){
                    Ale[ProdOilNum * 2 + i, ProdOilNum * j + i] = (CompOilList[j].suf - ProdOilList[i].sufHighLimit) * PropertyList[2].Apply;
                }
            }
            
            // Ale[4, 0] = (CompOilList[0].suf - ProdOilList[0].sufHighLimit) * PropertyList[2].Apply;
            // Ale[4, 2] = (CompOilList[1].suf - ProdOilList[0].sufHighLimit) * PropertyList[2].Apply;
            // Ale[4, 4] = (CompOilList[2].suf - ProdOilList[0].sufHighLimit) * PropertyList[2].Apply;
            // Ale[4, 6] = (CompOilList[3].suf - ProdOilList[0].sufHighLimit) * PropertyList[2].Apply;
            // Ale[4, 8] = (CompOilList[4].suf - ProdOilList[0].sufHighLimit) * PropertyList[2].Apply;
            // Ale[4, 10] = (CompOilList[5].suf - ProdOilList[0].sufHighLimit) * PropertyList[2].Apply;
            // Ale[4, 12] = (CompOilList[6].suf - ProdOilList[0].sufHighLimit) * PropertyList[2].Apply;
            // Ale[4, 14] = (CompOilList[7].suf - ProdOilList[0].sufHighLimit) * PropertyList[2].Apply;

            // // 出口柴油多环芳烃含量高限
            // Ale[5, 1] = (CompOilList[0].suf - ProdOilList[1].sufHighLimit) * PropertyList[2].Apply;
            // Ale[5, 3] = (CompOilList[1].suf - ProdOilList[1].sufHighLimit) * PropertyList[2].Apply;
            // Ale[5, 5] = (CompOilList[2].suf - ProdOilList[1].sufHighLimit) * PropertyList[2].Apply;
            // Ale[5, 7] = (CompOilList[3].suf - ProdOilList[1].sufHighLimit) * PropertyList[2].Apply;
            // Ale[5, 9] = (CompOilList[4].suf - ProdOilList[1].sufHighLimit) * PropertyList[2].Apply;
            // Ale[5, 11] = (CompOilList[5].suf - ProdOilList[1].sufHighLimit) * PropertyList[2].Apply;
            // Ale[5, 13] = (CompOilList[6].suf - ProdOilList[1].sufHighLimit) * PropertyList[2].Apply;
            // Ale[5, 15] = (CompOilList[7].suf - ProdOilList[1].sufHighLimit) * PropertyList[2].Apply;


            #endregion
            #region 密度高限

            for(int i = 0; i < ProdOilNum; i++){
                for(int j = 0; j < ComOilNum; j++){
                    Ale[ProdOilNum * 3 + i, ProdOilNum * j + i] = (CompOilList[j].den - ProdOilList[i].denHighLimit) * PropertyList[3].Apply;
                }
            }

            // Ale[6, 0] = (CompOilList[0].den - ProdOilList[0].denHighLimit) * PropertyList[3].Apply;
            // Ale[6, 2] = (CompOilList[1].den - ProdOilList[0].denHighLimit) * PropertyList[3].Apply;
            // Ale[6, 4] = (CompOilList[2].den - ProdOilList[0].denHighLimit) * PropertyList[3].Apply;
            // Ale[6, 6] = (CompOilList[3].den - ProdOilList[0].denHighLimit) * PropertyList[3].Apply;
            // Ale[6, 8] = (CompOilList[4].den - ProdOilList[0].denHighLimit) * PropertyList[3].Apply;
            // Ale[6, 10] = (CompOilList[5].den - ProdOilList[0].denHighLimit) * PropertyList[3].Apply;
            // Ale[6, 12] = (CompOilList[6].den - ProdOilList[0].denHighLimit) * PropertyList[3].Apply;
            // Ale[6, 14] = (CompOilList[7].den - ProdOilList[0].denHighLimit) * PropertyList[3].Apply;


            // //出口柴油密度高限
            // Ale[7, 1] = (CompOilList[0].den - ProdOilList[1].denHighLimit) * PropertyList[3].Apply;
            // Ale[7, 3] = (CompOilList[1].den - ProdOilList[1].denHighLimit) * PropertyList[3].Apply;
            // Ale[7, 5] = (CompOilList[2].den - ProdOilList[1].denHighLimit) * PropertyList[3].Apply;
            // Ale[7, 7] = (CompOilList[3].den - ProdOilList[1].denHighLimit) * PropertyList[3].Apply;
            // Ale[7, 9] = (CompOilList[4].den - ProdOilList[1].denHighLimit) * PropertyList[3].Apply;
            // Ale[7, 11] = (CompOilList[5].den - ProdOilList[1].denHighLimit) * PropertyList[3].Apply;
            // Ale[7, 13] = (CompOilList[6].den - ProdOilList[1].denHighLimit) * PropertyList[3].Apply;
            // Ale[7, 15] = (CompOilList[7].den - ProdOilList[1].denHighLimit) * PropertyList[3].Apply;


            #endregion

            #endregion

            #region  属性低限约束

            // 不等式约束定义
            Double[,] Age = new Double[4 * ProdOilNum, iNum_Uncontain];
            Double[] bge = new Double[4 * ProdOilNum];
            #region 十六烷值指数

            for(int i = 0; i < ProdOilNum; i++){
                for(int j = 0; j < ComOilNum; j++){
                    Age[i, ProdOilNum * j + i] = (CompOilList[j].ron - ProdOilList[i].ronLowLimit) * PropertyList[0].Apply;
                }
            }

            // Age[0, 0] = (CompOilList[0].ron - ProdOilList[0].ronLowLimit) * PropertyList[0].Apply;
            // Age[0, 2] = (CompOilList[1].ron - ProdOilList[0].ronLowLimit) * PropertyList[0].Apply;
            // Age[0, 4] = (CompOilList[2].ron - ProdOilList[0].ronLowLimit) * PropertyList[0].Apply;
            // Age[0, 6] = (CompOilList[3].ron - ProdOilList[0].ronLowLimit) * PropertyList[0].Apply;
            // Age[0, 8] = (CompOilList[4].ron - ProdOilList[0].ronLowLimit) * PropertyList[0].Apply;
            // Age[0, 10] = (CompOilList[5].ron - ProdOilList[0].ronLowLimit) * PropertyList[0].Apply;
            // Age[0, 12] = (CompOilList[6].ron - ProdOilList[0].ronLowLimit) * PropertyList[0].Apply;
            // Age[0, 14] = (CompOilList[7].ron - ProdOilList[0].ronLowLimit) * PropertyList[0].Apply;

            // //出柴十六烷值指数低限
            // Age[1, 1] = (CompOilList[0].ron - ProdOilList[1].ronLowLimit) * PropertyList[0].Apply;
            // Age[1, 3] = (CompOilList[1].ron - ProdOilList[1].ronLowLimit) * PropertyList[0].Apply;
            // Age[1, 5] = (CompOilList[2].ron - ProdOilList[1].ronLowLimit) * PropertyList[0].Apply;
            // Age[1, 7] = (CompOilList[3].ron - ProdOilList[1].ronLowLimit) * PropertyList[0].Apply;
            // Age[1, 9] = (CompOilList[4].ron - ProdOilList[1].ronLowLimit) * PropertyList[0].Apply;
            // Age[1, 11] = (CompOilList[5].ron - ProdOilList[1].ronLowLimit) * PropertyList[0].Apply;
            // Age[1, 13] = (CompOilList[6].ron - ProdOilList[1].ronLowLimit) * PropertyList[0].Apply;
            // Age[1, 15] = (CompOilList[7].ron - ProdOilList[1].ronLowLimit) * PropertyList[0].Apply;


            #endregion

            #region 馏程低限

            for(int i = 0; i < ProdOilNum; i++){
                for(int j = 0; j < ComOilNum; j++){
                    Age[ProdOilNum + i, ProdOilNum * j + i] = (CompOilList[j].t50 - ProdOilList[i].t50LowLimit) * PropertyList[1].Apply;
                }
            }

            // Age[2, 0] = (CompOilList[0].t50 - ProdOilList[0].t50LowLimit) * PropertyList[1].Apply;
            // Age[2, 2] = (CompOilList[1].t50 - ProdOilList[0].t50LowLimit) * PropertyList[1].Apply;
            // Age[2, 4] = (CompOilList[2].t50 - ProdOilList[0].t50LowLimit) * PropertyList[1].Apply;
            // Age[2, 6] = (CompOilList[3].t50 - ProdOilList[0].t50LowLimit) * PropertyList[1].Apply;
            // Age[2, 8] = (CompOilList[4].t50 - ProdOilList[0].t50LowLimit) * PropertyList[1].Apply;
            // Age[2, 10] = (CompOilList[5].t50 - ProdOilList[0].t50LowLimit) * PropertyList[1].Apply;
            // Age[2, 12] = (CompOilList[6].t50 - ProdOilList[0].t50LowLimit) * PropertyList[1].Apply;
            // Age[2, 14] = (CompOilList[7].t50 - ProdOilList[0].t50LowLimit) * PropertyList[1].Apply;


            // // 出口柴油馏程低限
            // Age[3, 1] = (CompOilList[0].t50 - ProdOilList[1].t50LowLimit) * PropertyList[1].Apply;
            // Age[3, 3] = (CompOilList[1].t50 - ProdOilList[1].t50LowLimit) * PropertyList[1].Apply;
            // Age[3, 5] = (CompOilList[2].t50 - ProdOilList[1].t50LowLimit) * PropertyList[1].Apply;
            // Age[3, 7] = (CompOilList[3].t50 - ProdOilList[1].t50LowLimit) * PropertyList[1].Apply;
            // Age[3, 9] = (CompOilList[4].t50 - ProdOilList[1].t50LowLimit) * PropertyList[1].Apply;
            // Age[3, 11] = (CompOilList[5].t50 - ProdOilList[1].t50LowLimit) * PropertyList[1].Apply;
            // Age[3, 13] = (CompOilList[6].t50 - ProdOilList[1].t50LowLimit) * PropertyList[1].Apply;
            // Age[3, 15] = (CompOilList[7].t50 - ProdOilList[1].t50LowLimit) * PropertyList[1].Apply;



            #endregion

            #region 多环芳烃含量低限
            // 车用柴油多环芳烃含量低限
            for(int i = 0; i < ProdOilNum; i++){
                for(int j = 0; j < ComOilNum; j++){
                    Age[ProdOilNum * 2 + i, ProdOilNum * j + i] = (CompOilList[j].suf - ProdOilList[i].sufLowLimit) * PropertyList[2].Apply;
                }
            }
            // Age[4, 0] = (CompOilList[0].suf - ProdOilList[0].sufLowLimit) * PropertyList[2].Apply;
            // Age[4, 2] = (CompOilList[1].suf - ProdOilList[0].sufLowLimit) * PropertyList[2].Apply;
            // Age[4, 4] = (CompOilList[2].suf - ProdOilList[0].sufLowLimit) * PropertyList[2].Apply;
            // Age[4, 6] = (CompOilList[3].suf - ProdOilList[0].sufLowLimit) * PropertyList[2].Apply;
            // Age[4, 8] = (CompOilList[4].suf - ProdOilList[0].sufLowLimit) * PropertyList[2].Apply;
            // Age[4, 10] = (CompOilList[5].suf - ProdOilList[0].sufLowLimit) * PropertyList[2].Apply;
            // Age[4, 12] = (CompOilList[6].suf - ProdOilList[0].sufLowLimit) * PropertyList[2].Apply;
            // Age[4, 14] = (CompOilList[7].suf - ProdOilList[0].sufLowLimit) * PropertyList[2].Apply;


            // // 出口柴油多芳烃含量低限
            // Age[5, 1] = (CompOilList[0].suf - ProdOilList[1].sufLowLimit) * PropertyList[2].Apply;
            // Age[5, 3] = (CompOilList[1].suf - ProdOilList[1].sufLowLimit) * PropertyList[2].Apply;
            // Age[5, 5] = (CompOilList[2].suf - ProdOilList[1].sufLowLimit) * PropertyList[2].Apply;
            // Age[5, 7] = (CompOilList[3].suf - ProdOilList[1].sufLowLimit) * PropertyList[2].Apply;
            // Age[5, 9] = (CompOilList[4].suf - ProdOilList[1].sufLowLimit) * PropertyList[2].Apply;
            // Age[5, 11] = (CompOilList[5].suf - ProdOilList[1].sufLowLimit) * PropertyList[2].Apply;
            // Age[5, 13] = (CompOilList[6].suf - ProdOilList[1].sufLowLimit) * PropertyList[2].Apply;
            // Age[5, 15] = (CompOilList[7].suf - ProdOilList[1].sufLowLimit) * PropertyList[2].Apply;

            #endregion

            #region 密度低限
            //车用柴油密度低限
            for(int i = 0; i < ProdOilNum; i++){
                for(int j = 0; j < ComOilNum; j++){
                    Age[ProdOilNum * 3 + i, ProdOilNum * j + i] = (CompOilList[j].den - ProdOilList[i].denLowLimit) * PropertyList[3].Apply;
                }
            }

            // Age[6, 0] = (CompOilList[0].den - ProdOilList[0].denLowLimit) * PropertyList[3].Apply;
            // Age[6, 2] = (CompOilList[1].den - ProdOilList[0].denLowLimit) * PropertyList[3].Apply;
            // Age[6, 4] = (CompOilList[2].den - ProdOilList[0].denLowLimit) * PropertyList[3].Apply;
            // Age[6, 6] = (CompOilList[3].den - ProdOilList[0].denLowLimit) * PropertyList[3].Apply;
            // Age[6, 8] = (CompOilList[4].den - ProdOilList[0].denLowLimit) * PropertyList[3].Apply;
            // Age[6, 10] = (CompOilList[5].den - ProdOilList[0].denLowLimit) * PropertyList[3].Apply;
            // Age[6, 12] = (CompOilList[6].den - ProdOilList[0].denLowLimit) * PropertyList[3].Apply;
            // Age[6, 14] = (CompOilList[7].den - ProdOilList[0].denLowLimit) * PropertyList[3].Apply;

            // // 出口柴油密度低限
            // Age[7, 1] = (CompOilList[0].den - ProdOilList[1].denLowLimit) * PropertyList[3].Apply;
            // Age[7, 3] = (CompOilList[1].den - ProdOilList[1].denLowLimit) * PropertyList[3].Apply;
            // Age[7, 5] = (CompOilList[2].den - ProdOilList[1].denLowLimit) * PropertyList[3].Apply;
            // Age[7, 7] = (CompOilList[3].den - ProdOilList[1].denLowLimit) * PropertyList[3].Apply;
            // Age[7, 9] = (CompOilList[4].den - ProdOilList[1].denLowLimit) * PropertyList[3].Apply;
            // Age[7, 11] = (CompOilList[5].den - ProdOilList[1].denLowLimit) * PropertyList[3].Apply;
            // Age[7, 13] = (CompOilList[6].den - ProdOilList[1].denLowLimit) * PropertyList[3].Apply;
            // Age[7, 15] = (CompOilList[7].den - ProdOilList[1].denLowLimit) * PropertyList[3].Apply;

            #endregion
            #endregion

            #region  在求解器里定义目标函数和约束条件，并求解
            //变量定义
            int n = iNum;
        
            //计算
            IntPtr lp;
            double[] Row = new double[iNum + 1];//因为Row[0]是常数位
            double[] Col;
            lp = lpsolve.make_lp(0, n);//变量个数

            #region 约束
            //等式约束
            //组分油产量不等式高限约束
            for (int i = 0; i < ComOilNum; i++) //i表示矩阵的行
            {
                for (int j = 0; j < iNum; j++) //j表示列
                {
                    Row[j + 1] = Comp_le_up[i, j];
                }
                Row[0] = 0;//加lp的都是要算的变量
                lpsolve.add_constraint(lp, Row, lpsolve.lpsolve_constr_types.LE, Comp_rle_up[i]);//组分油不等式约束
                //第一行Comp_le，单位是t/m³，乘以需要计算得出的体积m³，等于第一个组分油的产量Comp_rl，单位为t
            }

            //组分油产量不等式低限约束（各组分油的用量和 = 各组分油总产量）
            for (int i = 0; i < ComOilNum; i++) //i表示矩阵的行
            {
                for (int j = 0; j < iNum; j++) //j表示列 8行16列
                {
                    Row[j + 1] = Comp_le_low[i, j];
                }
                Row[0] = 0;//加lp的都是要算的变量
                lpsolve.add_constraint(lp, Row, lpsolve.lpsolve_constr_types.GE, Comp_rle_low[i]);//组分油不等式约束
                //第一行Comp_le，单位是t/m³，乘以需要计算得出的体积m³，等于第一个组分油的产量Comp_rl，单位为t
            }

            Array.Clear(Row, 0, Row.Length);//用完row数组对其清空（之前没有清空是因为每一次都重新赋值给覆盖了）          

            //不等式约束
            //
            for (int i = 0; i < ProdOilNum; i++)
            {
                for (int j = 0; j < iNum_Uncontain; j++)
                {
                    Row[j + 1] = Prod_le[i, j];
                }
                Row[0] = 0;
                lpsolve.add_constraint(lp, Row, lpsolve.lpsolve_constr_types.LE, Prod_rle[i]);//成品油产量不等式约束
                //
            }

            //属性高限
            for (int i = 0; i < 4 * ProdOilNum; i++)
            {
                for (int j = 0; j < iNum_Uncontain; j++)
                {
                    Row[j + 1] = Ale[i, j];
                }
                Row[0] = 0;
                lpsolve.add_constraint(lp, Row, lpsolve.lpsolve_constr_types.LE, ble[i]);//LE高限
            }

            //属性低限
            for (int i = 0; i < 4 * ProdOilNum; i++)
            {
                for (int j = 0; j < iNum_Uncontain; j++)
                {
                    Row[j + 1] = Age[i, j];
                }
                Row[0] = 0;
                lpsolve.add_constraint(lp, Row, lpsolve.lpsolve_constr_types.GE, bge[i]);//GE低限
            }

            Array.Clear(Row, 0, Row.Length);//用完row数组对其清空   
            //库存高限

            // //流速上下限 即需要求解变量的上下限
            // for (int i = 0; i < n; i++)
            // {
            //     lpsolve.set_lowbo(lp, i + 1, LB[i]);
            //     lpsolve.set_upbo(lp, i + 1, UB[i]);
            // }

            //变量上下限
            for (int i = iNum_Uncontain; i < n; i++)
            {
                lpsolve.set_lowbo(lp, i + 1, LB[i]);//低限
                lpsolve.set_upbo(lp, i + 1, UB[i]);//高限
            }
                     
            #endregion

            //目标函数
            for (int i = 0; i < n; i++)
            {
                Row[i + 1] = f[i];
            }
            Row[0] = 0;
            lpsolve.set_obj_fn(lp, Row);
            //求解
            lpsolve.set_minim(lp);
            lpsolve.solve(lp);
            int status = lpsolve.get_status(lp);//0或2 从solve之后，status的标志位才会变化
            #endregion

            // Col 为最优解
            Col = new double[lpsolve.get_Ncolumns(lp)]; 
            // Console.WriteLine(lpsolve.get_statustext(lp, 0));最优解
            // Console.WriteLine(lpsolve.get_statustext(lp, 1));1 次优解 2 无解 infeasible
            lpsolve.get_variables(lp, Col);            
            for(int i = 0; i < iNum; i++){
                Calc[i] = Col[i];
            }    
            Calc[iNum] = status;//求解标志位   
            return Calc;
        }      
        public double[] GetRecipe2()//场景2的lpsolve求解配方
        {
            IProdOilConfig _ProdOilConfig = new ProdOilConfig(context);
            IProperty _Property = new PropertyApply(context);
            IRecipeCalc _RecipeCalc = new RecipeCalc(context);
            ICompOilConfig _CompOilConfig = new CompOilConfig(context);
            
            //context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;

            var CompOilList = _CompOilConfig.GetAllCompOilConfigList().ToList();
            var ProdOilList = _ProdOilConfig.GetAllProdOilConfigList().Where(m=>m.Apply == 1).ToList();
            var PropertyList = _Property.GetAllPropertyList().ToList();
            var CompOilConstraint = _RecipeCalc.GetRecipeCalc1().ToList();//组分油产量 罐容高低限 配方高低限 约束
            var Weight = _RecipeCalc.GetRecipeCalc2_2().Where(m=>m.Apply == 1).ToList();//多目权值,三个场景的
            var ProdOilProduct = _RecipeCalc.GetRecipeCalc3().Where(m=>m.Apply == 1).ToList();//成品油产量限制

            int ComOilNum = CompOilList.Count;//组分油个数
            int ProdOilNum = ProdOilProduct.Count;//成品油个数

            //变量约束定义
            //变量个数应为 成品油启用个数 x 组分油个数
            //int iNum = 24;//2X8是组分油1（车柴 出柴）组分油2（车柴 出柴）
            int iNum = ProdOilNum * ComOilNum;
            double[] Calc = new double[iNum + 1];//多加的一位是求解标志位

            #region 目标函数定义

            // 目标函数定义
            float[] f = new float[iNum];
            for(int i = 0; i < ComOilNum; i++){//成品油的目标函数(产量最大 + 十六烷值 + 多芳烃)
                for(int j = 0; j < ProdOilNum; j++){
                    f[ProdOilNum * i + j] = -Weight[j].Weight * CompOilList[i].den / 1000 * 10000 + Weight[ProdOilNum + j].Weight * (CompOilList[i].ron - ProdOilList[j].ronLowLimit) * 1000 + Weight[ProdOilNum * 2 + j].Weight * (CompOilList[i].suf - ProdOilList[j].sufLowLimit) * 1000 ; 
                }
            }

            // f[0] = -Weight[0].Weight * CompOilList[0].den / 1000 * 10000 ;//质量产量乘以10000，质量指标乘以1000
            // f[1] = -Weight[3].Weight * CompOilList[0].den / 1000 * 10000 + Weight[1].Weight * (CompOilList[0].ron - ProdOilList[1].ronLowLimit) * 1000 - Weight[2].Weight * (CompOilList[0].suf - ProdOilList[1].sufHighLimit) * 1000 ;
            // //-CompOilList[0].den / 1000 * 10000;  一个向上卡边一个向下卡边
            // f[2] = -Weight[0].Weight * CompOilList[1].den / 1000 * 10000 ;
            // f[3] = -Weight[3].Weight * CompOilList[1].den / 1000 * 10000 + Weight[1].Weight * (CompOilList[1].ron - ProdOilList[1].ronLowLimit) * 1000 - Weight[2].Weight * (CompOilList[1].suf - ProdOilList[1].sufHighLimit) * 1000 ;
            // //-CompOilList[1].den / 1000 * 10000;
            // f[4] = -Weight[0].Weight * CompOilList[2].den / 1000 * 10000 ;
            // f[5] = -Weight[3].Weight * CompOilList[2].den / 1000 * 10000 + Weight[1].Weight * (CompOilList[2].ron - ProdOilList[1].ronLowLimit) * 1000 - Weight[2].Weight * (CompOilList[2].suf - ProdOilList[1].sufHighLimit) * 1000 ;
            // //-CompOilList[2].den / 1000 * 10000;
            // f[6] = -Weight[0].Weight * CompOilList[3].den / 1000 * 10000 ;
            // f[7] = -Weight[3].Weight * CompOilList[3].den / 1000 * 10000 + Weight[1].Weight * (CompOilList[3].ron - ProdOilList[1].ronLowLimit) * 1000 - Weight[2].Weight * (CompOilList[3].suf - ProdOilList[1].sufHighLimit) * 1000 ;
            // //-CompOilList[3].den / 1000 * 10000;
            // f[8] = -Weight[0].Weight * CompOilList[4].den / 1000 * 10000 ;
            // f[9] = -Weight[3].Weight * CompOilList[4].den / 1000 * 10000 + Weight[1].Weight * (CompOilList[4].ron - ProdOilList[1].ronLowLimit) * 1000 - Weight[2].Weight * (CompOilList[4].suf - ProdOilList[1].sufHighLimit) * 1000 ;

            // f[10] = -Weight[0].Weight * CompOilList[5].den / 1000 * 10000 ;
            // f[11] = -Weight[3].Weight * CompOilList[5].den / 1000 * 10000 + Weight[1].Weight * (CompOilList[5].ron - ProdOilList[1].ronLowLimit) * 1000 - Weight[2].Weight * (CompOilList[5].suf - ProdOilList[1].sufHighLimit) * 1000 ;

            // f[12] = -Weight[0].Weight * CompOilList[6].den / 1000 * 10000 ;
            // f[13] = -Weight[3].Weight * CompOilList[6].den / 1000 * 10000 + Weight[1].Weight * (CompOilList[6].ron - ProdOilList[1].ronLowLimit) * 1000 - Weight[2].Weight * (CompOilList[6].suf - ProdOilList[1].sufHighLimit) * 1000 ;

            // f[14] = -Weight[0].Weight * CompOilList[7].den / 1000 * 10000 ;
            // f[15] = -Weight[3].Weight * CompOilList[7].den / 1000 * 10000 + Weight[1].Weight * (CompOilList[7].ron - ProdOilList[1].ronLowLimit) * 1000 - Weight[2].Weight * (CompOilList[7].suf - ProdOilList[1].sufHighLimit) * 1000 ;
            
            // for(int i = 0; i < ComOilNum; i++){
            //     f[i + ProdOilNum * ComOilNum] = CompOilList[i].den / 1000 * CompOilList[i].Price;
            // }\

            //把数据库中的配方高低限转为二维数组
            double[,] ProdOilFlowLow = new double[ComOilNum, 4];//成品油内置个数4
            for(int i = 0; i < ComOilNum; i++){
                ProdOilFlowLow[i, 0] = CompOilConstraint[i].gas92FlowLow;//第一个成品油
                ProdOilFlowLow[i, 1] = CompOilConstraint[i].gas95FlowLow;//第二个成品油
                ProdOilFlowLow[i, 2] = CompOilConstraint[i].gas98FlowLow;
                ProdOilFlowLow[i, 3] = CompOilConstraint[i].gasSelfFlowLow;
            }

            double[,] ProdOilFlowUp = new double[ComOilNum, 4];//成品油内置个数4
            for(int i = 0; i < ComOilNum; i++){
                ProdOilFlowUp[i, 0] = CompOilConstraint[i].gas92FlowHigh;//第一个成品油
                ProdOilFlowUp[i, 1] = CompOilConstraint[i].gas95FlowHigh;//第二个成品油
                ProdOilFlowUp[i, 2] = CompOilConstraint[i].gas98FlowHigh;
                ProdOilFlowUp[i, 3] = CompOilConstraint[i].gasSelfFlowHigh;
            }

            #endregion

            //组分油参调流量上下限（用组分油配方上下限乘以参调总流量）

            Double[] LB = new Double[iNum];// 变量低限
            Double[] UB = new Double[iNum];// 高限

            for(int i = 0; i < ComOilNum; i++){
                for(int j = 0; j < ProdOilNum; j++){
                    LB[ProdOilNum * i + j] = ProdOilProduct[j].TotalFlow * ProdOilFlowLow[i, ProdOilProduct[j].Id - 1] / 100 / CompOilList[i].den * 1000;
                    UB[ProdOilNum * i + j] = ProdOilProduct[j].TotalFlow * ProdOilFlowUp[i, ProdOilProduct[j].Id - 1] / 100 / CompOilList[i].den * 1000;
                }
            }

            #region 流量上下限          
            // LB[0] = CompOilList[0].gas92Low1 / CompOilList[0].den * 1000;
            // LB[1] = CompOilList[0].gas95Low1 / CompOilList[0].den * 1000;

            // LB[2] = CompOilList[1].gas92Low1 / CompOilList[1].den * 1000;
            // LB[3] = CompOilList[1].gas95Low1 / CompOilList[1].den * 1000;

            // LB[4] = CompOilList[2].gas92Low1 / CompOilList[2].den * 1000;
            // LB[5] = CompOilList[2].gas95Low1 / CompOilList[2].den * 1000;

            // LB[6] = CompOilList[3].gas92Low1 / CompOilList[3].den * 1000;
            // LB[7] = CompOilList[3].gas95Low1 / CompOilList[3].den * 1000;

            // LB[8] = CompOilList[4].gas92Low1 / CompOilList[4].den * 1000;
            // LB[9] = CompOilList[4].gas95Low1 / CompOilList[4].den * 1000;

            // LB[10] = CompOilList[5].gas92Low1 / CompOilList[5].den * 1000;
            // LB[11] = CompOilList[5].gas95Low1 / CompOilList[5].den * 1000;

            // LB[12] = CompOilList[6].gas92Low1 / CompOilList[6].den * 1000;
            // LB[13] = CompOilList[6].gas95Low1 / CompOilList[6].den * 1000;

            // LB[14] = CompOilList[7].gas92Low1 / CompOilList[7].den * 1000;
            // LB[15] = CompOilList[7].gas95Low1 / CompOilList[7].den * 1000;

            // UB[0] = CompOilList[0].gas92High1 / CompOilList[0].den * 1000; 
            // UB[1] = CompOilList[0].gas95High1 / CompOilList[0].den * 1000;

            // UB[2] = CompOilList[1].gas92High1 / CompOilList[1].den * 1000;
            // UB[3] = CompOilList[1].gas95High1 / CompOilList[1].den * 1000;

            // UB[4] = CompOilList[2].gas92High1 / CompOilList[2].den * 1000;
            // UB[5] = CompOilList[2].gas95High1 / CompOilList[2].den * 1000;

            // UB[6] = CompOilList[3].gas92High1 / CompOilList[3].den * 1000;
            // UB[7] = CompOilList[3].gas95High1 / CompOilList[3].den * 1000;

            // UB[8] = CompOilList[4].gas92High1 / CompOilList[4].den * 1000;
            // UB[9] = CompOilList[4].gas95High1 / CompOilList[4].den * 1000;

            // UB[10] = CompOilList[5].gas92High1 / CompOilList[5].den * 1000;
            // UB[11] = CompOilList[5].gas95High1 / CompOilList[5].den * 1000;

            // UB[12] = CompOilList[6].gas92High1 / CompOilList[6].den * 1000;
            // UB[13] = CompOilList[6].gas95High1 / CompOilList[6].den * 1000;

            // UB[14] = CompOilList[7].gas92High1 / CompOilList[7].den * 1000;
            // UB[15] = CompOilList[7].gas95High1 / CompOilList[7].den * 1000;
            
            #endregion

            #region 成品油不等式约束
            // 成品油产量不等式约束

            Double[,] Prod_le = new Double[ProdOilNum, iNum];//2行16列矩阵 带变量和系数的计算式
            Double[] Prod_rle = new Double[ProdOilNum];

            for(int i = 0; i < ProdOilNum; i++){
                Prod_rle[i] = ProdOilProduct[i].TotalFlow;//成品油产量限制 
            }

            for(int i = 0; i < ProdOilNum; i++){
                for(int j = 0; j < ComOilNum; j++){
                    Prod_le[i, ProdOilNum * j + i] = CompOilList[j].den / 1000;//组分油密度，t/m³ 成品油产量限制
                }
            }

            // P1产量不等式约束 车用柴油

            // Prod_le[0, 2] = CompOilList[1].den / 1000;
            // Prod_le[0, 4] = CompOilList[2].den / 1000;
            // Prod_le[0, 6] = CompOilList[3].den / 1000;
            // Prod_le[0, 8] = CompOilList[4].den / 1000;
            // Prod_le[0, 10] = CompOilList[5].den / 1000;
            // Prod_le[0, 12] = CompOilList[6].den / 1000;
            // Prod_le[0, 14] = CompOilList[7].den / 1000;

            // // P2产量不等式约束 出口柴油
            // Prod_le[1, 1] = CompOilList[0].den / 1000;
            // Prod_le[1, 3] = CompOilList[1].den / 1000;
            // Prod_le[1, 5] = CompOilList[2].den / 1000;
            // Prod_le[1, 7] = CompOilList[3].den / 1000;
            // Prod_le[1, 9] = CompOilList[4].den / 1000;
            // Prod_le[1, 11] = CompOilList[5].den / 1000;
            // Prod_le[1, 13] = CompOilList[6].den / 1000;
            // Prod_le[1, 15] = CompOilList[7].den / 1000;

            #endregion

            #region 组分油不等式约束

            // 组分油不等式约束 有罐子

            // Double[,] Comp_le_up = new Double[ComOilNum, iNum];
            // Double[,] Comp_le_low = new Double[ComOilNum, iNum];
            // Double[] Comp_rle_low = new Double[ComOilNum];// 低限
            // Double[] Comp_rle_up = new Double[ComOilNum];// 高限

            // for( int i = 0; i < ComOilNum; i++)
            // {
            //     Comp_rle_low[i] = CompOilConstraint[i].VolumeLow - CompOilConstraint[i].IniVolume;           
            //     Comp_rle_up[i] = CompOilConstraint[i].VolumeHigh - CompOilConstraint[i].IniVolume;
            // }
            // //下限约束
            // for(int i = 0; i < ComOilNum; i++){
            //     for(int j = 0; j < ProdOilNum; j++){
            //         Comp_le_low[i, ProdOilNum * i + j] = - CompOilList[i].den / 1000;
            //     }
            //     Comp_le_low[i, iNum_Uncontain + i] = CompOilList[i].den / 1000;
            // }
            // //上限约束
            // for(int i = 0; i < ComOilNum; i++){
            //     Comp_le_up[i, iNum_Uncontain + i] = CompOilList[i].den / 1000;
            // }           

            // // F2产量等式约束 第二个组分油
            // Comp_le[1, 2] = CompOilList[1].den / 1000;第一个组分油的车柴变量
            // Comp_le[1, 3] = CompOilList[1].den / 1000;第一个组分油的出柴变量
            // Comp_le[1, 17] = CompOilList[1].den / 1000; 第一个组分油产量变量

            // // F3产量等式约束
            // Comp_le[2, 4] = CompOilList[2].den / 1000;
            // Comp_le[2, 5] = CompOilList[2].den / 1000;
            // Comp_le[2, 18] = CompOilList[2].den / 1000;

            // // F4产量等式约束
            // Comp_le[3, 6] = CompOilList[3].den / 1000;
            // Comp_le[3, 7] = CompOilList[3].den / 1000;
            // Comp_le[3, 19] = CompOilList[3].den / 1000;

            // // F5产量等式约束
            // Comp_le[4, 8] = CompOilList[4].den / 1000;
            // Comp_le[4, 9] = CompOilList[4].den / 1000;
            // Comp_le[4, 20] = CompOilList[4].den / 1000;

            // // F6产量等式约束
            // Comp_le[5, 10] = CompOilList[5].den / 1000;
            // Comp_le[5, 11] = CompOilList[5].den / 1000;
            // Comp_le[5, 21] = CompOilList[5].den / 1000;

            // // F7产量等式约束
            // Comp_le[6, 12] = CompOilList[6].den / 1000;
            // Comp_le[6, 13] = CompOilList[6].den / 1000;
            // Comp_le[6, 22] = CompOilList[6].den / 1000;

            // // F8产量等式约束 第八个组分油
            // Comp_le[7, 14] = CompOilList[7].den / 1000;
            // Comp_le[7, 15] = CompOilList[7].den / 1000;
            // Comp_le[7, 23] = CompOilList[7].den / 1000;

            #endregion

            #region 属性高限约束

            // 属性不等式约束 八个组分油 8行16列
            Double[,] Ale = new Double[4 * ProdOilNum, iNum];//这个的行应该是成品油个数乘以属性个数
            Double[] ble = new Double[4 * ProdOilNum];

            #region 十六烷值指数高限

            for(int i = 0; i < ProdOilNum; i++){
                for(int j = 0; j < ComOilNum; j++){
                    Ale[i, ProdOilNum * j + i] = (CompOilList[j].ron - ProdOilList[i].ronHighLimit) * PropertyList[0].Apply;
                }
            }

            // 车柴十六烷值指数高限 八个组分油

            // Ale[0, 2] = (CompOilList[1].ron - ProdOilList[0].ronHighLimit) * PropertyList[0].Apply;
            // Ale[0, 4] = (CompOilList[2].ron - ProdOilList[0].ronHighLimit) * PropertyList[0].Apply;
            // Ale[0, 6] = (CompOilList[3].ron - ProdOilList[0].ronHighLimit) * PropertyList[0].Apply;
            // Ale[0, 8] = (CompOilList[4].ron - ProdOilList[0].ronHighLimit) * PropertyList[0].Apply;
            // Ale[0, 10] = (CompOilList[5].ron - ProdOilList[0].ronHighLimit) * PropertyList[0].Apply;
            // Ale[0, 12] = (CompOilList[6].ron - ProdOilList[0].ronHighLimit) * PropertyList[0].Apply;
            // Ale[0, 14] = (CompOilList[7].ron - ProdOilList[0].ronHighLimit) * PropertyList[0].Apply;


            // //出柴十六烷值指数高限 
            // Ale[1, 1] = (CompOilList[0].ron - ProdOilList[1].ronHighLimit) * PropertyList[0].Apply;
            // Ale[1, 3] = (CompOilList[1].ron - ProdOilList[1].ronHighLimit) * PropertyList[0].Apply;
            // Ale[1, 5] = (CompOilList[2].ron - ProdOilList[1].ronHighLimit) * PropertyList[0].Apply;
            // Ale[1, 7] = (CompOilList[3].ron - ProdOilList[1].ronHighLimit) * PropertyList[0].Apply;
            // Ale[1, 9] = (CompOilList[4].ron - ProdOilList[1].ronHighLimit) * PropertyList[0].Apply;
            // Ale[1, 11] = (CompOilList[5].ron - ProdOilList[1].ronHighLimit) * PropertyList[0].Apply;
            // Ale[1, 13] = (CompOilList[6].ron - ProdOilList[1].ronHighLimit) * PropertyList[0].Apply;
            // Ale[1, 15] = (CompOilList[7].ron - ProdOilList[1].ronHighLimit) * PropertyList[0].Apply;

            #endregion

            #region 馏程高限

            for(int i = 0; i < ProdOilNum; i++){
                for(int j = 0; j < ComOilNum; j++){
                    Ale[ProdOilNum + i, ProdOilNum * j + i] = (CompOilList[j].t50 - ProdOilList[i].t50HighLimit) * PropertyList[1].Apply;
                }
            }

            // Ale[2, 0] = (CompOilList[0].t50 - ProdOilList[0].t50HighLimit) * PropertyList[1].Apply;
            // Ale[2, 2] = (CompOilList[1].t50 - ProdOilList[0].t50HighLimit) * PropertyList[1].Apply;
            // Ale[2, 4] = (CompOilList[2].t50 - ProdOilList[0].t50HighLimit) * PropertyList[1].Apply;
            // Ale[2, 6] = (CompOilList[3].t50 - ProdOilList[0].t50HighLimit) * PropertyList[1].Apply;
            // Ale[2, 8] = (CompOilList[4].t50 - ProdOilList[0].t50HighLimit) * PropertyList[1].Apply;
            // Ale[2, 10] = (CompOilList[5].t50 - ProdOilList[0].t50HighLimit) * PropertyList[1].Apply;
            // Ale[2, 12] = (CompOilList[6].t50 - ProdOilList[0].t50HighLimit) * PropertyList[1].Apply;
            // Ale[2, 14] = (CompOilList[7].t50 - ProdOilList[0].t50HighLimit) * PropertyList[1].Apply;


            // //出口柴油馏程高限
            // Ale[3, 1] = (CompOilList[0].t50 - ProdOilList[1].t50HighLimit) * PropertyList[1].Apply;
            // Ale[3, 3] = (CompOilList[1].t50 - ProdOilList[1].t50HighLimit) * PropertyList[1].Apply;
            // Ale[3, 5] = (CompOilList[2].t50 - ProdOilList[1].t50HighLimit) * PropertyList[1].Apply;
            // Ale[3, 7] = (CompOilList[3].t50 - ProdOilList[1].t50HighLimit) * PropertyList[1].Apply;
            // Ale[3, 9] = (CompOilList[4].t50 - ProdOilList[1].t50HighLimit) * PropertyList[1].Apply;
            // Ale[3, 11] = (CompOilList[5].t50 - ProdOilList[1].t50HighLimit) * PropertyList[1].Apply;
            // Ale[3, 13] = (CompOilList[6].t50 - ProdOilList[1].t50HighLimit) * PropertyList[1].Apply;
            // Ale[3, 15] = (CompOilList[7].t50 - ProdOilList[1].t50HighLimit) * PropertyList[1].Apply;

            #endregion

            #region 多环芳烃含量高限

            for(int i = 0; i < ProdOilNum; i++){
                for(int j = 0; j < ComOilNum; j++){
                    Ale[ProdOilNum * 2 + i, ProdOilNum * j + i] = (CompOilList[j].suf - ProdOilList[i].sufHighLimit) * PropertyList[2].Apply;
                }
            }
            
            // Ale[4, 0] = (CompOilList[0].suf - ProdOilList[0].sufHighLimit) * PropertyList[2].Apply;
            // Ale[4, 2] = (CompOilList[1].suf - ProdOilList[0].sufHighLimit) * PropertyList[2].Apply;
            // Ale[4, 4] = (CompOilList[2].suf - ProdOilList[0].sufHighLimit) * PropertyList[2].Apply;
            // Ale[4, 6] = (CompOilList[3].suf - ProdOilList[0].sufHighLimit) * PropertyList[2].Apply;
            // Ale[4, 8] = (CompOilList[4].suf - ProdOilList[0].sufHighLimit) * PropertyList[2].Apply;
            // Ale[4, 10] = (CompOilList[5].suf - ProdOilList[0].sufHighLimit) * PropertyList[2].Apply;
            // Ale[4, 12] = (CompOilList[6].suf - ProdOilList[0].sufHighLimit) * PropertyList[2].Apply;
            // Ale[4, 14] = (CompOilList[7].suf - ProdOilList[0].sufHighLimit) * PropertyList[2].Apply;

            // // 出口柴油多环芳烃含量高限
            // Ale[5, 1] = (CompOilList[0].suf - ProdOilList[1].sufHighLimit) * PropertyList[2].Apply;
            // Ale[5, 3] = (CompOilList[1].suf - ProdOilList[1].sufHighLimit) * PropertyList[2].Apply;
            // Ale[5, 5] = (CompOilList[2].suf - ProdOilList[1].sufHighLimit) * PropertyList[2].Apply;
            // Ale[5, 7] = (CompOilList[3].suf - ProdOilList[1].sufHighLimit) * PropertyList[2].Apply;
            // Ale[5, 9] = (CompOilList[4].suf - ProdOilList[1].sufHighLimit) * PropertyList[2].Apply;
            // Ale[5, 11] = (CompOilList[5].suf - ProdOilList[1].sufHighLimit) * PropertyList[2].Apply;
            // Ale[5, 13] = (CompOilList[6].suf - ProdOilList[1].sufHighLimit) * PropertyList[2].Apply;
            // Ale[5, 15] = (CompOilList[7].suf - ProdOilList[1].sufHighLimit) * PropertyList[2].Apply;


            #endregion
            #region 密度高限

            for(int i = 0; i < ProdOilNum; i++){
                for(int j = 0; j < ComOilNum; j++){
                    Ale[ProdOilNum * 3 + i, ProdOilNum * j + i] = (CompOilList[j].den - ProdOilList[i].denHighLimit) * PropertyList[3].Apply;
                }
            }

            // Ale[6, 0] = (CompOilList[0].den - ProdOilList[0].denHighLimit) * PropertyList[3].Apply;
            // Ale[6, 2] = (CompOilList[1].den - ProdOilList[0].denHighLimit) * PropertyList[3].Apply;
            // Ale[6, 4] = (CompOilList[2].den - ProdOilList[0].denHighLimit) * PropertyList[3].Apply;
            // Ale[6, 6] = (CompOilList[3].den - ProdOilList[0].denHighLimit) * PropertyList[3].Apply;
            // Ale[6, 8] = (CompOilList[4].den - ProdOilList[0].denHighLimit) * PropertyList[3].Apply;
            // Ale[6, 10] = (CompOilList[5].den - ProdOilList[0].denHighLimit) * PropertyList[3].Apply;
            // Ale[6, 12] = (CompOilList[6].den - ProdOilList[0].denHighLimit) * PropertyList[3].Apply;
            // Ale[6, 14] = (CompOilList[7].den - ProdOilList[0].denHighLimit) * PropertyList[3].Apply;


            // //出口柴油密度高限
            // Ale[7, 1] = (CompOilList[0].den - ProdOilList[1].denHighLimit) * PropertyList[3].Apply;
            // Ale[7, 3] = (CompOilList[1].den - ProdOilList[1].denHighLimit) * PropertyList[3].Apply;
            // Ale[7, 5] = (CompOilList[2].den - ProdOilList[1].denHighLimit) * PropertyList[3].Apply;
            // Ale[7, 7] = (CompOilList[3].den - ProdOilList[1].denHighLimit) * PropertyList[3].Apply;
            // Ale[7, 9] = (CompOilList[4].den - ProdOilList[1].denHighLimit) * PropertyList[3].Apply;
            // Ale[7, 11] = (CompOilList[5].den - ProdOilList[1].denHighLimit) * PropertyList[3].Apply;
            // Ale[7, 13] = (CompOilList[6].den - ProdOilList[1].denHighLimit) * PropertyList[3].Apply;
            // Ale[7, 15] = (CompOilList[7].den - ProdOilList[1].denHighLimit) * PropertyList[3].Apply;


            #endregion

            #endregion

            #region  属性低限约束

            // 不等式约束定义
            Double[,] Age = new Double[4 * ProdOilNum, iNum];//Age高限，Age低限
            Double[] bge = new Double[4 * ProdOilNum];
            #region 十六烷值指数

            for(int i = 0; i < ProdOilNum; i++){
                for(int j = 0; j < ComOilNum; j++){
                    Age[i, ProdOilNum * j + i] = (CompOilList[j].ron - ProdOilList[i].ronLowLimit) * PropertyList[0].Apply;
                }
            }

            // Age[0, 0] = (CompOilList[0].ron - ProdOilList[0].ronLowLimit) * PropertyList[0].Apply;
            // Age[0, 2] = (CompOilList[1].ron - ProdOilList[0].ronLowLimit) * PropertyList[0].Apply;
            // Age[0, 4] = (CompOilList[2].ron - ProdOilList[0].ronLowLimit) * PropertyList[0].Apply;
            // Age[0, 6] = (CompOilList[3].ron - ProdOilList[0].ronLowLimit) * PropertyList[0].Apply;
            // Age[0, 8] = (CompOilList[4].ron - ProdOilList[0].ronLowLimit) * PropertyList[0].Apply;
            // Age[0, 10] = (CompOilList[5].ron - ProdOilList[0].ronLowLimit) * PropertyList[0].Apply;
            // Age[0, 12] = (CompOilList[6].ron - ProdOilList[0].ronLowLimit) * PropertyList[0].Apply;
            // Age[0, 14] = (CompOilList[7].ron - ProdOilList[0].ronLowLimit) * PropertyList[0].Apply;

            // //出柴十六烷值指数低限
            // Age[1, 1] = (CompOilList[0].ron - ProdOilList[1].ronLowLimit) * PropertyList[0].Apply;
            // Age[1, 3] = (CompOilList[1].ron - ProdOilList[1].ronLowLimit) * PropertyList[0].Apply;
            // Age[1, 5] = (CompOilList[2].ron - ProdOilList[1].ronLowLimit) * PropertyList[0].Apply;
            // Age[1, 7] = (CompOilList[3].ron - ProdOilList[1].ronLowLimit) * PropertyList[0].Apply;
            // Age[1, 9] = (CompOilList[4].ron - ProdOilList[1].ronLowLimit) * PropertyList[0].Apply;
            // Age[1, 11] = (CompOilList[5].ron - ProdOilList[1].ronLowLimit) * PropertyList[0].Apply;
            // Age[1, 13] = (CompOilList[6].ron - ProdOilList[1].ronLowLimit) * PropertyList[0].Apply;
            // Age[1, 15] = (CompOilList[7].ron - ProdOilList[1].ronLowLimit) * PropertyList[0].Apply;


            #endregion

            #region 馏程低限

            for(int i = 0; i < ProdOilNum; i++){
                for(int j = 0; j < ComOilNum; j++){
                    Age[ProdOilNum + i, ProdOilNum * j + i] = (CompOilList[j].t50 - ProdOilList[i].t50LowLimit) * PropertyList[1].Apply;
                }
            }

            // Age[2, 0] = (CompOilList[0].t50 - ProdOilList[0].t50LowLimit) * PropertyList[1].Apply;
            // Age[2, 2] = (CompOilList[1].t50 - ProdOilList[0].t50LowLimit) * PropertyList[1].Apply;
            // Age[2, 4] = (CompOilList[2].t50 - ProdOilList[0].t50LowLimit) * PropertyList[1].Apply;
            // Age[2, 6] = (CompOilList[3].t50 - ProdOilList[0].t50LowLimit) * PropertyList[1].Apply;
            // Age[2, 8] = (CompOilList[4].t50 - ProdOilList[0].t50LowLimit) * PropertyList[1].Apply;
            // Age[2, 10] = (CompOilList[5].t50 - ProdOilList[0].t50LowLimit) * PropertyList[1].Apply;
            // Age[2, 12] = (CompOilList[6].t50 - ProdOilList[0].t50LowLimit) * PropertyList[1].Apply;
            // Age[2, 14] = (CompOilList[7].t50 - ProdOilList[0].t50LowLimit) * PropertyList[1].Apply;


            // // 出口柴油馏程低限
            // Age[3, 1] = (CompOilList[0].t50 - ProdOilList[1].t50LowLimit) * PropertyList[1].Apply;
            // Age[3, 3] = (CompOilList[1].t50 - ProdOilList[1].t50LowLimit) * PropertyList[1].Apply;
            // Age[3, 5] = (CompOilList[2].t50 - ProdOilList[1].t50LowLimit) * PropertyList[1].Apply;
            // Age[3, 7] = (CompOilList[3].t50 - ProdOilList[1].t50LowLimit) * PropertyList[1].Apply;
            // Age[3, 9] = (CompOilList[4].t50 - ProdOilList[1].t50LowLimit) * PropertyList[1].Apply;
            // Age[3, 11] = (CompOilList[5].t50 - ProdOilList[1].t50LowLimit) * PropertyList[1].Apply;
            // Age[3, 13] = (CompOilList[6].t50 - ProdOilList[1].t50LowLimit) * PropertyList[1].Apply;
            // Age[3, 15] = (CompOilList[7].t50 - ProdOilList[1].t50LowLimit) * PropertyList[1].Apply;



            #endregion

            #region 多环芳烃含量低限
            // 车用柴油多环芳烃含量低限
            for(int i = 0; i < ProdOilNum; i++){
                for(int j = 0; j < ComOilNum; j++){
                    Age[ProdOilNum * 2 + i, ProdOilNum * j + i] = (CompOilList[j].suf - ProdOilList[i].sufLowLimit) * PropertyList[2].Apply;
                }
            }
            // Age[4, 0] = (CompOilList[0].suf - ProdOilList[0].sufLowLimit) * PropertyList[2].Apply;
            // Age[4, 2] = (CompOilList[1].suf - ProdOilList[0].sufLowLimit) * PropertyList[2].Apply;
            // Age[4, 4] = (CompOilList[2].suf - ProdOilList[0].sufLowLimit) * PropertyList[2].Apply;
            // Age[4, 6] = (CompOilList[3].suf - ProdOilList[0].sufLowLimit) * PropertyList[2].Apply;
            // Age[4, 8] = (CompOilList[4].suf - ProdOilList[0].sufLowLimit) * PropertyList[2].Apply;
            // Age[4, 10] = (CompOilList[5].suf - ProdOilList[0].sufLowLimit) * PropertyList[2].Apply;
            // Age[4, 12] = (CompOilList[6].suf - ProdOilList[0].sufLowLimit) * PropertyList[2].Apply;
            // Age[4, 14] = (CompOilList[7].suf - ProdOilList[0].sufLowLimit) * PropertyList[2].Apply;


            // // 出口柴油多芳烃含量低限
            // Age[5, 1] = (CompOilList[0].suf - ProdOilList[1].sufLowLimit) * PropertyList[2].Apply;
            // Age[5, 3] = (CompOilList[1].suf - ProdOilList[1].sufLowLimit) * PropertyList[2].Apply;
            // Age[5, 5] = (CompOilList[2].suf - ProdOilList[1].sufLowLimit) * PropertyList[2].Apply;
            // Age[5, 7] = (CompOilList[3].suf - ProdOilList[1].sufLowLimit) * PropertyList[2].Apply;
            // Age[5, 9] = (CompOilList[4].suf - ProdOilList[1].sufLowLimit) * PropertyList[2].Apply;
            // Age[5, 11] = (CompOilList[5].suf - ProdOilList[1].sufLowLimit) * PropertyList[2].Apply;
            // Age[5, 13] = (CompOilList[6].suf - ProdOilList[1].sufLowLimit) * PropertyList[2].Apply;
            // Age[5, 15] = (CompOilList[7].suf - ProdOilList[1].sufLowLimit) * PropertyList[2].Apply;

            #endregion

            #region 密度低限
            //车用柴油密度低限
            for(int i = 0; i < ProdOilNum; i++){
                for(int j = 0; j < ComOilNum; j++){
                    Age[ProdOilNum * 3 + i, ProdOilNum * j + i] = (CompOilList[j].den - ProdOilList[i].denLowLimit) * PropertyList[3].Apply;
                }
            }

            // Age[6, 0] = (CompOilList[0].den - ProdOilList[0].denLowLimit) * PropertyList[3].Apply;
            // Age[6, 2] = (CompOilList[1].den - ProdOilList[0].denLowLimit) * PropertyList[3].Apply;
            // Age[6, 4] = (CompOilList[2].den - ProdOilList[0].denLowLimit) * PropertyList[3].Apply;
            // Age[6, 6] = (CompOilList[3].den - ProdOilList[0].denLowLimit) * PropertyList[3].Apply;
            // Age[6, 8] = (CompOilList[4].den - ProdOilList[0].denLowLimit) * PropertyList[3].Apply;
            // Age[6, 10] = (CompOilList[5].den - ProdOilList[0].denLowLimit) * PropertyList[3].Apply;
            // Age[6, 12] = (CompOilList[6].den - ProdOilList[0].denLowLimit) * PropertyList[3].Apply;
            // Age[6, 14] = (CompOilList[7].den - ProdOilList[0].denLowLimit) * PropertyList[3].Apply;

            // // 出口柴油密度低限
            // Age[7, 1] = (CompOilList[0].den - ProdOilList[1].denLowLimit) * PropertyList[3].Apply;
            // Age[7, 3] = (CompOilList[1].den - ProdOilList[1].denLowLimit) * PropertyList[3].Apply;
            // Age[7, 5] = (CompOilList[2].den - ProdOilList[1].denLowLimit) * PropertyList[3].Apply;
            // Age[7, 7] = (CompOilList[3].den - ProdOilList[1].denLowLimit) * PropertyList[3].Apply;
            // Age[7, 9] = (CompOilList[4].den - ProdOilList[1].denLowLimit) * PropertyList[3].Apply;
            // Age[7, 11] = (CompOilList[5].den - ProdOilList[1].denLowLimit) * PropertyList[3].Apply;
            // Age[7, 13] = (CompOilList[6].den - ProdOilList[1].denLowLimit) * PropertyList[3].Apply;
            // Age[7, 15] = (CompOilList[7].den - ProdOilList[1].denLowLimit) * PropertyList[3].Apply;

            #endregion
            #endregion

            #region  在求解器里定义目标函数和约束条件，并求解
            //变量定义
            int n = iNum;
        
            //计算
            IntPtr lp;
            double[] Row = new double[iNum + 1];//因为Row[0]是常数位
            double[] Col;
            lp = lpsolve.make_lp(0, n);//变量个数

            #region 约束
            //等式约束
            //组分油产量不等式高限约束
            // for (int i = 0; i < ComOilNum; i++) //i表示矩阵的行
            // {
            //     for (int j = 0; j < iNum; j++) //j表示列
            //     {
            //         Row[j + 1] = Comp_le_up[i, j];
            //     }
            //     Row[0] = 0;//加lp的都是要算的变量
            //     lpsolve.add_constraint(lp, Row, lpsolve.lpsolve_constr_types.LE, Comp_rle_up[i]);//组分油不等式约束
            //     //第一行Comp_le，单位是t/m³，乘以需要计算得出的体积m³，等于第一个组分油的产量Comp_rl，单位为t
            // }

            // //组分油产量不等式低限约束（各组分油的用量和 = 各组分油总产量）
            // for (int i = 0; i < ComOilNum; i++) //i表示矩阵的行
            // {
            //     for (int j = 0; j < iNum; j++) //j表示列 8行16列
            //     {
            //         Row[j + 1] = Comp_le_low[i, j];
            //     }
            //     Row[0] = 0;//加lp的都是要算的变量
            //     lpsolve.add_constraint(lp, Row, lpsolve.lpsolve_constr_types.GE, Comp_rle_low[i]);//组分油不等式约束
            //     //第一行Comp_le，单位是t/m³，乘以需要计算得出的体积m³，等于第一个组分油的产量Comp_rl，单位为t
            // }

            //不等式约束
            //
            for (int i = 0; i < ProdOilNum; i++)
            {
                for (int j = 0; j < iNum; j++)
                {
                    Row[j + 1] = Prod_le[i, j];
                }
                Row[0] = 0;
                lpsolve.add_constraint(lp, Row, lpsolve.lpsolve_constr_types.EQ, Prod_rle[i]);//成品油产量等式约束
                
                //
            }

            //属性高限
            for (int i = 0; i < 4 * ProdOilNum; i++)
            {
                for (int j = 0; j < iNum; j++)
                {
                    Row[j + 1] = Ale[i, j];
                }
                Row[0] = 0;
                lpsolve.add_constraint(lp, Row, lpsolve.lpsolve_constr_types.LE, ble[i]);//LE高限
            }
 
            //属性低限
            for (int i = 0; i < 4 * ProdOilNum; i++)
            {
                for (int j = 0; j < iNum; j++)
                {
                    Row[j + 1] = Age[i, j];
                }
                Row[0] = 0;
                lpsolve.add_constraint(lp, Row, lpsolve.lpsolve_constr_types.GE, bge[i]);//GE低限
            }
 
            //库存高限

            // //流速上下限 即需要求解变量的上下限
            // for (int i = 0; i < n; i++)
            // {
            //     lpsolve.set_lowbo(lp, i + 1, LB[i]);
            //     lpsolve.set_upbo(lp, i + 1, UB[i]);
            // }

            //变量上下限
            for (int i = 0; i < n; i++)
            {
                lpsolve.set_lowbo(lp, i + 1, LB[i]);//低限
                lpsolve.set_upbo(lp, i + 1, UB[i]);//高限
            }
                     
            #endregion

            //目标函数
            for (int i = 0; i < n; i++)
            {
                Row[i + 1] = f[i];
            }
            Row[0] = 0;
            lpsolve.set_obj_fn(lp, Row);

            //求解
            lpsolve.set_minim(lp);
            lpsolve.solve(lp);
            int status = lpsolve.get_status(lp);//0或2 从solve之后，status的标志位才会变化

            #endregion

            // Col 为最优解
            Col = new double[lpsolve.get_Ncolumns(lp)]; 
            lpsolve.get_variables(lp, Col);             
            for(int i = 0; i < iNum; i++){
                Calc[i] = Col[i];
            }       
            Calc[iNum] = status;

            return Calc;
        }   
        public double[] GetRecipe3()//场景3的lpsolve求解配方
        {
            IProdOilConfig _ProdOilConfig = new ProdOilConfig(context);
            IProperty _Property = new PropertyApply(context);
            IRecipeCalc _RecipeCalc = new RecipeCalc(context);
            ICompOilConfig _CompOilConfig = new CompOilConfig(context);
            
            //context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;

            var CompOilList = _CompOilConfig.GetAllCompOilConfigList().ToList();
            var ProdOilList = _ProdOilConfig.GetAllProdOilConfigList().Where(m=>m.Apply == 1).ToList();
            var PropertyList = _Property.GetAllPropertyList().ToList();
            var CompOilConstraint = _RecipeCalc.GetRecipeCalc1().ToList();//组分油产量 罐容高低限 配方高低限 约束
            var Weight = _RecipeCalc.GetRecipeCalc2_3().Where(m=>m.Apply == 1).ToList();//多目权值,三个场景的
            var ProdOilProduct = _RecipeCalc.GetRecipeCalc3().Where(m=>m.Apply == 1).ToList();//成品油产量限制

            int ComOilNum = CompOilList.Count;//组分油个数
            int ProdOilNum = ProdOilProduct.Count;//成品油个数

            //变量约束定义
            //变量个数应为 成品油启用个数 x 组分油个数
            //int iNum = 24;//2X8是组分油1（车柴 出柴）组分油2（车柴 出柴）
            int iNum = ProdOilNum * ComOilNum;
            double[] Calc = new double[iNum + 1];//多的一位是求解标志位

            #region 目标函数定义

            // 目标函数定义
            float[] f = new float[iNum];
            for(int i = 0; i < ComOilNum; i++){//成品油的目标函数(产量最大 + 十六烷值 + 多芳烃)
                for(int j = 0; j < ProdOilNum; j++){
                    f[ProdOilNum * i + j] = -Weight[j].Weight * CompOilList[i].den / 1000 * 10000 + Weight[ProdOilNum + j].Weight * (CompOilList[i].ron - ProdOilList[j].ronLowLimit) * 1000 + Weight[ProdOilNum * 2 + j].Weight * (CompOilList[i].suf - ProdOilList[j].sufLowLimit) * 1000 ; 
                }
            }

            // f[0] = -Weight[0].Weight * CompOilList[0].den / 1000 * 10000 ;//质量产量乘以10000，质量指标乘以1000
            // f[1] = -Weight[3].Weight * CompOilList[0].den / 1000 * 10000 + Weight[1].Weight * (CompOilList[0].ron - ProdOilList[1].ronLowLimit) * 1000 - Weight[2].Weight * (CompOilList[0].suf - ProdOilList[1].sufHighLimit) * 1000 ;
            // //-CompOilList[0].den / 1000 * 10000;  一个向上卡边一个向下卡边
            // f[2] = -Weight[0].Weight * CompOilList[1].den / 1000 * 10000 ;
            // f[3] = -Weight[3].Weight * CompOilList[1].den / 1000 * 10000 + Weight[1].Weight * (CompOilList[1].ron - ProdOilList[1].ronLowLimit) * 1000 - Weight[2].Weight * (CompOilList[1].suf - ProdOilList[1].sufHighLimit) * 1000 ;
            // //-CompOilList[1].den / 1000 * 10000;
            // f[4] = -Weight[0].Weight * CompOilList[2].den / 1000 * 10000 ;
            // f[5] = -Weight[3].Weight * CompOilList[2].den / 1000 * 10000 + Weight[1].Weight * (CompOilList[2].ron - ProdOilList[1].ronLowLimit) * 1000 - Weight[2].Weight * (CompOilList[2].suf - ProdOilList[1].sufHighLimit) * 1000 ;
            // //-CompOilList[2].den / 1000 * 10000;
            // f[6] = -Weight[0].Weight * CompOilList[3].den / 1000 * 10000 ;
            // f[7] = -Weight[3].Weight * CompOilList[3].den / 1000 * 10000 + Weight[1].Weight * (CompOilList[3].ron - ProdOilList[1].ronLowLimit) * 1000 - Weight[2].Weight * (CompOilList[3].suf - ProdOilList[1].sufHighLimit) * 1000 ;
            // //-CompOilList[3].den / 1000 * 10000;
            // f[8] = -Weight[0].Weight * CompOilList[4].den / 1000 * 10000 ;
            // f[9] = -Weight[3].Weight * CompOilList[4].den / 1000 * 10000 + Weight[1].Weight * (CompOilList[4].ron - ProdOilList[1].ronLowLimit) * 1000 - Weight[2].Weight * (CompOilList[4].suf - ProdOilList[1].sufHighLimit) * 1000 ;

            // f[10] = -Weight[0].Weight * CompOilList[5].den / 1000 * 10000 ;
            // f[11] = -Weight[3].Weight * CompOilList[5].den / 1000 * 10000 + Weight[1].Weight * (CompOilList[5].ron - ProdOilList[1].ronLowLimit) * 1000 - Weight[2].Weight * (CompOilList[5].suf - ProdOilList[1].sufHighLimit) * 1000 ;

            // f[12] = -Weight[0].Weight * CompOilList[6].den / 1000 * 10000 ;
            // f[13] = -Weight[3].Weight * CompOilList[6].den / 1000 * 10000 + Weight[1].Weight * (CompOilList[6].ron - ProdOilList[1].ronLowLimit) * 1000 - Weight[2].Weight * (CompOilList[6].suf - ProdOilList[1].sufHighLimit) * 1000 ;

            // f[14] = -Weight[0].Weight * CompOilList[7].den / 1000 * 10000 ;
            // f[15] = -Weight[3].Weight * CompOilList[7].den / 1000 * 10000 + Weight[1].Weight * (CompOilList[7].ron - ProdOilList[1].ronLowLimit) * 1000 - Weight[2].Weight * (CompOilList[7].suf - ProdOilList[1].sufHighLimit) * 1000 ;
            
            // for(int i = 0; i < ComOilNum; i++){
            //     f[i + ProdOilNum * ComOilNum] = CompOilList[i].den / 1000 * CompOilList[i].Price;
            // }\

            //把数据库中的配方高低限转为二维数组
            double[,] ProdOilFlowLow = new double[ComOilNum, 4];//成品油内置个数4
            for(int i = 0; i < ComOilNum; i++){
                ProdOilFlowLow[i, 0] = CompOilList[i].gas92Low1;//第一个成品油
                ProdOilFlowLow[i, 1] = CompOilList[i].gas95Low1;//第二个成品油
                ProdOilFlowLow[i, 2] = CompOilList[i].gas98Low1;
                ProdOilFlowLow[i, 3] = CompOilList[i].gasSelfLow1;
            }

            double[,] ProdOilFlowUp = new double[ComOilNum, 4];//成品油内置个数4
            for(int i = 0; i < ComOilNum; i++){
                ProdOilFlowUp[i, 0] = CompOilList[i].gas92High1;//第一个成品油
                ProdOilFlowUp[i, 1] = CompOilList[i].gas95High1;//第二个成品油
                ProdOilFlowUp[i, 2] = CompOilList[i].gas98High1;
                ProdOilFlowUp[i, 3] = CompOilList[i].gasSelfHigh1;
            }

            #endregion

            //组分油参调流量上下限

            Double[] LB = new Double[iNum];// 变量低限
            Double[] UB = new Double[iNum];// 高限

            for(int i = 0; i < ComOilNum; i++){
                for(int j = 0; j < ProdOilNum; j++){
                    LB[ProdOilNum * i + j] = ProdOilFlowLow[i, ProdOilProduct[j].Id - 1] / CompOilList[i].den * 1000;
                    UB[ProdOilNum * i + j] = ProdOilFlowUp[i, ProdOilProduct[j].Id - 1] / CompOilList[i].den * 1000;
                }
            }

            #region 流量上下限          
            // LB[0] = CompOilList[0].gas92Low1 / CompOilList[0].den * 1000;
            // LB[1] = CompOilList[0].gas95Low1 / CompOilList[0].den * 1000;

            // LB[2] = CompOilList[1].gas92Low1 / CompOilList[1].den * 1000;
            // LB[3] = CompOilList[1].gas95Low1 / CompOilList[1].den * 1000;

            // LB[4] = CompOilList[2].gas92Low1 / CompOilList[2].den * 1000;
            // LB[5] = CompOilList[2].gas95Low1 / CompOilList[2].den * 1000;

            // LB[6] = CompOilList[3].gas92Low1 / CompOilList[3].den * 1000;
            // LB[7] = CompOilList[3].gas95Low1 / CompOilList[3].den * 1000;

            // LB[8] = CompOilList[4].gas92Low1 / CompOilList[4].den * 1000;
            // LB[9] = CompOilList[4].gas95Low1 / CompOilList[4].den * 1000;

            // LB[10] = CompOilList[5].gas92Low1 / CompOilList[5].den * 1000;
            // LB[11] = CompOilList[5].gas95Low1 / CompOilList[5].den * 1000;

            // LB[12] = CompOilList[6].gas92Low1 / CompOilList[6].den * 1000;
            // LB[13] = CompOilList[6].gas95Low1 / CompOilList[6].den * 1000;

            // LB[14] = CompOilList[7].gas92Low1 / CompOilList[7].den * 1000;
            // LB[15] = CompOilList[7].gas95Low1 / CompOilList[7].den * 1000;

            // UB[0] = CompOilList[0].gas92High1 / CompOilList[0].den * 1000; 
            // UB[1] = CompOilList[0].gas95High1 / CompOilList[0].den * 1000;

            // UB[2] = CompOilList[1].gas92High1 / CompOilList[1].den * 1000;
            // UB[3] = CompOilList[1].gas95High1 / CompOilList[1].den * 1000;

            // UB[4] = CompOilList[2].gas92High1 / CompOilList[2].den * 1000;
            // UB[5] = CompOilList[2].gas95High1 / CompOilList[2].den * 1000;

            // UB[6] = CompOilList[3].gas92High1 / CompOilList[3].den * 1000;
            // UB[7] = CompOilList[3].gas95High1 / CompOilList[3].den * 1000;

            // UB[8] = CompOilList[4].gas92High1 / CompOilList[4].den * 1000;
            // UB[9] = CompOilList[4].gas95High1 / CompOilList[4].den * 1000;

            // UB[10] = CompOilList[5].gas92High1 / CompOilList[5].den * 1000;
            // UB[11] = CompOilList[5].gas95High1 / CompOilList[5].den * 1000;

            // UB[12] = CompOilList[6].gas92High1 / CompOilList[6].den * 1000;
            // UB[13] = CompOilList[6].gas95High1 / CompOilList[6].den * 1000;

            // UB[14] = CompOilList[7].gas92High1 / CompOilList[7].den * 1000;
            // UB[15] = CompOilList[7].gas95High1 / CompOilList[7].den * 1000;
            
            #endregion

            #region 成品油不等式约束
            // 成品油产量不等式约束

            Double[,] Prod_le = new Double[ProdOilNum, iNum];//2行16列矩阵 带变量和系数的计算式
            Double[] Prod_rle = new Double[ProdOilNum];

            for(int i = 0; i < ProdOilNum; i++){
                Prod_rle[i] = ProdOilProduct[i].TotalFlow2;//成品油产量限制 
            }

            for(int i = 0; i < ProdOilNum; i++){
                for(int j = 0; j < ComOilNum; j++){
                    Prod_le[i, ProdOilNum * j + i] = CompOilList[j].den / 1000;//组分油密度，t/m³ 成品油产量限制
                }
            }

            // P1产量不等式约束 车用柴油

            // Prod_le[0, 2] = CompOilList[1].den / 1000;
            // Prod_le[0, 4] = CompOilList[2].den / 1000;
            // Prod_le[0, 6] = CompOilList[3].den / 1000;
            // Prod_le[0, 8] = CompOilList[4].den / 1000;
            // Prod_le[0, 10] = CompOilList[5].den / 1000;
            // Prod_le[0, 12] = CompOilList[6].den / 1000;
            // Prod_le[0, 14] = CompOilList[7].den / 1000;

            // // P2产量不等式约束 出口柴油
            // Prod_le[1, 1] = CompOilList[0].den / 1000;
            // Prod_le[1, 3] = CompOilList[1].den / 1000;
            // Prod_le[1, 5] = CompOilList[2].den / 1000;
            // Prod_le[1, 7] = CompOilList[3].den / 1000;
            // Prod_le[1, 9] = CompOilList[4].den / 1000;
            // Prod_le[1, 11] = CompOilList[5].den / 1000;
            // Prod_le[1, 13] = CompOilList[6].den / 1000;
            // Prod_le[1, 15] = CompOilList[7].den / 1000;

            #endregion

            #region 组分油不等式约束

            // 组分油不等式约束 有罐子

            // Double[,] Comp_le_up = new Double[ComOilNum, iNum];
            // Double[,] Comp_le_low = new Double[ComOilNum, iNum];
            // Double[] Comp_rle_low = new Double[ComOilNum];// 低限
            // Double[] Comp_rle_up = new Double[ComOilNum];// 高限

            // for( int i = 0; i < ComOilNum; i++)
            // {
            //     Comp_rle_low[i] = CompOilConstraint[i].VolumeLow - CompOilConstraint[i].IniVolume;           
            //     Comp_rle_up[i] = CompOilConstraint[i].VolumeHigh - CompOilConstraint[i].IniVolume;
            // }
            // //下限约束
            // for(int i = 0; i < ComOilNum; i++){
            //     for(int j = 0; j < ProdOilNum; j++){
            //         Comp_le_low[i, ProdOilNum * i + j] = - CompOilList[i].den / 1000;
            //     }
            //     Comp_le_low[i, iNum_Uncontain + i] = CompOilList[i].den / 1000;
            // }
            // //上限约束
            // for(int i = 0; i < ComOilNum; i++){
            //     Comp_le_up[i, iNum_Uncontain + i] = CompOilList[i].den / 1000;
            // }           

            // // F2产量等式约束 第二个组分油
            // Comp_le[1, 2] = CompOilList[1].den / 1000;第一个组分油的车柴变量
            // Comp_le[1, 3] = CompOilList[1].den / 1000;第一个组分油的出柴变量
            // Comp_le[1, 17] = CompOilList[1].den / 1000; 第一个组分油产量变量

            // // F3产量等式约束
            // Comp_le[2, 4] = CompOilList[2].den / 1000;
            // Comp_le[2, 5] = CompOilList[2].den / 1000;
            // Comp_le[2, 18] = CompOilList[2].den / 1000;

            // // F4产量等式约束
            // Comp_le[3, 6] = CompOilList[3].den / 1000;
            // Comp_le[3, 7] = CompOilList[3].den / 1000;
            // Comp_le[3, 19] = CompOilList[3].den / 1000;

            // // F5产量等式约束
            // Comp_le[4, 8] = CompOilList[4].den / 1000;
            // Comp_le[4, 9] = CompOilList[4].den / 1000;
            // Comp_le[4, 20] = CompOilList[4].den / 1000;

            // // F6产量等式约束
            // Comp_le[5, 10] = CompOilList[5].den / 1000;
            // Comp_le[5, 11] = CompOilList[5].den / 1000;
            // Comp_le[5, 21] = CompOilList[5].den / 1000;

            // // F7产量等式约束
            // Comp_le[6, 12] = CompOilList[6].den / 1000;
            // Comp_le[6, 13] = CompOilList[6].den / 1000;
            // Comp_le[6, 22] = CompOilList[6].den / 1000;

            // // F8产量等式约束 第八个组分油
            // Comp_le[7, 14] = CompOilList[7].den / 1000;
            // Comp_le[7, 15] = CompOilList[7].den / 1000;
            // Comp_le[7, 23] = CompOilList[7].den / 1000;

            #endregion

            #region 属性高限约束

            // 属性不等式约束 八个组分油 8行16列
            Double[,] Ale = new Double[4 * ProdOilNum, iNum];//这个的行应该是成品油个数乘以属性个数
            Double[] ble = new Double[4 * ProdOilNum];

            #region 十六烷值指数高限

            for(int i = 0; i < ProdOilNum; i++){
                for(int j = 0; j < ComOilNum; j++){
                    Ale[i, ProdOilNum * j + i] = (CompOilList[j].ron - ProdOilList[i].ronHighLimit) * PropertyList[0].Apply;
                }
            }

            // 车柴十六烷值指数高限 八个组分油

            // Ale[0, 2] = (CompOilList[1].ron - ProdOilList[0].ronHighLimit) * PropertyList[0].Apply;
            // Ale[0, 4] = (CompOilList[2].ron - ProdOilList[0].ronHighLimit) * PropertyList[0].Apply;
            // Ale[0, 6] = (CompOilList[3].ron - ProdOilList[0].ronHighLimit) * PropertyList[0].Apply;
            // Ale[0, 8] = (CompOilList[4].ron - ProdOilList[0].ronHighLimit) * PropertyList[0].Apply;
            // Ale[0, 10] = (CompOilList[5].ron - ProdOilList[0].ronHighLimit) * PropertyList[0].Apply;
            // Ale[0, 12] = (CompOilList[6].ron - ProdOilList[0].ronHighLimit) * PropertyList[0].Apply;
            // Ale[0, 14] = (CompOilList[7].ron - ProdOilList[0].ronHighLimit) * PropertyList[0].Apply;


            // //出柴十六烷值指数高限 
            // Ale[1, 1] = (CompOilList[0].ron - ProdOilList[1].ronHighLimit) * PropertyList[0].Apply;
            // Ale[1, 3] = (CompOilList[1].ron - ProdOilList[1].ronHighLimit) * PropertyList[0].Apply;
            // Ale[1, 5] = (CompOilList[2].ron - ProdOilList[1].ronHighLimit) * PropertyList[0].Apply;
            // Ale[1, 7] = (CompOilList[3].ron - ProdOilList[1].ronHighLimit) * PropertyList[0].Apply;
            // Ale[1, 9] = (CompOilList[4].ron - ProdOilList[1].ronHighLimit) * PropertyList[0].Apply;
            // Ale[1, 11] = (CompOilList[5].ron - ProdOilList[1].ronHighLimit) * PropertyList[0].Apply;
            // Ale[1, 13] = (CompOilList[6].ron - ProdOilList[1].ronHighLimit) * PropertyList[0].Apply;
            // Ale[1, 15] = (CompOilList[7].ron - ProdOilList[1].ronHighLimit) * PropertyList[0].Apply;

            #endregion

            #region 馏程高限

            for(int i = 0; i < ProdOilNum; i++){
                for(int j = 0; j < ComOilNum; j++){
                    Ale[ProdOilNum + i, ProdOilNum * j + i] = (CompOilList[j].t50 - ProdOilList[i].t50HighLimit) * PropertyList[1].Apply;
                }
            }

            // Ale[2, 0] = (CompOilList[0].t50 - ProdOilList[0].t50HighLimit) * PropertyList[1].Apply;
            // Ale[2, 2] = (CompOilList[1].t50 - ProdOilList[0].t50HighLimit) * PropertyList[1].Apply;
            // Ale[2, 4] = (CompOilList[2].t50 - ProdOilList[0].t50HighLimit) * PropertyList[1].Apply;
            // Ale[2, 6] = (CompOilList[3].t50 - ProdOilList[0].t50HighLimit) * PropertyList[1].Apply;
            // Ale[2, 8] = (CompOilList[4].t50 - ProdOilList[0].t50HighLimit) * PropertyList[1].Apply;
            // Ale[2, 10] = (CompOilList[5].t50 - ProdOilList[0].t50HighLimit) * PropertyList[1].Apply;
            // Ale[2, 12] = (CompOilList[6].t50 - ProdOilList[0].t50HighLimit) * PropertyList[1].Apply;
            // Ale[2, 14] = (CompOilList[7].t50 - ProdOilList[0].t50HighLimit) * PropertyList[1].Apply;


            // //出口柴油馏程高限
            // Ale[3, 1] = (CompOilList[0].t50 - ProdOilList[1].t50HighLimit) * PropertyList[1].Apply;
            // Ale[3, 3] = (CompOilList[1].t50 - ProdOilList[1].t50HighLimit) * PropertyList[1].Apply;
            // Ale[3, 5] = (CompOilList[2].t50 - ProdOilList[1].t50HighLimit) * PropertyList[1].Apply;
            // Ale[3, 7] = (CompOilList[3].t50 - ProdOilList[1].t50HighLimit) * PropertyList[1].Apply;
            // Ale[3, 9] = (CompOilList[4].t50 - ProdOilList[1].t50HighLimit) * PropertyList[1].Apply;
            // Ale[3, 11] = (CompOilList[5].t50 - ProdOilList[1].t50HighLimit) * PropertyList[1].Apply;
            // Ale[3, 13] = (CompOilList[6].t50 - ProdOilList[1].t50HighLimit) * PropertyList[1].Apply;
            // Ale[3, 15] = (CompOilList[7].t50 - ProdOilList[1].t50HighLimit) * PropertyList[1].Apply;

            #endregion

            #region 多环芳烃含量高限

            for(int i = 0; i < ProdOilNum; i++){
                for(int j = 0; j < ComOilNum; j++){
                    Ale[ProdOilNum * 2 + i, ProdOilNum * j + i] = (CompOilList[j].suf - ProdOilList[i].sufHighLimit) * PropertyList[2].Apply;
                }
            }
            
            // Ale[4, 0] = (CompOilList[0].suf - ProdOilList[0].sufHighLimit) * PropertyList[2].Apply;
            // Ale[4, 2] = (CompOilList[1].suf - ProdOilList[0].sufHighLimit) * PropertyList[2].Apply;
            // Ale[4, 4] = (CompOilList[2].suf - ProdOilList[0].sufHighLimit) * PropertyList[2].Apply;
            // Ale[4, 6] = (CompOilList[3].suf - ProdOilList[0].sufHighLimit) * PropertyList[2].Apply;
            // Ale[4, 8] = (CompOilList[4].suf - ProdOilList[0].sufHighLimit) * PropertyList[2].Apply;
            // Ale[4, 10] = (CompOilList[5].suf - ProdOilList[0].sufHighLimit) * PropertyList[2].Apply;
            // Ale[4, 12] = (CompOilList[6].suf - ProdOilList[0].sufHighLimit) * PropertyList[2].Apply;
            // Ale[4, 14] = (CompOilList[7].suf - ProdOilList[0].sufHighLimit) * PropertyList[2].Apply;

            // // 出口柴油多环芳烃含量高限
            // Ale[5, 1] = (CompOilList[0].suf - ProdOilList[1].sufHighLimit) * PropertyList[2].Apply;
            // Ale[5, 3] = (CompOilList[1].suf - ProdOilList[1].sufHighLimit) * PropertyList[2].Apply;
            // Ale[5, 5] = (CompOilList[2].suf - ProdOilList[1].sufHighLimit) * PropertyList[2].Apply;
            // Ale[5, 7] = (CompOilList[3].suf - ProdOilList[1].sufHighLimit) * PropertyList[2].Apply;
            // Ale[5, 9] = (CompOilList[4].suf - ProdOilList[1].sufHighLimit) * PropertyList[2].Apply;
            // Ale[5, 11] = (CompOilList[5].suf - ProdOilList[1].sufHighLimit) * PropertyList[2].Apply;
            // Ale[5, 13] = (CompOilList[6].suf - ProdOilList[1].sufHighLimit) * PropertyList[2].Apply;
            // Ale[5, 15] = (CompOilList[7].suf - ProdOilList[1].sufHighLimit) * PropertyList[2].Apply;


            #endregion
            #region 密度高限

            for(int i = 0; i < ProdOilNum; i++){
                for(int j = 0; j < ComOilNum; j++){
                    Ale[ProdOilNum * 3 + i, ProdOilNum * j + i] = (CompOilList[j].den - ProdOilList[i].denHighLimit) * PropertyList[3].Apply;
                }
            }

            // Ale[6, 0] = (CompOilList[0].den - ProdOilList[0].denHighLimit) * PropertyList[3].Apply;
            // Ale[6, 2] = (CompOilList[1].den - ProdOilList[0].denHighLimit) * PropertyList[3].Apply;
            // Ale[6, 4] = (CompOilList[2].den - ProdOilList[0].denHighLimit) * PropertyList[3].Apply;
            // Ale[6, 6] = (CompOilList[3].den - ProdOilList[0].denHighLimit) * PropertyList[3].Apply;
            // Ale[6, 8] = (CompOilList[4].den - ProdOilList[0].denHighLimit) * PropertyList[3].Apply;
            // Ale[6, 10] = (CompOilList[5].den - ProdOilList[0].denHighLimit) * PropertyList[3].Apply;
            // Ale[6, 12] = (CompOilList[6].den - ProdOilList[0].denHighLimit) * PropertyList[3].Apply;
            // Ale[6, 14] = (CompOilList[7].den - ProdOilList[0].denHighLimit) * PropertyList[3].Apply;


            // //出口柴油密度高限
            // Ale[7, 1] = (CompOilList[0].den - ProdOilList[1].denHighLimit) * PropertyList[3].Apply;
            // Ale[7, 3] = (CompOilList[1].den - ProdOilList[1].denHighLimit) * PropertyList[3].Apply;
            // Ale[7, 5] = (CompOilList[2].den - ProdOilList[1].denHighLimit) * PropertyList[3].Apply;
            // Ale[7, 7] = (CompOilList[3].den - ProdOilList[1].denHighLimit) * PropertyList[3].Apply;
            // Ale[7, 9] = (CompOilList[4].den - ProdOilList[1].denHighLimit) * PropertyList[3].Apply;
            // Ale[7, 11] = (CompOilList[5].den - ProdOilList[1].denHighLimit) * PropertyList[3].Apply;
            // Ale[7, 13] = (CompOilList[6].den - ProdOilList[1].denHighLimit) * PropertyList[3].Apply;
            // Ale[7, 15] = (CompOilList[7].den - ProdOilList[1].denHighLimit) * PropertyList[3].Apply;


            #endregion

            #endregion

            #region  属性低限约束

            // 不等式约束定义
            Double[,] Age = new Double[4 * ProdOilNum, iNum];//Age高限，Age低限
            Double[] bge = new Double[4 * ProdOilNum];
            #region 十六烷值指数

            for(int i = 0; i < ProdOilNum; i++){
                for(int j = 0; j < ComOilNum; j++){
                    Age[i, ProdOilNum * j + i] = (CompOilList[j].ron - ProdOilList[i].ronLowLimit) * PropertyList[0].Apply;
                }
            }

            // Age[0, 0] = (CompOilList[0].ron - ProdOilList[0].ronLowLimit) * PropertyList[0].Apply;
            // Age[0, 2] = (CompOilList[1].ron - ProdOilList[0].ronLowLimit) * PropertyList[0].Apply;
            // Age[0, 4] = (CompOilList[2].ron - ProdOilList[0].ronLowLimit) * PropertyList[0].Apply;
            // Age[0, 6] = (CompOilList[3].ron - ProdOilList[0].ronLowLimit) * PropertyList[0].Apply;
            // Age[0, 8] = (CompOilList[4].ron - ProdOilList[0].ronLowLimit) * PropertyList[0].Apply;
            // Age[0, 10] = (CompOilList[5].ron - ProdOilList[0].ronLowLimit) * PropertyList[0].Apply;
            // Age[0, 12] = (CompOilList[6].ron - ProdOilList[0].ronLowLimit) * PropertyList[0].Apply;
            // Age[0, 14] = (CompOilList[7].ron - ProdOilList[0].ronLowLimit) * PropertyList[0].Apply;

            // //出柴十六烷值指数低限
            // Age[1, 1] = (CompOilList[0].ron - ProdOilList[1].ronLowLimit) * PropertyList[0].Apply;
            // Age[1, 3] = (CompOilList[1].ron - ProdOilList[1].ronLowLimit) * PropertyList[0].Apply;
            // Age[1, 5] = (CompOilList[2].ron - ProdOilList[1].ronLowLimit) * PropertyList[0].Apply;
            // Age[1, 7] = (CompOilList[3].ron - ProdOilList[1].ronLowLimit) * PropertyList[0].Apply;
            // Age[1, 9] = (CompOilList[4].ron - ProdOilList[1].ronLowLimit) * PropertyList[0].Apply;
            // Age[1, 11] = (CompOilList[5].ron - ProdOilList[1].ronLowLimit) * PropertyList[0].Apply;
            // Age[1, 13] = (CompOilList[6].ron - ProdOilList[1].ronLowLimit) * PropertyList[0].Apply;
            // Age[1, 15] = (CompOilList[7].ron - ProdOilList[1].ronLowLimit) * PropertyList[0].Apply;


            #endregion

            #region 馏程低限

            for(int i = 0; i < ProdOilNum; i++){
                for(int j = 0; j < ComOilNum; j++){
                    Age[ProdOilNum + i, ProdOilNum * j + i] = (CompOilList[j].t50 - ProdOilList[i].t50LowLimit) * PropertyList[1].Apply;
                }
            }

            // Age[2, 0] = (CompOilList[0].t50 - ProdOilList[0].t50LowLimit) * PropertyList[1].Apply;
            // Age[2, 2] = (CompOilList[1].t50 - ProdOilList[0].t50LowLimit) * PropertyList[1].Apply;
            // Age[2, 4] = (CompOilList[2].t50 - ProdOilList[0].t50LowLimit) * PropertyList[1].Apply;
            // Age[2, 6] = (CompOilList[3].t50 - ProdOilList[0].t50LowLimit) * PropertyList[1].Apply;
            // Age[2, 8] = (CompOilList[4].t50 - ProdOilList[0].t50LowLimit) * PropertyList[1].Apply;
            // Age[2, 10] = (CompOilList[5].t50 - ProdOilList[0].t50LowLimit) * PropertyList[1].Apply;
            // Age[2, 12] = (CompOilList[6].t50 - ProdOilList[0].t50LowLimit) * PropertyList[1].Apply;
            // Age[2, 14] = (CompOilList[7].t50 - ProdOilList[0].t50LowLimit) * PropertyList[1].Apply;


            // // 出口柴油馏程低限
            // Age[3, 1] = (CompOilList[0].t50 - ProdOilList[1].t50LowLimit) * PropertyList[1].Apply;
            // Age[3, 3] = (CompOilList[1].t50 - ProdOilList[1].t50LowLimit) * PropertyList[1].Apply;
            // Age[3, 5] = (CompOilList[2].t50 - ProdOilList[1].t50LowLimit) * PropertyList[1].Apply;
            // Age[3, 7] = (CompOilList[3].t50 - ProdOilList[1].t50LowLimit) * PropertyList[1].Apply;
            // Age[3, 9] = (CompOilList[4].t50 - ProdOilList[1].t50LowLimit) * PropertyList[1].Apply;
            // Age[3, 11] = (CompOilList[5].t50 - ProdOilList[1].t50LowLimit) * PropertyList[1].Apply;
            // Age[3, 13] = (CompOilList[6].t50 - ProdOilList[1].t50LowLimit) * PropertyList[1].Apply;
            // Age[3, 15] = (CompOilList[7].t50 - ProdOilList[1].t50LowLimit) * PropertyList[1].Apply;



            #endregion

            #region 多环芳烃含量低限
            // 车用柴油多环芳烃含量低限
            for(int i = 0; i < ProdOilNum; i++){
                for(int j = 0; j < ComOilNum; j++){
                    Age[ProdOilNum * 2 + i, ProdOilNum * j + i] = (CompOilList[j].suf - ProdOilList[i].sufLowLimit) * PropertyList[2].Apply;
                }
            }
            // Age[4, 0] = (CompOilList[0].suf - ProdOilList[0].sufLowLimit) * PropertyList[2].Apply;
            // Age[4, 2] = (CompOilList[1].suf - ProdOilList[0].sufLowLimit) * PropertyList[2].Apply;
            // Age[4, 4] = (CompOilList[2].suf - ProdOilList[0].sufLowLimit) * PropertyList[2].Apply;
            // Age[4, 6] = (CompOilList[3].suf - ProdOilList[0].sufLowLimit) * PropertyList[2].Apply;
            // Age[4, 8] = (CompOilList[4].suf - ProdOilList[0].sufLowLimit) * PropertyList[2].Apply;
            // Age[4, 10] = (CompOilList[5].suf - ProdOilList[0].sufLowLimit) * PropertyList[2].Apply;
            // Age[4, 12] = (CompOilList[6].suf - ProdOilList[0].sufLowLimit) * PropertyList[2].Apply;
            // Age[4, 14] = (CompOilList[7].suf - ProdOilList[0].sufLowLimit) * PropertyList[2].Apply;


            // // 出口柴油多芳烃含量低限
            // Age[5, 1] = (CompOilList[0].suf - ProdOilList[1].sufLowLimit) * PropertyList[2].Apply;
            // Age[5, 3] = (CompOilList[1].suf - ProdOilList[1].sufLowLimit) * PropertyList[2].Apply;
            // Age[5, 5] = (CompOilList[2].suf - ProdOilList[1].sufLowLimit) * PropertyList[2].Apply;
            // Age[5, 7] = (CompOilList[3].suf - ProdOilList[1].sufLowLimit) * PropertyList[2].Apply;
            // Age[5, 9] = (CompOilList[4].suf - ProdOilList[1].sufLowLimit) * PropertyList[2].Apply;
            // Age[5, 11] = (CompOilList[5].suf - ProdOilList[1].sufLowLimit) * PropertyList[2].Apply;
            // Age[5, 13] = (CompOilList[6].suf - ProdOilList[1].sufLowLimit) * PropertyList[2].Apply;
            // Age[5, 15] = (CompOilList[7].suf - ProdOilList[1].sufLowLimit) * PropertyList[2].Apply;

            #endregion

            #region 密度低限
            //车用柴油密度低限
            for(int i = 0; i < ProdOilNum; i++){
                for(int j = 0; j < ComOilNum; j++){
                    Age[ProdOilNum * 3 + i, ProdOilNum * j + i] = (CompOilList[j].den - ProdOilList[i].denLowLimit) * PropertyList[3].Apply;
                }
            }

            // Age[6, 0] = (CompOilList[0].den - ProdOilList[0].denLowLimit) * PropertyList[3].Apply;
            // Age[6, 2] = (CompOilList[1].den - ProdOilList[0].denLowLimit) * PropertyList[3].Apply;
            // Age[6, 4] = (CompOilList[2].den - ProdOilList[0].denLowLimit) * PropertyList[3].Apply;
            // Age[6, 6] = (CompOilList[3].den - ProdOilList[0].denLowLimit) * PropertyList[3].Apply;
            // Age[6, 8] = (CompOilList[4].den - ProdOilList[0].denLowLimit) * PropertyList[3].Apply;
            // Age[6, 10] = (CompOilList[5].den - ProdOilList[0].denLowLimit) * PropertyList[3].Apply;
            // Age[6, 12] = (CompOilList[6].den - ProdOilList[0].denLowLimit) * PropertyList[3].Apply;
            // Age[6, 14] = (CompOilList[7].den - ProdOilList[0].denLowLimit) * PropertyList[3].Apply;

            // // 出口柴油密度低限
            // Age[7, 1] = (CompOilList[0].den - ProdOilList[1].denLowLimit) * PropertyList[3].Apply;
            // Age[7, 3] = (CompOilList[1].den - ProdOilList[1].denLowLimit) * PropertyList[3].Apply;
            // Age[7, 5] = (CompOilList[2].den - ProdOilList[1].denLowLimit) * PropertyList[3].Apply;
            // Age[7, 7] = (CompOilList[3].den - ProdOilList[1].denLowLimit) * PropertyList[3].Apply;
            // Age[7, 9] = (CompOilList[4].den - ProdOilList[1].denLowLimit) * PropertyList[3].Apply;
            // Age[7, 11] = (CompOilList[5].den - ProdOilList[1].denLowLimit) * PropertyList[3].Apply;
            // Age[7, 13] = (CompOilList[6].den - ProdOilList[1].denLowLimit) * PropertyList[3].Apply;
            // Age[7, 15] = (CompOilList[7].den - ProdOilList[1].denLowLimit) * PropertyList[3].Apply;

            #endregion
            #endregion

            #region  在求解器里定义目标函数和约束条件，并求解
            //变量定义
            int n = iNum;
        
            //计算
            IntPtr lp;
            double[] Row = new double[iNum + 1];//因为Row[0]是常数位
            double[] Col;
            lp = lpsolve.make_lp(0, n);//变量个数

            #region 约束
            //等式约束
            //组分油产量不等式高限约束
            // for (int i = 0; i < ComOilNum; i++) //i表示矩阵的行
            // {
            //     for (int j = 0; j < iNum; j++) //j表示列
            //     {
            //         Row[j + 1] = Comp_le_up[i, j];
            //     }
            //     Row[0] = 0;//加lp的都是要算的变量
            //     lpsolve.add_constraint(lp, Row, lpsolve.lpsolve_constr_types.LE, Comp_rle_up[i]);//组分油不等式约束
            //     //第一行Comp_le，单位是t/m³，乘以需要计算得出的体积m³，等于第一个组分油的产量Comp_rl，单位为t
            // }

            // //组分油产量不等式低限约束（各组分油的用量和 = 各组分油总产量）
            // for (int i = 0; i < ComOilNum; i++) //i表示矩阵的行
            // {
            //     for (int j = 0; j < iNum; j++) //j表示列 8行16列
            //     {
            //         Row[j + 1] = Comp_le_low[i, j];
            //     }
            //     Row[0] = 0;//加lp的都是要算的变量
            //     lpsolve.add_constraint(lp, Row, lpsolve.lpsolve_constr_types.GE, Comp_rle_low[i]);//组分油不等式约束
            //     //第一行Comp_le，单位是t/m³，乘以需要计算得出的体积m³，等于第一个组分油的产量Comp_rl，单位为t
            // }

            //不等式约束
            //
            for (int i = 0; i < ProdOilNum; i++)
            {
                for (int j = 0; j < iNum; j++)
                {
                    Row[j + 1] = Prod_le[i, j];
                }
                Row[0] = 0;
                lpsolve.add_constraint(lp, Row, lpsolve.lpsolve_constr_types.EQ, Prod_rle[i]);//成品油产量等式约束
                
                //
            }
 

            //属性高限
            for (int i = 0; i < 4 * ProdOilNum; i++)
            {
                for (int j = 0; j < iNum; j++)
                {
                    Row[j + 1] = Ale[i, j];
                }
                Row[0] = 0;
                lpsolve.add_constraint(lp, Row, lpsolve.lpsolve_constr_types.LE, ble[i]);//LE高限
            }


            //属性低限
            for (int i = 0; i < 4 * ProdOilNum; i++)
            {
                for (int j = 0; j < iNum; j++)
                {
                    Row[j + 1] = Age[i, j];
                }
                Row[0] = 0;
                lpsolve.add_constraint(lp, Row, lpsolve.lpsolve_constr_types.GE, bge[i]);//GE低限
            }
 
            //库存高限

            // //流速上下限 即需要求解变量的上下限
            // for (int i = 0; i < n; i++)
            // {
            //     lpsolve.set_lowbo(lp, i + 1, LB[i]);
            //     lpsolve.set_upbo(lp, i + 1, UB[i]);
            // }

            //变量上下限
            for (int i = 0; i < n; i++)
            {
                lpsolve.set_lowbo(lp, i + 1, LB[i]);//低限
                lpsolve.set_upbo(lp, i + 1, UB[i]);//高限
            }
                     
            #endregion

            //目标函数
            for (int i = 0; i < n; i++)
            {
                Row[i + 1] = f[i];
            }
            Row[0] = 0;
            lpsolve.set_obj_fn(lp, Row);

            //求解
            lpsolve.set_minim(lp);
            lpsolve.solve(lp);
            int status = lpsolve.get_status(lp);//0或2 从solve之后，status的标志位才会变化

            #endregion

            // Col 为最优解
            Col = new double[lpsolve.get_Ncolumns(lp)]; 
            lpsolve.get_variables(lp, Col);             
            for(int i = 0; i < iNum; i++){
                Calc[i] = Col[i];
            }       
            Calc[iNum] = status;
            return Calc;
        }            
        public IEnumerable<GasRecipecalc_1Res_1> GetRecipecalc_1Res_ComOilSugProduct()//场景1 计算结果：组分油产量分配（组分油建议产量）
        {
            IProdOilConfig _ProdOilConfig = new ProdOilConfig(context);
            ICompOilConfig _CompOilConfig = new CompOilConfig(context);
    
            var CompOilList = _CompOilConfig.GetAllCompOilConfigList().ToList();
            var ProdOilList = _ProdOilConfig.GetAllProdOilConfigList().Where(m=>m.Apply == 1).ToList();

            int ComOilNum = CompOilList.Count;//组分油个数
            int ProdOilNum = ProdOilList.Count;//成品油个数

            //ComOilName//组分油名称
            //ComOilProduct//组分油产量

            List<GasRecipecalc_1Res_1> ResultList = new List<GasRecipecalc_1Res_1>();//新建一个List用来append的,返回的是list形式
            //调用的时候，list[0]就是append的第一个对象，list[1]就是第二个对象，每个对象对应model中的数据类型
            for(int i = 0; i < CompOilList.Count; i++){
                GasRecipecalc_1Res_1 Result = new GasRecipecalc_1Res_1();//
                Result.ComOilName = CompOilList[i].ComOilName;
                Result.ComOilProduct = (float)Math.Round(GetRecipe1()[ComOilNum * ProdOilNum + i]);//车柴总产量(质量)
                ResultList.Add(Result);
            }

            return ResultList;

        }
        public IEnumerable<GasRecipecalc_1Res_2> GetRecipecalc_1Res_ProdOilProduct()//场景1 计算结果：成品油质量产量
        {
            IProdOilConfig _ProdOilConfig = new ProdOilConfig(context);
            ICompOilConfig _CompOilConfig = new CompOilConfig(context);

            var CompOilList = _CompOilConfig.GetAllCompOilConfigList().ToList();
            var ProdOilList = _ProdOilConfig.GetAllProdOilConfigList().Where(m=>m.Apply == 1).ToList();

            int ComOilNum = CompOilList.Count;//组分油个数
            int ProdOilNum = ProdOilList.Count;//成品油个数

            // public string? ComOilName//组分油名称
            // gas92Product//车柴质量产量
            // gas95Product//出柴质量产量
            // gas98Product//备用成品油1质量产量
            // gasSelfProduct//备用成品油2质量产量
            List<GasRecipecalc_1Res_2> ResultList = new List<GasRecipecalc_1Res_2>();//新建一个List用来append的,返回的是list形式

            for(int i = 0; i < ComOilNum; i++){
                GasRecipecalc_1Res_2 Result = new GasRecipecalc_1Res_2();
                Result.ComOilName = CompOilList[i].ComOilName;
                for(int j = 0; j < ProdOilNum; j++){
                    if(ProdOilList[j].Id == 1){
                        Result.gas92Product = (float)Math.Round(GetRecipe1()[ProdOilNum * i + j] * CompOilList[i].den / 1000, 2);
                    }

                    if(ProdOilList[j].Id == 2){
                        Result.gas95Product = (float)Math.Round(GetRecipe1()[ProdOilNum * i + j] * CompOilList[i].den / 1000, 2);     
                    }

                    if(ProdOilList[j].Id == 3){
                        Result.gas98Product = (float)Math.Round(GetRecipe1()[ProdOilNum * i + j] * CompOilList[i].den / 1000, 2);
                    }

                    if(ProdOilList[j].Id == 4){
                        Result.gasSelfProduct = (float)Math.Round(GetRecipe1()[ProdOilNum * i + j] * CompOilList[i].den / 1000, 2);
                    }

                }
                ResultList.Add(Result);
            }


            //其实这个percent是质量配方，后来要改
            return ResultList;

        }
        public IEnumerable<GasRecipecalc_1Res_3> GetRecipecalc_1Res_Recipe()//场景1 计算结果：成品油优化配方
        {
            IProdOilConfig _ProdOilConfig = new ProdOilConfig(context);
            IProperty _Property = new PropertyApply(context);
            ICompOilConfig _CompOilConfig = new CompOilConfig(context);

            var CompOilList = _CompOilConfig.GetAllCompOilConfigList().ToList();
            var ProdOilList = _ProdOilConfig.GetAllProdOilConfigList().Where(m=>m.Apply == 1).ToList();
            var PropertyList = _Property.GetAllPropertyList().ToList();

            int ComOilNum = CompOilList.Count;//组分油个数
            int ProdOilNum = ProdOilList.Count;//成品油个数

            var prodMassProduct = GetRecipecalc_1Res_ProdOilProduct().ToList();//成品油质量产量

            double Sum_Weight1 = 0.0000000001;//防止分母为0
            double Sum_Weight2 = 0.0000000001;
            double Sum_Weight3 = 0.0000000001;
            double Sum_Weight4 = 0.0000000001;

            double[] percent1 = new double[ComOilNum];//第一个成品油的配方
            double[] percent2 = new double[ComOilNum];//第二个成品油的配方
            double[] percent3 = new double[ComOilNum];//第三个成品油的配方
            double[] percent4 = new double[ComOilNum];//第四个成品油的配方

            for(int i = 0; i < prodMassProduct.Count; i++){//此循环是组分油个数
                Sum_Weight1 = Sum_Weight1 + prodMassProduct[i].gas92Product;//第一个成品油的总产量
                Sum_Weight2 = Sum_Weight2 + prodMassProduct[i].gas95Product;
                Sum_Weight3 = Sum_Weight3 + prodMassProduct[i].gas98Product;
                Sum_Weight4 = Sum_Weight4 + prodMassProduct[i].gasSelfProduct;
            }

            for(int i = 0; i < prodMassProduct.Count; i++){//此循环是组分油个数
                percent1[i] = prodMassProduct[i].gas92Product / Sum_Weight1;//第一个成品油的配方(基于质量的配方)
                percent2[i] = prodMassProduct[i].gas95Product / Sum_Weight2;
                percent3[i] = prodMassProduct[i].gas98Product / Sum_Weight3;
                percent4[i] = prodMassProduct[i].gasSelfProduct / Sum_Weight4;
            }

            List<GasRecipecalc_1Res_3> ResultList = new List<GasRecipecalc_1Res_3>();//新建一个List用来append的,返回的是list形式

            for(int i = 0; i < ComOilNum; i++){
                GasRecipecalc_1Res_3 Result = new GasRecipecalc_1Res_3();
                Result.ComOilName = CompOilList[i].ComOilName;
                for(int j = 0; j < ProdOilNum; j++){
                    if(ProdOilList[j].Id == 1){
                        Result.gas92Recipe = (float)Math.Round(percent1[i] * 100, 2);//保留完三位小数，换算成百分比之后就是一位小数
                    }

                    if(ProdOilList[j].Id == 2){
                        Result.gas95Recipe = (float)Math.Round(percent2[i] * 100, 2);     
                    }

                    if(ProdOilList[j].Id == 3){
                        Result.gas98Recipe= (float)Math.Round(percent3[i] * 100, 2);
                    }

                    if(ProdOilList[j].Id == 4){
                        Result.gasSelfRecipe = (float)Math.Round(percent4[i] * 100, 2);
                    }
                }
                ResultList.Add(Result);
            }
            return ResultList;
        }
        public IEnumerable<GasRecipecalc_1Res_4> GetRecipecalc_1Res_Property()//场景1 计算结果：成品油属性
        {
            IProdOilConfig _ProdOilConfig = new ProdOilConfig(context);
            IProperty _Property = new PropertyApply(context);
            ICompOilConfig _CompOilConfig = new CompOilConfig(context);

            var CompOilList = _CompOilConfig.GetAllCompOilConfigList().ToList();
            var ProdOilList = _ProdOilConfig.GetAllProdOilConfigList().Where(m=>m.Apply == 1).ToList();
            var PropertyList = _Property.GetAllPropertyList().ToList();

            int ComOilNum = CompOilList.Count;//组分油个数
            int ProdOilNum = ProdOilList.Count;//成品油个数

            double[,] prodVolProduct = new double[ComOilNum, 4];//成品油体积产量(组分油个数为行，成品油内置个数为列)

            double Sum_Weight1 = 0.0000000001;//防止分母为0
            double Sum_Weight2 = 0.0000000001;
            double Sum_Weight3 = 0.0000000001;
            double Sum_Weight4 = 0.0000000001;

            double[,] percent = new double[ComOilNum, 4];//第一个成品油的配方(基于体积)

            for(int i = 0; i < ComOilNum; i++){
                for(int j = 0; j < ProdOilNum; j++){
                    prodVolProduct[i, ProdOilList[j].Id - 1] = (float)GetRecipe1()[ProdOilNum * i + j];//第i个组分油的体积产量
                }
            }

            for(int i = 0; i < ComOilNum; i++){//此循环是组分油个数
                    Sum_Weight1 = Sum_Weight1 + prodVolProduct[i, 0];//第一个成品油的总产量
                    Sum_Weight2 = Sum_Weight2 + prodVolProduct[i, 1];
                    Sum_Weight3 = Sum_Weight3 + prodVolProduct[i, 2];
                    Sum_Weight4 = Sum_Weight4 + prodVolProduct[i, 3];
            }

            for(int i = 0; i < ComOilNum; i++){//此循环是组分油个数
                percent[i, 0] = prodVolProduct[i, 0] / Sum_Weight1;//第一个成品油的配方(体积)
                percent[i, 1] = prodVolProduct[i, 1] / Sum_Weight2;
                percent[i, 2] = prodVolProduct[i, 2] / Sum_Weight3;
                percent[i, 3] = prodVolProduct[i, 3] / Sum_Weight4;
            }

            // ProdOilName //成品油名称
            // Prodron//成品油十六烷值  
            // ronLowLimit//成品油十六烷值低限
            // ronHighLimit//成品油十六烷值高限
            // Prodt50
            // t50LowLimit 
            // t50HighLimit 
            // Prodsuf
            // sufLowLimit 
            // sufHighLimit 
            // Prodden
            // denLowLimit
            // denHighLimit

            List<GasRecipecalc_1Res_4> ResultList = new List<GasRecipecalc_1Res_4>();//新建一个List用来append的,返回的是list形式
            for(int i = 0; i < ProdOilNum; i++){
                double prodron = 0;
                double prodt50 = 0;
                double prodsuf = 0;
                double prodden = 0;
                GasRecipecalc_1Res_4 Result = new GasRecipecalc_1Res_4();
                Result.ProdOilName = ProdOilList[i].ProdOilName;

                for(int j = 0; j < ComOilNum; j++){
                    prodron = prodron + CompOilList[j].ron * percent[j, ProdOilList[i].Id - 1];
                }
                Result.Prodron = Math.Round(prodron, 1).ToString("0.0");

                for(int j = 0; j < ComOilNum; j++){
                    prodt50 = prodt50 + CompOilList[j].t50 * percent[j, ProdOilList[i].Id - 1];
                }
                Result.Prodt50 = Math.Round(prodt50).ToString("0");

                for(int j = 0; j < ComOilNum; j++){
                    prodsuf = prodsuf + CompOilList[j].suf * percent[j, ProdOilList[i].Id - 1];
                }
                Result.Prodsuf = Math.Round(prodsuf, 2).ToString("0.00");

                for(int j = 0; j < ComOilNum; j++){
                    prodden = prodden + CompOilList[j].den * percent[j, ProdOilList[i].Id - 1];
                }
                Result.Prodden = Math.Round(prodden, 1).ToString("0.0");

                Result.ronHighLimit = ProdOilList[i].ronHighLimit;
                Result.ronLowLimit = ProdOilList[i].ronLowLimit;
                Result.t50HighLimit = ProdOilList[i].t50HighLimit;
                Result.t50LowLimit = ProdOilList[i].t50LowLimit;
                Result.sufHighLimit = ProdOilList[i].sufHighLimit;
                Result.sufLowLimit = ProdOilList[i].sufLowLimit;
                Result.denHighLimit = ProdOilList[i].denHighLimit;
                Result.denLowLimit = ProdOilList[i].denLowLimit;

                ResultList.Add(Result);

            }
            return ResultList;
        }
        public IEnumerable<GasRecipecalc_2Res_1> GetRecipecalc_2Res_ProdOilProduct()//场景2 计算结果：成品油质量产量
        {
            IProdOilConfig _ProdOilConfig = new ProdOilConfig(context);
            ICompOilConfig _CompOilConfig = new CompOilConfig(context);

            var CompOilList = _CompOilConfig.GetAllCompOilConfigList().ToList();
            var ProdOilList = _ProdOilConfig.GetAllProdOilConfigList().Where(m=>m.Apply == 1).ToList();

            int ComOilNum = CompOilList.Count;//组分油个数
            int ProdOilNum = ProdOilList.Count;//成品油个数

            // public string? ComOilName//组分油名称
            // gas92Product//车柴质量产量
            // gas95Product//出柴质量产量
            // gas98Product//备用成品油1质量产量
            // gasSelfProduct//备用成品油2质量产量
            List<GasRecipecalc_2Res_1> ResultList = new List<GasRecipecalc_2Res_1>();//新建一个List用来append的,返回的是list形式

            for(int i = 0; i < ComOilNum; i++){
                GasRecipecalc_2Res_1 Result = new GasRecipecalc_2Res_1();
                Result.ComOilName = CompOilList[i].ComOilName;
                for(int j = 0; j < ProdOilNum; j++){
                    if(ProdOilList[j].Id == 1){
                        Result.gas92Product = (float)Math.Round(GetRecipe2()[ProdOilNum * i + j] * CompOilList[i].den / 1000, 2);
                    }

                    if(ProdOilList[j].Id == 2){
                        Result.gas95Product = (float)Math.Round(GetRecipe2()[ProdOilNum * i + j] * CompOilList[i].den / 1000, 2);     
                    }

                    if(ProdOilList[j].Id == 3){
                        Result.gas98Product = (float)Math.Round(GetRecipe2()[ProdOilNum * i + j] * CompOilList[i].den / 1000, 2);
                    }

                    if(ProdOilList[j].Id == 4){
                        Result.gasSelfProduct = (float)Math.Round(GetRecipe2()[ProdOilNum * i + j] * CompOilList[i].den / 1000, 2);
                    }

                }
                ResultList.Add(Result);
            }
            return ResultList;               
        }
        public IEnumerable<GasRecipecalc_2Res_2> GetRecipecalc_2Res_Recipe()//场景2 计算结果：成品油优化配方
        {
            IProdOilConfig _ProdOilConfig = new ProdOilConfig(context);
            IProperty _Property = new PropertyApply(context);
            ICompOilConfig _CompOilConfig = new CompOilConfig(context);

            var CompOilList = _CompOilConfig.GetAllCompOilConfigList().ToList();
            var ProdOilList = _ProdOilConfig.GetAllProdOilConfigList().Where(m=>m.Apply == 1).ToList();
            var PropertyList = _Property.GetAllPropertyList().ToList();

            int ComOilNum = CompOilList.Count;//组分油个数
            int ProdOilNum = ProdOilList.Count;//成品油个数

            var prodMassProduct = GetRecipecalc_2Res_ProdOilProduct().ToList();//成品油质量产量

            double Sum_Weight1 = 0.0000000001;//防止分母为0
            double Sum_Weight2 = 0.0000000001;
            double Sum_Weight3 = 0.0000000001;
            double Sum_Weight4 = 0.0000000001;

            double[] percent1 = new double[ComOilNum];//第一个成品油的配方
            double[] percent2 = new double[ComOilNum];//第二个成品油的配方
            double[] percent3 = new double[ComOilNum];//第三个成品油的配方
            double[] percent4 = new double[ComOilNum];//第四个成品油的配方

            for(int i = 0; i < prodMassProduct.Count; i++){//此循环是组分油个数
                Sum_Weight1 = Sum_Weight1 + prodMassProduct[i].gas92Product;//第一个成品油的总产量
                Sum_Weight2 = Sum_Weight2 + prodMassProduct[i].gas95Product;
                Sum_Weight3 = Sum_Weight3 + prodMassProduct[i].gas98Product;
                Sum_Weight4 = Sum_Weight4 + prodMassProduct[i].gasSelfProduct;
            }

            for(int i = 0; i < prodMassProduct.Count; i++){//此循环是组分油个数
                percent1[i] = prodMassProduct[i].gas92Product / Sum_Weight1;//第一个成品油的配方(基于质量的配方)
                percent2[i] = prodMassProduct[i].gas95Product / Sum_Weight2;
                percent3[i] = prodMassProduct[i].gas98Product / Sum_Weight3;
                percent4[i] = prodMassProduct[i].gasSelfProduct / Sum_Weight4;
            }

            List<GasRecipecalc_2Res_2> ResultList = new List<GasRecipecalc_2Res_2>();//新建一个List用来append的,返回的是list形式

            for(int i = 0; i < ComOilNum; i++){
                GasRecipecalc_2Res_2 Result = new GasRecipecalc_2Res_2();
                Result.ComOilName = CompOilList[i].ComOilName;
                for(int j = 0; j < ProdOilNum; j++){
                    if(ProdOilList[j].Id == 1){
                        Result.gas92Recipe = (float)Math.Round(percent1[i] * 100, 2);//保留完三位小数，换算成百分比之后就是一位小数
                    }

                    if(ProdOilList[j].Id == 2){
                        Result.gas95Recipe = (float)Math.Round(percent2[i] * 100, 2);     
                    }

                    if(ProdOilList[j].Id == 3){
                        Result.gas98Recipe = (float)Math.Round(percent3[i] * 100, 2);
                    }

                    if(ProdOilList[j].Id == 4){
                        Result.gasSelfRecipe = (float)Math.Round(percent4[i] * 100, 2);
                    }
                }
                ResultList.Add(Result);
            }
            return ResultList;
        }
        public IEnumerable<GasRecipecalc_2Res_3> GetRecipecalc_2Res_Property()//场景2 计算结果：成品油属性
        {
            IProdOilConfig _ProdOilConfig = new ProdOilConfig(context);
            IProperty _Property = new PropertyApply(context);
            ICompOilConfig _CompOilConfig = new CompOilConfig(context);

            var CompOilList = _CompOilConfig.GetAllCompOilConfigList().ToList();
            var ProdOilList = _ProdOilConfig.GetAllProdOilConfigList().Where(m=>m.Apply == 1).ToList();
            var PropertyList = _Property.GetAllPropertyList().ToList();

            int ComOilNum = CompOilList.Count;//组分油个数
            int ProdOilNum = ProdOilList.Count;//成品油个数

            double[,] prodVolProduct = new double[ComOilNum, 4];//成品油体积产量(组分油个数为行，成品油内置个数为列)

            double Sum_Weight1 = 0.0000000001;//防止分母为0
            double Sum_Weight2 = 0.0000000001;
            double Sum_Weight3 = 0.0000000001;
            double Sum_Weight4 = 0.0000000001;

            double[,] percent = new double[ComOilNum, 4];//第一个成品油的配方(基于体积)

            for(int i = 0; i < ComOilNum; i++){
                for(int j = 0; j < ProdOilNum; j++){
                    prodVolProduct[i, ProdOilList[j].Id - 1] = (float)GetRecipe2()[ProdOilNum * i + j];//第i个组分油的体积产量
                }
            }

            for(int i = 0; i < ComOilNum; i++){//此循环是组分油个数
                    Sum_Weight1 = Sum_Weight1 + prodVolProduct[i, 0];//第一个成品油的总产量
                    Sum_Weight2 = Sum_Weight2 + prodVolProduct[i, 1];
                    Sum_Weight3 = Sum_Weight3 + prodVolProduct[i, 2];
                    Sum_Weight4 = Sum_Weight4 + prodVolProduct[i, 3];
            }

            for(int i = 0; i < ComOilNum; i++){//此循环是组分油个数
                percent[i, 0] = prodVolProduct[i, 0] / Sum_Weight1;//第一个成品油的配方(体积)
                percent[i, 1] = prodVolProduct[i, 1] / Sum_Weight2;
                percent[i, 2] = prodVolProduct[i, 2] / Sum_Weight3;
                percent[i, 3] = prodVolProduct[i, 3] / Sum_Weight4;
            }

            // ProdOilName //成品油名称
            // Prodron//成品油十六烷值  
            // ronLowLimit//成品油十六烷值低限
            // ronHighLimit//成品油十六烷值高限
            // Prodt50
            // t50LowLimit 
            // t50HighLimit 
            // Prodsuf
            // sufLowLimit 
            // sufHighLimit 
            // Prodden
            // denLowLimit
            // denHighLimit

            List<GasRecipecalc_2Res_3> ResultList = new List<GasRecipecalc_2Res_3>();//新建一个List用来append的,返回的是list形式
            for(int i = 0; i < ProdOilNum; i++){
                double prodron = 0;
                double prodt50 = 0;
                double prodsuf = 0;
                double prodden = 0;
                GasRecipecalc_2Res_3 Result = new GasRecipecalc_2Res_3();
                Result.ProdOilName = ProdOilList[i].ProdOilName;

                for(int j = 0; j < ComOilNum; j++){
                    prodron = prodron + CompOilList[j].ron * percent[j, ProdOilList[i].Id - 1];
                }
                Result.Prodron = Math.Round(prodron, 1).ToString("0.0");

                for(int j = 0; j < ComOilNum; j++){
                    prodt50 = prodt50 + CompOilList[j].t50 * percent[j, ProdOilList[i].Id - 1];
                }
                Result.Prodt50 = Math.Round(prodt50).ToString("0");

                for(int j = 0; j < ComOilNum; j++){
                    prodsuf = prodsuf + CompOilList[j].suf * percent[j, ProdOilList[i].Id - 1];
                }
                Result.Prodsuf = Math.Round(prodsuf, 2).ToString("0.00");

                for(int j = 0; j < ComOilNum; j++){
                    prodden = prodden + CompOilList[j].den * percent[j, ProdOilList[i].Id - 1];
                }
                Result.Prodden = Math.Round(prodden, 1).ToString("0.0");

                Result.ronHighLimit = ProdOilList[i].ronHighLimit;
                Result.ronLowLimit = ProdOilList[i].ronLowLimit;
                Result.t50HighLimit = ProdOilList[i].t50HighLimit;
                Result.t50LowLimit = ProdOilList[i].t50LowLimit;
                Result.sufHighLimit = ProdOilList[i].sufHighLimit;
                Result.sufLowLimit = ProdOilList[i].sufLowLimit;
                Result.denHighLimit = ProdOilList[i].denHighLimit;
                Result.denLowLimit = ProdOilList[i].denLowLimit;

                ResultList.Add(Result);

            }
            return ResultList;
        }    
        public IEnumerable<GasRecipecalc_3Res_1> GetRecipecalc_3Res_ProdOilProduct()//场景3 计算结果：成品油质量产量
        {
            IProdOilConfig _ProdOilConfig = new ProdOilConfig(context);
            ICompOilConfig _CompOilConfig = new CompOilConfig(context);

            var CompOilList = _CompOilConfig.GetAllCompOilConfigList().ToList();
            var ProdOilList = _ProdOilConfig.GetAllProdOilConfigList().Where(m=>m.Apply == 1).ToList();

            int ComOilNum = CompOilList.Count;//组分油个数
            int ProdOilNum = ProdOilList.Count;//成品油个数

            // public string? ComOilName//组分油名称
            // gas92Product//车柴质量产量
            // gas95Product//出柴质量产量
            // gas98Product//备用成品油1质量产量
            // gasSelfProduct//备用成品油2质量产量
            List<GasRecipecalc_3Res_1> ResultList = new List<GasRecipecalc_3Res_1>();//新建一个List用来append的,返回的是list形式

            for(int i = 0; i < ComOilNum; i++){
                GasRecipecalc_3Res_1 Result = new GasRecipecalc_3Res_1();
                Result.ComOilName = CompOilList[i].ComOilName;
                for(int j = 0; j < ProdOilNum; j++){
                    if(ProdOilList[j].Id == 1){
                        Result.gas92Product = (float)Math.Round(GetRecipe3()[ProdOilNum * i + j] * CompOilList[i].den / 1000, 2);
                    }

                    if(ProdOilList[j].Id == 2){
                        Result.gas95Product = (float)Math.Round(GetRecipe3()[ProdOilNum * i + j] * CompOilList[i].den / 1000, 2);     
                    }

                    if(ProdOilList[j].Id == 3){
                        Result.gas98Product = (float)Math.Round(GetRecipe3()[ProdOilNum * i + j] * CompOilList[i].den / 1000, 2);
                    }

                    if(ProdOilList[j].Id == 4){
                        Result.gasSelfProduct = (float)Math.Round(GetRecipe3()[ProdOilNum * i + j] * CompOilList[i].den / 1000, 2);
                    }

                }
                ResultList.Add(Result);
            }
            return ResultList;   
        }
        public IEnumerable<GasRecipecalc_3Res_2> GetRecipecalc_3Res_Recipe()//场景3 计算结果：成品油优化配方
        {
            IProdOilConfig _ProdOilConfig = new ProdOilConfig(context);
            IProperty _Property = new PropertyApply(context);
            ICompOilConfig _CompOilConfig = new CompOilConfig(context);

            var CompOilList = _CompOilConfig.GetAllCompOilConfigList().ToList();
            var ProdOilList = _ProdOilConfig.GetAllProdOilConfigList().Where(m=>m.Apply == 1).ToList();
            var PropertyList = _Property.GetAllPropertyList().ToList();

            int ComOilNum = CompOilList.Count;//组分油个数
            int ProdOilNum = ProdOilList.Count;//成品油个数

            var prodMassProduct = GetRecipecalc_3Res_ProdOilProduct().ToList();//成品油质量产量

            double Sum_Weight1 = 0.0000000001;//防止分母为0
            double Sum_Weight2 = 0.0000000001;
            double Sum_Weight3 = 0.0000000001;
            double Sum_Weight4 = 0.0000000001;

            double[] percent1 = new double[ComOilNum];//第一个成品油的配方
            double[] percent2 = new double[ComOilNum];//第二个成品油的配方
            double[] percent3 = new double[ComOilNum];//第三个成品油的配方
            double[] percent4 = new double[ComOilNum];//第四个成品油的配方

            for(int i = 0; i < prodMassProduct.Count; i++){//此循环是组分油个数
                Sum_Weight1 = Sum_Weight1 + prodMassProduct[i].gas92Product;//第一个成品油的总产量
                Sum_Weight2 = Sum_Weight2 + prodMassProduct[i].gas95Product;
                Sum_Weight3 = Sum_Weight3 + prodMassProduct[i].gas98Product;
                Sum_Weight4 = Sum_Weight4 + prodMassProduct[i].gasSelfProduct;
            }

            for(int i = 0; i < prodMassProduct.Count; i++){//此循环是组分油个数
                percent1[i] = prodMassProduct[i].gas92Product / Sum_Weight1;//第一个成品油的配方(基于质量的配方)
                percent2[i] = prodMassProduct[i].gas95Product / Sum_Weight2;
                percent3[i] = prodMassProduct[i].gas98Product / Sum_Weight3;
                percent4[i] = prodMassProduct[i].gasSelfProduct / Sum_Weight4;
            }

            List<GasRecipecalc_3Res_2> ResultList = new List<GasRecipecalc_3Res_2>();//新建一个List用来append的,返回的是list形式

            for(int i = 0; i < ComOilNum; i++){
                GasRecipecalc_3Res_2 Result = new GasRecipecalc_3Res_2();
                Result.ComOilName = CompOilList[i].ComOilName;
                for(int j = 0; j < ProdOilNum; j++){
                    if(ProdOilList[j].Id == 1){
                        Result.gas92Recipe = (float)Math.Round(percent1[i] * 100, 2);//保留完三位小数，换算成百分比之后就是一位小数
                    }

                    if(ProdOilList[j].Id == 2){
                        Result.gas95Recipe = (float)Math.Round(percent2[i] * 100, 2);     
                    }

                    if(ProdOilList[j].Id == 3){
                        Result.gas98Recipe= (float)Math.Round(percent3[i] * 100, 2);
                    }

                    if(ProdOilList[j].Id == 4){
                        Result.gasSelfRecipe = (float)Math.Round(percent4[i] * 100, 2);
                    }
                }
                ResultList.Add(Result);
            }
            return ResultList;
        }
        public IEnumerable<GasRecipecalc_3Res_3> GetRecipecalc_3Res_Property()//场景3 计算结果：成品油属性
        {
            IProdOilConfig _ProdOilConfig = new ProdOilConfig(context);
            IProperty _Property = new PropertyApply(context);
            ICompOilConfig _CompOilConfig = new CompOilConfig(context);

            var CompOilList = _CompOilConfig.GetAllCompOilConfigList().ToList();
            var ProdOilList = _ProdOilConfig.GetAllProdOilConfigList().Where(m=>m.Apply == 1).ToList();
            var PropertyList = _Property.GetAllPropertyList().ToList();

            int ComOilNum = CompOilList.Count;//组分油个数
            int ProdOilNum = ProdOilList.Count;//成品油个数

            double[,] prodVolProduct = new double[ComOilNum, 4];//成品油体积产量(组分油个数为行，成品油内置个数为列)

            double Sum_Weight1 = 0.0000000001;//防止分母为0
            double Sum_Weight2 = 0.0000000001;
            double Sum_Weight3 = 0.0000000001;
            double Sum_Weight4 = 0.0000000001;

            double[,] percent = new double[ComOilNum, 4];//第一个成品油的配方(基于体积)

            for(int i = 0; i < ComOilNum; i++){
                for(int j = 0; j < ProdOilNum; j++){
                    prodVolProduct[i, ProdOilList[j].Id - 1] = (float)GetRecipe3()[ProdOilNum * i + j];//第i个组分油的体积产量
                }
            }

            for(int i = 0; i < ComOilNum; i++){//此循环是组分油个数
                    Sum_Weight1 = Sum_Weight1 + prodVolProduct[i, 0];//第一个成品油的总产量
                    Sum_Weight2 = Sum_Weight2 + prodVolProduct[i, 1];
                    Sum_Weight3 = Sum_Weight3 + prodVolProduct[i, 2];
                    Sum_Weight4 = Sum_Weight4 + prodVolProduct[i, 3];
            }

            for(int i = 0; i < ComOilNum; i++){//此循环是组分油个数
                percent[i, 0] = prodVolProduct[i, 0] / Sum_Weight1;//第一个成品油的配方(体积)
                percent[i, 1] = prodVolProduct[i, 1] / Sum_Weight2;
                percent[i, 2] = prodVolProduct[i, 2] / Sum_Weight3;
                percent[i, 3] = prodVolProduct[i, 3] / Sum_Weight4;
            }

            // ProdOilName //成品油名称
            // Prodron//成品油十六烷值  
            // ronLowLimit//成品油十六烷值低限
            // ronHighLimit//成品油十六烷值高限
            // Prodt50
            // t50LowLimit 
            // t50HighLimit 
            // Prodsuf
            // sufLowLimit 
            // sufHighLimit 
            // Prodden
            // denLowLimit
            // denHighLimit

            List<GasRecipecalc_3Res_3> ResultList = new List<GasRecipecalc_3Res_3>();//新建一个List用来append的,返回的是list形式
            for(int i = 0; i < ProdOilNum; i++){
                double prodron = 0;
                double prodt50 = 0;
                double prodsuf = 0;
                double prodden = 0;
                GasRecipecalc_3Res_3 Result = new GasRecipecalc_3Res_3();
                Result.ProdOilName = ProdOilList[i].ProdOilName;

                for(int j = 0; j < ComOilNum; j++){
                    prodron = prodron + CompOilList[j].ron * percent[j, ProdOilList[i].Id - 1];
                }
                Result.Prodron = Math.Round(prodron, 1).ToString("0.0");

                for(int j = 0; j < ComOilNum; j++){
                    prodt50 = prodt50 + CompOilList[j].t50 * percent[j, ProdOilList[i].Id - 1];
                }
                Result.Prodt50 = Math.Round(prodt50).ToString("0");

                for(int j = 0; j < ComOilNum; j++){
                    prodsuf = prodsuf + CompOilList[j].suf * percent[j, ProdOilList[i].Id - 1];
                }
                Result.Prodsuf = Math.Round(prodsuf, 2).ToString("0.00");

                for(int j = 0; j < ComOilNum; j++){
                    prodden = prodden + CompOilList[j].den * percent[j, ProdOilList[i].Id - 1];
                }
                Result.Prodden = Math.Round(prodden, 1).ToString("0.0");

                Result.ronHighLimit = ProdOilList[i].ronHighLimit;
                Result.ronLowLimit = ProdOilList[i].ronLowLimit;
                Result.t50HighLimit = ProdOilList[i].t50HighLimit;
                Result.t50LowLimit = ProdOilList[i].t50LowLimit;
                Result.sufHighLimit = ProdOilList[i].sufHighLimit;
                Result.sufLowLimit = ProdOilList[i].sufLowLimit;
                Result.denHighLimit = ProdOilList[i].denHighLimit;
                Result.denLowLimit = ProdOilList[i].denLowLimit;

                ResultList.Add(Result);

            }
            return ResultList;         
        }  
    
    }
}

