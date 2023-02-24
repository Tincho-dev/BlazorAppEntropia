using BlazorAppCrud.Data;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;
using Models;

namespace Services
{
    public class LetraService : ILetraService
    {
        private readonly DataContext _context;

        public List<Letra> Letras { get; set; } = new List<Letra>();

        public LetraService(DataContext context)
        {
            _context = context;
            _context.Database.EnsureCreated();
        }

        public async Task LoadLetras()
        {
            Letras = await _context.Letras.ToListAsync();
        }

        public async Task<Letra> GetSingleLetra(int id)
        {
            var letra = await _context.Letras.FindAsync(id);
            if (letra == null)
            {
                throw new Exception("No hay letras aqui.");
            }
            return letra;
        }

        public async Task CreateLetra(Letra letra)
        {
            letra.Id = letra.IdFuente + letra.Name;
            _context.Letras.Add(letra);
            await _context.SaveChangesAsync();

        }

        public async Task DeleteLetra(int id)
        {
            var dbLetra = await _context.Letras.FindAsync(id);
            if (dbLetra == null)
            {
                throw new Exception("No hay letras aqui.");
            }
            _context.Letras.Remove(dbLetra);
            await _context.SaveChangesAsync();
        }
    }
}


