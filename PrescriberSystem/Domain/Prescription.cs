namespace PrescriberSystem.Domain;

public class Prescription
{
    public string Name { get; set; } = "";
    public string PotentialDisease { get; set; } = "";
    public List<string> Medicines { get; set; } = new();
    public string Usage { get; set; } = "";
}
