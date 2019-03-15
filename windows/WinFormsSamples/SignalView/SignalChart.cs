using System;
using System.Drawing;
using System.Windows.Forms;

namespace SignalView
{
	public partial class SignalChart : UserControl
	{
		private BufferedGraphicsContext context;
		private BufferedGraphics grafx;

		// необходимые цвета
		Color BackgroundColor = Color.WhiteSmoke;//Color.FromArgb(255, 239, 198);
		// необходимые ручки кисти
		Pen BlackPen = new Pen(Color.Black, 1);
		Pen RedPen = new Pen(Color.Red, 1);
		Pen BlackPen2 = new Pen(Color.Black, 2);
		Pen GridPen = new Pen(Color.DarkGray, 1); //сетка графика
		Pen GridPen2 = new Pen(Color.DarkGray, 1); //сетка графика
		Pen CursorPen = new Pen(Color.Blue, 1);
		//Pen redPen = new Pen(Color.Red, 1);
		SolidBrush BackgroundBrush;// = new SolidBrush(Color.FromArgb(255, 239, 198));
		SolidBrush BlackBrush = new SolidBrush(Color.Black);
		SolidBrush BlueBrush = new SolidBrush(Color.Blue);
		SolidBrush RedBrush = new SolidBrush(Color.Red);
		SolidBrush ActivBrush = new SolidBrush(SystemColors.GradientActiveCaption);
		//SolidBrush OverBrush = new SolidBrush(SystemColors.GradientInactiveCaption);

		// шрифты
		Font AxisFont = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular);		// шрифт подписей на осях
		Font scFont = new Font("Microsoft Sans Serif", 10f, FontStyle.Bold);			// шрифт контрола управления масштабом
		Font scFont2 = new Font("Microsoft Sans Serif", 7f, FontStyle.Bold);			// шрифт контрола управления масштабом

		// флаг, что мышь нажата
		bool isMouseDown = false;
		const int apHeightX = 70;	// высота панели оси X
		const int apWidthY = 80;	// ширина панели оси Y
		const int mpHeightX = 10;	// высота верхней панели маркера		
		const int mpWidthY = 10;	// ширина правой панели маркера	

		Point pointLeftUp = new Point(apWidthY, mpHeightX);		// левый, верхний угол координатной сетки
		Point pointRightDown = new Point(400, 400);				// правый, нижний угол
		Point pointOrigin = new Point(200, 200);				// центр координат

		// переменные контрола управления масштабом (ScaleControl)
		int scWidth = 15;	// размер квадратов в контроле управления масштабом
		int scHoverX = 0;
		int scHoverLastX = 0;
		Point scPosX = new Point(30, 30);
		int scHoverY = 0;
		int scHoverLastY = 0;
		Point scPosY = new Point(20, 40);
		
		// панель информации
		Point ipPos = new Point(40, 40);
		int ipWidth = 220;
		//int ipHeight = 100;

		// размеры сеток
		int gridStepX = 100;		// шаг в пикселах вывода сетки
		int gridStepY = 20;			// шаг в пикселах вывода сетки 
		int gridCountX = 5;			// число ячеек сетки на экране
		int gridCountY = 10;		// число ячеек сетки на экране
		// переменные масштабов для разных осей
		int scaleX = 14;			// масштаб по оси X
		//float shiftXuS = 0;			// сдвиг графика от начала координат в микросекундах
		int scaleLastX = 0;
		const int scaleCountX = 21;	// шагов масштаба по X

		int scaleY = 10;			// масштаб по оси Y
		int scaleLastY = 0;
		int scaleCountY = 21;	// шагов масштаба по Y

		string[] scaleTime = { "us", "ms", "s" };	//номинал по оси Y
		string[] scaleVolts = { "uV", "mV", "V" };	//номинал по оси Y

		float[] scaleBufY = { 0.1f, 0.2f, 0.5f, 1.0f, 2.0f, 5.0f, 10f, 20f, 50f };   //цена деления по оси У 
		//сколько нановольт в делении        
		float[] scaleYnV = { 100, 200, 500, 1000, 2000, 5000, 10000, 20000, 50000, 100000, 200000, 500000, 1000000, 2000000, 5000000, 10000000, 20000000, 50000000, 100000000, 200000000, 500000000, 1000000000 };
		double[] scaleBufX = { 1.0, 2.0, 5.0, 10, 20, 50, 0.1, 0.2, 0.5 };   //цена деления по оси X (у флоата не хватает точности, появляются дробные хвосты)
		string[] scaleBufStringX = { " 1", " 2", " 5", "10", "20", "50", ".1", ".2", ".5", " 1", " 2", " 5", "10", "20", "50", ".1", ".2", ".5", " 1", " 2", " 5", "" };
		string[] scaleBufStringY = { ".1", ".2", ".5", " 1", " 2", " 5", "10", "20", "50", ".1", ".2", ".5", " 1", " 2", " 5", "10", "20", "50", ".1", ".2", ".5", "" };   

		//сколько микросекунд на дел. 
		float[] scaleXuS = { 1, 2, 5, 10, 20, 50, 100, 200, 500, 1000, 2000, 5000, 10000, 20000, 50000, 100000, 200000, 500000, 1000000, 2000000, 5000000 };

		// переменные, связанные с текущим отображением (масштабом)
		float uSPixel;			// микросекунд на пиксел
		float pixelsBetweenSampels;		// количество пикселов между соседними отсчетами входных данных
		double nVPixel;			// нановольт на пиксел
		float shiftYnV;			// сдвиг по оси Y в нановольтах
		int samplesOnScreen;	// столько отсчетов входного сигнала помещается на экран
		long nVOnScreen;			// столько нановольт помещается на экран
		//переменные, связаные со входным массивом
		double[] inputBuf = null;    //указатель на входной массив данных
		float inputTuS = 100;  //период дискретизации входного сигнала в микросекундах
		int inputTimeOffset = 0;	// сдвиг входного сигнала в микросекундах
		int inputLength = 0;    //длина входного сигнала 
		int inputIndex;
		string[] inputMessage = null;	// указатель на входной массив сообщений
		// переменные для курсоров
		int cursorUpY = -150;			// позиция верхнего курсора (отсчитываются от центра)
		int cursorDownY = 150;		// позиция нижнего курсора (отсчитываются от центра)
		bool cursorUpActiv = false;		// курсор выбран
		bool cursorDownActiv = false;	// курсор выбран

		// различные флаги (режимы работы)
		bool peakDetector = false;		// режим отображения при усреднении - пиковый детектор

		// свойства компонента
		public int ScaleX
		{
			get { return scaleX; }
			set { scaleX = value; }
		}
		public int ScaleY
		{
			get { return scaleY; }
			set { scaleY = value; }
		}
		public bool PeakDetector
		{
			get { return peakDetector; }
			set { peakDetector = value; }
		}
		public int SamplesOnScreen
		{
			get { return samplesOnScreen; }
		}


		public SignalChart()
		{
			BackgroundBrush = new SolidBrush(BackgroundColor);
			GridPen.DashStyle = System.Drawing.Drawing2D.DashStyle.Dash;
			this.MinimumSize = new System.Drawing.Size(gridStepX * 4 + apWidthY + 20, gridStepY * 18 + apHeightX + 10);
			InitializeComponent();

			XScrollRightButton.Text = ">";
			XScrollLeftButton.Text = "<";
			YScrollUpButton.Text = "^";
			YScrollDownButton.Text = "v";

			this.MouseDown += new MouseEventHandler(this.MouseDownHandler);
			this.MouseMove += new MouseEventHandler(this.MouseMoveHandler);	
			this.MouseUp += new MouseEventHandler(this.MouseUpHandler);
			this.Resize += new EventHandler(this.OnResize);
			this.SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint, true);
			this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);

			// Retrieves the BufferedGraphicsContext for the 
			// current application domain.
			context = BufferedGraphicsManager.Current;
			context.MaximumBuffer = new Size(this.Width + 1, this.Height + 1);
			grafx = context.Allocate(this.CreateGraphics(), new Rectangle(0, 0, this.Width, this.Height));
			
			RecalcVariables();
			// Draw the first frame to the buffer.
			RedrawScreen(grafx.Graphics);	
		}

		public void DrawSignal(double[] data, int index, int length, int fSample, string[] message)
		{
			DrawSignal(data, index, length, 0, fSample, message);
		}
		/// </summary>
		/// функция передачи параметров в элемент отображения
		/// </summary>
		public void DrawSignal(double[] data, int index, int length, int timeOffset,int fSample,  string[] message)
		{
			//for (int i = 0; i < length; i++)
			//  input[i] = data[i];
			inputBuf = data; //запомнить указатель на массив
			inputTuS = 1000000f / fSample;
			inputLength = length;
			inputTimeOffset = timeOffset;


			RecalcVariables();
			// вычисляем сдвиг по оси X в зависимости от заданого index
			if ((length - index) < (samplesOnScreen/2))
				index = length - samplesOnScreen/2;
			if (index < 0)
				index = 0;

			inputIndex = index;

			inputMessage = message;

            //после передачи - перерисовать
            try
            {
                RedrawScreen(grafx.Graphics);
            }
            catch { }

            this.Refresh();
		}



		private void RecalcVariables()
		{
			uSPixel = scaleXuS[scaleX] / gridStepX;		// микросекунд на пиксел
			pixelsBetweenSampels = (float)(inputTuS / uSPixel);		// количество пикселов между соседними отсчетами входных данных
			nVPixel = scaleYnV[scaleY] / gridStepY;		// нановольт на пиксел

			samplesOnScreen = 2 * Convert.ToInt32(Math.Ceiling((pointOrigin.X - pointLeftUp.X) / pixelsBetweenSampels));

			nVOnScreen = 2 * Convert.ToInt64(Math.Ceiling((pointOrigin.Y - pointLeftUp.Y) *nVPixel));
		}

		/// <summary>
		/// Изменение размеров компонента, изменяем размеры панелей
		/// </summary>
		private void OnResize(object sender, EventArgs e)
		{
			// Re-create the graphics buffer for a new window size.
			context.MaximumBuffer = new Size(this.Width + 1, this.Height + 1);
			if (grafx != null)
			{
				grafx.Dispose();
				grafx = null;
			}
			grafx = context.Allocate(this.CreateGraphics(), new Rectangle(0, 0, this.Width, this.Height));

			// определяем размеры экрана и координаты трех ключевых точек
			// координаты точки pointLeftUp заданы константами вверху и не переопределяются
			// размеры области должны быть четными, т.е. влево и вправо от центра должно быть одинаковое растояние
			pointRightDown.X = (this.Width - pointLeftUp.X - mpWidthY) / 2;
			pointRightDown.X = pointLeftUp.X + pointRightDown.X * 2;
			pointRightDown.Y = (this.Height - apHeightX) / 2;
			pointRightDown.Y = pointLeftUp.Y + pointRightDown.Y * 2;
			pointOrigin.X = pointLeftUp.X + (pointRightDown.X - pointLeftUp.X) / 2;
			pointOrigin.Y = pointLeftUp.Y + (pointRightDown.Y - pointLeftUp.Y) / 2;

			// все переменные линиий (шагов сетки) отсчитываются относительно 0 координат (центра)
			// т.е. реально на эране линий будет в два раза больше, в право и влево от центра
			// определяем, сколько шагов сетки (линий) помещается по вертикали
			gridCountY = (pointOrigin.Y - pointLeftUp.Y) / gridStepY ;
			// определяем, сколько шагов сетки (линий) помещается по горизонтали
			gridCountX = (pointOrigin.X - pointLeftUp.X) / gridStepX;

			// панели масштаба
			scPosX = new Point(pointLeftUp.X - 30, pointRightDown.Y + 30);
			scPosY.Y = pointRightDown.Y + 45 - scaleCountY*scWidth;
			
			// панель информации
			ipPos = new Point(pointRightDown.X - ipWidth, pointLeftUp.Y);
			// определяем позицию кнопок прокрутки
			XScrollRightButton.Left = this.Width - 10 - XScrollRightButton.Width;
			XScrollRightButton.Top = scPosX.Y;
			XScrollLeftButton.Left = XScrollRightButton.Left - 10 - XScrollRightButton.Width;
			XScrollLeftButton.Top = XScrollRightButton.Top;
			
			YScrollUpButton.Left = 10;
			YScrollUpButton.Top = 10;
			YScrollDownButton.Left = YScrollUpButton.Left;
			YScrollDownButton.Top = YScrollUpButton.Top + 10 + YScrollUpButton.Height;

			RecalcVariables();
			RedrawScreen(grafx.Graphics);
			this.Refresh();
		}

		// перерисовать экран
		private void RedrawScreen(Graphics g)
		{			
			// вычисляем сдвиг по оси X в пикселах
			// для этого умножаем inputIndex на inputTuS (получаем сдвиг в микросекундах) и делим на uSPixel
			float shiftXpix = (-inputIndex * inputTuS) / uSPixel;
			// вычисляем сдвиг по оси Y в пикселах
			// для этого shiftYnV (сдвиг в нановольтах) делим на nVPixel
			float shiftYpix = (float)(shiftYnV / nVPixel);

			// Перерисовать панель сигнала
			// заливаем фон
			g.FillRectangle(BackgroundBrush, this.ClientRectangle);
			// оси и границы графика
			g.DrawRectangle(BlackPen, pointLeftUp.X, pointLeftUp.Y,
				pointRightDown.X - pointLeftUp.X, pointRightDown.Y - pointLeftUp.Y);
			//------------------------------------------------------------------------------------------
			// вертикальные линии
			// рисуем точки от нуля (центра) в плюс (вправо) и в минус (влево)
			StringFormat graphTextFormat = new StringFormat();  //формат вывода значений
			//задание формата вывода текста по оси X
			graphTextFormat.Alignment = StringAlignment.Center;
			graphTextFormat.LineAlignment = StringAlignment.Center;
			// текущий номер отображаемой линии сетки
			float shiftXpixStr = (-(inputIndex + inputTimeOffset) * inputTuS) / uSPixel;
			int index = Convert.ToInt32(Math.Ceiling(-shiftXpixStr / gridStepX)) - gridCountX;
			float pointGridX = shiftXpixStr + pointOrigin.X + index * gridStepX;
			do
			{
				// длинные линии
				g.DrawLine(GridPen, pointGridX, pointLeftUp.Y, pointGridX, pointRightDown.Y + 4);
				// подписи
				g.DrawString((scaleBufX[scaleX % 9] * index).ToString(), AxisFont, BlackBrush,
					pointGridX, pointRightDown.Y + 15, graphTextFormat);
				index++;
				pointGridX = pointGridX + gridStepX;
			} while (pointGridX < pointRightDown.X);
			//------------------------------------------------------------------------------------------
			//горизонтальные линии линии и надписи возле них
			graphTextFormat.Alignment = StringAlignment.Far;
			graphTextFormat.LineAlignment = StringAlignment.Center;
			graphTextFormat.FormatFlags = StringFormatFlags.NoClip;
			// текущий номер отображаемой линии сетки
			index = Convert.ToInt32(Math.Ceiling(-shiftYpix / gridStepY)) - gridCountY;
			float pointGridY = shiftYpix + pointOrigin.Y + index * gridStepY;
			do
			{
				// длинные линии
				g.DrawLine(GridPen, pointLeftUp.X - 4, pointGridY, pointRightDown.X, pointGridY);
				// короткие линии
				for (int i = 1; i < 5;i++ )
					g.DrawLine(GridPen2, pointLeftUp.X - 2, pointGridY + gridStepY / 5 * i, pointLeftUp.X, pointGridY + gridStepY / 5 * i);

				// подписи
				g.DrawString((-scaleBufY[scaleY % 9] * index).ToString(), AxisFont, BlackBrush,
					pointLeftUp.X - 6, pointGridY, graphTextFormat);
				index++;
				pointGridY = pointGridY + gridStepY;
			} while (pointGridY < pointRightDown.Y);
			
			//------------------------------------------------------------------------------------------
			// рисуем контрол управления масштабом оси X
			g.FillRectangle(ActivBrush, scPosX.X, scPosX.Y, scWidth * (scaleX + 1), scWidth);
			graphTextFormat.Alignment = StringAlignment.Center;
			graphTextFormat.LineAlignment = StringAlignment.Center;
			for (int i = 0; i <= scaleCountX; i++)
			{
				if ((i == 6) | (i == 15))
					g.DrawLine(BlackPen2, scPosX.X + i * scWidth, scPosX.Y - 3, scPosX.X + i * scWidth, scPosX.Y + scWidth + 3);
				else
					g.DrawLine(BlackPen, scPosX.X + i * scWidth, scPosX.Y, scPosX.X + i * scWidth, scPosX.Y + scWidth);
				// надпись в клеточке
				g.DrawString(scaleBufStringX[i], scFont2, BlackBrush,
					scPosX.X + i * scWidth, scPosX.Y + 2);
			}

			g.DrawLine(BlackPen, scPosX.X, scPosX.Y, scPosX.X + scaleCountX * scWidth, scPosX.Y);
			g.DrawLine(BlackPen, scPosX.X, scPosX.Y + scWidth, scPosX.X + scaleCountX * scWidth, scPosX.Y + scWidth);
			// если мышь над контролом - выделяем его
			if (scHoverX != 0)
			{
				g.DrawLine(BlackPen, scPosX.X + 1, scPosX.Y, scPosX.X + 1, scPosX.Y + scWidth);
				g.DrawLine(BlackPen, scPosX.X + scaleCountX * scWidth - 1, scPosX.Y, scPosX.X + scaleCountX * scWidth - 1, scPosX.Y + scWidth);
				g.DrawLine(BlackPen, scPosX.X, scPosX.Y + 1, scPosX.X + scaleCountX * scWidth, scPosX.Y + 1);
				g.DrawLine(BlackPen, scPosX.X, scPosX.Y + scWidth - 1, scPosX.X + scaleCountX * scWidth, scPosX.Y + scWidth - 1);
			}
			// подписи под контролом
			g.DrawString(scaleTime[0], scFont, BlackBrush, scPosX.X + scWidth * 2, scPosX.Y + scWidth - 3);
			g.DrawString(scaleTime[1], scFont, BlackBrush, scPosX.X + scWidth * 10, scPosX.Y + scWidth - 3);
			g.DrawString(scaleTime[2], scFont, BlackBrush, scPosX.X + scWidth * 18, scPosX.Y + scWidth - 3);
			
			//------------------------------------------------------------------------------------------
			// рисуем контрол управления масштабом оси Y
			g.FillRectangle(ActivBrush, scPosY.X, scPosY.Y + scWidth * (scaleCountY - scaleY-1),
				scWidth, scWidth * (scaleY + 1));
			for (int i = 0; i <= scaleCountY; i++)
			{
				if ((i == 3) | (i == 12))
					g.DrawLine(BlackPen2, scPosY.X - 3, scPosY.Y + i * scWidth, scPosY.X + scWidth + 3, scPosY.Y + i * scWidth);
				else
					g.DrawLine(BlackPen, scPosY.X, scPosY.Y + i * scWidth, scPosY.X + scWidth, scPosY.Y + i * scWidth);
			}
			g.DrawLine(BlackPen, scPosY.X, scPosY.Y, scPosY.X, scPosY.Y + scaleCountY * scWidth);
			g.DrawLine(BlackPen, scPosY.X + scWidth, scPosY.Y, scPosY.X + scWidth, scPosY.Y + scaleCountY * scWidth);
			if (scHoverY != 0)
			{
				g.DrawLine(BlackPen, scPosY.X+1, scPosY.Y, scPosY.X+1, scPosY.Y + scaleCountY * scWidth);
				g.DrawLine(BlackPen, scPosY.X + scWidth-1, scPosY.Y, scPosY.X + scWidth-1, scPosY.Y + scaleCountY * scWidth);
				g.DrawLine(BlackPen, scPosY.X, scPosY.Y + 1, scPosY.X + scWidth, scPosY.Y + 1);
				g.DrawLine(BlackPen, scPosY.X, scPosY.Y + scaleCountY * scWidth-1, 
					scPosY.X + scWidth, scPosY.Y + scaleCountY * scWidth-1);
			}

			g.TranslateTransform(0, scPosY.Y + scWidth * scaleCountY);
			g.RotateTransform(-90); // производим поворот на -90
			g.DrawString(scaleVolts[0], scFont, BlackBrush, scWidth * 4, 5);
			g.DrawString(scaleVolts[1], scFont, BlackBrush, scWidth * 13, 5);
			g.DrawString(scaleVolts[2], scFont, BlackBrush, scWidth * 19, 5);
			for (int i = 0; i <= scaleCountY; i++)
			{
				// надпись в клеточке
				g.DrawString(scaleBufStringY[i], scFont2, BlackBrush,
					i * scWidth, 22);
			}
			g.ResetTransform();

			//-------------------------------------------------------------------------------------------------------
			// Перерисовать панель информации
			// выводим информационные строки из буфера inputMessage
			if (inputMessage != null)
			{
				for (int i = 0; i < inputMessage.Length; i++)
					g.DrawString(inputMessage[i], scFont, BlackBrush, ipPos.X, ipPos.Y + 20*i);	
			}
			
			//-------------------------------------------------------------------------------------------------------
			// отображаем график сигнала
			//отображать только если есть реальная ссылка на входное значение (сначала - не будет)
			if (inputBuf != null)
			{
				Pen sigPen;
				index = -samplesOnScreen / 2;
				if ((index + inputIndex) < 0)
					index = -inputIndex;
				// до попадания сюда уже известно с какого индекса отображать входной буфер
				// отображаем до тех пор, пока точки не вылезают за пределы экрана
				//если период входного сигнала меньше, чем расстояние между соседними пикселами
				//то на один пиксел графика отображать несколько пикселов входного сигнала                        
				if (inputTuS < uSPixel)
				{
					if (peakDetector == true)
					{
						// если растояние между соседними точками меньше одного пиксела
						// то график строится из кусочков вертикальных линий
						// при этом каждая вертикальная линия соединяет минимум и максимум из всех точек, попадающих на пиксел
						// если на некоторые пикселы будет приходится только один входной отсчет, то в этом месте будет точка
						PointF signalPointMax = new PointF(0, 0);
						PointF signalPointMin = new PointF(0, 0);
						// у min и max значение координаты X одинаковое
						signalPointMax.X = (float)Math.Floor(shiftXpix + pointOrigin.X + (float)((index + inputIndex) * pixelsBetweenSampels));
						signalPointMin.X = signalPointMax.X;
						float currY = 0;
						do
						{
							// вычисляем координату по Х
							// для этого умножаем текущий index входного буфера на период сигнала
							// вычитаем смещение координат и делим на разрешение (uSPixel)
							currY = (float)(inputBuf[index + inputIndex]*1e9);
							signalPointMax.Y = currY;
							signalPointMin.Y = currY;
							// перебираем все точки, приходящиеся на этот пиксел
							do
							{
								currY = (float)(inputBuf[index + inputIndex] * 1e9);
                                signalPointMax.Y = Math.Max(signalPointMax.Y, currY);
								signalPointMin.Y = Math.Min(signalPointMin.Y, currY);
								index++;
								if ((index + inputIndex) >= inputLength)
									break;
								//if (index > 0))
							} while ((pointOrigin.X + (index * pixelsBetweenSampels)) < signalPointMin.X);

							signalPointMax.Y = (float)(shiftYpix + pointOrigin.Y - (signalPointMax.Y / nVPixel));
							signalPointMin.Y = (float)(shiftYpix + 0.1f + pointOrigin.Y - (signalPointMin.Y / nVPixel));
							sigPen = BlackPen;

							if (signalPointMax.Y < pointLeftUp.Y)
							{
								sigPen = RedPen;
								signalPointMax.Y = pointLeftUp.Y;
							}
							if (signalPointMax.Y > pointRightDown.Y)
							{
								sigPen = RedPen;
								signalPointMax.Y = pointRightDown.Y;
							}
							if (signalPointMin.Y < pointLeftUp.Y)
							{
								sigPen = RedPen;
								signalPointMin.Y = pointLeftUp.Y;
							}
							if (signalPointMin.Y > pointRightDown.Y)
							{
								sigPen = RedPen;
								signalPointMin.Y = pointRightDown.Y;
							}


							g.DrawLine(sigPen, signalPointMax, signalPointMin);
							signalPointMax.X++;
							signalPointMin.X++;

							if ((index + inputIndex) >= inputLength)
								break;
						} while (signalPointMax.X < pointRightDown.X);
					}
					else 
					{
						PointF signalPointLast = new PointF(0, 0);
						PointF signalPoint = new PointF(0, 0);
						// вычисляем, сколько точек сигнала поместится справа и слева от центра
						// рисуем точки от нуля (центра) в плюс (вправо) и в минус (в лево)
						do
						{
							signalPoint.X = (float)Math.Floor(shiftXpix + pointOrigin.X + (float)((index + inputIndex) * pixelsBetweenSampels));
							int M = 0;
							signalPoint.Y = 0;
							// перебираем все точки, приходящиеся на этот пиксел
							do
							{
								M++;
								signalPoint.Y += (float)(inputBuf[index + inputIndex] * 1e9);
                                index++;
								if ((index + inputIndex) >= inputLength)
									break;
								//if (index > 0))
							} while ((pointOrigin.X + (index * pixelsBetweenSampels)) < signalPoint.X);
							signalPoint.Y = (float)(shiftYpix + pointOrigin.Y - (signalPoint.Y / nVPixel/M));

							sigPen = BlackPen;

							if (signalPoint.Y < pointLeftUp.Y)
							{
								sigPen = RedPen;
								signalPoint.Y = pointLeftUp.Y;
							}
							if (signalPoint.Y > pointRightDown.Y)
							{
								sigPen = RedPen;
								signalPoint.Y = pointRightDown.Y;
							}
							if (signalPointLast.X != 0)
							{
								g.DrawLine(sigPen, signalPointLast, signalPoint);
							}
							// если между точками больше 10 пикселей, то рисуем квадратики
							if (pixelsBetweenSampels >= 10)
								g.DrawRectangle(sigPen, signalPoint.X - 1, signalPoint.Y - 1, 2.0f, 2.0f);



							signalPointLast = signalPoint;
							if ((index + inputIndex) >= inputLength)
								break;

						} while (signalPoint.X < pointRightDown.X);
					}

				}
				else
				// если период входного сигнала больше или равен периоду пиксела,
				// то отображать по одному отсчету входного сигнала на каждый пиксел или через несколько пикселов
				{
					PointF signalPointLast = new PointF(0, 0);
					PointF signalPoint = new PointF(0, 0);
					// вычисляем, сколько точек сигнала поместится справа и слева от центра
					// рисуем точки от нуля (центра) в плюс (вправо) и в минус (в лево)
					do
					{
						// вычисляем координату по Х
						// для этого умножаем текущий (index + inputIndex) входного буфера на период сигнала
						// и прибавляем сдвиг и координаты центра
						sigPen = BlackPen;
						signalPoint.X = shiftXpix + pointOrigin.X + (float)((index + inputIndex) * pixelsBetweenSampels);
						signalPoint.Y = (float)(shiftYpix + pointOrigin.Y - ((float)(inputBuf[index + inputIndex] * 1e9) / nVPixel));
						if (signalPoint.Y < pointLeftUp.Y)
						{
							sigPen = RedPen;
							signalPoint.Y = pointLeftUp.Y;
						}
						if (signalPoint.Y > pointRightDown.Y)
						{
							sigPen = RedPen;
							signalPoint.Y = pointRightDown.Y;
						}
						if (signalPointLast.X != 0)
						{
							g.DrawLine(sigPen, signalPointLast, signalPoint);
						}
						// если между точками больше 10 пикселей, то рисуем квадратики
						if (pixelsBetweenSampels >= 10)
							g.DrawRectangle(sigPen, signalPoint.X - 1, signalPoint.Y - 1, 2.0f, 2.0f);
						


						signalPointLast = signalPoint;
						index++;
						if ((index + inputIndex) >= inputLength)
							break;

					} while (signalPoint.X < pointRightDown.X);

				}
			}

			// отображаем курсоры
			float cursorUpValue = (float)Math.Round(scaleBufY[scaleY % 9] * ((float)(shiftYpix - cursorUpY) / (float)gridStepY), 2);
			float cursorDownValue = (float)Math.Round(scaleBufY[scaleY % 9] * ((float)(shiftYpix - cursorDownY) / (float)gridStepY), 2);
			//отображение верхнего курсора 
			g.DrawLine((cursorUpActiv ? RedPen : CursorPen), pointLeftUp.X, pointOrigin.Y + cursorUpY, 
				pointRightDown.X, pointOrigin.Y + cursorUpY);
			g.DrawString(cursorUpValue.ToString(), scFont, (cursorUpActiv ? RedBrush : BlueBrush),
				pointRightDown.X-20, pointOrigin.Y + cursorUpY-8, graphTextFormat);
			//отображение нижнего курсора
			g.DrawLine((cursorDownActiv ? RedPen : CursorPen), pointLeftUp.X, pointOrigin.Y + cursorDownY, 
				pointRightDown.X, pointOrigin.Y + cursorDownY);
			g.DrawString(cursorDownValue.ToString(), scFont, (cursorDownActiv ? RedBrush : BlueBrush),
				pointRightDown.X -20, pointOrigin.Y + cursorDownY-8, graphTextFormat);
 
		}

		protected override void OnPaint(PaintEventArgs e)
		{
			grafx.Render(e.Graphics);
		}

		
		// Движение мышки над формой, подсвечиваем органы упрвления
		private void MouseMoveHandler(object sender, MouseEventArgs e)
		{
			// проверка наведения на контрол управления масштабом по X
			scHoverX = 0;
			if ((e.X > scPosX.X) &
				(e.X < (scPosX.X + scaleCountX * scWidth)) &
				(e.Y > scPosX.Y) &
				(e.Y < (scPosX.Y + scWidth)))
			{
				scHoverX = 1;

				if (e.Button == System.Windows.Forms.MouseButtons.Left)
				{
					scaleX = (e.X - scPosX.X) / scWidth;
					RecalcVariables();			// пересчитываем смещение начала координат по X
				}

			}
			if ((scHoverLastX != scHoverX) | (scaleLastX != scaleX))
			{
				RedrawScreen(grafx.Graphics);
				this.Refresh();
			}
			scaleLastX = scaleX;
			scHoverLastX = scHoverX;

			// проверка наведения на контрол управления масштабом по Y
			scHoverY = 0;
			if ((e.X > scPosY.X) &
				(e.X < (scPosY.X + scWidth)) &
				(e.Y > scPosY.Y) &
				(e.Y < (scPosY.Y + scaleCountY * scWidth)))
			{
				scHoverY = 1;

				if (e.Button == System.Windows.Forms.MouseButtons.Left)
				{
					scaleY = scaleCountY - (e.Y - scPosY.Y) / scWidth - 1;
					RecalcVariables();			// пересчитываем смещение начала координат по Y
				}
			}
			if ((scHoverLastY != scHoverY) | (scaleLastY != scaleY))
			{
				RedrawScreen(grafx.Graphics);
				this.Refresh();
			}
			scaleLastY = scaleY;
			scHoverLastY = scHoverY;

			// подсветка курсоров
			// верхний курсор
			if (isMouseDown == false)
			{
				if (
					(e.Y > pointOrigin.Y + cursorUpY - 3) && (e.Y < pointOrigin.Y + cursorUpY + 3) &&
					(e.X > pointLeftUp.X + 5) && (e.X < pointRightDown.X - 5)
					)
				{
					if (cursorUpActiv == false)
					{
						cursorUpActiv = true;
						RedrawScreen(grafx.Graphics);
						this.Refresh();
					}
				}
				else
				{
					if (cursorUpActiv == true)
					{
						cursorUpActiv = false;
						RedrawScreen(grafx.Graphics);
						this.Refresh();
					}
				}


				// нижний курсор
				if (
					(e.Y > pointOrigin.Y + cursorDownY - 3) && (e.Y < pointOrigin.Y + cursorDownY + 3) &&
					(e.X > pointLeftUp.X + 5) && (e.X < pointRightDown.X - 5)
					)
				{
					if (cursorDownActiv == false)
					{
						cursorDownActiv = true;
						RedrawScreen(grafx.Graphics);
						this.Refresh();
					}
				}
				else
				{
					if (cursorDownActiv == true)
					{
						cursorDownActiv = false;
						RedrawScreen(grafx.Graphics);
						this.Refresh();
					}
				}
			}
			else
			{
				if (cursorUpActiv == true)
				{
					cursorUpY = e.Y - pointOrigin.Y;
					if (cursorUpY < (pointLeftUp.Y - pointOrigin.Y))
						cursorUpY = (pointLeftUp.Y - pointOrigin.Y);
					if (cursorUpY > (pointRightDown.Y - pointOrigin.Y))
						cursorUpY = (pointRightDown.Y - pointOrigin.Y);
					RedrawScreen(grafx.Graphics);
					this.Refresh();
				}
				if (cursorDownActiv == true)
				{
					cursorDownY = e.Y - pointOrigin.Y;
					if (cursorDownY < (pointLeftUp.Y - pointOrigin.Y))
						cursorDownY = (pointLeftUp.Y - pointOrigin.Y);
					if (cursorDownY > (pointRightDown.Y - pointOrigin.Y))
						cursorDownY = (pointRightDown.Y - pointOrigin.Y);
					cursorDownY = e.Y - pointOrigin.Y;
					RedrawScreen(grafx.Graphics);
					this.Refresh();
				}
			}
			
		}
		// щелчек мышки на форме, проверяем, не щелкнули ли по органам управления
		private void MouseDownHandler(object sender, MouseEventArgs e)
		{
			isMouseDown = true;
			// шелчек мышкой по контролу управления масштабом по X
			if ((e.X > scPosX.X) &
				(e.X < (scPosX.X + scaleCountX * scWidth)) &
				(e.Y > scPosX.Y) &
				(e.Y < (scPosX.Y + scWidth)))
			{
				scaleX = (e.X - scPosX.X) / scWidth;
			}
			if ((scaleLastX != scaleX))
			{
				RecalcVariables();			// пересчитываем смещение начала координат по X
				RedrawScreen(grafx.Graphics);
				this.Refresh();
			}
			scaleLastX = scaleX;

			// шелчек мышкой по контролу управления масштабом по Y
			if ((e.X > scPosY.X) &
				(e.X < (scPosY.X + scWidth)) &
				(e.Y > scPosY.Y) &
				(e.Y < (scPosY.Y + scaleCountY * scWidth)))
			{
				scaleY = scaleCountY - (e.Y - scPosY.Y) / scWidth-1;
			}
			if ((scaleLastY != scaleY))
			{
				RecalcVariables();			// пересчитываем смещение начала координат по Y
				RedrawScreen(grafx.Graphics);
				this.Refresh();
			}
			scaleLastY = scaleY;

			// проверяем, не щелкнули ли по курсору
			/*// Захватываем нижний Y 
			if (downMarkActive == true)
			{
				yMark[0] = e.Y - LeftUpCorner.Y;
				if (yMark[0] >= (grapHeight + 3))
					yMark[0] = grapHeight + 3;
				if ((yMark[0] - yMark[1]) < 4)
					yMark[0] = yMark[1] + 4;
				this.Refresh();
			}*/
			// Захватываем верхний Y 
			/*if (cursorUpActiv == true)
			{
				yMark[1] = e.Y - LeftUpCorner.Y;
				if (yMark[1] < -3)
					yMark[1] = -3;
				if ((yMark[0] - yMark[1]) < 4)
					yMark[1] = yMark[0] - 4;
				this.Refresh();
			}*/

		}
		// щелчек мышки на форме, проверяем, не щелкнули ли по органам управления
		private void MouseUpHandler(object sender, MouseEventArgs e)
		{
			isMouseDown = false;
		}
		// 2 кнопки прокрутки по оси Х
		// прокрутка вправо (двигаем сам график)
		// смещаем график на 1/4
		private void XScrollRightButton_Click(object sender, EventArgs e)
		{
			inputIndex = inputIndex + samplesOnScreen / 4;
			if (inputIndex >= inputLength)
				inputIndex = inputLength - 1;
			RedrawScreen(grafx.Graphics);
			this.Refresh();


		}
		// прокрутка влево
		private void XScrollLeftButton_Click(object sender, EventArgs e)
		{	
			inputIndex = inputIndex - samplesOnScreen/4;
			if (inputIndex < 0)
				inputIndex = 0;
			RedrawScreen(grafx.Graphics);
			this.Refresh();
		}
		// 2 кнопки сдвига по оси Y
		// сдвиг вверх
		private void YScrollUpButton_Click(object sender, EventArgs e)
		{
			shiftYnV += +nVOnScreen / 16;
			RedrawScreen(grafx.Graphics);
			this.Refresh();
		}
		// сдвиг вверх
		private void YScrollDownButton_Click(object sender, EventArgs e)
		{
			shiftYnV += -nVOnScreen/16;
			RedrawScreen(grafx.Graphics);
			this.Refresh();
		}
	}
}
