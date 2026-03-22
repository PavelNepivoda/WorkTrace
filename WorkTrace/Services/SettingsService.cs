using WorkTrace.Data;

namespace WorkTrace.Services
{
    public class SettingsService : ISettingsService
    {
        private readonly ApplicationDbContext _context;

        public SettingsService(ApplicationDbContext context)
        {
            _context = context;
        }

        public double GetWorkingDayHours()
        {
            var setting = _context.SystemSettings.Find("WorkingDayHours");
            if (setting != null && double.TryParse(setting.Value, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out double hours))
            {
                return hours;
            }
            return 8;
        }
    }
}