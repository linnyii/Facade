using Microsoft.Extensions.Hosting;
using PrescriberSystem.Core;
using PrescriberSystem.Domain;

namespace PrescriberSystem.Hosting;

/// <summary>
/// 以 BackgroundService 包裝診斷服務，讓 .NET Generic Host 管理其生命週期。
/// Host 收到 SIGTERM（K8s 終止 Pod 時送出）會觸發 StopAsync，
/// 進而走 Prescriber 的排空式關閉，把排隊中的診斷處理完才結束。
/// </summary>
public class PrescriberHostedService : BackgroundService
{
    private readonly PrescriberFacade _facade;

    public PrescriberHostedService(PrescriberFacade facade)
    {
        _facade = facade;
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        // 範例：開機後送出幾筆診斷要求（實務上這裡會改成從 API / 訊息佇列接收）
        _facade.Prescribe("A123456789", [Symptom.Sneeze, Symptom.Headache, Symptom.Cough], "result_covid.json", Format.Json);
        _facade.Prescribe("B987654321", [Symptom.Sneeze], "result_attractive.csv", Format.Csv);
        _facade.Prescribe("C111222333", [Symptom.Snore], "result_sleep.json", Format.Json);

        Console.WriteLine("[Host] 診斷要求已送出，服務運行中……（等待 SIGTERM 觸發關閉）");
        return Task.CompletedTask;
    }

    public override async Task StopAsync(CancellationToken cancellationToken)
    {
        // 收到關閉訊號（SIGTERM）→ 排空式關閉，把排隊中的診斷處理完。
        Console.WriteLine("[Host] 收到關閉訊號，開始優雅關閉……");
        _facade.Shutdown();
        await base.StopAsync(cancellationToken);
    }
}
