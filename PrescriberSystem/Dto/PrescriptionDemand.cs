namespace PrescriberSystem.Domain;

public class PrescriptionDemand(string patientId, List<Symptom> symptoms, string outputFile, Format format)
{
    public string PatientId { get; } = patientId;
    public List<Symptom> Symptoms { get; } = symptoms;
    public string OutputFile { get; } = outputFile;
    public Format Format { get; } = format;
}
