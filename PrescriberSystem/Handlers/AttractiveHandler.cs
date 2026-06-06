using PrescriberSystem.Domain;
using PrescriberSystem.Dto;

namespace PrescriberSystem.Handlers;

public class AttractiveHandler : PrescriptionHandler
{
    protected override bool CanHandle(PrescriptionDemand demand, Patient patient) =>
        patient is { Age: 18, Gender: Gender.Female } &&
        demand.Symptoms.Contains(Symptom.Sneeze);

    protected override Prescription CreatePrescription() =>
        new()
        {
            Name = "青春抑制劑",
            PotentialDisease = "Attractive",
            Medicines = ["假鬢角", "臭味"],
            Usage = "把假鬢角黏在臉的兩側，讓自己異性緣差一點，自然就不會有人想妳了。"
        };
}
