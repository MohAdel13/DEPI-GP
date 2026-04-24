namespace JustTech.Core.Interfaces
{
    public interface IUnitOfWork : IDisposable /*IDisposable: ensures database connection is properly closed*/
    {
        //gives access to course data operations through a single entry point
        ICourseRepository Courses { get; }

        // commits all pending changes to database in one transaction
        Task<int> SaveChangesAsync();
    }
}
