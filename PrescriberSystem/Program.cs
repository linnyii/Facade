using PrescriberSystem.Core;
using PrescriberSystem.Domain;

// 初始化 Facade（1 行）
var facade = new PrescriberFacade("patients.json", "diseases.txt");

// 診斷 1：John 有打噴嚏、頭痛、咳嗽 → COVID-19
facade.Prescribe("A123456789", new List<Symptom> { Symptom.Sneeze, Symptom.Headache, Symptom.Cough }, "result_covid.json", Format.Json);

// 診斷 2：Mary 18歲女性有打噴嚏 → Attractive
facade.Prescribe("B987654321", new List<Symptom> { Symptom.Sneeze }, "result_attractive.csv", Format.Csv);

// 診斷 3：Bob BMI > 26 且打呼 → SleepApneaSyndrome
facade.Prescribe("C111222333", new List<Symptom> { Symptom.Snore }, "result_sleep.json", Format.Json);

// 等待所有診斷完成（每筆 3 秒，共 3 筆）
Console.WriteLine("等待診斷完成中...");
Thread.Sleep(12000);
Console.WriteLine("所有診斷已完成。");
