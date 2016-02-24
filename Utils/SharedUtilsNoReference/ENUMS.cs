using System;
using System.Collections.Generic;
using System.Text;

namespace SharedUtilsNoReference
{
    public static class ENUMS
    {
        public static String GetName(this Enum eff)
        {
            return Enum.GetName(eff.GetType(), eff);
        }
    }
}
