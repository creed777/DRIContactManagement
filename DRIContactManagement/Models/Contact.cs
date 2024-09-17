using System.ComponentModel.DataAnnotations;

namespace DRIContactManagement.Models
{
    public class Contact
    {
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        [Key]
        public string ServiceId {get; set;}
        public string? DRIName { get; set;}
        public string? DRIEmail { get; set;}
        public string? DelegateName { get; set;}
        public string? DelegateEmail { get; set;}
        public bool IsRetired { get; set;}
        public bool IsDeleted { get; set;}
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    }
}
