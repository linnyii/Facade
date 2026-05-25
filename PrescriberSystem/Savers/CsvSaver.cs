using PrescriberSystem.Domain;

namespace PrescriberSystem.Savers;

public class CsvSaver : IPrescriptionResultSaver
{
    public void Save(Prescription prescription, PrescriptionDemand demand)
    {
        var lines = new List<string>
        {
            "patientId,diagnosedAt,prescriptionName,potentialDisease,medicines,usage",
            $"{demand.PatientId},{DateTime.Now:yyyy-MM-dd HH:mm:ss},{prescription.Name},{prescription.PotentialDisease},{string.Join("|", prescription.Medicines)},{prescription.Usage}"
        };

        File.WriteAllLines(demand.OutputFile, lines);
        Console.WriteLine($"[CsvSaver] 診斷結果已儲存至 {demand.OutputFile}");
    }
}
