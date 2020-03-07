using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace Chat.Models.Repository
{
    public interface IDataRepository<TEntity>
    {
        List<Filme> GetAll();

        Filme Get(long id);
        void Add(TEntity entity);
        void Register(Login entity);
        void Update(TEntity dbEntity, TEntity entity);
        void Delete(string imdbID, string username);
        Login GetUser(string username);
        List<Filme> GetFavorits(string username);

    }

}