using System.Collections.Generic;

namespace GAP.BL
{
    public class Family
    {
        public Family()
        {
            Units = new List<string>();
        }
        public string FamilyName { get; set; }
        public List<string> Units { get; set; }
    }//end class
}//end namespace
