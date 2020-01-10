using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using CallibriFeatures.DrawableControl;

namespace CallibriFeatures.Spectrum.View
{
    public class SpectrumChartDrawable : IDrawable, IMouseEventsHandler
    {
        private readonly IReadOnlyCollection<SpectrumField> _channelsFields;
        private readonly VerticalRuler _verticalRuler;
        private readonly HorizontalRuler _horizontalRuler;

        private IDrawable _chartDrawable;

        public SizeF DrawableSize
        {
            set => _chartDrawable = CreateChartDrawable(_channelsFields, _verticalRuler, _horizontalRuler, value);
        }

        public SpectrumChartDrawable(IReadOnlyCollection<SpectrumField> channels, VerticalRuler verticalRuler, HorizontalRuler horizontalRuler, Size initialSize)
        {
            _channelsFields = channels ?? throw new ArgumentNullException(nameof(channels), "Channels collection cannot be null");
            _verticalRuler = verticalRuler ?? throw new ArgumentNullException(nameof(verticalRuler), "Vertical ruler object cannot be null");
            _horizontalRuler = horizontalRuler ?? throw new ArgumentNullException(nameof(horizontalRuler), "Horizontal ruler object cannot be null");

            _chartDrawable = CreateChartDrawable(_channelsFields, _verticalRuler, horizontalRuler, initialSize);
        }

        public void Draw(Graphics graphics)
        {
           _chartDrawable?.Draw(graphics);
        }

        private static IDrawable CreateChartDrawable(IReadOnlyCollection<SpectrumField> channels, VerticalRuler verticalRuler, HorizontalRuler horizontalRuler, SizeF chartSize)
        {
            if (channels == null)
                throw new ArgumentNullException(nameof(channels), "Channels collection cannot be null");
            if (verticalRuler == null)
                throw new ArgumentNullException(nameof(verticalRuler), "Vertical ruler object cannot be null");

            if (chartSize.Width == 0 || chartSize.Height == 0)
                return new EmptyDrawable(Color.LightYellow);

            if (channels.Count == 0)
                return new EmptyDrawable(Color.LightYellow);

            var columnsCount = 0;
            float chHeight;
            float chWidth;
            const float horizontalRulerHeight = 40f;
            const float verticalRulerWidthSmall = 35f;
            const float verticalRulerWidthBig = 45f;
            float verticalRulerWidth;
            do
            {
                columnsCount++;
                verticalRulerWidth = columnsCount == 1 ? verticalRulerWidthSmall : verticalRulerWidthBig;
                var verticalRulersCount = columnsCount > 1 ? columnsCount - 1 : 1;
                chHeight = (chartSize.Height - horizontalRulerHeight) / (float)Math.Ceiling((double)channels.Count / columnsCount);
                chWidth = (chartSize.Width - verticalRulerWidth * verticalRulersCount) / columnsCount;
            } while (chWidth / chHeight > 5);

            var column = 0;
            var row = 0;
            var firstLeft = columnsCount > 1 ? 0f : verticalRulerWidth;
            var chartDrawable = new CompoundDrawable();
            verticalRuler.IsSmall = columnsCount == 1;
            foreach (var spectrumField in channels)
            {
                if (columnsCount == 1)
                {
                    var verticalRulerPos = new PointF(0f, row * chHeight);
                    var verticalRulerSize = new SizeF(verticalRulerWidth, chHeight);
                    chartDrawable.AddDrawable(verticalRuler, new AbsolutePosition(verticalRulerPos), new AbsoluteSize(verticalRulerSize));
                }
                else if (column < columnsCount - 1)
                {
                    var verticalRulerPos = new PointF(chWidth + column * (chWidth + verticalRulerWidth), row * chHeight);
                    var verticalRulerSize = new SizeF(verticalRulerWidth, chHeight);
                    chartDrawable.AddDrawable(verticalRuler, new AbsolutePosition(verticalRulerPos), new AbsoluteSize(verticalRulerSize));
                }

                var additionalLeft = verticalRulerWidth * column;
                var position = new PointF(firstLeft + additionalLeft + column * chWidth, row * chHeight);
                var size = new SizeF(chWidth, chHeight);
                chartDrawable.AddDrawable(spectrumField, new AbsolutePosition(position), new AbsoluteSize(size));

                column = (column + 1) % columnsCount;
                if (column == 0) ++row;
            }

            if (column != 0)
            {
                var additionalLeft = verticalRulerWidth * column;
                var position = new PointF(firstLeft + additionalLeft + column * chWidth, row * chHeight);
                var size = new SizeF(chWidth, chHeight);
                chartDrawable.AddDrawable(new EmptySpectrumField(), new AbsolutePosition(position), new AbsoluteSize(size));
            }

            column = 0;
            for (var i = 0; i < columnsCount; ++i)
            {
                if (columnsCount == 1)
                {
                    var unitsFieldPos = new PointF(0f, chartSize.Height - horizontalRulerHeight);
                    var unitsFieldSize = new SizeF(verticalRulerWidth, horizontalRulerHeight);
                    chartDrawable.AddDrawable(new UnitsField(), new AbsolutePosition(unitsFieldPos), new AbsoluteSize(unitsFieldSize));
                }
                else if (column < columnsCount - 1)
                {
                    var unitsFieldPos = new PointF(chWidth + column * (chWidth + verticalRulerWidth), chartSize.Height - horizontalRulerHeight);
                    var unitsFieldSize = new SizeF(verticalRulerWidth, horizontalRulerHeight);
                    chartDrawable.AddDrawable(new UnitsField(), new AbsolutePosition(unitsFieldPos), new AbsoluteSize(unitsFieldSize));
                }

                var additionalLeft = verticalRulerWidth * column;
                var horizontalRulerPos = new PointF(firstLeft + additionalLeft + i * chWidth, chartSize.Height - horizontalRulerHeight);
                var horizontalRulerSize = new SizeF(chWidth, horizontalRulerHeight);
                chartDrawable.AddDrawable(horizontalRuler, new AbsolutePosition(horizontalRulerPos), new AbsoluteSize(horizontalRulerSize));
                column = (column + 1) % columnsCount;
            }

            chartDrawable.DrawableSize = chartSize;
            return chartDrawable;
        }

        public bool OnMouseMove(MouseEventArgs mouseEventArgs)
        {
            return (_chartDrawable as IMouseEventsHandler)?.OnMouseMove(mouseEventArgs) ?? false;
        }

        public bool OnMouseClick(MouseEventArgs mouseEventArgs)
        {
            return (_chartDrawable as IMouseEventsHandler)?.OnMouseClick(mouseEventArgs) ?? false;
        }

        public bool OnMouseDown(MouseEventArgs mouseEventArgs)
        {
            return (_chartDrawable as IMouseEventsHandler)?.OnMouseDown(mouseEventArgs) ?? false;
        }

        public bool OnMouseUp(MouseEventArgs mouseEventArgs)
        {
            return (_chartDrawable as IMouseEventsHandler)?.OnMouseUp(mouseEventArgs) ?? false;
        }

        public bool OnMouseLeave()
        {
            return (_chartDrawable as IMouseEventsHandler)?.OnMouseLeave() ?? false;
        }
    }
}