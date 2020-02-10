using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Standard.Core.DataManager.MongoDB.DbModels;
using Standard.Core.DataManager.MongoDB.IRepository;
using Standard.Core.DataManager.MongoDB.Repository;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Movie.DataManagement.MongoDB.Repositories;
using Server.DataManagement.SQL.Repositories;
using Standard.Core.Dependency;

namespace Server
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;

            StartDb();
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            //            services.Configure<Settings>(o => { o.IConfigurationRoot = Configuration; } );
            //            services.AddTransient<IUserRepository, UserRepository>();
            //            services.AddTransient<IFeedRepository, FeedRepository>();
            //            services.AddTransient<IChatRepository, ChatRepository>();
            services.AddTransient<IMonitorRepository, MonitorRepository>();
            ServiceDependency.Current = services.BuildServiceProvider();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMvc();
        }

        private void StartDb()
        {

            //string connectionString;
            //SqlConnection cnn;
            //connectionString = @"Data Source=localhost\SQLEXPRESS;Initial Catalog=connector;User ID=DESKTOP-9J0UP14\Greg;Password=";
            //cnn = new SqlConnection(connectionString);
            //cnn.Open();

        }
    }
}
