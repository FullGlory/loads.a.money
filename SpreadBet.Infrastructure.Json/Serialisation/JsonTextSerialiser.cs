namespace SpreadBet.Infrastructure.Json.Serialisation
{
    using System.IO;
    using Newtonsoft.Json;
    using SpreadBet.Infrastructure.Serialisation;

    public class JsonTextSerialiser : ITextSerialiser
    {
        private readonly JsonSerializer _serialiser;

        public JsonTextSerialiser()
        {
            this._serialiser = JsonSerializer.Create(
                new JsonSerializerSettings
                {
                    // Allows deserializing to the actual runtime type                
                    TypeNameHandling = TypeNameHandling.All,
                    // In a version resilient way                
                    TypeNameAssemblyFormat = System.Runtime.Serialization.Formatters.FormatterAssemblyStyle.Simple
                });
        }

        public void Serialize(TextWriter writer, object graph)
        {
            var jsonWriter = new JsonTextWriter(writer);
            this._serialiser.Serialize(jsonWriter, graph);
            writer.Flush();
        }

        public object Deserialize(TextReader reader)
        {
            var jsonReader = new JsonTextReader(reader);

            return this._serialiser.Deserialize(jsonReader);
        }
    }
}
