using PrescriberSystem.Domain;

namespace PrescriberSystem.Dto;

public class Patient
{
    public string Id { get; set; } = "";
    public Gender Gender { get; }
    public int Age { get; }
    public float Height { get; }
    public float Weight { get; }
    public List<Case> Cases { get; } = [];

    public double Bmi => Weight / Math.Pow(Height / 100.0, 2);
}
