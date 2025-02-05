using System.Collections.Concurrent;

namespace ToDo_GPT.Logging;

public class CustomerLogger : ILogger
{
    private readonly string _loggerName;
    private readonly CustomLoggerProviderConfiguration _loggerConfig;

    private readonly ConcurrentDictionary<string, CustomerLogger> _loggers =
                                  new ConcurrentDictionary<string, CustomerLogger>();

    public CustomerLogger(string name, CustomLoggerProviderConfiguration config)
    {
        _loggerName = name;
        _loggerConfig = config;
    }

    public bool IsEnabled(LogLevel logLevel)
    {
        return logLevel >= _loggerConfig.LogLevel;
    }

    public IDisposable? BeginScope<TState>(TState state) where TState : notnull
    {
        return null;
    }

    public void Log<TState>(LogLevel logLevel, EventId eventId, TState state,
        Exception? exception, Func<TState, Exception?, string> formatter)
    {
        if (!IsEnabled(logLevel))
        {
            return;
        }

        string mensagem = $"{logLevel}: {eventId} - {DateTime.Now} - {formatter(state, exception)}";
        EscreverTextoNoArquivo(mensagem);
    }

    public ILogger CreateLogger(string categoryName)
    {
        return _loggers.GetOrAdd(categoryName, name => new CustomerLogger(name, _loggerConfig));
    }

    public void Dispose()
    {
        _loggers.Clear();
    }

    private void EscreverTextoNoArquivo(string mensagem)
    {
        string caminhoArquivoLog = @"D:\Camilo\estudos_C#\ToDoLogs\API_Logs.txt";
        string diretorio = Path.GetDirectoryName(caminhoArquivoLog)!;

        if (!Directory.Exists(diretorio))
        {
            Directory.CreateDirectory(diretorio);
        }

        using (StreamWriter streamWriter = new StreamWriter(caminhoArquivoLog, true))
        {
            try
            {
                streamWriter.WriteLine(mensagem);
            }
            catch (Exception ex)
            {
                throw new IOException("Erro ao escrever no arquivo de log", ex);
            }
        }
    }
}