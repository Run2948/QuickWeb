using System;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;

namespace $safeprojectname$.Migrations
{
    /// <summary>
    /// ��������������
    /// </summary>
    internal sealed class Configuration : DbMigrationsConfiguration<$safeprojectname$.Application.DataContext>
    {
        public Configuration()
        {
            //�����Զ�Ǩ��
            AutomaticMigrationsEnabled = true;
            AutomaticMigrationDataLossAllowed = true;
        }

        /// <summary>
        /// ��������
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
