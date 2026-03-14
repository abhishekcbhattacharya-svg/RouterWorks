# RouterWorks
A playwright based automation to block ip address on TPLink router. Set your list of blocked domains and execute

# Open Program.cs
`var domains = new List<string>
{
    "instagram.com",
    "facebook.com"
};`

Add / remove / Change domains for website block

`var routerConfig = new RouterConfig 
{
    Ip = "192.168.0.1",
    RouterPassword = "-pwd-",
};`

Modify IP address to match your router, apply your router password

# Prerequisites
1. Visual studio 2022 with .NET 8 installation.
2. Playwright for browser automation support
