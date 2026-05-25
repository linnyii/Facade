using PrescriberSystem.Domain;

namespace PrescriberSystem.Handlers;

public abstract class PrescriptionHandler
{
    private PrescriptionHandler? _next;

    public PrescriptionHandler SetNext(PrescriptionHandler next)
    {
        _next = next;
        return next;
    }

    public Prescription? Handle(PrescriptionDemand demand, Patient patient)
    {
        if (CanHandle(demand, patient))
            return CreatePrescription(demand, patient);
        return _next?.Handle(demand, patient);
    }

    protected abstract bool CanHandle(PrescriptionDemand demand, Patient patient);
    protected abstract Prescription CreatePrescription(PrescriptionDemand demand, Patient patient);
}
