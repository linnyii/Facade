using PrescriberSystem.Savers;

namespace PrescriberSystem.Domain;

public class PrescriptionDemand(string patientId, List<Symptom> symptoms, string outputFile, IPrescriptionResultSaver saver)
{
    public string PatientId { get; } = patientId;
    public List<Symptom> Symptoms { get; } = symptoms;
    public string OutputFile { get; } = outputFile;
    public IPrescriptionResultSaver Saver { get; } = saver;
}
