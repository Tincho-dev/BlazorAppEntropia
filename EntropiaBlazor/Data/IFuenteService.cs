using Models;

namespace EntropiaBlazor.Data
{
    public interface IFuenteService
    {
        List<Fuente> Fuentes { get; }
        Task DeleteFuente(string id);
        Task CreateFuente(Fuente fuente);
        Task<Fuente> GetSingleFuente(string id);
        Task LoadFuentes();
        Task UpdateFuente(Fuente fuente,string id);

    }
}
