using System;
using System.Windows.Forms;

namespace Kat_UNIT21
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // Create instances of required classes
            IXmlParser xmlParser = new XmlDonationParser();
            ICsvFileLocator csvFileLocator = new CsvFileLocator();
            ICsvParser csvParser = new CsvDonationParser(csvFileLocator); // Pass CsvFileLocator instance to CsvDonationParser constructor

            // Pass instances to Form1 constructor
            Application.Run(new Form1(xmlParser, csvParser));
        }
    }
}
