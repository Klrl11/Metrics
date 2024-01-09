using AutoMapper;
using FluentMigrator.Runner;
using MetricsManager.Mappings;
using MetricsManager.Migrations;
using MetricsManager.Models.Options;
using MetricsManager.Services.Impl;
using MetricsManager.Services.Impl.Clients;
using Microsoft.Extensions.DependencyInjection;
using Polly;
using System.Diagnostics;

namespace MetricsManager
{
    public class Program
    {

        /// <summary>
        /// TODO:
        ///  @"CREATE TABLE AgentsPool(
        ///  id INTEGER PRIMARY KEY,
        ///  address STR)";
        /// 
        /// </summary>
        /// <param name="args"></param>
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(configure =>
            {
                configure.EnableAnnotations();
            });

            builder.Services.AddScoped<IAgentsRepository, AgentsRepository>();

            #region Конфигурирование AutoMapper

            var mapperConfiguration = new MapperConfiguration(configuration =>
            {
                configuration.AddProfile(new MapperProfile());
            });
            builder.Services.AddSingleton(mapperConfiguration.CreateMapper());

            #endregion

            #region Конфигурирование опций

            //builder.Services.Configure<ServiceSettingsAgents>(configure =>
            //{
            //    builder.Configuration.GetSection("ServiceSettingsAgents").Bind(configure);
            //});

            builder.Services.Configure<ConnectionStrings>(configure =>
            {
                configure.Default = builder.Configuration.GetConnectionString("ManagerDb");
            });

            #endregion

            #region Конфигурирование FluentMigrator

            builder.Services.AddFluentMigratorCore()
                .ConfigureRunner(migrationBuilder =>
                {
                    migrationBuilder
                        // Добавление поддержки БД SQLite
                        .AddSQLite()
                        // Установка строки соединения с БД
                        .WithGlobalConnectionString(builder.Configuration.GetConnectionString("ManagerDb"))
                        // Показываем, где искать классы с миграциями
                        .ScanIn(typeof(_0_InitialMigration).Assembly)
                        .For.Migrations();
                }).AddLogging(loggingBuilder =>
                {
                    loggingBuilder.AddFluentMigratorConsole();
                });

            #endregion

            #region Конфигурирование Http-клиентов

            builder.Services.AddHttpClient<IMetricsAgentDefaultClient, MetricsAgentDefaultClient>()
                .AddTransientHttpErrorPolicy(pol =>
                    pol.WaitAndRetryAsync(
                        retryCount: 5,
                        sleepDurationProvider: (attemptCount) => TimeSpan.FromSeconds(attemptCount * 2),
                        onRetry: (response, sleepDuration, attemptNumber, context) =>
                        {
                            Debug.WriteLine(
                                $"{(response.Exception != null ? response.Exception.ToString() : response.Result.StatusCode)}\n attempt: {attemptNumber} - MetricsAgentDefaultClient Error");
                        })
                );

            #endregion

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            var serviceProvider = builder.Services.BuildServiceProvider();
            var migrationRunner = serviceProvider.GetRequiredService<IMigrationRunner>();
            migrationRunner.MigrateUp();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}