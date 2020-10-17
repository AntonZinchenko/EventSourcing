namespace SeedWorks.Validation
{
    public interface IBusinessRule
    {
        bool IsBroken();

        string Message { get; }
    }
}
