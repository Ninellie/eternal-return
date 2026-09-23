namespace EternalReturn.Burnout
{
    public interface ISlowableByBurnout
    {
        bool IsSlowed { get; }

        void Slow();

        void Unslow();
    }
}
