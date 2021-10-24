﻿using System.Net.Http;
using Domain.Clients.Firebase;
using Domain.Services;
using Microsoft.Extensions.DependencyInjection;
using Persistence.Repositories;


namespace Domain
{
    public static class ServiceExtensions
    {
        public static IServiceCollection AddDomain(this IServiceCollection services)
        {
            return services
                .AddClients()
                .AddServices();
        }

        private static IServiceCollection AddClients(this IServiceCollection services)
        {
           
            services.AddHttpClient<IFirebaseClient, FirebaseClient>();
          
            
            return services;
        }
        
        private static IServiceCollection AddServices(this IServiceCollection services)
        {
            services.AddSingleton<IAuthService, AuthService>();
            services.AddSingleton<IAccountService, AccountService>();

            return services;
        }
    }
}