using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using EmotionalStates.Drawable;
using Neuro;

namespace EmotionalStates.SignalChart
{
    public class DrawableSignalChart : IDrawable, IMouseEventsHandler
    {
        private readonly SignalView.SignalChart _signalChartControl;
        private BipolarDoubleChannel _signalChannel;
        
        private int totalLengthChanged;

        public void SetChannel(BipolarDoubleChannel channel)
        {
            if (channel == _signalChannel) return;
            if (_signalChannel != null)
            {
                _signalChannel.LengthChanged -= OnLengthChanged;
            }
            _signalChannel = channel;
            if (_signalChannel != null)
            {
                _signalChannel.LengthChanged += OnLengthChanged;
            }
        }

        private void OnLengthChanged(object sender, int length)
        {
            if (_signalChannel == null) return;
            totalLengthChanged += length;
        }

        public DrawableSignalChart()
        {
            DrawableSize = new Size(500, 440);

            _signalChartControl = new SignalView.SignalChart();

            _signalChartControl.BackColor = System.Drawing.SystemColors.Control;
            _signalChartControl.Location = new System.Drawing.Point(0, 0);
            _signalChartControl.Name = "_signalChartControl";
            _signalChartControl.PeakDetector = false;
            _signalChartControl.ScaleX = 14;
            _signalChartControl.ScaleY = 10;
            _signalChartControl.Size = DrawableSize;

        }



        private Size _drawableSize;
        public Size DrawableSize
        {
            get => _drawableSize;
            set
            {
                _drawableSize = value;
                _signalChartControl.Size = _drawableSize;
            }
        }

        public void Draw(Graphics graphics)
        {
            if (_signalChannel == null) return;


            var data = _signalChannel.ReadData(_signalChannel.TotalLength - _signalChartControl.SamplesOnScreen,
                _signalChartControl.SamplesOnScreen);

            _signalChartControl.DrawSignal(data, data.Length, data.Length, totalLengthChanged, (int)_signalChannel.SamplingFrequency,
                new[] { _signalChannel.ToString() }, graphics);



            totalLengthChanged = 0;
        }

        public bool OnMouseMove(MouseEventArgs mouseEventArgs)
        {
            return false;
        }

        public bool OnMouseClick(MouseEventArgs mouseEventArgs)
        {
            return false;
        }

        public bool OnMouseDown(MouseEventArgs mouseEventArgs)
        {
            _signalChartControl.MouseDownHandler(this, mouseEventArgs);
            return false;
        }

        public bool OnMouseUp(MouseEventArgs mouseEventArgs)
        {
            _signalChartControl.MouseUpHandler(this, mouseEventArgs);
            return false;
        }
    }
}
