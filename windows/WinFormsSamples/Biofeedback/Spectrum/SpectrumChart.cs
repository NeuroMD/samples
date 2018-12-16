using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.Drawing.Drawing2D;
using Neuro;

namespace Biofeedback.Spectrum
{
	public class SpectrumChart : UserControl
	{
        // двойная буфферизация
        private BufferedGraphicsContext screenContext = new BufferedGraphicsContext();
		private BufferedGraphics screenBG;
        Graphics _screen;
		int screenWidth;
		int screenHeight;


		private Dictionary<string, double[]> spList = new Dictionary<string, double[]>();				// спектр для отображения
		private IList<EEGRhythm> rhythmList;


		#region Масштаб и развертка
		private int sigScale;
		public int SigScale 				// чувствительность в микровольтах на один канал ЭЭГ (+/-)
		{
			get
			{
				return sigScale;
			}
			set
			{
				if (sigScale != value)
				{
					sigScale = value;
					Redraw();
				}
			}
		}
        #endregion

	    public double FStep { get; set; }
		double chHeight;								// длина и ширина каждого спектра
		double chWidth;
		int columns;									// число колонок
		//int hoveredCh;									// канал, на который навели мышкой
		//RectangleF allChannelRect = new RectangleF();
		//bool allChannelHover = false;

		double pixelsPeruV;                     // пикселей на 1 В входных ЭЭГ данных 
//		double pixelsPerPOLYnv;                     // пикселей на 1 нВ входных ПОЛИ данных 
		double pixelsPerHz;							// пикселей на 1 ГЦ

		public int VRulerWidth = 25;				// ширина вертикальной линейки
		public int HRulerHeight = 42;				// высота горизонтальной линейки

		double highFreq;							// верхняя частота спектра
		double lowFreq;								// нижняя частота спектра
		int lowFreqIndex;							// идекс нижней частоты во входном массиве
		int sCount;									// количество шагов частоты в спектре

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
		Pen gridPen = new Pen(Color.DimGray, 1);
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


					if (screenBG != null)
					{
						screenBG.Dispose();
						screenBG = null;
					}
					screenContext.Dispose();
				}
				disposed = true;
			}
			base.Dispose(disposing);
		}

		public SpectrumChart()
		{
			rhythmList = new List<EEGRhythm>
			{
			    new EEGRhythm(EegStandardIndices.Delta, Color.IndianRed, "δ"),
			    new EEGRhythm(EegStandardIndices.Theta, Color.Orange, "θ"),
			    new EEGRhythm(EegStandardIndices.Alpha, Color.DodgerBlue, "α"),
			    new EEGRhythm(EegStandardIndices.Beta, Color.DarkOliveGreen, "β")
            };
			sigScale = 100;
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
			
			
			gridPen.DashPattern = new float[] { 4.0F, 6.0F };
			gridPen.DashStyle = DashStyle.Custom;
		}


		/// <summary>
		/// Активирует контрол - подключает все события, пересоздает буферы
		/// Сделано для подавления многократного вызова перерисовки при инициализации
		/// </summary>
		private void Activate()
		{
			Resize += new EventHandler(OnResize);
			/*this.MouseClick += new MouseEventHandler(OnMouseClick);
			this.MouseDown += new MouseEventHandler(OnMouseDown);
			this.MouseUp += new MouseEventHandler(OnMouseUp);
			*/

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
			screenContext.MaximumBuffer = new Size(Width + 1, Height + 1);
			screenWidth = Width;
			screenHeight = Height;

			if (screenBG != null)
			{
				screenBG.Dispose();
				screenBG = null;
			}
			// Allocates a graphics buffer the size of this form
			// using the pixel format of the Graphics created by 
			// the Form.CreateGraphics() method, which returns a 
			// Graphics object that matches the pixel format of the form.
			screenBG = screenContext.Allocate(CreateGraphics(), new Rectangle(0, 0, screenWidth, screenHeight));
            _screen = screenBG.Graphics;
		}

		public void SetSpectrumList(string channel, double[] spectrum)
		{
			if (screenBG == null)
				Activate();
		    spList[channel] = spectrum;
			// необходимые переменные
			highFreq = rhythmList[rhythmList.Count - 1].FreqEnd;		// верхняя частота спектра
			lowFreq = rhythmList[0].FreqBegin;								// нижняя частота спектра
			lowFreqIndex = Convert.ToInt32(Math.Round(lowFreq / FStep));			// идекс нижней частоты во входном массиве
			sCount = Convert.ToInt32(Math.Round(highFreq / FStep));		// количество шагов частоты в спектре

			Redraw();
		}

	    private void DrawSpectrum(Graphics screen, int left, int top, int width, int height)
	    {
            screen.Clear(Color.LightYellow);
	        // вычисляем размеры каждого Chart
	        columns = 0;
	        do
	        {
	            columns++;
	            chHeight = (double) (height - HRulerHeight - top*2)/spList.Count*columns;
	            chWidth = (double) (width - left*2)/columns - VRulerWidth;
	        } while (((double) chWidth/chHeight) > 4);


	        double pixelsPerSample = (double) chWidth/sCount; // пикселей на один отсчет спектра
	        pixelsPeruV = chHeight/sigScale; // разрешение экрана по оси Y в пикселах на нановольт для ЭЭГ каналов
	        //				pixelsPerPOLYnv = chHeight / sigScale / 1000;		// разрешение экрана по оси Y в пикселах на нановольт для ПОЛИ каналов

	        pixelsPerHz = pixelsPerSample/FStep; // пикселей на один Герц

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
	            rect.X = (float) (col*(chWidth + VRulerWidth) + VRulerWidth + left);
	            rect.Y = top;
	            rect.Width = (float) (chWidth);
	            rect.Height = height - HRulerHeight - top*2;
	            screen.FillRectangle(backBrush, rect); // фон спектра
	            screen.FillRectangle(rulerBrush, rect.X - VRulerWidth, rect.Y, VRulerWidth, rect.Height); // фон линейки
	        }

	        int ch = 0;
	        foreach (var spListKey in spList.Keys)
	        {
	            int index = lowFreqIndex;

	            rect.X = (float) ((ch%columns)*(chWidth + VRulerWidth) + VRulerWidth + left);
	            rect.Y = (float) ((ch/columns)*chHeight) + top;
	            rect.Width = (float) (chWidth);
	            rect.Height = (float) (chHeight);
	            if (spList[spListKey] != null)
	            {
	                for (int r = 0; r < rhythmList.Count; r++)
	                {
	                    // Заполняем X и Y у всех точек
	                    int len = 3 +
	                              Convert.ToInt32(
	                                  Math.Round((rhythmList[r].FreqEnd - rhythmList[r].FreqBegin)/
	                                             FStep));
	                    points = new PointF[len];

	                    points[0].X = (float) (rect.X + rhythmList[r].FreqBegin*pixelsPerHz);
	                    points[0].Y = rect.Bottom;
	                    for (int p = 1; p < len - 1; p++)
	                    {
	                        points[p].X = (float) (points[0].X + (p - 1)*pixelsPerSample);
	                        points[p].Y = rect.Bottom - (float) (spList[spListKey][index]*pixelsPeruV*1000);
	                        if (points[p].Y < (rect.Bottom - chHeight))
	                            points[p].Y = (float) (rect.Bottom - chHeight);
	                        index++;
	                    }
	                    index--;
	                    points[len - 1].X = (float) (rect.X + rhythmList[r].FreqEnd*pixelsPerHz);
	                    points[len - 1].Y = rect.Bottom;

	                    path = new GraphicsPath();
	                    path.AddLines(points);
	                    screen.FillPath(rhythmBrush[r], path);
	                    screen.DrawPath(rhythmPen[r], path);
	                }
	            }

	            // рисуем название каналов
	            textSize = screen.MeasureString(spListKey, nameFont);
	            screen.DrawString(spListKey, nameFont, nameBrush,
	                rect.Right - textSize.Width - 5, rect.Top + textSize.Height - 5);

	            /*if (SelectedChannel != -1)
					{
						allChannelRect.Y = textSize.Height * 2;
						allChannelRect.Size = SizeF.Add(
							screen.MeasureString(Properties.Resources.SpectrumChartAllChannels, rulerFont), 
							new SizeF(5, 5));
						allChannelRect.X = rect.Right - allChannelRect.Size.Width - 2;
						screen.DrawString(Properties.Resources.SpectrumChartAllChannels, 
							rulerFont, nameBrush, allChannelRect, sf);
						if (allChannelHover == true)
							screen.DrawRectangle(rulerPenThin, allChannelRect.X, allChannelRect.Y,
								allChannelRect.Width, allChannelRect.Height);
					}*/

	            // рисуем вертикальную линейку
	            screen.DrawLine(rulerPenThin, rect.X, rect.Y, rect.X, rect.Bottom);
	            //for (float y = 0; y < chHeight; y += (float)(chHeight / 4))
	            screen.DrawLine(rulerPenThick, rect.X, rect.Bottom - 1, rect.X - 6, rect.Bottom - 1);
	            // короткие черточки и подписи
	            double sc = 0;
	            Y = rect.Bottom;
	            for (int ry = 0; ry < 3; ry++)
	            {
	                Y = Y - (float) (chHeight/4);
	                sc = sc + sigScale/4;

	                //screen.DrawLine(gridPen, rect.X, Y, (float)(rect.X + chWidth), Y);
	                screen.DrawLine(rulerPenThin, rect.X, Y, rect.X - 4, Y);
	                textSize = screen.MeasureString(sc.ToString(), rulerFont);
	                //if (sc != scaleString)
	                screen.DrawString(sc.ToString(), rulerFont, nameBrush, rect.X - textSize.Width - 1,
	                    Y - textSize.Height/2);
	            }

	            ch++;
	        }


	        //------------------------------------------
	        // рисуем горизонтальную линейку
	        // у каждого столбца своя линейка
	        screen.FillRectangle(rulerBrush, left, height - HRulerHeight - top, width - left*2, HRulerHeight);
	        //StringFormat sfv = new StringFormat(StringFormatFlags.DirectionVertical);
	        for (int col = 0; col < columns; col++)
	        {
	            int freq = 0;
	            X = (float) (col*(chWidth + VRulerWidth) + VRulerWidth + left);
	            Y = height - HRulerHeight - top;
	            screen.DrawLine(rulerPenThin, X, Y, (float) (X + chWidth), Y);
	            // длинные линии и надписи рядом с ними
	            do
	            {
	                screen.DrawLine(rulerPenThick, X, Y, X, Y + 8);
	                if (freq != 0)
	                    screen.DrawLine(gridPen, X, top, X, Y);
	                textSize = screen.MeasureString(freq.ToString(), rulerFont);
	                screen.DrawString(freq.ToString(), rulerFont, nameBrush, X + 1 - textSize.Width/2, Y + 8);
	                X += (float) (5*pixelsPerHz);
	                freq += 5;
	                //time = time.Add(new TimeSpan(0, 0, 1));
	            } while (freq < highFreq);

	            // короткие линии
	            freq = 0;
	            X = (float) (col*(chWidth + VRulerWidth) + VRulerWidth + left);
	            do
	            {
	                screen.DrawLine(rulerPenThin, X, Y, X, Y + 4);
	                freq++;
	                X += (float) (pixelsPerHz);
	            } while (freq < highFreq);

	            // Герцы
	            X = (float) (col*(chWidth + VRulerWidth) + left);
	            textSize = screen.MeasureString("Hz", nameFont);
	            screen.DrawString("Hz", dimensionFont, nameBrush, X + 6, Y + 15);

	            // Микровольты
	            textSize = screen.MeasureString("Hz", nameFont);
	            screen.TranslateTransform(X, Y + 16);
	            screen.RotateTransform(270);

	            screen.DrawString("Hz", dimensionFont, nameBrush, 0, 0);
	            screen.ResetTransform();

	            // рисуем ритмы
	            X = (float) (col*(chWidth + VRulerWidth) + VRulerWidth + left);
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

	    private void Redraw()
        {
            if ((spList != null) && (screenBG != null))
            {

                DrawSpectrum(_screen, 0, 0, screenWidth, screenHeight);
				screenBG.Render(Graphics.FromHwnd(Handle));
			}
		}

		#region События
		/// <summary>
		/// Изменение размеров компонента, изменяем размеры панелей
		/// </summary>
		private void OnResize(object sender, EventArgs e)
		{
			// пересоздать графические буферы
			RecreateBuffers();
			Redraw();
		}
		/// <summary>
		/// Событие Paint - перерисовать контрол из буфера
		/// </summary>
		protected override void OnPaint(PaintEventArgs e)
		{
			if (screenBG!=null)
				screenBG.Render(e.Graphics);
		}

		#endregion

	}
}



