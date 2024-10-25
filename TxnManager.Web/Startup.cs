using System;
using AutoMapper;
using Hellang.Middleware.ProblemDetails;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using TxnManager.Data.Abstractions;
using TxnManager.Data.EntityFramework;
using TxnManager.Domain.Model;
using TxnManager.Domain.Service.Abstractions;
using TxnManager.Domain.Service.Implementations;
using TxnManager.Infrastructure;
using TxnManager.Infrastructure.Csv;
using TxnManager.Infrastructure.Xml;
using TxnManager.Web.ProblemDetails;

namespace TxnManager.Web
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            // Transactions Manager DI
            var connectionString = Configuration.GetConnectionString("TransactionsManagerConnectionString");
            services.AddDbContext<TransactionsManagerDbContext>(options =>
                options.UseSqlServer(connectionString)
            );

            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            services.AddLogging(builder => { builder.AddConsole(); });

            services.AddProblemDetails(options =>
            {
                options.Map<FileParseException>(ex => new FileParseProblemDetails(ex));
                options.Map<BusinessRuleValidationException>(ex => new BusinessRuleValidationProblemDetails(ex));
            });

            services.AddScoped<IXMlTransactionFileValidator, XmlTransactionFileValidator>();
            services.AddScoped<IFileParser, XmlFileParser>();
            services.AddScoped<IFileParser, CsvFileParser>();
            services.AddScoped<IUploadService, UploadService>();
            services.AddScoped<ITransactionsService, TransactionsService>();
            services.AddScoped<IFileParseStrategy, FileParseStrategy>();

            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            services.AddScoped<ITransactionsRepository, TransactionsRepository>();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler(builder =>
                    builder.Run(async context =>
                        {
                            context.Response.StatusCode = 500;
                            await context.Response.WriteAsync("An unexpected fault happened. Try again later.");
                        }
                    ));
            }

            app.UseStaticFiles();

            app.UseProblemDetails();
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=UploadTransactionFile}/{action=Upload}");
            });
        }
    }
}
