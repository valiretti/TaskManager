using TrainingTask.BLL.Interfaces;
using TrainingTask.DAL.Interfaces;

namespace TrainingTask.BLL.Services
{
    public abstract class Service<T> : IService<T> where T : class
    {
        private readonly IRepository<T> _repository;

        protected Service(IRepository<T> repository)
        {
            _repository = repository;
        }

        public void Delete(int id)
        {
            _repository.Delete(id);
        }

        public T Get(int id)
        {
           return _repository.Get(id);
        }
    }
}
