using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Series.DataManagement.MongoDB.Repositories;
using Standard.Core.DataManager.Mongo.Repository;
using Standard.Core.DataManager.MongoDB.DbModels;
using Standard.Core.DataManager.MongoDB.IRepository;
using Standard.Core.DataManager.MongoDB.Repository;
using Standard.Core.Dependency;

namespace Series.Service
{
    public class Startup
    {
        public Startup(IConfigurationRoot configuration)
        {
            Configuration = configuration;
        }

        public IConfigurationRoot Configuration { get; set; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
            //services.Configure<MongoDbSettings>(o => { o.IConfigurationRoot = Configuration; });

            services.AddTransient<IUserRepository, UserRepository>();
            services.AddTransient<IFeedRepository, FeedRepository>();
            services.AddTransient<IChatRepository, ChatRepository>();
            services.AddTransient<ISeriesRepository, SeriesRepository>();

            SetConfiguration();

            ServiceDependency.Current = services.BuildServiceProvider();
        }

        private void SetConfiguration()
        {
            Configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseMvc();
        }
    }
}