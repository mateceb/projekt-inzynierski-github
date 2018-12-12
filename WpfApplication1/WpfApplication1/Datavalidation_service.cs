using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApplication1
{
    class Datavalidation_service
    {
        public bool Checking_name(string box)
        {
            if (System.Text.RegularExpressions.Regex.IsMatch(box, "[\",/,\\,<,>,?,:,*,|]"))
            {

                return true;
            }
            return false;
        }
    }
}
