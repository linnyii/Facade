using System.Collections.Concurrent;
using PrescriberSystem.Domain;
using PrescriberSystem.Dto;
using PrescriberSystem.Handlers;

namespace PrescriberSystem.Core;

public class Prescriber : IDisposable
{
    private readonly PatientDatabase _patientDatabase;
    private readonly PrescriptionHandler _handlerChain;
    private readonly BlockingCollection<PendingDemand> _queue = new();
    private readonly CancellationTokenSource _cancellation = new();
    private readonly Task _worker;

    private record PendingDemand(PrescriptionDemand Demand, Action<Prescription, PrescriptionDemand> OnCompleted);

    public Prescriber(PatientDatabase patientDatabase, PrescriptionHandler handlerChain)
    {
        _patientDatabase = patientDatabase;
        _handlerChain = handlerChain;
        _worker = Task.Run(ProcessDemandQueue);
    }

    public void Prescribe(PrescriptionDemand demand, Action<Prescription, PrescriptionDemand> onCompleted)
    {
        if (_queue.IsAddingCompleted)
        {
            Console.WriteLine($"[Prescriber] 已關閉，拒絕新的診斷要求：{demand.PatientId}");
            return;
        }

        _queue.Add(new PendingDemand(demand, onCompleted));
        Console.WriteLine($"[Prescriber] 診斷要求已加入排隊：{demand.PatientId}");
    }

    public void Shutdown()
    {
        Console.WriteLine("[Prescriber] 開始關閉：不再接受新要求，等待排隊中的診斷處理完畢……");
        _queue.CompleteAdding();
        _worker.Wait();
    }

    public void Cancel()
    {
        Console.WriteLine("[Prescriber] 要求立即取消……");
        _cancellation.Cancel();
        _queue.CompleteAdding();
        _worker.Wait();
    }

    public void Dispose()
    {
        if (!_queue.IsAddingCompleted)
        {
            Shutdown();
        }

        _queue.Dispose();
        _cancellation.Dispose();
    }

    private void ProcessDemandQueue()
    {
        try
        {
            foreach (var pending in _queue.GetConsumingEnumerable(_cancellation.Token))
            {
                try
                {
                    Console.WriteLine($"[Prescriber] 開始診斷：{pending.Demand.PatientId}");
                    Task.Delay(3000, _cancellation.Token).Wait();

                    var patient = FindPatient(pending.Demand.PatientId);
                    if (patient == null)
                    {
                        continue;
                    }

                    var prescription = Diagnosing(pending.Demand, patient);
                    if (prescription == null)
                    {
                        continue;
                    }

                    DiagnosedDoneAndNotifyToSaveResult(pending, prescription);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"[Prescriber] 診斷 {pending.Demand.PatientId} 出錯：{ex.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[Prescriber] 已取消，停止處理排隊中的診斷要求。{ex.Message}");
        }

        Console.WriteLine("[Prescriber] 背景處理迴圈已結束。");
    }

    private Prescription? Diagnosing(PrescriptionDemand demand, Patient patient)
    {
        var prescription = _handlerChain.Handle(demand, patient);
        if (prescription == null)
        {
            Console.WriteLine($"[Prescriber] 無法診斷：{demand.PatientId}，症狀不符合任何已知疾病");
            return null;
        }

        Console.WriteLine($"[Prescriber] 診斷完成：{demand.PatientId}，處方：{prescription.Name}");
        return prescription;
    }

    private void DiagnosedDoneAndNotifyToSaveResult(PendingDemand pending, Prescription prescription)
    {
        try
        {
            pending.OnCompleted(prescription, pending.Demand);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[Prescriber] OnCompleted callback 出錯：{pending.Demand.PatientId}，{ex.Message}");
        }
    }

    private Patient? FindPatient(string patientId)
    {
        var patient = _patientDatabase.SearchPatientById(patientId);
        if (patient == null)
        {
            Console.WriteLine($"[Prescriber] 找不到病患：{patientId}");
        }

        return patient;
    }
}
