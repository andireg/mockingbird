namespace Mockingbird.Factory.Http
{
    internal class HttpResponse
    {
        public int Status { get; set; }
        public string? Content { get; set; }
        public string? ContentType { get; set; }
    }
}