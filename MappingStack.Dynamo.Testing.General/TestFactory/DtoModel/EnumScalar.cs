namespace MappingStack.Dynamo.Testing.TestFactory.DtoModel
{
    /// <summary>
    /// Enum cannot be nested, otherwise it is not resolved on calling .ApplyTo(...)
    /// </summary>
    public enum EnumScalar
    {
        EnumScalarFirst,
        EnumScalarSecond,
        EnumScalarThird,
    }
}
