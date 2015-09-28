using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GAP.BL
{
    public class AnnotationErrorInfo : SubBaseEntity
    {
        int _errorID;
        public int ErrorID
        {
            get { return _errorID; }
            set
            {
                _errorID = value;
                NotifyPropertyChanged("ErrorID");
            }
        }
        string _errorDescription;
        public string ErrorDescription
        {
            get { return _errorDescription; }
            set
            {
                _errorDescription = value;
                NotifyPropertyChanged("ErrorDescription");
            }
        }
    }//end class
}//end namespace
