using OilBlendSystem.Models.Gas.DataBaseModel;
using OilBlendSystem.Models.Gas.ConstructModel;
using OilBlendSystem.Models;
using Microsoft.EntityFrameworkCore;
using OilBlendSystem.BLL.Interface.Gas;

namespace OilBlendSystem.BLL.Implementation.Gas
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

        public IEnumerable<Schemeverify1_gas> GetSchemeVerify1()
        {
            context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
            return context.Schemeverify1_gases.ToList();
        }

        public IEnumerable<Schemeverify2_gas> GetSchemeVerify2()
        {
            context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
            return context.Schemeverify2_gases.ToList();
        }

        public IEnumerable<GasSchemeVerify_1Res_1> GetSchemeverifyResult1_mass_Time()
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
            double gas92MassProduct = 0;//车用柴油质量产量
            for (int i = 0; i < SchemeCompOilList.Count; i++) 
            {
                gas92MassProduct = gas92MassProduct + SchemeCompOilList[i].gas92QualityProduct;             
            }
            double gas95MassProduct = 0;//出口柴油质量产量
            for (int i = 0; i < SchemeCompOilList.Count; i++) 
            {
                gas95MassProduct = gas95MassProduct + SchemeCompOilList[i].gas95QualityProduct;             
            }
            double gas98MassProduct = 0;//备用油1质量产量
            for (int i = 0; i < SchemeCompOilList.Count; i++) 
            {
                gas98MassProduct = gas98MassProduct + SchemeCompOilList[i].gas98QualityProduct;             
            }
            double gasSelfMassProduct = 0;//备用油2质量产量
            for (int i = 0; i < SchemeCompOilList.Count; i++) 
            {
                gasSelfMassProduct = gasSelfMassProduct + SchemeCompOilList[i].gasSelfQualityProduct;             
            }

            List<GasSchemeVerify_1Res_1> ResultList = new List<GasSchemeVerify_1Res_1>();//列表，里面可以添加很多个对象
            //车柴
            GasSchemeVerify_1Res_1 result1 = new GasSchemeVerify_1Res_1();//实体，可以理解为一个对象
            result1.ProdOilName = ProdOilList[0].ProdOilName;
            result1.Time = (float)Math.Round((SchemeBottomList[0].TotalBlendMass - SchemeBottomList[0].BottomMass) / gas92MassProduct * 24, 1);
            ResultList.Add(result1);
            //出柴
            GasSchemeVerify_1Res_1 result2 = new GasSchemeVerify_1Res_1();//实体，可以理解为一个对象
            result2.ProdOilName = ProdOilList[1].ProdOilName;
            result2.Time = (float)Math.Round((SchemeBottomList[1].TotalBlendMass - SchemeBottomList[1].BottomMass) / gas95MassProduct * 24, 1);
            ResultList.Add(result2);
            //备用油1
            GasSchemeVerify_1Res_1 result3 = new GasSchemeVerify_1Res_1();//实体，可以理解为一个对象
            result3.ProdOilName = ProdOilList[2].ProdOilName;
            result3.Time = (float)Math.Round((SchemeBottomList[2].TotalBlendMass - SchemeBottomList[2].BottomMass) / gas98MassProduct * 24, 1);
            ResultList.Add(result3);
            //备用油2
            GasSchemeVerify_1Res_1 result4 = new GasSchemeVerify_1Res_1();//实体，可以理解为一个对象
            result4.ProdOilName = ProdOilList[3].ProdOilName;
            result4.Time = (float)Math.Round((SchemeBottomList[3].TotalBlendMass - SchemeBottomList[3].BottomMass) / gasSelfMassProduct * 24, 1);
            ResultList.Add(result4);
            return ResultList;

        }

        public IEnumerable<GasSchemeVerify_1Res_1> GetSchemeverifyResult1_vol_Time()
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
            double gas92VolProduct = 0;//车用柴油体积产量
            for (int i = 0; i < SchemeCompOilList.Count; i++) 
            {
                gas92VolProduct = gas92VolProduct + SchemeCompOilList[i].gas92VolumeProduct;             
            }
            double gas95VolProduct = 0;//出口柴油体积产量
            for (int i = 0; i < SchemeCompOilList.Count; i++) 
            {
                gas95VolProduct = gas95VolProduct + SchemeCompOilList[i].gas95VolumeProduct;             
            }
            double gas98VolProduct = 0;//备用油1体积产量
            for (int i = 0; i < SchemeCompOilList.Count; i++) 
            {
                gas98VolProduct = gas98VolProduct + SchemeCompOilList[i].gas98VolumeProduct;             
            }
            double gasSelfVolProduct = 0;//备用油2体积产量
            for (int i = 0; i < SchemeCompOilList.Count; i++) 
            {
                gasSelfVolProduct = gasSelfVolProduct + SchemeCompOilList[i].gasSelfVolumeProduct;             
            }

            List<GasSchemeVerify_1Res_1> ResultList = new List<GasSchemeVerify_1Res_1>();//列表，里面可以添加很多个对象
            //车柴
            GasSchemeVerify_1Res_1 result1 = new GasSchemeVerify_1Res_1();//实体，可以理解为一个对象
            result1.ProdOilName = ProdOilList[0].ProdOilName;
            result1.Time = (float)Math.Round((SchemeBottomList[0].TotalBlendVol - SchemeBottomList[0].BottomVolume) / gas92VolProduct * 24, 1);
            ResultList.Add(result1);
            //出柴
            GasSchemeVerify_1Res_1 result2 = new GasSchemeVerify_1Res_1();//实体，可以理解为一个对象
            result2.ProdOilName = ProdOilList[1].ProdOilName;
            result2.Time = (float)Math.Round((SchemeBottomList[1].TotalBlendVol - SchemeBottomList[1].BottomVolume) / gas95VolProduct * 24, 1);
            ResultList.Add(result2);
            //备用油1
            GasSchemeVerify_1Res_1 result3 = new GasSchemeVerify_1Res_1();//实体，可以理解为一个对象
            result3.ProdOilName = ProdOilList[2].ProdOilName;
            result3.Time = (float)Math.Round((SchemeBottomList[2].TotalBlendVol - SchemeBottomList[2].BottomVolume) / gas98VolProduct * 24, 1);
            ResultList.Add(result3);
            //备用油2
            GasSchemeVerify_1Res_1 result4 = new GasSchemeVerify_1Res_1();//实体，可以理解为一个对象
            result4.ProdOilName = ProdOilList[3].ProdOilName;
            result4.Time = (float)Math.Round((SchemeBottomList[3].TotalBlendVol - SchemeBottomList[3].BottomVolume) / gasSelfVolProduct * 24, 1);
            ResultList.Add(result4);
            return ResultList;

        }

        public IEnumerable<GasSchemeVerify_1Res_2> GetSchemeverifyResult1_mass_Product()//场景1质量 成品油产量
        {
            var SchemeCompOilList = context.Schemeverify1_gases.ToList();
            var SchemeBottomList = context.Schemeverify2_gases.ToList();
            double gas92MassProduct = 0;//车用柴油质量产量
            for (int i = 0; i < SchemeCompOilList.Count; i++) 
            {
                gas92MassProduct = gas92MassProduct + SchemeCompOilList[i].gas92QualityProduct;             
            }
            double gas95MassProduct = 0;//出口柴油质量产量
            for (int i = 0; i < SchemeCompOilList.Count; i++) 
            {
                gas95MassProduct = gas95MassProduct + SchemeCompOilList[i].gas95QualityProduct;             
            }
            double gas98MassProduct = 0;//备用油1质量产量
            for (int i = 0; i < SchemeCompOilList.Count; i++) 
            {
                gas98MassProduct = gas98MassProduct + SchemeCompOilList[i].gas98QualityProduct;             
            }
            double gasSelfMassProduct = 0;//备用油2质量产量
            for (int i = 0; i < SchemeCompOilList.Count; i++) 
            {
                gasSelfMassProduct = gasSelfMassProduct + SchemeCompOilList[i].gasSelfQualityProduct;             
            }

            List<GasSchemeVerify_1Res_2> ResultList = new List<GasSchemeVerify_1Res_2>();//列表，里面可以添加很多个对象
            for(int i = 0; i < SchemeCompOilList.Count; i++){
                GasSchemeVerify_1Res_2 result = new GasSchemeVerify_1Res_2();//实体，可以理解为一个对象
                result.ComOilName = SchemeCompOilList[i].ComOilName;
                result.gas92Product = (float)Math.Round(SchemeCompOilList[i].gas92QualityProduct / gas92MassProduct * (SchemeBottomList[0].TotalBlendMass - SchemeBottomList[0].BottomMass), 2);
                result.gas95Product = (float)Math.Round(SchemeCompOilList[i].gas95QualityProduct / gas95MassProduct * (SchemeBottomList[1].TotalBlendMass - SchemeBottomList[1].BottomMass), 2);
                result.gas98Product = (float)Math.Round(SchemeCompOilList[i].gas98QualityProduct / gas98MassProduct * (SchemeBottomList[2].TotalBlendMass - SchemeBottomList[2].BottomMass), 2);
                result.gasSelfProduct = (float)Math.Round(SchemeCompOilList[i].gasSelfQualityProduct / gasSelfMassProduct * (SchemeBottomList[3].TotalBlendMass - SchemeBottomList[3].BottomMass), 2);
                ResultList.Add(result);
            }
            return ResultList;         

        }

        public IEnumerable<GasSchemeVerify_1Res_2> GetSchemeverifyResult1_vol_Product()//场景1体积 成品油产量
        {
            var SchemeCompOilList = context.Schemeverify1_gases.ToList();
            var SchemeBottomList = context.Schemeverify2_gases.ToList();
            double gas92VolProduct = 0;//车用柴油体积产量
            for (int i = 0; i < SchemeCompOilList.Count; i++) 
            {
                gas92VolProduct = gas92VolProduct + SchemeCompOilList[i].gas92VolumeProduct;             
            }
            double gas95VolProduct = 0;//出口柴油体积产量
            for (int i = 0; i < SchemeCompOilList.Count; i++) 
            {
                gas95VolProduct = gas95VolProduct + SchemeCompOilList[i].gas95VolumeProduct;             
            }
            double gas98VolProduct = 0;//备用油1体积产量
            for (int i = 0; i < SchemeCompOilList.Count; i++) 
            {
                gas98VolProduct = gas98VolProduct + SchemeCompOilList[i].gas98VolumeProduct;             
            }
            double gasSelfVolProduct = 0;//备用油2体积产量
            for (int i = 0; i < SchemeCompOilList.Count; i++) 
            {
                gasSelfVolProduct = gasSelfVolProduct + SchemeCompOilList[i].gasSelfVolumeProduct;             
            }

            List<GasSchemeVerify_1Res_2> ResultList = new List<GasSchemeVerify_1Res_2>();//列表，里面可以添加很多个对象
            for(int i = 0; i < SchemeCompOilList.Count; i++){
                GasSchemeVerify_1Res_2 result = new GasSchemeVerify_1Res_2();//实体，可以理解为一个对象
                result.ComOilName = SchemeCompOilList[i].ComOilName;
                result.gas92Product = (float)Math.Round(SchemeCompOilList[i].gas92VolumeProduct / gas92VolProduct * (SchemeBottomList[0].TotalBlendVol - SchemeBottomList[0].BottomVolume), 2);
                result.gas95Product = (float)Math.Round(SchemeCompOilList[i].gas95VolumeProduct / gas95VolProduct * (SchemeBottomList[1].TotalBlendVol - SchemeBottomList[1].BottomVolume), 2);
                result.gas98Product = (float)Math.Round(SchemeCompOilList[i].gas98VolumeProduct / gas98VolProduct * (SchemeBottomList[2].TotalBlendVol - SchemeBottomList[2].BottomVolume), 2);
                result.gasSelfProduct = (float)Math.Round(SchemeCompOilList[i].gasSelfVolumeProduct / gasSelfVolProduct * (SchemeBottomList[3].TotalBlendVol - SchemeBottomList[3].BottomVolume), 2);
                ResultList.Add(result);
            }
            return ResultList;    
        }

        public IEnumerable<GasSchemeVerify_1Res_3> GetSchemeverifyResult1_mass_Property()//场景1质量 成品油属性
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


            List<GasSchemeVerify_1Res_3> ResultList = new List<GasSchemeVerify_1Res_3>();//列表，里面可以添加很多个对象

            #region 场景1——质量 
            #region 车用柴油
            /// <summary>
            /// 车用柴油
            /// </summary>
            double gas92MassProduct = 0;//车用柴油质量产量
            for (int i = 0; i < SchemeCompOilList.Count; i++) 
            {
                gas92MassProduct = gas92MassProduct + SchemeCompOilList[i].gas92QualityProduct;             
            }

            double gas92Volume = 0;//车用柴油总体积
            for (int i = 0; i < SchemeCompOilList.Count; i++)
            {
                gas92Volume = gas92Volume + SchemeCompOilList[i].gas92QualityProduct / CompOilList[i].den * 1000;//体积
            }
            // 车柴十六烷值指数 基于体积
            double gas92ronMass = 0;
            for(int i = 0;i < CompOilList.Count; i++)
            {
                gas92ronMass = gas92ronMass + CompOilList[i].ron * SchemeCompOilList[i].gas92QualityProduct / CompOilList[i].den * 1000;//十六烷值乘以体积
            }

            double Totalgas92ronMass = Math.Round((gas92ronMass + SchemeBottomList[0].BottomMass * 1000 / SchemeBottomList[0].denMass * SchemeBottomList[0].ronMass) / (gas92Volume + SchemeBottomList[0].BottomMass * 1000 / SchemeBottomList[0].denMass), 1); 

            // 车柴50%回收温度 基于体积
            double gas92t50Mass = 0;
            for (int i = 0; i < CompOilList.Count; i++)
            {
                gas92t50Mass = gas92t50Mass + CompOilList[i].t50 * SchemeCompOilList[i].gas92QualityProduct / CompOilList[i].den * 1000;//馏程乘以体积
            }

            double Totalgas92t50Mass = Math.Round((gas92t50Mass + SchemeBottomList[0].BottomMass * 1000 / SchemeBottomList[0].denMass * SchemeBottomList[0].t50Mass) / (gas92Volume + SchemeBottomList[0].BottomMass * 1000 / SchemeBottomList[0].denMass));

            // 多环芳烃含量 基于质量
            double gas92sufMass = 0;
            for (int i = 0; i < CompOilList.Count; i++)
            {
                gas92sufMass = gas92sufMass + CompOilList[i].suf * SchemeCompOilList[i].gas92QualityProduct;//多芳烃基于质量
            }
            double gas92BottomMass = SchemeBottomList[0].BottomMass;
            double Totalgas92sufMass = Math.Round((gas92sufMass + gas92BottomMass * SchemeBottomList[0].sufMass) / (gas92MassProduct + gas92BottomMass), 2);

            // 密度 基于质量
            double gas92denMass = 0;
            for (int i = 0; i < CompOilList.Count; i++)
            {
                gas92denMass = gas92denMass + CompOilList[i].den * SchemeCompOilList[i].gas92QualityProduct;//密度乘以质量
            }
            double Totalgas92denMass = Math.Round((gas92denMass + gas92BottomMass * SchemeBottomList[0].denMass) / (gas92MassProduct + gas92BottomMass), 1);
            
            #endregion

            #region 出口柴油
            /// <summary>
            /// 出口柴油
            /// </summary>
            double gas95MassProduct = 0;//出口柴油质量产量
            for (int i = 0; i < SchemeCompOilList.Count; i++) 
            {
                gas95MassProduct = gas95MassProduct + SchemeCompOilList[i].gas95QualityProduct;             
            }

            double gas95Volume = 0;//出口柴油总体积
            for (int i = 0; i < SchemeCompOilList.Count; i++)
            {
                gas95Volume = gas95Volume + SchemeCompOilList[i].gas95QualityProduct / CompOilList[i].den * 1000;//体积
            }
            // 出柴十六烷值指数 基于体积
            double gas95ronMass = 0;
            for(int i = 0;i < CompOilList.Count; i++)
            {
                gas95ronMass = gas95ronMass + CompOilList[i].ron * SchemeCompOilList[i].gas95QualityProduct / CompOilList[i].den * 1000;//十六烷值乘以体积
            }

            double Totalgas95ronMass = Math.Round((gas95ronMass + SchemeBottomList[1].BottomMass * 1000 / SchemeBottomList[1].denMass * SchemeBottomList[1].ronMass) / (gas95Volume + SchemeBottomList[1].BottomMass * 1000 / SchemeBottomList[1].denMass), 1); 

            // 出柴50%回收温度 基于体积
            double gas95t50Mass = 0;
            for (int i = 0; i < CompOilList.Count; i++)
            {
                gas95t50Mass = gas95t50Mass + CompOilList[i].t50 * SchemeCompOilList[i].gas95QualityProduct / CompOilList[i].den * 1000;//馏程乘以体积
            }

            double Totalgas95t50Mass = Math.Round((gas95t50Mass + SchemeBottomList[1].BottomMass * 1000 / SchemeBottomList[1].denMass * SchemeBottomList[1].t50Mass) / (gas95Volume + SchemeBottomList[1].BottomMass * 1000 / SchemeBottomList[1].denMass));

            // 多环芳烃含量 基于质量
            double gas95sufMass = 0;
            for (int i = 0; i < CompOilList.Count; i++)
            {
                gas95sufMass = gas95sufMass + CompOilList[i].suf * SchemeCompOilList[i].gas95QualityProduct;//多芳烃基于质量
            }
            double gas95BottomMass = SchemeBottomList[1].BottomMass;
            double Totalgas95sufMass = Math.Round((gas95sufMass + gas95BottomMass * SchemeBottomList[1].sufMass) / (gas95MassProduct + gas95BottomMass), 2);

            // 密度 基于质量
            double gas95denMass = 0;
            for (int i = 0; i < CompOilList.Count; i++)
            {
                gas95denMass = gas95denMass + CompOilList[i].den * SchemeCompOilList[i].gas95QualityProduct;//密度乘以质量
            }
            double Totalgas95denMass = Math.Round((gas95denMass + gas95BottomMass * SchemeBottomList[1].denMass) / (gas95MassProduct + gas95BottomMass), 1);
            #endregion

            #region 备用成品油1
            /// <summary>
            /// 备用成品油1
            /// </summary>    
            double gas98MassProduct = 0;//车用柴油质量产量
            for (int i = 0; i < SchemeCompOilList.Count; i++) 
            {
                gas98MassProduct = gas98MassProduct + SchemeCompOilList[i].gas98QualityProduct;             
            }

            double gas98Volume = 0;//车用柴油总体积
            for (int i = 0; i < SchemeCompOilList.Count; i++)
            {
                gas98Volume = gas98Volume + SchemeCompOilList[i].gas98QualityProduct / CompOilList[i].den * 1000;//体积
            }
            // 车柴十六烷值指数 基于体积
            double gas98ronMass = 0;
            for(int i = 0;i < CompOilList.Count; i++)
            {
                gas98ronMass = gas98ronMass + CompOilList[i].ron * SchemeCompOilList[i].gas98QualityProduct / CompOilList[i].den * 1000;//十六烷值乘以体积
            }

            double Totalgas98ronMass = Math.Round((gas98ronMass + SchemeBottomList[2].BottomMass * 1000 / SchemeBottomList[2].denMass * SchemeBottomList[2].ronMass) / (gas98Volume + SchemeBottomList[2].BottomMass * 1000 / SchemeBottomList[2].denMass), 1); 

            // 车柴50%回收温度 基于体积
            double gas98t50Mass = 0;
            for (int i = 0; i < CompOilList.Count; i++)
            {
                gas98t50Mass = gas98t50Mass + CompOilList[i].t50 * SchemeCompOilList[i].gas98QualityProduct / CompOilList[i].den * 1000;//馏程乘以体积
            }

            double Totalgas98t50Mass = Math.Round((gas98t50Mass + SchemeBottomList[2].BottomMass * 1000 / SchemeBottomList[2].denMass * SchemeBottomList[2].t50Mass) / (gas98Volume + SchemeBottomList[2].BottomMass * 1000 / SchemeBottomList[2].denMass));

            // 多环芳烃含量 基于质量
            double gas98sufMass = 0;
            for (int i = 0; i < CompOilList.Count; i++)
            {
                gas98sufMass = gas98sufMass + CompOilList[i].suf * SchemeCompOilList[i].gas98QualityProduct;//多芳烃基于质量
            }
            double gas98BottomMass = SchemeBottomList[2].BottomMass;
            double Totalgas98sufMass = Math.Round((gas98sufMass + gas98BottomMass * SchemeBottomList[2].sufMass) / (gas98MassProduct + gas98BottomMass), 2);

            // 密度 基于质量
            double gas98denMass = 0;
            for (int i = 0; i < CompOilList.Count; i++)
            {
                gas98denMass = gas98denMass + CompOilList[i].den * SchemeCompOilList[i].gas98QualityProduct;//密度乘以质量
            }
            double Totalgas98denMass = Math.Round((gas98denMass + gas98BottomMass * SchemeBottomList[2].denMass) / (gas98MassProduct + gas98BottomMass), 1);

            #endregion    

            #region 备用成品油2
            /// <summary>
            /// 备用成品油2
            /// </summary>
            double gasSelfMassProduct = 0;//车用柴油质量产量
            for (int i = 0; i < SchemeCompOilList.Count; i++) 
            {
                gasSelfMassProduct = gasSelfMassProduct + SchemeCompOilList[i].gasSelfQualityProduct;             
            }

            double gasSelfVolume = 0;//车用柴油总体积
            for (int i = 0; i < SchemeCompOilList.Count; i++)
            {
                gasSelfVolume = gasSelfVolume + SchemeCompOilList[i].gasSelfQualityProduct / CompOilList[i].den * 1000;//体积
            }
            // 车柴十六烷值指数 基于体积
            double gasSelfronMass = 0;
            for(int i = 0;i < CompOilList.Count; i++)
            {
                gasSelfronMass = gasSelfronMass + CompOilList[i].ron * SchemeCompOilList[i].gasSelfQualityProduct / CompOilList[i].den * 1000;//十六烷值乘以体积
            }

            double TotalgasSelfronMass = Math.Round((gasSelfronMass + SchemeBottomList[3].BottomMass * 1000 / SchemeBottomList[3].denMass * SchemeBottomList[3].ronMass) / (gasSelfVolume + SchemeBottomList[3].BottomMass * 1000 / SchemeBottomList[3].denMass), 1); 

            // 车柴50%回收温度 基于体积
            double gasSelft50Mass = 0;
            for (int i = 0; i < CompOilList.Count; i++)
            {
                gasSelft50Mass = gasSelft50Mass + CompOilList[i].t50 * SchemeCompOilList[i].gasSelfQualityProduct / CompOilList[i].den * 1000;//馏程乘以体积
            }

            double TotalgasSelft50Mass = Math.Round((gasSelft50Mass + SchemeBottomList[3].BottomMass * 1000 / SchemeBottomList[3].denMass * SchemeBottomList[3].t50Mass) / (gasSelfVolume + SchemeBottomList[3].BottomMass * 1000 / SchemeBottomList[3].denMass));

            // 多环芳烃含量 基于质量
            double gasSelfsufMass = 0;
            for (int i = 0; i < CompOilList.Count; i++)
            {
                gasSelfsufMass = gasSelfsufMass + CompOilList[i].suf * SchemeCompOilList[i].gasSelfQualityProduct;//多芳烃基于质量
            }
            double gasSelfBottomMass = SchemeBottomList[2].BottomMass;
            double TotalgasSelfsufMass = Math.Round((gasSelfsufMass + gasSelfBottomMass * SchemeBottomList[3].sufMass) / (gasSelfMassProduct + gasSelfBottomMass), 2);

            // 密度 基于质量
            double gasSelfdenMass = 0;
            for (int i = 0; i < CompOilList.Count; i++)
            {
                gasSelfdenMass = gasSelfdenMass + CompOilList[i].den * SchemeCompOilList[i].gasSelfQualityProduct;//密度乘以质量
            }
            double TotalgasSelfdenMass = Math.Round((gasSelfdenMass + gasSelfBottomMass * SchemeBottomList[3].denMass) / (gasSelfMassProduct + gasSelfBottomMass), 1);
            
            #endregion

            #region 闪点和粘度
            //     //闪点
            //     //double Sum1_Flash= 0;
            //     double Sum1_index = 0;
            //     //double P1_Bottom_volume = Bottom_FC_1 / list2[0].Bottom_den;
            //     double P1_total_volume = P1_Volume + list2[0].Bottom_Volume * 1000 / list2[0].Bottom_den;

            //     for (int i = 0; i < CompOilList.Count; i++)
            //     {
            //         Sum1_index = Sum1_index + Math.Pow(0.929, CompOilList[i].Flash) * CompOilList[i].P0_FC *1000 / CompOilList[i].den / P1_total_volume;//
            //     }
            //     Sum1_index = Sum1_index + Math.Pow(0.929, list2[0].Bottom_Flash) * list2[0].Bottom_Volume * 1000 / list2[0].Bottom_den / P1_total_volume;
            //     //Sum1_Flash = Math.Log(Sum1_index, 0.929);

            //     //double P1_den = Math.Round(Sum1_den / P1_Weight, 2);//总密度除以总质量
            //     double P1_Flash = Math.Round(Math.Log(Sum1_index, 0.929), 2);

            //    // 粘度
            //     double Sum1_Viscosity = 0;
            //     for (int i = 0; i < CompOilList.Count; i++)
            //     {
            //         Sum1_Viscosity = Sum1_Viscosity + Math.Pow(CompOilList[i].Viscosity, 1.0/3) * CompOilList[i].P0_FC / CompOilList[i].den * 1000;//
            //     }
            //     Sum1_Viscosity = (Sum1_Viscosity + Math.Pow(list2[0].Bottom_Viscosity, 1.0/3) * list2[0].Bottom_Volume * 1000 / list2[0].Bottom_den) / P1_total_volume;

            //     //double P1_den = Math.Round(Sum1_den / P1_Weight, 2);//总密度除以总质量
            //     double P1_Viscosity = Math.Round(Math.Pow(Sum1_Viscosity, 3), 2);
            #endregion
            #endregion

            #region 返回车柴数据
            GasSchemeVerify_1Res_3 result1 = new GasSchemeVerify_1Res_3();//实体，可以理解为一个对象
            /// <summary>
            /// 质量界面
            /// </summary>
            result1.ProdOilName = SchemeBottomList[0].ProdOilName;
            result1.Prodron = (float)Totalgas92ronMass;
            result1.Prodt50 = (float)Totalgas92t50Mass;
            result1.Prodsuf = (float)Totalgas92sufMass;
            result1.Prodden = (float)Totalgas92denMass;
            /// <summary>
            /// 高低限不分质量和体积
            /// </summary>
            result1.ronLowLimit = ProdOilList[0].ronLowLimit;
            result1.ronHighLimit = ProdOilList[0].ronHighLimit;
            result1.t50LowLimit = ProdOilList[0].t50LowLimit;
            result1.t50HighLimit = ProdOilList[0].t50HighLimit;
            result1.sufLowLimit = ProdOilList[0].sufLowLimit;
            result1.sufHighLimit = ProdOilList[0].sufHighLimit;
            result1.denLowLimit = ProdOilList[0].denLowLimit;
            result1.denHighLimit = ProdOilList[0].denHighLimit;

            ResultList.Add(result1);           
            #endregion

            #region 返回出柴数据
            GasSchemeVerify_1Res_3 result2 = new GasSchemeVerify_1Res_3();//实体，可以理解为一个对象
            /// <summary>
            /// 质量界面
            /// </summary>
            result2.ProdOilName = SchemeBottomList[1].ProdOilName;
            result2.Prodron = (float)Totalgas95ronMass;
            result2.Prodt50 = (float)Totalgas95t50Mass;
            result2.Prodsuf = (float)Totalgas95sufMass;
            result2.Prodden = (float)Totalgas95denMass;
            /// <summary>
            /// 高地限不分质量和体积
            /// </summary>
            result2.ronLowLimit = ProdOilList[1].ronLowLimit;
            result2.ronHighLimit = ProdOilList[1].ronHighLimit;
            result2.t50LowLimit = ProdOilList[1].t50LowLimit;
            result2.t50HighLimit = ProdOilList[1].t50HighLimit;
            result2.sufLowLimit = ProdOilList[1].sufLowLimit;
            result2.sufHighLimit = ProdOilList[1].sufHighLimit;
            result2.denLowLimit = ProdOilList[1].denLowLimit;
            result2.denHighLimit = ProdOilList[1].denHighLimit;

            ResultList.Add(result2);

            #endregion
            
            #region 返回备用成品油1数据
            GasSchemeVerify_1Res_3 result3 = new GasSchemeVerify_1Res_3();//实体，可以理解为一个对象
            /// <summary>
            /// 质量界面
            /// </summary>
            result3.ProdOilName = SchemeBottomList[2].ProdOilName;
            result3.Prodron = (float)Totalgas98ronMass;
            result3.Prodt50 = (float)Totalgas98t50Mass;
            result3.Prodsuf = (float)Totalgas98sufMass;
            result3.Prodden = (float)Totalgas98denMass;
            /// <summary>
            /// 高地限不分质量和体积
            /// </summary>
            result3.ronLowLimit = ProdOilList[2].ronLowLimit;
            result3.ronHighLimit = ProdOilList[2].ronHighLimit;
            result3.t50LowLimit = ProdOilList[2].t50LowLimit;
            result3.t50HighLimit = ProdOilList[2].t50HighLimit;
            result3.sufLowLimit = ProdOilList[2].sufLowLimit;
            result3.sufHighLimit = ProdOilList[2].sufHighLimit;
            result3.denLowLimit = ProdOilList[2].denLowLimit;
            result3.denHighLimit = ProdOilList[2].denHighLimit;

            ResultList.Add(result3);

            #endregion

            #region 返回备用成品油2数据
            GasSchemeVerify_1Res_3 result4 = new GasSchemeVerify_1Res_3();//实体，可以理解为一个对象
            /// <summary>
            /// 质量界面
            /// </summary>
            result4.ProdOilName = SchemeBottomList[3].ProdOilName;
            result4.Prodron = (float)TotalgasSelfronMass;
            result4.Prodt50 = (float)TotalgasSelft50Mass;
            result4.Prodsuf = (float)TotalgasSelfsufMass;
            result4.Prodden = (float)TotalgasSelfdenMass;
            /// <summary>
            /// 高地限不分质量和体积
            /// </summary>
            result4.ronLowLimit = ProdOilList[3].ronLowLimit;
            result4.ronHighLimit = ProdOilList[3].ronHighLimit;
            result4.t50LowLimit = ProdOilList[3].t50LowLimit;
            result4.t50HighLimit = ProdOilList[3].t50HighLimit;
            result4.sufLowLimit = ProdOilList[3].sufLowLimit;
            result4.sufHighLimit = ProdOilList[3].sufHighLimit;
            result4.denLowLimit = ProdOilList[3].denLowLimit;
            result4.denHighLimit = ProdOilList[3].denHighLimit;

            ResultList.Add(result4);

            #endregion

            return ResultList;           

        }
        
        public IEnumerable<GasSchemeVerify_1Res_3> GetSchemeverifyResult1_vol_Property()//场景1体积 成品油属性
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

            List<GasSchemeVerify_1Res_3> ResultList = new List<GasSchemeVerify_1Res_3>();//列表，里面可以添加很多个对象

            #region 场景1——体积
            #region 车用柴油
            /// <summary>
            /// 车用柴油
            /// </summary>
            double gas92VolProduct = 0;//车用柴油体积产量
            for (int i = 0; i < SchemeCompOilList.Count; i++) 
            {
                gas92VolProduct = gas92VolProduct + SchemeCompOilList[i].gas92VolumeProduct;             
            }

            double gas92Mass = 0;//车用柴油总质量
            for (int i = 0; i < SchemeCompOilList.Count; i++)
            {
                gas92Mass = gas92Mass + SchemeCompOilList[i].gas92VolumeProduct * CompOilList[i].den / 1000;//单位是吨
            }
            // 车柴十六烷值指数 基于体积
            double gas92ronVol = 0;
            for(int i = 0;i < CompOilList.Count; i++)
            {
                gas92ronVol = gas92ronVol + CompOilList[i].ron * SchemeCompOilList[i].gas92VolumeProduct;//十六烷值乘以体积
            }

            double Totalgas92ronVol = Math.Round((gas92ronVol + SchemeBottomList[0].BottomVolume * SchemeBottomList[0].ronVol) / (gas92VolProduct + SchemeBottomList[0].BottomVolume), 1); 

            // 车柴50%回收温度 基于体积
            double gas92t50Vol = 0;
            for (int i = 0; i < CompOilList.Count; i++)
            {
                gas92t50Vol = gas92t50Vol + CompOilList[i].t50 * SchemeCompOilList[i].gas92VolumeProduct;//馏程乘以体积
            }

            double Totalgas92t50Vol = Math.Round((gas92t50Vol + SchemeBottomList[0].BottomVolume * SchemeBottomList[0].t50Vol) / (gas92VolProduct + SchemeBottomList[0].BottomVolume));

            // 多环芳烃含量 基于质量
            double gas92sufVol = 0;
            for (int i = 0; i < CompOilList.Count; i++)
            {
                gas92sufVol = gas92sufVol + CompOilList[i].suf * SchemeCompOilList[i].gas92VolumeProduct * CompOilList[i].den / 1000;//多芳烃基于质量 单位吨
            }
            double gas92BottomVol = SchemeBottomList[0].BottomVolume;
            double gas92BottomMass_1 = gas92BottomVol * SchemeBottomList[0].denVol / 1000;//场景1的体积界面下的车柴罐底油质量 单位吨
            double Totalgas92sufVol = Math.Round((gas92sufVol + gas92BottomMass_1 * SchemeBottomList[0].sufVol) / (gas92Mass + gas92BottomMass_1), 2);

            // 密度 基于质量
            double gas92denVol = 0;
            for (int i = 0; i < CompOilList.Count; i++)
            {
                gas92denVol = gas92denVol + CompOilList[i].den * SchemeCompOilList[i].gas92VolumeProduct * CompOilList[i].den / 1000;//密度基于质量 单位吨
            }
            double Totalgas92denVol = Math.Round((gas92denVol + gas92BottomMass_1 * SchemeBottomList[0].denVol) / (gas92Mass + gas92BottomMass_1), 1);
            #endregion
            
            #region 出口柴油
            /// <summary>
            /// 出口柴油
            /// </summary>
            double gas95VolProduct = 0;//出口柴油体积产量
            for (int i = 0; i < SchemeCompOilList.Count; i++) 
            {
                gas95VolProduct = gas95VolProduct + SchemeCompOilList[i].gas95VolumeProduct;             
            }

            double gas95Mass = 0;//出口柴油总质量
            for (int i = 0; i < SchemeCompOilList.Count; i++)
            {
                gas95Mass = gas95Mass + SchemeCompOilList[i].gas95VolumeProduct * CompOilList[i].den / 1000;//单位是吨
            }
            // 出柴十六烷值指数 基于体积
            double gas95ronVol = 0;
            for(int i = 0;i < CompOilList.Count; i++)
            {
                gas95ronVol = gas95ronVol + CompOilList[i].ron * SchemeCompOilList[i].gas95VolumeProduct;//十六烷值乘以体积
            }

            double Totalgas95ronVol = Math.Round((gas95ronVol + SchemeBottomList[1].BottomVolume * SchemeBottomList[1].ronVol) / (gas95VolProduct + SchemeBottomList[1].BottomVolume), 1); 

            // 出柴50%回收温度 基于体积
            double gas95t50Vol = 0;
            for (int i = 0; i < CompOilList.Count; i++)
            {
                gas95t50Vol = gas95t50Vol + CompOilList[i].t50 * SchemeCompOilList[i].gas95VolumeProduct;//馏程乘以体积
            }

            double Totalgas95t50Vol = Math.Round((gas95t50Vol + SchemeBottomList[1].BottomVolume * SchemeBottomList[1].t50Vol) / (gas95VolProduct + SchemeBottomList[1].BottomVolume));

            // 多环芳烃含量 基于质量
            double gas95sufVol = 0;
            for (int i = 0; i < CompOilList.Count; i++)
            {
                gas95sufVol = gas95sufVol + CompOilList[i].suf * SchemeCompOilList[i].gas95VolumeProduct * CompOilList[i].den / 1000;//多芳烃基于质量 单位吨
            }
            double gas95BottomVol = SchemeBottomList[1].BottomVolume;
            double gas95BottomMass_1 = gas95BottomVol * SchemeBottomList[1].denVol / 1000;//场景1的体积界面下的车柴罐底油质量 单位吨
            double Totalgas95sufVol = Math.Round((gas95sufVol + gas95BottomMass_1 * SchemeBottomList[1].sufVol) / (gas95Mass + gas95BottomMass_1), 2);

            // 密度 基于质量
            double gas95denVol = 0;
            for (int i = 0; i < CompOilList.Count; i++)
            {
                gas95denVol = gas95denVol + CompOilList[i].den * SchemeCompOilList[i].gas95VolumeProduct * CompOilList[i].den / 1000;//密度基于质量 单位吨
            }
            double Totalgas95denVol = Math.Round((gas95denVol + gas95BottomMass_1 * SchemeBottomList[1].denVol) / (gas95Mass + gas95BottomMass_1), 1);
            #endregion

            #region 备用成品油1
            /// <summary>
            /// 备用成品油1
            /// </summary>
            double gas98VolProduct = 0;//车用柴油体积产量
            for (int i = 0; i < SchemeCompOilList.Count; i++) 
            {
                gas98VolProduct = gas98VolProduct + SchemeCompOilList[i].gas98VolumeProduct;             
            }

            double gas98Mass = 0;//车用柴油总质量
            for (int i = 0; i < SchemeCompOilList.Count; i++)
            {
                gas98Mass = gas98Mass + SchemeCompOilList[i].gas98VolumeProduct * CompOilList[i].den / 1000;//单位是吨
            }
            // 车柴十六烷值指数 基于体积
            double gas98ronVol = 0;
            for(int i = 0;i < CompOilList.Count; i++)
            {
                gas98ronVol = gas98ronVol + CompOilList[i].ron * SchemeCompOilList[i].gas98VolumeProduct;//十六烷值乘以体积
            }

            double Totalgas98ronVol = Math.Round((gas98ronVol + SchemeBottomList[2].BottomVolume * SchemeBottomList[2].ronVol) / (gas98VolProduct + SchemeBottomList[2].BottomVolume), 1); 

            // 车柴50%回收温度 基于体积
            double gas98t50Vol = 0;
            for (int i = 0; i < CompOilList.Count; i++)
            {
                gas98t50Vol = gas98t50Vol + CompOilList[i].t50 * SchemeCompOilList[i].gas98VolumeProduct;//馏程乘以体积
            }

            double Totalgas98t50Vol = Math.Round((gas98t50Vol + SchemeBottomList[2].BottomVolume * SchemeBottomList[2].t50Vol) / (gas98VolProduct + SchemeBottomList[2].BottomVolume));

            // 多环芳烃含量 基于质量
            double gas98sufVol = 0;
            for (int i = 0; i < CompOilList.Count; i++)
            {
                gas98sufVol = gas98sufVol + CompOilList[i].suf * SchemeCompOilList[i].gas98VolumeProduct * CompOilList[i].den / 1000;//多芳烃基于质量 单位吨
            }
            double gas98BottomVol = SchemeBottomList[2].BottomVolume;
            double gas98BottomMass_1 = gas98BottomVol * SchemeBottomList[2].denVol / 1000;//场景1的体积界面下的车柴罐底油质量 单位吨
            double Totalgas98sufVol = Math.Round((gas98sufVol + gas98BottomMass_1 * SchemeBottomList[2].sufVol) / (gas98Mass + gas98BottomMass_1), 2);

            // 密度 基于质量
            double gas98denVol = 0;
            for (int i = 0; i < CompOilList.Count; i++)
            {
                gas98denVol = gas98denVol + CompOilList[i].den * SchemeCompOilList[i].gas98VolumeProduct * CompOilList[i].den / 1000;//密度基于质量 单位吨
            }
            double Totalgas98denVol = Math.Round((gas98denVol + gas98BottomMass_1 * SchemeBottomList[2].denVol) / (gas98Mass + gas98BottomMass_1), 1);
            #endregion

            #region 备用成品油2
            /// <summary>
            /// 备用成品油2
            /// </summary>
            double gasSelfVolProduct = 0;//车用柴油体积产量
            for (int i = 0; i < SchemeCompOilList.Count; i++) 
            {
                gasSelfVolProduct = gasSelfVolProduct + SchemeCompOilList[i].gasSelfVolumeProduct;             
            }

            double gasSelfMass = 0;//车用柴油总质量
            for (int i = 0; i < SchemeCompOilList.Count; i++)
            {
                gasSelfMass = gasSelfMass + SchemeCompOilList[i].gasSelfVolumeProduct * CompOilList[i].den / 1000;//单位是吨
            }
            // 车柴十六烷值指数 基于体积
            double gasSelfronVol = 0;
            for(int i = 0;i < CompOilList.Count; i++)
            {
                gasSelfronVol = gasSelfronVol + CompOilList[i].ron * SchemeCompOilList[i].gasSelfVolumeProduct;//十六烷值乘以体积
            }

            double TotalgasSelfronVol = Math.Round((gasSelfronVol + SchemeBottomList[3].BottomVolume * SchemeBottomList[3].ronVol) / (gasSelfVolProduct + SchemeBottomList[3].BottomVolume), 1); 

            // 车柴50%回收温度 基于体积
            double gasSelft50Vol = 0;
            for (int i = 0; i < CompOilList.Count; i++)
            {
                gasSelft50Vol = gasSelft50Vol + CompOilList[i].t50 * SchemeCompOilList[i].gasSelfVolumeProduct;//馏程乘以体积
            }

            double TotalgasSelft50Vol = Math.Round((gasSelft50Vol + SchemeBottomList[3].BottomVolume * SchemeBottomList[3].t50Vol) / (gasSelfVolProduct + SchemeBottomList[3].BottomVolume));

            // 多环芳烃含量 基于质量
            double gasSelfsufVol = 0;
            for (int i = 0; i < CompOilList.Count; i++)
            {
                gasSelfsufVol = gasSelfsufVol + CompOilList[i].suf * SchemeCompOilList[i].gasSelfVolumeProduct * CompOilList[i].den / 1000;//多芳烃基于质量 单位吨
            }
            double gasSelfBottomVol = SchemeBottomList[3].BottomVolume;
            double gasSelfBottomMass_1 = gasSelfBottomVol * SchemeBottomList[3].denVol / 1000;//场景1的体积界面下的车柴罐底油质量 单位吨
            double TotalgasSelfsufVol = Math.Round((gasSelfsufVol + gasSelfBottomMass_1 * SchemeBottomList[3].sufVol) / (gasSelfMass + gasSelfBottomMass_1), 2);

            // 密度 基于质量
            double gasSelfdenVol = 0;
            for (int i = 0; i < CompOilList.Count; i++)
            {
                gasSelfdenVol = gasSelfdenVol + CompOilList[i].den * SchemeCompOilList[i].gasSelfVolumeProduct * CompOilList[i].den / 1000;//密度基于质量 单位吨
            }
            double TotalgasSelfdenVol = Math.Round((gasSelfdenVol + gasSelfBottomMass_1 * SchemeBottomList[3].denVol) / (gasSelfMass + gasSelfBottomMass_1), 1);
            #endregion

            #endregion

            #region 返回车柴数据
            GasSchemeVerify_1Res_3 result1 = new GasSchemeVerify_1Res_3();//实体，可以理解为一个对象
            /// <summary>
            /// 体积界面
            /// </summary>
            result1.ProdOilName = SchemeBottomList[0].ProdOilName;
            result1.Prodron = (float)Totalgas92ronVol;
            result1.Prodt50 = (float)Totalgas92t50Vol;
            result1.Prodsuf = (float)Totalgas92sufVol;
            result1.Prodden = (float)Totalgas92denVol;
            /// <summary>
            /// 高低限不分质量和体积
            /// </summary>
            result1.ronLowLimit = ProdOilList[0].ronLowLimit;
            result1.ronHighLimit = ProdOilList[0].ronHighLimit;
            result1.t50LowLimit = ProdOilList[0].t50LowLimit;
            result1.t50HighLimit = ProdOilList[0].t50HighLimit;
            result1.sufLowLimit = ProdOilList[0].sufLowLimit;
            result1.sufHighLimit = ProdOilList[0].sufHighLimit;
            result1.denLowLimit = ProdOilList[0].denLowLimit;
            result1.denHighLimit = ProdOilList[0].denHighLimit;

            ResultList.Add(result1);           
            #endregion

            #region 返回出柴数据
            GasSchemeVerify_1Res_3 result2 = new GasSchemeVerify_1Res_3();//实体，可以理解为一个对象
            /// <summary>
            /// 体积界面
            /// </summary>
            result2.ProdOilName = SchemeBottomList[1].ProdOilName;
            result2.Prodron = (float)Totalgas95ronVol;
            result2.Prodt50 = (float)Totalgas95t50Vol;
            result2.Prodsuf = (float)Totalgas95sufVol;
            result2.Prodden = (float)Totalgas95denVol;
            /// <summary>
            /// 高地限不分质量和体积
            /// </summary>
            result2.ronLowLimit = ProdOilList[1].ronLowLimit;
            result2.ronHighLimit = ProdOilList[1].ronHighLimit;
            result2.t50LowLimit = ProdOilList[1].t50LowLimit;
            result2.t50HighLimit = ProdOilList[1].t50HighLimit;
            result2.sufLowLimit = ProdOilList[1].sufLowLimit;
            result2.sufHighLimit = ProdOilList[1].sufHighLimit;
            result2.denLowLimit = ProdOilList[1].denLowLimit;
            result2.denHighLimit = ProdOilList[1].denHighLimit;

            ResultList.Add(result2);

            #endregion

            #region 返回备用成品油1数据
            GasSchemeVerify_1Res_3 result3 = new GasSchemeVerify_1Res_3();//实体，可以理解为一个对象
            /// <summary>
            /// 体积界面
            /// </summary>
            result3.ProdOilName = SchemeBottomList[2].ProdOilName;
            result3.Prodron = (float)Totalgas98ronVol;
            result3.Prodt50 = (float)Totalgas98t50Vol;
            result3.Prodsuf = (float)Totalgas98sufVol;
            result3.Prodden = (float)Totalgas98denVol;
            /// <summary>
            /// 高低限不分质量和体积
            /// </summary>
            result3.ronLowLimit = ProdOilList[2].ronLowLimit;
            result3.ronHighLimit = ProdOilList[2].ronHighLimit;
            result3.t50LowLimit = ProdOilList[2].t50LowLimit;
            result3.t50HighLimit = ProdOilList[2].t50HighLimit;
            result3.sufLowLimit = ProdOilList[2].sufLowLimit;
            result3.sufHighLimit = ProdOilList[2].sufHighLimit;
            result3.denLowLimit = ProdOilList[2].denLowLimit;
            result3.denHighLimit = ProdOilList[2].denHighLimit;

            ResultList.Add(result3);           
            #endregion

            #region 返回备用成品油2数据
            GasSchemeVerify_1Res_3 result4 = new GasSchemeVerify_1Res_3();//实体，可以理解为一个对象
            /// <summary>
            /// 体积界面
            /// </summary>
            result4.ProdOilName = SchemeBottomList[3].ProdOilName;
            result4.Prodron = (float)TotalgasSelfronVol;
            result4.Prodt50 = (float)TotalgasSelft50Vol;
            result4.Prodsuf = (float)TotalgasSelfsufVol;
            result4.Prodden = (float)TotalgasSelfdenVol;
            /// <summary>
            /// 高低限不分质量和体积
            /// </summary>
            result4.ronLowLimit = ProdOilList[3].ronLowLimit;
            result4.ronHighLimit = ProdOilList[3].ronHighLimit;
            result4.t50LowLimit = ProdOilList[3].t50LowLimit;
            result4.t50HighLimit = ProdOilList[3].t50HighLimit;
            result4.sufLowLimit = ProdOilList[3].sufLowLimit;
            result4.sufHighLimit = ProdOilList[3].sufHighLimit;
            result4.denLowLimit = ProdOilList[3].denLowLimit;
            result4.denHighLimit = ProdOilList[3].denHighLimit;

            ResultList.Add(result4);           
            #endregion

            return ResultList;
        }

        public IEnumerable<GasSchemeVerify_2Res_1> GetSchemeverifyResult2_mass_Product()//场景2质量 成品油产量
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

            List<GasSchemeVerify_2Res_1> ResultList = new List<GasSchemeVerify_2Res_1>();//列表，里面可以添加很多个对象
            /// <summary>
            /// 场景二——质量
            /// </summary>
            float[] gas92QualityProduct = new float[SchemeCompOilList.Count];//暂存车用柴油配方乘以调合总量得出的质量产量
            float[] gas95QualityProduct = new float[SchemeCompOilList.Count];//暂存出口柴油配方乘以调合总量得出的质量产量  
            float[] gas98QualityProduct = new float[SchemeCompOilList.Count];//暂存配方乘以调合总量得出的质量产量
            float[] gasSelfQualityProduct = new float[SchemeCompOilList.Count];//暂存配方乘以调合总量得出的质量产量  
            for (int i = 0; i < SchemeCompOilList.Count; i++)
            {//将配方转化为质量产量
                gas92QualityProduct[i] = SchemeCompOilList[i].gas92FlowPercentMass * SchemeBottomList[0].TotalBlendMass2 / 100;
                gas95QualityProduct[i] = SchemeCompOilList[i].gas95FlowPercentMass * SchemeBottomList[1].TotalBlendMass2 / 100;
                gas98QualityProduct[i] = SchemeCompOilList[i].gas98FlowPercentMass * SchemeBottomList[2].TotalBlendMass2 / 100;
                gasSelfQualityProduct[i] = SchemeCompOilList[i].gasSelfFlowPercentMass * SchemeBottomList[3].TotalBlendMass2 / 100;
            }            
            /// <summary>
            /// 返回成品油数据
            /// </summary>
            for (int i = 0; i < SchemeCompOilList.Count; i++)
            {
                GasSchemeVerify_2Res_1 temp_result = new GasSchemeVerify_2Res_1();
                temp_result.ComOilName = SchemeCompOilList[i].ComOilName;
                temp_result.gas92Product = gas92QualityProduct[i];
                temp_result.gas95Product = gas95QualityProduct[i];
                temp_result.gas98Product = gas98QualityProduct[i];
                temp_result.gasSelfProduct = gasSelfQualityProduct[i]; 
                ResultList.Add(temp_result);
            }
            return ResultList;
        }

        public IEnumerable<GasSchemeVerify_2Res_1> GetSchemeverifyResult2_vol_Product()//场景2体积 成品油产量
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

            List<GasSchemeVerify_2Res_1> ResultList = new List<GasSchemeVerify_2Res_1>();//列表，里面可以添加很多个对象
            /// <summary>
            /// 场景二——体积
            /// </summary>
            float[] gas92VolumeProduct = new float[SchemeCompOilList.Count];//暂存车用柴油配方乘以调合总量得出的体积产量
            float[] gas95VolumeProduct = new float[SchemeCompOilList.Count];//暂存出口柴油配方乘以调合总量得出的体积产量   
            float[] gas98VolumeProduct = new float[SchemeCompOilList.Count];//暂存配方乘以调合总量得出的体积产量
            float[] gasSelfVolumeProduct = new float[SchemeCompOilList.Count];//暂存配方乘以调合总量得出的体积产量   
            for (int i = 0; i < SchemeCompOilList.Count; i++)
            {//将配方转化为体积产量
                gas92VolumeProduct[i] = SchemeCompOilList[i].gas92FlowPercentVol * SchemeBottomList[0].TotalBlendVol2 / 100;
                gas95VolumeProduct[i] = SchemeCompOilList[i].gas95FlowPercentVol * SchemeBottomList[1].TotalBlendVol2 / 100;
                gas98VolumeProduct[i] = SchemeCompOilList[i].gas98FlowPercentVol * SchemeBottomList[2].TotalBlendVol2 / 100;
                gasSelfVolumeProduct[i] = SchemeCompOilList[i].gasSelfFlowPercentVol * SchemeBottomList[3].TotalBlendVol2 / 100;
            }             
            /// <summary>
            /// 返回成品油数据
            /// </summary>
            for (int i = 0; i < SchemeCompOilList.Count; i++)
            {
                GasSchemeVerify_2Res_1 temp_result = new GasSchemeVerify_2Res_1();
                temp_result.ComOilName = SchemeCompOilList[i].ComOilName;
                temp_result.gas92Product = gas92VolumeProduct[i];
                temp_result.gas95Product = gas95VolumeProduct[i];
                temp_result.gas98Product = gas98VolumeProduct[i];
                temp_result.gasSelfProduct = gasSelfVolumeProduct[i]; 
                ResultList.Add(temp_result);
            }
            return ResultList;
        }

        public IEnumerable<GasSchemeVerify_2Res_2> GetSchemeverifyResult2_mass_Property()//场景2质量 成品油属性
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

            List<GasSchemeVerify_2Res_2> ResultList = new List<GasSchemeVerify_2Res_2>();//列表，里面可以添加很多个对象

            #region 场景2——质量 

            #region 车用柴油
            /// <summary>
            /// 车用柴油
            /// </summary>
            double gas92MassProduct = 0;//车用柴油总质量产量
            double[] gas92QualityProduct = new double[SchemeCompOilList.Count];//暂存质量产量            
            for (int i = 0; i < SchemeCompOilList.Count; i++)//将配方转化为质量产量
            {
                gas92QualityProduct[i] = SchemeCompOilList[i].gas92FlowPercentMass * SchemeBottomList[0].TotalBlendMass2 / 100;
            }           
            for (int i = 0; i < SchemeCompOilList.Count; i++) 
            {
                gas92MassProduct = gas92MassProduct + gas92QualityProduct[i];         
            }

            double gas92Volume = 0;//车用柴油总体积
            for (int i = 0; i < SchemeCompOilList.Count; i++)
            {
                gas92Volume = gas92Volume + gas92QualityProduct[i] / CompOilList[i].den * 1000;//体积
            }
            // 车柴十六烷值指数 基于体积
            double gas92ronMass = 0;
            for(int i = 0;i < CompOilList.Count; i++)
            {
                gas92ronMass = gas92ronMass + CompOilList[i].ron * gas92QualityProduct[i] / CompOilList[i].den * 1000;//十六烷值乘以体积
            }

            double Totalgas92ronMass = Math.Round(gas92ronMass / gas92Volume, 1); 

            // 车柴50%回收温度 基于体积
            double gas92t50Mass = 0;
            for (int i = 0; i < CompOilList.Count; i++)
            {
                gas92t50Mass = gas92t50Mass + CompOilList[i].t50 * gas92QualityProduct[i] / CompOilList[i].den * 1000;//馏程乘以体积
            }

            double Totalgas92t50Mass = Math.Round(gas92t50Mass / gas92Volume); 

            //double Totalgas92t50Mass = Math.Round((gas92t50Mass + SchemeBottomList[0].BottomMass * 1000 / SchemeBottomList[0].denMass * SchemeBottomList[0].t50Mass) / (gas92Volume + SchemeBottomList[0].BottomMass * 1000 / SchemeBottomList[0].denMass), 2);

            // 多环芳烃含量 基于质量
            double gas92sufMass = 0;
            for (int i = 0; i < CompOilList.Count; i++)
            {
                gas92sufMass = gas92sufMass + CompOilList[i].suf * gas92QualityProduct[i];//多芳烃基于质量
            }
            //double gas92BottomMass = SchemeBottomList[0].BottomMass;
            double Totalgas92sufMass = Math.Round(gas92sufMass / gas92MassProduct, 2);

            // 密度 基于质量
            double gas92denMass = 0;
            for (int i = 0; i < CompOilList.Count; i++)
            {
                gas92denMass = gas92denMass + CompOilList[i].den * gas92QualityProduct[i];//密度乘以质量
            }
           // double Totalgas92denMass = Math.Round((gas92denMass + gas92BottomMass * SchemeBottomList[0].denMass) / (gas92MassProduct + gas92BottomMass), 2);
            double Totalgas92denMass = Math.Round(gas92denMass / gas92MassProduct, 1);
            # endregion
            
            #region 出口柴油
            /// <summary>
            /// 出口柴油
            /// </summary>
            double gas95MassProduct = 0;//出口柴油总质量产量
            double[] gas95QualityProduct = new double[SchemeCompOilList.Count];//暂存质量产量  
            for (int i = 0; i < SchemeCompOilList.Count; i++)//将配方转化为质量产量
            {
                gas95QualityProduct[i] = SchemeCompOilList[i].gas95FlowPercentMass * SchemeBottomList[1].TotalBlendMass2 / 100;
            }    
            for (int i = 0; i < SchemeCompOilList.Count; i++) 
            {
                gas95MassProduct = gas95MassProduct + gas95QualityProduct[i];             
            }

            double gas95Volume = 0;//出口柴油总体积
            for (int i = 0; i < SchemeCompOilList.Count; i++)
            {
                gas95Volume = gas95Volume + gas95QualityProduct[i] / CompOilList[i].den * 1000;//体积
            }
            // 出柴十六烷值指数 基于体积
            double gas95ronMass = 0;
            for(int i = 0;i < CompOilList.Count; i++)
            {
                gas95ronMass = gas95ronMass + CompOilList[i].ron * gas95QualityProduct[i] / CompOilList[i].den * 1000;//十六烷值乘以体积
            }

            double Totalgas95ronMass = Math.Round(gas95ronMass / gas95Volume, 1); 

            // 出柴50%回收温度 基于体积
            double gas95t50Mass = 0;
            for (int i = 0; i < CompOilList.Count; i++)
            {
                gas95t50Mass = gas95t50Mass + CompOilList[i].t50 * gas95QualityProduct[i] / CompOilList[i].den * 1000;//馏程乘以体积
            }
            double Totalgas95t50Mass = Math.Round(gas95t50Mass / gas95Volume); 
            //double Totalgas95t50Mass = Math.Round((gas95t50Mass + SchemeBottomList[1].BottomMass * 1000 / SchemeBottomList[1].denMass * SchemeBottomList[1].t50Mass) / (gas95Volume + SchemeBottomList[1].BottomMass * 1000 / SchemeBottomList[1].denMass), 2);

            // 多环芳烃含量 基于质量
            double gas95sufMass = 0;
            for (int i = 0; i < CompOilList.Count; i++)
            {
                gas95sufMass = gas95sufMass + CompOilList[i].suf * gas95QualityProduct[i];//多芳烃基于质量
            }
            //double gas95BottomMass = SchemeBottomList[1].BottomMass;
           double Totalgas95sufMass = Math.Round(gas95sufMass / gas95MassProduct, 2);

            // 密度 基于质量
            double gas95denMass = 0;
            for (int i = 0; i < CompOilList.Count; i++)
            {
                gas95denMass = gas95denMass + CompOilList[i].den * gas95QualityProduct[i];//密度乘以质量
            }
            double Totalgas95denMass = Math.Round(gas95denMass / gas95MassProduct, 1);

            #region 闪点和粘度
            //     //闪点
            //     //double Sum1_Flash= 0;
            //     double Sum1_index = 0;
            //     //double P1_Bottom_volume = Bottom_FC_1 / list2[0].Bottom_den;
            //     double P1_total_volume = P1_Volume + list2[0].Bottom_Volume * 1000 / list2[0].Bottom_den;

            //     for (int i = 0; i < CompOilList.Count; i++)
            //     {
            //         Sum1_index = Sum1_index + Math.Pow(0.929, CompOilList[i].Flash) * CompOilList[i].P0_FC *1000 / CompOilList[i].den / P1_total_volume;//
            //     }
            //     Sum1_index = Sum1_index + Math.Pow(0.929, list2[0].Bottom_Flash) * list2[0].Bottom_Volume * 1000 / list2[0].Bottom_den / P1_total_volume;
            //     //Sum1_Flash = Math.Log(Sum1_index, 0.929);

            //     //double P1_den = Math.Round(Sum1_den / P1_Weight, 2);//总密度除以总质量
            //     double P1_Flash = Math.Round(Math.Log(Sum1_index, 0.929), 2);

            //    // 粘度
            //     double Sum1_Viscosity = 0;
            //     for (int i = 0; i < CompOilList.Count; i++)
            //     {
            //         Sum1_Viscosity = Sum1_Viscosity + Math.Pow(CompOilList[i].Viscosity, 1.0/3) * CompOilList[i].P0_FC / CompOilList[i].den * 1000;//
            //     }
            //     Sum1_Viscosity = (Sum1_Viscosity + Math.Pow(list2[0].Bottom_Viscosity, 1.0/3) * list2[0].Bottom_Volume * 1000 / list2[0].Bottom_den) / P1_total_volume;

            //     //double P1_den = Math.Round(Sum1_den / P1_Weight, 2);//总密度除以总质量
            //     double P1_Viscosity = Math.Round(Math.Pow(Sum1_Viscosity, 3), 2);
            #endregion
            
            #endregion 出口柴油 end

            #region 备用成品油1
            /// <summary>
            /// 备用成品油1
            /// </summary>
            double gas98MassProduct = 0;//总质量产量
            double[] gas98QualityProduct = new double[SchemeCompOilList.Count];//暂存质量产量  
            for (int i = 0; i < SchemeCompOilList.Count; i++)//将配方转化为质量产量
            {
                gas98QualityProduct[i] = SchemeCompOilList[i].gas98FlowPercentMass * SchemeBottomList[2].TotalBlendMass2 / 100;
            }    
            for (int i = 0; i < SchemeCompOilList.Count; i++) 
            {
                gas98MassProduct = gas98MassProduct + gas98QualityProduct[i];             
            }
            double gas98Volume = 0;//总体积
            for (int i = 0; i < SchemeCompOilList.Count; i++)
            {
                gas98Volume = gas98Volume + gas98QualityProduct[i] / CompOilList[i].den * 1000;//体积
            }

            // 十六烷值指数 基于体积
            double gas98ronMass = 0;
            for(int i = 0;i < CompOilList.Count; i++)
            {
                gas98ronMass = gas98ronMass + CompOilList[i].ron * gas98QualityProduct[i] / CompOilList[i].den * 1000;//十六烷值乘以体积
            }

            double Totalgas98ronMass = Math.Round(gas98ronMass / gas98Volume, 1); 

            // 50%回收温度 基于体积
            double gas98t50Mass = 0;
            for (int i = 0; i < CompOilList.Count; i++)
            {
                gas98t50Mass = gas98t50Mass + CompOilList[i].t50 * gas98QualityProduct[i] / CompOilList[i].den * 1000;//馏程乘以体积
            }

            double Totalgas98t50Mass = Math.Round(gas98t50Mass / gas98Volume);

            // 多环芳烃含量 基于质量
            double gas98sufMass = 0;
            for (int i = 0; i < CompOilList.Count; i++)
            {
                gas98sufMass = gas98sufMass + CompOilList[i].suf * gas98QualityProduct[i];//多芳烃基于质量
            }
            //double gas98BottomMass = SchemeBottomList[1].BottomMass;
           double Totalgas98sufMass = Math.Round(gas98sufMass / gas98MassProduct, 2);

            // 密度 基于质量
            double gas98denMass = 0;
            for (int i = 0; i < CompOilList.Count; i++)
            {
                gas98denMass = gas98denMass + CompOilList[i].den * gas98QualityProduct[i];//密度乘以质量
            }
            double Totalgas98denMass = Math.Round(gas98denMass / gas98MassProduct, 1);
            
            #endregion 备用成品油1 end

            #region 备用成品油2
            /// <summary>
            /// 备用成品油2
            /// </summary>
            double gasSelfMassProduct = 0;//总质量产量
            double[] gasSelfQualityProduct = new double[SchemeCompOilList.Count];//暂存质量产量  
            for (int i = 0; i < SchemeCompOilList.Count; i++)//将配方转化为质量产量
            {
                gasSelfQualityProduct[i] = SchemeCompOilList[i].gasSelfFlowPercentMass * SchemeBottomList[3].TotalBlendMass2 / 100;
            }    
            for (int i = 0; i < SchemeCompOilList.Count; i++) 
            {
                gasSelfMassProduct = gasSelfMassProduct + gasSelfQualityProduct[i];             
            }
            double gasSelfVolume = 0;//总体积
            for (int i = 0; i < SchemeCompOilList.Count; i++)
            {
                gasSelfVolume = gasSelfVolume + gasSelfQualityProduct[i] / CompOilList[i].den * 1000;//体积
            }

            // 十六烷值指数 基于体积
            double gasSelfronMass = 0;
            for(int i = 0;i < CompOilList.Count; i++)
            {
                gasSelfronMass = gasSelfronMass + CompOilList[i].ron * gasSelfQualityProduct[i] / CompOilList[i].den * 1000;//十六烷值乘以体积
            }

            double TotalgasSelfronMass = Math.Round(gasSelfronMass / gasSelfVolume, 1); 

            // 50%回收温度 基于体积
            double gasSelft50Mass = 0;
            for (int i = 0; i < CompOilList.Count; i++)
            {
                gasSelft50Mass = gasSelft50Mass + CompOilList[i].t50 * gasSelfQualityProduct[i] / CompOilList[i].den * 1000;//馏程乘以体积
            }

            double TotalgasSelft50Mass = Math.Round(gasSelft50Mass / gasSelfVolume);

            // 多环芳烃含量 基于质量
            double gasSelfsufMass = 0;
            for (int i = 0; i < CompOilList.Count; i++)
            {
                gasSelfsufMass = gasSelfsufMass + CompOilList[i].suf * gasSelfQualityProduct[i];//多芳烃基于质量
            }
            //double gasSelfBottomMass = SchemeBottomList[1].BottomMass;
            double TotalgasSelfsufMass = Math.Round(gasSelfsufMass / gasSelfMassProduct, 2);

            // 密度 基于质量
            double gasSelfdenMass = 0;
            for (int i = 0; i < CompOilList.Count; i++)
            {
                gasSelfdenMass = gasSelfdenMass + CompOilList[i].den * gasSelfQualityProduct[i];//密度乘以质量
            }
            double TotalgasSelfdenMass = Math.Round(gasSelfdenMass / gasSelfMassProduct, 1);
            
            #endregion 备用成品油2 end           
            
            #endregion 场景2—质量 end

            #region 返回车柴数据
            GasSchemeVerify_2Res_2 result1 = new GasSchemeVerify_2Res_2();//实体，可以理解为一个对象
            /// <summary>
            /// 质量界面
            /// </summary>
            result1.ProdOilName = SchemeBottomList[0].ProdOilName;
            result1.Prodron = (float)Totalgas92ronMass;
            result1.Prodt50 = (float)Totalgas92t50Mass;
            result1.Prodsuf = (float)Totalgas92sufMass;
            result1.Prodden = (float)Totalgas92denMass;
            /// <summary>
            /// 高低限不分质量和体积
            /// </summary>
            result1.ronLowLimit = ProdOilList[0].ronLowLimit;
            result1.ronHighLimit = ProdOilList[0].ronHighLimit;
            result1.t50LowLimit = ProdOilList[0].t50LowLimit;
            result1.t50HighLimit = ProdOilList[0].t50HighLimit;
            result1.sufLowLimit = ProdOilList[0].sufLowLimit;
            result1.sufHighLimit = ProdOilList[0].sufHighLimit;
            result1.denLowLimit = ProdOilList[0].denLowLimit;
            result1.denHighLimit = ProdOilList[0].denHighLimit;

            ResultList.Add(result1);           
            #endregion

            #region 返回出柴数据
            GasSchemeVerify_2Res_2 result2 = new GasSchemeVerify_2Res_2();//实体，可以理解为一个对象
            /// <summary>
            /// 质量界面
            /// </summary>
            result2.ProdOilName = SchemeBottomList[1].ProdOilName;
            result2.Prodron = (float)Totalgas95ronMass;
            result2.Prodt50 = (float)Totalgas95t50Mass;
            result2.Prodsuf = (float)Totalgas95sufMass;
            result2.Prodden = (float)Totalgas95denMass;
            /// <summary>
            /// 高地限不分质量和体积
            /// </summary>
            result2.ronLowLimit = ProdOilList[1].ronLowLimit;
            result2.ronHighLimit = ProdOilList[1].ronHighLimit;
            result2.t50LowLimit = ProdOilList[1].t50LowLimit;
            result2.t50HighLimit = ProdOilList[1].t50HighLimit;
            result2.sufLowLimit = ProdOilList[1].sufLowLimit;
            result2.sufHighLimit = ProdOilList[1].sufHighLimit;
            result2.denLowLimit = ProdOilList[1].denLowLimit;
            result2.denHighLimit = ProdOilList[1].denHighLimit;

            ResultList.Add(result2);

            #endregion
            
            #region 返回备用成品油1数据
            GasSchemeVerify_2Res_2 result3 = new GasSchemeVerify_2Res_2();//实体，可以理解为一个对象
            /// <summary>
            /// 质量界面
            /// </summary>
            result3.ProdOilName = SchemeBottomList[2].ProdOilName;
            result3.Prodron = (float)Totalgas98ronMass;
            result3.Prodt50 = (float)Totalgas98t50Mass;
            result3.Prodsuf = (float)Totalgas98sufMass;
            result3.Prodden = (float)Totalgas98denMass;
            /// <summary>
            /// 高地限不分质量和体积
            /// </summary>
            result3.ronLowLimit = ProdOilList[2].ronLowLimit;
            result3.ronHighLimit = ProdOilList[2].ronHighLimit;
            result3.t50LowLimit = ProdOilList[2].t50LowLimit;
            result3.t50HighLimit = ProdOilList[2].t50HighLimit;
            result3.sufLowLimit = ProdOilList[2].sufLowLimit;
            result3.sufHighLimit = ProdOilList[2].sufHighLimit;
            result3.denLowLimit = ProdOilList[2].denLowLimit;
            result3.denHighLimit = ProdOilList[2].denHighLimit;

            ResultList.Add(result3);

            #endregion
            
            #region 返回备用成品油2数据
            GasSchemeVerify_2Res_2 result4 = new GasSchemeVerify_2Res_2();//实体，可以理解为一个对象
            /// <summary>
            /// 质量界面
            /// </summary>
            result4.ProdOilName = SchemeBottomList[3].ProdOilName;
            result4.Prodron = (float)Totalgas98ronMass;
            result4.Prodt50 = (float)Totalgas98t50Mass;
            result4.Prodsuf = (float)Totalgas98sufMass;
            result4.Prodden = (float)Totalgas98denMass;
            /// <summary>
            /// 高地限不分质量和体积
            /// </summary>
            result4.ronLowLimit = ProdOilList[3].ronLowLimit;
            result4.ronHighLimit = ProdOilList[3].ronHighLimit;
            result4.t50LowLimit = ProdOilList[3].t50LowLimit;
            result4.t50HighLimit = ProdOilList[3].t50HighLimit;
            result4.sufLowLimit = ProdOilList[3].sufLowLimit;
            result4.sufHighLimit = ProdOilList[3].sufHighLimit;
            result4.denLowLimit = ProdOilList[3].denLowLimit;
            result4.denHighLimit = ProdOilList[3].denHighLimit;

            ResultList.Add(result4);

            #endregion

            return ResultList;
        }
        
        public IEnumerable<GasSchemeVerify_2Res_2> GetSchemeverifyResult2_vol_Property()//场景2体积 成品油属性
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

            List<GasSchemeVerify_2Res_2> ResultList = new List<GasSchemeVerify_2Res_2>();//列表，里面可以添加很多个对象

            #region 场景2——体积

            #region 车用柴油

            double[] gas92VolumeProduct = new double[SchemeCompOilList.Count];//暂存体积产量

            for (int i = 0; i < SchemeCompOilList.Count; i++)//将配方转化为体积产量
            {
                gas92VolumeProduct[i] = SchemeCompOilList[i].gas92FlowPercentVol * SchemeBottomList[0].TotalBlendVol2 / 100;
            } 
            double gas92VolProduct = 0;//车用柴油总体积
            for (int i = 0; i < SchemeCompOilList.Count; i++)
            {
                gas92VolProduct = gas92VolProduct + gas92VolumeProduct[i];//体积
            }

            double gas92Mass = 0;//车用柴油总质量
            for (int i = 0; i < SchemeCompOilList.Count; i++)
            {
                gas92Mass = gas92Mass +  gas92VolumeProduct[i] * CompOilList[i].den / 1000;//体积
            }

            // 车柴十六烷值指数 基于体积
            double gas92ronVol = 0;
            for(int i = 0;i < CompOilList.Count; i++)
            {
                gas92ronVol = gas92ronVol + CompOilList[i].ron * gas92VolumeProduct[i];//十六烷值乘以体积
            }

            double Totalgas92ronVol = Math.Round(gas92ronVol / gas92VolProduct, 1); 

            // 车柴50%回收温度 基于体积
            double gas92t50Vol = 0;
            for (int i = 0; i < CompOilList.Count; i++)
            {
                gas92t50Vol = gas92t50Vol + CompOilList[i].t50 * gas92VolumeProduct[i];//馏程乘以体积
            }

            double Totalgas92t50Vol = Math.Round(gas92t50Vol / gas92VolProduct);

            // 多环芳烃含量 基于质量
            double gas92sufVol = 0;
            for (int i = 0; i < CompOilList.Count; i++)
            {
                gas92sufVol = gas92sufVol + CompOilList[i].suf * gas92VolumeProduct[i] * CompOilList[i].den / 1000;//多芳烃基于质量 单位吨
            }
            //double gas92BottomVol = SchemeBottomList[0].BottomVolume;
            //double gas92BottomMass_1 = gas92BottomVol * SchemeBottomList[0].denVol / 1000;//场景1的体积界面下的车柴罐底油质量 单位吨
            double Totalgas92sufVol = Math.Round(gas92sufVol / gas92Mass, 2);

            // 密度 基于质量
            double gas92denVol = 0;
            for (int i = 0; i < CompOilList.Count; i++)
            {
                gas92denVol = gas92denVol + CompOilList[i].den * gas92VolumeProduct[i] * CompOilList[i].den / 1000;//密度基于质量 单位吨
            }
            double Totalgas92denVol = Math.Round(gas92denVol / gas92Mass, 1);
            
            #endregion 车用柴油 end
            
            #region 出口柴油
            /// <summary>
            /// 出口柴油
            /// </summary>
            double[] gas95VolumeProduct = new double[SchemeCompOilList.Count];//暂存体积产量  
            for (int i = 0; i < SchemeCompOilList.Count; i++)//将配方转化为质量产量
            {
                gas95VolumeProduct[i] = SchemeCompOilList[i].gas95FlowPercentVol * SchemeBottomList[1].TotalBlendVol2 / 100;
            }  
            double gas95VolProduct = 0;//总体积
            for (int i = 0; i < SchemeCompOilList.Count; i++)
            {
                gas95VolProduct = gas95VolProduct + gas95VolumeProduct[i];//体积
            }

            double gas95Mass = 0;//总质量
            for (int i = 0; i < SchemeCompOilList.Count; i++)
            {
                gas95Mass = gas95Mass +  gas95VolumeProduct[i] * CompOilList[i].den / 1000;//体积
            }
            // 出柴十六烷值指数 基于体积
            double gas95ronVol = 0;
            for(int i = 0;i < CompOilList.Count; i++)
            {
                gas95ronVol = gas95ronVol + CompOilList[i].ron * gas95VolumeProduct[i];//十六烷值乘以体积
            }

            double Totalgas95ronVol = Math.Round(gas95ronVol / gas95VolProduct, 1); 

            // 出柴50%回收温度 基于体积
            double gas95t50Vol = 0;
            for (int i = 0; i < CompOilList.Count; i++)
            {
                gas95t50Vol = gas95t50Vol + CompOilList[i].t50 * gas95VolumeProduct[i];//馏程乘以体积
            }

            double Totalgas95t50Vol = Math.Round(gas95t50Vol / gas95VolProduct);

            // 多环芳烃含量 基于质量
            double gas95sufVol = 0;
            for (int i = 0; i < CompOilList.Count; i++)
            {
                gas95sufVol = gas95sufVol + CompOilList[i].suf * gas95VolumeProduct[i] * CompOilList[i].den / 1000;//多芳烃基于质量 单位吨
            }
            //double gas95BottomVol = SchemeBottomList[1].BottomVolume;
            //double gas95BottomMass_1 = gas95BottomVol * SchemeBottomList[1].denVol / 1000;//场景1的体积界面下的车柴罐底油质量 单位吨
            double Totalgas95sufVol = Math.Round(gas95sufVol / gas95Mass, 2);

            // 密度 基于质量
            double gas95denVol = 0;
            for (int i = 0; i < CompOilList.Count; i++)
            {
                gas95denVol = gas95denVol + CompOilList[i].den * gas95VolumeProduct[i] * CompOilList[i].den / 1000;//密度基于质量 单位吨
            }
            double Totalgas95denVol = Math.Round(gas95denVol / gas95Mass, 1);
            #endregion 出口柴油 end

            #region 备用成品油1
            /// <summary>
            /// 备用成品油1
            /// </summary>

            double[] gas98VolumeProduct = new double[SchemeCompOilList.Count];//暂存体积产量  
            for (int i = 0; i < SchemeCompOilList.Count; i++)//将配方转化为质量产量
            {
                gas98VolumeProduct[i] = SchemeCompOilList[i].gas98FlowPercentVol * SchemeBottomList[2].TotalBlendVol2 / 100;
            }  
            double gas98VolProduct = 0;//总体积
            for (int i = 0; i < SchemeCompOilList.Count; i++)
            {
                gas98VolProduct = gas98VolProduct + gas98VolumeProduct[i];//体积
            }

            double gas98Mass = 0;//总质量
            for (int i = 0; i < SchemeCompOilList.Count; i++)
            {
                gas98Mass = gas98Mass +  gas98VolumeProduct[i] * CompOilList[i].den / 1000;//体积
            }
            // 十六烷值指数 基于体积
            double gas98ronVol = 0;
            for(int i = 0;i < CompOilList.Count; i++)
            {
                gas98ronVol = gas98ronVol + CompOilList[i].ron * gas98VolumeProduct[i];//十六烷值乘以体积
            }

            double Totalgas98ronVol = Math.Round(gas98ronVol / gas98VolProduct, 1); 

            // 50%回收温度 基于体积
            double gas98t50Vol = 0;
            for (int i = 0; i < CompOilList.Count; i++)
            {
                gas98t50Vol = gas98t50Vol + CompOilList[i].t50 * gas98VolumeProduct[i];//馏程乘以体积
            }

            double Totalgas98t50Vol = Math.Round(gas98t50Vol / gas98VolProduct);

            // 多环芳烃含量 基于质量
            double gas98sufVol = 0;
            for (int i = 0; i < CompOilList.Count; i++)
            {
                gas98sufVol = gas98sufVol + CompOilList[i].suf * gas98VolumeProduct[i] * CompOilList[i].den / 1000;//多芳烃基于质量 单位吨
            }
            //double gas98BottomVol = SchemeBottomList[1].BottomVolume;
            //double gas98BottomMass_1 = gas98BottomVol * SchemeBottomList[1].denVol / 1000;//场景1的体积界面下的车柴罐底油质量 单位吨
            double Totalgas98sufVol = Math.Round(gas98sufVol / gas98Mass, 2);

            // 密度 基于质量
            double gas98denVol = 0;
            for (int i = 0; i < CompOilList.Count; i++)
            {
                gas98denVol = gas98denVol + CompOilList[i].den * gas98VolumeProduct[i] * CompOilList[i].den / 1000;//密度基于质量 单位吨
            }
            double Totalgas98denVol = Math.Round(gas98denVol / gas98Mass, 1);
            #endregion 备用成品油1 end

            #region 备用成品油2
            /// <summary>
            /// 出口柴油
            /// </summary>
            double[] gasSelfVolumeProduct = new double[SchemeCompOilList.Count];//暂存体积产量  
            for (int i = 0; i < SchemeCompOilList.Count; i++)//将配方转化为体积产量
            {
                gasSelfVolumeProduct[i] = SchemeCompOilList[i].gasSelfFlowPercentVol * SchemeBottomList[3].TotalBlendVol2 / 100;
            }  
            double gasSelfVolProduct = 0;//总体积
            for (int i = 0; i < SchemeCompOilList.Count; i++)
            {
                gasSelfVolProduct = gasSelfVolProduct + gasSelfVolumeProduct[i];//体积
            }

            double gasSelfMass = 0;//总质量
            for (int i = 0; i < SchemeCompOilList.Count; i++)
            {
                gasSelfMass = gasSelfMass +  gasSelfVolumeProduct[i] * CompOilList[i].den / 1000;//体积
            }

            // 出柴十六烷值指数 基于体积
            double gasSelfronVol = 0;
            for(int i = 0;i < CompOilList.Count; i++)
            {
                gasSelfronVol = gasSelfronVol + CompOilList[i].ron * gasSelfVolumeProduct[i];//十六烷值乘以体积
            }

            double TotalgasSelfronVol = Math.Round(gasSelfronVol / gasSelfVolProduct, 1); 

            // 出柴50%回收温度 基于体积
            double gasSelft50Vol = 0;
            for (int i = 0; i < CompOilList.Count; i++)
            {
                gasSelft50Vol = gasSelft50Vol + CompOilList[i].t50 * gasSelfVolumeProduct[i];//馏程乘以体积
            }

            double TotalgasSelft50Vol = Math.Round(gasSelft50Vol / gasSelfVolProduct);

            // 多环芳烃含量 基于质量
            double gasSelfsufVol = 0;
            for (int i = 0; i < CompOilList.Count; i++)
            {
                gasSelfsufVol = gasSelfsufVol + CompOilList[i].suf * gasSelfVolumeProduct[i] * CompOilList[i].den / 1000;//多芳烃基于质量 单位吨
            }
            //double gasSelfBottomVol = SchemeBottomList[1].BottomVolume;
            //double gasSelfBottomMass_1 = gasSelfBottomVol * SchemeBottomList[1].denVol / 1000;//场景1的体积界面下的车柴罐底油质量 单位吨
            double TotalgasSelfsufVol = Math.Round(gasSelfsufVol / gasSelfMass, 2);

            // 密度 基于质量
            double gasSelfdenVol = 0;
            for (int i = 0; i < CompOilList.Count; i++)
            {
                gasSelfdenVol = gasSelfdenVol + CompOilList[i].den * gasSelfVolumeProduct[i] * CompOilList[i].den / 1000;//密度基于质量 单位吨
            }
            double TotalgasSelfdenVol = Math.Round(gasSelfdenVol / gasSelfMass, 1);
            #endregion 备用成品油2 end

            #endregion 场景2——体积 end

            #region 返回车柴数据
            GasSchemeVerify_2Res_2 result1 = new GasSchemeVerify_2Res_2();//实体，可以理解为一个对象
            /// <summary>
            /// 体积界面
            /// </summary>
            result1.ProdOilName = SchemeBottomList[0].ProdOilName;
            result1.Prodron = (float)Totalgas92ronVol;
            result1.Prodt50 = (float)Totalgas92t50Vol;
            result1.Prodsuf = (float)Totalgas92sufVol;
            result1.Prodden = (float)Totalgas92denVol;
            /// <summary>
            /// 高低限不分质量和体积
            /// </summary>
            result1.ronLowLimit = ProdOilList[0].ronLowLimit;
            result1.ronHighLimit = ProdOilList[0].ronHighLimit;
            result1.t50LowLimit = ProdOilList[0].t50LowLimit;
            result1.t50HighLimit = ProdOilList[0].t50HighLimit;
            result1.sufLowLimit = ProdOilList[0].sufLowLimit;
            result1.sufHighLimit = ProdOilList[0].sufHighLimit;
            result1.denLowLimit = ProdOilList[0].denLowLimit;
            result1.denHighLimit = ProdOilList[0].denHighLimit;

            ResultList.Add(result1);           
            #endregion

            #region 返回出柴数据
            GasSchemeVerify_2Res_2 result2 = new GasSchemeVerify_2Res_2();//实体，可以理解为一个对象
            /// <summary>
            /// 体积界面
            /// </summary>
            result2.ProdOilName = SchemeBottomList[1].ProdOilName;
            result2.Prodron = (float)Totalgas95ronVol;
            result2.Prodt50 = (float)Totalgas95t50Vol;
            result2.Prodsuf = (float)Totalgas95sufVol;
            result2.Prodden = (float)Totalgas95denVol;
            /// <summary>
            /// 高地限不分质量和体积
            /// </summary>
            result2.ronLowLimit = ProdOilList[1].ronLowLimit;
            result2.ronHighLimit = ProdOilList[1].ronHighLimit;
            result2.t50LowLimit = ProdOilList[1].t50LowLimit;
            result2.t50HighLimit = ProdOilList[1].t50HighLimit;
            result2.sufLowLimit = ProdOilList[1].sufLowLimit;
            result2.sufHighLimit = ProdOilList[1].sufHighLimit;
            result2.denLowLimit = ProdOilList[1].denLowLimit;
            result2.denHighLimit = ProdOilList[1].denHighLimit;

            ResultList.Add(result2);

            #endregion

            #region 返回备用成品油1数据
            GasSchemeVerify_2Res_2 result3 = new GasSchemeVerify_2Res_2();//实体，可以理解为一个对象
            /// <summary>
            /// 体积界面
            /// </summary>
            result3.ProdOilName = SchemeBottomList[2].ProdOilName;
            result3.Prodron = (float)Totalgas98ronVol;
            result3.Prodt50 = (float)Totalgas98t50Vol;
            result3.Prodsuf = (float)Totalgas98sufVol;
            result3.Prodden = (float)Totalgas98denVol;
            /// <summary>
            /// 高地限不分质量和体积
            /// </summary>
            result3.ronLowLimit = ProdOilList[2].ronLowLimit;
            result3.ronHighLimit = ProdOilList[2].ronHighLimit;
            result3.t50LowLimit = ProdOilList[2].t50LowLimit;
            result3.t50HighLimit = ProdOilList[2].t50HighLimit;
            result3.sufLowLimit = ProdOilList[2].sufLowLimit;
            result3.sufHighLimit = ProdOilList[2].sufHighLimit;
            result3.denLowLimit = ProdOilList[2].denLowLimit;
            result3.denHighLimit = ProdOilList[2].denHighLimit;

            ResultList.Add(result3);

            #endregion

            #region 返回备用成品油2数据
            GasSchemeVerify_2Res_2 result4 = new GasSchemeVerify_2Res_2();//实体，可以理解为一个对象
            /// <summary>
            /// 体积界面
            /// </summary>
            result4.ProdOilName = SchemeBottomList[3].ProdOilName;
            result4.Prodron = (float)TotalgasSelfronVol;
            result4.Prodt50 = (float)TotalgasSelft50Vol;
            result4.Prodsuf = (float)TotalgasSelfsufVol;
            result4.Prodden = (float)TotalgasSelfdenVol;
            /// <summary>
            /// 高地限不分质量和体积
            /// </summary>
            result4.ronLowLimit = ProdOilList[3].ronLowLimit;
            result4.ronHighLimit = ProdOilList[3].ronHighLimit;
            result4.t50LowLimit = ProdOilList[3].t50LowLimit;
            result4.t50HighLimit = ProdOilList[3].t50HighLimit;
            result4.sufLowLimit = ProdOilList[3].sufLowLimit;
            result4.sufHighLimit = ProdOilList[3].sufHighLimit;
            result4.denLowLimit = ProdOilList[3].denLowLimit;
            result4.denHighLimit = ProdOilList[3].denHighLimit;

            ResultList.Add(result4);

            #endregion

            return ResultList;
        }
        
        public IEnumerable<GasSchemeVerify_3Res_1> GetSchemeverifyResult3_mass_Time()//场景3质量 调合时间
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

            List<GasSchemeVerify_3Res_1> ResultList = new List<GasSchemeVerify_3Res_1>();//列表，里面可以添加很多个对象

            double gas92MassFlow = 0;//车用柴油质量产量
            for (int i = 0; i < SchemeCompOilList.Count; i++) 
            {
                gas92MassFlow = gas92MassFlow + SchemeCompOilList[i].gas92FlowMass;             
            }

            double gas95MassFlow = 0;//出口柴油质量产量
            for (int i = 0; i < SchemeCompOilList.Count; i++) 
            {
                gas95MassFlow = gas95MassFlow + SchemeCompOilList[i].gas95FlowMass;             
            }
            double gas98MassFlow = 0;//质量产量
            for (int i = 0; i < SchemeCompOilList.Count; i++) 
            {
                gas98MassFlow = gas98MassFlow + SchemeCompOilList[i].gas98FlowMass;             
            }

            double gasSelfMassFlow = 0;//质量产量
            for (int i = 0; i < SchemeCompOilList.Count; i++) 
            {
                gasSelfMassFlow = gasSelfMassFlow + SchemeCompOilList[i].gasSelfFlowMass;             
            }

            //车柴
            GasSchemeVerify_3Res_1 result1 = new GasSchemeVerify_3Res_1();//实体，可以理解为一个对象
            /// <summary>
            /// 质量界面
            /// </summary>
            result1.ProdOilName = SchemeBottomList[0].ProdOilName;
            result1.Time = (float)Math.Round((SchemeBottomList[0].TotalBlendMass / gas92MassFlow) * 24, 1);

            ResultList.Add(result1);   

            //出柴
            GasSchemeVerify_3Res_1 result2 = new GasSchemeVerify_3Res_1();//实体，可以理解为一个对象
            /// <summary>
            /// 质量界面
            /// </summary>
            result2.ProdOilName = SchemeBottomList[1].ProdOilName;
            result2.Time = (float)Math.Round((SchemeBottomList[1].TotalBlendMass / gas95MassFlow) * 24, 1);

            ResultList.Add(result2);

            //备用成品油1
            GasSchemeVerify_3Res_1 result3 = new GasSchemeVerify_3Res_1();//实体，可以理解为一个对象
            /// <summary>
            /// 质量界面
            /// </summary>
            result3.ProdOilName = SchemeBottomList[2].ProdOilName;
            result3.Time = (float)Math.Round((SchemeBottomList[2].TotalBlendMass / gas98MassFlow) * 24, 1);

            ResultList.Add(result3);

            //备用成品油2
            GasSchemeVerify_3Res_1 result4 = new GasSchemeVerify_3Res_1();//实体，可以理解为一个对象
            /// <summary>
            /// 质量界面
            /// </summary>
            result4.ProdOilName = SchemeBottomList[3].ProdOilName;
            result4.Time = (float)Math.Round((SchemeBottomList[3].TotalBlendMass / gasSelfMassFlow) * 24, 1);

            ResultList.Add(result4);

            return ResultList;
        }
        
        public IEnumerable<GasSchemeVerify_3Res_1> GetSchemeverifyResult3_vol_Time()//场景3体积 调合时间
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

            List<GasSchemeVerify_3Res_1> ResultList = new List<GasSchemeVerify_3Res_1>();//列表，里面可以添加很多个对象

            double gas92VolFlow = 0;//车用柴油体积流量
            for (int i = 0; i < SchemeCompOilList.Count; i++) 
            {
                gas92VolFlow = gas92VolFlow + SchemeCompOilList[i].gas92FlowVol;             
            }
            double gas95VolFlow = 0;//出口柴油体积产量
            for (int i = 0; i < SchemeCompOilList.Count; i++) 
            {
                gas95VolFlow = gas95VolFlow + SchemeCompOilList[i].gas95FlowVol;             
            }
            double gas98VolFlow = 0;//体积流量
            for (int i = 0; i < SchemeCompOilList.Count; i++) 
            {
                gas98VolFlow = gas98VolFlow + SchemeCompOilList[i].gas98FlowVol;             
            }
            double gasSelfVolFlow = 0;//体积产量
            for (int i = 0; i < SchemeCompOilList.Count; i++) 
            {
                gasSelfVolFlow = gasSelfVolFlow + SchemeCompOilList[i].gasSelfFlowVol;             
            }

            //车柴
            GasSchemeVerify_3Res_1 result1 = new GasSchemeVerify_3Res_1();//实体，可以理解为一个对象
            /// <summary>
            /// 体积界面
            /// </summary>
            result1.ProdOilName = SchemeBottomList[0].ProdOilName;
            result1.Time = (float)Math.Round((SchemeBottomList[0].TotalBlendVol / gas92VolFlow) * 24, 1);

            ResultList.Add(result1);  

            //出柴
            GasSchemeVerify_3Res_1 result2 = new GasSchemeVerify_3Res_1();//实体，可以理解为一个对象
            /// <summary>
            /// 体积界面
            /// </summary>
            result2.ProdOilName = SchemeBottomList[1].ProdOilName;
            result2.Time = (float)Math.Round((SchemeBottomList[1].TotalBlendVol / gas95VolFlow) * 24, 1);

            ResultList.Add(result2);

            //备用成品油1
            GasSchemeVerify_3Res_1 result3 = new GasSchemeVerify_3Res_1();//实体，可以理解为一个对象
            /// <summary>
            /// 体积界面
            /// </summary>
            result3.ProdOilName = SchemeBottomList[2].ProdOilName;
            result3.Time = (float)Math.Round((SchemeBottomList[2].TotalBlendVol / gas98VolFlow) * 24, 1);

            ResultList.Add(result3);

            //备用成品油2
            GasSchemeVerify_3Res_1 result4 = new GasSchemeVerify_3Res_1();//实体，可以理解为一个对象
            /// <summary>
            /// 体积界面
            /// </summary>
            result4.ProdOilName = SchemeBottomList[3].ProdOilName;
            result4.Time = (float)Math.Round((SchemeBottomList[3].TotalBlendVol / gasSelfVolFlow) * 24, 1);

            ResultList.Add(result4);
            
            return ResultList;
        }
        
        public IEnumerable<GasSchemeVerify_3Res_2> GetSchemeverifyResult3_mass_Product()//场景3质量 成品油产量
        {
            var SchemeCompOilList = context.Schemeverify1_gases.ToList();
            var SchemeBottomList = context.Schemeverify2_gases.ToList();
            double gas92MassFlow = 0;//车用柴油质量流量
            for (int i = 0; i < SchemeCompOilList.Count; i++) 
            {
                gas92MassFlow = gas92MassFlow + SchemeCompOilList[i].gas92FlowMass;             
            }
            double gas95MassFlow = 0;//出口柴油质量流量
            for (int i = 0; i < SchemeCompOilList.Count; i++) 
            {
                gas95MassFlow = gas95MassFlow + SchemeCompOilList[i].gas95FlowMass;             
            }
            double gas98MassFlow = 0;//备用油1质量流量
            for (int i = 0; i < SchemeCompOilList.Count; i++) 
            {
                gas98MassFlow = gas98MassFlow + SchemeCompOilList[i].gas98FlowMass;             
            }
            double gasSelfMassFlow = 0;//备用油2质量流量
            for (int i = 0; i < SchemeCompOilList.Count; i++) 
            {
                gasSelfMassFlow = gasSelfMassFlow + SchemeCompOilList[i].gasSelfFlowMass;             
            }

            List<GasSchemeVerify_3Res_2> ResultList = new List<GasSchemeVerify_3Res_2>();//列表，里面可以添加很多个对象
            for(int i = 0; i < SchemeCompOilList.Count; i++){
                GasSchemeVerify_3Res_2 result = new GasSchemeVerify_3Res_2();//实体，可以理解为一个对象
                result.ComOilName = SchemeCompOilList[i].ComOilName;
                result.gas92Product = (float)Math.Round(SchemeCompOilList[i].gas92FlowMass / gas92MassFlow * SchemeBottomList[0].TotalBlendMass3, 2);
                result.gas95Product = (float)Math.Round(SchemeCompOilList[i].gas95FlowMass / gas95MassFlow * SchemeBottomList[1].TotalBlendMass3, 2);
                result.gas98Product = (float)Math.Round(SchemeCompOilList[i].gas98FlowMass / gas98MassFlow * SchemeBottomList[2].TotalBlendMass3, 2);
                result.gasSelfProduct = (float)Math.Round(SchemeCompOilList[i].gasSelfFlowMass / gasSelfMassFlow * SchemeBottomList[3].TotalBlendMass3, 2);
                ResultList.Add(result);
            }
            return ResultList;         

        }
        
        public IEnumerable<GasSchemeVerify_3Res_2> GetSchemeverifyResult3_vol_Product()//场景3体积 成品油产量
        {
            var SchemeCompOilList = context.Schemeverify1_gases.ToList();
            var SchemeBottomList = context.Schemeverify2_gases.ToList();
            double gas92VolFlow = 0;//车用柴油体积流量
            for (int i = 0; i < SchemeCompOilList.Count; i++) 
            {
                gas92VolFlow = gas92VolFlow + SchemeCompOilList[i].gas92FlowVol;             
            }
            double gas95VolFlow = 0;//出口柴油体积流量
            for (int i = 0; i < SchemeCompOilList.Count; i++) 
            {
                gas95VolFlow = gas95VolFlow + SchemeCompOilList[i].gas95FlowVol;             
            }
            double gas98VolFlow = 0;//备用油1体积流量
            for (int i = 0; i < SchemeCompOilList.Count; i++) 
            {
                gas98VolFlow = gas98VolFlow + SchemeCompOilList[i].gas98FlowVol;             
            }
            double gasSelfVolFlow = 0;//备用油2体积流量
            for (int i = 0; i < SchemeCompOilList.Count; i++) 
            {
                gasSelfVolFlow = gasSelfVolFlow + SchemeCompOilList[i].gasSelfFlowVol;             
            }

            List<GasSchemeVerify_3Res_2> ResultList = new List<GasSchemeVerify_3Res_2>();//列表，里面可以添加很多个对象
            for(int i = 0; i < SchemeCompOilList.Count; i++){
                GasSchemeVerify_3Res_2 result = new GasSchemeVerify_3Res_2();//实体，可以理解为一个对象
                result.ComOilName = SchemeCompOilList[i].ComOilName;
                result.gas92Product = (float)Math.Round(SchemeCompOilList[i].gas92FlowVol / gas92VolFlow * SchemeBottomList[0].TotalBlendVol3, 2);
                result.gas95Product = (float)Math.Round(SchemeCompOilList[i].gas95FlowVol / gas95VolFlow * SchemeBottomList[1].TotalBlendVol3, 2);
                result.gas98Product = (float)Math.Round(SchemeCompOilList[i].gas98FlowVol / gas98VolFlow * SchemeBottomList[2].TotalBlendVol3, 2);
                result.gasSelfProduct = (float)Math.Round(SchemeCompOilList[i].gasSelfFlowVol / gasSelfVolFlow * SchemeBottomList[3].TotalBlendVol3, 2);
                ResultList.Add(result);
            }
            return ResultList;    
        }
        
        public IEnumerable<GasSchemeVerify_3Res_3> GetSchemeverifyResult3_mass_Property()//场景3质量 成品油属性
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

            List<GasSchemeVerify_3Res_3> ResultList = new List<GasSchemeVerify_3Res_3>();//列表，里面可以添加很多个对象

            #region 场景3——质量 

            #region 车柴
            /// <summary>
            /// 车用柴油
            /// </summary>
            double gas92MassFlow = 0;//车用柴油总质量流量
            for (int i = 0; i < SchemeCompOilList.Count; i++) 
            {
                gas92MassFlow = gas92MassFlow + SchemeCompOilList[i].gas92FlowMass;             
            }

            double gas92Volume = 0;//车用柴油总体积
            for (int i = 0; i < SchemeCompOilList.Count; i++)
            {
                gas92Volume = gas92Volume + SchemeCompOilList[i].gas92FlowMass / CompOilList[i].den * 1000;//体积
            }
            // 车柴十六烷值指数 基于体积
            double gas92ronMass = 0;
            for(int i = 0;i < CompOilList.Count; i++)
            {
                gas92ronMass = gas92ronMass + CompOilList[i].ron * SchemeCompOilList[i].gas92FlowMass / CompOilList[i].den * 1000;//十六烷值乘以体积
            }

            double Totalgas92ronMass = Math.Round(gas92ronMass / gas92Volume, 1); 
            // 车柴50%回收温度 基于体积
            double gas92t50Mass = 0;
            for (int i = 0; i < CompOilList.Count; i++)
            {
                gas92t50Mass = gas92t50Mass + CompOilList[i].t50 * SchemeCompOilList[i].gas92FlowMass / CompOilList[i].den * 1000;//馏程乘以体积
            }

            double Totalgas92t50Mass = Math.Round(gas92t50Mass / gas92Volume);

            // 多环芳烃含量 基于质量
            double gas92sufMass = 0;
            for (int i = 0; i < CompOilList.Count; i++)
            {
                gas92sufMass = gas92sufMass + CompOilList[i].suf * SchemeCompOilList[i].gas92FlowMass;//多芳烃基于质量
            }
            double Totalgas92sufMass = Math.Round(gas92sufMass / gas92MassFlow, 2);

            // 密度 基于质量
            double gas92denMass = 0;
            for (int i = 0; i < CompOilList.Count; i++)
            {
                gas92denMass = gas92denMass + CompOilList[i].den * SchemeCompOilList[i].gas92FlowMass;//密度乘以质量
            }
            double Totalgas92denMass = Math.Round(gas92denMass / gas92MassFlow, 1);
            #endregion 车用柴油 end

            #region 出口柴油         
            /// <summary>
            /// 出口柴油
            /// </summary>
            double gas95MassFlow = 0;//出口柴油质量产量
            for (int i = 0; i < SchemeCompOilList.Count; i++) 
            {
                gas95MassFlow = gas95MassFlow + SchemeCompOilList[i].gas95FlowMass;             
            }

            double gas95Volume = 0;//出口柴油总体积
            for (int i = 0; i < SchemeCompOilList.Count; i++)
            {
                gas95Volume = gas95Volume + SchemeCompOilList[i].gas95FlowMass / CompOilList[i].den * 1000;//体积
            }
            // 出柴十六烷值指数 基于体积
            double gas95ronMass = 0;
            for(int i = 0;i < CompOilList.Count; i++)
            {
                gas95ronMass = gas95ronMass + CompOilList[i].ron * SchemeCompOilList[i].gas95FlowMass / CompOilList[i].den * 1000;//十六烷值乘以体积
            }

            double Totalgas95ronMass = Math.Round(gas95ronMass / gas95Volume, 1); 

            // 出柴50%回收温度 基于体积
            double gas95t50Mass = 0;
            for (int i = 0; i < CompOilList.Count; i++)
            {
                gas95t50Mass = gas95t50Mass + CompOilList[i].t50 * SchemeCompOilList[i].gas95FlowMass / CompOilList[i].den * 1000;//馏程乘以体积
            }

            double Totalgas95t50Mass = Math.Round(gas95t50Mass / gas95Volume);

            // 多环芳烃含量 基于质量
            double gas95sufMass = 0;
            for (int i = 0; i < CompOilList.Count; i++)
            {
                gas95sufMass = gas95sufMass + CompOilList[i].suf * SchemeCompOilList[i].gas95FlowMass;//多芳烃基于质量
            }
            double Totalgas95sufMass = Math.Round(gas95sufMass / gas95MassFlow, 2);

            // 密度 基于质量
            double gas95denMass = 0;
            for (int i = 0; i < CompOilList.Count; i++)
            {
                gas95denMass = gas95denMass + CompOilList[i].den * SchemeCompOilList[i].gas95FlowMass;//密度乘以质量
            }
            double Totalgas95denMass = Math.Round(gas95denMass / gas95MassFlow, 1);

            #endregion 出口柴油 end

            #region 备用成品油1         
            /// <summary>
            /// 备用成品油1
            /// </summary>
            double gas98MassFlow = 0;//出口柴油质量产量
            for (int i = 0; i < SchemeCompOilList.Count; i++) 
            {
                gas98MassFlow = gas98MassFlow + SchemeCompOilList[i].gas98FlowMass;             
            }

            double gas98Volume = 0;//出口柴油总体积
            for (int i = 0; i < SchemeCompOilList.Count; i++)
            {
                gas98Volume = gas98Volume + SchemeCompOilList[i].gas98FlowMass / CompOilList[i].den * 1000;//体积
            }
            // 出柴十六烷值指数 基于体积
            double gas98ronMass = 0;
            for(int i = 0;i < CompOilList.Count; i++)
            {
                gas98ronMass = gas98ronMass + CompOilList[i].ron * SchemeCompOilList[i].gas98FlowMass / CompOilList[i].den * 1000;//十六烷值乘以体积
            }

            double Totalgas98ronMass = Math.Round(gas98ronMass / gas98Volume, 1); 

            // 出柴50%回收温度 基于体积
            double gas98t50Mass = 0;
            for (int i = 0; i < CompOilList.Count; i++)
            {
                gas98t50Mass = gas98t50Mass + CompOilList[i].t50 * SchemeCompOilList[i].gas98FlowMass / CompOilList[i].den * 1000;//馏程乘以体积
            }

            double Totalgas98t50Mass = Math.Round(gas98t50Mass / gas98Volume);

            // 多环芳烃含量 基于质量
            double gas98sufMass = 0;
            for (int i = 0; i < CompOilList.Count; i++)
            {
                gas98sufMass = gas98sufMass + CompOilList[i].suf * SchemeCompOilList[i].gas98FlowMass;//多芳烃基于质量
            }
            double Totalgas98sufMass = Math.Round(gas98sufMass / gas98MassFlow, 2);

            // 密度 基于质量
            double gas98denMass = 0;
            for (int i = 0; i < CompOilList.Count; i++)
            {
                gas98denMass = gas98denMass + CompOilList[i].den * SchemeCompOilList[i].gas98FlowMass;//密度乘以质量
            }
            double Totalgas98denMass = Math.Round(gas98denMass / gas98MassFlow, 1);

            #endregion 出口柴油 end

            #region 备用成品油2         
            /// <summary>
            /// 备用成品油2
            /// </summary>
            double gasSelfMassFlow = 0;//质量流量
            for (int i = 0; i < SchemeCompOilList.Count; i++) 
            {
                gasSelfMassFlow = gasSelfMassFlow + SchemeCompOilList[i].gasSelfFlowMass;             
            }

            double gasSelfVolume = 0;//总体积
            for (int i = 0; i < SchemeCompOilList.Count; i++)
            {
                gasSelfVolume = gasSelfVolume + SchemeCompOilList[i].gasSelfFlowMass / CompOilList[i].den * 1000;//体积
            }
            // 出柴十六烷值指数 基于体积
            double gasSelfronMass = 0;
            for(int i = 0;i < CompOilList.Count; i++)
            {
                gasSelfronMass = gasSelfronMass + CompOilList[i].ron * SchemeCompOilList[i].gasSelfFlowMass / CompOilList[i].den * 1000;//十六烷值乘以体积
            }

            double TotalgasSelfronMass = Math.Round(gasSelfronMass / gasSelfVolume, 1); 

            // 出柴50%回收温度 基于体积
            double gasSelft50Mass = 0;
            for (int i = 0; i < CompOilList.Count; i++)
            {
                gasSelft50Mass = gasSelft50Mass + CompOilList[i].t50 * SchemeCompOilList[i].gasSelfFlowMass / CompOilList[i].den * 1000;//馏程乘以体积
            }

            double TotalgasSelft50Mass = Math.Round(gasSelft50Mass / gasSelfVolume);

            // 多环芳烃含量 基于质量
            double gasSelfsufMass = 0;
            for (int i = 0; i < CompOilList.Count; i++)
            {
                gasSelfsufMass = gasSelfsufMass + CompOilList[i].suf * SchemeCompOilList[i].gasSelfFlowMass;//多芳烃基于质量
            }
            double TotalgasSelfsufMass = Math.Round(gasSelfsufMass / gasSelfMassFlow, 2);

            // 密度 基于质量
            double gasSelfdenMass = 0;
            for (int i = 0; i < CompOilList.Count; i++)
            {
                gasSelfdenMass = gasSelfdenMass + CompOilList[i].den * SchemeCompOilList[i].gasSelfFlowMass;//密度乘以质量
            }
            double TotalgasSelfdenMass = Math.Round(gasSelfdenMass / gasSelfMassFlow, 1);

            #endregion 出口柴油 end
           
            #endregion 场景3——质量 end

            #region 返回车柴数据
            GasSchemeVerify_3Res_3 result1 = new GasSchemeVerify_3Res_3();//实体，可以理解为一个对象
            /// <summary>
            /// 质量界面
            /// </summary>
            result1.ProdOilName = SchemeBottomList[0].ProdOilName;
            result1.Prodron = (float)Totalgas92ronMass;
            result1.Prodt50 = (float)Totalgas92t50Mass;
            result1.Prodsuf = (float)Totalgas92sufMass;
            result1.Prodden = (float)Totalgas92denMass;
            /// <summary>
            /// 高低限不分质量和体积
            /// </summary>
            result1.ronLowLimit = ProdOilList[0].ronLowLimit;
            result1.ronHighLimit = ProdOilList[0].ronHighLimit;
            result1.t50LowLimit = ProdOilList[0].t50LowLimit;
            result1.t50HighLimit = ProdOilList[0].t50HighLimit;
            result1.sufLowLimit = ProdOilList[0].sufLowLimit;
            result1.sufHighLimit = ProdOilList[0].sufHighLimit;
            result1.denLowLimit = ProdOilList[0].denLowLimit;
            result1.denHighLimit = ProdOilList[0].denHighLimit;

            ResultList.Add(result1);           
            #endregion

            #region 返回出柴数据
            GasSchemeVerify_3Res_3 result2 = new GasSchemeVerify_3Res_3();//实体，可以理解为一个对象
            /// <summary>
            /// 质量界面
            /// </summary>
            result2.ProdOilName = SchemeBottomList[1].ProdOilName;
            result2.Prodron = (float)Totalgas95ronMass;
            result2.Prodt50 = (float)Totalgas95t50Mass;
            result2.Prodsuf = (float)Totalgas95sufMass;
            result2.Prodden = (float)Totalgas95denMass;
            /// <summary>
            /// 高地限不分质量和体积
            /// </summary>
            result2.ronLowLimit = ProdOilList[1].ronLowLimit;
            result2.ronHighLimit = ProdOilList[1].ronHighLimit;
            result2.t50LowLimit = ProdOilList[1].t50LowLimit;
            result2.t50HighLimit = ProdOilList[1].t50HighLimit;
            result2.sufLowLimit = ProdOilList[1].sufLowLimit;
            result2.sufHighLimit = ProdOilList[1].sufHighLimit;
            result2.denLowLimit = ProdOilList[1].denLowLimit;
            result2.denHighLimit = ProdOilList[1].denHighLimit;

            ResultList.Add(result2);

            #endregion

            #region 返回备用成品油1数据
            GasSchemeVerify_3Res_3 result3 = new GasSchemeVerify_3Res_3();//实体，可以理解为一个对象
            /// <summary>
            /// 质量界面
            /// </summary>
            result3.ProdOilName = SchemeBottomList[2].ProdOilName;
            result3.Prodron = (float)Totalgas98ronMass;
            result3.Prodt50 = (float)Totalgas98t50Mass;
            result3.Prodsuf = (float)Totalgas98sufMass;
            result3.Prodden = (float)Totalgas98denMass;
            /// <summary>
            /// 高地限不分质量和体积
            /// </summary>
            result3.ronLowLimit = ProdOilList[2].ronLowLimit;
            result3.ronHighLimit = ProdOilList[2].ronHighLimit;
            result3.t50LowLimit = ProdOilList[2].t50LowLimit;
            result3.t50HighLimit = ProdOilList[2].t50HighLimit;
            result3.sufLowLimit = ProdOilList[2].sufLowLimit;
            result3.sufHighLimit = ProdOilList[2].sufHighLimit;
            result3.denLowLimit = ProdOilList[2].denLowLimit;
            result3.denHighLimit = ProdOilList[2].denHighLimit;

            ResultList.Add(result3);

            #endregion

            #region 返回备用成品油2数据
            GasSchemeVerify_3Res_3 result4 = new GasSchemeVerify_3Res_3();//实体，可以理解为一个对象
            /// <summary>
            /// 质量界面
            /// </summary>
            result4.ProdOilName = SchemeBottomList[3].ProdOilName;
            result4.Prodron = (float)TotalgasSelfronMass;
            result4.Prodt50 = (float)TotalgasSelft50Mass;
            result4.Prodsuf = (float)TotalgasSelfsufMass;
            result4.Prodden = (float)TotalgasSelfdenMass;
            /// <summary>
            /// 高地限不分质量和体积
            /// </summary>
            result4.ronLowLimit = ProdOilList[3].ronLowLimit;
            result4.ronHighLimit = ProdOilList[3].ronHighLimit;
            result4.t50LowLimit = ProdOilList[3].t50LowLimit;
            result4.t50HighLimit = ProdOilList[3].t50HighLimit;
            result4.sufLowLimit = ProdOilList[3].sufLowLimit;
            result4.sufHighLimit = ProdOilList[3].sufHighLimit;
            result4.denLowLimit = ProdOilList[3].denLowLimit;
            result4.denHighLimit = ProdOilList[3].denHighLimit;

            ResultList.Add(result4);

            #endregion

            return ResultList;
        }
        
        public IEnumerable<GasSchemeVerify_3Res_3> GetSchemeverifyResult3_vol_Property()//场景3体积 成品油属性
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

            List<GasSchemeVerify_3Res_3> ResultList = new List<GasSchemeVerify_3Res_3>();//列表，里面可以添加很多个对象

            #region 场景3——体积

            #region 车用柴油
            /// <summary>
            /// 车用柴油
            /// </summary>
            double gas92VolFlow = 0;//车用柴油体积流量
            for (int i = 0; i < SchemeCompOilList.Count; i++) 
            {
                gas92VolFlow = gas92VolFlow + SchemeCompOilList[i].gas92FlowVol;             
            }

            double gas92Mass = 0;//车用柴油总质量
            for (int i = 0; i < SchemeCompOilList.Count; i++)
            {
                gas92Mass = gas92Mass + SchemeCompOilList[i].gas92FlowVol * CompOilList[i].den / 1000;//单位是吨
            }
            // 车柴十六烷值指数 基于体积
            double gas92ronVol = 0;
            for(int i = 0;i < CompOilList.Count; i++)
            {
                gas92ronVol = gas92ronVol + CompOilList[i].ron * SchemeCompOilList[i].gas92FlowVol;//十六烷值乘以体积
            }

            double Totalgas92ronVol = Math.Round(gas92ronVol / gas92VolFlow, 1); 

            // 车柴50%回收温度 基于体积
            double gas92t50Vol = 0;
            for (int i = 0; i < CompOilList.Count; i++)
            {
                gas92t50Vol = gas92t50Vol + CompOilList[i].t50 * SchemeCompOilList[i].gas92FlowVol;//馏程乘以体积
            }

            double Totalgas92t50Vol = Math.Round(gas92t50Vol  / gas92VolFlow);

            // 多环芳烃含量 基于质量
            double gas92sufVol = 0;
            for (int i = 0; i < CompOilList.Count; i++)
            {
                gas92sufVol = gas92sufVol + CompOilList[i].suf * SchemeCompOilList[i].gas92FlowVol * CompOilList[i].den / 1000;//多芳烃基于质量 单位吨
            }
            double Totalgas92sufVol = Math.Round(gas92sufVol / gas92Mass, 2);

            // 密度 基于质量
            double gas92denVol = 0;
            for (int i = 0; i < CompOilList.Count; i++)
            {
                gas92denVol = gas92denVol + CompOilList[i].den * SchemeCompOilList[i].gas92FlowVol * CompOilList[i].den / 1000;//密度基于质量 单位吨
            }
            double Totalgas92denVol = Math.Round(gas92denVol / gas92Mass, 1);
            
            #endregion

            #region 出口柴油
            /// <summary>
            /// 出口柴油
            /// </summary>
            double gas95VolFlow = 0;//出口柴油体积产量
            for (int i = 0; i < SchemeCompOilList.Count; i++) 
            {
                gas95VolFlow = gas95VolFlow + SchemeCompOilList[i].gas95FlowVol;             
            }

            double gas95Mass = 0;//出口柴油总质量
            for (int i = 0; i < SchemeCompOilList.Count; i++)
            {
                gas95Mass = gas95Mass + SchemeCompOilList[i].gas95FlowVol * CompOilList[i].den / 1000;//单位是吨
            }
            // 出柴十六烷值指数 基于体积
            double gas95ronVol = 0;
            for(int i = 0;i < CompOilList.Count; i++)
            {
                gas95ronVol = gas95ronVol + CompOilList[i].ron * SchemeCompOilList[i].gas95FlowVol;//十六烷值乘以体积
            }

            double Totalgas95ronVol = Math.Round(gas95ronVol / gas95VolFlow, 1); 

            // 出柴50%回收温度 基于体积
            double gas95t50Vol = 0;
            for (int i = 0; i < CompOilList.Count; i++)
            {
                gas95t50Vol = gas95t50Vol + CompOilList[i].t50 * SchemeCompOilList[i].gas95FlowVol;//馏程乘以体积
            }

            double Totalgas95t50Vol = Math.Round(gas95t50Vol / gas95VolFlow);

            // 多环芳烃含量 基于质量
            double gas95sufVol = 0;
            for (int i = 0; i < CompOilList.Count; i++)
            {
                gas95sufVol = gas95sufVol + CompOilList[i].suf * SchemeCompOilList[i].gas95FlowVol * CompOilList[i].den / 1000;//多芳烃基于质量 单位吨
            }
            double Totalgas95sufVol = Math.Round(gas95sufVol / gas95Mass, 2);

            // 密度 基于质量
            double gas95denVol = 0;
            for (int i = 0; i < CompOilList.Count; i++)
            {
                gas95denVol = gas95denVol + CompOilList[i].den * SchemeCompOilList[i].gas95FlowVol * CompOilList[i].den / 1000;//密度基于质量 单位吨
            }
            double Totalgas95denVol = Math.Round(gas95denVol/ gas95Mass, 1);

            #endregion 出口柴油 end

            #region 备用成品油1
            /// <summary>
            /// 备用成品油1
            /// </summary>
            double gas98VolFlow = 0;//体积产量
            for (int i = 0; i < SchemeCompOilList.Count; i++) 
            {
                gas98VolFlow = gas98VolFlow + SchemeCompOilList[i].gas98FlowVol;             
            }

            double gas98Mass = 0;//总质量
            for (int i = 0; i < SchemeCompOilList.Count; i++)
            {
                gas98Mass = gas98Mass + SchemeCompOilList[i].gas98FlowVol * CompOilList[i].den / 1000;//单位是吨
            }
            // 十六烷值指数 基于体积
            double gas98ronVol = 0;
            for(int i = 0;i < CompOilList.Count; i++)
            {
                gas98ronVol = gas98ronVol + CompOilList[i].ron * SchemeCompOilList[i].gas98FlowVol;//十六烷值乘以体积
            }

            double Totalgas98ronVol = Math.Round(gas98ronVol / gas98VolFlow, 1); 

            // 50%回收温度 基于体积
            double gas98t50Vol = 0;
            for (int i = 0; i < CompOilList.Count; i++)
            {
                gas98t50Vol = gas98t50Vol + CompOilList[i].t50 * SchemeCompOilList[i].gas98FlowVol;//馏程乘以体积
            }

            double Totalgas98t50Vol = Math.Round(gas98t50Vol / gas98VolFlow);

            // 多环芳烃含量 基于质量
            double gas98sufVol = 0;
            for (int i = 0; i < CompOilList.Count; i++)
            {
                gas98sufVol = gas98sufVol + CompOilList[i].suf * SchemeCompOilList[i].gas98FlowVol * CompOilList[i].den / 1000;//多芳烃基于质量 单位吨
            }
            double Totalgas98sufVol = Math.Round(gas98sufVol / gas98Mass, 2);

            // 密度 基于质量
            double gas98denVol = 0;
            for (int i = 0; i < CompOilList.Count; i++)
            {
                gas98denVol = gas98denVol + CompOilList[i].den * SchemeCompOilList[i].gas98FlowVol * CompOilList[i].den / 1000;//密度基于质量 单位吨
            }
            double Totalgas98denVol = Math.Round(gas98denVol/ gas98Mass, 1);

            #endregion 备用成品油1 end

            #region 备用成品油2
            /// <summary>
            /// 备用成品油2
            /// </summary>
            double gasSelfVolFlow = 0;//备用成品油2体积产量
            for (int i = 0; i < SchemeCompOilList.Count; i++) 
            {
                gasSelfVolFlow = gasSelfVolFlow + SchemeCompOilList[i].gasSelfFlowVol;             
            }

            double gasSelfMass = 0;//备用成品油2总质量
            for (int i = 0; i < SchemeCompOilList.Count; i++)
            {
                gasSelfMass = gasSelfMass + SchemeCompOilList[i].gasSelfFlowVol * CompOilList[i].den / 1000;//单位是吨
            }
            // 备用成品油2十六烷值指数 基于体积
            double gasSelfronVol = 0;
            for(int i = 0;i < CompOilList.Count; i++)
            {
                gasSelfronVol = gasSelfronVol + CompOilList[i].ron * SchemeCompOilList[i].gasSelfFlowVol;//十六烷值乘以体积
            }

            double TotalgasSelfronVol = Math.Round(gasSelfronVol / gasSelfVolFlow, 1); 

            // 备用成品油2 50%回收温度 基于体积
            double gasSelft50Vol = 0;
            for (int i = 0; i < CompOilList.Count; i++)
            {
                gasSelft50Vol = gasSelft50Vol + CompOilList[i].t50 * SchemeCompOilList[i].gasSelfFlowVol;//馏程乘以体积
            }

            double TotalgasSelft50Vol = Math.Round(gasSelft50Vol / gasSelfVolFlow);

            // 多环芳烃含量 基于质量
            double gasSelfsufVol = 0;
            for (int i = 0; i < CompOilList.Count; i++)
            {
                gasSelfsufVol = gasSelfsufVol + CompOilList[i].suf * SchemeCompOilList[i].gasSelfFlowVol * CompOilList[i].den / 1000;//多芳烃基于质量 单位吨
            }
            double TotalgasSelfsufVol = Math.Round(gasSelfsufVol / gasSelfMass, 2);

            // 密度 基于质量
            double gasSelfdenVol = 0;
            for (int i = 0; i < CompOilList.Count; i++)
            {
                gasSelfdenVol = gasSelfdenVol + CompOilList[i].den * SchemeCompOilList[i].gasSelfFlowVol * CompOilList[i].den / 1000;//密度基于质量 单位吨
            }
            double TotalgasSelfdenVol = Math.Round(gasSelfdenVol/ gasSelfMass, 1);

            #endregion 备用成品油2 end

            #endregion 场景3——体积 end

            #region 返回车柴数据
            GasSchemeVerify_3Res_3 result1 = new GasSchemeVerify_3Res_3();//实体，可以理解为一个对象
            /// <summary>
            /// 体积界面
            /// </summary>
            result1.ProdOilName = SchemeBottomList[0].ProdOilName;
            result1.Prodron = (float)Totalgas92ronVol;
            result1.Prodt50 = (float)Totalgas92t50Vol;
            result1.Prodsuf = (float)Totalgas92sufVol;
            result1.Prodden = (float)Totalgas92denVol;
            /// <summary>
            /// 高低限不分质量和体积
            /// </summary>
            result1.ronLowLimit = ProdOilList[0].ronLowLimit;
            result1.ronHighLimit = ProdOilList[0].ronHighLimit;
            result1.t50LowLimit = ProdOilList[0].t50LowLimit;
            result1.t50HighLimit = ProdOilList[0].t50HighLimit;
            result1.sufLowLimit = ProdOilList[0].sufLowLimit;
            result1.sufHighLimit = ProdOilList[0].sufHighLimit;
            result1.denLowLimit = ProdOilList[0].denLowLimit;
            result1.denHighLimit = ProdOilList[0].denHighLimit;

            ResultList.Add(result1);           
            #endregion

            #region 返回出柴数据
            GasSchemeVerify_3Res_3 result2 = new GasSchemeVerify_3Res_3();//实体，可以理解为一个对象
            /// <summary>
            /// 体积界面
            /// </summary>
            result2.ProdOilName = SchemeBottomList[1].ProdOilName;
            result2.Prodron = (float)Totalgas95ronVol;
            result2.Prodt50 = (float)Totalgas95t50Vol;
            result2.Prodsuf = (float)Totalgas95sufVol;
            result2.Prodden = (float)Totalgas95denVol;
            /// <summary>
            /// 高地限不分质量和体积
            /// </summary>
            result2.ronLowLimit = ProdOilList[1].ronLowLimit;
            result2.ronHighLimit = ProdOilList[1].ronHighLimit;
            result2.t50LowLimit = ProdOilList[1].t50LowLimit;
            result2.t50HighLimit = ProdOilList[1].t50HighLimit;
            result2.sufLowLimit = ProdOilList[1].sufLowLimit;
            result2.sufHighLimit = ProdOilList[1].sufHighLimit;
            result2.denLowLimit = ProdOilList[1].denLowLimit;
            result2.denHighLimit = ProdOilList[1].denHighLimit;

            ResultList.Add(result2);

            #endregion

            #region 返回备用成品油1数据
            GasSchemeVerify_3Res_3 result3 = new GasSchemeVerify_3Res_3();//实体，可以理解为一个对象
            /// <summary>
            /// 体积界面
            /// </summary>
            result3.ProdOilName = SchemeBottomList[2].ProdOilName;
            result3.Prodron = (float)Totalgas98ronVol;
            result3.Prodt50 = (float)Totalgas98t50Vol;
            result3.Prodsuf = (float)Totalgas98sufVol;
            result3.Prodden = (float)Totalgas98denVol;
            /// <summary>
            /// 高地限不分质量和体积
            /// </summary>
            result3.ronLowLimit = ProdOilList[2].ronLowLimit;
            result3.ronHighLimit = ProdOilList[2].ronHighLimit;
            result3.t50LowLimit = ProdOilList[2].t50LowLimit;
            result3.t50HighLimit = ProdOilList[2].t50HighLimit;
            result3.sufLowLimit = ProdOilList[2].sufLowLimit;
            result3.sufHighLimit = ProdOilList[2].sufHighLimit;
            result3.denLowLimit = ProdOilList[2].denLowLimit;
            result3.denHighLimit = ProdOilList[2].denHighLimit;

            ResultList.Add(result3);

            #endregion

            #region 返回备用成品油2数据
            GasSchemeVerify_3Res_3 result4 = new GasSchemeVerify_3Res_3();//实体，可以理解为一个对象
            /// <summary>
            /// 体积界面
            /// </summary>
            result4.ProdOilName = SchemeBottomList[3].ProdOilName;
            result4.Prodron = (float)TotalgasSelfronVol;
            result4.Prodt50 = (float)TotalgasSelft50Vol;
            result4.Prodsuf = (float)TotalgasSelfsufVol;
            result4.Prodden = (float)TotalgasSelfdenVol;
            /// <summary>
            /// 高地限不分质量和体积
            /// </summary>
            result4.ronLowLimit = ProdOilList[3].ronLowLimit;
            result4.ronHighLimit = ProdOilList[3].ronHighLimit;
            result4.t50LowLimit = ProdOilList[3].t50LowLimit;
            result4.t50HighLimit = ProdOilList[3].t50HighLimit;
            result4.sufLowLimit = ProdOilList[3].sufLowLimit;
            result4.sufHighLimit = ProdOilList[3].sufHighLimit;
            result4.denLowLimit = ProdOilList[3].denLowLimit;
            result4.denHighLimit = ProdOilList[3].denHighLimit;

            ResultList.Add(result4);

            #endregion

            return ResultList;
        }
        
        
    }
}