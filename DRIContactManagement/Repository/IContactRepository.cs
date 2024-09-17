using DRIContactManagement.Models;

namespace DRIContactManagement.Repository
{
    public interface IContactRepository
    {
        public Task<List<Contact>> GetContactsAsync();
        public Task<List<Contact>> GetContactsAsync(List<string> contactIds);
        public Task UpdateContactsAsync(List<Contact> newContacts);
        public Task<List<string>> GetServiceIds();
        public Task<int> DeleteContactData(List<string> serviceIds);
    }
}
