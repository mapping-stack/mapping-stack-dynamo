namespace MappingStack.Dynamo.Testing.TestFactory
{
    public partial class ModelFactory
    {
        public class Dto
        {
            public int id { get; set; }

            public string title { get; set; }

            public InnerDto inner { get; set; }

            public DynamoDto dynamo { get; set; }
        }
    }
}
