namespace PrescriberSystem.Domain;

public class Prescription
{
    public string Name { get; init; } = "";
    public PotentialDisease PotentialDisease { get; init; }
    public List<string> Medicines { get; init; } = [];
    public string Usage { get; init; } = "";
}
