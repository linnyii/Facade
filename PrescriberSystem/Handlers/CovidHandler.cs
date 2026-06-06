using PrescriberSystem.Domain;
using PrescriberSystem.Dto;

namespace PrescriberSystem.Handlers;

public class CovidHandler : PrescriptionHandler
{
    protected override bool CanHandle(PrescriptionDemand demand, Patient patient) =>
        demand.Symptoms.Contains(Symptom.Sneeze) &&
        demand.Symptoms.Contains(Symptom.Headache) &&
        demand.Symptoms.Contains(Symptom.Cough);

    protected override Prescription CreatePrescription() =>
        new()
        {
            Name = "清冠一號",
            PotentialDisease = "COVID-19",
            Medicines = ["清冠一號"],
            Usage = "將相關藥材裝入茶包裡，使用500 mL 溫、熱水沖泡悶煮1~3 分鐘後即可飲用。"
        };
}
