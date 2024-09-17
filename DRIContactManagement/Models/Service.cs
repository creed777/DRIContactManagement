namespace DRIContactManagement.Models
{
    public class Service
    {

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        public string ServiceId { get; set; }
        public string ServiceName { get; set; }
        public string DivisionName { get; set;}
        public string OrganizationName { get; set; }
        public string ServiceGroupName {  get; set; }
        public string TeamGroupName { get; set; }
        public DateTime CreatedDated { get; set; }
        public DateTime ModifiedDate { get; set; }
        public bool IsRetired { get; set; }
        public bool IsDeleted { get; set; }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    }
}
