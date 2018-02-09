using Mammoth.Common;
using System.Collections.Generic;
using System.Linq;

namespace MammothWebApi.Common
{
    public class PrimeAffinityPsgSettings : IPrimeAffinityPsgSettings
    {
        public bool EnablePrimeAffinityPsgMessages => AppSettingsAccessor.GetBoolSetting(nameof(EnablePrimeAffinityPsgMessages));
        public List<int> ExcludedPsNumbers => AppSettingsAccessor.GetStringSetting(nameof(ExcludedPsNumbers))
            .Split(',')
            .Select(s => int.Parse(s.Trim()))
            .ToList();
    }
}
