using GAP.Helpers;
using GAP.MainUI.ViewModels.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace GAP
{
    /// <summary>
    /// Interaction logic for PorePressureView.xaml
    /// </summary>
    public partial class PorePressureView
    {
        public PorePressureView()
        {
            InitializeComponent();
            _dataContext = new PorePressureViewModel(Token);
            DataContext = _dataContext;
        }

        PorePressureViewModel _dataContext;
        
        protected override void ReceiveMessage(NotificationMessageType messageType)
        {
            switch (messageType.MessageType)
            {
                case NotificationMessageEnum.PorePresureDrillingExponent:
                    PorePressureDrillingExponent view = new PorePressureDrillingExponent();
                    view.Show();
                    break;
            }
        }
    }///end class
}//end namespace
