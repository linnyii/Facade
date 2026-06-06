using PrescriberSystem.Domain;
using PrescriberSystem.Dto;

namespace PrescriberSystem.Handlers;

public abstract class PrescriptionHandler
{
    private PrescriptionHandler? _next;

    public void SetNext(PrescriptionHandler next)
    {
        _next = next;
    }

    public Prescription? Handle(PrescriptionDemand demand, Patient patient)
    {
        if (CanHandle(demand, patient))
            return CreatePrescription();
        return _next?.Handle(demand, patient);
    }

    protected abstract bool CanHandle(PrescriptionDemand demand, Patient patient);
    protected abstract Prescription CreatePrescription();
}
