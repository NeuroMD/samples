using System;
using System.Drawing;
using System.Windows.Forms;

namespace EmotionalStates.Drawable
{
    public partial class DrawableControl : Control
    {
        private readonly Timer _redrawTimer; 
        private readonly BufferedGraphicsContext _graphicsContext = BufferedGraphicsManager.Current;
        private BufferedGraphics _graphicsBuffer;
        private IDrawable _drawable = new EmptyDrawable();

        public IDrawable Drawable
        {
            get => _drawable;
            set
            {
                _drawable = value ?? new EmptyDrawable();
                _drawable.DrawableSize = this.Size;
            }
        }

        public DrawableControl()
        {
            InitializeComponent();
            SetStyle(ControlStyles.DoubleBuffer, true);
            SizeChanged += EegIndexChart_SizeChanged; 
            UpdateBufferedGraphics();
            _redrawTimer = new Timer(components) { Interval = 50 };
            _redrawTimer.Tick += RedrawTimerTick;
            _redrawTimer.Start();
            _drawable.DrawableSize = this.Size;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            _graphicsBuffer.Render(e.Graphics);
        }

        private void RedrawTimerTick(object sender, System.EventArgs e)
        {
            try
            {
                _drawable.Draw(_graphicsBuffer.Graphics);
            }
            catch(Exception exc)
            {
                Console.WriteLine($"Unhandled exception in user provided draw method: {exc.Message}");
            }
            Refresh();
        }

        private void EegIndexChart_SizeChanged(object sender, System.EventArgs e)
        {
            UpdateBufferedGraphics();
            _drawable.DrawableSize = this.Size;
        }

        private void UpdateBufferedGraphics()
        {
            _graphicsContext.MaximumBuffer = new Size(this.Width + 1, this.Height + 1);
            _graphicsBuffer?.Dispose();
            _graphicsBuffer = _graphicsContext.Allocate(this.CreateGraphics(), new Rectangle(0, 0, this.Width, this.Height));
        }

        private void DrawableControl_MouseClick(object sender, MouseEventArgs e)
        {
            (Drawable as IMouseEventsHandler)?.OnMouseClick(e);
        }

        private void DrawableControl_MouseDown(object sender, MouseEventArgs e)
        {
            (Drawable as IMouseEventsHandler)?.OnMouseDown(e);
        }

        private void DrawableControl_MouseUp(object sender, MouseEventArgs e)
        {
            (Drawable as IMouseEventsHandler)?.OnMouseUp(e);
        }

        private void DrawableControl_MouseMove(object sender, MouseEventArgs e)
        {
            (Drawable as IMouseEventsHandler)?.OnMouseMove(e);
        }
    }

    internal sealed class EmptyDrawable : IDrawable
    {
        public Size DrawableSize
        {
            set
            { 
                //do nothing
            }
        }

        public void Draw(Graphics graphics)
        {
            graphics.Clear(Color.Black);
        }
    }
}
