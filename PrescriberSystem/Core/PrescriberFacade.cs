using PrescriberSystem.Domain;
using PrescriberSystem.Handlers;
using PrescriberSystem.Savers;

namespace PrescriberSystem.Core;

public class PrescriberFacade
{
    private readonly Prescriber _prescriber;
    private readonly PatientDatabase _patientDatabase;

    public PrescriberFacade(string patientsDataJsonFile, string supportDiseasesFile)
    {
        _patientDatabase = new PatientDatabase(patientsDataJsonFile);
        var supportDiseasesHandlerChain = BuildHandlerChain(supportDiseasesFile);
        _prescriber = new Prescriber(_patientDatabase, supportDiseasesHandlerChain);
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

    /// <summary>排空式關閉：把已排隊的診斷要求處理完才停。</summary>
    public void Shutdown() => _prescriber.Shutdown();

    /// <summary>立即取消：放棄排隊中與處理中的要求，盡快停止。</summary>
    public void Cancel() => _prescriber.Cancel();

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
