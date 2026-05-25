using System.Text.Json;
using System.Text.Json.Serialization;
using PrescriberSystem.Domain;

namespace PrescriberSystem.Core;

public class PatientDatabase
{
    private readonly List<Patient> _patients;

    public PatientDatabase(string jsonFilePath)
    {
        var json = File.ReadAllText(jsonFilePath);
        var options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
            Converters = { new JsonStringEnumConverter() }
        };
        _patients = JsonSerializer.Deserialize<List<Patient>>(json, options) ?? new List<Patient>();
    }

    public Patient? SearchPatientById(string id) =>
        _patients.FirstOrDefault(p => p.Id == id);

    public Patient? SearchPatientByName(string name) =>
        _patients.FirstOrDefault(p => p.Name == name);

    public void AddPatient(Patient patient) =>
        _patients.Add(patient);

    public void SaveResult(Prescription prescription, PrescriptionDemand demand)
    {
        var patient = SearchPatientById(demand.PatientId);
        if (patient == null) return;

        patient.Cases.Add(new Case
        {
            Symptoms = demand.Symptoms,
            Prescription = prescription,
            CaseTime = DateTime.Now
        });
    }
}
