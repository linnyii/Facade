namespace PrescriberSystem.Domain;

public class Patient
{
    public string Id { get; set; } = "";
    public Gender Gender { get; set; }
    public int Age { get; set; }
    public float Height { get; set; }
    public float Weight { get; set; }
    public List<Case> Cases { get; set; } = [];

    public double Bmi => Weight / Math.Pow(Height / 100.0, 2);
}
