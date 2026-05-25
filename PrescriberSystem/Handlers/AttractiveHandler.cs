using PrescriberSystem.Domain;

namespace PrescriberSystem.Handlers;

public class AttractiveHandler : PrescriptionHandler
{
    protected override bool CanHandle(PrescriptionDemand demand, Patient patient) =>
        patient.Age == 18 &&
        patient.Gender == Gender.Female &&
        demand.Symptoms.Contains(Symptom.Sneeze);

    protected override Prescription CreatePrescription(PrescriptionDemand demand, Patient patient) =>
        new Prescription
        {
            Name = "青春抑制劑",
            PotentialDisease = "Attractive",
            Medicines = new List<string> { "假鬢角", "臭味" },
            Usage = "把假鬢角黏在臉的兩側，讓自己異性緣差一點，自然就不會有人想妳了。"
        };
}
