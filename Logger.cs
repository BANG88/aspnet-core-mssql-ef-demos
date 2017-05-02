using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.Logging;

public class SqlLoggerProvider : ILoggerProvider
    {
        public ILogger CreateLogger(string categoryName)
        {
            if (categoryName == typeof(Microsoft.EntityFrameworkCore.Storage.IRelationalCommandBuilderFactory).FullName)
            {
                return new SqlLogger(categoryName);
            }

            return new NullLogger();
        }

        public void Dispose()
        { }

        private class SqlLogger : ILogger
        {
            private string _test;

            public SqlLogger(string test)
            {
                _test = test;
            }

            public bool IsEnabled(LogLevel logLevel)
            {
                return true;
            }

            public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
            {
                if (eventId.Id == (int)RelationalEventId.ExecutedCommand)
                {
                    var data = state as IEnumerable<KeyValuePair<string, object>>;
                    if (data != null)
                    {
                        var commandText = data.Single(p => p.Key == "CommandText").Value;
                        Console.WriteLine(commandText);
                    }
                }
            }

            public IDisposable BeginScope<TState>(TState state)
            {
                return null;
            }

		
		IDisposable ILogger.BeginScope<TState>(TState state)
		{
			throw new NotImplementedException();
		}
	}

        private class NullLogger : ILogger
        {
            public bool IsEnabled(LogLevel logLevel)
            {
                return false;
            }

            public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
            { }

            public IDisposable BeginScope<TState>(TState state)
            {
                return null;
            }
        }
    }