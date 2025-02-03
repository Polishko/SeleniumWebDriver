using System.Collections.ObjectModel;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace TestProject2
{
    [TestFixture]
    public class WorkingWithWebTable
    {
        IWebDriver driver;

        [SetUp]
        public void SetUp()
        {
            var options = new ChromeOptions();

            // Use a unique temporary directory for each session to prevent conflicts
            string tempUserDataDir = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
            options.AddArgument($"--user-data-dir={tempUserDataDir}");

            // Recommended options for CI/CD environments
            options.AddArgument("--headless");  // Run in headless mode for CI
            options.AddArgument("--no-sandbox");  // Required for some Linux environments
            options.AddArgument("--disable-dev-shm-usage");  // Avoid shared memory issues in CI

            driver = new ChromeDriver(options);
        }

        [Test]
        public void TestExtractProductInformation()
        {
            // Launch Chrome browser with the given URL
            driver.Url = "http://practice.bpbonline.com/";

            // Identify the web table
            IWebElement productTable = driver.FindElement(By.XPath("//*[@id='bodyContent']/div/div[2]/table"));

            // Find the number of rows
            ReadOnlyCollection<IWebElement> tableRows = productTable.FindElements(By.XPath("//tbody/tr"));

            // Path to save the CSV file
            string path = System.IO.Directory.GetCurrentDirectory() + "/productinformation.csv";

            // If the file exists in the location, delete it
                        if (File.Exists(path))
                File.Delete(path);

            // Traverse through table rows to find the table columns
               foreach (IWebElement trow in tableRows)
            {
                ReadOnlyCollection<IWebElement> tableCols = trow.FindElements(By.XPath("td"));
                foreach (IWebElement tcol in tableCols)
                {
                    // Extract product name and cost
                    String data = tcol.Text;
                    String[] productinfo = data.Split('\n');
                    String printProductinfo = productinfo[0].Trim() + "," + productinfo[1].Trim() + "\n";

                    // Write product information extracted to the file
                    File.AppendAllText(path, printProductinfo);
                }
            }

            // Verify the file was created and has content
            Assert.IsTrue(File.Exists(path), "CSV file was not created");
            Assert.IsTrue(new FileInfo(path).Length > 0, "CSV file is empty");
        }

        [TearDown]
        public void TearDown()
        {
            // Quit the driver
            if (driver != null)
            {
                driver.Quit();  // Ensures ChromeDriver process is completely terminated
            }
        }
    }
}