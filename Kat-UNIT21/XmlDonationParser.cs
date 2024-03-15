using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Xml.Linq;

namespace Kat_UNIT21
{
    public class XmlDonationParser : IXmlParser
    {
        public IEnumerable<MemberDonation> ParseXml(string url)
        {
            var donations = new List<MemberDonation>();
            using (var client = new WebClient())
            {
                string xmlContent = client.DownloadString(url);
                var doc = XDocument.Parse(xmlContent);

                foreach (var memberElem in doc.Descendants("regmem"))
                {
                    string memberName = memberElem.Attribute("membername")?.Value;
                    string personId = memberElem.Attribute("personid")?.Value.Split('/').Last();
                    decimal totalAmount = ExtractDonations(memberElem);

                    donations.Add(new MemberDonation
                    {
                        MemberName = memberName,
                        PersonID = personId,
                        AmountDonated = totalAmount
                    });
                }
            }

            return donations;
        }

        private decimal ExtractDonations(XElement memberElem)
        {
            decimal totalAmount = 0;

            var categoryTypes = new[] { "2", "3", "4", "5" };
            var categories = memberElem.Elements("category")
                .Where(cat => categoryTypes.Contains(cat.Attribute("type")?.Value));

            foreach (var category in categories)
            {
                foreach (var item in category.Descendants("item"))
                {
                    var amount = ExtractAmount(item.Value);
                    if (amount.HasValue)
                    {
                        totalAmount += amount.Value;
                    }
                }
            }

            return totalAmount;
        }

        private decimal? ExtractAmount(string donationText)
        {
            var matches = Regex.Matches(donationText, @"£(\d{1,3}(?:,\d{3})*\.?\d*)");
            decimal totalAmount = 0;

            foreach (Match match in matches)
            {
                if (match.Success)
                {
                    string valueWithoutCommas = match.Groups[1].Value.Replace(",", "");
                    if (decimal.TryParse(valueWithoutCommas, NumberStyles.Any, CultureInfo.InvariantCulture, out var amount))
                    {
                        totalAmount += amount;
                    }
                }
            }

            return totalAmount > 0 ? totalAmount : (decimal?)null;
        }
    }
}
