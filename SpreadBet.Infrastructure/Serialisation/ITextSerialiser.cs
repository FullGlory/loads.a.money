namespace SpreadBet.Infrastructure.Serialisation
{
    using System.IO;

    public interface ITextSerialiser
    {
        void Serialize(TextWriter writer, object graph);
        object Deserialize(TextReader reader);
    }
}
