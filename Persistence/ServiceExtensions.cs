﻿using System;
using Dapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Persistence.Repositories;

namespace Persistence
{
    public static class ServiceExtensions
    {
        public static IServiceCollection AddPersistence(this IServiceCollection services, IConfiguration configuration)
        {
            SqlMapper.AddTypeHandler(new MySqlGuidTypeHandler());
            SqlMapper.RemoveTypeMap(typeof(Guid));
            SqlMapper.RemoveTypeMap(typeof(Guid?));

            return services
                .AddSqlClient(configuration)
                .AddRepositories();
        }
        private static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            return services
                .AddSingleton<IAccountRepository, AccountRepository>()
                .AddSingleton<IUsersRepository, UsersRepository>()
                .AddSingleton<ITransactionRepository, TransactionRepository>();
        }
        private static IServiceCollection AddSqlClient(this IServiceCollection services, IConfiguration configuration)
        {

            var connectionString = configuration
                .GetSection("ConnectionStrings")
                .GetSection("SqlConnectionString").Value;

            return services.AddTransient<ISqlClient>(_ => new SqlClient(connectionString));
        }
    }
}