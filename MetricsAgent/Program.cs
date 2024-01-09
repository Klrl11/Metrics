using MetricsAgent.Models;
using MetricsAgent.Models.Options;
using MetricsAgent.Services;
using MetricsAgent.Services.Impl;
using Microsoft.Data.Sqlite;
using FluentMigrator.Runner;
using FluentMigrator.Runner.Initialization;
using Microsoft.Extensions.DependencyInjection;
using MetricsAgent.Migrations;
using AutoMapper;
using MetricsAgent.Mappings;
using MetricsAgent.Jobs;
using Quartz.Spi;
using Quartz;
using Quartz.Impl;

namespace MetricsAgent
{
    public class Program
    {
        public static void Main(string[] args)
        {

            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(configure =>
            {
                configure.EnableAnnotations();
            });
            builder.Services.AddScoped<ICpuMetricsRepository, CpuMetricsRepository>();
            builder.Services.AddScoped<INetworkMetricsRepository, NetworkMetricsRepository>();
            builder.Services.AddScoped<IHddMetricsRepository, HddMetricsRepository>();
            builder.Services.AddScoped<IRamMetricsRepository, RamMetricsRepository>();

            #region ���������������� Quartz

            // ����������� ��������-���������� ����������� �����
            JobSchedule cpuMetricJobSchedule = new JobSchedule(typeof(CpuMetricJob), "0/10 * * ? * * *");
            JobSchedule networkMetricJobSchedule = new JobSchedule(typeof(NetworkMetricJob), "0/10 * * ? * * *");
            JobSchedule hddMetricJobSchedule = new JobSchedule(typeof(HddMetricJob), "0/10 * * ? * * *");
            JobSchedule ramMetricJobSchedule = new JobSchedule(typeof(RamMetricJob), "0/10 * * ? * * *");
             
            builder.Services.AddSingleton(cpuMetricJobSchedule);
            builder.Services.AddSingleton(networkMetricJobSchedule);
            builder.Services.AddSingleton(hddMetricJobSchedule);
            builder.Services.AddSingleton(ramMetricJobSchedule);


            // ����������� ���� ������
            builder.Services.AddSingleton<CpuMetricJob>();
            builder.Services.AddSingleton<NetworkMetricJob>();
            builder.Services.AddSingleton<HddMetricJob>();
            builder.Services.AddSingleton<RamMetricJob>();

            // ����������� �������� ���������� ������� Quartz
            builder.Services.AddSingleton<ISchedulerFactory, StdSchedulerFactory>();
            // ����������� ������� �� �������� ������
            builder.Services.AddSingleton<IJobFactory, SingletonJobFactory>();


            builder.Services.AddHostedService<QuartzHostedService>();

            #endregion

            #region ���������������� AutoMapper

            var mapperConfiguration = new MapperConfiguration(configuration =>
            {
                configuration.AddProfile(new MapperProfile());
            });
            builder.Services.AddSingleton(mapperConfiguration.CreateMapper());

            #endregion

            #region ���������������� �����

            builder.Services.Configure<ServiceSettings>(configure =>
            {
                builder.Configuration.GetSection("ServiceSettings").Bind(configure);
            });

            builder.Services.Configure<ConnectionStrings>(configure =>
            {
                configure.Default = builder.Configuration.GetConnectionString("MetricsDb");
            });

            #endregion

            #region ���������������� FluentMigrator

            builder.Services.AddFluentMigratorCore()
                .ConfigureRunner(migrationBuilder =>
                {
                    migrationBuilder
                        // ���������� ��������� �� SQLite
                        .AddSQLite()
                        // ��������� ������ ���������� � ��
                        .WithGlobalConnectionString(builder.Configuration.GetConnectionString("MetricsDb"))
                        // ����������, ��� ������ ������ � ����������
                        .ScanIn(typeof(Program).Assembly)
                        .ScanIn(typeof(_0_InitialMigration).Assembly)
                        .For.Migrations();
                }).AddLogging(loggingBuilder =>
                {
                    loggingBuilder.AddFluentMigratorConsole();
                });

            #endregion

            var app = builder.Build();

            var serviceProvider = builder.Services.BuildServiceProvider();
            var migrationRunner = serviceProvider.GetRequiredService<IMigrationRunner>();
            migrationRunner.MigrateUp();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}