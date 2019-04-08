using System;
using System.Drawing;
using EmotionalStates.Drawable;
using Neuro;

namespace EmotionalStates.EmotionsChart
{
    public class EmotionsChart : IDrawable, IEmotionsChart
    {
        private readonly Color _backgroundColor = Color.LightYellow;
        private readonly Color _barColor = Color.ForestGreen;
        private readonly Color _frameColor = Color.DarkSlateGray;
        private readonly Color _textColor = Color.Black;
        private readonly Font _textFont = new Font("Arial", 10);
        private readonly Font _textBoldFont = new Font("Arial", 10, FontStyle.Bold);

        private Size _drawableSize;

        private EmotionalState _emotionalState = new EmotionalState()
            {Attention = 0, Meditation = 0, Relax = 0, Stress = 0, State = 0};

        private string _productiveRelaxLabelText = "Productive relax: 0 %";
        private string _meditationLabelText = "Meditation: 0 %";
        private string _attentionLabelText = "Attention: 0 %";
        private string _stressLabelText = "Stress: 0 %";
        private string _stateRelaxLabelText = "Relax";
        private string _stateDeepRelaxLabelText = "Deep relax";
        private string _stateSleepLabelText = "Sleep";
        private string _stateNormalActivationLabelText = "Normal activation";
        private string _stateExcitementLabelText = "Excitement";
        private string _stateDeepExcitementLabelText = "Deep excitement";
        
        public EmotionBarMode Mode { private get; set; }

        public Size DrawableSize
        {
            set
            {
                _drawableSize = value;
                UpdateLabels();
            }
        }

        public void Draw(Graphics graphics)
        {
            using (var backgroundBrush = new SolidBrush(_backgroundColor))
            {
                graphics.FillRectangle(backgroundBrush, 0, 0, _drawableSize.Width, _drawableSize.Height);
            }

            var stateRelaxLabelSize = graphics.MeasureString(_stateRelaxLabelText, _textBoldFont);
            var stateDeepRelaxLabelSize = graphics.MeasureString(_stateDeepRelaxLabelText, _textBoldFont);
            var stateSleepLabelSize = graphics.MeasureString(_stateSleepLabelText, _textBoldFont);
            var stateNormalActivationLabelSize = graphics.MeasureString(_stateNormalActivationLabelText, _textBoldFont);
            var stateExcitementLabelSize = graphics.MeasureString(_stateExcitementLabelText, _textBoldFont);
            var stateDeepExcitementLabelSize = graphics.MeasureString(_stateDeepExcitementLabelText, _textBoldFont);

            var productiveRelaxLabelSize = graphics.MeasureString(_productiveRelaxLabelText, _textFont);
            var meditationLabelSize = graphics.MeasureString(_meditationLabelText, _textFont);
            var attentionLabelSize = graphics.MeasureString(_attentionLabelText, _textFont);
            var stressLabelSize = graphics.MeasureString(_stressLabelText, _textFont);
            var barY = stateRelaxLabelSize.Height;
            var fullBarWidth = _drawableSize.Width - Math.Max(productiveRelaxLabelSize.Width, meditationLabelSize.Width) -
                           Math.Max(attentionLabelSize.Width, stressLabelSize.Width);


            var blockWidth = fullBarWidth / 6;
            var barX = 0.0f;
            var barWidth = 0.0f;
            if (Mode == EmotionBarMode.Data)
            {
                switch (_emotionalState.State)
                {
                    case 10:
                        barX = 1;
                        barWidth = fullBarWidth / 2;
                        break;
                    case 9:
                        barX = blockWidth / 2;
                        barWidth = fullBarWidth / 2 - barX;
                        break;
                    case 8:
                        barX = blockWidth;
                        barWidth = fullBarWidth / 2 - barX;
                        break;
                    case 7:
                        barX = blockWidth + blockWidth / 4;
                        barWidth = fullBarWidth / 2 - barX;
                        break;
                    case 6:
                        barX = blockWidth + blockWidth / 2;
                        barWidth = fullBarWidth / 2 - barX;
                        break;
                    case 5:
                        barX = 2 * blockWidth - blockWidth / 4;
                        barWidth = fullBarWidth / 2 - barX;
                        break;
                    case 4:
                        barX = 2 * blockWidth;
                        barWidth = fullBarWidth / 2 - barX;
                        break;
                    case 3:
                        barX = 2 * blockWidth + blockWidth / 4;
                        barWidth = fullBarWidth / 2 - barX;
                        break;
                    case 2:
                        barX = 2 * blockWidth + blockWidth / 2;
                        barWidth = fullBarWidth / 2 - barX;
                        break;
                    case 1:
                        barX = 3 * blockWidth - blockWidth / 4;
                        barWidth = fullBarWidth / 2 - barX;
                        break;

                    case -10:
                        barX = fullBarWidth / 2;
                        barWidth = fullBarWidth / 2;
                        break;
                    case -9:
                        barX = fullBarWidth / 2;
                        barWidth = fullBarWidth / 2 - blockWidth / 2;
                        break;
                    case -8:
                        barX = fullBarWidth / 2;
                        barWidth = fullBarWidth / 2 - blockWidth;
                        break;
                    case -7:
                        barX = fullBarWidth / 2;
                        barWidth = fullBarWidth / 2 - blockWidth - blockWidth / 4;
                        break;
                    case -6:
                        barX = fullBarWidth / 2;
                        barWidth = fullBarWidth / 2 - blockWidth - blockWidth / 2;
                        break;
                    case -5:
                        barX = fullBarWidth / 2;
                        barWidth = fullBarWidth / 2 - 2 * blockWidth + blockWidth / 4;
                        break;
                    case -4:
                        barX = fullBarWidth / 2;
                        barWidth = fullBarWidth / 2 - 2 * blockWidth;
                        break;
                    case -3:
                        barX = fullBarWidth / 2;
                        barWidth = fullBarWidth / 2 - 2 * blockWidth - blockWidth / 4;
                        ;
                        break;
                    case -2:
                        barX = fullBarWidth / 2;
                        barWidth = fullBarWidth / 2 - 2 * blockWidth - blockWidth / 2;
                        ;
                        break;
                    case -1:
                        barX = fullBarWidth / 2;
                        barWidth = fullBarWidth / 2 - 3 * blockWidth + blockWidth / 4;
                        ;
                        break;
                }
            }
            else 
            {
                barX = 0;
                barWidth = fullBarWidth;
            }

            using (var barBrush = new SolidBrush(_barColor))
            {
                graphics.FillRectangle(barBrush, barX, barY, barWidth, _drawableSize.Height - barY);
            }

            using (var framePen = new Pen(_frameColor, 2))
            {
                graphics.DrawRectangle(framePen, 1, barY, fullBarWidth, _drawableSize.Height - barY-1);
                for (var lineX = blockWidth; lineX < fullBarWidth; lineX += blockWidth)
                {
                    graphics.DrawLine(framePen, lineX, barY, lineX, _drawableSize.Height-1);
                }
            }

            if (Mode == EmotionBarMode.Wait)
            {
                using (var waitBrush = new SolidBrush(Color.FromArgb(160, Color.White)))
                {
                    graphics.FillRectangle(waitBrush, barX, barY-1, barWidth+2, _drawableSize.Height - barY+1);
                    var waitString = "Waiting for data...";
                    var waitStringFont = new Font("Arial", 14);
                    var stringSize = graphics.MeasureString(waitString, waitStringFont);
                    var barCenterX = barX + barWidth / 2;
                    var barCenterY = barY + (_drawableSize.Height - barY) / 2;
                    graphics.DrawString(waitString, waitStringFont, Brushes.Black,
                        barCenterX - stringSize.Width / 2, barCenterY - stringSize.Height / 2);
                }
            }

            using (var textBrush = new SolidBrush(_textColor))
            {
                var emotionalStateName = EmotionStateChannel.ValueToName(_emotionalState.State);
                var labelCenterX = blockWidth / 2;
                graphics.DrawString(_stateSleepLabelText,
                    emotionalStateName == EmotionalStateName.Sleep ? _textBoldFont : _textFont, textBrush,
                    labelCenterX - stateSleepLabelSize.Width / 2, 0);
                labelCenterX += blockWidth;

                graphics.DrawString(_stateDeepRelaxLabelText,
                    emotionalStateName == EmotionalStateName.DeepRelax ? _textBoldFont : _textFont, textBrush,
                    labelCenterX - stateDeepRelaxLabelSize.Width / 2, 0);
                labelCenterX += blockWidth;

                graphics.DrawString(_stateRelaxLabelText,
                    emotionalStateName == EmotionalStateName.Relax ? _textBoldFont : _textFont, textBrush,
                    labelCenterX - stateRelaxLabelSize.Width / 2, 0);
                labelCenterX += blockWidth;

                graphics.DrawString(_stateNormalActivationLabelText,
                    emotionalStateName == EmotionalStateName.NormalActivation ? _textBoldFont : _textFont, textBrush,
                    labelCenterX - stateNormalActivationLabelSize.Width / 2, 0);
                labelCenterX += blockWidth;

                graphics.DrawString(_stateExcitementLabelText,
                    emotionalStateName == EmotionalStateName.Excitement ? _textBoldFont : _textFont, textBrush,
                    labelCenterX - stateExcitementLabelSize.Width / 2, 0);
                labelCenterX += blockWidth;

                graphics.DrawString(_stateDeepExcitementLabelText,
                    emotionalStateName == EmotionalStateName.DeepExcitement ? _textBoldFont : _textFont, textBrush,
                    labelCenterX - stateDeepExcitementLabelSize.Width / 2, 0);

                var drawableCenterY = _drawableSize.Height / 2;
                var labelX = fullBarWidth + 2;
                graphics.DrawString(_productiveRelaxLabelText, _textFont, textBrush, labelX, drawableCenterY - productiveRelaxLabelSize.Height);
                graphics.DrawString(_meditationLabelText, _textFont, textBrush, labelX, drawableCenterY);
                labelX += Math.Max(meditationLabelSize.Width, productiveRelaxLabelSize.Width);

                graphics.DrawString(_attentionLabelText, _textFont, textBrush, labelX, drawableCenterY - attentionLabelSize.Height);
                graphics.DrawString(_stressLabelText, _textFont, textBrush, labelX, drawableCenterY);
            }
        }

        public EmotionalState State
        {
            set
            {
                _emotionalState = value;
                UpdateLabels();
            }
        }

        private void UpdateLabels()
        {
            _productiveRelaxLabelText = $"Productive relax: {_emotionalState.Relax} %";
            _meditationLabelText = $"Meditation: {_emotionalState.Meditation} %";
            _attentionLabelText = $"Attention: {_emotionalState.Attention} %";
            _stressLabelText = $"Stress: {_emotionalState.Stress} %";
        }
    }
}