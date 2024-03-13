using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.CrossCuttingConcerns.Serilog.ConfigurationModels;
using Core.CrossCuttingConcerns.Serilog.Messages;
using Microsoft.Extensions.Configuration;
using Serilog;
using Serilog.Sinks.MSSqlServer;

namespace Core.CrossCuttingConcerns.Serilog.Loggers
{
    public class MsSqlLogger : LoggerServiceBase
    {
        private readonly IConfiguration _configuration;
        public MsSqlLogger(IConfiguration configuration)
        {
            _configuration = configuration;

             MsSqlConfiguration logConfig = configuration.GetSection("SerilogLogConfigurations:MsSqlConfiguration").Get<MsSqlConfiguration>() ?? throw new Exception(SerilogMessages.NullOptionsMessage);

            MSSqlServerSinkOptions sinkOptions = new MSSqlServerSinkOptions()
            {
                TableName = logConfig.TableName,
                AutoCreateSqlDatabase = logConfig.AutoCreateSqlTable,
            };

            ColumnOptions columnOptions = new();

            global::Serilog.Core.Logger seriLogConfig = new LoggerConfiguration().WriteTo
                .MSSqlServer(connectionString: logConfig.ConnectionString, sinkOptions: sinkOptions,
                    columnOptions: columnOptions).CreateLogger();

            Logger = seriLogConfig;
        }
    }
}
