namespace JustTech.Core.DTOs
{
    public class CreateCertificateDto
    {
        public int StudentId { get; set; }
        public int RoundId { get; set; }
        public string? Url { get; set; }
    }
}
