using Microsoft.Playwright;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Router.Tasks
{
    public class RouterBlock
    {
        private readonly Blockdomains blockdomains;
        private readonly TaskPageDI taskPageDI;
        private readonly RouterConfig routerConfig;
        public RouterBlock(Blockdomains blockdomains, TaskPageDI taskPageDI, RouterConfig routerConfig)
        {
            this.blockdomains = blockdomains;
            this.taskPageDI = taskPageDI;
            this.routerConfig = routerConfig;
        }
        public async Task ProcessAsync(List<string> domains, Action<string> log)
        {
            Dictionary<string, string> blockIPs = blockdomains.Domains([.. domains]);
            log?.Invoke($"IP addresses retrived");
            foreach (var ipAddress in blockIPs)
            {
                log?.Invoke($"{ipAddress.Key} - {ipAddress.Value}");
            }
            
            Func<IPage, Task> adavancedSettings = async (page) =>
            {
                await _AddToBlock(page, blockIPs);
            };

            await taskPageDI.Execute(_Login, adavancedSettings);
            log?.Invoke($"Browser process completed");
        }


        private async Task _Login(IPage page)
        {
            // Navigate to the login page
            await page.GotoAsync($"http://{routerConfig.Ip}/");

            await page.FillAsync("input.text-text.password-text.password-hidden", routerConfig.RouterPassword);

            // Click the login button
            await page.ClickAsync("a.button-button");
            //desktop
            await page.WaitForURLAsync($"http://{routerConfig.Ip}/#networkMap");
        }

        private async Task _AddToBlock(IPage page, Dictionary<string, string> blockIPs)
        {
            // Navigate to the login page
            await page.GotoAsync($"http://{routerConfig.Ip}/#routingAdv");

            //div#static-routing-grid-panel div.grid-content-container a.grid-content-btn.grid-content-btn-delete.btn-delete
            //div#static-routing-grid-panel a.grid-content-btn.grid-content-btn-delete.btn-delete
            if (routerConfig.RemoveOld)
            {
                await _DeleteOld(page);
            }

            List<string> ipAddresses = await _AllExistingIPs(page);

            foreach (var blockIP in blockIPs)
            {
                if (ipAddresses.Contains(blockIP.Key))
                {
                    continue;
                }

                await _AddIPAddress(page, blockIP);
                await Task.Delay(1000);
                await page.ReloadAsync();
            }
        }

        private async Task _DeleteOld(IPage page)
        {
            bool tryDelete = true;
            while (tryDelete)
            {
                await Task.Delay(2000);
                //await page.ReloadAsync();
                var deleteHandle = await page.QuerySelectorAsync("div#static-routing-grid-panel a.grid-content-btn.grid-content-btn-delete.btn-delete");
                tryDelete = deleteHandle != null;
                if (deleteHandle != null)
                {
                    await deleteHandle.ClickAsync();
                }
            }
        }

        private async Task<List<string>> _AllExistingIPs(IPage page)
        {
            await Task.Delay(1000);
            var matches = await page.QuerySelectorAllAsync("div#static-routing-grid-panel tr td:nth-child(2)>div.td-content");

            var ipAddresses = new List<string>();
            foreach (var match in matches)
            {
                var ip = await match.InnerTextAsync();
                ipAddresses.Add(ip);
            }

            return ipAddresses;
        }

        private async Task _AddIPAddress(IPage page, KeyValuePair<string, string> blockIP)
        {
            //operation-btn btn-add fst lst
            await page.ClickAsync("a.operation-btn.btn-add.fst.lst");

            //add ip address
            //div#static-routing-config>div:nth-child(1) input - ip address
            await page.FillAsync("div#static-routing-config>div:nth-child(1) input", blockIP.Key);
            //div#static-routing-config>div:nth-child(2) input - 255.255.255.255
            await page.FillAsync("div#static-routing-config>div:nth-child(2) input", "255.255.255.255");
            //div#static-routing-config>div:nth-child(3) input - 192.168.0.1
            await page.FillAsync("div#static-routing-config>div:nth-child(3) input", routerConfig.Ip);
            //div#static-routing-config>div:nth-child(5) input - domain
            await page.FillAsync("div#static-routing-config>div:nth-child(5) input", blockIP.Value);

            //div#static-routing-config>div:nth-child(4) input - LAN/WLAN
            //await page.FillAsync("div#static-routing-config>div:nth-child(4) input.combobox-text", "LAN/WLAN");

            await page.ClickAsync("div#static-routing-config>div:nth-child(4) a.combobox-switch");

            var optionHandles = await page.QuerySelectorAllAsync("div.combobox-list-wrap li.combobox-list");
            foreach (var option in optionHandles)
            {
                var textH = await option.QuerySelectorAsync("label.combobox-label.single span.text");
                var text = await textH.InnerTextAsync();
                if (text == "LAN/WLAN")
                {
                    var box = await option.BoundingBoxAsync();
                    if (box != null)
                    {
                        // Click at the center of the element
                        await page.Mouse.ClickAsync(
                            box.X + box.Width / 2,
                            box.Y + box.Height / 2
                        );
                    }

                    break;
                }
            }

            //div#static-routing-grid-save-button a.button-button
            await page.ClickAsync("div#static-routing-grid-save-button a.button-button");
        }

    }
}