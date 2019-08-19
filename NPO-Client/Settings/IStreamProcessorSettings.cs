namespace NPO_Client.Settings
{
    interface IStreamProcessorSettings
    {
        string Host { get; set; }
        int Port { get; set; }
        string Topic { get; set; }
    }
}
