namespace Entities.Dto
{
    public class CompanyDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string FullAddress { get; set; } // Combines Address and Country
        public string Conutry { get; set; }
    }
}
