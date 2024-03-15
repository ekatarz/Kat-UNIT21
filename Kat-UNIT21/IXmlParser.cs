using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kat_UNIT21
{   //interface segregation
    public interface IXmlParser
    {
        IEnumerable<MemberDonation> ParseXml(string url);
    }
}
