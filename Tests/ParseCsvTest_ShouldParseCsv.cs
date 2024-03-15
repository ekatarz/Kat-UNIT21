using Microsoft.VisualStudio.TestTools.UnitTesting;
using Kat_UNIT21;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Kat_UNIT21.Tests
{
    [TestClass]
    public class CsvDonationParserTests
    {
        private string tempCsvFilePath;

        [TestInitialize]
        public void Initialize()
        {
            // Create a temporary CSV file for testing
            tempCsvFilePath = Path.GetTempFileName();
            var csvContent = new StringBuilder();
            csvContent.AppendLine("PersonID,PartyAffiliation,AmountDonated");
            csvContent.AppendLine("1,Party A,0");
            csvContent.AppendLine("2,Party B,0");
            File.WriteAllText(tempCsvFilePath, csvContent.ToString());
        }

        [TestMethod]
        public void ParseCsv_ShouldCorrectlyParseCsv()
        {
            // Arrange
            var csvFileLocator = new CsvFileLocator(); // Assuming CsvFileLocator is adjusted to return tempCsvFilePath for the purpose of the test
            var csvParser = new CsvDonationParser(csvFileLocator);

            // Act
            var donations = csvParser.ParseCsv(tempCsvFilePath).ToList();

            // Assert
            Assert.IsNotNull(donations);
            Assert.AreEqual(2, donations.Count);

            var firstDonation = donations[0];
            Assert.AreEqual("1", firstDonation.PersonID);
            Assert.AreEqual("Party A", firstDonation.PartyAffiliation);
            Assert.AreEqual(100.00m, firstDonation.AmountDonated);

            var secondDonation = donations[1];
            Assert.AreEqual("2", secondDonation.PersonID);
            Assert.AreEqual("Party B", secondDonation.PartyAffiliation);
            Assert.AreEqual(200.00m, secondDonation.AmountDonated);
        }

        [TestCleanup]
        public void Cleanup()
        {
            // Delete the temporary CSV file after the test
            if (File.Exists(tempCsvFilePath))
            {
                File.Delete(tempCsvFilePath);
            }
        }
    }
}
