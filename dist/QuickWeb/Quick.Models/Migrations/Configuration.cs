using System;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Data.SqlClient;
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
            try
            {
                #region ���Լ��

                context.Database.ExecuteSqlCommand(@"ALTER TABLE [dbo].[Post] ADD DEFAULT getdate() FOR [PostDate];
                                                    ALTER TABLE [dbo].[Post] ADD DEFAULT getdate() FOR [ModifyDate];
                                                    ALTER TABLE [dbo].[Post] ADD DEFAULT 0 FOR [IsFixedTop];
                                                    ALTER TABLE [dbo].[Post] ADD DEFAULT 0 FOR [IsBanner];
                                                    ALTER TABLE [dbo].[SystemSetting] ADD DEFAULT 1 FOR [IsAvailable];
                                                    ALTER TABLE [dbo].[UserInfo] ADD DEFAULT 0 FOR [IsAdmin];
                                                    ");

                #endregion
            }
            catch (SqlException)
            {

            }
        }
    }
}
