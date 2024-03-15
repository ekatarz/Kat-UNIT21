using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Kat_UNIT21
{
    public class CsvDonationParser : ICsvParser
    {
        private readonly ICsvFileLocator _csvFileLocator;

        public CsvDonationParser(ICsvFileLocator csvFileLocator)
        {
            _csvFileLocator = csvFileLocator;
        }

        public IEnumerable<MemberDonation> ParseCsv(string filePath)
        {
            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException("CSV file not found.");
            }

            var donations = new List<MemberDonation>();

            var lines = File.ReadAllLines(filePath).Skip(1);
            foreach (var line in lines)
            {
                var columns = line.Split(',');
                donations.Add(new MemberDonation
                {
                    PersonID = columns[0].Trim(),
                    PartyAffiliation = columns[3].Trim(), 
                });
            }

            return donations;
        }

    }
}
