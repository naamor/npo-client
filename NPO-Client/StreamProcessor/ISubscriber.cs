using System.Threading.Tasks;

namespace NPO_Client.StreamProcessor
{
    public interface ISubscriber
    {
        Task Subscribe();
        Task Unsubscribe();
        Task Disconnect();
        void Dispose();
    }
}
