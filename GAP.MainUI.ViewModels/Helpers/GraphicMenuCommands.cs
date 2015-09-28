using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using GAP.Helpers;
using System.Linq;
using System.Windows.Input;
using Ninject;
using GAP.BL.CollectionEntities;
using GAP.BL;
using System.Collections.Generic;
using GAP.MainUI.ViewModels.Helpers;
namespace GAP.MainUI.ViewModels.ViewModel
{
    public class GraphicMenuCommands
    {
        string _token;
        public GraphicMenuCommands(string token)
        {
            _token = token;
        }
        ICommand _graphicsPrintCurveCommand;
     
        ICommand _graphicsAddChartCommand;
              
        private void GraphicsAddChart()
        {
            GlobalDataModel.Instance.SendMessage(_token, NotificationMessageEnum.AddNewChart);            
        }

        public ICommand GraphicsAddChartCommand
        {
            get { return _graphicsAddChartCommand ?? (_graphicsAddChartCommand = new RelayCommand(GraphicsAddChart)); }
        }
        
        ICommand _graphicsRemoveChartCommand;

        private void GraphicsRemoveChart()
        {
            GlobalDataModel.Instance.SendMessage(_token, NotificationMessageEnum.RemoveChart);
        }

        private bool CanGraphicsRemoveChart()
        {
            return GlobalCollection.Instance.Charts.Any();
        }

        public ICommand GraphicsRemoveChartCommand
        {
            get { return _graphicsRemoveChartCommand ?? (_graphicsRemoveChartCommand = new RelayCommand(GraphicsRemoveChart, CanGraphicsRemoveChart)); }
        }


        ICommand _graphicsPropertiesCommand;

        private void GraphicsChartProperties()
        {
            GlobalDataModel.Instance.SendMessage(_token, NotificationMessageEnum.SaveChartProperties);
        }

        private bool CanGraphicsChartProperties()
        {
            return GlobalCollection.Instance.Charts.Count > 1;
        }

        public ICommand GraphicsChartPropertiesCommand
        {
            get { return _graphicsPropertiesCommand ?? (_graphicsPropertiesCommand = new RelayCommand(GraphicsChartProperties, CanGraphicsChartProperties)); }
        }


        ICommand _graphicsAddTrackCommand;

        private void GraphicsAddTrack()
        {
            GlobalDataModel.Instance.SendMessage(_token, NotificationMessageEnum.AddNewTrack);
        }

        private bool CanGraphicsAddTrack()
        {
            return GlobalCollection.Instance.Charts.Any();
        }

        public ICommand GraphicsAddTrackCommand
        {
            get { return _graphicsAddTrackCommand ?? (_graphicsAddTrackCommand = new RelayCommand(GraphicsAddTrack, CanGraphicsAddTrack)); }
        }


        ICommand _graphicsRemoveTrackCommand;

        private void GraphicsRemoveTrack()
        {
            GlobalDataModel.Instance.SendMessage(_token, NotificationMessageEnum.RemoveTrack);
        }

        private bool CanGraphicsRemoveTrack()
        {
            return HelperMethods.Instance.AnyTrackExists();// TrackCollection.Instance.TrackList.Any();
        }

        public ICommand GraphicsRemoveTrackCommand
        {
            get { return _graphicsRemoveTrackCommand ?? (_graphicsRemoveTrackCommand = new RelayCommand(GraphicsRemoveTrack, CanGraphicsRemoveTrack)); }
        }


        ICommand _graphicsTrackPropertiesCommand;

        private void GraphicsTrackProperties()
        {
            GlobalDataModel.Instance.SendMessage(_token, NotificationMessageEnum.SaveTrackProperties);
        }

        private bool CanGraphicsTrackProperties()
        {
            return HelperMethods.Instance.GetAllTracks().Count() > 1;
        }

        public ICommand GraphicsTrackPropertiesCommand
        {
            get { return _graphicsTrackPropertiesCommand ?? (_graphicsTrackPropertiesCommand = new RelayCommand(GraphicsTrackProperties, CanGraphicsTrackProperties)); }
        }


        ICommand _graphicsAddCurveCommand;

        private void GraphicsAddCurve()
        {
            GlobalDataModel.Instance.SendMessage(_token, NotificationMessageEnum.AddNewCurve);
        }

        private bool CanGraphicsAddCurve()
        {
            return HelperMethods.Instance.AnyTrackExists() && HelperMethods.Instance.AnyDatasetExist();
        }

        public ICommand GraphicsAddCurveCommand
        {
            get { return _graphicsAddCurveCommand ?? (_graphicsAddCurveCommand = new RelayCommand(GraphicsAddCurve, CanGraphicsAddCurve)); }
        }

        ICommand _graphicsRemoveCurveCommand;

        private void GraphicsCurveRemove()
        {
            GlobalDataModel.Instance.SendMessage(_token, NotificationMessageEnum.RemoveCurveScreen);
        }

        private bool CanGraphicsCurveRemove()
        {
            return HelperMethods.Instance.AnyCurveExists();
        }
        
        public ICommand GraphicsRemoveCurveCommand
        {
            get { return _graphicsRemoveCurveCommand ?? (_graphicsRemoveCurveCommand = new RelayCommand(GraphicsCurveRemove, CanGraphicsCurveRemove)); }
        }

        ICommand _graphicsAddRemoveNCTCommand;

        private void GraphicsAddRemoveNCT()
        {

        }

        private bool CanGraphicsAddRemoveNCT()
        {
            return false;
        }

        public ICommand GraphicsAddRemoveNCTCommand
        {
            get { return _graphicsAddRemoveNCTCommand ?? (_graphicsAddRemoveNCTCommand = new RelayCommand(GraphicsAddRemoveNCT, CanGraphicsAddRemoveNCT)); }
        }

        
        ICommand _graphicsNCTPropertiesCommand;

        private void GraphicsNCTProperties()
        {

        }

        private bool CanGraphicsNCTProperties()
        {
            return false;
        }

        public ICommand GraphicsNCTPropertiesCommand
        {
            get { return _graphicsNCTPropertiesCommand ?? (_graphicsNCTPropertiesCommand = new RelayCommand(GraphicsNCTProperties, CanGraphicsNCTProperties)); }
        }

        public ICommand GraphicsPrintCurveCommand
        {
            get { return _graphicsPrintCurveCommand ?? (_graphicsPrintCurveCommand = new RelayCommand(GraphicsPrintCurve, () =>HelperMethods.Instance.AnyTrackExists())); }
        }

        private void GraphicsPrintCurve()
        {
            GlobalDataModel.Instance.SendMessage(_token, NotificationMessageEnum.CurvePrinting);
        }
    }//end class
}//end namespace
