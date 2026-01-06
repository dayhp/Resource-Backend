namespace Entities.RequestFeatures
{
    public class EmployeeParameters : RequestParameters
    {
        public EmployeeParameters()
        {
            OrderBy = "FirstName";
        }
        public uint MinAge { get; set; } = 1;
        public uint MaxAge { get; set; } = 200;
        public string SearchTerm { get; set; }
        public bool ValidAgeRange => MaxAge > MinAge;
    }
}
