using DRIContactManagement.Models;
using DRIContactManagement.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;

namespace DRIContactManagement.Pages
{

    public class IndexModel : PageModel
    {
        private AppDbContext? AppDbContext { get; set; }
        private readonly ILogger<IndexModel>? _logger;
        public List<ServiceContact>? ServiceContacts { get; set; }
        public List<string>? ServiceIds { get; set; }
        IContactRepository? ContactRepository { get; set; }
        IServiceRepository? ServiceRepository { get; set; }

        public IndexModel(IContactRepository? contactRepository, IServiceRepository? serviceRepository)
        {
            ContactRepository = contactRepository;
            ServiceRepository = serviceRepository;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            List<Service> services = new();

            if(ServiceRepository != null)
            {
                services = await ServiceRepository.GetServiceData().ConfigureAwait(false);
            }

            List<Contact> contacts = new();
                if (ContactRepository != null)
                {
                    contacts = await ContactRepository.GetContactsAsync().ConfigureAwait(false);
                }

            ServiceContacts = services.GroupJoin(
                contacts,
                service => service.ServiceId,
                contact => contact.ServiceId,
                (service, contact) => new { service, subgroup = contact.DefaultIfEmpty() })
                .Select(gj => new ServiceContact
                {
                    ServiceId = gj.service.ServiceId,
                    ServiceName = gj.service.ServiceName,
                    OrganizationName = gj.service.OrganizationName,//3
                    DivisionName = gj.service.DivisionName, //4
                    ServiceGroupName = gj.service.ServiceGroupName, //5
                    TeamGroupName = gj.service.TeamGroupName, //6
                    DRIName = gj.subgroup.FirstOrDefault()?.DRIName ?? string.Empty, //7
                    DRIEmail = gj.subgroup.FirstOrDefault()?.DRIEmail ?? string.Empty, //8
                    DelegateName = gj.subgroup.FirstOrDefault()?.DelegateName ?? string.Empty,
                    DelegateEmail = gj.subgroup.FirstOrDefault()?.DelegateEmail ?? string.Empty,
                }).ToList();

            return Page();
        }

        public async Task<IActionResult> OnPostAsync([FromBody] List<object> contacts)
        {
            List<Contact>? NewContacts = new();
            if (contacts != null)
            {
                foreach (var contact in contacts)
                {
                    var temp = JsonConvert.DeserializeObject<Contact>(contact.ToString()??string.Empty);
                    NewContacts.Add(temp??new Contact());
                }

                if(ContactRepository != null) 
                    await ContactRepository.UpdateContactsAsync(NewContacts).ConfigureAwait(false);
            }

            return Page();
        }

        public async Task<int> SetServicesAsRetiredAsync()
        {
            List<Service> services = new();

            if (ServiceRepository != null)
            {
                services = await ServiceRepository.GetServiceData().ConfigureAwait(false);
            }

            List<Contact> contacts = new();
            if (ContactRepository != null)
            {
                contacts = await ContactRepository.GetContactsAsync().ConfigureAwait(false);

                var servicesToDelete = contacts.Where(c => !services
                    .Select(s => s.ServiceId)
                    .Contains(c.ServiceId))
                    .Select(s => s.ServiceId)
                    .ToList();

               return await ContactRepository.DeleteContactData(servicesToDelete).ConfigureAwait(false);
            }

            return 0;
        }
    }
}
