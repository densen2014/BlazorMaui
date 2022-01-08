namespace BlazorShared.Services
{
    public interface IIPAddressManager
    {
        string firstMacAddress();
        string GetIPAddress();
        List<string> GetIPAddresList();
        string GetVersion();
        string GetIdentifier();

    }
}
