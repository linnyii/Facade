namespace PrescriberSystem.Domain;

public class Prescription
{
    public string Name { get; init; } = "";
    public string PotentialDisease { get; init; } = "";
    public List<string> Medicines { get; init; } = [];
    public string Usage { get; init; } = "";
}
