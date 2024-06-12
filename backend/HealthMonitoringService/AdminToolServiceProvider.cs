using DataRepository.tables.entities;
using DataRepository.tables;
using DataRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthMonitoringService
{
    public class AdminToolServiceProvider
    {
        private static AlertEmailRepository repository = new AlertEmailRepository();

        public async Task<bool> AddEmail(AlertEmailDTO alertEmail)
        {
            return await repository.CreateAsync(new AlertEmail(alertEmail.Email, alertEmail.Id));
        }

        public async Task<List<AlertEmailDTO>> ReadAllEmails()
        {
            var alertEmails = await repository.ReadAllAsync();
            return alertEmails.Select(alertEmail => new AlertEmailDTO(alertEmail.RowKey, alertEmail.Email)).ToList();
        }

        public async Task<bool> RemoveEmail(string id)
        {
            return await repository.DeleteAsync(id);
        }
    }
}
