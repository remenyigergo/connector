﻿using Microsoft.Extensions.DependencyInjection;
using Series.DataManagement.MongoDB.Repositories;
using Standard.Core.DataManager.MongoDB.IRepository;
using Standard.Core.DataManager.MongoDB.Repository;

namespace Series.Service.ServiceConfigurationExtensions
{
    public static class ConfigureRepositoriesExtension
    {
        public static void ConfigureRepositories(this IServiceCollection services)
        {
            services.AddTransient<IUserRepository, UserRepository>();
            services.AddTransient<IFeedRepository, FeedRepository>();
            services.AddTransient<IChatRepository, ChatRepository>();
            services.AddTransient<ISeriesRepository, SeriesRepository>();
        }
    }
}
