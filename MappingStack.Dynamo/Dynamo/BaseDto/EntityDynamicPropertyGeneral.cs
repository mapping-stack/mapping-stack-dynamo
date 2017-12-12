namespace MappingStack.Dynamo.BaseDto
{
    using KeyAttribute = System.ComponentModel.DataAnnotations.KeyAttribute;  // TODO: eliminate it ?? needed for OData exposing related IEdmModel
    public class EntityDynamicPropertyGeneral<T> 
        : IDynamicPropertyGeneral<T>
        , IDynamicPropertyGeneral
    {
        [Key] public int    entityId { get; set; }
        [Key] public string dynamicKey { get; set; }
        public T      value { get; set; }
        dynamic IDynamicPropertyGeneral.value
        {
            get => value;
            set => this.value = value;
        }
    }
//    public class DynamicProperty   // : EntityDynamicPropertyGeneral<dynamic>  // ??
//    {
//        [Key] public int entityId { get; set; }
//        [Key] public string dynamicKey { get; set; }
//        // public object value { get; set; }
//        public dynamic value { get; set; }
//    }


    public class DynamicBool  : EntityDynamicPropertyGeneral<bool  > {}
    public class DynamicInt   : EntityDynamicPropertyGeneral<int   > {}
    public class DynamicFloat : EntityDynamicPropertyGeneral<double> {}
}
