using System;
using System.Drawing;
using Neuro;

namespace EmotionalStates.IndexChart
{
    internal sealed class EegIndexChart : IDrawable, IEegIndexChart
    {
        private readonly EegIndexMap _chartMap = new EegIndexMap();
        private readonly EegIndexChartCursor _chartCursor = new EegIndexChartCursor();
        private EegIndexValues[] _indicesData = new EegIndexValues[0];
        private Size _drawableSize = new Size(100, 100);

        public Size DrawableSize
        {
            get => _drawableSize;
            set
            {
                _drawableSize = value;
                SizeChanged?.Invoke(this, _drawableSize);
            }
        }

        public void Draw(Graphics graphics)
        {
            using (var backgroundBrush = new SolidBrush(Color.LightYellow))
            {
                graphics.FillRectangle(backgroundBrush, 0, 0, _drawableSize.Width, _drawableSize.Height+1);
            }

            if (Mode == EegIndexChartMode.Signal)
            {
                _chartMap.Draw(graphics);
                _chartCursor.Draw(graphics);
            }
            else if (Mode == EegIndexChartMode.Waiting)
            {
                var waitString = "Waiting for signal...";
                var waitStringFont = new Font("Arial", 30);
                var stringSize = graphics.MeasureString(waitString, waitStringFont);
                var drawableCenterX = _drawableSize.Width / 2;
                var drawableCenterY = _drawableSize.Height / 2;
                graphics.DrawString("Waiting for signal...", waitStringFont, Brushes.Black,
                    drawableCenterX - stringSize.Width / 2, drawableCenterY - stringSize.Height / 2);
            }
        }

        public EegIndexValues[] IndicesData
        {
            set
            {
                _indicesData = value;
                _chartMap.RecalculatePolygons(_indicesData, DrawableSize);
                _chartCursor.RecalculateLabels(_indicesData, DrawableSize);
            }
        }

        public EegIndexChartMode Mode { get; set; }

        public event EventHandler<Size> SizeChanged;
    }

    class EegIndexMap
    {
        private object _polygonLockObject = new object();
        private Point[] _deltaPolygon = { new Point(0, 0) };
        private Point[] _thetaPolygon = { new Point(0, 0) };
        private Point[] _alphaPolygon = { new Point(0, 0) };
        private Point[] _betaPolygon = { new Point(0, 0) };
        private readonly Color _deltaColor = Color.IndianRed;
        private readonly Color _thetaColor = Color.Orange;
        private readonly Color _alphaColor = Color.DodgerBlue;
        private readonly Color _betaColor = Color.DarkOliveGreen;

        public void Draw(Graphics graphics)
        {
            lock (_polygonLockObject)
            {
                graphics.FillPolygon(new SolidBrush(_alphaColor), _alphaPolygon);
                graphics.FillPolygon(new SolidBrush(_betaColor), _betaPolygon);
                graphics.FillPolygon(new SolidBrush(_deltaColor), _deltaPolygon);
                graphics.FillPolygon(new SolidBrush(_thetaColor), _thetaPolygon);
            }
        }

        public void RecalculatePolygons(EegIndexValues[] indicesData, Size drawableSize)
        {
            var drawStartX = drawableSize.Width - indicesData.Length;

            var deltaPolygon = new Point[indicesData.Length + 2];
            deltaPolygon[0] = new Point(drawableSize.Width - 1, 0);
            deltaPolygon[1] = new Point(drawStartX, 0);

            var thetaPolygon = new Point[indicesData.Length * 2];

            var alphaPolygon = new Point[indicesData.Length * 2];

            var betaPolygon = new Point[indicesData.Length * 2];

            for (var i = 0; i < indicesData.Length; ++i)
            {
                var deltaY = indicesData[i].DeltaRate * drawableSize.Height;
                deltaPolygon[2 + i] = new Point(drawStartX + i, (int)deltaY);

                var thetaYTop = deltaY;
                var thetaYBottom = thetaYTop + indicesData[i].ThetaRate * drawableSize.Height;
                thetaPolygon[thetaPolygon.Length - 1 - i] = new Point(drawStartX + i, (int)thetaYTop);
                thetaPolygon[i] = new Point(drawStartX + i, (int)thetaYBottom);

                var alphaYTop = thetaYBottom;
                var alphaYBottom = alphaYTop + indicesData[i].AlphaRate * drawableSize.Height;
                alphaPolygon[alphaPolygon.Length - 1 - i] = new Point(drawStartX + i, (int)alphaYTop);
                alphaPolygon[i] = new Point(drawStartX + i, (int)alphaYBottom);

                var betaYTop = alphaYBottom;
                var betaYBottom = betaYTop + indicesData[i].BetaRate * drawableSize.Height;
                betaPolygon[betaPolygon.Length - 1 - i] = new Point(drawStartX + i, (int)betaYTop);
                betaPolygon[i] = new Point(drawStartX + i, (int)betaYBottom);
            }

            lock (_polygonLockObject)
            {
                _deltaPolygon = deltaPolygon;
                _thetaPolygon = thetaPolygon;
                _alphaPolygon = alphaPolygon;
                _betaPolygon = betaPolygon;
            }
        }
    }

    class EegIndexChartCursor
    {
        private readonly Color _cursorBackground = Color.FromArgb(50, Color.White);
        private readonly Font _cursorLabelFont = new Font("Arial", 14);
        private double _cursorPosition = 0.95;
        private Rectangle _cursorRectangle = new Rectangle(100, 0, 100, 100);
        private int _alphaLabelY = 20;
        private int _betaLabelY = 45;
        private int _deltaLabelY = 70;
        private int _thetaLabelY = 90;
        private string _alphaLabelText;
        private string _betaLabelText;
        private string _deltaLabelText;
        private string _thetaLabelText;

        public void RecalculateLabels(EegIndexValues[] indicesData, Size drawableSize)
        {
            _cursorRectangle.X = (int) ((drawableSize.Width - _cursorRectangle.Width) * _cursorPosition);
            _cursorRectangle.Height = drawableSize.Height;
            var alpha = 0.25;
            var beta = 0.25;
            var delta = 0.25;
            var theta = 0.25;
            var drawStartX = drawableSize.Width - indicesData.Length;
            var cursorCenter = _cursorRectangle.X + _cursorRectangle.Width / 2;
            if (cursorCenter >= drawStartX)
            {
                var dataIndex = cursorCenter - drawStartX;
                alpha = indicesData[dataIndex].AlphaRate;
                beta = indicesData[dataIndex].BetaRate;
                delta = indicesData[dataIndex].DeltaRate;
                theta = indicesData[dataIndex].ThetaRate;
            }
            
            _alphaLabelText = $"α {alpha * 100.0:F1} %";
            _betaLabelText = $"β {beta * 100.0:F1} %";
            _deltaLabelText = $"δ {delta * 100.0:F1} %";
            _thetaLabelText = $"θ {theta * 100.0:F1} %";

            var deltaHeight = delta * drawableSize.Height;
            _deltaLabelY = (int)(deltaHeight / 2.0);
            var thetaHeight = theta * drawableSize.Height;
            _thetaLabelY = (int)(deltaHeight + thetaHeight / 2);
            var alphaHeight = alpha * drawableSize.Height;
            _alphaLabelY = (int)(deltaHeight + thetaHeight + alphaHeight / 2);
            var betaHeight = beta * drawableSize.Height;
            _betaLabelY = (int)(deltaHeight + thetaHeight + alphaHeight + betaHeight / 2);
        }

        public void Draw(Graphics graphics)
        {
            graphics.FillRectangle(new SolidBrush(_cursorBackground), _cursorRectangle);
            var cursorCenter = _cursorRectangle.X + _cursorRectangle.Width / 2;
            graphics.DrawLine(Pens.White, cursorCenter, 0, cursorCenter, _cursorRectangle.Height);

            var alphaLabelSize = graphics.MeasureString(_alphaLabelText, _cursorLabelFont);
            var alphaX = cursorCenter - alphaLabelSize.Width / 2;
            var betaLabelSize = graphics.MeasureString(_alphaLabelText, _cursorLabelFont);
            var betaX = cursorCenter - betaLabelSize.Width / 2;
            var deltaLabelSize = graphics.MeasureString(_alphaLabelText, _cursorLabelFont);
            var deltaX = cursorCenter - deltaLabelSize.Width / 2;
            var thetaLabelSize = graphics.MeasureString(_alphaLabelText, _cursorLabelFont);
            var thetaX = cursorCenter - thetaLabelSize.Width / 2;

            using (var fontBrush = new SolidBrush(Color.Black))
            {
                graphics.DrawString(_alphaLabelText, _cursorLabelFont, fontBrush, alphaX, _alphaLabelY);
                graphics.DrawString(_betaLabelText, _cursorLabelFont, fontBrush, betaX, _betaLabelY);
                graphics.DrawString(_deltaLabelText, _cursorLabelFont, fontBrush, deltaX, _deltaLabelY);
                graphics.DrawString(_thetaLabelText, _cursorLabelFont, fontBrush, thetaX, _thetaLabelY);
            }
        }
    }
}
