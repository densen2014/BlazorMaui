using BlazorShared.Services;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Runtime.Versioning;
#if (ANDROID)
using System.Net;
#endif
#if (IOS || MACCATALYST)
using Security;
#endif


namespace Shared.DependencyServices
{
    public class IPAddressManager : IIPAddressManager
    {
        [UnsupportedOSPlatform("browser")]
        public string? FirstMacAddress()
        {
            try
            {                
                return NetworkInterface
                       .GetAllNetworkInterfaces()
                       .Where(nic => nic.OperationalStatus == OperationalStatus.Up && nic.NetworkInterfaceType != NetworkInterfaceType.Loopback)
                       .Select(nic => nic.GetPhysicalAddress().ToString())
                       .FirstOrDefault()?? "UnknowMacAddress";
            }
            catch (Exception)
            {
                return "UnknowMacAddress";
            }

        }


#if (ANDROID)
        public string? GetIPAddress()
        {
            IPAddress[] adresses = Dns.GetHostAddresses(Dns.GetHostName());

            if (adresses != null && adresses[0] != null)
            {
                return adresses[0].ToString();
            }
            else
            {
                return null;
            }
        }
        public List<string> GetIPAddresList()
        {
            IPAddress[] adresses = Dns.GetHostAddresses(Dns.GetHostName());
            return adresses?.Where(a => a != null).Select(a => a.ToString()).ToList();

        }


#else

        [UnsupportedOSPlatform("browser")]
        public string? GetIPAddress()
        {
            string ipAddress = "";

            try
            {
                foreach (var netInterface in NetworkInterface.GetAllNetworkInterfaces())
                {
                    if (netInterface.NetworkInterfaceType == NetworkInterfaceType.Wireless80211 ||
                        netInterface.NetworkInterfaceType == NetworkInterfaceType.Ethernet)
                    {
                        foreach (var addrInfo in netInterface.GetIPProperties().UnicastAddresses)
                        {
                            if (addrInfo.Address.AddressFamily == AddressFamily.InterNetwork)
                            {
                                ipAddress = addrInfo.Address.ToString();

                            }
                        }
                    }
                }
            }
            catch (Exception)
            {
                return "UnknowIpAddress";
            }

            return ipAddress;
        }

        [UnsupportedOSPlatform("browser")]
        public List<string> GetIPAddresList()
        {
            var ipAddress = new List<string>();

            try
            {
                foreach (var netInterface in NetworkInterface.GetAllNetworkInterfaces())
                {
                    if (netInterface.NetworkInterfaceType == NetworkInterfaceType.Wireless80211 ||
                        netInterface.NetworkInterfaceType == NetworkInterfaceType.Ethernet)
                    {
                        foreach (var addrInfo in netInterface.GetIPProperties().UnicastAddresses)
                        {
                            if (addrInfo.Address.AddressFamily == AddressFamily.InterNetwork)
                            {
                                ipAddress.Add(addrInfo.Address.ToString());

                            }
                        }
                    }
                }
            }
            catch (Exception)
            {
                ipAddress.Add("UnknowIpAddress");
            }

            return ipAddress;
        }
#endif

#if (ANDROID)
        public string? GetIdentifier()
        {
            return Android.Provider.Settings.Secure.GetString(Android.App.Application.Context.ContentResolver, Android.Provider.Settings.Secure.AndroidId);
        }

        public string? GetVersion()
        {
            var activity = Android.App.Application.Context;
            return activity.PackageManager.GetPackageInfo(activity.PackageName, 0).VersionName;
        }
#else
#if (IOS || MACCATALYST)
        public string? GetIdentifier()
        {
            var query = new SecRecord(SecKind.GenericPassword);
            query.Service = NSBundle.MainBundle.BundleIdentifier;
            query.Account = "UniqueID";

            NSData? uniqueId = SecKeyChain.QueryAsData(query);
            if (uniqueId == null)
            {
                query.ValueData = NSData.FromString(System.Guid.NewGuid().ToString());
                var err = SecKeyChain.Add(query);
                if (err != SecStatusCode.Success && err != SecStatusCode.DuplicateItem)
                    throw new Exception("Cannot store Unique ID");

                return query.ValueData.ToString();
            }
            else
            {
                return uniqueId.ToString();
            }
        }

        public string? GetVersion()
        {
            return NSBundle.MainBundle.InfoDictionary["CFBundleShortVersionString"].ToString();
        }
#else
        [UnsupportedOSPlatform("browser")]
        public string? GetIdentifier()
        {
            try
            {
                var macAddress = NetworkInterface
                               .GetAllNetworkInterfaces()
                               .Where(nic => nic.OperationalStatus == OperationalStatus.Up && nic.NetworkInterfaceType != NetworkInterfaceType.Loopback && (nic.NetworkInterfaceType == NetworkInterfaceType.Wireless80211 ||
                                                   nic.NetworkInterfaceType == NetworkInterfaceType.Ethernet))
                               .Select(nic => nic.GetPhysicalAddress())
                               .FirstOrDefault();
                if (macAddress==null) return "UnknowIdentifier";
                return string.Join(":", (from ma in macAddress.GetAddressBytes() select ma.ToString("X2")).ToArray());
            }
            catch (Exception)
            {
                return "UnknowIdentifier";
            }
        }

        public string? GetVersion()
        {
            return "1.17";
        }

#endif
#endif

        //[SupportedOSPlatform("linux")]
        //https://docs.microsoft.com/zh-cn/dotnet/fundamentals/code-analysis/quality-rules/ca1416

    }


}
