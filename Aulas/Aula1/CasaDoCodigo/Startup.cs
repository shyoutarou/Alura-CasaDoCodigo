﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CasaDoCodigo
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // Adiciona Serviços como SQL Server ou Serviço de Log
        public void ConfigureServices(IServiceCollection services)
        {
            // Adiciona MVC
            services.AddMvc();

            // Seta ConnectionString e adiciona DbContext do EFCore para usar o Banco de Dados
            string connectionString = Configuration.GetConnectionString("Default");

            services.AddDbContext<ApplicationContext>(options =>
                options.UseSqlServer(connectionString)
            );
        }

        // Configuração de Pipeline - Serve para consumir os serviços
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IServiceProvider serviceProvider)
        {
            if (env.IsDevelopment())
            {
                app.UseBrowserLink();
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Pedido}/{action=Carrossel}/{id?}");
            });

            // Cria automaticamente o Banco de Dados na execução, caso ele não exista
            serviceProvider.GetService<ApplicationContext>().Database.Migrate();
        }
    }
}
