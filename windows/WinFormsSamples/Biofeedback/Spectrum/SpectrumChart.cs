using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.Drawing.Drawing2D;
using Neuro;

namespace Indices.Spectrum
{
	public class SpectrumChart : UserControl
	{
        private readonly BufferedGraphicsContext _screenContext = new BufferedGraphicsContext();
		private BufferedGraphics _screenBg;
        Graphics _screen;
        private IList<EEGRhythm> rhythmList;
        
		#region Масштаб и развертка
		private int _sigScale;
		public int SigScale 				// чувствительность в микровольтах на один канал ЭЭГ (+/-)
		{
			get => _sigScale;
		    set
			{
				if (_sigScale != value)
				{
					_sigScale = value;
				}
			}
		}
        #endregion

	    public double FrequencyStep { get; set; }

		public int VRulerWidth = 25;				// ширина вертикальной линейки
		public int HRulerHeight = 42;				// высота горизонтальной линейки

		Brush nameBrush = new SolidBrush(Color.Black);
		Brush backBrush = new SolidBrush(Color.LightYellow);
		Brush[] rhythmBrush;
		SolidBrush rulerBrush = new SolidBrush(SystemColors.Control);
		Font nameFont = new Font("Tahoma", 12, FontStyle.Bold, GraphicsUnit.Pixel);		// Название ритма и канала
		Font dimensionFont = new Font("Tahoma", 10, FontStyle.Bold, GraphicsUnit.Pixel);		// размерность
		Font hintFont = new Font("Tahoma", 10, FontStyle.Bold, GraphicsUnit.Pixel);
		Font rulerFont = new Font("Tahoma", 9, FontStyle.Regular, GraphicsUnit.Pixel);
		Pen rulerPenThin = new Pen(Color.Black, 1);
		Pen rulerPenThick = new Pen(Color.Black, 2);
		Pen gridPen = new Pen(Color.Gray, 1);
		Pen[] rhythmPen;

		private bool disposed = false;

		/// <summary> 
		/// Освободить все используемые ресурсы.
		/// </summary>
		protected override void Dispose(bool disposing)
		{
			if (!disposed)
			{
				if (disposing == true)
				{
					nameBrush.Dispose();
					backBrush.Dispose();
					if (rhythmBrush != null)
						for (int i = 0; i < rhythmBrush.Length; i++)
							rhythmBrush[i].Dispose();
					rhythmBrush = null;
					rulerBrush.Dispose();

					nameFont.Dispose();
					dimensionFont.Dispose();
					hintFont.Dispose();
					rulerFont.Dispose();
					rulerPenThin.Dispose();
					rulerPenThick.Dispose();
					
					gridPen.Dispose();

					if (rhythmPen != null)
						for (int i = 0; i < rhythmPen.Length; i++)
							rhythmPen[i].Dispose();
					rhythmPen = null;


					if (_screenBg != null)
					{
						_screenBg.Dispose();
						_screenBg = null;
					}
					_screenContext.Dispose();
				}
				disposed = true;
			}
			base.Dispose(disposing);
		}

		public SpectrumChart()
		{
			rhythmList = new List<EEGRhythm>
			{
			    new EEGRhythm("Delta", 0.5, 4,  Color.IndianRed, "δ"),
			    new EEGRhythm("Theta", 4, 8,  Color.Orange, "θ"),
			    new EEGRhythm("Alpha", 8, 14, Color.DodgerBlue, "α"),
			    new EEGRhythm("Beta", 14, 34,  Color.DarkOliveGreen, "β"),
                new EEGRhythm("Gamma", 34, 80, Color.DarkRed, "γ")
            };
			_sigScale = 100;
			// создаем кисти для ритмов
			rhythmBrush = new Brush[rhythmList.Count];
			rhythmPen = new Pen[rhythmList.Count];
			for (int i = 0; i < rhythmList.Count; i++)
			{
				rhythmBrush[i] = new SolidBrush(rhythmList[i].Color);
				rhythmPen[i] = new Pen(rhythmList[i].Color, 1);
			}
			AutoScaleMode = AutoScaleMode.None;
			SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint, true);
			SetStyle(ControlStyles.OptimizedDoubleBuffer, false);
			
			
			//gridPen.DashPattern = new float[] { 4.0F, 6.0F };
			gridPen.DashStyle = DashStyle.Dot;
            Resize += SpectrumChart_Resize;
		}

        private void SpectrumChart_Resize(object sender, EventArgs e)
        {
            RecreateBuffers();
        }

        private void RecreateBuffers()
		{
			if ((Height <= 0) || (Width <= 0))
				return;

			// Sets the maximum size for the primary graphics buffer
			// of the buffered graphics context for the application
			// domain.  Any allocation requests for a buffer larger 
			// than this will create a temporary buffered graphics 
			// context to host the graphics buffer.
			_screenContext.MaximumBuffer = new Size(Width + 1, Height + 1);

			if (_screenBg != null)
			{
				_screenBg.Dispose();
				_screenBg = null;
			}
			// Allocates a graphics buffer the size of this form
			// using the pixel format of the Graphics created by 
			// the Form.CreateGraphics() method, which returns a 
			// Graphics object that matches the pixel format of the form.
			_screenBg = _screenContext.Allocate(CreateGraphics(), new Rectangle(0, 0, Width, Height));
            _screen = _screenBg.Graphics;
		}

        private float _lowMarker = 8f;
        private float _highMarker = 14f;
        public void SetFreqMarkers(float low, float high)
        {
            _lowMarker = low;
            _highMarker = high;
        }

		public void DrawSpectrum(Spectrum spectrum)
		{
			if (_screenBg == null)
				RecreateBuffers();

			// необходимые переменные
			var highFreq = rhythmList[rhythmList.Count - 1].FreqEnd;		// верхняя частота спектра
			var lowFreq = rhythmList[0].FreqBegin;								// нижняя частота спектра
			var lowFreqIndex = Convert.ToInt32(Math.Round(lowFreq / FrequencyStep));			// идекс нижней частоты во входном массиве
			var sCount = Convert.ToInt32(Math.Round(highFreq / FrequencyStep));		// количество шагов частоты в спектре

		    _screen.Clear(Color.LightYellow);
	        // вычисляем размеры каждого Chart
	        var columns = 0;
	        var chHeight = 0.0;
	        var chWidth = 0.0;
	        do
	        {
	            columns++;
	            chHeight = (double) (Height - HRulerHeight);
	            chWidth = (double) (Width)/columns - VRulerWidth;
	        } while (((double) chWidth/chHeight) > 4);


	        double pixelsPerSample = (double) chWidth/sCount; // пикселей на один отсчет спектра
	        var pixelsPeruV = chHeight/_sigScale; // разрешение экрана по оси Y в пикселах на нановольт для ЭЭГ каналов
	        //				pixelsPerPOLYnv = chHeight / sigScale / 1000;		// разрешение экрана по оси Y в пикселах на нановольт для ПОЛИ каналов

	        var pixelsPerHz = pixelsPerSample/FrequencyStep; // пикселей на один Герц

	        SizeF textSize;
	        RectangleF rect = new RectangleF(); // рамка спектра канала
	        StringFormat sf = new StringFormat();
	        sf.Alignment = StringAlignment.Center;
	        sf.LineAlignment = StringAlignment.Center;
	        float X, Y;

	        // создаем path для каждого ритма через массив точек
	        GraphicsPath path;
	        PointF[] points;
	        // заполняем фон
	        for (int col = 0; col < columns; col++)
	        {
	            rect.X = (float) (col*(chWidth + VRulerWidth) + VRulerWidth);
	            rect.Y = 0;
	            rect.Width = (float) (chWidth);
	            rect.Height = Height - HRulerHeight;
	            _screen.FillRectangle(backBrush, rect); // фон спектра
	            _screen.FillRectangle(rulerBrush, rect.X - VRulerWidth, rect.Y, VRulerWidth, rect.Height); // фон линейки
	        }

	        int ch = 0;
	            int index = lowFreqIndex;

	            rect.X = (float) ((ch%columns)*(chWidth + VRulerWidth) + VRulerWidth);
	            rect.Y = (float) ((ch/columns)*chHeight);
	            rect.Width = (float) (chWidth);
	            rect.Height = (float) (chHeight);
	            if (spectrum.Data.Length > 0)
	            {
	                for (int r = 0; r < rhythmList.Count; r++)
	                {
	                    // Заполняем X и Y у всех точек
	                    int len = 3 +
	                              Convert.ToInt32(
	                                  Math.Round((rhythmList[r].FreqEnd - rhythmList[r].FreqBegin) /
	                                             FrequencyStep));
	                    points = new PointF[len];

	                    points[0].X = (float) (rect.X + rhythmList[r].FreqBegin * pixelsPerHz);
	                    points[0].Y = rect.Bottom;
	                    for (int p = 1; p < len - 1; p++)
	                    {
	                        points[p].X = (float) (points[0].X + (p - 1) * pixelsPerSample);
	                        points[p].Y = rect.Bottom - (float) (spectrum.Data[index] * pixelsPeruV * 1000);
	                        if (points[p].Y < (rect.Bottom - chHeight))
	                            points[p].Y = (float) (rect.Bottom - chHeight);
	                        index++;
	                    }

	                    index--;
	                    points[len - 1].X = (float) (rect.X + rhythmList[r].FreqEnd * pixelsPerHz);
	                    points[len - 1].Y = rect.Bottom;

	                    path = new GraphicsPath();
	                    path.AddLines(points);
	                    _screen.FillPath(rhythmBrush[r], path);
	                    _screen.DrawPath(rhythmPen[r], path);
	                }
	            }

	            // рисуем название каналов
	            textSize = _screen.MeasureString(spectrum.Name, nameFont);
	            _screen.DrawString(spectrum.Name, nameFont, nameBrush,
	                rect.Right - textSize.Width - 5, rect.Top + textSize.Height - 5);

	            // рисуем вертикальную линейку
	            _screen.DrawLine(rulerPenThin, rect.X, rect.Y, rect.X, rect.Bottom);
	            //for (float y = 0; y < chHeight; y += (float)(chHeight / 4))
	            _screen.DrawLine(rulerPenThick, rect.X, rect.Bottom - 1, rect.X - 6, rect.Bottom - 1);
	            // короткие черточки и подписи
	            double sc = 0;
	            Y = rect.Bottom;
	            for (int ry = 0; ry < 9; ry++)
	            {
	                Y = Y - (float) (chHeight/10);
	                sc = sc + _sigScale/10;

	                _screen.DrawLine(gridPen, rect.X, Y, (float)(rect.X + chWidth), Y);
	                _screen.DrawLine(rulerPenThin, rect.X, Y, rect.X - 4, Y);
	                textSize = _screen.MeasureString(sc.ToString(), rulerFont);
	                //if (sc != scaleString)
	                _screen.DrawString(sc.ToString(), rulerFont, nameBrush, rect.X - textSize.Width - 1,
	                    Y - textSize.Height/2);
	            }

	            ch++;
	        


	        //------------------------------------------
	        // рисуем горизонтальную линейку
	        // у каждого столбца своя линейка
	        _screen.FillRectangle(rulerBrush, 0, Height - HRulerHeight, Width, HRulerHeight);
	        //StringFormat sfv = new StringFormat(StringFormatFlags.DirectionVertical);

	            int freq = 0;
	            X = (float) (VRulerWidth);
	            Y = Height - HRulerHeight;
	            _screen.DrawLine(rulerPenThin, X, Y, (float) (X + chWidth), Y);
	            // длинные линии и надписи рядом с ними
	            do
	            {
	                _screen.DrawLine(rulerPenThick, X, Y, X, Y + 8);
	                if (freq != 0)
	                    _screen.DrawLine(gridPen, X, 0, X, Y);
	                textSize = _screen.MeasureString(freq.ToString(), rulerFont);
	                _screen.DrawString(freq.ToString(), rulerFont, nameBrush, X + 1 - textSize.Width/2, Y + 8);
	                X += (float) (5*pixelsPerHz);
	                freq += 5;
	                //time = time.Add(new TimeSpan(0, 0, 1));
	            } while (freq < highFreq);

	            // короткие линии
	            freq = 0;
	            X = (float) (VRulerWidth);
	            do
	            {
	                _screen.DrawLine(rulerPenThin, X, Y, X, Y + 4);
	                freq++;
	                X += (float) (pixelsPerHz);
	            } while (freq < highFreq);

                //маркеры
                var lowMarkerX = VRulerWidth + _lowMarker / highFreq * chWidth;
                _screen.DrawLine(rulerPenThin, (float)lowMarkerX, 0, (float)lowMarkerX, Height - HRulerHeight);

                var highMarkerX = VRulerWidth + _highMarker / highFreq * chWidth;
                _screen.DrawLine(rulerPenThin, (float)highMarkerX, 0, (float)highMarkerX, Height - HRulerHeight);

            // Герцы
            X = (float) (0);
	            textSize = _screen.MeasureString("Hz", nameFont);
	            _screen.DrawString("Hz", dimensionFont, nameBrush, X + 6, Y + 15);

	            // Микровольты
	            textSize = _screen.MeasureString("Hz", nameFont);
	            _screen.TranslateTransform(X, Y + 16);
	            _screen.RotateTransform(270);

	            _screen.DrawString("Hz", dimensionFont, nameBrush, 0, 0);
	            _screen.ResetTransform();

	            // рисуем ритмы
	            X = (float) (VRulerWidth);
	            rect.Y = Y + 22;
	            rect.Height = 16;
	            for (int i = 0; i < rhythmList.Count; i++)
	            {
	                rect.X = (float) (X + rhythmList[i].FreqBegin*pixelsPerHz);
	                rect.Width = (float) ((rhythmList[i].FreqEnd - rhythmList[i].FreqBegin)*pixelsPerHz);
	                _screen.FillRectangle(rhythmBrush[i], rect);
	                _screen.DrawString(rhythmList[i].Symbol, nameFont, nameBrush, rect, sf);
	            }
	        
		    _screenBg.Render(Graphics.FromHwnd(Handle));
        }

		protected override void OnPaint(PaintEventArgs e)
		{
		    _screenBg?.Render(e.Graphics);
		}
    }
}



