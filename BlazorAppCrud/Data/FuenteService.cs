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
            Fuentes = await _context.Fuentes.ToListAsync();
        }


        public async Task<Fuente> GetSingleFuente(string id)
        {

            var fuente = await _context.Fuentes.FindAsync(id);
            if (fuente == null)
            {
                throw new Exception("No gaem here.");
            }
            return fuente;
        }


        public async Task CreateFuente(Fuente fuente)
        {
            foreach (var Letra in fuente.Letras)
            {
                Letra.IdFuente = (fuente.IdFuente);
                _context.Letras.Add(Letra);
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
                throw new Exception("No Fuente here.");
            }
            _context.Fuentes.Remove(dbFuente);
            await _context.SaveChangesAsync();
            _navigationManager.NavigateTo("videoFuentes");
        }

        public async Task UpdateFuente(Fuente fuente, string id)
        {
            var dbGame = await _context.Fuentes.FindAsync(id);
            if (dbGame == null)
            {
                throw new Exception("no games here.");
            }
            dbGame.IdFuente = fuente.IdFuente;
            dbGame.CadenaFuente = fuente.CadenaFuente;
            dbGame.Letras = fuente.Letras;

            await _context.SaveChangesAsync();
            _navigationManager.NavigateTo("videogames");
        }
    }
}


