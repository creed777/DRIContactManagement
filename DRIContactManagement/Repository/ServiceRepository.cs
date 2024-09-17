using Azure.Identity;
using DRIContactManagement.Models;
using Kusto.Data;
using Kusto.Data.Net.Client;
using Microsoft.EntityFrameworkCore;

namespace DRIContactManagement.Repository
{
    public class ServiceRepository : IServiceRepository
    {
        private readonly string clusterUri = "<kusto cluster url>";
        private AppDbContext AppDbContext { get; set; }

        public ServiceRepository(AppDbContext appDbContext)
        {
            AppDbContext = appDbContext;
        }

        public async Task<List<Service>> GetServiceData()
        {
            var kcsb = new KustoConnectionStringBuilder(clusterUri)
                .WithAadAzureTokenCredentialsAuthentication(new DefaultAzureCredential()); ;

            List<Service> services = new();
            //List<ServiceContact> serviceContacts = new();

            using (var kustoClient = KustoClientFactory.CreateCslQueryProvider(kcsb))
            {
                //Get All Services in HR Group division and give contact emails for each
                // Check for retired services that are part of the Public Azure cloud

                string database = "Shared";
                string query = @"cluster('<cluster name>
                | where DivisionId == 'f4aaa25f-95f6-4b3a-87ea-b7607feb7cba'
                | where Level in (""Service"")
                | join kind=leftouter (
                    cluster('<cluster name>
                ) on $left.ServiceId == $right.ServiceId
                | project
                            ServiceId,
                            ServiceName,
                            ServiceGroupName,
                            TeamGroupName,
                            DivisionName,
                            OrganizationName,
                            Created,
                            Modified
                | join kind=leftouter (
                    cluster('<cluster name>
                    | where Lifecycle == ""Retired""
                    | where AzureCloud == ""Public""
                ) on $left.ServiceId == $right.ServiceId
                | project
                          ServiceId,
                          ServiceName,
                          ServiceGroupName,
                          TeamGroupName,
                          DivisionName,
                          OrganizationName,
                          Created,
                          Modified,
                          Lifecycle";

                using (var response = await kustoClient.ExecuteQueryAsync(database, query, null))
                {
                    int columnServiceId = response.GetOrdinal("ServiceId");
                    int columnServiceName = response.GetOrdinal("ServiceName");
                    int columnOrganizationName = response.GetOrdinal("OrganizationName");
                    int columnDivisionName = response.GetOrdinal("DivisionName");
                    int columnServiceGroupName = response.GetOrdinal("ServiceGroupName");
                    int columnTeamGroupName = response.GetOrdinal("TeamGroupName");

                    while (response.Read())
                    {
                        var service = new Service
                        {
                            ServiceId = response.GetString(columnServiceId),
                            ServiceName = response.GetString(columnServiceName),
                            OrganizationName = response.GetString(columnOrganizationName),
                            DivisionName = response.GetString(columnDivisionName),
                            ServiceGroupName = response.GetString(columnServiceGroupName),
                            TeamGroupName = response.GetString(columnTeamGroupName)
                        };
                        services.Add(service);
                    }
                }
            }
            return services;
        }
        public async Task<bool> AddServiceDataToContacts(List<string> serviceIds)
        {
            List<Contact> ContactList = new();

            foreach (var service in serviceIds)
            {
                var result = new Contact() { ServiceId = service };
                ContactList.Add(result);
            }

            if (ContactList != null)
            {
                await AppDbContext.Contact.AddRangeAsync(ContactList).ConfigureAwait(false);
                await AppDbContext.SaveChangesAsync().ConfigureAwait(false);
                return true;
            }
            else
                return false;
        }
    }
}
