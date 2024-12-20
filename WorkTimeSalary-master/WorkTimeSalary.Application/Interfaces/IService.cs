namespace WorkTimeSalary.Application.Interfaces
{
    public interface IService<T>
        where T : class
    {
        Task DeleteAsync(int id, CancellationToken cancellation = default);
        Task<List<T>> GetAllAsync(CancellationToken cancellation = default);
        Task<T> GetByIdAsync(int id, CancellationToken cancellation = default);
        Task UpdateAsync(T model, CancellationToken cancellation = default);

        Task AddAsync(T model, CancellationToken cancellation = default);
    }
}
