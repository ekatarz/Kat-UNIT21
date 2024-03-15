using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace Kat_UNIT21
{
    public partial class Form1 : Form
    {
        private readonly IXmlParser _xmlParser;
        private readonly ICsvParser _csvParser;

        public Form1(IXmlParser xmlParser, ICsvParser csvParser)
        {
            InitializeComponent();
            _xmlParser = xmlParser;
            _csvParser = csvParser;
            gridContributions.AutoGenerateColumns = true;
            LoadData();
        }

        private void LoadData()
        {
            try
            {
                // load and parse xml
                var xmlUrl = "https://www.theyworkforyou.com/pwdata/scrapedxml/regmem/regmem2021-12-13.xml";
                var xmlDonations = _xmlParser.ParseXml(xmlUrl).ToList();

                // load csv from bin folder
                var csvFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "mps.csv");
                var csvDonations = _csvParser.ParseCsv(csvFilePath).ToList(); // passing file path to parseCsv

                // combine xml and csv 
                var summedDonations = xmlDonations
                    .GroupJoin(csvDonations,
                               xmlDonation => xmlDonation.PersonID,
                               csvDonation => csvDonation.PersonID,
                               (xmlDonation, csvDonationGroup) =>
                               {
                                   var csvDonation = csvDonationGroup.FirstOrDefault(); 
                                   return new MemberDonation
                                   {
                                       PersonID = xmlDonation.PersonID,
                                       MemberName = xmlDonation.MemberName,
                                       PartyAffiliation = csvDonation?.PartyAffiliation ?? "Unknown", 
                                       AmountDonated = xmlDonation.AmountDonated
                                   };
                               })
                    .ToList();

                
                gridContributions.DataSource = summedDonations;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}");
            }
        }
    }
}
