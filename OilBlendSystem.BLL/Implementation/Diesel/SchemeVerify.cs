using OilBlendSystem.Models.Diesel.DataBaseModel;
using OilBlendSystem.Models.Diesel.ConstructModel;
using OilBlendSystem.Models;
using Microsoft.EntityFrameworkCore;
using OilBlendSystem.BLL.Interface.Diesel;

namespace OilBlendSystem.BLL.Implementation.Diesel
{
    public partial class SchemeVerify : ISchemeVerify
    {
        private readonly oilblendContext context;//带问号是可以为空
        // double[] MassProperty = new double[4];//暂存方案验证场景1—质量 计算的属性

        public SchemeVerify(oilblendContext _context)
        {
           //oilblendContext _context = new();
           context = _context;
        }

        public IEnumerable<Schemeverify1> GetSchemeVerify1()
        {
            context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
            return context.Schemeverify1s.ToList();
        }

        public IEnumerable<Schemeverify2> GetSchemeVerify2()
        {
            context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
            return context.Schemeverify2s.ToList();
        }

        public IEnumerable<SchemeVerify_1Res_1> GetSchemeverifyResult1_mass_Time()
        {
            IProdOilConfig _ProdOilConfig = new ProdOilConfig(context);
            IProperty _Property = new PropertyApply(context);
            ICompOilConfig _CompOilConfig = new CompOilConfig(context);
            ISchemeVerify _Schemeverify = new SchemeVerify(context);           

            var CompOilList = _CompOilConfig.GetAllCompOilConfigList().ToList();
            var ProdOilList = _ProdOilConfig.GetAllProdOilConfigList().ToList();
            var PropertyList = _Property.GetAllPropertyList().ToList();
            var SchemeCompOilList = _Schemeverify.GetSchemeVerify1().ToList();//组分油的车柴出柴 质量/体积的质量产量，体积产量
            var SchemeBottomList = _Schemeverify.GetSchemeVerify2().ToList();//成品油的罐底油信息和调合总量
            double AutoMassProduct = 0;//车用柴油质量产量
            for (int i = 0; i < SchemeCompOilList.Count; i++) 
            {
                AutoMassProduct = AutoMassProduct + SchemeCompOilList[i].AutoQualityProduct;             
            }
            double ExpMassProduct = 0;//出口柴油质量产量
            for (int i = 0; i < SchemeCompOilList.Count; i++) 
            {
                ExpMassProduct = ExpMassProduct + SchemeCompOilList[i].ExpQualityProduct;             
            }
            double Prod1MassProduct = 0;//备用油1质量产量
            for (int i = 0; i < SchemeCompOilList.Count; i++) 
            {
                Prod1MassProduct = Prod1MassProduct + SchemeCompOilList[i].Prod1QualityProduct;             
            }
            double Prod2MassProduct = 0;//备用油2质量产量
            for (int i = 0; i < SchemeCompOilList.Count; i++) 
            {
                Prod2MassProduct = Prod2MassProduct + SchemeCompOilList[i].Prod2QualityProduct;             
            }

            List<SchemeVerify_1Res_1> ResultList = new List<SchemeVerify_1Res_1>();//列表，里面可以添加很多个对象
            //车柴
            SchemeVerify_1Res_1 result1 = new SchemeVerify_1Res_1();//实体，可以理解为一个对象
            result1.ProdOilName = ProdOilList[0].ProdOilName;
            result1.Time = (float)Math.Round((SchemeBottomList[0].TotalBlendMass - SchemeBottomList[0].BottomMass) / AutoMassProduct * 24, 1);
            ResultList.Add(result1);
            //出柴
            SchemeVerify_1Res_1 result2 = new SchemeVerify_1Res_1();//实体，可以理解为一个对象
            result2.ProdOilName = ProdOilList[1].ProdOilName;
            result2.Time = (float)Math.Round((SchemeBottomList[1].TotalBlendMass - SchemeBottomList[1].BottomMass) / ExpMassProduct * 24, 1);
            ResultList.Add(result2);
            //备用油1
            SchemeVerify_1Res_1 result3 = new SchemeVerify_1Res_1();//实体，可以理解为一个对象
            result3.ProdOilName = ProdOilList[2].ProdOilName;
            result3.Time = (float)Math.Round((SchemeBottomList[2].TotalBlendMass - SchemeBottomList[2].BottomMass) / Prod1MassProduct * 24, 1);
            ResultList.Add(result3);
            //备用油2
            SchemeVerify_1Res_1 result4 = new SchemeVerify_1Res_1();//实体，可以理解为一个对象
            result4.ProdOilName = ProdOilList[3].ProdOilName;
            result4.Time = (float)Math.Round((SchemeBottomList[3].TotalBlendMass - SchemeBottomList[3].BottomMass) / Prod2MassProduct * 24, 1);
            ResultList.Add(result4);
            return ResultList;

        }

        public IEnumerable<SchemeVerify_1Res_1> GetSchemeverifyResult1_vol_Time()
        {
            IProdOilConfig _ProdOilConfig = new ProdOilConfig(context);
            IProperty _Property = new PropertyApply(context);
            ICompOilConfig _CompOilConfig = new CompOilConfig(context);
            ISchemeVerify _Schemeverify = new SchemeVerify(context);           

            var CompOilList = _CompOilConfig.GetAllCompOilConfigList().ToList();
            var ProdOilList = _ProdOilConfig.GetAllProdOilConfigList().ToList();
            var PropertyList = _Property.GetAllPropertyList().ToList();
            var SchemeCompOilList = _Schemeverify.GetSchemeVerify1().ToList();//组分油的车柴出柴 质量/体积的质量产量，体积产量
            var SchemeBottomList = _Schemeverify.GetSchemeVerify2().ToList();//成品油的罐底油信息和调合总量
            double AutoVolProduct = 0;//车用柴油体积产量
            for (int i = 0; i < SchemeCompOilList.Count; i++) 
            {
                AutoVolProduct = AutoVolProduct + SchemeCompOilList[i].AutoVolumeProduct;             
            }
            double ExpVolProduct = 0;//出口柴油体积产量
            for (int i = 0; i < SchemeCompOilList.Count; i++) 
            {
                ExpVolProduct = ExpVolProduct + SchemeCompOilList[i].ExpVolumeProduct;             
            }
            double Prod1VolProduct = 0;//备用油1体积产量
            for (int i = 0; i < SchemeCompOilList.Count; i++) 
            {
                Prod1VolProduct = Prod1VolProduct + SchemeCompOilList[i].Prod1VolumeProduct;             
            }
            double Prod2VolProduct = 0;//备用油2体积产量
            for (int i = 0; i < SchemeCompOilList.Count; i++) 
            {
                Prod2VolProduct = Prod2VolProduct + SchemeCompOilList[i].Prod2VolumeProduct;             
            }

            List<SchemeVerify_1Res_1> ResultList = new List<SchemeVerify_1Res_1>();//列表，里面可以添加很多个对象
            //车柴
            SchemeVerify_1Res_1 result1 = new SchemeVerify_1Res_1();//实体，可以理解为一个对象
            result1.ProdOilName = ProdOilList[0].ProdOilName;
            result1.Time = (float)Math.Round((SchemeBottomList[0].TotalBlendVol - SchemeBottomList[0].BottomVolume) / AutoVolProduct * 24, 1);
            ResultList.Add(result1);
            //出柴
            SchemeVerify_1Res_1 result2 = new SchemeVerify_1Res_1();//实体，可以理解为一个对象
            result2.ProdOilName = ProdOilList[1].ProdOilName;
            result2.Time = (float)Math.Round((SchemeBottomList[1].TotalBlendVol - SchemeBottomList[1].BottomVolume) / ExpVolProduct * 24, 1);
            ResultList.Add(result2);
            //备用油1
            SchemeVerify_1Res_1 result3 = new SchemeVerify_1Res_1();//实体，可以理解为一个对象
            result3.ProdOilName = ProdOilList[2].ProdOilName;
            result3.Time = (float)Math.Round((SchemeBottomList[2].TotalBlendVol - SchemeBottomList[2].BottomVolume) / Prod1VolProduct * 24, 1);
            ResultList.Add(result3);
            //备用油2
            SchemeVerify_1Res_1 result4 = new SchemeVerify_1Res_1();//实体，可以理解为一个对象
            result4.ProdOilName = ProdOilList[3].ProdOilName;
            result4.Time = (float)Math.Round((SchemeBottomList[3].TotalBlendVol - SchemeBottomList[3].BottomVolume) / Prod2VolProduct * 24, 1);
            ResultList.Add(result4);
            return ResultList;

        }

        public IEnumerable<SchemeVerify_1Res_2> GetSchemeverifyResult1_mass_Product()//场景1质量 成品油产量
        {
            var SchemeCompOilList = context.Schemeverify1s.ToList();
            var SchemeBottomList = context.Schemeverify2s.ToList();
            double AutoMassProduct = 0;//车用柴油质量产量
            for (int i = 0; i < SchemeCompOilList.Count; i++) 
            {
                AutoMassProduct = AutoMassProduct + SchemeCompOilList[i].AutoQualityProduct;             
            }
            double ExpMassProduct = 0;//出口柴油质量产量
            for (int i = 0; i < SchemeCompOilList.Count; i++) 
            {
                ExpMassProduct = ExpMassProduct + SchemeCompOilList[i].ExpQualityProduct;             
            }
            double Prod1MassProduct = 0;//备用油1质量产量
            for (int i = 0; i < SchemeCompOilList.Count; i++) 
            {
                Prod1MassProduct = Prod1MassProduct + SchemeCompOilList[i].Prod1QualityProduct;             
            }
            double Prod2MassProduct = 0;//备用油2质量产量
            for (int i = 0; i < SchemeCompOilList.Count; i++) 
            {
                Prod2MassProduct = Prod2MassProduct + SchemeCompOilList[i].Prod2QualityProduct;             
            }

            List<SchemeVerify_1Res_2> ResultList = new List<SchemeVerify_1Res_2>();//列表，里面可以添加很多个对象
            for(int i = 0; i < SchemeCompOilList.Count; i++){
                SchemeVerify_1Res_2 result = new SchemeVerify_1Res_2();//实体，可以理解为一个对象
                result.ComOilName = SchemeCompOilList[i].ComOilName;
                result.AutoProduct = (float)Math.Round(SchemeCompOilList[i].AutoQualityProduct / AutoMassProduct * (SchemeBottomList[0].TotalBlendMass - SchemeBottomList[0].BottomMass), 2);
                result.ExpProduct = (float)Math.Round(SchemeCompOilList[i].ExpQualityProduct / ExpMassProduct * (SchemeBottomList[1].TotalBlendMass - SchemeBottomList[1].BottomMass), 2);
                result.Prod1Product = (float)Math.Round(SchemeCompOilList[i].Prod1QualityProduct / Prod1MassProduct * (SchemeBottomList[2].TotalBlendMass - SchemeBottomList[2].BottomMass), 2);
                result.Prod2Product = (float)Math.Round(SchemeCompOilList[i].Prod2QualityProduct / Prod2MassProduct * (SchemeBottomList[3].TotalBlendMass - SchemeBottomList[3].BottomMass), 2);
                ResultList.Add(result);
            }
            return ResultList;         

        }

        public IEnumerable<SchemeVerify_1Res_2> GetSchemeverifyResult1_vol_Product()//场景1体积 成品油产量
        {
            var SchemeCompOilList = context.Schemeverify1s.ToList();
            var SchemeBottomList = context.Schemeverify2s.ToList();
            double AutoVolProduct = 0;//车用柴油体积产量
            for (int i = 0; i < SchemeCompOilList.Count; i++) 
            {
                AutoVolProduct = AutoVolProduct + SchemeCompOilList[i].AutoVolumeProduct;             
            }
            double ExpVolProduct = 0;//出口柴油体积产量
            for (int i = 0; i < SchemeCompOilList.Count; i++) 
            {
                ExpVolProduct = ExpVolProduct + SchemeCompOilList[i].ExpVolumeProduct;             
            }
            double Prod1VolProduct = 0;//备用油1体积产量
            for (int i = 0; i < SchemeCompOilList.Count; i++) 
            {
                Prod1VolProduct = Prod1VolProduct + SchemeCompOilList[i].Prod1VolumeProduct;             
            }
            double Prod2VolProduct = 0;//备用油2体积产量
            for (int i = 0; i < SchemeCompOilList.Count; i++) 
            {
                Prod2VolProduct = Prod2VolProduct + SchemeCompOilList[i].Prod2VolumeProduct;             
            }

            List<SchemeVerify_1Res_2> ResultList = new List<SchemeVerify_1Res_2>();//列表，里面可以添加很多个对象
            for(int i = 0; i < SchemeCompOilList.Count; i++){
                SchemeVerify_1Res_2 result = new SchemeVerify_1Res_2();//实体，可以理解为一个对象
                result.ComOilName = SchemeCompOilList[i].ComOilName;
                result.AutoProduct = (float)Math.Round(SchemeCompOilList[i].AutoVolumeProduct / AutoVolProduct * (SchemeBottomList[0].TotalBlendVol - SchemeBottomList[0].BottomVolume), 2);
                result.ExpProduct = (float)Math.Round(SchemeCompOilList[i].ExpVolumeProduct / ExpVolProduct * (SchemeBottomList[1].TotalBlendVol - SchemeBottomList[1].BottomVolume), 2);
                result.Prod1Product = (float)Math.Round(SchemeCompOilList[i].Prod1VolumeProduct / Prod1VolProduct * (SchemeBottomList[2].TotalBlendVol - SchemeBottomList[2].BottomVolume), 2);
                result.Prod2Product = (float)Math.Round(SchemeCompOilList[i].Prod2VolumeProduct / Prod2VolProduct * (SchemeBottomList[3].TotalBlendVol - SchemeBottomList[3].BottomVolume), 2);
                ResultList.Add(result);
            }
            return ResultList;    
        }

        public IEnumerable<SchemeVerify_1Res_3> GetSchemeverifyResult1_mass_Property()//场景1质量 成品油属性
        {
            IProdOilConfig _ProdOilConfig = new ProdOilConfig(context);
            IProperty _Property = new PropertyApply(context);
            ICompOilConfig _CompOilConfig = new CompOilConfig(context);
            ISchemeVerify _Schemeverify = new SchemeVerify(context);           

            var CompOilList = _CompOilConfig.GetAllCompOilConfigList().ToList();
            var ProdOilList = _ProdOilConfig.GetAllProdOilConfigList().ToList();
            var PropertyList = _Property.GetAllPropertyList().ToList();
            var SchemeCompOilList = _Schemeverify.GetSchemeVerify1().ToList();//组分油的车柴出柴 质量/体积的质量产量，体积产量
            var SchemeBottomList = _Schemeverify.GetSchemeVerify2().ToList();//成品油的罐底油信息和调合总量


            List<SchemeVerify_1Res_3> ResultList = new List<SchemeVerify_1Res_3>();//列表，里面可以添加很多个对象

            #region 场景1——质量 
            #region 车用柴油
            /// <summary>
            /// 车用柴油
            /// </summary>
            double AutoMassProduct = 0;//车用柴油质量产量
            for (int i = 0; i < SchemeCompOilList.Count; i++) 
            {
                AutoMassProduct = AutoMassProduct + SchemeCompOilList[i].AutoQualityProduct;             
            }

            double AutoVolume = 0;//车用柴油总体积
            for (int i = 0; i < SchemeCompOilList.Count; i++)
            {
                AutoVolume = AutoVolume + SchemeCompOilList[i].AutoQualityProduct / CompOilList[i].Den * 1000;//体积
            }
            // 车柴十六烷值指数 基于体积
            double AutoCETMass = 0;
            for(int i = 0;i < CompOilList.Count; i++)
            {
                AutoCETMass = AutoCETMass + CompOilList[i].Cet * SchemeCompOilList[i].AutoQualityProduct / CompOilList[i].Den * 1000;//十六烷值乘以体积
            }

            double TotalAutoCETMass = Math.Round((AutoCETMass + SchemeBottomList[0].BottomMass * 1000 / SchemeBottomList[0].DenMass * SchemeBottomList[0].CetMass) / (AutoVolume + SchemeBottomList[0].BottomMass * 1000 / SchemeBottomList[0].DenMass), 1); 

            // 车柴50%回收温度 基于体积
            double AutoD50Mass = 0;
            for (int i = 0; i < CompOilList.Count; i++)
            {
                AutoD50Mass = AutoD50Mass + CompOilList[i].D50 * SchemeCompOilList[i].AutoQualityProduct / CompOilList[i].Den * 1000;//馏程乘以体积
            }

            double TotalAutoD50Mass = Math.Round((AutoD50Mass + SchemeBottomList[0].BottomMass * 1000 / SchemeBottomList[0].DenMass * SchemeBottomList[0].D50Mass) / (AutoVolume + SchemeBottomList[0].BottomMass * 1000 / SchemeBottomList[0].DenMass));

            // 多环芳烃含量 基于质量
            double AutoPOLMass = 0;
            for (int i = 0; i < CompOilList.Count; i++)
            {
                AutoPOLMass = AutoPOLMass + CompOilList[i].Pol * SchemeCompOilList[i].AutoQualityProduct;//多芳烃基于质量
            }
            double AutoBottomMass = SchemeBottomList[0].BottomMass;
            double TotalAutoPOLMass = Math.Round((AutoPOLMass + AutoBottomMass * SchemeBottomList[0].PolMass) / (AutoMassProduct + AutoBottomMass), 2);

            // 密度 基于质量
            double AutoDENMass = 0;
            for (int i = 0; i < CompOilList.Count; i++)
            {
                AutoDENMass = AutoDENMass + CompOilList[i].Den * SchemeCompOilList[i].AutoQualityProduct;//密度乘以质量
            }
            double TotalAutoDENMass = Math.Round((AutoDENMass + AutoBottomMass * SchemeBottomList[0].DenMass) / (AutoMassProduct + AutoBottomMass), 1);
            
            #endregion

            #region 出口柴油
            /// <summary>
            /// 出口柴油
            /// </summary>
            double ExpMassProduct = 0;//出口柴油质量产量
            for (int i = 0; i < SchemeCompOilList.Count; i++) 
            {
                ExpMassProduct = ExpMassProduct + SchemeCompOilList[i].ExpQualityProduct;             
            }

            double ExpVolume = 0;//出口柴油总体积
            for (int i = 0; i < SchemeCompOilList.Count; i++)
            {
                ExpVolume = ExpVolume + SchemeCompOilList[i].ExpQualityProduct / CompOilList[i].Den * 1000;//体积
            }
            // 出柴十六烷值指数 基于体积
            double ExpCETMass = 0;
            for(int i = 0;i < CompOilList.Count; i++)
            {
                ExpCETMass = ExpCETMass + CompOilList[i].Cet * SchemeCompOilList[i].ExpQualityProduct / CompOilList[i].Den * 1000;//十六烷值乘以体积
            }

            double TotalExpCETMass = Math.Round((ExpCETMass + SchemeBottomList[1].BottomMass * 1000 / SchemeBottomList[1].DenMass * SchemeBottomList[1].CetMass) / (ExpVolume + SchemeBottomList[1].BottomMass * 1000 / SchemeBottomList[1].DenMass), 1); 

            // 出柴50%回收温度 基于体积
            double ExpD50Mass = 0;
            for (int i = 0; i < CompOilList.Count; i++)
            {
                ExpD50Mass = ExpD50Mass + CompOilList[i].D50 * SchemeCompOilList[i].ExpQualityProduct / CompOilList[i].Den * 1000;//馏程乘以体积
            }

            double TotalExpD50Mass = Math.Round((ExpD50Mass + SchemeBottomList[1].BottomMass * 1000 / SchemeBottomList[1].DenMass * SchemeBottomList[1].D50Mass) / (ExpVolume + SchemeBottomList[1].BottomMass * 1000 / SchemeBottomList[1].DenMass));

            // 多环芳烃含量 基于质量
            double ExpPOLMass = 0;
            for (int i = 0; i < CompOilList.Count; i++)
            {
                ExpPOLMass = ExpPOLMass + CompOilList[i].Pol * SchemeCompOilList[i].ExpQualityProduct;//多芳烃基于质量
            }
            double ExpBottomMass = SchemeBottomList[1].BottomMass;
            double TotalExpPOLMass = Math.Round((ExpPOLMass + ExpBottomMass * SchemeBottomList[1].PolMass) / (ExpMassProduct + ExpBottomMass), 2);

            // 密度 基于质量
            double ExpDENMass = 0;
            for (int i = 0; i < CompOilList.Count; i++)
            {
                ExpDENMass = ExpDENMass + CompOilList[i].Den * SchemeCompOilList[i].ExpQualityProduct;//密度乘以质量
            }
            double TotalExpDENMass = Math.Round((ExpDENMass + ExpBottomMass * SchemeBottomList[1].DenMass) / (ExpMassProduct + ExpBottomMass), 1);
            #endregion

            #region 备用成品油1
            /// <summary>
            /// 备用成品油1
            /// </summary>    
            double Prod1MassProduct = 0;//车用柴油质量产量
            for (int i = 0; i < SchemeCompOilList.Count; i++) 
            {
                Prod1MassProduct = Prod1MassProduct + SchemeCompOilList[i].Prod1QualityProduct;             
            }

            double Prod1Volume = 0;//车用柴油总体积
            for (int i = 0; i < SchemeCompOilList.Count; i++)
            {
                Prod1Volume = Prod1Volume + SchemeCompOilList[i].Prod1QualityProduct / CompOilList[i].Den * 1000;//体积
            }
            // 车柴十六烷值指数 基于体积
            double Prod1CETMass = 0;
            for(int i = 0;i < CompOilList.Count; i++)
            {
                Prod1CETMass = Prod1CETMass + CompOilList[i].Cet * SchemeCompOilList[i].Prod1QualityProduct / CompOilList[i].Den * 1000;//十六烷值乘以体积
            }

            double TotalProd1CETMass = Math.Round((Prod1CETMass + SchemeBottomList[2].BottomMass * 1000 / SchemeBottomList[2].DenMass * SchemeBottomList[2].CetMass) / (Prod1Volume + SchemeBottomList[2].BottomMass * 1000 / SchemeBottomList[2].DenMass), 1); 

            // 车柴50%回收温度 基于体积
            double Prod1D50Mass = 0;
            for (int i = 0; i < CompOilList.Count; i++)
            {
                Prod1D50Mass = Prod1D50Mass + CompOilList[i].D50 * SchemeCompOilList[i].Prod1QualityProduct / CompOilList[i].Den * 1000;//馏程乘以体积
            }

            double TotalProd1D50Mass = Math.Round((Prod1D50Mass + SchemeBottomList[2].BottomMass * 1000 / SchemeBottomList[2].DenMass * SchemeBottomList[2].D50Mass) / (Prod1Volume + SchemeBottomList[2].BottomMass * 1000 / SchemeBottomList[2].DenMass));

            // 多环芳烃含量 基于质量
            double Prod1POLMass = 0;
            for (int i = 0; i < CompOilList.Count; i++)
            {
                Prod1POLMass = Prod1POLMass + CompOilList[i].Pol * SchemeCompOilList[i].Prod1QualityProduct;//多芳烃基于质量
            }
            double Prod1BottomMass = SchemeBottomList[2].BottomMass;
            double TotalProd1POLMass = Math.Round((Prod1POLMass + Prod1BottomMass * SchemeBottomList[2].PolMass) / (Prod1MassProduct + Prod1BottomMass), 2);

            // 密度 基于质量
            double Prod1DENMass = 0;
            for (int i = 0; i < CompOilList.Count; i++)
            {
                Prod1DENMass = Prod1DENMass + CompOilList[i].Den * SchemeCompOilList[i].Prod1QualityProduct;//密度乘以质量
            }
            double TotalProd1DENMass = Math.Round((Prod1DENMass + Prod1BottomMass * SchemeBottomList[2].DenMass) / (Prod1MassProduct + Prod1BottomMass), 1);

            #endregion    

            #region 备用成品油2
            /// <summary>
            /// 备用成品油2
            /// </summary>
            double Prod2MassProduct = 0;//车用柴油质量产量
            for (int i = 0; i < SchemeCompOilList.Count; i++) 
            {
                Prod2MassProduct = Prod2MassProduct + SchemeCompOilList[i].Prod2QualityProduct;             
            }

            double Prod2Volume = 0;//车用柴油总体积
            for (int i = 0; i < SchemeCompOilList.Count; i++)
            {
                Prod2Volume = Prod2Volume + SchemeCompOilList[i].Prod2QualityProduct / CompOilList[i].Den * 1000;//体积
            }
            // 车柴十六烷值指数 基于体积
            double Prod2CETMass = 0;
            for(int i = 0;i < CompOilList.Count; i++)
            {
                Prod2CETMass = Prod2CETMass + CompOilList[i].Cet * SchemeCompOilList[i].Prod2QualityProduct / CompOilList[i].Den * 1000;//十六烷值乘以体积
            }

            double TotalProd2CETMass = Math.Round((Prod2CETMass + SchemeBottomList[3].BottomMass * 1000 / SchemeBottomList[3].DenMass * SchemeBottomList[3].CetMass) / (Prod2Volume + SchemeBottomList[3].BottomMass * 1000 / SchemeBottomList[3].DenMass), 1); 

            // 车柴50%回收温度 基于体积
            double Prod2D50Mass = 0;
            for (int i = 0; i < CompOilList.Count; i++)
            {
                Prod2D50Mass = Prod2D50Mass + CompOilList[i].D50 * SchemeCompOilList[i].Prod2QualityProduct / CompOilList[i].Den * 1000;//馏程乘以体积
            }

            double TotalProd2D50Mass = Math.Round((Prod2D50Mass + SchemeBottomList[3].BottomMass * 1000 / SchemeBottomList[3].DenMass * SchemeBottomList[3].D50Mass) / (Prod2Volume + SchemeBottomList[3].BottomMass * 1000 / SchemeBottomList[3].DenMass));

            // 多环芳烃含量 基于质量
            double Prod2POLMass = 0;
            for (int i = 0; i < CompOilList.Count; i++)
            {
                Prod2POLMass = Prod2POLMass + CompOilList[i].Pol * SchemeCompOilList[i].Prod2QualityProduct;//多芳烃基于质量
            }
            double Prod2BottomMass = SchemeBottomList[2].BottomMass;
            double TotalProd2POLMass = Math.Round((Prod2POLMass + Prod2BottomMass * SchemeBottomList[3].PolMass) / (Prod2MassProduct + Prod2BottomMass), 2);

            // 密度 基于质量
            double Prod2DENMass = 0;
            for (int i = 0; i < CompOilList.Count; i++)
            {
                Prod2DENMass = Prod2DENMass + CompOilList[i].Den * SchemeCompOilList[i].Prod2QualityProduct;//密度乘以质量
            }
            double TotalProd2DENMass = Math.Round((Prod2DENMass + Prod2BottomMass * SchemeBottomList[3].DenMass) / (Prod2MassProduct + Prod2BottomMass), 1);
            
            #endregion

            #region 闪点和粘度
            //     //闪点
            //     //double Sum1_Flash= 0;
            //     double Sum1_index = 0;
            //     //double P1_Bottom_volume = Bottom_FC_1 / list2[0].Bottom_DEN;
            //     double P1_total_volume = P1_Volume + list2[0].Bottom_Volume * 1000 / list2[0].Bottom_DEN;

            //     for (int i = 0; i < CompOilList.Count; i++)
            //     {
            //         Sum1_index = Sum1_index + Math.Pow(0.929, CompOilList[i].Flash) * CompOilList[i].P0_FC *1000 / CompOilList[i].DEN / P1_total_volume;//
            //     }
            //     Sum1_index = Sum1_index + Math.Pow(0.929, list2[0].Bottom_Flash) * list2[0].Bottom_Volume * 1000 / list2[0].Bottom_DEN / P1_total_volume;
            //     //Sum1_Flash = Math.Log(Sum1_index, 0.929);

            //     //double P1_DEN = Math.Round(Sum1_DEN / P1_Weight, 2);//总密度除以总质量
            //     double P1_Flash = Math.Round(Math.Log(Sum1_index, 0.929), 2);

            //    // 粘度
            //     double Sum1_Viscosity = 0;
            //     for (int i = 0; i < CompOilList.Count; i++)
            //     {
            //         Sum1_Viscosity = Sum1_Viscosity + Math.Pow(CompOilList[i].Viscosity, 1.0/3) * CompOilList[i].P0_FC / CompOilList[i].DEN * 1000;//
            //     }
            //     Sum1_Viscosity = (Sum1_Viscosity + Math.Pow(list2[0].Bottom_Viscosity, 1.0/3) * list2[0].Bottom_Volume * 1000 / list2[0].Bottom_DEN) / P1_total_volume;

            //     //double P1_DEN = Math.Round(Sum1_DEN / P1_Weight, 2);//总密度除以总质量
            //     double P1_Viscosity = Math.Round(Math.Pow(Sum1_Viscosity, 3), 2);
            #endregion
            #endregion

            #region 返回车柴数据
            SchemeVerify_1Res_3 result1 = new SchemeVerify_1Res_3();//实体，可以理解为一个对象
            /// <summary>
            /// 质量界面
            /// </summary>
            result1.ProdOilName = SchemeBottomList[0].ProdOilName;
            result1.ProdCET = (float)TotalAutoCETMass;
            result1.ProdD50 = (float)TotalAutoD50Mass;
            result1.ProdPOL = (float)TotalAutoPOLMass;
            result1.ProdDEN = (float)TotalAutoDENMass;
            /// <summary>
            /// 高低限不分质量和体积
            /// </summary>
            result1.CETLowLimit = ProdOilList[0].CetLowLimit;
            result1.CETHighLimit = ProdOilList[0].CetHighLimit;
            result1.D50LowLimit = ProdOilList[0].D50LowLimit;
            result1.D50HighLimit = ProdOilList[0].D50HighLimit;
            result1.POLLowLimit = ProdOilList[0].PolLowLimit;
            result1.POLHighLimit = ProdOilList[0].PolHighLimit;
            result1.DENLowLimit = ProdOilList[0].DenLowLimit;
            result1.DENHighLimit = ProdOilList[0].DenHighLimit;

            ResultList.Add(result1);           
            #endregion

            #region 返回出柴数据
            SchemeVerify_1Res_3 result2 = new SchemeVerify_1Res_3();//实体，可以理解为一个对象
            /// <summary>
            /// 质量界面
            /// </summary>
            result2.ProdOilName = SchemeBottomList[1].ProdOilName;
            result2.ProdCET = (float)TotalExpCETMass;
            result2.ProdD50 = (float)TotalExpD50Mass;
            result2.ProdPOL = (float)TotalExpPOLMass;
            result2.ProdDEN = (float)TotalExpDENMass;
            /// <summary>
            /// 高地限不分质量和体积
            /// </summary>
            result2.CETLowLimit = ProdOilList[1].CetLowLimit;
            result2.CETHighLimit = ProdOilList[1].CetHighLimit;
            result2.D50LowLimit = ProdOilList[1].D50LowLimit;
            result2.D50HighLimit = ProdOilList[1].D50HighLimit;
            result2.POLLowLimit = ProdOilList[1].PolLowLimit;
            result2.POLHighLimit = ProdOilList[1].PolHighLimit;
            result2.DENLowLimit = ProdOilList[1].DenLowLimit;
            result2.DENHighLimit = ProdOilList[1].DenHighLimit;

            ResultList.Add(result2);

            #endregion
            
            #region 返回备用成品油1数据
            SchemeVerify_1Res_3 result3 = new SchemeVerify_1Res_3();//实体，可以理解为一个对象
            /// <summary>
            /// 质量界面
            /// </summary>
            result3.ProdOilName = SchemeBottomList[2].ProdOilName;
            result3.ProdCET = (float)TotalProd1CETMass;
            result3.ProdD50 = (float)TotalProd1D50Mass;
            result3.ProdPOL = (float)TotalProd1POLMass;
            result3.ProdDEN = (float)TotalProd1DENMass;
            /// <summary>
            /// 高地限不分质量和体积
            /// </summary>
            result3.CETLowLimit = ProdOilList[2].CetLowLimit;
            result3.CETHighLimit = ProdOilList[2].CetHighLimit;
            result3.D50LowLimit = ProdOilList[2].D50LowLimit;
            result3.D50HighLimit = ProdOilList[2].D50HighLimit;
            result3.POLLowLimit = ProdOilList[2].PolLowLimit;
            result3.POLHighLimit = ProdOilList[2].PolHighLimit;
            result3.DENLowLimit = ProdOilList[2].DenLowLimit;
            result3.DENHighLimit = ProdOilList[2].DenHighLimit;

            ResultList.Add(result3);

            #endregion

            #region 返回备用成品油2数据
            SchemeVerify_1Res_3 result4 = new SchemeVerify_1Res_3();//实体，可以理解为一个对象
            /// <summary>
            /// 质量界面
            /// </summary>
            result4.ProdOilName = SchemeBottomList[3].ProdOilName;
            result4.ProdCET = (float)TotalProd2CETMass;
            result4.ProdD50 = (float)TotalProd2D50Mass;
            result4.ProdPOL = (float)TotalProd2POLMass;
            result4.ProdDEN = (float)TotalProd2DENMass;
            /// <summary>
            /// 高地限不分质量和体积
            /// </summary>
            result4.CETLowLimit = ProdOilList[3].CetLowLimit;
            result4.CETHighLimit = ProdOilList[3].CetHighLimit;
            result4.D50LowLimit = ProdOilList[3].D50LowLimit;
            result4.D50HighLimit = ProdOilList[3].D50HighLimit;
            result4.POLLowLimit = ProdOilList[3].PolLowLimit;
            result4.POLHighLimit = ProdOilList[3].PolHighLimit;
            result4.DENLowLimit = ProdOilList[3].DenLowLimit;
            result4.DENHighLimit = ProdOilList[3].DenHighLimit;

            ResultList.Add(result4);

            #endregion

            return ResultList;           

        }
        
        public IEnumerable<SchemeVerify_1Res_3> GetSchemeverifyResult1_vol_Property()//场景1体积 成品油属性
        {
            IProdOilConfig _ProdOilConfig = new ProdOilConfig(context);
            IProperty _Property = new PropertyApply(context);
            ICompOilConfig _CompOilConfig = new CompOilConfig(context);
            ISchemeVerify _Schemeverify = new SchemeVerify(context);           

            var CompOilList = _CompOilConfig.GetAllCompOilConfigList().ToList();
            var ProdOilList = _ProdOilConfig.GetAllProdOilConfigList().ToList();
            var PropertyList = _Property.GetAllPropertyList().ToList();
            var SchemeCompOilList = _Schemeverify.GetSchemeVerify1().ToList();//组分油的车柴出柴 质量/体积的质量产量，体积产量
            var SchemeBottomList = _Schemeverify.GetSchemeVerify2().ToList();//成品油的罐底油信息和调合总量

            List<SchemeVerify_1Res_3> ResultList = new List<SchemeVerify_1Res_3>();//列表，里面可以添加很多个对象

            #region 场景1——体积
            #region 车用柴油
            /// <summary>
            /// 车用柴油
            /// </summary>
            double AutoVolProduct = 0;//车用柴油体积产量
            for (int i = 0; i < SchemeCompOilList.Count; i++) 
            {
                AutoVolProduct = AutoVolProduct + SchemeCompOilList[i].AutoVolumeProduct;             
            }

            double AutoMass = 0;//车用柴油总质量
            for (int i = 0; i < SchemeCompOilList.Count; i++)
            {
                AutoMass = AutoMass + SchemeCompOilList[i].AutoVolumeProduct * CompOilList[i].Den / 1000;//单位是吨
            }
            // 车柴十六烷值指数 基于体积
            double AutoCETVol = 0;
            for(int i = 0;i < CompOilList.Count; i++)
            {
                AutoCETVol = AutoCETVol + CompOilList[i].Cet * SchemeCompOilList[i].AutoVolumeProduct;//十六烷值乘以体积
            }

            double TotalAutoCETVol = Math.Round((AutoCETVol + SchemeBottomList[0].BottomVolume * SchemeBottomList[0].CetVol) / (AutoVolProduct + SchemeBottomList[0].BottomVolume), 1); 

            // 车柴50%回收温度 基于体积
            double AutoD50Vol = 0;
            for (int i = 0; i < CompOilList.Count; i++)
            {
                AutoD50Vol = AutoD50Vol + CompOilList[i].D50 * SchemeCompOilList[i].AutoVolumeProduct;//馏程乘以体积
            }

            double TotalAutoD50Vol = Math.Round((AutoD50Vol + SchemeBottomList[0].BottomVolume * SchemeBottomList[0].D50Vol) / (AutoVolProduct + SchemeBottomList[0].BottomVolume));

            // 多环芳烃含量 基于质量
            double AutoPOLVol = 0;
            for (int i = 0; i < CompOilList.Count; i++)
            {
                AutoPOLVol = AutoPOLVol + CompOilList[i].Pol * SchemeCompOilList[i].AutoVolumeProduct * CompOilList[i].Den / 1000;//多芳烃基于质量 单位吨
            }
            double AutoBottomVol = SchemeBottomList[0].BottomVolume;
            double AutoBottomMass_1 = AutoBottomVol * SchemeBottomList[0].DenVol / 1000;//场景1的体积界面下的车柴罐底油质量 单位吨
            double TotalAutoPOLVol = Math.Round((AutoPOLVol + AutoBottomMass_1 * SchemeBottomList[0].PolVol) / (AutoMass + AutoBottomMass_1), 2);

            // 密度 基于质量
            double AutoDENVol = 0;
            for (int i = 0; i < CompOilList.Count; i++)
            {
                AutoDENVol = AutoDENVol + CompOilList[i].Den * SchemeCompOilList[i].AutoVolumeProduct * CompOilList[i].Den / 1000;//密度基于质量 单位吨
            }
            double TotalAutoDENVol = Math.Round((AutoDENVol + AutoBottomMass_1 * SchemeBottomList[0].DenVol) / (AutoMass + AutoBottomMass_1), 1);
            #endregion
            
            #region 出口柴油
            /// <summary>
            /// 出口柴油
            /// </summary>
            double ExpVolProduct = 0;//出口柴油体积产量
            for (int i = 0; i < SchemeCompOilList.Count; i++) 
            {
                ExpVolProduct = ExpVolProduct + SchemeCompOilList[i].ExpVolumeProduct;             
            }

            double ExpMass = 0;//出口柴油总质量
            for (int i = 0; i < SchemeCompOilList.Count; i++)
            {
                ExpMass = ExpMass + SchemeCompOilList[i].ExpVolumeProduct * CompOilList[i].Den / 1000;//单位是吨
            }
            // 出柴十六烷值指数 基于体积
            double ExpCETVol = 0;
            for(int i = 0;i < CompOilList.Count; i++)
            {
                ExpCETVol = ExpCETVol + CompOilList[i].Cet * SchemeCompOilList[i].ExpVolumeProduct;//十六烷值乘以体积
            }

            double TotalExpCETVol = Math.Round((ExpCETVol + SchemeBottomList[1].BottomVolume * SchemeBottomList[1].CetVol) / (ExpVolProduct + SchemeBottomList[1].BottomVolume), 1); 

            // 出柴50%回收温度 基于体积
            double ExpD50Vol = 0;
            for (int i = 0; i < CompOilList.Count; i++)
            {
                ExpD50Vol = ExpD50Vol + CompOilList[i].D50 * SchemeCompOilList[i].ExpVolumeProduct;//馏程乘以体积
            }

            double TotalExpD50Vol = Math.Round((ExpD50Vol + SchemeBottomList[1].BottomVolume * SchemeBottomList[1].D50Vol) / (ExpVolProduct + SchemeBottomList[1].BottomVolume));

            // 多环芳烃含量 基于质量
            double ExpPOLVol = 0;
            for (int i = 0; i < CompOilList.Count; i++)
            {
                ExpPOLVol = ExpPOLVol + CompOilList[i].Pol * SchemeCompOilList[i].ExpVolumeProduct * CompOilList[i].Den / 1000;//多芳烃基于质量 单位吨
            }
            double ExpBottomVol = SchemeBottomList[1].BottomVolume;
            double ExpBottomMass_1 = ExpBottomVol * SchemeBottomList[1].DenVol / 1000;//场景1的体积界面下的车柴罐底油质量 单位吨
            double TotalExpPOLVol = Math.Round((ExpPOLVol + ExpBottomMass_1 * SchemeBottomList[1].PolVol) / (ExpMass + ExpBottomMass_1), 2);

            // 密度 基于质量
            double ExpDENVol = 0;
            for (int i = 0; i < CompOilList.Count; i++)
            {
                ExpDENVol = ExpDENVol + CompOilList[i].Den * SchemeCompOilList[i].ExpVolumeProduct * CompOilList[i].Den / 1000;//密度基于质量 单位吨
            }
            double TotalExpDENVol = Math.Round((ExpDENVol + ExpBottomMass_1 * SchemeBottomList[1].DenVol) / (ExpMass + ExpBottomMass_1), 1);
            #endregion

            #region 备用成品油1
            /// <summary>
            /// 备用成品油1
            /// </summary>
            double Prod1VolProduct = 0;//车用柴油体积产量
            for (int i = 0; i < SchemeCompOilList.Count; i++) 
            {
                Prod1VolProduct = Prod1VolProduct + SchemeCompOilList[i].Prod1VolumeProduct;             
            }

            double Prod1Mass = 0;//车用柴油总质量
            for (int i = 0; i < SchemeCompOilList.Count; i++)
            {
                Prod1Mass = Prod1Mass + SchemeCompOilList[i].Prod1VolumeProduct * CompOilList[i].Den / 1000;//单位是吨
            }
            // 车柴十六烷值指数 基于体积
            double Prod1CETVol = 0;
            for(int i = 0;i < CompOilList.Count; i++)
            {
                Prod1CETVol = Prod1CETVol + CompOilList[i].Cet * SchemeCompOilList[i].Prod1VolumeProduct;//十六烷值乘以体积
            }

            double TotalProd1CETVol = Math.Round((Prod1CETVol + SchemeBottomList[2].BottomVolume * SchemeBottomList[2].CetVol) / (Prod1VolProduct + SchemeBottomList[2].BottomVolume), 1); 

            // 车柴50%回收温度 基于体积
            double Prod1D50Vol = 0;
            for (int i = 0; i < CompOilList.Count; i++)
            {
                Prod1D50Vol = Prod1D50Vol + CompOilList[i].D50 * SchemeCompOilList[i].Prod1VolumeProduct;//馏程乘以体积
            }

            double TotalProd1D50Vol = Math.Round((Prod1D50Vol + SchemeBottomList[2].BottomVolume * SchemeBottomList[2].D50Vol) / (Prod1VolProduct + SchemeBottomList[2].BottomVolume));

            // 多环芳烃含量 基于质量
            double Prod1POLVol = 0;
            for (int i = 0; i < CompOilList.Count; i++)
            {
                Prod1POLVol = Prod1POLVol + CompOilList[i].Pol * SchemeCompOilList[i].Prod1VolumeProduct * CompOilList[i].Den / 1000;//多芳烃基于质量 单位吨
            }
            double Prod1BottomVol = SchemeBottomList[2].BottomVolume;
            double Prod1BottomMass_1 = Prod1BottomVol * SchemeBottomList[2].DenVol / 1000;//场景1的体积界面下的车柴罐底油质量 单位吨
            double TotalProd1POLVol = Math.Round((Prod1POLVol + Prod1BottomMass_1 * SchemeBottomList[2].PolVol) / (Prod1Mass + Prod1BottomMass_1), 2);

            // 密度 基于质量
            double Prod1DENVol = 0;
            for (int i = 0; i < CompOilList.Count; i++)
            {
                Prod1DENVol = Prod1DENVol + CompOilList[i].Den * SchemeCompOilList[i].Prod1VolumeProduct * CompOilList[i].Den / 1000;//密度基于质量 单位吨
            }
            double TotalProd1DENVol = Math.Round((Prod1DENVol + Prod1BottomMass_1 * SchemeBottomList[2].DenVol) / (Prod1Mass + Prod1BottomMass_1), 1);
            #endregion

            #region 备用成品油2
            /// <summary>
            /// 备用成品油2
            /// </summary>
            double Prod2VolProduct = 0;//车用柴油体积产量
            for (int i = 0; i < SchemeCompOilList.Count; i++) 
            {
                Prod2VolProduct = Prod2VolProduct + SchemeCompOilList[i].Prod2VolumeProduct;             
            }

            double Prod2Mass = 0;//车用柴油总质量
            for (int i = 0; i < SchemeCompOilList.Count; i++)
            {
                Prod2Mass = Prod2Mass + SchemeCompOilList[i].Prod2VolumeProduct * CompOilList[i].Den / 1000;//单位是吨
            }
            // 车柴十六烷值指数 基于体积
            double Prod2CETVol = 0;
            for(int i = 0;i < CompOilList.Count; i++)
            {
                Prod2CETVol = Prod2CETVol + CompOilList[i].Cet * SchemeCompOilList[i].Prod2VolumeProduct;//十六烷值乘以体积
            }

            double TotalProd2CETVol = Math.Round((Prod2CETVol + SchemeBottomList[3].BottomVolume * SchemeBottomList[3].CetVol) / (Prod2VolProduct + SchemeBottomList[3].BottomVolume), 1); 

            // 车柴50%回收温度 基于体积
            double Prod2D50Vol = 0;
            for (int i = 0; i < CompOilList.Count; i++)
            {
                Prod2D50Vol = Prod2D50Vol + CompOilList[i].D50 * SchemeCompOilList[i].Prod2VolumeProduct;//馏程乘以体积
            }

            double TotalProd2D50Vol = Math.Round((Prod2D50Vol + SchemeBottomList[3].BottomVolume * SchemeBottomList[3].D50Vol) / (Prod2VolProduct + SchemeBottomList[3].BottomVolume));

            // 多环芳烃含量 基于质量
            double Prod2POLVol = 0;
            for (int i = 0; i < CompOilList.Count; i++)
            {
                Prod2POLVol = Prod2POLVol + CompOilList[i].Pol * SchemeCompOilList[i].Prod2VolumeProduct * CompOilList[i].Den / 1000;//多芳烃基于质量 单位吨
            }
            double Prod2BottomVol = SchemeBottomList[3].BottomVolume;
            double Prod2BottomMass_1 = Prod2BottomVol * SchemeBottomList[3].DenVol / 1000;//场景1的体积界面下的车柴罐底油质量 单位吨
            double TotalProd2POLVol = Math.Round((Prod2POLVol + Prod2BottomMass_1 * SchemeBottomList[3].PolVol) / (Prod2Mass + Prod2BottomMass_1), 2);

            // 密度 基于质量
            double Prod2DENVol = 0;
            for (int i = 0; i < CompOilList.Count; i++)
            {
                Prod2DENVol = Prod2DENVol + CompOilList[i].Den * SchemeCompOilList[i].Prod2VolumeProduct * CompOilList[i].Den / 1000;//密度基于质量 单位吨
            }
            double TotalProd2DENVol = Math.Round((Prod2DENVol + Prod2BottomMass_1 * SchemeBottomList[3].DenVol) / (Prod2Mass + Prod2BottomMass_1), 1);
            #endregion

            #endregion

            #region 返回车柴数据
            SchemeVerify_1Res_3 result1 = new SchemeVerify_1Res_3();//实体，可以理解为一个对象
            /// <summary>
            /// 体积界面
            /// </summary>
            result1.ProdOilName = SchemeBottomList[0].ProdOilName;
            result1.ProdCET = (float)TotalAutoCETVol;
            result1.ProdD50 = (float)TotalAutoD50Vol;
            result1.ProdPOL = (float)TotalAutoPOLVol;
            result1.ProdDEN = (float)TotalAutoDENVol;
            /// <summary>
            /// 高低限不分质量和体积
            /// </summary>
            result1.CETLowLimit = ProdOilList[0].CetLowLimit;
            result1.CETHighLimit = ProdOilList[0].CetHighLimit;
            result1.D50LowLimit = ProdOilList[0].D50LowLimit;
            result1.D50HighLimit = ProdOilList[0].D50HighLimit;
            result1.POLLowLimit = ProdOilList[0].PolLowLimit;
            result1.POLHighLimit = ProdOilList[0].PolHighLimit;
            result1.DENLowLimit = ProdOilList[0].DenLowLimit;
            result1.DENHighLimit = ProdOilList[0].DenHighLimit;

            ResultList.Add(result1);           
            #endregion

            #region 返回出柴数据
            SchemeVerify_1Res_3 result2 = new SchemeVerify_1Res_3();//实体，可以理解为一个对象
            /// <summary>
            /// 体积界面
            /// </summary>
            result2.ProdOilName = SchemeBottomList[1].ProdOilName;
            result2.ProdCET = (float)TotalExpCETVol;
            result2.ProdD50 = (float)TotalExpD50Vol;
            result2.ProdPOL = (float)TotalExpPOLVol;
            result2.ProdDEN = (float)TotalExpDENVol;
            /// <summary>
            /// 高地限不分质量和体积
            /// </summary>
            result2.CETLowLimit = ProdOilList[1].CetLowLimit;
            result2.CETHighLimit = ProdOilList[1].CetHighLimit;
            result2.D50LowLimit = ProdOilList[1].D50LowLimit;
            result2.D50HighLimit = ProdOilList[1].D50HighLimit;
            result2.POLLowLimit = ProdOilList[1].PolLowLimit;
            result2.POLHighLimit = ProdOilList[1].PolHighLimit;
            result2.DENLowLimit = ProdOilList[1].DenLowLimit;
            result2.DENHighLimit = ProdOilList[1].DenHighLimit;

            ResultList.Add(result2);

            #endregion

            #region 返回备用成品油1数据
            SchemeVerify_1Res_3 result3 = new SchemeVerify_1Res_3();//实体，可以理解为一个对象
            /// <summary>
            /// 体积界面
            /// </summary>
            result3.ProdOilName = SchemeBottomList[2].ProdOilName;
            result3.ProdCET = (float)TotalProd1CETVol;
            result3.ProdD50 = (float)TotalProd1D50Vol;
            result3.ProdPOL = (float)TotalProd1POLVol;
            result3.ProdDEN = (float)TotalProd1DENVol;
            /// <summary>
            /// 高低限不分质量和体积
            /// </summary>
            result3.CETLowLimit = ProdOilList[2].CetLowLimit;
            result3.CETHighLimit = ProdOilList[2].CetHighLimit;
            result3.D50LowLimit = ProdOilList[2].D50LowLimit;
            result3.D50HighLimit = ProdOilList[2].D50HighLimit;
            result3.POLLowLimit = ProdOilList[2].PolLowLimit;
            result3.POLHighLimit = ProdOilList[2].PolHighLimit;
            result3.DENLowLimit = ProdOilList[2].DenLowLimit;
            result3.DENHighLimit = ProdOilList[2].DenHighLimit;

            ResultList.Add(result3);           
            #endregion

            #region 返回备用成品油2数据
            SchemeVerify_1Res_3 result4 = new SchemeVerify_1Res_3();//实体，可以理解为一个对象
            /// <summary>
            /// 体积界面
            /// </summary>
            result4.ProdOilName = SchemeBottomList[3].ProdOilName;
            result4.ProdCET = (float)TotalProd2CETVol;
            result4.ProdD50 = (float)TotalProd2D50Vol;
            result4.ProdPOL = (float)TotalProd2POLVol;
            result4.ProdDEN = (float)TotalProd2DENVol;
            /// <summary>
            /// 高低限不分质量和体积
            /// </summary>
            result4.CETLowLimit = ProdOilList[3].CetLowLimit;
            result4.CETHighLimit = ProdOilList[3].CetHighLimit;
            result4.D50LowLimit = ProdOilList[3].D50LowLimit;
            result4.D50HighLimit = ProdOilList[3].D50HighLimit;
            result4.POLLowLimit = ProdOilList[3].PolLowLimit;
            result4.POLHighLimit = ProdOilList[3].PolHighLimit;
            result4.DENLowLimit = ProdOilList[3].DenLowLimit;
            result4.DENHighLimit = ProdOilList[3].DenHighLimit;

            ResultList.Add(result4);           
            #endregion

            return ResultList;
        }

        public IEnumerable<SchemeVerify_2Res_1> GetSchemeverifyResult2_mass_Product()//场景2质量 成品油产量
        {
            IProdOilConfig _ProdOilConfig = new ProdOilConfig(context);
            IProperty _Property = new PropertyApply(context);
            ICompOilConfig _CompOilConfig = new CompOilConfig(context);
            ISchemeVerify _Schemeverify = new SchemeVerify(context);           

            var CompOilList = _CompOilConfig.GetAllCompOilConfigList().ToList();
            var ProdOilList = _ProdOilConfig.GetAllProdOilConfigList().ToList();
            var PropertyList = _Property.GetAllPropertyList().ToList();
            var SchemeCompOilList = _Schemeverify.GetSchemeVerify1().ToList();//组分油的车柴出柴 质量/体积的质量产量，体积产量
            var SchemeBottomList = _Schemeverify.GetSchemeVerify2().ToList();//成品油的罐底油信息和调合总量

            List<SchemeVerify_2Res_1> ResultList = new List<SchemeVerify_2Res_1>();//列表，里面可以添加很多个对象
            /// <summary>
            /// 场景二——质量
            /// </summary>
            float[] AutoQualityProduct = new float[SchemeCompOilList.Count];//暂存车用柴油配方乘以调合总量得出的质量产量
            float[] ExpQualityProduct = new float[SchemeCompOilList.Count];//暂存出口柴油配方乘以调合总量得出的质量产量  
            float[] Prod1QualityProduct = new float[SchemeCompOilList.Count];//暂存配方乘以调合总量得出的质量产量
            float[] Prod2QualityProduct = new float[SchemeCompOilList.Count];//暂存配方乘以调合总量得出的质量产量  
            for (int i = 0; i < SchemeCompOilList.Count; i++)
            {//将配方转化为质量产量
                AutoQualityProduct[i] = SchemeCompOilList[i].AutoFlowPercentMass * SchemeBottomList[0].TotalBlendMass2 / 100;
                ExpQualityProduct[i] = SchemeCompOilList[i].ExpFlowPercentMass * SchemeBottomList[1].TotalBlendMass2 / 100;
                Prod1QualityProduct[i] = SchemeCompOilList[i].Prod1FlowPercentMass * SchemeBottomList[2].TotalBlendMass2 / 100;
                Prod2QualityProduct[i] = SchemeCompOilList[i].Prod2FlowPercentMass * SchemeBottomList[3].TotalBlendMass2 / 100;
            }            
            /// <summary>
            /// 返回成品油数据
            /// </summary>
            for (int i = 0; i < SchemeCompOilList.Count; i++)
            {
                SchemeVerify_2Res_1 temp_result = new SchemeVerify_2Res_1();
                temp_result.ComOilName = SchemeCompOilList[i].ComOilName;
                temp_result.AutoProduct = AutoQualityProduct[i];
                temp_result.ExpProduct = ExpQualityProduct[i];
                temp_result.Prod1Product = Prod1QualityProduct[i];
                temp_result.Prod2Product = Prod2QualityProduct[i]; 
                ResultList.Add(temp_result);
            }
            return ResultList;
        }

        public IEnumerable<SchemeVerify_2Res_1> GetSchemeverifyResult2_vol_Product()//场景2体积 成品油产量
        {
            IProdOilConfig _ProdOilConfig = new ProdOilConfig(context);
            IProperty _Property = new PropertyApply(context);
            ICompOilConfig _CompOilConfig = new CompOilConfig(context);
            ISchemeVerify _Schemeverify = new SchemeVerify(context);           

            var CompOilList = _CompOilConfig.GetAllCompOilConfigList().ToList();
            var ProdOilList = _ProdOilConfig.GetAllProdOilConfigList().ToList();
            var PropertyList = _Property.GetAllPropertyList().ToList();
            var SchemeCompOilList = _Schemeverify.GetSchemeVerify1().ToList();//组分油的车柴出柴 质量/体积的质量产量，体积产量
            var SchemeBottomList = _Schemeverify.GetSchemeVerify2().ToList();//成品油的罐底油信息和调合总量

            List<SchemeVerify_2Res_1> ResultList = new List<SchemeVerify_2Res_1>();//列表，里面可以添加很多个对象
            /// <summary>
            /// 场景二——体积
            /// </summary>
            float[] AutoVolumeProduct = new float[SchemeCompOilList.Count];//暂存车用柴油配方乘以调合总量得出的体积产量
            float[] ExpVolumeProduct = new float[SchemeCompOilList.Count];//暂存出口柴油配方乘以调合总量得出的体积产量   
            float[] Prod1VolumeProduct = new float[SchemeCompOilList.Count];//暂存配方乘以调合总量得出的体积产量
            float[] Prod2VolumeProduct = new float[SchemeCompOilList.Count];//暂存配方乘以调合总量得出的体积产量   
            for (int i = 0; i < SchemeCompOilList.Count; i++)
            {//将配方转化为体积产量
                AutoVolumeProduct[i] = SchemeCompOilList[i].AutoFlowPercentVol * SchemeBottomList[0].TotalBlendVol2 / 100;
                ExpVolumeProduct[i] = SchemeCompOilList[i].ExpFlowPercentVol * SchemeBottomList[1].TotalBlendVol2 / 100;
                Prod1VolumeProduct[i] = SchemeCompOilList[i].Prod1FlowPercentVol * SchemeBottomList[2].TotalBlendVol2 / 100;
                Prod2VolumeProduct[i] = SchemeCompOilList[i].Prod2FlowPercentVol * SchemeBottomList[3].TotalBlendVol2 / 100;
            }             
            /// <summary>
            /// 返回成品油数据
            /// </summary>
            for (int i = 0; i < SchemeCompOilList.Count; i++)
            {
                SchemeVerify_2Res_1 temp_result = new SchemeVerify_2Res_1();
                temp_result.ComOilName = SchemeCompOilList[i].ComOilName;
                temp_result.AutoProduct = AutoVolumeProduct[i];
                temp_result.ExpProduct = ExpVolumeProduct[i];
                temp_result.Prod1Product = Prod1VolumeProduct[i];
                temp_result.Prod2Product = Prod2VolumeProduct[i]; 
                ResultList.Add(temp_result);
            }
            return ResultList;
        }

        public IEnumerable<SchemeVerify_2Res_2> GetSchemeverifyResult2_mass_Property()//场景2质量 成品油属性
        {
            IProdOilConfig _ProdOilConfig = new ProdOilConfig(context);
            IProperty _Property = new PropertyApply(context);
            ICompOilConfig _CompOilConfig = new CompOilConfig(context);
            ISchemeVerify _Schemeverify = new SchemeVerify(context);           

            var CompOilList = _CompOilConfig.GetAllCompOilConfigList().ToList();
            var ProdOilList = _ProdOilConfig.GetAllProdOilConfigList().ToList();
            var PropertyList = _Property.GetAllPropertyList().ToList();
            var SchemeCompOilList = _Schemeverify.GetSchemeVerify1().ToList();//组分油的车柴出柴 质量/体积的质量产量，体积产量
            var SchemeBottomList = _Schemeverify.GetSchemeVerify2().ToList();//成品油的罐底油信息和调合总量

            List<SchemeVerify_2Res_2> ResultList = new List<SchemeVerify_2Res_2>();//列表，里面可以添加很多个对象

            #region 场景2——质量 

            #region 车用柴油
            /// <summary>
            /// 车用柴油
            /// </summary>
            double AutoMassProduct = 0;//车用柴油总质量产量
            double[] AutoQualityProduct = new double[SchemeCompOilList.Count];//暂存质量产量            
            for (int i = 0; i < SchemeCompOilList.Count; i++)//将配方转化为质量产量
            {
                AutoQualityProduct[i] = SchemeCompOilList[i].AutoFlowPercentMass * SchemeBottomList[0].TotalBlendMass2 / 100;
            }           
            for (int i = 0; i < SchemeCompOilList.Count; i++) 
            {
                AutoMassProduct = AutoMassProduct + AutoQualityProduct[i];         
            }

            double AutoVolume = 0;//车用柴油总体积
            for (int i = 0; i < SchemeCompOilList.Count; i++)
            {
                AutoVolume = AutoVolume + AutoQualityProduct[i] / CompOilList[i].Den * 1000;//体积
            }
            // 车柴十六烷值指数 基于体积
            double AutoCETMass = 0;
            for(int i = 0;i < CompOilList.Count; i++)
            {
                AutoCETMass = AutoCETMass + CompOilList[i].Cet * AutoQualityProduct[i] / CompOilList[i].Den * 1000;//十六烷值乘以体积
            }

            double TotalAutoCETMass = Math.Round(AutoCETMass / AutoVolume, 1); 

            // 车柴50%回收温度 基于体积
            double AutoD50Mass = 0;
            for (int i = 0; i < CompOilList.Count; i++)
            {
                AutoD50Mass = AutoD50Mass + CompOilList[i].D50 * AutoQualityProduct[i] / CompOilList[i].Den * 1000;//馏程乘以体积
            }

            double TotalAutoD50Mass = Math.Round(AutoD50Mass / AutoVolume); 

            //double TotalAutoD50Mass = Math.Round((AutoD50Mass + SchemeBottomList[0].BottomMass * 1000 / SchemeBottomList[0].DenMass * SchemeBottomList[0].D50Mass) / (AutoVolume + SchemeBottomList[0].BottomMass * 1000 / SchemeBottomList[0].DenMass), 2);

            // 多环芳烃含量 基于质量
            double AutoPOLMass = 0;
            for (int i = 0; i < CompOilList.Count; i++)
            {
                AutoPOLMass = AutoPOLMass + CompOilList[i].Pol * AutoQualityProduct[i];//多芳烃基于质量
            }
            //double AutoBottomMass = SchemeBottomList[0].BottomMass;
            double TotalAutoPOLMass = Math.Round(AutoPOLMass / AutoMassProduct, 2);

            // 密度 基于质量
            double AutoDENMass = 0;
            for (int i = 0; i < CompOilList.Count; i++)
            {
                AutoDENMass = AutoDENMass + CompOilList[i].Den * AutoQualityProduct[i];//密度乘以质量
            }
           // double TotalAutoDENMass = Math.Round((AutoDENMass + AutoBottomMass * SchemeBottomList[0].DenMass) / (AutoMassProduct + AutoBottomMass), 2);
            double TotalAutoDENMass = Math.Round(AutoDENMass / AutoMassProduct, 1);
            # endregion
            
            #region 出口柴油
            /// <summary>
            /// 出口柴油
            /// </summary>
            double ExpMassProduct = 0;//出口柴油总质量产量
            double[] ExpQualityProduct = new double[SchemeCompOilList.Count];//暂存质量产量  
            for (int i = 0; i < SchemeCompOilList.Count; i++)//将配方转化为质量产量
            {
                ExpQualityProduct[i] = SchemeCompOilList[i].ExpFlowPercentMass * SchemeBottomList[1].TotalBlendMass2 / 100;
            }    
            for (int i = 0; i < SchemeCompOilList.Count; i++) 
            {
                ExpMassProduct = ExpMassProduct + ExpQualityProduct[i];             
            }

            double ExpVolume = 0;//出口柴油总体积
            for (int i = 0; i < SchemeCompOilList.Count; i++)
            {
                ExpVolume = ExpVolume + ExpQualityProduct[i] / CompOilList[i].Den * 1000;//体积
            }
            // 出柴十六烷值指数 基于体积
            double ExpCETMass = 0;
            for(int i = 0;i < CompOilList.Count; i++)
            {
                ExpCETMass = ExpCETMass + CompOilList[i].Cet * ExpQualityProduct[i] / CompOilList[i].Den * 1000;//十六烷值乘以体积
            }

            double TotalExpCETMass = Math.Round(ExpCETMass / ExpVolume, 1); 

            // 出柴50%回收温度 基于体积
            double ExpD50Mass = 0;
            for (int i = 0; i < CompOilList.Count; i++)
            {
                ExpD50Mass = ExpD50Mass + CompOilList[i].D50 * ExpQualityProduct[i] / CompOilList[i].Den * 1000;//馏程乘以体积
            }
            double TotalExpD50Mass = Math.Round(ExpD50Mass / ExpVolume); 
            //double TotalExpD50Mass = Math.Round((ExpD50Mass + SchemeBottomList[1].BottomMass * 1000 / SchemeBottomList[1].DenMass * SchemeBottomList[1].D50Mass) / (ExpVolume + SchemeBottomList[1].BottomMass * 1000 / SchemeBottomList[1].DenMass), 2);

            // 多环芳烃含量 基于质量
            double ExpPOLMass = 0;
            for (int i = 0; i < CompOilList.Count; i++)
            {
                ExpPOLMass = ExpPOLMass + CompOilList[i].Pol * ExpQualityProduct[i];//多芳烃基于质量
            }
            //double ExpBottomMass = SchemeBottomList[1].BottomMass;
           double TotalExpPOLMass = Math.Round(ExpPOLMass / ExpMassProduct, 2);

            // 密度 基于质量
            double ExpDENMass = 0;
            for (int i = 0; i < CompOilList.Count; i++)
            {
                ExpDENMass = ExpDENMass + CompOilList[i].Den * ExpQualityProduct[i];//密度乘以质量
            }
            double TotalExpDENMass = Math.Round(ExpDENMass / ExpMassProduct, 1);

            #region 闪点和粘度
            //     //闪点
            //     //double Sum1_Flash= 0;
            //     double Sum1_index = 0;
            //     //double P1_Bottom_volume = Bottom_FC_1 / list2[0].Bottom_DEN;
            //     double P1_total_volume = P1_Volume + list2[0].Bottom_Volume * 1000 / list2[0].Bottom_DEN;

            //     for (int i = 0; i < CompOilList.Count; i++)
            //     {
            //         Sum1_index = Sum1_index + Math.Pow(0.929, CompOilList[i].Flash) * CompOilList[i].P0_FC *1000 / CompOilList[i].DEN / P1_total_volume;//
            //     }
            //     Sum1_index = Sum1_index + Math.Pow(0.929, list2[0].Bottom_Flash) * list2[0].Bottom_Volume * 1000 / list2[0].Bottom_DEN / P1_total_volume;
            //     //Sum1_Flash = Math.Log(Sum1_index, 0.929);

            //     //double P1_DEN = Math.Round(Sum1_DEN / P1_Weight, 2);//总密度除以总质量
            //     double P1_Flash = Math.Round(Math.Log(Sum1_index, 0.929), 2);

            //    // 粘度
            //     double Sum1_Viscosity = 0;
            //     for (int i = 0; i < CompOilList.Count; i++)
            //     {
            //         Sum1_Viscosity = Sum1_Viscosity + Math.Pow(CompOilList[i].Viscosity, 1.0/3) * CompOilList[i].P0_FC / CompOilList[i].DEN * 1000;//
            //     }
            //     Sum1_Viscosity = (Sum1_Viscosity + Math.Pow(list2[0].Bottom_Viscosity, 1.0/3) * list2[0].Bottom_Volume * 1000 / list2[0].Bottom_DEN) / P1_total_volume;

            //     //double P1_DEN = Math.Round(Sum1_DEN / P1_Weight, 2);//总密度除以总质量
            //     double P1_Viscosity = Math.Round(Math.Pow(Sum1_Viscosity, 3), 2);
            #endregion
            
            #endregion 出口柴油 end

            #region 备用成品油1
            /// <summary>
            /// 备用成品油1
            /// </summary>
            double Prod1MassProduct = 0;//总质量产量
            double[] Prod1QualityProduct = new double[SchemeCompOilList.Count];//暂存质量产量  
            for (int i = 0; i < SchemeCompOilList.Count; i++)//将配方转化为质量产量
            {
                Prod1QualityProduct[i] = SchemeCompOilList[i].Prod1FlowPercentMass * SchemeBottomList[2].TotalBlendMass2 / 100;
            }    
            for (int i = 0; i < SchemeCompOilList.Count; i++) 
            {
                Prod1MassProduct = Prod1MassProduct + Prod1QualityProduct[i];             
            }
            double Prod1Volume = 0;//总体积
            for (int i = 0; i < SchemeCompOilList.Count; i++)
            {
                Prod1Volume = Prod1Volume + Prod1QualityProduct[i] / CompOilList[i].Den * 1000;//体积
            }

            // 十六烷值指数 基于体积
            double Prod1CETMass = 0;
            for(int i = 0;i < CompOilList.Count; i++)
            {
                Prod1CETMass = Prod1CETMass + CompOilList[i].Cet * Prod1QualityProduct[i] / CompOilList[i].Den * 1000;//十六烷值乘以体积
            }

            double TotalProd1CETMass = Math.Round(Prod1CETMass / Prod1Volume, 1); 

            // 50%回收温度 基于体积
            double Prod1D50Mass = 0;
            for (int i = 0; i < CompOilList.Count; i++)
            {
                Prod1D50Mass = Prod1D50Mass + CompOilList[i].D50 * Prod1QualityProduct[i] / CompOilList[i].Den * 1000;//馏程乘以体积
            }

            double TotalProd1D50Mass = Math.Round(Prod1D50Mass / Prod1Volume);

            // 多环芳烃含量 基于质量
            double Prod1POLMass = 0;
            for (int i = 0; i < CompOilList.Count; i++)
            {
                Prod1POLMass = Prod1POLMass + CompOilList[i].Pol * Prod1QualityProduct[i];//多芳烃基于质量
            }
            //double Prod1BottomMass = SchemeBottomList[1].BottomMass;
           double TotalProd1POLMass = Math.Round(Prod1POLMass / Prod1MassProduct, 2);

            // 密度 基于质量
            double Prod1DENMass = 0;
            for (int i = 0; i < CompOilList.Count; i++)
            {
                Prod1DENMass = Prod1DENMass + CompOilList[i].Den * Prod1QualityProduct[i];//密度乘以质量
            }
            double TotalProd1DENMass = Math.Round(Prod1DENMass / Prod1MassProduct, 1);
            
            #endregion 备用成品油1 end

            #region 备用成品油2
            /// <summary>
            /// 备用成品油2
            /// </summary>
            double Prod2MassProduct = 0;//总质量产量
            double[] Prod2QualityProduct = new double[SchemeCompOilList.Count];//暂存质量产量  
            for (int i = 0; i < SchemeCompOilList.Count; i++)//将配方转化为质量产量
            {
                Prod2QualityProduct[i] = SchemeCompOilList[i].Prod2FlowPercentMass * SchemeBottomList[3].TotalBlendMass2 / 100;
            }    
            for (int i = 0; i < SchemeCompOilList.Count; i++) 
            {
                Prod2MassProduct = Prod2MassProduct + Prod2QualityProduct[i];             
            }
            double Prod2Volume = 0;//总体积
            for (int i = 0; i < SchemeCompOilList.Count; i++)
            {
                Prod2Volume = Prod2Volume + Prod2QualityProduct[i] / CompOilList[i].Den * 1000;//体积
            }

            // 十六烷值指数 基于体积
            double Prod2CETMass = 0;
            for(int i = 0;i < CompOilList.Count; i++)
            {
                Prod2CETMass = Prod2CETMass + CompOilList[i].Cet * Prod2QualityProduct[i] / CompOilList[i].Den * 1000;//十六烷值乘以体积
            }

            double TotalProd2CETMass = Math.Round(Prod2CETMass / Prod2Volume, 1); 

            // 50%回收温度 基于体积
            double Prod2D50Mass = 0;
            for (int i = 0; i < CompOilList.Count; i++)
            {
                Prod2D50Mass = Prod2D50Mass + CompOilList[i].D50 * Prod2QualityProduct[i] / CompOilList[i].Den * 1000;//馏程乘以体积
            }

            double TotalProd2D50Mass = Math.Round(Prod2D50Mass / Prod2Volume);

            // 多环芳烃含量 基于质量
            double Prod2POLMass = 0;
            for (int i = 0; i < CompOilList.Count; i++)
            {
                Prod2POLMass = Prod2POLMass + CompOilList[i].Pol * Prod2QualityProduct[i];//多芳烃基于质量
            }
            //double Prod2BottomMass = SchemeBottomList[1].BottomMass;
            double TotalProd2POLMass = Math.Round(Prod2POLMass / Prod2MassProduct, 2);

            // 密度 基于质量
            double Prod2DENMass = 0;
            for (int i = 0; i < CompOilList.Count; i++)
            {
                Prod2DENMass = Prod2DENMass + CompOilList[i].Den * Prod2QualityProduct[i];//密度乘以质量
            }
            double TotalProd2DENMass = Math.Round(Prod2DENMass / Prod2MassProduct, 1);
            
            #endregion 备用成品油2 end           
            
            #endregion 场景2—质量 end

            #region 返回车柴数据
            SchemeVerify_2Res_2 result1 = new SchemeVerify_2Res_2();//实体，可以理解为一个对象
            /// <summary>
            /// 质量界面
            /// </summary>
            result1.ProdOilName = SchemeBottomList[0].ProdOilName;
            result1.ProdCET = (float)TotalAutoCETMass;
            result1.ProdD50 = (float)TotalAutoD50Mass;
            result1.ProdPOL = (float)TotalAutoPOLMass;
            result1.ProdDEN = (float)TotalAutoDENMass;
            /// <summary>
            /// 高低限不分质量和体积
            /// </summary>
            result1.CETLowLimit = ProdOilList[0].CetLowLimit;
            result1.CETHighLimit = ProdOilList[0].CetHighLimit;
            result1.D50LowLimit = ProdOilList[0].D50LowLimit;
            result1.D50HighLimit = ProdOilList[0].D50HighLimit;
            result1.POLLowLimit = ProdOilList[0].PolLowLimit;
            result1.POLHighLimit = ProdOilList[0].PolHighLimit;
            result1.DENLowLimit = ProdOilList[0].DenLowLimit;
            result1.DENHighLimit = ProdOilList[0].DenHighLimit;

            ResultList.Add(result1);           
            #endregion

            #region 返回出柴数据
            SchemeVerify_2Res_2 result2 = new SchemeVerify_2Res_2();//实体，可以理解为一个对象
            /// <summary>
            /// 质量界面
            /// </summary>
            result2.ProdOilName = SchemeBottomList[1].ProdOilName;
            result2.ProdCET = (float)TotalExpCETMass;
            result2.ProdD50 = (float)TotalExpD50Mass;
            result2.ProdPOL = (float)TotalExpPOLMass;
            result2.ProdDEN = (float)TotalExpDENMass;
            /// <summary>
            /// 高地限不分质量和体积
            /// </summary>
            result2.CETLowLimit = ProdOilList[1].CetLowLimit;
            result2.CETHighLimit = ProdOilList[1].CetHighLimit;
            result2.D50LowLimit = ProdOilList[1].D50LowLimit;
            result2.D50HighLimit = ProdOilList[1].D50HighLimit;
            result2.POLLowLimit = ProdOilList[1].PolLowLimit;
            result2.POLHighLimit = ProdOilList[1].PolHighLimit;
            result2.DENLowLimit = ProdOilList[1].DenLowLimit;
            result2.DENHighLimit = ProdOilList[1].DenHighLimit;

            ResultList.Add(result2);

            #endregion
            
            #region 返回备用成品油1数据
            SchemeVerify_2Res_2 result3 = new SchemeVerify_2Res_2();//实体，可以理解为一个对象
            /// <summary>
            /// 质量界面
            /// </summary>
            result3.ProdOilName = SchemeBottomList[2].ProdOilName;
            result3.ProdCET = (float)TotalProd1CETMass;
            result3.ProdD50 = (float)TotalProd1D50Mass;
            result3.ProdPOL = (float)TotalProd1POLMass;
            result3.ProdDEN = (float)TotalProd1DENMass;
            /// <summary>
            /// 高地限不分质量和体积
            /// </summary>
            result3.CETLowLimit = ProdOilList[2].CetLowLimit;
            result3.CETHighLimit = ProdOilList[2].CetHighLimit;
            result3.D50LowLimit = ProdOilList[2].D50LowLimit;
            result3.D50HighLimit = ProdOilList[2].D50HighLimit;
            result3.POLLowLimit = ProdOilList[2].PolLowLimit;
            result3.POLHighLimit = ProdOilList[2].PolHighLimit;
            result3.DENLowLimit = ProdOilList[2].DenLowLimit;
            result3.DENHighLimit = ProdOilList[2].DenHighLimit;

            ResultList.Add(result3);

            #endregion
            
            #region 返回备用成品油2数据
            SchemeVerify_2Res_2 result4 = new SchemeVerify_2Res_2();//实体，可以理解为一个对象
            /// <summary>
            /// 质量界面
            /// </summary>
            result4.ProdOilName = SchemeBottomList[3].ProdOilName;
            result4.ProdCET = (float)TotalProd1CETMass;
            result4.ProdD50 = (float)TotalProd1D50Mass;
            result4.ProdPOL = (float)TotalProd1POLMass;
            result4.ProdDEN = (float)TotalProd1DENMass;
            /// <summary>
            /// 高地限不分质量和体积
            /// </summary>
            result4.CETLowLimit = ProdOilList[3].CetLowLimit;
            result4.CETHighLimit = ProdOilList[3].CetHighLimit;
            result4.D50LowLimit = ProdOilList[3].D50LowLimit;
            result4.D50HighLimit = ProdOilList[3].D50HighLimit;
            result4.POLLowLimit = ProdOilList[3].PolLowLimit;
            result4.POLHighLimit = ProdOilList[3].PolHighLimit;
            result4.DENLowLimit = ProdOilList[3].DenLowLimit;
            result4.DENHighLimit = ProdOilList[3].DenHighLimit;

            ResultList.Add(result4);

            #endregion

            return ResultList;
        }
        
        public IEnumerable<SchemeVerify_2Res_2> GetSchemeverifyResult2_vol_Property()//场景2体积 成品油属性
        {
            IProdOilConfig _ProdOilConfig = new ProdOilConfig(context);
            IProperty _Property = new PropertyApply(context);
            ICompOilConfig _CompOilConfig = new CompOilConfig(context);
            ISchemeVerify _Schemeverify = new SchemeVerify(context);           

            var CompOilList = _CompOilConfig.GetAllCompOilConfigList().ToList();
            var ProdOilList = _ProdOilConfig.GetAllProdOilConfigList().ToList();
            var PropertyList = _Property.GetAllPropertyList().ToList();
            var SchemeCompOilList = _Schemeverify.GetSchemeVerify1().ToList();//组分油的车柴出柴 质量/体积的质量产量，体积产量
            var SchemeBottomList = _Schemeverify.GetSchemeVerify2().ToList();//成品油的罐底油信息和调合总量

            List<SchemeVerify_2Res_2> ResultList = new List<SchemeVerify_2Res_2>();//列表，里面可以添加很多个对象

            #region 场景2——体积

            #region 车用柴油

            double[] AutoVolumeProduct = new double[SchemeCompOilList.Count];//暂存体积产量

            for (int i = 0; i < SchemeCompOilList.Count; i++)//将配方转化为体积产量
            {
                AutoVolumeProduct[i] = SchemeCompOilList[i].AutoFlowPercentVol * SchemeBottomList[0].TotalBlendVol2 / 100;
            } 
            double AutoVolProduct = 0;//车用柴油总体积
            for (int i = 0; i < SchemeCompOilList.Count; i++)
            {
                AutoVolProduct = AutoVolProduct + AutoVolumeProduct[i];//体积
            }

            double AutoMass = 0;//车用柴油总质量
            for (int i = 0; i < SchemeCompOilList.Count; i++)
            {
                AutoMass = AutoMass +  AutoVolumeProduct[i] * CompOilList[i].Den / 1000;//体积
            }

            // 车柴十六烷值指数 基于体积
            double AutoCETVol = 0;
            for(int i = 0;i < CompOilList.Count; i++)
            {
                AutoCETVol = AutoCETVol + CompOilList[i].Cet * AutoVolumeProduct[i];//十六烷值乘以体积
            }

            double TotalAutoCETVol = Math.Round(AutoCETVol / AutoVolProduct, 1); 

            // 车柴50%回收温度 基于体积
            double AutoD50Vol = 0;
            for (int i = 0; i < CompOilList.Count; i++)
            {
                AutoD50Vol = AutoD50Vol + CompOilList[i].D50 * AutoVolumeProduct[i];//馏程乘以体积
            }

            double TotalAutoD50Vol = Math.Round(AutoD50Vol / AutoVolProduct);

            // 多环芳烃含量 基于质量
            double AutoPOLVol = 0;
            for (int i = 0; i < CompOilList.Count; i++)
            {
                AutoPOLVol = AutoPOLVol + CompOilList[i].Pol * AutoVolumeProduct[i] * CompOilList[i].Den / 1000;//多芳烃基于质量 单位吨
            }
            //double AutoBottomVol = SchemeBottomList[0].BottomVolume;
            //double AutoBottomMass_1 = AutoBottomVol * SchemeBottomList[0].DenVol / 1000;//场景1的体积界面下的车柴罐底油质量 单位吨
            double TotalAutoPOLVol = Math.Round(AutoPOLVol / AutoMass, 2);

            // 密度 基于质量
            double AutoDENVol = 0;
            for (int i = 0; i < CompOilList.Count; i++)
            {
                AutoDENVol = AutoDENVol + CompOilList[i].Den * AutoVolumeProduct[i] * CompOilList[i].Den / 1000;//密度基于质量 单位吨
            }
            double TotalAutoDENVol = Math.Round(AutoDENVol / AutoMass, 1);
            
            #endregion 车用柴油 end
            
            #region 出口柴油
            /// <summary>
            /// 出口柴油
            /// </summary>
            double[] ExpVolumeProduct = new double[SchemeCompOilList.Count];//暂存体积产量  
            for (int i = 0; i < SchemeCompOilList.Count; i++)//将配方转化为质量产量
            {
                ExpVolumeProduct[i] = SchemeCompOilList[i].ExpFlowPercentVol * SchemeBottomList[1].TotalBlendVol2 / 100;
            }  
            double ExpVolProduct = 0;//总体积
            for (int i = 0; i < SchemeCompOilList.Count; i++)
            {
                ExpVolProduct = ExpVolProduct + ExpVolumeProduct[i];//体积
            }

            double ExpMass = 0;//总质量
            for (int i = 0; i < SchemeCompOilList.Count; i++)
            {
                ExpMass = ExpMass +  ExpVolumeProduct[i] * CompOilList[i].Den / 1000;//体积
            }
            // 出柴十六烷值指数 基于体积
            double ExpCETVol = 0;
            for(int i = 0;i < CompOilList.Count; i++)
            {
                ExpCETVol = ExpCETVol + CompOilList[i].Cet * ExpVolumeProduct[i];//十六烷值乘以体积
            }

            double TotalExpCETVol = Math.Round(ExpCETVol / ExpVolProduct, 1); 

            // 出柴50%回收温度 基于体积
            double ExpD50Vol = 0;
            for (int i = 0; i < CompOilList.Count; i++)
            {
                ExpD50Vol = ExpD50Vol + CompOilList[i].D50 * ExpVolumeProduct[i];//馏程乘以体积
            }

            double TotalExpD50Vol = Math.Round(ExpD50Vol / ExpVolProduct);

            // 多环芳烃含量 基于质量
            double ExpPOLVol = 0;
            for (int i = 0; i < CompOilList.Count; i++)
            {
                ExpPOLVol = ExpPOLVol + CompOilList[i].Pol * ExpVolumeProduct[i] * CompOilList[i].Den / 1000;//多芳烃基于质量 单位吨
            }
            //double ExpBottomVol = SchemeBottomList[1].BottomVolume;
            //double ExpBottomMass_1 = ExpBottomVol * SchemeBottomList[1].DenVol / 1000;//场景1的体积界面下的车柴罐底油质量 单位吨
            double TotalExpPOLVol = Math.Round(ExpPOLVol / ExpMass, 2);

            // 密度 基于质量
            double ExpDENVol = 0;
            for (int i = 0; i < CompOilList.Count; i++)
            {
                ExpDENVol = ExpDENVol + CompOilList[i].Den * ExpVolumeProduct[i] * CompOilList[i].Den / 1000;//密度基于质量 单位吨
            }
            double TotalExpDENVol = Math.Round(ExpDENVol / ExpMass, 1);
            #endregion 出口柴油 end

            #region 备用成品油1
            /// <summary>
            /// 备用成品油1
            /// </summary>

            double[] Prod1VolumeProduct = new double[SchemeCompOilList.Count];//暂存体积产量  
            for (int i = 0; i < SchemeCompOilList.Count; i++)//将配方转化为质量产量
            {
                Prod1VolumeProduct[i] = SchemeCompOilList[i].Prod1FlowPercentVol * SchemeBottomList[2].TotalBlendVol2 / 100;
            }  
            double Prod1VolProduct = 0;//总体积
            for (int i = 0; i < SchemeCompOilList.Count; i++)
            {
                Prod1VolProduct = Prod1VolProduct + Prod1VolumeProduct[i];//体积
            }

            double Prod1Mass = 0;//总质量
            for (int i = 0; i < SchemeCompOilList.Count; i++)
            {
                Prod1Mass = Prod1Mass +  Prod1VolumeProduct[i] * CompOilList[i].Den / 1000;//体积
            }
            // 十六烷值指数 基于体积
            double Prod1CETVol = 0;
            for(int i = 0;i < CompOilList.Count; i++)
            {
                Prod1CETVol = Prod1CETVol + CompOilList[i].Cet * Prod1VolumeProduct[i];//十六烷值乘以体积
            }

            double TotalProd1CETVol = Math.Round(Prod1CETVol / Prod1VolProduct, 1); 

            // 50%回收温度 基于体积
            double Prod1D50Vol = 0;
            for (int i = 0; i < CompOilList.Count; i++)
            {
                Prod1D50Vol = Prod1D50Vol + CompOilList[i].D50 * Prod1VolumeProduct[i];//馏程乘以体积
            }

            double TotalProd1D50Vol = Math.Round(Prod1D50Vol / Prod1VolProduct);

            // 多环芳烃含量 基于质量
            double Prod1POLVol = 0;
            for (int i = 0; i < CompOilList.Count; i++)
            {
                Prod1POLVol = Prod1POLVol + CompOilList[i].Pol * Prod1VolumeProduct[i] * CompOilList[i].Den / 1000;//多芳烃基于质量 单位吨
            }
            //double Prod1BottomVol = SchemeBottomList[1].BottomVolume;
            //double Prod1BottomMass_1 = Prod1BottomVol * SchemeBottomList[1].DenVol / 1000;//场景1的体积界面下的车柴罐底油质量 单位吨
            double TotalProd1POLVol = Math.Round(Prod1POLVol / Prod1Mass, 2);

            // 密度 基于质量
            double Prod1DENVol = 0;
            for (int i = 0; i < CompOilList.Count; i++)
            {
                Prod1DENVol = Prod1DENVol + CompOilList[i].Den * Prod1VolumeProduct[i] * CompOilList[i].Den / 1000;//密度基于质量 单位吨
            }
            double TotalProd1DENVol = Math.Round(Prod1DENVol / Prod1Mass, 1);
            #endregion 备用成品油1 end

            #region 备用成品油2
            /// <summary>
            /// 出口柴油
            /// </summary>
            double[] Prod2VolumeProduct = new double[SchemeCompOilList.Count];//暂存体积产量  
            for (int i = 0; i < SchemeCompOilList.Count; i++)//将配方转化为体积产量
            {
                Prod2VolumeProduct[i] = SchemeCompOilList[i].Prod2FlowPercentVol * SchemeBottomList[3].TotalBlendVol2 / 100;
            }  
            double Prod2VolProduct = 0;//总体积
            for (int i = 0; i < SchemeCompOilList.Count; i++)
            {
                Prod2VolProduct = Prod2VolProduct + Prod2VolumeProduct[i];//体积
            }

            double Prod2Mass = 0;//总质量
            for (int i = 0; i < SchemeCompOilList.Count; i++)
            {
                Prod2Mass = Prod2Mass +  Prod2VolumeProduct[i] * CompOilList[i].Den / 1000;//体积
            }

            // 出柴十六烷值指数 基于体积
            double Prod2CETVol = 0;
            for(int i = 0;i < CompOilList.Count; i++)
            {
                Prod2CETVol = Prod2CETVol + CompOilList[i].Cet * Prod2VolumeProduct[i];//十六烷值乘以体积
            }

            double TotalProd2CETVol = Math.Round(Prod2CETVol / Prod2VolProduct, 1); 

            // 出柴50%回收温度 基于体积
            double Prod2D50Vol = 0;
            for (int i = 0; i < CompOilList.Count; i++)
            {
                Prod2D50Vol = Prod2D50Vol + CompOilList[i].D50 * Prod2VolumeProduct[i];//馏程乘以体积
            }

            double TotalProd2D50Vol = Math.Round(Prod2D50Vol / Prod2VolProduct);

            // 多环芳烃含量 基于质量
            double Prod2POLVol = 0;
            for (int i = 0; i < CompOilList.Count; i++)
            {
                Prod2POLVol = Prod2POLVol + CompOilList[i].Pol * Prod2VolumeProduct[i] * CompOilList[i].Den / 1000;//多芳烃基于质量 单位吨
            }
            //double Prod2BottomVol = SchemeBottomList[1].BottomVolume;
            //double Prod2BottomMass_1 = Prod2BottomVol * SchemeBottomList[1].DenVol / 1000;//场景1的体积界面下的车柴罐底油质量 单位吨
            double TotalProd2POLVol = Math.Round(Prod2POLVol / Prod2Mass, 2);

            // 密度 基于质量
            double Prod2DENVol = 0;
            for (int i = 0; i < CompOilList.Count; i++)
            {
                Prod2DENVol = Prod2DENVol + CompOilList[i].Den * Prod2VolumeProduct[i] * CompOilList[i].Den / 1000;//密度基于质量 单位吨
            }
            double TotalProd2DENVol = Math.Round(Prod2DENVol / Prod2Mass, 1);
            #endregion 备用成品油2 end

            #endregion 场景2——体积 end

            #region 返回车柴数据
            SchemeVerify_2Res_2 result1 = new SchemeVerify_2Res_2();//实体，可以理解为一个对象
            /// <summary>
            /// 体积界面
            /// </summary>
            result1.ProdOilName = SchemeBottomList[0].ProdOilName;
            result1.ProdCET = (float)TotalAutoCETVol;
            result1.ProdD50 = (float)TotalAutoD50Vol;
            result1.ProdPOL = (float)TotalAutoPOLVol;
            result1.ProdDEN = (float)TotalAutoDENVol;
            /// <summary>
            /// 高低限不分质量和体积
            /// </summary>
            result1.CETLowLimit = ProdOilList[0].CetLowLimit;
            result1.CETHighLimit = ProdOilList[0].CetHighLimit;
            result1.D50LowLimit = ProdOilList[0].D50LowLimit;
            result1.D50HighLimit = ProdOilList[0].D50HighLimit;
            result1.POLLowLimit = ProdOilList[0].PolLowLimit;
            result1.POLHighLimit = ProdOilList[0].PolHighLimit;
            result1.DENLowLimit = ProdOilList[0].DenLowLimit;
            result1.DENHighLimit = ProdOilList[0].DenHighLimit;

            ResultList.Add(result1);           
            #endregion

            #region 返回出柴数据
            SchemeVerify_2Res_2 result2 = new SchemeVerify_2Res_2();//实体，可以理解为一个对象
            /// <summary>
            /// 体积界面
            /// </summary>
            result2.ProdOilName = SchemeBottomList[1].ProdOilName;
            result2.ProdCET = (float)TotalExpCETVol;
            result2.ProdD50 = (float)TotalExpD50Vol;
            result2.ProdPOL = (float)TotalExpPOLVol;
            result2.ProdDEN = (float)TotalExpDENVol;
            /// <summary>
            /// 高地限不分质量和体积
            /// </summary>
            result2.CETLowLimit = ProdOilList[1].CetLowLimit;
            result2.CETHighLimit = ProdOilList[1].CetHighLimit;
            result2.D50LowLimit = ProdOilList[1].D50LowLimit;
            result2.D50HighLimit = ProdOilList[1].D50HighLimit;
            result2.POLLowLimit = ProdOilList[1].PolLowLimit;
            result2.POLHighLimit = ProdOilList[1].PolHighLimit;
            result2.DENLowLimit = ProdOilList[1].DenLowLimit;
            result2.DENHighLimit = ProdOilList[1].DenHighLimit;

            ResultList.Add(result2);

            #endregion

            #region 返回备用成品油1数据
            SchemeVerify_2Res_2 result3 = new SchemeVerify_2Res_2();//实体，可以理解为一个对象
            /// <summary>
            /// 体积界面
            /// </summary>
            result3.ProdOilName = SchemeBottomList[2].ProdOilName;
            result3.ProdCET = (float)TotalProd1CETVol;
            result3.ProdD50 = (float)TotalProd1D50Vol;
            result3.ProdPOL = (float)TotalProd1POLVol;
            result3.ProdDEN = (float)TotalProd1DENVol;
            /// <summary>
            /// 高地限不分质量和体积
            /// </summary>
            result3.CETLowLimit = ProdOilList[2].CetLowLimit;
            result3.CETHighLimit = ProdOilList[2].CetHighLimit;
            result3.D50LowLimit = ProdOilList[2].D50LowLimit;
            result3.D50HighLimit = ProdOilList[2].D50HighLimit;
            result3.POLLowLimit = ProdOilList[2].PolLowLimit;
            result3.POLHighLimit = ProdOilList[2].PolHighLimit;
            result3.DENLowLimit = ProdOilList[2].DenLowLimit;
            result3.DENHighLimit = ProdOilList[2].DenHighLimit;

            ResultList.Add(result3);

            #endregion

            #region 返回备用成品油2数据
            SchemeVerify_2Res_2 result4 = new SchemeVerify_2Res_2();//实体，可以理解为一个对象
            /// <summary>
            /// 体积界面
            /// </summary>
            result4.ProdOilName = SchemeBottomList[3].ProdOilName;
            result4.ProdCET = (float)TotalProd2CETVol;
            result4.ProdD50 = (float)TotalProd2D50Vol;
            result4.ProdPOL = (float)TotalProd2POLVol;
            result4.ProdDEN = (float)TotalProd2DENVol;
            /// <summary>
            /// 高地限不分质量和体积
            /// </summary>
            result4.CETLowLimit = ProdOilList[3].CetLowLimit;
            result4.CETHighLimit = ProdOilList[3].CetHighLimit;
            result4.D50LowLimit = ProdOilList[3].D50LowLimit;
            result4.D50HighLimit = ProdOilList[3].D50HighLimit;
            result4.POLLowLimit = ProdOilList[3].PolLowLimit;
            result4.POLHighLimit = ProdOilList[3].PolHighLimit;
            result4.DENLowLimit = ProdOilList[3].DenLowLimit;
            result4.DENHighLimit = ProdOilList[3].DenHighLimit;

            ResultList.Add(result4);

            #endregion

            return ResultList;
        }
        
        public IEnumerable<SchemeVerify_3Res_1> GetSchemeverifyResult3_mass_Time()//场景3质量 调合时间
        {
            IProdOilConfig _ProdOilConfig = new ProdOilConfig(context);
            IProperty _Property = new PropertyApply(context);
            ICompOilConfig _CompOilConfig = new CompOilConfig(context);
            ISchemeVerify _Schemeverify = new SchemeVerify(context);           

            var CompOilList = _CompOilConfig.GetAllCompOilConfigList().ToList();
            var ProdOilList = _ProdOilConfig.GetAllProdOilConfigList().ToList();
            var PropertyList = _Property.GetAllPropertyList().ToList();
            var SchemeCompOilList = _Schemeverify.GetSchemeVerify1().ToList();//组分油的车柴出柴 质量/体积的质量产量，体积产量
            var SchemeBottomList = _Schemeverify.GetSchemeVerify2().ToList();//成品油的罐底油信息和调合总量

            List<SchemeVerify_3Res_1> ResultList = new List<SchemeVerify_3Res_1>();//列表，里面可以添加很多个对象

            double AutoMassFlow = 0;//车用柴油质量产量
            for (int i = 0; i < SchemeCompOilList.Count; i++) 
            {
                AutoMassFlow = AutoMassFlow + SchemeCompOilList[i].AutoFlowMass;             
            }

            double ExpMassFlow = 0;//出口柴油质量产量
            for (int i = 0; i < SchemeCompOilList.Count; i++) 
            {
                ExpMassFlow = ExpMassFlow + SchemeCompOilList[i].ExpFlowMass;             
            }
            double Prod1MassFlow = 0;//质量产量
            for (int i = 0; i < SchemeCompOilList.Count; i++) 
            {
                Prod1MassFlow = Prod1MassFlow + SchemeCompOilList[i].Prod1FlowMass;             
            }

            double Prod2MassFlow = 0;//质量产量
            for (int i = 0; i < SchemeCompOilList.Count; i++) 
            {
                Prod2MassFlow = Prod2MassFlow + SchemeCompOilList[i].Prod2FlowMass;             
            }

            //车柴
            SchemeVerify_3Res_1 result1 = new SchemeVerify_3Res_1();//实体，可以理解为一个对象
            /// <summary>
            /// 质量界面
            /// </summary>
            result1.ProdOilName = SchemeBottomList[0].ProdOilName;
            result1.Time = (float)Math.Round((SchemeBottomList[0].TotalBlendMass / AutoMassFlow) * 24, 1);

            ResultList.Add(result1);   

            //出柴
            SchemeVerify_3Res_1 result2 = new SchemeVerify_3Res_1();//实体，可以理解为一个对象
            /// <summary>
            /// 质量界面
            /// </summary>
            result2.ProdOilName = SchemeBottomList[1].ProdOilName;
            result2.Time = (float)Math.Round((SchemeBottomList[1].TotalBlendMass / ExpMassFlow) * 24, 1);

            ResultList.Add(result2);

            //备用成品油1
            SchemeVerify_3Res_1 result3 = new SchemeVerify_3Res_1();//实体，可以理解为一个对象
            /// <summary>
            /// 质量界面
            /// </summary>
            result3.ProdOilName = SchemeBottomList[2].ProdOilName;
            result3.Time = (float)Math.Round((SchemeBottomList[2].TotalBlendMass / Prod1MassFlow) * 24, 1);

            ResultList.Add(result3);

            //备用成品油2
            SchemeVerify_3Res_1 result4 = new SchemeVerify_3Res_1();//实体，可以理解为一个对象
            /// <summary>
            /// 质量界面
            /// </summary>
            result4.ProdOilName = SchemeBottomList[3].ProdOilName;
            result4.Time = (float)Math.Round((SchemeBottomList[3].TotalBlendMass / Prod2MassFlow) * 24, 1);

            ResultList.Add(result4);

            return ResultList;
        }
        
        public IEnumerable<SchemeVerify_3Res_1> GetSchemeverifyResult3_vol_Time()//场景3体积 调合时间
        {
            IProdOilConfig _ProdOilConfig = new ProdOilConfig(context);
            IProperty _Property = new PropertyApply(context);
            ICompOilConfig _CompOilConfig = new CompOilConfig(context);
            ISchemeVerify _Schemeverify = new SchemeVerify(context);           

            var CompOilList = _CompOilConfig.GetAllCompOilConfigList().ToList();
            var ProdOilList = _ProdOilConfig.GetAllProdOilConfigList().ToList();
            var PropertyList = _Property.GetAllPropertyList().ToList();
            var SchemeCompOilList = _Schemeverify.GetSchemeVerify1().ToList();//组分油的车柴出柴 质量/体积的质量产量，体积产量
            var SchemeBottomList = _Schemeverify.GetSchemeVerify2().ToList();//成品油的罐底油信息和调合总量

            List<SchemeVerify_3Res_1> ResultList = new List<SchemeVerify_3Res_1>();//列表，里面可以添加很多个对象

            double AutoVolFlow = 0;//车用柴油体积流量
            for (int i = 0; i < SchemeCompOilList.Count; i++) 
            {
                AutoVolFlow = AutoVolFlow + SchemeCompOilList[i].AutoFlowVol;             
            }
            double ExpVolFlow = 0;//出口柴油体积产量
            for (int i = 0; i < SchemeCompOilList.Count; i++) 
            {
                ExpVolFlow = ExpVolFlow + SchemeCompOilList[i].ExpFlowVol;             
            }
            double Prod1VolFlow = 0;//体积流量
            for (int i = 0; i < SchemeCompOilList.Count; i++) 
            {
                Prod1VolFlow = Prod1VolFlow + SchemeCompOilList[i].Prod1FlowVol;             
            }
            double Prod2VolFlow = 0;//体积产量
            for (int i = 0; i < SchemeCompOilList.Count; i++) 
            {
                Prod2VolFlow = Prod2VolFlow + SchemeCompOilList[i].Prod2FlowVol;             
            }

            //车柴
            SchemeVerify_3Res_1 result1 = new SchemeVerify_3Res_1();//实体，可以理解为一个对象
            /// <summary>
            /// 体积界面
            /// </summary>
            result1.ProdOilName = SchemeBottomList[0].ProdOilName;
            result1.Time = (float)Math.Round((SchemeBottomList[0].TotalBlendVol / AutoVolFlow) * 24, 1);

            ResultList.Add(result1);  

            //出柴
            SchemeVerify_3Res_1 result2 = new SchemeVerify_3Res_1();//实体，可以理解为一个对象
            /// <summary>
            /// 体积界面
            /// </summary>
            result2.ProdOilName = SchemeBottomList[1].ProdOilName;
            result2.Time = (float)Math.Round((SchemeBottomList[1].TotalBlendVol / ExpVolFlow) * 24, 1);

            ResultList.Add(result2);

            //备用成品油1
            SchemeVerify_3Res_1 result3 = new SchemeVerify_3Res_1();//实体，可以理解为一个对象
            /// <summary>
            /// 体积界面
            /// </summary>
            result3.ProdOilName = SchemeBottomList[2].ProdOilName;
            result3.Time = (float)Math.Round((SchemeBottomList[2].TotalBlendVol / Prod1VolFlow) * 24, 1);

            ResultList.Add(result3);

            //备用成品油2
            SchemeVerify_3Res_1 result4 = new SchemeVerify_3Res_1();//实体，可以理解为一个对象
            /// <summary>
            /// 体积界面
            /// </summary>
            result4.ProdOilName = SchemeBottomList[3].ProdOilName;
            result4.Time = (float)Math.Round((SchemeBottomList[3].TotalBlendVol / Prod2VolFlow) * 24, 1);

            ResultList.Add(result4);
            
            return ResultList;
        }
        
        public IEnumerable<SchemeVerify_3Res_2> GetSchemeverifyResult3_mass_Product()//场景3质量 成品油产量
        {
            var SchemeCompOilList = context.Schemeverify1s.ToList();
            var SchemeBottomList = context.Schemeverify2s.ToList();
            double AutoMassFlow = 0;//车用柴油质量流量
            for (int i = 0; i < SchemeCompOilList.Count; i++) 
            {
                AutoMassFlow = AutoMassFlow + SchemeCompOilList[i].AutoFlowMass;             
            }
            double ExpMassFlow = 0;//出口柴油质量流量
            for (int i = 0; i < SchemeCompOilList.Count; i++) 
            {
                ExpMassFlow = ExpMassFlow + SchemeCompOilList[i].ExpFlowMass;             
            }
            double Prod1MassFlow = 0;//备用油1质量流量
            for (int i = 0; i < SchemeCompOilList.Count; i++) 
            {
                Prod1MassFlow = Prod1MassFlow + SchemeCompOilList[i].Prod1FlowMass;             
            }
            double Prod2MassFlow = 0;//备用油2质量流量
            for (int i = 0; i < SchemeCompOilList.Count; i++) 
            {
                Prod2MassFlow = Prod2MassFlow + SchemeCompOilList[i].Prod2FlowMass;             
            }

            List<SchemeVerify_3Res_2> ResultList = new List<SchemeVerify_3Res_2>();//列表，里面可以添加很多个对象
            for(int i = 0; i < SchemeCompOilList.Count; i++){
                SchemeVerify_3Res_2 result = new SchemeVerify_3Res_2();//实体，可以理解为一个对象
                result.ComOilName = SchemeCompOilList[i].ComOilName;
                result.AutoProduct = (float)Math.Round(SchemeCompOilList[i].AutoFlowMass / AutoMassFlow * SchemeBottomList[0].TotalBlendMass3, 2);
                result.ExpProduct = (float)Math.Round(SchemeCompOilList[i].ExpFlowMass / ExpMassFlow * SchemeBottomList[1].TotalBlendMass3, 2);
                result.Prod1Product = (float)Math.Round(SchemeCompOilList[i].Prod1FlowMass / Prod1MassFlow * SchemeBottomList[2].TotalBlendMass3, 2);
                result.Prod2Product = (float)Math.Round(SchemeCompOilList[i].Prod2FlowMass / Prod2MassFlow * SchemeBottomList[3].TotalBlendMass3, 2);
                ResultList.Add(result);
            }
            return ResultList;         

        }
        
        public IEnumerable<SchemeVerify_3Res_2> GetSchemeverifyResult3_vol_Product()//场景3体积 成品油产量
        {
            var SchemeCompOilList = context.Schemeverify1s.ToList();
            var SchemeBottomList = context.Schemeverify2s.ToList();
            double AutoVolFlow = 0;//车用柴油体积流量
            for (int i = 0; i < SchemeCompOilList.Count; i++) 
            {
                AutoVolFlow = AutoVolFlow + SchemeCompOilList[i].AutoFlowVol;             
            }
            double ExpVolFlow = 0;//出口柴油体积流量
            for (int i = 0; i < SchemeCompOilList.Count; i++) 
            {
                ExpVolFlow = ExpVolFlow + SchemeCompOilList[i].ExpFlowVol;             
            }
            double Prod1VolFlow = 0;//备用油1体积流量
            for (int i = 0; i < SchemeCompOilList.Count; i++) 
            {
                Prod1VolFlow = Prod1VolFlow + SchemeCompOilList[i].Prod1FlowVol;             
            }
            double Prod2VolFlow = 0;//备用油2体积流量
            for (int i = 0; i < SchemeCompOilList.Count; i++) 
            {
                Prod2VolFlow = Prod2VolFlow + SchemeCompOilList[i].Prod2FlowVol;             
            }

            List<SchemeVerify_3Res_2> ResultList = new List<SchemeVerify_3Res_2>();//列表，里面可以添加很多个对象
            for(int i = 0; i < SchemeCompOilList.Count; i++){
                SchemeVerify_3Res_2 result = new SchemeVerify_3Res_2();//实体，可以理解为一个对象
                result.ComOilName = SchemeCompOilList[i].ComOilName;
                result.AutoProduct = (float)Math.Round(SchemeCompOilList[i].AutoFlowVol / AutoVolFlow * SchemeBottomList[0].TotalBlendVol3, 2);
                result.ExpProduct = (float)Math.Round(SchemeCompOilList[i].ExpFlowVol / ExpVolFlow * SchemeBottomList[1].TotalBlendVol3, 2);
                result.Prod1Product = (float)Math.Round(SchemeCompOilList[i].Prod1FlowVol / Prod1VolFlow * SchemeBottomList[2].TotalBlendVol3, 2);
                result.Prod2Product = (float)Math.Round(SchemeCompOilList[i].Prod2FlowVol / Prod2VolFlow * SchemeBottomList[3].TotalBlendVol3, 2);
                ResultList.Add(result);
            }
            return ResultList;    
        }
        
        public IEnumerable<SchemeVerify_3Res_3> GetSchemeverifyResult3_mass_Property()//场景3质量 成品油属性
        {
            IProdOilConfig _ProdOilConfig = new ProdOilConfig(context);
            IProperty _Property = new PropertyApply(context);
            ICompOilConfig _CompOilConfig = new CompOilConfig(context);
            ISchemeVerify _Schemeverify = new SchemeVerify(context);           

            var CompOilList = _CompOilConfig.GetAllCompOilConfigList().ToList();
            var ProdOilList = _ProdOilConfig.GetAllProdOilConfigList().ToList();
            var PropertyList = _Property.GetAllPropertyList().ToList();
            var SchemeCompOilList = _Schemeverify.GetSchemeVerify1().ToList();//组分油的车柴出柴 质量/体积的质量产量，体积产量
            var SchemeBottomList = _Schemeverify.GetSchemeVerify2().ToList();//成品油的罐底油信息和调合总量

            List<SchemeVerify_3Res_3> ResultList = new List<SchemeVerify_3Res_3>();//列表，里面可以添加很多个对象

            #region 场景3——质量 

            #region 车柴
            /// <summary>
            /// 车用柴油
            /// </summary>
            double AutoMassFlow = 0;//车用柴油总质量流量
            for (int i = 0; i < SchemeCompOilList.Count; i++) 
            {
                AutoMassFlow = AutoMassFlow + SchemeCompOilList[i].AutoFlowMass;             
            }

            double AutoVolume = 0;//车用柴油总体积
            for (int i = 0; i < SchemeCompOilList.Count; i++)
            {
                AutoVolume = AutoVolume + SchemeCompOilList[i].AutoFlowMass / CompOilList[i].Den * 1000;//体积
            }
            // 车柴十六烷值指数 基于体积
            double AutoCETMass = 0;
            for(int i = 0;i < CompOilList.Count; i++)
            {
                AutoCETMass = AutoCETMass + CompOilList[i].Cet * SchemeCompOilList[i].AutoFlowMass / CompOilList[i].Den * 1000;//十六烷值乘以体积
            }

            double TotalAutoCETMass = Math.Round(AutoCETMass / AutoVolume, 1); 
            // 车柴50%回收温度 基于体积
            double AutoD50Mass = 0;
            for (int i = 0; i < CompOilList.Count; i++)
            {
                AutoD50Mass = AutoD50Mass + CompOilList[i].D50 * SchemeCompOilList[i].AutoFlowMass / CompOilList[i].Den * 1000;//馏程乘以体积
            }

            double TotalAutoD50Mass = Math.Round(AutoD50Mass / AutoVolume);

            // 多环芳烃含量 基于质量
            double AutoPOLMass = 0;
            for (int i = 0; i < CompOilList.Count; i++)
            {
                AutoPOLMass = AutoPOLMass + CompOilList[i].Pol * SchemeCompOilList[i].AutoFlowMass;//多芳烃基于质量
            }
            double TotalAutoPOLMass = Math.Round(AutoPOLMass / AutoMassFlow, 2);

            // 密度 基于质量
            double AutoDENMass = 0;
            for (int i = 0; i < CompOilList.Count; i++)
            {
                AutoDENMass = AutoDENMass + CompOilList[i].Den * SchemeCompOilList[i].AutoFlowMass;//密度乘以质量
            }
            double TotalAutoDENMass = Math.Round(AutoDENMass / AutoMassFlow, 1);
            #endregion 车用柴油 end

            #region 出口柴油         
            /// <summary>
            /// 出口柴油
            /// </summary>
            double ExpMassFlow = 0;//出口柴油质量产量
            for (int i = 0; i < SchemeCompOilList.Count; i++) 
            {
                ExpMassFlow = ExpMassFlow + SchemeCompOilList[i].ExpFlowMass;             
            }

            double ExpVolume = 0;//出口柴油总体积
            for (int i = 0; i < SchemeCompOilList.Count; i++)
            {
                ExpVolume = ExpVolume + SchemeCompOilList[i].ExpFlowMass / CompOilList[i].Den * 1000;//体积
            }
            // 出柴十六烷值指数 基于体积
            double ExpCETMass = 0;
            for(int i = 0;i < CompOilList.Count; i++)
            {
                ExpCETMass = ExpCETMass + CompOilList[i].Cet * SchemeCompOilList[i].ExpFlowMass / CompOilList[i].Den * 1000;//十六烷值乘以体积
            }

            double TotalExpCETMass = Math.Round(ExpCETMass / ExpVolume, 1); 

            // 出柴50%回收温度 基于体积
            double ExpD50Mass = 0;
            for (int i = 0; i < CompOilList.Count; i++)
            {
                ExpD50Mass = ExpD50Mass + CompOilList[i].D50 * SchemeCompOilList[i].ExpFlowMass / CompOilList[i].Den * 1000;//馏程乘以体积
            }

            double TotalExpD50Mass = Math.Round(ExpD50Mass / ExpVolume);

            // 多环芳烃含量 基于质量
            double ExpPOLMass = 0;
            for (int i = 0; i < CompOilList.Count; i++)
            {
                ExpPOLMass = ExpPOLMass + CompOilList[i].Pol * SchemeCompOilList[i].ExpFlowMass;//多芳烃基于质量
            }
            double TotalExpPOLMass = Math.Round(ExpPOLMass / ExpMassFlow, 2);

            // 密度 基于质量
            double ExpDENMass = 0;
            for (int i = 0; i < CompOilList.Count; i++)
            {
                ExpDENMass = ExpDENMass + CompOilList[i].Den * SchemeCompOilList[i].ExpFlowMass;//密度乘以质量
            }
            double TotalExpDENMass = Math.Round(ExpDENMass / ExpMassFlow, 1);

            #endregion 出口柴油 end

            #region 备用成品油1         
            /// <summary>
            /// 备用成品油1
            /// </summary>
            double Prod1MassFlow = 0;//出口柴油质量产量
            for (int i = 0; i < SchemeCompOilList.Count; i++) 
            {
                Prod1MassFlow = Prod1MassFlow + SchemeCompOilList[i].Prod1FlowMass;             
            }

            double Prod1Volume = 0;//出口柴油总体积
            for (int i = 0; i < SchemeCompOilList.Count; i++)
            {
                Prod1Volume = Prod1Volume + SchemeCompOilList[i].Prod1FlowMass / CompOilList[i].Den * 1000;//体积
            }
            // 出柴十六烷值指数 基于体积
            double Prod1CETMass = 0;
            for(int i = 0;i < CompOilList.Count; i++)
            {
                Prod1CETMass = Prod1CETMass + CompOilList[i].Cet * SchemeCompOilList[i].Prod1FlowMass / CompOilList[i].Den * 1000;//十六烷值乘以体积
            }

            double TotalProd1CETMass = Math.Round(Prod1CETMass / Prod1Volume, 1); 

            // 出柴50%回收温度 基于体积
            double Prod1D50Mass = 0;
            for (int i = 0; i < CompOilList.Count; i++)
            {
                Prod1D50Mass = Prod1D50Mass + CompOilList[i].D50 * SchemeCompOilList[i].Prod1FlowMass / CompOilList[i].Den * 1000;//馏程乘以体积
            }

            double TotalProd1D50Mass = Math.Round(Prod1D50Mass / Prod1Volume);

            // 多环芳烃含量 基于质量
            double Prod1POLMass = 0;
            for (int i = 0; i < CompOilList.Count; i++)
            {
                Prod1POLMass = Prod1POLMass + CompOilList[i].Pol * SchemeCompOilList[i].Prod1FlowMass;//多芳烃基于质量
            }
            double TotalProd1POLMass = Math.Round(Prod1POLMass / Prod1MassFlow, 2);

            // 密度 基于质量
            double Prod1DENMass = 0;
            for (int i = 0; i < CompOilList.Count; i++)
            {
                Prod1DENMass = Prod1DENMass + CompOilList[i].Den * SchemeCompOilList[i].Prod1FlowMass;//密度乘以质量
            }
            double TotalProd1DENMass = Math.Round(Prod1DENMass / Prod1MassFlow, 1);

            #endregion 出口柴油 end

            #region 备用成品油2         
            /// <summary>
            /// 备用成品油2
            /// </summary>
            double Prod2MassFlow = 0;//质量流量
            for (int i = 0; i < SchemeCompOilList.Count; i++) 
            {
                Prod2MassFlow = Prod2MassFlow + SchemeCompOilList[i].Prod2FlowMass;             
            }

            double Prod2Volume = 0;//总体积
            for (int i = 0; i < SchemeCompOilList.Count; i++)
            {
                Prod2Volume = Prod2Volume + SchemeCompOilList[i].Prod2FlowMass / CompOilList[i].Den * 1000;//体积
            }
            // 出柴十六烷值指数 基于体积
            double Prod2CETMass = 0;
            for(int i = 0;i < CompOilList.Count; i++)
            {
                Prod2CETMass = Prod2CETMass + CompOilList[i].Cet * SchemeCompOilList[i].Prod2FlowMass / CompOilList[i].Den * 1000;//十六烷值乘以体积
            }

            double TotalProd2CETMass = Math.Round(Prod2CETMass / Prod2Volume, 1); 

            // 出柴50%回收温度 基于体积
            double Prod2D50Mass = 0;
            for (int i = 0; i < CompOilList.Count; i++)
            {
                Prod2D50Mass = Prod2D50Mass + CompOilList[i].D50 * SchemeCompOilList[i].Prod2FlowMass / CompOilList[i].Den * 1000;//馏程乘以体积
            }

            double TotalProd2D50Mass = Math.Round(Prod2D50Mass / Prod2Volume);

            // 多环芳烃含量 基于质量
            double Prod2POLMass = 0;
            for (int i = 0; i < CompOilList.Count; i++)
            {
                Prod2POLMass = Prod2POLMass + CompOilList[i].Pol * SchemeCompOilList[i].Prod2FlowMass;//多芳烃基于质量
            }
            double TotalProd2POLMass = Math.Round(Prod2POLMass / Prod2MassFlow, 2);

            // 密度 基于质量
            double Prod2DENMass = 0;
            for (int i = 0; i < CompOilList.Count; i++)
            {
                Prod2DENMass = Prod2DENMass + CompOilList[i].Den * SchemeCompOilList[i].Prod2FlowMass;//密度乘以质量
            }
            double TotalProd2DENMass = Math.Round(Prod2DENMass / Prod2MassFlow, 1);

            #endregion 出口柴油 end
           
            #endregion 场景3——质量 end

            #region 返回车柴数据
            SchemeVerify_3Res_3 result1 = new SchemeVerify_3Res_3();//实体，可以理解为一个对象
            /// <summary>
            /// 质量界面
            /// </summary>
            result1.ProdOilName = SchemeBottomList[0].ProdOilName;
            result1.ProdCET = (float)TotalAutoCETMass;
            result1.ProdD50 = (float)TotalAutoD50Mass;
            result1.ProdPOL = (float)TotalAutoPOLMass;
            result1.ProdDEN = (float)TotalAutoDENMass;
            /// <summary>
            /// 高低限不分质量和体积
            /// </summary>
            result1.CETLowLimit = ProdOilList[0].CetLowLimit;
            result1.CETHighLimit = ProdOilList[0].CetHighLimit;
            result1.D50LowLimit = ProdOilList[0].D50LowLimit;
            result1.D50HighLimit = ProdOilList[0].D50HighLimit;
            result1.POLLowLimit = ProdOilList[0].PolLowLimit;
            result1.POLHighLimit = ProdOilList[0].PolHighLimit;
            result1.DENLowLimit = ProdOilList[0].DenLowLimit;
            result1.DENHighLimit = ProdOilList[0].DenHighLimit;

            ResultList.Add(result1);           
            #endregion

            #region 返回出柴数据
            SchemeVerify_3Res_3 result2 = new SchemeVerify_3Res_3();//实体，可以理解为一个对象
            /// <summary>
            /// 质量界面
            /// </summary>
            result2.ProdOilName = SchemeBottomList[1].ProdOilName;
            result2.ProdCET = (float)TotalExpCETMass;
            result2.ProdD50 = (float)TotalExpD50Mass;
            result2.ProdPOL = (float)TotalExpPOLMass;
            result2.ProdDEN = (float)TotalExpDENMass;
            /// <summary>
            /// 高地限不分质量和体积
            /// </summary>
            result2.CETLowLimit = ProdOilList[1].CetLowLimit;
            result2.CETHighLimit = ProdOilList[1].CetHighLimit;
            result2.D50LowLimit = ProdOilList[1].D50LowLimit;
            result2.D50HighLimit = ProdOilList[1].D50HighLimit;
            result2.POLLowLimit = ProdOilList[1].PolLowLimit;
            result2.POLHighLimit = ProdOilList[1].PolHighLimit;
            result2.DENLowLimit = ProdOilList[1].DenLowLimit;
            result2.DENHighLimit = ProdOilList[1].DenHighLimit;

            ResultList.Add(result2);

            #endregion

            #region 返回备用成品油1数据
            SchemeVerify_3Res_3 result3 = new SchemeVerify_3Res_3();//实体，可以理解为一个对象
            /// <summary>
            /// 质量界面
            /// </summary>
            result3.ProdOilName = SchemeBottomList[2].ProdOilName;
            result3.ProdCET = (float)TotalProd1CETMass;
            result3.ProdD50 = (float)TotalProd1D50Mass;
            result3.ProdPOL = (float)TotalProd1POLMass;
            result3.ProdDEN = (float)TotalProd1DENMass;
            /// <summary>
            /// 高地限不分质量和体积
            /// </summary>
            result3.CETLowLimit = ProdOilList[2].CetLowLimit;
            result3.CETHighLimit = ProdOilList[2].CetHighLimit;
            result3.D50LowLimit = ProdOilList[2].D50LowLimit;
            result3.D50HighLimit = ProdOilList[2].D50HighLimit;
            result3.POLLowLimit = ProdOilList[2].PolLowLimit;
            result3.POLHighLimit = ProdOilList[2].PolHighLimit;
            result3.DENLowLimit = ProdOilList[2].DenLowLimit;
            result3.DENHighLimit = ProdOilList[2].DenHighLimit;

            ResultList.Add(result3);

            #endregion

            #region 返回备用成品油2数据
            SchemeVerify_3Res_3 result4 = new SchemeVerify_3Res_3();//实体，可以理解为一个对象
            /// <summary>
            /// 质量界面
            /// </summary>
            result4.ProdOilName = SchemeBottomList[3].ProdOilName;
            result4.ProdCET = (float)TotalProd2CETMass;
            result4.ProdD50 = (float)TotalProd2D50Mass;
            result4.ProdPOL = (float)TotalProd2POLMass;
            result4.ProdDEN = (float)TotalProd2DENMass;
            /// <summary>
            /// 高地限不分质量和体积
            /// </summary>
            result4.CETLowLimit = ProdOilList[3].CetLowLimit;
            result4.CETHighLimit = ProdOilList[3].CetHighLimit;
            result4.D50LowLimit = ProdOilList[3].D50LowLimit;
            result4.D50HighLimit = ProdOilList[3].D50HighLimit;
            result4.POLLowLimit = ProdOilList[3].PolLowLimit;
            result4.POLHighLimit = ProdOilList[3].PolHighLimit;
            result4.DENLowLimit = ProdOilList[3].DenLowLimit;
            result4.DENHighLimit = ProdOilList[3].DenHighLimit;

            ResultList.Add(result4);

            #endregion

            return ResultList;
        }
        
        public IEnumerable<SchemeVerify_3Res_3> GetSchemeverifyResult3_vol_Property()//场景3体积 成品油属性
        {
            IProdOilConfig _ProdOilConfig = new ProdOilConfig(context);
            IProperty _Property = new PropertyApply(context);
            ICompOilConfig _CompOilConfig = new CompOilConfig(context);
            ISchemeVerify _Schemeverify = new SchemeVerify(context);           

            var CompOilList = _CompOilConfig.GetAllCompOilConfigList().ToList();
            var ProdOilList = _ProdOilConfig.GetAllProdOilConfigList().ToList();
            var PropertyList = _Property.GetAllPropertyList().ToList();
            var SchemeCompOilList = _Schemeverify.GetSchemeVerify1().ToList();//组分油的车柴出柴 质量/体积的质量产量，体积产量
            var SchemeBottomList = _Schemeverify.GetSchemeVerify2().ToList();//成品油的罐底油信息和调合总量

            List<SchemeVerify_3Res_3> ResultList = new List<SchemeVerify_3Res_3>();//列表，里面可以添加很多个对象

            #region 场景3——体积

            #region 车用柴油
            /// <summary>
            /// 车用柴油
            /// </summary>
            double AutoVolFlow = 0;//车用柴油体积流量
            for (int i = 0; i < SchemeCompOilList.Count; i++) 
            {
                AutoVolFlow = AutoVolFlow + SchemeCompOilList[i].AutoFlowVol;             
            }

            double AutoMass = 0;//车用柴油总质量
            for (int i = 0; i < SchemeCompOilList.Count; i++)
            {
                AutoMass = AutoMass + SchemeCompOilList[i].AutoFlowVol * CompOilList[i].Den / 1000;//单位是吨
            }
            // 车柴十六烷值指数 基于体积
            double AutoCETVol = 0;
            for(int i = 0;i < CompOilList.Count; i++)
            {
                AutoCETVol = AutoCETVol + CompOilList[i].Cet * SchemeCompOilList[i].AutoFlowVol;//十六烷值乘以体积
            }

            double TotalAutoCETVol = Math.Round(AutoCETVol / AutoVolFlow, 1); 

            // 车柴50%回收温度 基于体积
            double AutoD50Vol = 0;
            for (int i = 0; i < CompOilList.Count; i++)
            {
                AutoD50Vol = AutoD50Vol + CompOilList[i].D50 * SchemeCompOilList[i].AutoFlowVol;//馏程乘以体积
            }

            double TotalAutoD50Vol = Math.Round(AutoD50Vol  / AutoVolFlow);

            // 多环芳烃含量 基于质量
            double AutoPOLVol = 0;
            for (int i = 0; i < CompOilList.Count; i++)
            {
                AutoPOLVol = AutoPOLVol + CompOilList[i].Pol * SchemeCompOilList[i].AutoFlowVol * CompOilList[i].Den / 1000;//多芳烃基于质量 单位吨
            }
            double TotalAutoPOLVol = Math.Round(AutoPOLVol / AutoMass, 2);

            // 密度 基于质量
            double AutoDENVol = 0;
            for (int i = 0; i < CompOilList.Count; i++)
            {
                AutoDENVol = AutoDENVol + CompOilList[i].Den * SchemeCompOilList[i].AutoFlowVol * CompOilList[i].Den / 1000;//密度基于质量 单位吨
            }
            double TotalAutoDENVol = Math.Round(AutoDENVol / AutoMass, 1);
            
            #endregion

            #region 出口柴油
            /// <summary>
            /// 出口柴油
            /// </summary>
            double ExpVolFlow = 0;//出口柴油体积产量
            for (int i = 0; i < SchemeCompOilList.Count; i++) 
            {
                ExpVolFlow = ExpVolFlow + SchemeCompOilList[i].ExpFlowVol;             
            }

            double ExpMass = 0;//出口柴油总质量
            for (int i = 0; i < SchemeCompOilList.Count; i++)
            {
                ExpMass = ExpMass + SchemeCompOilList[i].ExpFlowVol * CompOilList[i].Den / 1000;//单位是吨
            }
            // 出柴十六烷值指数 基于体积
            double ExpCETVol = 0;
            for(int i = 0;i < CompOilList.Count; i++)
            {
                ExpCETVol = ExpCETVol + CompOilList[i].Cet * SchemeCompOilList[i].ExpFlowVol;//十六烷值乘以体积
            }

            double TotalExpCETVol = Math.Round(ExpCETVol / ExpVolFlow, 1); 

            // 出柴50%回收温度 基于体积
            double ExpD50Vol = 0;
            for (int i = 0; i < CompOilList.Count; i++)
            {
                ExpD50Vol = ExpD50Vol + CompOilList[i].D50 * SchemeCompOilList[i].ExpFlowVol;//馏程乘以体积
            }

            double TotalExpD50Vol = Math.Round(ExpD50Vol / ExpVolFlow);

            // 多环芳烃含量 基于质量
            double ExpPOLVol = 0;
            for (int i = 0; i < CompOilList.Count; i++)
            {
                ExpPOLVol = ExpPOLVol + CompOilList[i].Pol * SchemeCompOilList[i].ExpFlowVol * CompOilList[i].Den / 1000;//多芳烃基于质量 单位吨
            }
            double TotalExpPOLVol = Math.Round(ExpPOLVol / ExpMass, 2);

            // 密度 基于质量
            double ExpDENVol = 0;
            for (int i = 0; i < CompOilList.Count; i++)
            {
                ExpDENVol = ExpDENVol + CompOilList[i].Den * SchemeCompOilList[i].ExpFlowVol * CompOilList[i].Den / 1000;//密度基于质量 单位吨
            }
            double TotalExpDENVol = Math.Round(ExpDENVol/ ExpMass, 1);

            #endregion 出口柴油 end

            #region 备用成品油1
            /// <summary>
            /// 备用成品油1
            /// </summary>
            double Prod1VolFlow = 0;//体积产量
            for (int i = 0; i < SchemeCompOilList.Count; i++) 
            {
                Prod1VolFlow = Prod1VolFlow + SchemeCompOilList[i].Prod1FlowVol;             
            }

            double Prod1Mass = 0;//总质量
            for (int i = 0; i < SchemeCompOilList.Count; i++)
            {
                Prod1Mass = Prod1Mass + SchemeCompOilList[i].Prod1FlowVol * CompOilList[i].Den / 1000;//单位是吨
            }
            // 十六烷值指数 基于体积
            double Prod1CETVol = 0;
            for(int i = 0;i < CompOilList.Count; i++)
            {
                Prod1CETVol = Prod1CETVol + CompOilList[i].Cet * SchemeCompOilList[i].Prod1FlowVol;//十六烷值乘以体积
            }

            double TotalProd1CETVol = Math.Round(Prod1CETVol / Prod1VolFlow, 1); 

            // 50%回收温度 基于体积
            double Prod1D50Vol = 0;
            for (int i = 0; i < CompOilList.Count; i++)
            {
                Prod1D50Vol = Prod1D50Vol + CompOilList[i].D50 * SchemeCompOilList[i].Prod1FlowVol;//馏程乘以体积
            }

            double TotalProd1D50Vol = Math.Round(Prod1D50Vol / Prod1VolFlow);

            // 多环芳烃含量 基于质量
            double Prod1POLVol = 0;
            for (int i = 0; i < CompOilList.Count; i++)
            {
                Prod1POLVol = Prod1POLVol + CompOilList[i].Pol * SchemeCompOilList[i].Prod1FlowVol * CompOilList[i].Den / 1000;//多芳烃基于质量 单位吨
            }
            double TotalProd1POLVol = Math.Round(Prod1POLVol / Prod1Mass, 2);

            // 密度 基于质量
            double Prod1DENVol = 0;
            for (int i = 0; i < CompOilList.Count; i++)
            {
                Prod1DENVol = Prod1DENVol + CompOilList[i].Den * SchemeCompOilList[i].Prod1FlowVol * CompOilList[i].Den / 1000;//密度基于质量 单位吨
            }
            double TotalProd1DENVol = Math.Round(Prod1DENVol/ Prod1Mass, 1);

            #endregion 备用成品油1 end

            #region 备用成品油2
            /// <summary>
            /// 备用成品油2
            /// </summary>
            double Prod2VolFlow = 0;//备用成品油2体积产量
            for (int i = 0; i < SchemeCompOilList.Count; i++) 
            {
                Prod2VolFlow = Prod2VolFlow + SchemeCompOilList[i].Prod2FlowVol;             
            }

            double Prod2Mass = 0;//备用成品油2总质量
            for (int i = 0; i < SchemeCompOilList.Count; i++)
            {
                Prod2Mass = Prod2Mass + SchemeCompOilList[i].Prod2FlowVol * CompOilList[i].Den / 1000;//单位是吨
            }
            // 备用成品油2十六烷值指数 基于体积
            double Prod2CETVol = 0;
            for(int i = 0;i < CompOilList.Count; i++)
            {
                Prod2CETVol = Prod2CETVol + CompOilList[i].Cet * SchemeCompOilList[i].Prod2FlowVol;//十六烷值乘以体积
            }

            double TotalProd2CETVol = Math.Round(Prod2CETVol / Prod2VolFlow, 1); 

            // 备用成品油2 50%回收温度 基于体积
            double Prod2D50Vol = 0;
            for (int i = 0; i < CompOilList.Count; i++)
            {
                Prod2D50Vol = Prod2D50Vol + CompOilList[i].D50 * SchemeCompOilList[i].Prod2FlowVol;//馏程乘以体积
            }

            double TotalProd2D50Vol = Math.Round(Prod2D50Vol / Prod2VolFlow);

            // 多环芳烃含量 基于质量
            double Prod2POLVol = 0;
            for (int i = 0; i < CompOilList.Count; i++)
            {
                Prod2POLVol = Prod2POLVol + CompOilList[i].Pol * SchemeCompOilList[i].Prod2FlowVol * CompOilList[i].Den / 1000;//多芳烃基于质量 单位吨
            }
            double TotalProd2POLVol = Math.Round(Prod2POLVol / Prod2Mass, 2);

            // 密度 基于质量
            double Prod2DENVol = 0;
            for (int i = 0; i < CompOilList.Count; i++)
            {
                Prod2DENVol = Prod2DENVol + CompOilList[i].Den * SchemeCompOilList[i].Prod2FlowVol * CompOilList[i].Den / 1000;//密度基于质量 单位吨
            }
            double TotalProd2DENVol = Math.Round(Prod2DENVol/ Prod2Mass, 1);

            #endregion 备用成品油2 end

            #endregion 场景3——体积 end

            #region 返回车柴数据
            SchemeVerify_3Res_3 result1 = new SchemeVerify_3Res_3();//实体，可以理解为一个对象
            /// <summary>
            /// 体积界面
            /// </summary>
            result1.ProdOilName = SchemeBottomList[0].ProdOilName;
            result1.ProdCET = (float)TotalAutoCETVol;
            result1.ProdD50 = (float)TotalAutoD50Vol;
            result1.ProdPOL = (float)TotalAutoPOLVol;
            result1.ProdDEN = (float)TotalAutoDENVol;
            /// <summary>
            /// 高低限不分质量和体积
            /// </summary>
            result1.CETLowLimit = ProdOilList[0].CetLowLimit;
            result1.CETHighLimit = ProdOilList[0].CetHighLimit;
            result1.D50LowLimit = ProdOilList[0].D50LowLimit;
            result1.D50HighLimit = ProdOilList[0].D50HighLimit;
            result1.POLLowLimit = ProdOilList[0].PolLowLimit;
            result1.POLHighLimit = ProdOilList[0].PolHighLimit;
            result1.DENLowLimit = ProdOilList[0].DenLowLimit;
            result1.DENHighLimit = ProdOilList[0].DenHighLimit;

            ResultList.Add(result1);           
            #endregion

            #region 返回出柴数据
            SchemeVerify_3Res_3 result2 = new SchemeVerify_3Res_3();//实体，可以理解为一个对象
            /// <summary>
            /// 体积界面
            /// </summary>
            result2.ProdOilName = SchemeBottomList[1].ProdOilName;
            result2.ProdCET = (float)TotalExpCETVol;
            result2.ProdD50 = (float)TotalExpD50Vol;
            result2.ProdPOL = (float)TotalExpPOLVol;
            result2.ProdDEN = (float)TotalExpDENVol;
            /// <summary>
            /// 高地限不分质量和体积
            /// </summary>
            result2.CETLowLimit = ProdOilList[1].CetLowLimit;
            result2.CETHighLimit = ProdOilList[1].CetHighLimit;
            result2.D50LowLimit = ProdOilList[1].D50LowLimit;
            result2.D50HighLimit = ProdOilList[1].D50HighLimit;
            result2.POLLowLimit = ProdOilList[1].PolLowLimit;
            result2.POLHighLimit = ProdOilList[1].PolHighLimit;
            result2.DENLowLimit = ProdOilList[1].DenLowLimit;
            result2.DENHighLimit = ProdOilList[1].DenHighLimit;

            ResultList.Add(result2);

            #endregion

            #region 返回备用成品油1数据
            SchemeVerify_3Res_3 result3 = new SchemeVerify_3Res_3();//实体，可以理解为一个对象
            /// <summary>
            /// 体积界面
            /// </summary>
            result3.ProdOilName = SchemeBottomList[2].ProdOilName;
            result3.ProdCET = (float)TotalProd1CETVol;
            result3.ProdD50 = (float)TotalProd1D50Vol;
            result3.ProdPOL = (float)TotalProd1POLVol;
            result3.ProdDEN = (float)TotalProd1DENVol;
            /// <summary>
            /// 高地限不分质量和体积
            /// </summary>
            result3.CETLowLimit = ProdOilList[2].CetLowLimit;
            result3.CETHighLimit = ProdOilList[2].CetHighLimit;
            result3.D50LowLimit = ProdOilList[2].D50LowLimit;
            result3.D50HighLimit = ProdOilList[2].D50HighLimit;
            result3.POLLowLimit = ProdOilList[2].PolLowLimit;
            result3.POLHighLimit = ProdOilList[2].PolHighLimit;
            result3.DENLowLimit = ProdOilList[2].DenLowLimit;
            result3.DENHighLimit = ProdOilList[2].DenHighLimit;

            ResultList.Add(result3);

            #endregion

            #region 返回备用成品油2数据
            SchemeVerify_3Res_3 result4 = new SchemeVerify_3Res_3();//实体，可以理解为一个对象
            /// <summary>
            /// 体积界面
            /// </summary>
            result4.ProdOilName = SchemeBottomList[3].ProdOilName;
            result4.ProdCET = (float)TotalProd2CETVol;
            result4.ProdD50 = (float)TotalProd2D50Vol;
            result4.ProdPOL = (float)TotalProd2POLVol;
            result4.ProdDEN = (float)TotalProd2DENVol;
            /// <summary>
            /// 高地限不分质量和体积
            /// </summary>
            result4.CETLowLimit = ProdOilList[3].CetLowLimit;
            result4.CETHighLimit = ProdOilList[3].CetHighLimit;
            result4.D50LowLimit = ProdOilList[3].D50LowLimit;
            result4.D50HighLimit = ProdOilList[3].D50HighLimit;
            result4.POLLowLimit = ProdOilList[3].PolLowLimit;
            result4.POLHighLimit = ProdOilList[3].PolHighLimit;
            result4.DENLowLimit = ProdOilList[3].DenLowLimit;
            result4.DENHighLimit = ProdOilList[3].DenHighLimit;

            ResultList.Add(result4);

            #endregion

            return ResultList;
        }
        
        
    }
}