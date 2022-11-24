using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace OilBlendSystem.Models.DataBaseModel
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

        public virtual DbSet<Compoilconfig> Compoilconfigs { get; set; } = null!;
        public virtual DbSet<Menulist> Menulists { get; set; } = null!;
        public virtual DbSet<Prodoilconfig> Prodoilconfigs { get; set; } = null!;
        public virtual DbSet<Property> Properties { get; set; } = null!;
        public virtual DbSet<Recipecalc1> Recipecalc1s { get; set; } = null!;
        public virtual DbSet<Recipecalc2> Recipecalc2s { get; set; } = null!;
        public virtual DbSet<Recipecalc3> Recipecalc3s { get; set; } = null!;
        public virtual DbSet<Schemeverify1> Schemeverify1s { get; set; } = null!;
        public virtual DbSet<Schemeverify2> Schemeverify2s { get; set; } = null!;
        public virtual DbSet<Dispatchweight> Dispatchweights { get; set; } = null!;

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

                entity.Property(e => e.Prod1High1).HasColumnName("prod1High2");

                entity.Property(e => e.Prod2High1).HasColumnName("prod2High2");

                entity.Property(e => e.Prod1Low1).HasColumnName("prod1Low2");

                entity.Property(e => e.Prod2Low1).HasColumnName("prod2Low2");

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

                entity.Property(e => e.ExpQualityProduct).HasColumnName("prod1QualityProduct");

                entity.Property(e => e.ExpQualityProduct).HasColumnName("prod2QualityProduct");

                entity.Property(e => e.AutoFlowPercentMass).HasColumnName("autoFlowPercentMass");

                entity.Property(e => e.ExpFlowPercentMass).HasColumnName("expFlowPercentMass");

                entity.Property(e => e.ExpFlowPercentMass).HasColumnName("prod1FlowPercentMass");

                entity.Property(e => e.ExpFlowPercentMass).HasColumnName("prod2FlowPercentMass");

                entity.Property(e => e.AutoFlowMass).HasColumnName("autoFlowMass");

                entity.Property(e => e.ExpFlowMass).HasColumnName("expFlowMass");

                entity.Property(e => e.ExpFlowMass).HasColumnName("prod1FlowMass");

                entity.Property(e => e.ExpFlowMass).HasColumnName("prod2FlowMass");

                entity.Property(e => e.AutoVolumeProduct).HasColumnName("autoVolumeProduct");

                entity.Property(e => e.ExpVolumeProduct).HasColumnName("expVolumeProduct");

                entity.Property(e => e.ExpVolumeProduct).HasColumnName("prod1VolumeProduct");

                entity.Property(e => e.ExpVolumeProduct).HasColumnName("prod2VolumeProduct");

                entity.Property(e => e.AutoFlowPercentVol).HasColumnName("autoFlowPercentVol");

                entity.Property(e => e.ExpFlowPercentVol).HasColumnName("expFlowPercentVol");

                entity.Property(e => e.ExpFlowPercentVol).HasColumnName("prod1FlowPercentVol");

                entity.Property(e => e.ExpFlowPercentVol).HasColumnName("prod2FlowPercentVol");

                entity.Property(e => e.AutoFlowVol).HasColumnName("autoFlowVol");

                entity.Property(e => e.ExpFlowVol).HasColumnName("expFlowVol");

                entity.Property(e => e.ExpFlowVol).HasColumnName("prod1FlowVol");

                entity.Property(e => e.ExpFlowVol).HasColumnName("prod2FlowVol");

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


            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
