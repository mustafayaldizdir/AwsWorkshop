
using Serilog;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using System.Reflection;
using Serilog.Sinks.Elasticsearch;
using Microsoft.Extensions.Configuration;

namespace AwsWorkshop.Product.Api.Core.LogConfigurations
{
    public static class SerilogConfiguationHelper
    {
        public static IServiceCollection AddSerilogCustomizationService(this IServiceCollection services, WebApplicationBuilder builder)
        {

            Serilog.Core.Logger log = new LoggerConfiguration().WriteTo.Console()
                  //.WriteTo.Elasticsearch(new ElasticsearchSinkOptions(new Uri(builder.Configuration.GetValue<string>("LogSettings:Uri")))
                  //{
                  //    AutoRegisterTemplate = true,
                  //    AutoRegisterTemplateVersion = AutoRegisterTemplateVersion.ESv7,
                  //    IndexFormat = $"aws-service-logs-{DateTime.UtcNow:yyyy.MM}.01",
                  //    ModifyConnectionSettings = x =>
                  //    {
                  //        x.BasicAuthentication(builder.Configuration.GetValue<string>("LogSettings:UserName"), builder.Configuration.GetValue<string>("LogSettings:Password"));
                  //        x.ServerCertificateValidationCallback((o, certificate, arg3, arg4) => true);
                  //        return x;
                  //    }
                  //})
                 .Enrich.FromLogContext().Enrich.WithCorrelationIdHeader("aws-logs-correlation-id")
                 .MinimumLevel.Information()
                 .CreateLogger();
            Serilog.Debugging.SelfLog.Enable(Console.Error);
            Serilog.Debugging.SelfLog.Enable(msg => log.Error($"Serilog-SelfLog:{msg}"));
            builder.Host.UseSerilog(log);

            return services;
        }
    }
}
