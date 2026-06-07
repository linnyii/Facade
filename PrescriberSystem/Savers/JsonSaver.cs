using System.Text.Json;
using PrescriberSystem.Domain;

namespace PrescriberSystem.Savers;

public class JsonSaver : IPrescriptionResultSaver
{
    public void Save(Prescription prescription, PrescriptionDemand demand)
    {
        var result = new
        {
            patientId = demand.PatientId,
            diagnosedAt = DateTime.Now,
            prescription = new
            {
                name = prescription.Name,
                potentialDisease = prescription.PotentialDisease.ToString(),
                medicines = prescription.Medicines,
                usage = prescription.Usage
            }
        };

        var json = JsonSerializer.Serialize(result, new JsonSerializerOptions { WriteIndented = true });
        File.WriteAllText(demand.OutputFile, json);
        Console.WriteLine($"[JsonSaver] 診斷結果已儲存至 {demand.OutputFile}");
    }
}
