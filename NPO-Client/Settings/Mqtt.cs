namespace NPO_Client.Settings
{
    public class Mqtt : IStreamProcessorSettings
    {
        public string Host { get; set; }
        public int Port { get; set; }
        public string Topic { get; set; }
    }
}
