namespace PrescriberSystem.Domain;

public class PrescriptionDemand
{
    public string PatientId { get; }
    public List<Symptom> Symptoms { get; }
    public string OutputFile { get; }
    public Format Format { get; }

    public PrescriptionDemand(string patientId, List<Symptom> symptoms, string outputFile, Format format)
    {
        PatientId = patientId;
        Symptoms = symptoms;
        OutputFile = outputFile;
        Format = format;
    }
}
