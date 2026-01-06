namespace Entities.Models
{
    public class Company : DomainEntity<Guid>
    {
        public string Name { get; set; }
        public string Address { get; set; }
        public string Country { get; set; }
        public ICollection<Employee> Employees { get; set; }
    }
}
