namespace Mammoth.PrimeAffinity.Library.Processors
{
    public interface IPrimeAffinityPsgProcessor<TParameters>
    {
        void SendPsgs(TParameters parameters);
    }
}
