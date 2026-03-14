using Microsoft.Playwright;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Router.Tasks
{
    public class TaskPage
    {
        public async Task Execute() 
        {
            // Initialize Playwright and launch browser
            using var playwright = await Playwright.CreateAsync();
            await using var browser = await playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions
            {
                Headless = false // Set to true if you don't want to see the browser
            });

            // Create a new browser context and page
            var context = await browser.NewContextAsync();
            var page = await context.NewPageAsync();

            // Navigate to the login page
            await page.GotoAsync("https://example.com/login");

            // Fill in login credentials
            await page.FillAsync("#username", "yourUsername");
            await page.FillAsync("#password", "yourPassword");

            // Click the login button
            await page.ClickAsync("#loginButton");

            // Wait for navigation or some element that confirms login
            await page.WaitForURLAsync("https://example.com/dashboard");

            // Optional: Take a screenshot after login
            //await page.ScreenshotAsync(new PageScreenshotOptions { Path = "loggedin.png" });

            // Close browser
            await browser.CloseAsync();
        }
    }
}
