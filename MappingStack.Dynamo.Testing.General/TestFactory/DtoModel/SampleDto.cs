using MappingStack.Dynamo.Testing.TestFactory.DtoModel;

namespace MappingStack.Dynamo.Testing.TestFactory
{
    public class SampleDto
    {
        public int id { get; set; }

        public string title { get; set; }

        public EnumScalar? @enum { get; set; }

        public InnerDto inner { get; set; }

        public DynamoDto dynamo { get; set; }
    }
}
