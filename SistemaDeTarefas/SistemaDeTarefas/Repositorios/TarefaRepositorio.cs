using Microsoft.EntityFrameworkCore;
using SistemaDeTarefas.Data;
using SistemaDeTarefas.Models;
using SistemaDeTarefas.Repositorios.Interfaces;

namespace SistemaDeTarefas.Repositorios
{
    public class TarefaRepositorio : ITarefaRepositorio
    {
        private readonly SistemaDeTarefasDBContext _dbContext;

        public TarefaRepositorio(SistemaDeTarefasDBContext sistemaDeTarefasDBContext)
        {
            _dbContext = sistemaDeTarefasDBContext;
        }

        public async Task<TarefaModel> BuscarTarefaId(int id)
        {
            return await _dbContext.Tarefas
                .Include(x => x.Usuario)
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<List<TarefaModel>> BuscarTodasTarefas()
        {
            return await _dbContext.Tarefas
                .Include(x => x.Usuario)
                .ToListAsync();
        }

        public async Task<TarefaModel> Adicionar(TarefaModel tarefa)
        {
            await _dbContext.Tarefas.AddAsync(tarefa);
            await _dbContext.SaveChangesAsync();
            
            return tarefa;
        }

        public async Task<TarefaModel> Atualizar(TarefaModel tarefa, int id)
        {
            TarefaModel tarefaPorId = await BuscarTarefaId(id);

            if (tarefaPorId == null)
            {
                throw new Exception($"Tarefa para o ID: {id} não foi encontrado.");
            }
            tarefaPorId.Nome = tarefa.Nome;
            tarefaPorId.Descricao = tarefa.Descricao;
            tarefaPorId.Status = tarefa.Status;
            tarefaPorId.UsuarioId = tarefa.UsuarioId;

            _dbContext.Tarefas.Update(tarefaPorId);
            await _dbContext.SaveChangesAsync();

            return tarefaPorId;
        }

        public async Task<bool> Apagar(int id)
        {
            TarefaModel tarefaPorId = await BuscarTarefaId(id);

            if (tarefaPorId == null)
            {
                throw new Exception($"Tarefa para o ID: {id} não foi encontrado.");
            }
            _dbContext.Tarefas.Remove(tarefaPorId);
            await _dbContext.SaveChangesAsync();

            return true;
        }
    }
}
