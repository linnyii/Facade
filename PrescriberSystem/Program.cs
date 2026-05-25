using PrescriberSystem.Core;
using PrescriberSystem.Domain;

var facade = new PrescriberFacade("patients.json", "diseases.txt");

facade.Prescribe("A123456789", [Symptom.Sneeze, Symptom.Headache, Symptom.Cough], "result_covid.json", Format.Json);

facade.Prescribe("B987654321", [Symptom.Sneeze], "result_attractive.csv", Format.Csv);

facade.Prescribe("C111222333", [Symptom.Snore], "result_sleep.json", Format.Json);

Console.WriteLine("等待診斷完成中...");
Thread.Sleep(12000);
Console.WriteLine("所有診斷已完成。");
