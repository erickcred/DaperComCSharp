using System;
using System.Collections.Generic;
using System.Linq;
using eCommerce.Models;

namespace eCommerce.Repositories.Interfaces
{
    public interface IRepository<T> where T : class
    {
        public List<T> Get();
        public T GetById(int id);
        public List<T> Lixeira();
        public void Create(T usuario);
        public void Update(T id);
        public void Lixeira(int id);
        public void Delete(int id);
    }
}
