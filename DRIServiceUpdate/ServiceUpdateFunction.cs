using DRIContactManagement;
using DRIContactManagement.Models;
using DRIContactManagement.Repository;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace DRIServiceUpdate
{
    public class ServiceUpdateFunction
    {
        private readonly ILogger _logger;
        private readonly AppDbContext DbContext;
        private readonly IServiceRepository ServiceRepository;
        private readonly IContactRepository ContactRepository;

        public ServiceUpdateFunction(ILoggerFactory loggerFactory, AppDbContext dbContext, IContactRepository contactRepository, IServiceRepository serviceRepository)
        {
            _logger = loggerFactory.CreateLogger<ServiceUpdateFunction>();
            DbContext = dbContext;
            ContactRepository = contactRepository;
            ServiceRepository = serviceRepository;
        }

        //TimerTrigger("0 */10 * * *"  every 10 min
        [Function("ServiceUpdateFunction")]
        public async Task Run([ TimerTrigger("0 0 * * *"  
            #if DEBUG
            ,RunOnStartup = true
            #endif
            )] TimerInfo myTimer)
        {

            _logger.LogInformation($"C# Timer trigger function executed at: {DateTime.Now}");
            var contacts = await GetContactServiceIds().ConfigureAwait(false);
            var services = await GetServices().ConfigureAwait(false);
            var missingIds = services.Where(s => !contacts
                .Contains(s.ServiceId))
                .Select(s => s.ServiceId)
                .ToList();

            if (missingIds.Any())
            {
                var result = await ServiceRepository.AddServiceDataToContacts(missingIds).ConfigureAwait(false);
                if (result == true)
                {
                    _logger.LogInformation($"The following service id's were added: {missingIds}");
                }
            }

            var serviceServiceIds = services.Select(x => x.ServiceId).ToList();
            var servicesToDelete = contacts.Except(serviceServiceIds).ToList();
            var deletes = await ContactRepository.DeleteContactData(servicesToDelete);

            if (myTimer.ScheduleStatus is not null)
            {
                _logger.LogInformation($"Next timer schedule at: {myTimer.ScheduleStatus.Next}");
            }
        }

        public async Task<List<Service>> GetServices()
        {
            return await ServiceRepository.GetServiceData().ConfigureAwait(false);
        }

        public async Task<List<string>> GetContactServiceIds()
        {
            return await ContactRepository.GetServiceIds().ConfigureAwait(false);
        }
    }
}
