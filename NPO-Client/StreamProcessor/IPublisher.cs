using System.Threading.Tasks;

namespace NPO_Client.StreamProcessor
{
    public interface IPublisher
    {
        Task Publish(byte[] message);
    }
}
