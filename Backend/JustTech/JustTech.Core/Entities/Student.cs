namespace JustTech.Core.Entities
{
    public class Student : BaseEntity
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string? Phone { get; set; }
        public string? Profession { get; set; }
        public string? Image { get; set; }
        public string? City { get; set; }
        public string? Country { get; set; }
        public string? StudentStatus { get; set; }   // student / graduate
        public DateTime? Birthdate { get; set; }


        // Navigation Properties
        public virtual ICollection<Enrollment> Enrollment { get; set; }
        public virtual ICollection<Progress> Progresses { get; set; }
        public virtual ICollection<Submission> Submissions { get; set; }
        public virtual ICollection<Certificate> Certificates { get; set; }


    }
}
