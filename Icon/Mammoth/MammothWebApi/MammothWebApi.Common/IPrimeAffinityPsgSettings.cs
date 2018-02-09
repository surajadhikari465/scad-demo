using System.Collections.Generic;

namespace MammothWebApi.Common
{
    public interface IPrimeAffinityPsgSettings
    {
        bool EnablePrimeAffinityPsgMessages { get; }
        List<int> ExcludedPsNumbers { get; }
    }
}
