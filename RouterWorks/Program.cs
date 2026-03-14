// See https://aka.ms/new-console-template for more information
using Router.Tasks;

string disclaimer = $@"Disclaimer
Use of this product/service is at your own risk. No warranty is provided, express or implied, including but not limited to warranties of merchantability or fitness for a particular purpose. 
The provider assumes no responsibility for any data loss, corruption, or other damages that may occur. 
By using this product/service, you acknowledge and accept these terms.

Please enter y/Y to continue";
Console.WriteLine(disclaimer);


int key = Console.Read();
bool _key = key == 89 || key == 121;
if (!_key)
{
    return;
}

var domains = new List<string>
{
    "instagram.com",
    "i.instagram.com",
    "facebook.com",
    "api.facebook.com",
    "graph.facebook.com",
    "b-graph.facebook.com",
    "lookaside.facebook.com",
    "mobile.facebook.com",
    "googleads.g.doubleclick.net",
    "www.googleadservices.com",
    "static.xx.fbcdn.net"
};

Action<Exception> logEx = (ex) =>
{
    string error = ex.ToString();
    Console.WriteLine(error);
};
TaskPageDI router = new TaskPageDI(logEx);

IPAddressCollector iPAddressCollector = new IPAddressCollector();
Blockdomains blockdomains = new Blockdomains(iPAddressCollector);
var routerConfig = new RouterConfig 
{
    Ip = "192.168.0.1",
    RouterPassword = "-pwd-",
    RemoveOld = true,
};
RouterBlock routerBlock = new RouterBlock(blockdomains, router, routerConfig);

Action<string> log = txt =>
{
    Console.WriteLine($"{txt}");
};

routerBlock.ProcessAsync(domains, log).Wait();
Console.WriteLine($"Done. Please any key to exit");
Console.Read();