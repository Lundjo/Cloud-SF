using DataRepository.tables;
using HealthCheckStatusUI.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace HealthCheckStatusUI.Controllers
{
    public class HomeController : Controller
    {
        private static HealthCheckRepository _healthCheckRepository = new HealthCheckRepository();

        public async Task<ActionResult> IndexAsync()
        {
            DateTime now = DateTime.UtcNow;
            DateTime oneHourAgo = now.AddHours(-1);
            DateTime twentyFourHoursAgo = now.AddHours(-24);

            int oneHourOkCount = await _healthCheckRepository.GetOkCheckCountAsync(oneHourAgo, now);
            int oneHourTotalCount = await _healthCheckRepository.GetCheckCountAsync(oneHourAgo, now);

            double oneHourRatio = oneHourTotalCount == 0 ? 0 : (double)oneHourOkCount / oneHourTotalCount;

            ViewBag.OneHourRatio = oneHourRatio * 100;

            List<double> twentyFourHoursRatios = new List<double>();

            for (int i = 0; i < 24; i++)
            {
                DateTime hourStartTime = DateTime.UtcNow.AddHours(-24 + i);
                DateTime hourEndTime = hourStartTime.AddHours(1);

                int hourOkCount = await _healthCheckRepository.GetOkCheckCountAsync(hourStartTime, hourEndTime);
                int hourTotalCount = await _healthCheckRepository.GetCheckCountAsync(hourStartTime, hourEndTime);

                double hourRatio = hourTotalCount == 0 ? 0 : (double)hourOkCount / hourTotalCount;

                twentyFourHoursRatios.Add(hourRatio);
            }

            ViewBag.TwentyFourHoursRatios = twentyFourHoursRatios;

            return View();
        }
    }
}
