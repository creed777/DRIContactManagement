using DRIContactManagement.Models;
using DRIContactManagement.Pages;
using Microsoft.EntityFrameworkCore;

namespace DRIContactManagement.Repository
{
    public class ContactRepository : IContactRepository
    {
        private AppDbContext AppDbContext { get; set; }
        private readonly ILogger<IndexModel> _logger;

        public ContactRepository(AppDbContext appDbContext, ILogger<IndexModel> logger)
        {
            AppDbContext = appDbContext;
            _logger = logger;
        }

        public async Task<List<Contact>> GetContactsAsync()
        {
            return await AppDbContext.Contact
                .Where(x => !x.IsRetired && !x.IsDeleted)
                .AsNoTracking()
                .ToListAsync()
                .ConfigureAwait(false);
        }

        public async Task<List<Contact>> GetContactsAsync(List<string> contactIds)
        {
            return await AppDbContext.Contact
                .Where(x => contactIds.Contains(x.ServiceId))
                .AsNoTracking()
                .ToListAsync()
                .ConfigureAwait(false);
        }

        public async Task UpdateContactsAsync(List<Contact> newContacts)
        {
            try
            {
                foreach(var contact in newContacts)
                {
                    AppDbContext.Contact.Attach(contact).State = EntityState.Modified;
                }
                
                AppDbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<List<string>> GetServiceIds()
        {
            return await AppDbContext.Contact
                .Select(x => x.ServiceId)
                .AsNoTracking()
                .ToListAsync()
                .ConfigureAwait(false);
        }

        public async Task<int> DeleteContactData(List<string> serviceIds)
        {
            var contactsToUpdate = AppDbContext.Contact.Where(x => serviceIds.Contains(x.ServiceId));
            await contactsToUpdate.ForEachAsync(x => x.IsDeleted = true)
                .ConfigureAwait(false);
            AppDbContext.UpdateRange(contactsToUpdate);
            return AppDbContext.SaveChanges();
        }
    }
}
