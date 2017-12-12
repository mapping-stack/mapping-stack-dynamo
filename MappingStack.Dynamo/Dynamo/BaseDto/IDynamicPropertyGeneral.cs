namespace MappingStack.Dynamo.BaseDto
{
    public interface IDynamicPropertyGeneral
    {
        string dynamicKey { get; set; }
        dynamic value { get; set; }
    }
    public interface IDynamicPropertyGeneral<T>
    {
        string dynamicKey { get; set; }
        T value { get; set; }
    }
}
