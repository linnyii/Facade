using PrescriberSystem.Domain;

namespace PrescriberSystem.Handlers;

public class SleepApneaSyndromeHandler : PrescriptionHandler
{
    protected override bool CanHandle(PrescriptionDemand demand, Patient patient) =>
        patient.Bmi > 26 &&
        demand.Symptoms.Contains(Symptom.Snore);

    protected override Prescription CreatePrescription(PrescriptionDemand demand, Patient patient) =>
        new Prescription
        {
            Name = "打呼抑制劑",
            PotentialDisease = "SleepApneaSyndrome",
            Medicines = new List<string> { "一捲膠帶" },
            Usage = "睡覺時，撕下兩塊膠帶，將兩塊膠帶交錯黏在關閉的嘴巴上，就不會打呼了。"
        };
}
