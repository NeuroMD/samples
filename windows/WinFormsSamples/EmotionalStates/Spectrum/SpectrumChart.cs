using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.Drawing.Drawing2D;
using EmotionalStates.Drawable;
using Neuro;

namespace EmotionalStates.Spectrum
{
	public class SpectrumChart : IDrawable
	{
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

        public Spectrum Spectrum { get; set; }

        public Size DrawableSize { get; set; }

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
		Pen gridPen = new Pen(Color.LightGray, 1);
		Pen[] rhythmPen;

		public SpectrumChart()
		{
			rhythmList = new List<EEGRhythm>
			{
			    new EEGRhythm("Delta", 0, 4,  Color.IndianRed, "δ"),
			    new EEGRhythm("Theta", 4, 8,  Color.Orange, "θ"),
			    new EEGRhythm("Alpha", 8, 14, Color.DodgerBlue, "α"),
			    new EEGRhythm("Beta", 14, 34,  Color.DarkOliveGreen, "β")
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
		}

        private float _lowMarker = 8f;
        private float _highMarker = 14f;
        public void SetFreqMarkers(float low, float high)
        {
            _lowMarker = low;
            _highMarker = high;
        }

		public void Draw(Graphics screen)
		{
			// необходимые переменные
			var highFreq = rhythmList[rhythmList.Count - 1].FreqEnd;		// верхняя частота спектра
			var lowFreq = rhythmList[0].FreqBegin;								// нижняя частота спектра
			var lowFreqIndex = Convert.ToInt32(Math.Round(lowFreq / FrequencyStep));			// идекс нижней частоты во входном массиве
			var sCount = Convert.ToInt32(Math.Round(highFreq / FrequencyStep));		// количество шагов частоты в спектре

		    screen.FillRectangle(Brushes.LightYellow, 0,1, DrawableSize.Width, DrawableSize.Height+1);
	        // вычисляем размеры каждого Chart
	        var columns = 0;
	        var chHeight = 0.0;
	        var chWidth = 0.0;
	        do
	        {
	            columns++;
	            chHeight = (double) (DrawableSize.Height - HRulerHeight);
	            chWidth = (double) (DrawableSize.Width)/columns - VRulerWidth;
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
	            rect.Height = DrawableSize.Height - HRulerHeight;
	            screen.FillRectangle(backBrush, rect); // фон спектра
	            screen.FillRectangle(backBrush, rect.X - VRulerWidth, rect.Y, VRulerWidth, rect.Height); // фон линейки
	        }

	        int ch = 0;
	            int index = lowFreqIndex;

	            rect.X = (float) ((ch%columns)*(chWidth + VRulerWidth) + VRulerWidth);
	            rect.Y = (float) ((ch/columns)*chHeight);
	            rect.Width = (float) (chWidth);
	            rect.Height = (float) (chHeight);
	            if (Spectrum.Data.Length > 0)
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
	                        points[p].Y = rect.Bottom - (float) (Spectrum.Data[index] * pixelsPeruV * 1000);
	                        if (points[p].Y < (rect.Bottom - chHeight))
	                            points[p].Y = (float) (rect.Bottom - chHeight);
	                        index++;
	                    }

	                    index--;
	                    points[len - 1].X = (float) (rect.X + rhythmList[r].FreqEnd * pixelsPerHz);
	                    points[len - 1].Y = rect.Bottom;

	                    path = new GraphicsPath();
	                    path.AddLines(points);
	                    screen.FillPath(rhythmBrush[r], path);
	                    screen.DrawPath(rhythmPen[r], path);
	                }
	            }

	     

	            // рисуем вертикальную линейку
	            screen.DrawLine(rulerPenThin, rect.X, rect.Y, rect.X, rect.Bottom);
	            //for (float y = 0; y < chHeight; y += (float)(chHeight / 4))
	            screen.DrawLine(rulerPenThick, rect.X, rect.Bottom - 1, rect.X - 6, rect.Bottom - 1);
	            // короткие черточки и подписи
	            double sc = 0;
	            Y = rect.Bottom;
	            for (int ry = 0; ry < 9; ry++)
	            {
	                Y = Y - (float) (chHeight/10);
	                sc = sc + _sigScale/10;

	                screen.DrawLine(gridPen, rect.X, Y, (float)(rect.X + chWidth), Y);
	                screen.DrawLine(rulerPenThin, rect.X, Y, rect.X - 4, Y);
	                textSize = screen.MeasureString(sc.ToString(), rulerFont);
	                //if (sc != scaleString)
	                screen.DrawString(sc.ToString(), rulerFont, nameBrush, rect.X - textSize.Width - 1,
	                    Y - textSize.Height/2);
	            }

	            ch++;
	        


	        //------------------------------------------
	        // рисуем горизонтальную линейку
	        // у каждого столбца своя линейка
	        screen.FillRectangle(backBrush, 0, DrawableSize.Height - HRulerHeight, DrawableSize.Width, HRulerHeight);
	        //StringFormat sfv = new StringFormat(StringFormatFlags.DirectionVertical);

	            int freq = 0;
	            X = (float) (VRulerWidth);
	            Y = DrawableSize.Height - HRulerHeight;
	            screen.DrawLine(rulerPenThin, X, Y, (float) (X + chWidth), Y);
	            // длинные линии и надписи рядом с ними
	            do
	            {
	                screen.DrawLine(rulerPenThick, X, Y, X, Y + 8);
	                if (freq != 0)
	                    screen.DrawLine(gridPen, X, 0, X, Y);
	                textSize = screen.MeasureString(freq.ToString(), rulerFont);
	                screen.DrawString(freq.ToString(), rulerFont, nameBrush, X + 1 - textSize.Width/2, Y + 8);
	                X += (float) (5*pixelsPerHz);
	                freq += 5;
	                //time = time.Add(new TimeSpan(0, 0, 1));
	            } while (freq < highFreq);

	            // короткие линии
	            freq = 0;
	            X = (float) (VRulerWidth);
	            do
	            {
	                screen.DrawLine(rulerPenThin, X, Y, X, Y + 4);
	                freq++;
	                X += (float) (pixelsPerHz);
	            } while (freq < highFreq);

                // рисуем название каналов
                textSize = screen.MeasureString(Spectrum.Name, nameFont);
                screen.DrawString(Spectrum.Name, nameFont, nameBrush,
                    rect.Right - textSize.Width - 5, rect.Top + textSize.Height - 5);
            // рисуем ритмы
            X = (float) (VRulerWidth);
	            rect.Y = Y + 22;
	            rect.Height = 16;
	            for (int i = 0; i < rhythmList.Count; i++)
	            {
	                rect.X = (float) (X + rhythmList[i].FreqBegin*pixelsPerHz);
	                rect.Width = (float) ((rhythmList[i].FreqEnd - rhythmList[i].FreqBegin)*pixelsPerHz);
	                screen.FillRectangle(rhythmBrush[i], rect);
	                screen.DrawString(rhythmList[i].Symbol, nameFont, nameBrush, rect, sf);
	            }
                
        }

    }
}



