using System.Collections.Concurrent;
using PrescriberSystem.Domain;
using PrescriberSystem.Handlers;

namespace PrescriberSystem.Core;

public class Prescriber
{
    private readonly PatientDatabase _patientDatabase;
    private readonly PrescriptionHandler _handlerChain;
    private readonly BlockingCollection<PendingDemand> _queue = new();

    private record PendingDemand(PrescriptionDemand Demand, Action<Prescription, PrescriptionDemand> OnCompleted);

    public Prescriber(PatientDatabase patientDatabase, PrescriptionHandler handlerChain)
    {
        _patientDatabase = patientDatabase;
        _handlerChain = handlerChain;
        Task.Run(ProcessQueue);
    }

    public void Prescribe(PrescriptionDemand demand, Action<Prescription, PrescriptionDemand> onCompleted)
    {
        _queue.Add(new PendingDemand(demand, onCompleted));
        Console.WriteLine($"[Prescriber] 診斷要求已加入排隊：{demand.PatientId}");
    }

    private void ProcessQueue()
    {
        foreach (var pending in _queue.GetConsumingEnumerable())
        {
            Console.WriteLine($"[Prescriber] 開始診斷：{pending.Demand.PatientId}");
            Thread.Sleep(3000);

            var patient = _patientDatabase.SearchPatientById(pending.Demand.PatientId);
            if (patient == null)
            {
                Console.WriteLine($"[Prescriber] 找不到病患：{pending.Demand.PatientId}");
                continue;
            }

            var prescription = _handlerChain.Handle(pending.Demand, patient);
            if (prescription == null)
            {
                Console.WriteLine($"[Prescriber] 無法診斷：{pending.Demand.PatientId}，症狀不符合任何已知疾病");
                continue;
            }

            Console.WriteLine($"[Prescriber] 診斷完成：{pending.Demand.PatientId}，處方：{prescription.Name}");
            pending.OnCompleted(prescription, pending.Demand);
        }
    }
}
