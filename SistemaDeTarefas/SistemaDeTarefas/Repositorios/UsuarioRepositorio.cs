using Microsoft.EntityFrameworkCore;
using SistemaDeTarefas.Data;
using SistemaDeTarefas.Models;
using SistemaDeTarefas.Repositorios.Interfaces;

namespace SistemaDeTarefas.Repositorios
{
    public class UsuarioRepositorio : IUsuarioRepositorio
    {
        private readonly SistemaDeTarefasDBContext _dbContext;

        public UsuarioRepositorio(SistemaDeTarefasDBContext sistemaDeTarefasDBContext)
        {
            _dbContext = sistemaDeTarefasDBContext;
        }

        public async Task<UsuarioModel> BuscarUsuarioId(int id)
        {
            return await _dbContext.Usuarios.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<List<UsuarioModel>> BuscarTodosUsuarios()
        {
            return await _dbContext.Usuarios.ToListAsync();
        }

        public async Task<UsuarioModel> Adicionar(UsuarioModel usuario)
        {
            await _dbContext.Usuarios.AddAsync(usuario);
            await _dbContext.SaveChangesAsync();
            
            return usuario;
        }

        public async Task<UsuarioModel> Atualizar(UsuarioModel usuario, int id)
        {
            UsuarioModel usuarioPorId = await BuscarUsuarioId(id);

            if (usuarioPorId == null)
            {
                throw new Exception($"Usuario para o ID: {id} não foi encontrado.");
            }
            usuarioPorId.Nome = usuario.Nome;
            usuarioPorId.Email = usuario.Email;

            _dbContext.Usuarios.Update(usuarioPorId);
            await _dbContext.SaveChangesAsync();

            return usuarioPorId;
        }

        public async Task<bool> Apagar(int id)
        {
            UsuarioModel usuarioPorId = await BuscarUsuarioId(id);

            if (usuarioPorId == null)
            {
                throw new Exception($"Usuario para o ID: {id} não foi encontrado.");
            }
            _dbContext.Usuarios.Remove(usuarioPorId);
            await _dbContext.SaveChangesAsync();

            return true;
        }
    }
}
