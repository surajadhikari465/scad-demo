namespace Testing.Core
{
    public interface IObjectBuilderTemplate<T> where T : class
    {
        ObjectBuilder<T> BuildDefaults();
    }
}