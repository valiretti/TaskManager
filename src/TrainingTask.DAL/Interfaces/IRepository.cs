namespace TrainingTask.DAL.Interfaces
{
    public interface IRepository<T> where T: class
    {
        T Get(int id);

        int Create(T item);

        void Update(T item);

        void Delete(int id);
    }
}
