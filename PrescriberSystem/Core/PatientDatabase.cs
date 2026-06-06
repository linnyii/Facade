using System.Text.Json;
using System.Text.Json.Serialization;
using PrescriberSystem.Domain;
using PrescriberSystem.Dto;

namespace PrescriberSystem.Core;

public class PatientDatabase
{
    private List<Patient> _patients;

    public PatientDatabase(string jsonFilePath)
    {
        InitPatientsDataBase(jsonFilePath);
    }

    private void InitPatientsDataBase(string jsonFilePath)
    {
        var json = File.ReadAllText(jsonFilePath);
        var options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };
        options.Converters.Add(new JsonStringEnumConverter());
        _patients = JsonSerializer.Deserialize<List<Patient>>(json, options) ?? [];
    }

    public Patient? SearchPatientById(string id) =>
        _patients.FirstOrDefault(p => p.Id == id);

    public void SaveResult(Prescription prescription, PrescriptionDemand demand)
    {
        var patient = SearchPatientById(demand.PatientId);

        patient?.Cases.Add(new Case
        {
            Symptoms = demand.Symptoms,
            Prescription = prescription,
            CaseTime = DateTime.Now
        });
    }
}
