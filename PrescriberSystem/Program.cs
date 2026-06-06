using PrescriberSystem.Core;
using PrescriberSystem.Domain;
using PrescriberSystem.Savers;

using var facade = new PrescriberFacade("patients.json", "diseases.txt");

facade.Prescribe("A123456789", [Symptom.Sneeze, Symptom.Headache, Symptom.Cough], "result_covid.json", new JsonSaver());
facade.Prescribe("B987654321", [Symptom.Sneeze], "result_attractive.csv", new CsvSaver());
facade.Prescribe("C111222333", [Symptom.Snore], "result_sleep.json", new JsonSaver());
