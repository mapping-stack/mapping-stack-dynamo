namespace MappingStack.Dynamo.Testing.TestFactory
{
    using System.Collections.Generic;
    using System.Linq;

    using MappingStack.Dynamo.BaseDto;

    public class DynamoDto
    {
        public IDictionary<string, object> _dyn // to be exposed dynamically
            // { get; set; }
            => dynBool.ToDictionary(
                _ => AdjustOptionKey(_.dynamicKey)
                , _ => (object) _.value
                , EqualityComparer<string>.Default);

        public static System.Func<string, string> AdjustOptionKey => _ => _
            .Replace("prefix_", "") // ad-hoc method adjusting keys                                                             - do we need a (an explicit) reversing method ??
        // .Replace("-", "_")      // to be implemented by a special (extendable/with callback to extend ??) key-id mapping - do we need a (an explicit) reversing method ??
        ;

        public ICollection<DynamicBool> dynBool { get; set; }
    }
}
