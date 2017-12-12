namespace MappingStack.Dynamo.BaseDto
{
    using KeyAttribute = System.ComponentModel.DataAnnotations.KeyAttribute;  // TODO: eliminate it ??
    public class MultiEntityDynamicPropertyGeneral<T> : EntityDynamicPropertyGeneral<T> 
//        : IDynamicOptionGeneral<T>
//        , IDynamicOptionGeneral
    {
        [Key] public string entityKey { get; set; }
//        [Key] public int    entityId { get; set; }
//        [Key] public string optionKey { get; set; }
//        public T      value { get; set; }

    }
}
