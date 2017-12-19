using System.Collections.Generic;

namespace MappingStack.Dynamo.Testing.TestFactory.DtoModel
{
    public class SampleDto
    {
        public int id { get; set; }

        public string title { get; set; }

        public EnumScalar? @enum { get; set; }

        public InnerDto inner { get; set; }

        public ICollection<CollectionElementDto> collection { get; set; }

        public DynamoDto dynamo { get; set; }
    }
}
