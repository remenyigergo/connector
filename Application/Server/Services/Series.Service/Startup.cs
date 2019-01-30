using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Standard.Core.DataManager.MongoDB.DbModels;
using Standard.Core.DataManager.MongoDB.IRepository;
using Standard.Core.DataManager.MongoDB.Repository;
using Standard.Core.Dependency;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Series.DataManagement.MongoDB;
using Series.DataManagement.MongoDB.Repositories;
using Standard.Core.DataManager.Mongo.Repository;

namespace Series.Service
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            services.Configure<MongoDbSettings>(o => { o.IConfigurationRoot = Configuration; });
            services.AddTransient<IUserRepository, UserRepository>();
            services.AddTransient<IFeedRepository, FeedRepository>();
            services.AddTransient<IChatRepository, ChatRepository>();
            services.AddTransient<ISeriesRepository, SeriesRepository>();
            

            ServiceDependency.Current = services.BuildServiceProvider();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
//            if (env.IsDevelopment())
//            {
//                app.UseDeveloperExceptionPage();
//            }

            app.UseMvc();
        }
    }
}
