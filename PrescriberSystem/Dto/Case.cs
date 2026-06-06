using PrescriberSystem.Domain;

namespace PrescriberSystem.Dto;

public class Case
{
    public List<Symptom> Symptoms { get; init; } = [];
    public Prescription? Prescription { get; init; }
    public DateTime CaseTime { get; init; }
}
