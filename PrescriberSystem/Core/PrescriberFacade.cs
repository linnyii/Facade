using PrescriberSystem.Domain;
using PrescriberSystem.Handlers;
using PrescriberSystem.Savers;

namespace PrescriberSystem.Core;

public class PrescriberFacade : IDisposable
{
    private static readonly Dictionary<string, PrescriptionHandler> Handlers = new()
    {
        ["COVID-19"] = new CovidHandler(),
        ["Attractive"] = new AttractiveHandler(),
        ["SleepApneaSyndrome"] = new SleepApneaSyndromeHandler()
    };

    private readonly Prescriber _prescriber;
    private readonly PatientDatabase _patientDatabase;

    public PrescriberFacade(string patientsDataJsonFile, string supportDiseasesFile)
    {
        _patientDatabase = new PatientDatabase(patientsDataJsonFile);
        var supportDiseasesHandlerChain = BuildHandlerChain(supportDiseasesFile);
        _prescriber = new Prescriber(_patientDatabase, supportDiseasesHandlerChain);
    }

    public void Prescribe(string patientId, List<Symptom> symptoms, string outputFile, IPrescriptionResultSaver saver)
    {
        var demand = new PrescriptionDemand(patientId, symptoms, outputFile, saver);
        _prescriber.Prescribe(demand, (prescription, d) =>
        {
            _patientDatabase.SaveResult(prescription, d);
            d.Saver.Save(prescription, d);
        });
    }

    //public void Shutdown() => _prescriber.Shutdown();

    //public void Cancel() => _prescriber.Cancel();

    public void Dispose() => _prescriber.Shutdown();

    private static PrescriptionHandler BuildHandlerChain(string diseasesFile)
    {
        var diseases = File.ReadAllLines(diseasesFile)
            .Select(l => l.Trim())
            .Where(l => !string.IsNullOrEmpty(l))
            .ToList();

        var chain = diseases
            .Where(Handlers.ContainsKey)
            .Select(disease => Handlers[disease])
            .ToList();

        if (!HasAnyDiseaseHandler(chain))
            throw new InvalidOperationException("diseases 檔案中沒有任何合法的疾病名稱");

        return DiseasesHandlerChain(chain);
    }

    private static bool HasAnyDiseaseHandler(List<PrescriptionHandler> chain) => chain.Count > 0;

    private static PrescriptionHandler DiseasesHandlerChain(List<PrescriptionHandler> chain)
    {
        for (var i = 0; i < chain.Count - 1; i++)
            chain[i].SetNext(chain[i + 1]);

        return chain[0];
    }
}
