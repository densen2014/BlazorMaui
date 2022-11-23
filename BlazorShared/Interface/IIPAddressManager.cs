namespace BlazorShared.Services
{
    public interface IIPAddressManager
    {
        string? FirstMacAddress();
        string? GetIPAddress();
        List<string>? GetIPAddresList();
        string? GetVersion();
        string? GetIdentifier();

    }
}
