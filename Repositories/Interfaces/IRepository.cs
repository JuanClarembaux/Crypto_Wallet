namespace CryptoWallet.Repositories.Interfaces
{
    public interface IRepository<T>
    {
        public IEnumerable<T> GetAll();
        T findId(object Id);

        void Add(T entity);
        void Update(T entity);
        void Delete(T entity);

    }
}
