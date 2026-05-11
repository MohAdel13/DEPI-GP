namespace JustTech.Core.Interfaces
{
    public interface IUnitOfWork : IDisposable /*IDisposable: ensures database connection is properly closed*/
    {
        //gives access to course data operations through a single entry point
        ICourseRepository Courses { get; }
        IStudentRepository Students { get; }
        IRoundRepository Rounds { get; }
        IEnrollmentRepository Enrollments { get; }
        IInstructorRepository Instructors { get; }
        ILectureRepository Lectures { get; }
        IMaterialRepository Materials { get; }
        IProgressRepository Progresses { get; }
        IAssignmentRepository Assignments { get; }

        // commits all pending changes to database in one transaction
        Task<int> SaveChangesAsync();
    }
}
