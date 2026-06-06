using Microsoft.Extensions.Hosting;
using PrescriberSystem.Core;
using PrescriberSystem.Domain;

namespace PrescriberSystem.Hosting;

public class PrescriberHostedService : BackgroundService
{
    private readonly PrescriberFacade _facade;

    public PrescriberHostedService(PrescriberFacade facade)
    {
        _facade = facade;
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _facade.Prescribe("A123456789", [Symptom.Sneeze, Symptom.Headache, Symptom.Cough], "result_covid.json", Format.Json);
        _facade.Prescribe("B987654321", [Symptom.Sneeze], "result_attractive.csv", Format.Csv);
        _facade.Prescribe("C111222333", [Symptom.Snore], "result_sleep.json", Format.Json);

        Console.WriteLine("[Host] 診斷要求已送出，服務運行中……（等待 SIGTERM 觸發關閉）");
        return Task.CompletedTask;
    }

    public override async Task StopAsync(CancellationToken cancellationToken)
    {
        Console.WriteLine("[Host] 收到關閉訊號，開始優雅關閉……");
        _facade.Shutdown();
        await base.StopAsync(cancellationToken);
    }
}
