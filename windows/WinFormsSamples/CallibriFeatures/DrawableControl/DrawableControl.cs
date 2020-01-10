using System;
using System.Drawing;
using System.Windows.Forms;
using NLog;

namespace CallibriFeatures.DrawableControl
{
    public partial class DrawableControl : Control
    {
        // ReSharper disable once PrivateFieldCanBeConvertedToLocalVariable
        private readonly Timer _redrawTimer; 
        private readonly BufferedGraphicsContext _graphicsContext = BufferedGraphicsManager.Current;
        private BufferedGraphics _graphicsBuffer;
        private IDrawable _drawable = new EmptyDrawable();

        public IDrawable Drawable
        {
            get => _drawable;
            set
            {
                _drawable = value ?? throw new ArgumentNullException(nameof(Drawable), "Cannot set null reference as Drawable value");
                _drawable.DrawableSize = this.Size;
            }
        }

        public DrawableControl()
        {
            InitializeComponent();
            SetStyle(ControlStyles.DoubleBuffer, true);
            SizeChanged += DrawableChart_SizeChanged;
            UpdateBufferedGraphics();
            _redrawTimer = new Timer(components ?? throw new InvalidOperationException("Components of Drawable control are not initialized")) { Interval = 50 };
            _redrawTimer.Tick += RedrawTimerTick;
            EnabledChanged += DrawableControl_EnabledChanged;
            _redrawTimer.Enabled = this.Enabled;

            if (_drawable == null)
                throw new InvalidOperationException("Drawable control default state is corrupted");

            _drawable.DrawableSize = this.Size;
        }

        private void DrawableControl_EnabledChanged(object sender, EventArgs e)
        {
            _redrawTimer.Enabled = this.Enabled;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            _graphicsBuffer?.Render(e.Graphics);
        }

        private void RedrawTimerTick(object sender, System.EventArgs e)
        {
            try
            {
                var graphics = _graphicsBuffer?.Graphics;
                if (graphics == null)
                {
                    LogManager.GetCurrentClassLogger()?.Warn("Graphics buffer is null");
                    return;
                }

                _drawable?.Draw(graphics);
            }
            catch(Exception exc)
            {
                LogManager.GetCurrentClassLogger()?.Error($"Unhandled exception in user provided draw method: {exc.Message}");
            }
            Refresh();
        }

        private void DrawableChart_SizeChanged(object sender, System.EventArgs e)
        {
            UpdateBufferedGraphics();
            if (_drawable == null)
            {
                LogManager.GetCurrentClassLogger()?.Warn("Drawable is null");
                return;
            }
            _drawable.DrawableSize = this.Size;
        }

        private void UpdateBufferedGraphics()
        {
            if (_graphicsContext == null)
            {
                LogManager.GetCurrentClassLogger()?.Error("Graphics context is null");
                return;
            }

            try
            {
                _graphicsContext.MaximumBuffer = new Size(this.Width + 1, this.Height + 1);
                var oldBuffer = _graphicsBuffer;
                _graphicsBuffer = _graphicsContext.Allocate(this.CreateGraphics(), new Rectangle(0, 0, this.Width, this.Height));
                oldBuffer?.Dispose();
            }
            catch (Exception e)
            {
                LogManager.GetCurrentClassLogger()?.Warn($"Graphics buffer allocation error: {e.Message}");
            }
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

        private void DrawableControl_MouseLeave(object sender, EventArgs e)
        {
            (Drawable as IMouseEventsHandler)?.OnMouseLeave();
        }
    }
}
