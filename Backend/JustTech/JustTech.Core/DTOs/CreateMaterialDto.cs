namespace JustTech.Core.DTOs
{
    public class CreateMaterialDto
    {
        public int LectureId { get; set; }
        public string Url { get; set; }
        public string Type { get; set; }  // video, pdf, document, etc.
    }
}
