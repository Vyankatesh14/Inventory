using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unioteq.TrackNTrace.Models.Entity;



namespace Unioteq.TrackNTrace.Models
{
    public class ApplicationDBContext  :DbContext
    {

        public ApplicationDBContext(DbContextOptions<ApplicationDBContext> options) : base(options)
        {

        }
        public DbSet<UserEntity> Users { get; set; }
        public DbSet<PlantEntity>  plants { get; set; }
        public DbSet<ShopFloorEntity> shopFloors { get; set; }
        public DbSet<LineEntity> Lines { get; set; }
        public DbSet<MachineEntity> Machines { get; set; }
        public DbSet<CompanyEntity> Companies { get; set; }
        public DbSet<DeviceMasterEntity> Devicemaster { get; set; }
        public DbSet<RoleEntity> Role { get; set; }
         public DbSet<CustomerEntity> Customer { get; set; }

        public DbSet<StationEntity> Stations { get; set; } 

        public DbSet<MachinePartEntity> MachineParts { get; set; }

        public DbSet<MachineCheckPointEntity> MachineCheckPoints { get; set; }

        public DbSet<DepartmentEntity> Departments { get; set; }

        public DbSet<UserShopFloorEntity> UserShopFloors { get; set; }

        public DbSet<ModelEntity> Models { get; set; }

        public DbSet<ModelPartsEntity> ModelParts { get; set; }

        public DbSet<OperatorSkillEntity> OperatorSkills { get; set; }

        public DbSet<OperationsEntity> Operations { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PlantEntity>().ToTable("Table_Plant");

            modelBuilder.Entity<LineEntity>().ToTable("Table_Line");

            modelBuilder.Entity<MachineEntity>().ToTable("Table_Machine");

            modelBuilder.Entity<CompanyEntity>().ToTable("Table_Company");

            modelBuilder.Entity<RoleEntity>().ToTable("Table_Role");

            modelBuilder.Entity<MachinePartEntity>().ToTable("Table_MachinePart");

            modelBuilder.Entity<DepartmentEntity>().ToTable("Table_Department");

            modelBuilder.Entity<UserEntity>().ToTable("Table_User");

            modelBuilder.Entity<ModelPartsEntity>().ToTable("Table_ModelParts");

            modelBuilder.Entity<StationEntity>().ToTable("Table_Station");

            modelBuilder.Entity<ShopFloorEntity>().ToTable("Table_ShopFloor");

            modelBuilder.Entity<ModelEntity>().ToTable("Table_Model");

            modelBuilder.Entity<OperationsEntity>().ToTable("Table_Operation");

        }
    }
}
