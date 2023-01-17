using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using OilBlendSystem.Models.Diesel.DataBaseModel;
using OilBlendSystem.Models.Gas.DataBaseModel;

namespace OilBlendSystem.Models
{
    public partial class oilblendContext : DbContext
    {
        public oilblendContext()
        {
        }

        public oilblendContext(DbContextOptions<oilblendContext> options)
            : base(options)
        {
        }

        //柴油
        public virtual DbSet<Compoilconfig> Compoilconfigs { get; set; } = null!;
        public virtual DbSet<Menulist> Menulists { get; set; } = null!;
        public virtual DbSet<Prodoilconfig> Prodoilconfigs { get; set; } = null!;
        public virtual DbSet<Property> Properties { get; set; } = null!;
        public virtual DbSet<Recipecalc1> Recipecalc1s { get; set; } = null!;
        public virtual DbSet<Recipecalc2> Recipecalc2s { get; set; } = null!;
        public virtual DbSet<Recipecalc2_2> Recipecalc2_2s { get; set; } = null!;
        public virtual DbSet<Recipecalc2_3> Recipecalc2_3s { get; set; } = null!;
        public virtual DbSet<Recipecalc3> Recipecalc3s { get; set; } = null!;
        public virtual DbSet<Schemeverify1> Schemeverify1s { get; set; } = null!;
        public virtual DbSet<Schemeverify2> Schemeverify2s { get; set; } = null!;
        public virtual DbSet<Dispatchweight> Dispatchweights { get; set; } = null!;

        //汽油
        public virtual DbSet<Compoilconfig_gas> Compoilconfig_gases { get; set; } = null!;
        public virtual DbSet<Menulist_gas> Menulist_gases { get; set; } = null!;
        public virtual DbSet<Prodoilconfig_gas> Prodoilconfig_gases { get; set; } = null!;
        public virtual DbSet<Property_gas> Propertie_gases { get; set; } = null!;
        public virtual DbSet<Recipecalc1_gas> Recipecalc1_gases { get; set; } = null!;
        public virtual DbSet<Recipecalc2_gas> Recipecalc2_gases { get; set; } = null!;
        public virtual DbSet<Recipecalc2_2_gas> Recipecalc2_2_gases { get; set; } = null!;
        public virtual DbSet<Recipecalc2_3_gas> Recipecalc2_3_gases { get; set; } = null!;
        public virtual DbSet<Recipecalc3_gas> Recipecalc3_gases { get; set; } = null!;
        public virtual DbSet<Schemeverify1_gas> Schemeverify1_gases { get; set; } = null!;
        public virtual DbSet<Schemeverify2_gas> Schemeverify2_gases { get; set; } = null!;
        public virtual DbSet<Dispatchweight_gas> Dispatchweight_gases { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //             if (!optionsBuilder.IsConfigured)
            //             {
            // //#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
            //                 optionsBuilder.UseMySql("server=localhost;database=oilblend;uid=root;pwd=535667", Microsoft.EntityFrameworkCore.ServerVersion.Parse("8.0.29-mysql"));
            //             }
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.UseCollation("utf8mb4_0900_ai_ci")
                .HasCharSet("utf8mb4");
            
            #region 柴油
            modelBuilder.Entity<Compoilconfig>(entity =>
            {
                entity.ToTable("compoilconfig");

                entity.Property(e => e.Id)
                    .ValueGeneratedNever()
                    .HasColumnName("id");

                entity.Property(e => e.AutoHigh1).HasColumnName("autoHigh1");

                entity.Property(e => e.AutoHigh2).HasColumnName("autoHigh2");

                entity.Property(e => e.AutoLow1).HasColumnName("autoLow1");

                entity.Property(e => e.AutoLow2).HasColumnName("autoLow2");

                entity.Property(e => e.Cet).HasColumnName("cet");

                entity.Property(e => e.ComOilName)
                    .HasMaxLength(128)
                    .HasColumnName("comOilName");

                entity.Property(e => e.D50).HasColumnName("d50");

                entity.Property(e => e.Den).HasColumnName("den");

                entity.Property(e => e.ExpHigh1).HasColumnName("expHigh1");

                entity.Property(e => e.ExpHigh2).HasColumnName("expHigh2");//带2的是智能决策用的

                entity.Property(e => e.ExpLow1).HasColumnName("expLow1");

                entity.Property(e => e.ExpLow2).HasColumnName("expLow2");
                
                entity.Property(e => e.Prod1High1).HasColumnName("prod1High1");

                entity.Property(e => e.Prod2High1).HasColumnName("prod2High1");

                entity.Property(e => e.Prod1Low1).HasColumnName("prod1Low1");

                entity.Property(e => e.Prod2Low1).HasColumnName("prod2Low1");

                entity.Property(e => e.Prod1High2).HasColumnName("prod1High2");

                entity.Property(e => e.Prod2High2).HasColumnName("prod2High2");

                entity.Property(e => e.Prod1Low2).HasColumnName("prod1Low2");

                entity.Property(e => e.Prod2Low2).HasColumnName("prod2Low2");

                entity.Property(e => e.HighVolume).HasColumnName("highVolume");

                entity.Property(e => e.IniVolume).HasColumnName("iniVolume");

                entity.Property(e => e.LowVolume).HasColumnName("lowVolume");

                entity.Property(e => e.PlanProduct1).HasColumnName("planProduct1");

                entity.Property(e => e.PlanProduct2).HasColumnName("planProduct2");

                entity.Property(e => e.PlanProduct3).HasColumnName("planProduct3");

                entity.Property(e => e.PlanProduct4).HasColumnName("planProduct4");

                entity.Property(e => e.PlanProduct5).HasColumnName("planProduct5");

                entity.Property(e => e.PlanProduct6).HasColumnName("planProduct6");

                entity.Property(e => e.PlanProduct7).HasColumnName("planProduct7");

                entity.Property(e => e.Pol).HasColumnName("pol");

                entity.Property(e => e.Price).HasColumnName("price");
            });

            modelBuilder.Entity<Menulist>(entity =>
            {
                entity.ToTable("menulist");

                entity.Property(e => e.ID)
                    .ValueGeneratedNever()
                    .HasColumnName("id");

                entity.Property(e => e.ChildID)
                    .HasMaxLength(128)
                    .HasColumnName("childID");

                entity.Property(e => e.Component)
                    .HasMaxLength(128)
                    .HasColumnName("component");

                entity.Property(e => e.Icon)
                    .HasMaxLength(128)
                    .HasColumnName("icon");

                entity.Property(e => e.MenuCode)
                    .HasMaxLength(128)
                    .HasColumnName("menuCode");

                entity.Property(e => e.MenuName)
                    .HasMaxLength(128)
                    .HasColumnName("menuName");

                entity.Property(e => e.MenuState)
                    .HasMaxLength(128)
                    .HasColumnName("menuState");

                entity.Property(e => e.MenuType)
                    .HasMaxLength(128)
                    .HasColumnName("menuType");

                entity.Property(e => e.ParentID)
                    .HasMaxLength(128)
                    .HasColumnName("parentID");

                entity.Property(e => e.Path)
                    .HasMaxLength(128)
                    .HasColumnName("path");
            });

            modelBuilder.Entity<Prodoilconfig>(entity =>
            {
                entity.ToTable("prodoilconfig");

                entity.Property(e => e.Id)
                    .ValueGeneratedNever()
                    .HasColumnName("id");

                entity.Property(e => e.CetHighLimit).HasColumnName("cetHighLimit");

                entity.Property(e => e.CetLowLimit).HasColumnName("cetLowLimit");

                entity.Property(e => e.D50HighLimit).HasColumnName("d50HighLimit");

                entity.Property(e => e.D50LowLimit).HasColumnName("d50LowLimit");

                entity.Property(e => e.Demand1HighLimit).HasColumnName("demand1HighLimit");

                entity.Property(e => e.Demand1LowLimit).HasColumnName("demand1LowLimit");

                entity.Property(e => e.Demand2HighLimit).HasColumnName("demand2HighLimit");

                entity.Property(e => e.Demand2LowLimit).HasColumnName("demand2LowLimit");

                entity.Property(e => e.Demand3HighLimit).HasColumnName("demand3HighLimit");

                entity.Property(e => e.Demand3LowLimit).HasColumnName("demand3LowLimit");

                entity.Property(e => e.Demand4HighLimit).HasColumnName("demand4HighLimit");

                entity.Property(e => e.Demand4LowLimit).HasColumnName("demand4LowLimit");

                entity.Property(e => e.Demand5HighLimit).HasColumnName("demand5HighLimit");

                entity.Property(e => e.Demand5LowLimit).HasColumnName("demand5LowLimit");

                entity.Property(e => e.Demand6HighLimit).HasColumnName("demand6HighLimit");

                entity.Property(e => e.Demand6LowLimit).HasColumnName("demand6LowLimit");

                entity.Property(e => e.Demand7HighLimit).HasColumnName("demand7HighLimit");

                entity.Property(e => e.Demand7LowLimit).HasColumnName("demand7LowLimit");

                entity.Property(e => e.DenHighLimit).HasColumnName("denHighLimit");

                entity.Property(e => e.DenLowLimit).HasColumnName("denLowLimit");

                entity.Property(e => e.IniVolume).HasColumnName("iniVolume");

                entity.Property(e => e.PolHighLimit).HasColumnName("polHighLimit");

                entity.Property(e => e.PolLowLimit).HasColumnName("polLowLimit");

                entity.Property(e => e.ProdOilName)
                    .HasMaxLength(128)
                    .HasColumnName("prodOilName");

                entity.Property(e => e.ProdVolumeHighLimit).HasColumnName("prodVolumeHighLimit");

                entity.Property(e => e.ProdVolumeLowLimit).HasColumnName("prodVolumeLowLimit");

                entity.Property(e => e.Apply).HasColumnName("Apply");
            });

            modelBuilder.Entity<Property>(entity =>
            {
                entity.ToTable("property");

                entity.Property(e => e.Id)
                    .ValueGeneratedNever()
                    .HasColumnName("id");

                entity.Property(e => e.Apply).HasColumnName("apply");

                entity.Property(e => e.PropertyName)
                    .HasMaxLength(128)
                    .HasColumnName("propertyName");
            });

            modelBuilder.Entity<Recipecalc1>(entity =>
            {
                entity.ToTable("recipecalc1");

                entity.Property(e => e.Id)
                    .ValueGeneratedNever()
                    .HasColumnName("id");

                entity.Property(e => e.ComOilName)
                    .HasMaxLength(128)
                    .HasColumnName("comOilName");

                entity.Property(e => e.ComOilProductHigh).HasColumnName("comOilProductHigh");

                entity.Property(e => e.ComOilProductLow).HasColumnName("comOilProductLow");

                entity.Property(e => e.AutoFlowHigh).HasColumnName("autoFlowHigh");

                entity.Property(e => e.AutoFlowLow).HasColumnName("autoFlowLow");

                entity.Property(e => e.ExpFlowHigh).HasColumnName("expFlowHigh");

                entity.Property(e => e.ExpFlowLow).HasColumnName("expFlowLow");

                entity.Property(e => e.Prod1FlowHigh).HasColumnName("prod1FlowHigh");

                entity.Property(e => e.Prod1FlowLow).HasColumnName("prod1FlowLow");

                entity.Property(e => e.Prod2FlowHigh).HasColumnName("prod2FlowHigh");

                entity.Property(e => e.Prod2FlowLow).HasColumnName("prod2FlowLow");

                entity.Property(e => e.IniVolume).HasColumnName("iniVolume");

                entity.Property(e => e.VolumeHigh).HasColumnName("volumeHigh");

                entity.Property(e => e.VolumeLow).HasColumnName("volumeLow");
            });

            modelBuilder.Entity<Recipecalc2>(entity =>
            {
                entity.ToTable("recipecalc2");

                entity.Property(e => e.Id)
                    .ValueGeneratedNever()
                    .HasColumnName("id");

                entity.Property(e => e.Weight).HasColumnName("weight");

                entity.Property(e => e.WeightName)
                    .HasMaxLength(128)
                    .HasColumnName("weightName");

                entity.Property(e => e.ProdOilStatus).HasColumnName("ProdOilStatus");

                entity.Property(e => e.Apply).HasColumnName("Apply");
            });

            modelBuilder.Entity<Recipecalc2_2>(entity =>
            {
                entity.ToTable("recipecalc2_2");

                entity.Property(e => e.Id)
                    .ValueGeneratedNever()
                    .HasColumnName("id");

                entity.Property(e => e.Weight).HasColumnName("weight");

                entity.Property(e => e.WeightName)
                    .HasMaxLength(128)
                    .HasColumnName("weightName");

                entity.Property(e => e.ProdOilStatus).HasColumnName("ProdOilStatus");

                entity.Property(e => e.Apply).HasColumnName("Apply");
            });

            modelBuilder.Entity<Recipecalc2_3>(entity =>
            {
                entity.ToTable("recipecalc2_3");

                entity.Property(e => e.Id)
                    .ValueGeneratedNever()
                    .HasColumnName("id");

                entity.Property(e => e.Weight).HasColumnName("weight");

                entity.Property(e => e.WeightName)
                    .HasMaxLength(128)
                    .HasColumnName("weightName");

                entity.Property(e => e.ProdOilStatus).HasColumnName("ProdOilStatus");

                entity.Property(e => e.Apply).HasColumnName("Apply");
            });

            modelBuilder.Entity<Recipecalc3>(entity =>
            {
                entity.ToTable("recipecalc3");

                entity.Property(e => e.Id)
                    .ValueGeneratedNever()
                    .HasColumnName("id");

                entity.Property(e => e.ProdOilName)
                    .HasMaxLength(128)
                    .HasColumnName("prodOilName");

                entity.Property(e => e.ProdOilProduct).HasColumnName("prodOilProduct");

                entity.Property(e => e.ProdOilProductLow).HasColumnName("prodOilProductLow");

                entity.Property(e => e.TotalFlow).HasColumnName("totalFlow");

                entity.Property(e => e.TotalFlow2).HasColumnName("totalFlow2");
                
                entity.Property(e => e.Apply).HasColumnName("Apply");
            });

            modelBuilder.Entity<Schemeverify1>(entity =>
            {
                entity.ToTable("schemeverify1");

                entity.Property(e => e.Id)
                    .ValueGeneratedNever()
                    .HasColumnName("id");

                entity.Property(e => e.AutoQualityProduct).HasColumnName("autoQualityProduct");

                entity.Property(e => e.ExpQualityProduct).HasColumnName("expQualityProduct");

                entity.Property(e => e.Prod1QualityProduct).HasColumnName("prod1QualityProduct");

                entity.Property(e => e.Prod2QualityProduct).HasColumnName("prod2QualityProduct");

                entity.Property(e => e.AutoFlowPercentMass).HasColumnName("autoFlowPercentMass");

                entity.Property(e => e.ExpFlowPercentMass).HasColumnName("expFlowPercentMass");

                entity.Property(e => e.Prod1FlowPercentMass).HasColumnName("prod1FlowPercentMass");

                entity.Property(e => e.Prod2FlowPercentMass).HasColumnName("prod2FlowPercentMass");

                entity.Property(e => e.AutoFlowMass).HasColumnName("autoFlowMass");

                entity.Property(e => e.ExpFlowMass).HasColumnName("expFlowMass");

                entity.Property(e => e.Prod1FlowMass).HasColumnName("prod1FlowMass");

                entity.Property(e => e.Prod2FlowMass).HasColumnName("prod2FlowMass");

                entity.Property(e => e.AutoVolumeProduct).HasColumnName("autoVolumeProduct");

                entity.Property(e => e.ExpVolumeProduct).HasColumnName("expVolumeProduct");

                entity.Property(e => e.Prod1VolumeProduct).HasColumnName("prod1VolumeProduct");

                entity.Property(e => e.Prod2VolumeProduct).HasColumnName("prod2VolumeProduct");

                entity.Property(e => e.AutoFlowPercentVol).HasColumnName("autoFlowPercentVol");

                entity.Property(e => e.ExpFlowPercentVol).HasColumnName("expFlowPercentVol");

                entity.Property(e => e.Prod1FlowPercentVol).HasColumnName("prod1FlowPercentVol");

                entity.Property(e => e.Prod2FlowPercentVol).HasColumnName("prod2FlowPercentVol");

                entity.Property(e => e.AutoFlowVol).HasColumnName("autoFlowVol");

                entity.Property(e => e.ExpFlowVol).HasColumnName("expFlowVol");

                entity.Property(e => e.Prod1FlowVol).HasColumnName("prod1FlowVol");

                entity.Property(e => e.Prod2FlowVol).HasColumnName("prod2FlowVol");

                entity.Property(e => e.ComOilName)
                    .HasMaxLength(128)
                    .HasColumnName("comOilName");

            });

            modelBuilder.Entity<Schemeverify2>(entity =>
            {
                entity.ToTable("schemeverify2");

                entity.Property(e => e.Id)
                    .ValueGeneratedNever()
                    .HasColumnName("id");

                entity.Property(e => e.BottomVolume).HasColumnName("bottomVolume");

                entity.Property(e => e.CetVol).HasColumnName("cetVol");

                entity.Property(e => e.D50Vol).HasColumnName("d50Vol");

                entity.Property(e => e.DenVol).HasColumnName("denVol");

                entity.Property(e => e.PolVol).HasColumnName("polVol");

                entity.Property(e => e.TotalBlendVol).HasColumnName("totalblendVol");

                entity.Property(e => e.TotalBlendVol2).HasColumnName("totalblendVol2");

                entity.Property(e => e.TotalBlendVol3).HasColumnName("totalblendVol3");

                entity.Property(e => e.BottomMass).HasColumnName("bottomMass");

                entity.Property(e => e.CetMass).HasColumnName("cetMass");

                entity.Property(e => e.D50Mass).HasColumnName("d50Mass");

                entity.Property(e => e.DenMass).HasColumnName("denMass");

                entity.Property(e => e.PolMass).HasColumnName("polMass");

                entity.Property(e => e.TotalBlendMass).HasColumnName("totalblendMass");

                entity.Property(e => e.TotalBlendMass2).HasColumnName("totalblendMass2");

                entity.Property(e => e.TotalBlendMass3).HasColumnName("totalblendMass3");


                entity.Property(e => e.ProdOilName)
                    .HasMaxLength(128)
                    .HasColumnName("prodOilName");
            });

            modelBuilder.Entity<Dispatchweight>(entity =>
            {
                entity.ToTable("dispatchweight");

                entity.Property(e => e.Id)
                    .ValueGeneratedNever()
                    .HasColumnName("id");

                entity.Property(e => e.Weight).HasColumnName("weight");

                entity.Property(e => e.WeightName)
                    .HasMaxLength(128)
                    .HasColumnName("weightName");
            });

            #endregion 柴油

            #region 汽油
            modelBuilder.Entity<Compoilconfig_gas>(entity =>
            {
                entity.ToTable("compoilconfig_gas");

                entity.Property(e => e.Id)
                    .ValueGeneratedNever()
                    .HasColumnName("id");

                entity.Property(e => e.gas92High1).HasColumnName("gas92High1");

                entity.Property(e => e.gas92High2).HasColumnName("gas92High2");

                entity.Property(e => e.gas92Low1).HasColumnName("gas92Low1");

                entity.Property(e => e.gas92Low2).HasColumnName("gas92Low2");

                entity.Property(e => e.ron).HasColumnName("ron");

                entity.Property(e => e.ComOilName)
                    .HasMaxLength(128)
                    .HasColumnName("comOilName");

                entity.Property(e => e.t50).HasColumnName("t50");

                entity.Property(e => e.den).HasColumnName("den");

                entity.Property(e => e.gas95High1).HasColumnName("gas95High1");

                entity.Property(e => e.gas95High2).HasColumnName("gas95High2");//带2的是智能决策用的

                entity.Property(e => e.gas95Low1).HasColumnName("gas95Low1");

                entity.Property(e => e.gas95Low2).HasColumnName("gas95Low2");
                
                entity.Property(e => e.gas98High1).HasColumnName("gas98High1");

                entity.Property(e => e.gasSelfHigh1).HasColumnName("gasSelfHigh1");

                entity.Property(e => e.gas98Low1).HasColumnName("gas98Low1");

                entity.Property(e => e.gasSelfLow1).HasColumnName("gasSelfLow1");

                entity.Property(e => e.gas98High2).HasColumnName("gas98High2");

                entity.Property(e => e.gasSelfHigh2).HasColumnName("gasSelfHigh2");

                entity.Property(e => e.gas98Low2).HasColumnName("gas98Low2");

                entity.Property(e => e.gasSelfLow2).HasColumnName("gasSelfLow2");

                entity.Property(e => e.HighVolume).HasColumnName("highVolume");

                entity.Property(e => e.IniVolume).HasColumnName("iniVolume");

                entity.Property(e => e.LowVolume).HasColumnName("lowVolume");

                entity.Property(e => e.PlanProduct1).HasColumnName("planProduct1");

                entity.Property(e => e.PlanProduct2).HasColumnName("planProduct2");

                entity.Property(e => e.PlanProduct3).HasColumnName("planProduct3");

                entity.Property(e => e.PlanProduct4).HasColumnName("planProduct4");

                entity.Property(e => e.PlanProduct5).HasColumnName("planProduct5");

                entity.Property(e => e.PlanProduct6).HasColumnName("planProduct6");

                entity.Property(e => e.PlanProduct7).HasColumnName("planProduct7");

                entity.Property(e => e.suf).HasColumnName("suf");

                entity.Property(e => e.Price).HasColumnName("price");
            });

            modelBuilder.Entity<Menulist_gas>(entity =>
            {
                entity.ToTable("menulist_gas");

                entity.Property(e => e.ID)
                    .ValueGeneratedNever()
                    .HasColumnName("id");

                entity.Property(e => e.ChildID)
                    .HasMaxLength(128)
                    .HasColumnName("childID");

                entity.Property(e => e.Component)
                    .HasMaxLength(128)
                    .HasColumnName("component");

                entity.Property(e => e.Icon)
                    .HasMaxLength(128)
                    .HasColumnName("icon");

                entity.Property(e => e.MenuCode)
                    .HasMaxLength(128)
                    .HasColumnName("menuCode");

                entity.Property(e => e.MenuName)
                    .HasMaxLength(128)
                    .HasColumnName("menuName");

                entity.Property(e => e.MenuState)
                    .HasMaxLength(128)
                    .HasColumnName("menuState");

                entity.Property(e => e.MenuType)
                    .HasMaxLength(128)
                    .HasColumnName("menuType");

                entity.Property(e => e.ParentID)
                    .HasMaxLength(128)
                    .HasColumnName("parentID");

                entity.Property(e => e.Path)
                    .HasMaxLength(128)
                    .HasColumnName("path");
            });

            modelBuilder.Entity<Prodoilconfig_gas>(entity =>
            {
                entity.ToTable("prodoilconfig_gas");

                entity.Property(e => e.Id)
                    .ValueGeneratedNever()
                    .HasColumnName("id");

                entity.Property(e => e.ronHighLimit).HasColumnName("ronHighLimit");

                entity.Property(e => e.ronLowLimit).HasColumnName("ronLowLimit");

                entity.Property(e => e.t50HighLimit).HasColumnName("t50HighLimit");

                entity.Property(e => e.t50LowLimit).HasColumnName("t50LowLimit");

                entity.Property(e => e.Demand1HighLimit).HasColumnName("demand1HighLimit");

                entity.Property(e => e.Demand1LowLimit).HasColumnName("demand1LowLimit");

                entity.Property(e => e.Demand2HighLimit).HasColumnName("demand2HighLimit");

                entity.Property(e => e.Demand2LowLimit).HasColumnName("demand2LowLimit");

                entity.Property(e => e.Demand3HighLimit).HasColumnName("demand3HighLimit");

                entity.Property(e => e.Demand3LowLimit).HasColumnName("demand3LowLimit");

                entity.Property(e => e.Demand4HighLimit).HasColumnName("demand4HighLimit");

                entity.Property(e => e.Demand4LowLimit).HasColumnName("demand4LowLimit");

                entity.Property(e => e.Demand5HighLimit).HasColumnName("demand5HighLimit");

                entity.Property(e => e.Demand5LowLimit).HasColumnName("demand5LowLimit");

                entity.Property(e => e.Demand6HighLimit).HasColumnName("demand6HighLimit");

                entity.Property(e => e.Demand6LowLimit).HasColumnName("demand6LowLimit");

                entity.Property(e => e.Demand7HighLimit).HasColumnName("demand7HighLimit");

                entity.Property(e => e.Demand7LowLimit).HasColumnName("demand7LowLimit");

                entity.Property(e => e.denHighLimit).HasColumnName("denHighLimit");

                entity.Property(e => e.denLowLimit).HasColumnName("denLowLimit");

                entity.Property(e => e.IniVolume).HasColumnName("iniVolume");

                entity.Property(e => e.sufHighLimit).HasColumnName("sufHighLimit");

                entity.Property(e => e.sufLowLimit).HasColumnName("sufLowLimit");

                entity.Property(e => e.ProdOilName)
                    .HasMaxLength(128)
                    .HasColumnName("prodOilName");

                entity.Property(e => e.ProdVolumeHighLimit).HasColumnName("prodVolumeHighLimit");

                entity.Property(e => e.ProdVolumeLowLimit).HasColumnName("prodVolumeLowLimit");

                entity.Property(e => e.Apply).HasColumnName("Apply");
            });

            modelBuilder.Entity<Property_gas>(entity =>
            {
                entity.ToTable("property_gas");

                entity.Property(e => e.Id)
                    .ValueGeneratedNever()
                    .HasColumnName("id");

                entity.Property(e => e.Apply).HasColumnName("apply");

                entity.Property(e => e.PropertyName)
                    .HasMaxLength(128)
                    .HasColumnName("propertyName");
            });

            modelBuilder.Entity<Recipecalc1_gas>(entity =>
            {
                entity.ToTable("recipecalc1_gas");

                entity.Property(e => e.Id)
                    .ValueGeneratedNever()
                    .HasColumnName("id");

                entity.Property(e => e.ComOilName)
                    .HasMaxLength(128)
                    .HasColumnName("comOilName");

                entity.Property(e => e.ComOilProductHigh).HasColumnName("comOilProductHigh");

                entity.Property(e => e.ComOilProductLow).HasColumnName("comOilProductLow");

                entity.Property(e => e.gas92FlowHigh).HasColumnName("gas92FlowHigh");

                entity.Property(e => e.gas92FlowLow).HasColumnName("gas92FlowLow");

                entity.Property(e => e.gas95FlowHigh).HasColumnName("gas95FlowHigh");

                entity.Property(e => e.gas95FlowLow).HasColumnName("gas95FlowLow");

                entity.Property(e => e.gas98FlowHigh).HasColumnName("gas98FlowHigh");

                entity.Property(e => e.gas98FlowLow).HasColumnName("gas98FlowLow");

                entity.Property(e => e.gasSelfFlowHigh).HasColumnName("gasSelfFlowHigh");

                entity.Property(e => e.gasSelfFlowLow).HasColumnName("gasSelfFlowLow");

                entity.Property(e => e.IniVolume).HasColumnName("iniVolume");

                entity.Property(e => e.VolumeHigh).HasColumnName("volumeHigh");

                entity.Property(e => e.VolumeLow).HasColumnName("volumeLow");
            });

            modelBuilder.Entity<Recipecalc2_gas>(entity =>
            {
                entity.ToTable("recipecalc2_gas");

                entity.Property(e => e.Id)
                    .ValueGeneratedNever()
                    .HasColumnName("id");

                entity.Property(e => e.Weight).HasColumnName("weight");

                entity.Property(e => e.WeightName)
                    .HasMaxLength(128)
                    .HasColumnName("weightName");

                entity.Property(e => e.ProdOilStatus).HasColumnName("ProdOilStatus");

                entity.Property(e => e.Apply).HasColumnName("Apply");
            });

            modelBuilder.Entity<Recipecalc2_2_gas>(entity =>
            {
                entity.ToTable("recipecalc2_2_gas");

                entity.Property(e => e.Id)
                    .ValueGeneratedNever()
                    .HasColumnName("id");

                entity.Property(e => e.Weight).HasColumnName("weight");

                entity.Property(e => e.WeightName)
                    .HasMaxLength(128)
                    .HasColumnName("weightName");

                entity.Property(e => e.ProdOilStatus).HasColumnName("ProdOilStatus");

                entity.Property(e => e.Apply).HasColumnName("Apply");
            });

            modelBuilder.Entity<Recipecalc2_3_gas>(entity =>
            {
                entity.ToTable("recipecalc2_3_gas");

                entity.Property(e => e.Id)
                    .ValueGeneratedNever()
                    .HasColumnName("id");

                entity.Property(e => e.Weight).HasColumnName("weight");

                entity.Property(e => e.WeightName)
                    .HasMaxLength(128)
                    .HasColumnName("weightName");

                entity.Property(e => e.ProdOilStatus).HasColumnName("ProdOilStatus");

                entity.Property(e => e.Apply).HasColumnName("Apply");
            });

            modelBuilder.Entity<Recipecalc3_gas>(entity =>
            {
                entity.ToTable("recipecalc3_gas");

                entity.Property(e => e.Id)
                    .ValueGeneratedNever()
                    .HasColumnName("id");

                entity.Property(e => e.ProdOilName)
                    .HasMaxLength(128)
                    .HasColumnName("prodOilName");

                entity.Property(e => e.ProdOilProduct).HasColumnName("prodOilProduct");

                entity.Property(e => e.TotalFlow).HasColumnName("totalFlow");

                entity.Property(e => e.TotalFlow2).HasColumnName("totalFlow2");
                
                entity.Property(e => e.Apply).HasColumnName("Apply");
            });

            modelBuilder.Entity<Schemeverify1_gas>(entity =>
            {
                entity.ToTable("schemeverify1_gas");

                entity.Property(e => e.Id)
                    .ValueGeneratedNever()
                    .HasColumnName("id");

                entity.Property(e => e.gas92QualityProduct).HasColumnName("gas92QualityProduct");

                entity.Property(e => e.gas95QualityProduct).HasColumnName("gas95QualityProduct");

                entity.Property(e => e.gas98QualityProduct).HasColumnName("gas98QualityProduct");

                entity.Property(e => e.gasSelfQualityProduct).HasColumnName("gasSelfQualityProduct");

                entity.Property(e => e.gas92FlowPercentMass).HasColumnName("gas92FlowPercentMass");

                entity.Property(e => e.gas95FlowPercentMass).HasColumnName("gas95FlowPercentMass");

                entity.Property(e => e.gas98FlowPercentMass).HasColumnName("gas98FlowPercentMass");

                entity.Property(e => e.gasSelfFlowPercentMass).HasColumnName("gasSelfFlowPercentMass");

                entity.Property(e => e.gas92FlowMass).HasColumnName("gas92FlowMass");

                entity.Property(e => e.gas95FlowMass).HasColumnName("gas95FlowMass");

                entity.Property(e => e.gas98FlowMass).HasColumnName("gas98FlowMass");

                entity.Property(e => e.gasSelfFlowMass).HasColumnName("gasSelfFlowMass");

                entity.Property(e => e.gas92VolumeProduct).HasColumnName("gas92VolumeProduct");

                entity.Property(e => e.gas95VolumeProduct).HasColumnName("gas95VolumeProduct");

                entity.Property(e => e.gas98VolumeProduct).HasColumnName("gas98VolumeProduct");

                entity.Property(e => e.gasSelfVolumeProduct).HasColumnName("gasSelfVolumeProduct");

                entity.Property(e => e.gas92FlowPercentVol).HasColumnName("gas92FlowPercentVol");

                entity.Property(e => e.gas95FlowPercentVol).HasColumnName("gas95FlowPercentVol");

                entity.Property(e => e.gas98FlowPercentVol).HasColumnName("gas98FlowPercentVol");

                entity.Property(e => e.gasSelfFlowPercentVol).HasColumnName("gasSelfFlowPercentVol");

                entity.Property(e => e.gas92FlowVol).HasColumnName("gas92FlowVol");

                entity.Property(e => e.gas95FlowVol).HasColumnName("gas95FlowVol");

                entity.Property(e => e.gas98FlowVol).HasColumnName("gas98FlowVol");

                entity.Property(e => e.gasSelfFlowVol).HasColumnName("gasSelfFlowVol");

                entity.Property(e => e.ComOilName)
                    .HasMaxLength(128)
                    .HasColumnName("comOilName");

            });

            modelBuilder.Entity<Schemeverify2_gas>(entity =>
            {
                entity.ToTable("schemeverify2_gas");

                entity.Property(e => e.Id)
                    .ValueGeneratedNever()
                    .HasColumnName("id");

                entity.Property(e => e.BottomVolume).HasColumnName("bottomVolume");

                entity.Property(e => e.ronVol).HasColumnName("ronVol");

                entity.Property(e => e.t50Vol).HasColumnName("t50Vol");

                entity.Property(e => e.denVol).HasColumnName("denVol");

                entity.Property(e => e.sufVol).HasColumnName("sufVol");

                entity.Property(e => e.TotalBlendVol).HasColumnName("totalblendVol");

                entity.Property(e => e.TotalBlendVol2).HasColumnName("totalblendVol2");

                entity.Property(e => e.TotalBlendVol3).HasColumnName("totalblendVol3");

                entity.Property(e => e.BottomMass).HasColumnName("bottomMass");

                entity.Property(e => e.ronMass).HasColumnName("ronMass");

                entity.Property(e => e.t50Mass).HasColumnName("t50Mass");

                entity.Property(e => e.denMass).HasColumnName("denMass");

                entity.Property(e => e.sufMass).HasColumnName("sufMass");

                entity.Property(e => e.TotalBlendMass).HasColumnName("totalblendMass");

                entity.Property(e => e.TotalBlendMass2).HasColumnName("totalblendMass2");

                entity.Property(e => e.TotalBlendMass3).HasColumnName("totalblendMass3");


                entity.Property(e => e.ProdOilName)
                    .HasMaxLength(128)
                    .HasColumnName("prodOilName");
            });

            modelBuilder.Entity<Dispatchweight_gas>(entity =>
            {
                entity.ToTable("dispatchweight_gas");

                entity.Property(e => e.Id)
                    .ValueGeneratedNever()
                    .HasColumnName("id");

                entity.Property(e => e.Weight).HasColumnName("weight");

                entity.Property(e => e.WeightName)
                    .HasMaxLength(128)
                    .HasColumnName("weightName");
            });

            #endregion 汽油

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
