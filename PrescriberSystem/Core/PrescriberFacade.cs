using PrescriberSystem.Domain;
using PrescriberSystem.Handlers;
using PrescriberSystem.Savers;

namespace PrescriberSystem.Core;

public class PrescriberFacade
{
    private readonly Prescriber _prescriber;
    private readonly PatientDatabase _patientDatabase;

    public PrescriberFacade(string patientJsonFile, string diseasesFile)
    {
        _patientDatabase = new PatientDatabase(patientJsonFile);
        var handlerChain = BuildHandlerChain(diseasesFile);
        _prescriber = new Prescriber(_patientDatabase, handlerChain);
    }

    public void Prescribe(string patientId, List<Symptom> symptoms, string outputFile, Format format)
    {
        var demand = new PrescriptionDemand(patientId, symptoms, outputFile, format);
        _prescriber.Prescribe(demand, (prescription, d) =>
        {
            _patientDatabase.SaveResult(prescription, d);

            IPrescriptionResultSaver saver = d.Format == Format.Json ? new JsonSaver() : new CsvSaver();
            saver.Save(prescription, d);
        });
    }

    private static PrescriptionHandler BuildHandlerChain(string diseasesFile)
    {
        var diseases = File.ReadAllLines(diseasesFile)
            .Select(l => l.Trim())
            .Where(l => !string.IsNullOrEmpty(l))
            .ToList();

        var handlers = new Dictionary<string, PrescriptionHandler>
        {
            ["COVID-19"] = new CovidHandler(),
            ["Attractive"] = new AttractiveHandler(),
            ["SleepApneaSyndrome"] = new SleepApneaSyndromeHandler()
        };

        PrescriptionHandler? head = null;
        PrescriptionHandler? tail = null;

        foreach (var disease in diseases)
        {
            if (!handlers.TryGetValue(disease, out var handler)) continue;

            if (head == null)
            {
                head = handler;
                tail = handler;
            }
            else
            {
                tail!.SetNext(handler);
                tail = handler;
            }
        }

        if (head == null)
            throw new InvalidOperationException("diseases 檔案中沒有任何合法的疾病名稱");

        return head;
    }
}
