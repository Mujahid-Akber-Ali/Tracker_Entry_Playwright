using Microsoft.Playwright;
using Microsoft.Playwright.NUnit;

namespace Tracker_Entry
{
    public class Tests : PageTest
    {
        private static IEnumerable<TestCaseData> TestData()
        {
            string csvFilePath = @"C:\Users\DELL\Desktop\tracker.csv";
            var lines = File.ReadAllLines(csvFilePath).Skip(1); // Skip header line
            foreach (var line in lines)
            {
                var values = line.Split(',');
                string username = values[0];
                string email = values[1];
                yield return new TestCaseData(username,email);
            }
        }

        [Test]
        [TestCaseSource(nameof(TestData))]
        public async Task Test1(string username,string email)
        {
            await Page.GotoAsync("https://surveillance.utopiadeals.com/surveillance/login");
            await Write("xpath=//html/body/div/div/div/form/div[1]/input", "mujahid.akber@utopia.pk", "Add Email");
            await Write("xpath=//html/body/div/div/div/form/div[2]/input", "Utopia01", "Add PAssword");
            await Click("xpath=//html/body/div/div/div/form/div[3]/div/button","Login");

            await Page.GotoAsync("https://surveillance.utopiadeals.com/surveillance/tracker-dashboard/users?userId=&name=&hubstaff-status=&tracker-status=&count=100");
            await Click("xpath=//html/body/div[1]/div[2]/div/div/div[1]/div/div/button","Add User");
            await Write("xpath=//html/body/div[2]/div/div/div[2]/div[4]/input", username,"Add Username");
            await Write("xpath=//html/body/div[2]/div/div/div[2]/div[5]/input", email, "Add Username");
            await Click("xpath=//html/body/div[2]/div/div/div[2]/h6[2]/div/div[1]/input", "Roles");
            await Press("xpath=//html/body/div[2]/div/div/div[2]/div[5]/input", "Enter");
            await Click("xpath=//html/body/div[2]/div/div/div[3]/button[2]", "Click Enter");


        }

        public async Task Click(string locator, string detail)
        {
            try
            {
                await Page.ClickAsync(locator);
                //  await ExtentReport.TakeScreenshot(Page, Status.Pass, detail);
                Thread.Sleep(1000);
            }
            catch (Exception ex)
            {
                //await ExtentReport.TakeScreenshot(Page, Status.Fail, "Click Failed" + ex);
            }
        }

        public async Task Write(string locator, string data, string detail)
        {
            try
            {
                await Page.FillAsync(locator, data);
                //await ExtentReport.TakeScreenshot(Page, Status.Pass, detail);
                Thread.Sleep(1000);
            }
            catch (Exception ex)
            {
                //await ExtentReport.TakeScreenshot(Page, Status.Fail, "Entry Failed" + ex);
            }
        }

        public async Task Press(string locator, string key)
        {
            try
            {
                await Page.PressAsync(locator, key);
                Thread.Sleep(1000);
            }
            catch (Exception ex)
            {
                // await ExtentReport.TakeScreenshot(Page, Status.Fail, "Click Failed" + ex);
            }
        }

        public async Task Popup(string locator, string dec)
        {
            var dialogTaskCompletionSource = new TaskCompletionSource<IDialog>();

            // Event listener to capture dialog creation
            Page.Dialog += (_, dialog) =>
            {
                dialogTaskCompletionSource.TrySetResult(dialog);
            };

            // Click on something that might trigger a dialog
            await Page.ClickAsync(locator);

            // Wait for the dialog to be created
            var dialog = await dialogTaskCompletionSource.Task;

            try
            {
                if (dec == "Accept")
                {
                    await dialog.AcceptAsync();
                }
                else
                {
                    await dialog.DismissAsync();
                }


                await Page.WaitForTimeoutAsync(3000);


            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while handling the dialog: {ex.Message}");
                // Handle the exception gracefully if needed
            }

        }
    }
}