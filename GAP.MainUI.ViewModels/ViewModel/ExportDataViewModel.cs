using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Input;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Ninject;
using GalaSoft.MvvmLight.Command;
using GAP.BL;
using GAP.Helpers;
using GAP.MainUI.ViewModels.Helpers;
using GAP.BL.CollectionEntities;

namespace GAP.MainUI.ViewModels.ViewModel
{
    public class ExportDataViewModel : BaseViewModel<Dataset>
    {
        public List<Project> Projects { get; set; }

        public ExportDataViewModel(string token)
            : base(token)
        {
            Projects = GlobalCollection.Instance.Projects.ToList();
            AddDataSourceItems();
            if (Projects.Any())
            {
                SelectedProject = Projects.First();
                ProjectSelectionChanged();
            }
        }

        private void AddDataSourceItems()
        {
            DataSourceItems = new List<string>
            {
                "TXT"
            };
            SelectedDataSource = DataSourceItems[0];
        }


        List<string> _dataSourceItems;

        public List<string> DataSourceItems
        {
            get { return _dataSourceItems; }
            set
            {
                _dataSourceItems = value;
                NotifyPropertyChanged("DataSourceItems");
            }
        }

        ICommand _browseCommand;
        public ICommand BrowseCommand
        {
            get { return _browseCommand ?? (_browseCommand = new RelayCommand(Browse)); }
        }

        private void Browse()
        {
            GlobalDataModel.Instance.SendMessage(Token, NotificationMessageEnum.BrowseFiles);
        }

        string _fileName;
        public string FileName
        {
            get { return _fileName; }
            set
            {
                _fileName = value;
                NotifyPropertyChanged("FileName");
            }
        }

        Well _selectedWell;

        Project _selectedProject;
        public Project SelectedProject
        {
            get { return _selectedProject; }
            set
            {
                if (_selectedProject == value) return;
                _selectedProject = value;
                ProjectSelectionChanged();
                NotifyPropertyChanged("SelectedProject");
            }
        }

        public Well SelectedWell
        {
            get { return _selectedWell; }
            set
            {
                // if (_wellName == value) return;
                //above line does not make any sense here as upon changing the project it could result in the same well name
                //but its ref project could be different so we must need to execute the below code to get the updated dataset
                _selectedWell = value;
                WellSelectionChanged();
                NotifyPropertyChanged("SelectedWell");
            }
        }

        private void ProjectSelectionChanged()
        {
            Wells = new ObservableCollection<Well>(GlobalCollection.Instance.Projects.Single(u=>u.ID==SelectedProject.ID).Wells);
            if (Wells.Any()) SelectedWell = Wells.First();
        }

        ObservableCollection<Well> _wells;
        public ObservableCollection<Well> Wells
        {
            get { return _wells; }
            set
            {
                _wells = value;
                NotifyPropertyChanged("Wells");
            }
        }
        public List<Dataset> Datasets { get; set; }
        string _selectedDataFormat;
        public string SelectedDataSource
        {
            get { return _selectedDataFormat; }
            set
            {
                _selectedDataFormat = value;
                FileName = string.Empty;
                NotifyPropertyChanged("SelectedDataFormat");
            }
        }

        private void WellSelectionChanged()
        {
            Datasets = HelperMethods.Instance.GetDatasetsByWellID(SelectedWell.ID).ToList();
        }

        protected override bool CanSave()
        {
            return !string.IsNullOrWhiteSpace(FileName) && SelectedProject != null
                                    && SelectedWell!=null && SelectedDataSource == "TXT";
        }

        public override void Save()
        {
            //need to check the final depth and 
            if (!Datasets.Any())
            {
                IoC.Kernel.Get<ISendMessage>().MessageBox(
                    IoC.Kernel.Get<IResourceHelper>().ReadResource("NoDatasetFoundAgainstThisProjectAndWell"), Token);
                return;
            }
            if (!VerifyIndex()) return;

            WriteTextFile();
        }

        private void WriteTextFile()
        {
            var headers = string.Empty;
            headers += "Depth" + "\t";
            foreach (var dataset in Datasets)
                headers = headers + (dataset.Name + "\t");

            headers = headers.Remove(headers.Length - 1);
            headers += "\n";
            headers += "\n";

            var sbuilder = new StringBuilder();

            ///this could be a very buggy code, lets assume first dataset has 10 rows and subsequent datasets
            // have less than 10 records
            for (var j = 0; j < Datasets.First().DepthAndCurves.Count; j++)
            {
                sbuilder.AppendLine();
                sbuilder.Append(Convert.ToString(Datasets.First().DepthAndCurves[j].Depth) + "\t");
                for (var i = 0; i < Datasets.Count(); i++)
                {
                    //count is 10 and index works only upto 9
                    if (j >= Datasets[i].DepthAndCurves.Count) continue;
                    sbuilder.Append(Convert.ToString(Datasets[i].DepthAndCurves[j].Curve) + "\t");
                }
            }

            using (var file = new StreamWriter(FileName, true))
            {
                file.WriteLine(headers + sbuilder.ToString());
                IoC.Kernel.Get<ISendMessage>().MessageBoxWithInformation
                    (Token, IoC.Kernel.Get<IResourceHelper>().ReadResource("TextFileSuccessfullyExported"));
                GlobalDataModel.Instance.SendMessage(Token, NotificationMessageEnum.CloseScreen);
            }
        }

        private bool VerifyIndex()
        {
            for (var i = 0; i < Datasets.Count() - 1; i++)
            {
                if (Datasets[i].DepthAndCurves.Count != Datasets[i + 1].DepthAndCurves.Count)
                {
                    IoC.Kernel.Get<ISendMessage>().MessageBoxWithExclamation
                        (Token, string.Format(IoC.Kernel.Get<IResourceHelper>().ReadResource("OneDatasetHasDifferentDepthThanAnother"),
                        Datasets[i].Name, Datasets[i + 1].Name));
                    return false;
                }

                for (var j = 0; j < Datasets[i].DepthAndCurves.Count; j++)
                {
                    if (Datasets[i].DepthAndCurves[j].Depth == Datasets[i + 1].DepthAndCurves[j].Depth) continue;
                    IoC.Kernel.Get<ISendMessage>().MessageBoxWithExclamation(Token, string.Format
                        (IoC.Kernel.Get<IResourceHelper>().ReadResource("OneDatasetHasDifferentDepthThanAnother"),
                            Datasets[i].Name, Datasets[i + 1].Name));
                    return false;
                }
            }
            return true;
        }

    }//end class
}//end namespace
