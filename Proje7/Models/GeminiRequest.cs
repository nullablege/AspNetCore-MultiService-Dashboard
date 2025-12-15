namespace Proje7.Models
{
    // Gemini'ye göndereceğimiz istek kalıbı
    public class GeminiRequest
    {
        public List<Content> Contents { get; set; }
    }

    public class Content
    {
        public List<Part> Parts { get; set; }
    }

    public class Part
    {
        public string Text { get; set; }
    }

    public class GeminiApiRoot
    {
        public List<Candidate> Candidates { get; set; }
    }

    public class Candidate
    {
        public Content Content { get; set; }
    }
}
