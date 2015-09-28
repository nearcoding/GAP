using GAP.BL;
using GAP.MainUI.ViewModels.Helpers;
using GAP.Helpers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace GAP.MainUI.ViewModels.ViewModel
{
	public partial class GlobalDataModel
	{
		public static ExtendedBindingList<Curve> CurveList { get; set; }

		private static Curve DeletedCurve { get; set; }
	   
		private static void CurveDeleted()
		{
			if (DeletedCurve == null)
				return;

			//Updating MainUI using MainViewModel
			var selectedChart = MainViewModel.Charts.SingleOrDefault(u => u.Chart.ChartName == DeletedCurve.RefChart);
		    if (selectedChart == null) return;
		    var selectedTrack = selectedChart.TrackGroup.Tracks.SingleOrDefault(u => u.Track.TrackName == DeletedCurve.RefTrack);
		    if (selectedTrack == null) return;
		    var selectedCurve = selectedTrack.Curves.SingleOrDefault
		        (u => u.RefProject == DeletedCurve.RefProject &&
		              u.RefWell == DeletedCurve.RefWell &&
		              u.RefDataset == DeletedCurve.RefDataset);

		    if (selectedCurve != null)
		        selectedTrack.Curves.Remove(selectedCurve);
		}
		
		static void CurveList_BeforeItemDelete(Curve objectToBeDeleted)
		{
			DeletedCurve = objectToBeDeleted;
		}
		
		private static void InitiateCurveDeletion()
		{
			UndoRedoObject.CurveUndoRedoOperation(DeletedCurve, true);
			CurveDeleted();
		}

		static void CurveList_ListChanged(object sender, ListChangedEventArgs e)
		{
			switch (e.ListChangedType)
			{
				case ListChangedType.ItemAdded:
					var curveObject = AddCurvesToMainUI(e.NewIndex);
					UndoRedoObject.CurveUndoRedoOperation(curveObject, false);
					break;
				case ListChangedType.ItemDeleted:
					InitiateCurveDeletion();
					break;
			}
		}

		private static Curve AddCurvesToMainUI(int index)
		{
			var curveObject = CurveList[index];
			var chartToShow = MainViewModel.Charts.SingleOrDefault(u => u.Chart.ChartName == curveObject.RefChart);

			if (chartToShow==null || chartToShow.TrackGroup.Tracks == null)
				return null;

			var selectedTrack = chartToShow.TrackGroup.Tracks.SingleOrDefault(u => u.Track.TrackName == curveObject.RefTrack);
            if (selectedTrack == null) return null;
			if (selectedTrack.Curves == null)
				selectedTrack.Curves = new ObservableCollection<Curve>();

			selectedTrack.Curves.Add(curveObject);

			return curveObject;
		}


		public static void UpdateCurveWithProjectReference(Project project)
		{
			var curves = CurveList.Where(u => u.RefProject == project.OldName);
		    if (!curves.Any()) return;
		    foreach (var curve in curves)
		        curve.RefProject = project.ProjectName;
		}
	}//end class
}//end namespace
