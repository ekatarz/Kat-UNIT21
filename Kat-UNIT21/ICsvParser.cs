using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kat_UNIT21
{
    public interface ICsvParser
    {
        IEnumerable<MemberDonation> ParseCsv(string filePath);
    }
}
