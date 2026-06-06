using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PrescriberSystem.Core;
using PrescriberSystem.Hosting;

var host = Host.CreateApplicationBuilder(args);

// 註冊診斷服務（in-memory queue，單例）
host.Services.AddSingleton(_ => new PrescriberFacade("patients.json", "diseases.txt"));

// 以 BackgroundService 形態託管，交由 Host 管理生命週期與關閉訊號（SIGTERM）
host.Services.AddHostedService<PrescriberHostedService>();

await host.Build().RunAsync();
