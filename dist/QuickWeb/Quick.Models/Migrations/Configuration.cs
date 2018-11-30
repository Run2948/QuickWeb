using System;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;

namespace $safeprojectname$.Migrations
{
    /// <summary>
    /// 数据上下文配置
    /// </summary>
    internal sealed class Configuration : DbMigrationsConfiguration<$safeprojectname$.Application.DataContext>
    {
        public Configuration()
        {
            //开启自动迁移
            AutomaticMigrationsEnabled = true;
            AutomaticMigrationDataLossAllowed = true;
        }

        /// <summary>
        /// 种子数据
        /// </summary>
        /// <param name="context"></param>
        protected override void Seed($safeprojectname$.Application.DataContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data.
        }
    }
}
