

using Abt.Controls.SciChart.Model.DataSeries;
using Abt.Controls.SciChart.Numerics.CoordinateCalculators;
using System.Collections;
using System.Collections.Generic;
using System.Windows;
namespace GAP.MainUI.ViewModels.Helpers
{
    /// <summary>
    /// A class used to convert a <see cref="IPointSeries"/> which contains resampled data-points, 
    /// into a stream of <see cref="Point"/> pixel coordinates for the screen. This class re-uses logic used 
    /// throughout the <see cref="BaseRenderableSeries"/> types.
    /// </summary>
    /// <remarks>Intended to be used only be SciChart</remarks>
    public class LinesEnumerable : IEnumerable<Point>
    {
        protected readonly IPointSeries _pointSeries;
        protected readonly ICoordinateCalculator<double> _xCoordinateCalculator;
        protected readonly ICoordinateCalculator<double> _yCoordinateCalculator;
        protected readonly bool _isDigitalLine;

        /// <summary>
        /// Initializes a new instance of the <see cref="LinesEnumerable" /> class.
        /// </summary>
        /// <param name="pointSeries">The point series.</param>
        /// <param name="xCoordinateCalculator">The x coordinate calculator.</param>
        /// <param name="yCoordinateCalculator">The y coordinate calculator.</param>
        /// <param name="isDigitalLine">if set to <c>true</c> return a digital line .</param>
        public LinesEnumerable(IPointSeries pointSeries, ICoordinateCalculator<double> xCoordinateCalculator, ICoordinateCalculator<double> yCoordinateCalculator, bool isDigitalLine)
        {
            _pointSeries = pointSeries;
            _xCoordinateCalculator = xCoordinateCalculator;
            _yCoordinateCalculator = yCoordinateCalculator;
            _isDigitalLine = isDigitalLine;
        }

        /// <summary>
        /// Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns>
        /// An <see cref="T:System.Collections.IEnumerator" /> object that can be used to iterate through the collection.
        /// </returns>
        public virtual IEnumerator<Point> GetEnumerator()
        {
            return _isDigitalLine ?
                (IEnumerator<Point>)new DigitalLinesIterator(_pointSeries, _xCoordinateCalculator, _yCoordinateCalculator) :
                new LinesIterator(_pointSeries, _xCoordinateCalculator, _yCoordinateCalculator);
        }

        /// <summary>
        /// Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns>
        /// An <see cref="T:System.Collections.IEnumerator" /> object that can be used to iterate through the collection.
        /// </returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }//end class

    /// <summary>
    /// A custom <see cref="IEnumerator{Point}"/> implementation to provide line coordinates from <see cref="IPointSeries"/> input
    /// </summary>
    public class LinesIterator : PointSeriesEnumerator, IEnumerator<Point>
    {
        private readonly ICoordinateCalculator<double> _xCoordinateCalculator;
        private readonly ICoordinateCalculator<double> _yCoordinateCalculator;

        /// <summary>
        /// Initializes a new instance of the <see cref="LinesIterator" /> class.
        /// </summary>
        /// <param name="pointSeries">The point series.</param>
        /// <param name="xCoordinateCalculator">The x coordinate calculator.</param>
        /// <param name="yCoordinateCalculator">The y coordinate calculator.</param>
        public LinesIterator(IPointSeries pointSeries, ICoordinateCalculator<double> xCoordinateCalculator, ICoordinateCalculator<double> yCoordinateCalculator)
            : base(pointSeries)
        {
            _xCoordinateCalculator = xCoordinateCalculator;
            _yCoordinateCalculator = yCoordinateCalculator;
        }

        /// <summary>
        /// Advances the enumerator to the next element of the collection.
        /// </summary>
        /// <returns>
        /// true if the enumerator was successfully advanced to the next element; false if the enumerator has passed the end of the collection.
        /// </returns>
        public override bool MoveNext()
        {
            if (!base.MoveNext())
                return false;

            var pt = base.CurrentPoint2D;
            if (double.IsNaN(pt.Y))
            {
                Current = new Point(float.NaN, float.NaN);
                return true;
            }

            var x = (float)_xCoordinateCalculator.GetCoordinate(pt.X);
            var y = (float)_yCoordinateCalculator.GetCoordinate(pt.Y);

            var isVerticalChart = !_xCoordinateCalculator.IsHorizontalAxisCalculator;
            Current = TransformPoint(new Point(x, y), isVerticalChart);

            return true;
        }

        /// <summary>
        /// Sets the enumerator to its initial position, which is before the first element in the collection.
        /// </summary>
        public override void Reset()
        {
            base.Reset();

            Current = new Point(float.NaN, float.NaN);
        }

        /// <summary>
        /// Gets the <see cref="IPoint" /> in the collection at the current position of the enumerator.
        /// </summary>
        /// <returns>The element in the collection at the current position of the enumerator.</returns>
        public new Point Current { get; private set; }

        /// <summary>
        /// Gets the <see cref="IPoint" /> in the collection at the current position of the enumerator.
        /// </summary>
        /// <returns>The element in the collection at the current position of the enumerator.</returns>
        object IEnumerator.Current
        {
            get { return Current; }
        }

        public static Point TransformPoint(Point point, bool isVerticalChart)
        {
            //swap coords if vertical chart
            if (isVerticalChart)
            {
                var x = point.X;

                point.X = point.Y;
                point.Y = x;
            }

            return point;
        }
    }//end class

    /// <summary>
    /// A custom <see cref="IEnumerator{T}"/> implementation to provide digital line pixel coordinates from <see cref="IPointSeries"/> input
    /// </summary>
    public class DigitalLinesIterator : PointSeriesEnumerator, IEnumerator<Point>
    {
        private readonly ICoordinateCalculator<double> _xCoordinateCalculator;
        private readonly ICoordinateCalculator<double> _yCoordinateCalculator;

        private Point _next;
        private bool _toggle;

        /// <summary>
        /// Initializes a new instance of the <see cref="DigitalLinesIterator" /> class.
        /// </summary>
        /// <param name="pointSeries">The point series.</param>
        /// <param name="xCoordinateCalculator">The x coordinate calculator.</param>
        /// <param name="yCoordinateCalculator">The y coordinate calculator.</param>
        public DigitalLinesIterator(IPointSeries pointSeries, ICoordinateCalculator<double> xCoordinateCalculator,
                                    ICoordinateCalculator<double> yCoordinateCalculator)
            : base(pointSeries)
        {
            _xCoordinateCalculator = xCoordinateCalculator;
            _yCoordinateCalculator = yCoordinateCalculator;

            _toggle = false;
        }


        /// <summary>
        /// Advances the enumerator to the next element of the collection.
        /// </summary>
        /// <returns>
        /// true if the enumerator was successfully advanced to the next element; false if the enumerator has passed the end of the collection.
        /// </returns>
        public override bool MoveNext()
        {
            var isVerticalChart = !_xCoordinateCalculator.IsHorizontalAxisCalculator;

            if (_toggle)
            {
                Current = _next;
                Current = LinesIterator.TransformPoint(Current, isVerticalChart);

                _toggle = false;

                return true;
            }

            bool isFirstPoint = IsReset;

            if (!base.MoveNext())
                return false;

            var pt = base.CurrentPoint2D;
            if (double.IsNaN(pt.Y))
            {
                Current = new Point(float.NaN, float.NaN);
                return true;
            }

            var x = (float)_xCoordinateCalculator.GetCoordinate(pt.X);
            var y = (float)_yCoordinateCalculator.GetCoordinate(pt.Y);

            _next = new Point(x, y);

            Current = isFirstPoint ? _next : new Point(_next.X, isVerticalChart ? Current.X : Current.Y);
            Current = LinesIterator.TransformPoint(Current, isVerticalChart);

            _toggle = !isFirstPoint;

            return true;
        }

        /// <summary>
        /// Sets the enumerator to its initial position, which is before the first element in the collection.
        /// </summary>
        public override void Reset()
        {
            base.Reset();

            _toggle = false;
            Current = new Point(float.NaN, float.NaN);
        }

        /// <summary>
        /// Gets the <see cref="IPoint" /> in the collection at the current position of the enumerator.
        /// </summary>
        /// <returns>The element in the collection at the current position of the enumerator.</returns>
        public new Point Current { get; private set; }

        /// <summary>
        /// Gets the <see cref="IPoint" /> in the collection at the current position of the enumerator.
        /// </summary>
        /// <returns>The element in the collection at the current position of the enumerator.</returns>
        object IEnumerator.Current
        {
            get { return Current; }
        }

    }

}//end namespace
