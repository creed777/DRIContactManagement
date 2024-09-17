namespace DRIContactManagement.Repository
{
    using DRIContactManagement.Models;

    public interface IServiceRepository
    {
        public Task<List<Service>> GetServiceData();
        public Task<bool> AddServiceDataToContacts(List<string> serviceIds);
    }
}
