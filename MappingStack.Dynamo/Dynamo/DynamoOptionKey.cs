namespace MappingStack.Dynamo
{
    public class DynamoOptionKey
    {
        public string key { get; }
        public string jsonId { get; }

        public DynamoOptionKey(string key, string jsonId)
        {
            this.key = key;
            this.jsonId = jsonId;
        }
    }
}
