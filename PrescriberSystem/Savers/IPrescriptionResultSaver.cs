using PrescriberSystem.Domain;

namespace PrescriberSystem.Savers;

public interface IPrescriptionResultSaver
{
    void Save(Prescription prescription, PrescriptionDemand demand);
}
