using BlazorAppCrud.Data;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;
using Models;

namespace Services
{
    public class FuenteService : IFuenteService
    {
        private readonly DataContext _context;
        private readonly NavigationManager _navigationManager;
        public List<Fuente> Fuentes { get; set; } = new List<Fuente>();

        public FuenteService(DataContext context, NavigationManager navigationManager)
        {
            _context = context;
            _navigationManager = navigationManager;
            _context.Database.EnsureCreated();
        }


        public async Task LoadFuentes()
        {
            Fuentes.Clear();
            var FuentesDb = await _context.Fuentes.ToListAsync();
            foreach (var item in FuentesDb)
            {
                Fuentes.Add(new Fuente(item.CadenaFuente) { IdFuente = item.IdFuente});
            }
        }


        public async Task<Fuente> GetSingleFuente(string id)
        {

            var fuente = await _context.Fuentes.FindAsync(id);
            if (fuente == null)
            {
                throw new Exception("No hay fuentes con este id.");
            }
            return fuente;
        }


        public async Task CreateFuente(Fuente fuente)
        {
            foreach (var Letra in fuente.Letras)
            {
                Letra.IdFuente = (fuente.IdFuente);
            }
            _context.Fuentes.Add(fuente);
            await _context.SaveChangesAsync();
            _navigationManager.NavigateTo("fuentes");
        }

        public async Task DeleteFuente(string id)
        {
            var dbFuente = await _context.Fuentes.FindAsync(id);
            if (dbFuente == null)
            {
                throw new Exception("No hay Fuente con este id.");
            }
            _context.Fuentes.Remove(dbFuente);
            await _context.SaveChangesAsync();
            _navigationManager.NavigateTo("fuentes");
        }

        public async Task UpdateFuente(Fuente fuente, string id)
        {
            var dbGame = await _context.Fuentes.FindAsync(id);
            if (dbGame == null)
            {
                throw new Exception("No hay ninguna fuente con este id.");
            }
            dbGame.CadenaFuente = fuente.CadenaFuente;

            await _context.SaveChangesAsync();
            _navigationManager.NavigateTo("fuentes");
        }
    }
}


