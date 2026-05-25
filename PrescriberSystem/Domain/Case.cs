namespace PrescriberSystem.Domain;

public class Case
{
    public List<Symptom> Symptoms { get; set; } = new();
    public Prescription? Prescription { get; set; }
    public DateTime CaseTime { get; set; }
}
