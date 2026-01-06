namespace Entities.Models
{
    public class Employee : DomainEntity<Guid>
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Address { get; set; }
        public int Age { get; set; }
        public string City { get; set; }
        public string Position { get; set; }
        public Guid CompanyId { get; set; }
        public Company Company { get; set; }
    }
}
