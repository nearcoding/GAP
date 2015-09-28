using GAP.BL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ninject;
using GAP.Helpers;
using System.Collections.ObjectModel;

namespace GAP.MainUI.ViewModels.Helpers
{
    public class AddDataSetEquation
    {
        ExcelWorkSheetFuncions excelFuntions;

        //OR THE DATASET OBJECT
        private ObservableCollection<DepthCurveInfo> _oldDepthAndCurves;
        private ObservableCollection<DepthCurveInfo> _newDepthAndCurves;

        public AddDataSetEquation(ObservableCollection<DepthCurveInfo> depthAndCurves)
        {
            // TODO: Complete member initialization
            this._oldDepthAndCurves = depthAndCurves;
            excelFuntions = new ExcelWorkSheetFuncions();
        }

        //TEST
        public ObservableCollection<DepthCurveInfo> LogarithmDataSet(string to)
        {
            double logNumber;
            if (Double.TryParse(to, out logNumber))
            {
                _newDepthAndCurves = new ObservableCollection<DepthCurveInfo>();
                foreach (var oldItem in _oldDepthAndCurves)
                {
                    double oldCurve;
                    double newCurve;
                    if (Double.TryParse(oldItem.Curve.ToString(), out oldCurve))
                    {
                        newCurve = excelFuntions.GetLog(oldCurve, logNumber);
                        DepthCurveInfo curve = new DepthCurveInfo();
                        curve.Curve = Decimal.Parse(newCurve.ToString());
                        curve.Depth = oldItem.Depth;
                        _newDepthAndCurves.Add(curve);
                    }
                }
                return _newDepthAndCurves;
            }
            else
                return null;
        }

        public ObservableCollection<DepthCurveInfo> SumDataSet(ObservableCollection<DepthCurveInfo> depthAndCurvesToSum)
        {
            _newDepthAndCurves = new ObservableCollection<DepthCurveInfo>();
            foreach (var dephAndCurves in depthAndCurvesToSum)
            {
                DepthCurveInfo curve = new DepthCurveInfo();
                curve.Curve = _oldDepthAndCurves.First(A => A.Depth == dephAndCurves.Depth).Curve + dephAndCurves.Curve;
                curve.Depth = dephAndCurves.Depth;
                _newDepthAndCurves.Add(curve);
            }
            return _newDepthAndCurves;
        }

        public ObservableCollection<DepthCurveInfo> SubTDataSet(ObservableCollection<DepthCurveInfo> depthAndCurvesToSubT)
        {
            _newDepthAndCurves = new ObservableCollection<DepthCurveInfo>();
            foreach (var dephAndCurves in depthAndCurvesToSubT)
            {
                DepthCurveInfo curve = new DepthCurveInfo();
                curve.Curve = _oldDepthAndCurves.First(A => A.Depth == dephAndCurves.Depth).Curve - dephAndCurves.Curve;
                curve.Depth = dephAndCurves.Depth;
                _newDepthAndCurves.Add(curve);
            }
            return _newDepthAndCurves;
        }

        public ObservableCollection<DepthCurveInfo> DivDataSet(ObservableCollection<DepthCurveInfo> depthAndCurvesToDiv)
        {
            _newDepthAndCurves = new ObservableCollection<DepthCurveInfo>();
            foreach (var dephAndCurves in depthAndCurvesToDiv)
            {
                DepthCurveInfo curve = new DepthCurveInfo();
                curve.Curve = _oldDepthAndCurves.First(A => A.Depth == dephAndCurves.Depth).Curve / dephAndCurves.Curve;
                curve.Depth = dephAndCurves.Depth;
                _newDepthAndCurves.Add(curve);
            }
            return _newDepthAndCurves;
        }

        public ObservableCollection<DepthCurveInfo> MulDataSet(ObservableCollection<DepthCurveInfo> depthAndCurvesToDiv)
        {
            _newDepthAndCurves = new ObservableCollection<DepthCurveInfo>();
            foreach (var dephAndCurves in depthAndCurvesToDiv)
            {
                DepthCurveInfo curve = new DepthCurveInfo();
                curve.Curve = _oldDepthAndCurves.First(A => A.Depth == dephAndCurves.Depth).Curve * dephAndCurves.Curve;
                curve.Depth = dephAndCurves.Depth;
                _newDepthAndCurves.Add(curve);
            }
            return _newDepthAndCurves;
        }
    }
}
