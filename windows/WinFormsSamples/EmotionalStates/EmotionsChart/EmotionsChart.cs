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
            {RelaxationRate = 0.0, ConcentrationRate = 0.0};

        private BaseEmotionalValue _baseValue = new BaseEmotionalValue(){Alpha = 0.0, Beta = 0.0};

        private double _alphaLeftValue = 0.0;
        private double _betaLeftValue = 0.0;
        private double _alphaRightValue = 0.0;
        private double _betaRightValue = 0.0;
        private double _basePower = 0.0;

        private string _baseAlphaLabelText = "Base Alpha: 0.00";
        private string _baseBetaLabelText = "Base Beta: 0.00";
        private string _alphaPlusBetaLeftLabelText = "Alpha+Beta Left: 0.00";
        private string _alphaPlusBetaRightLabelText = "Alpha+Beta Right: 0.00";
        private string _baseLabelText = "Alpha+Beta Base: 0.00";
        private string _stateRelaxationLabelText = "Relaxation";
        private string _stateNormalActivationLabelText = "Normal activation";
        private string _concentrationLabelText = "Concentration";
        
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

            var stateRelaxationLabelSize = graphics.MeasureString(_stateRelaxationLabelText, _textBoldFont);
            var stateNormalActivationLabelSize = graphics.MeasureString(_stateNormalActivationLabelText, _textBoldFont);
            var stateConcentrationLabelSize = graphics.MeasureString(_concentrationLabelText, _textBoldFont);

            var baseAlphaLabelSize = graphics.MeasureString(_baseAlphaLabelText, _textFont);
            var baseBetaLabelSize = graphics.MeasureString(_baseBetaLabelText, _textFont);
            var alphaPlusBetaLeftLabelSize = graphics.MeasureString(_alphaPlusBetaLeftLabelText, _textFont);
            var alphaPlusBetaRightLabelSize = graphics.MeasureString(_alphaPlusBetaRightLabelText, _textFont);
            var baseLabelSize = graphics.MeasureString(_baseLabelText, _textFont);
            var barY = stateRelaxationLabelSize.Height;
            var fullBarWidth = _drawableSize.Width - Math.Max(baseAlphaLabelSize.Width, baseBetaLabelSize.Width) -
                               Math.Max(alphaPlusBetaLeftLabelSize.Width, alphaPlusBetaRightLabelSize.Width) -
                baseLabelSize.Width;


            var relaxationBlockWidth = fullBarWidth * 0.4f;
            var concentrationBlockWidth =relaxationBlockWidth;
            var normalActivationBlockWidth = fullBarWidth * 0.2f;
            var barX = 0.0f;
            var barWidth = 0.0f;
            if (Mode == EmotionBarMode.Data || Mode == EmotionBarMode.Artifact)
            {
                if (_emotionalState.RelaxationRate > 0.0)
                {
                    barWidth = (float) _emotionalState.RelaxationRate * (fullBarWidth / 2);
                    barX = fullBarWidth / 2 - barWidth;
                }
                else if (_emotionalState.ConcentrationRate > 0.0)
                {
                    barWidth = (float)_emotionalState.ConcentrationRate * (fullBarWidth / 2);
                    barX = fullBarWidth / 2;
                }
                else
                {
                    barX = 0;
                    barWidth = 0;
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
                
                graphics.DrawLine(framePen, relaxationBlockWidth, barY, relaxationBlockWidth, _drawableSize.Height-1);
                graphics.DrawLine(framePen, relaxationBlockWidth+normalActivationBlockWidth/2, barY, relaxationBlockWidth + normalActivationBlockWidth / 2, _drawableSize.Height-1);
                graphics.DrawLine(framePen, relaxationBlockWidth+normalActivationBlockWidth, barY, relaxationBlockWidth+normalActivationBlockWidth, _drawableSize.Height-1);
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
                var emotionalStateName = EmotionStateChannel.ValueToName(_emotionalState);
                var labelCenterX = relaxationBlockWidth / 2;

                graphics.DrawString(_stateRelaxationLabelText,
                    emotionalStateName == EmotionalStateName.Relaxation ? _textBoldFont : _textFont, textBrush,
                    labelCenterX - stateRelaxationLabelSize.Width / 2, 0);

                labelCenterX = relaxationBlockWidth + normalActivationBlockWidth / 2;
                graphics.DrawString(_stateNormalActivationLabelText,
                    emotionalStateName == EmotionalStateName.NormalActivation ? _textBoldFont : _textFont, textBrush,
                    labelCenterX - stateNormalActivationLabelSize.Width / 2, 0);

                labelCenterX = relaxationBlockWidth + normalActivationBlockWidth + concentrationBlockWidth / 2;
                graphics.DrawString(_concentrationLabelText,
                    emotionalStateName == EmotionalStateName.Concentration ? _textBoldFont : _textFont, textBrush,
                    labelCenterX - stateConcentrationLabelSize.Width / 2, 0);

                var drawableCenterY = _drawableSize.Height / 2;
                var labelX = fullBarWidth + 2;
                graphics.DrawString(_baseAlphaLabelText, _textFont, textBrush, labelX, drawableCenterY - baseAlphaLabelSize.Height);
                graphics.DrawString(_baseBetaLabelText, _textFont, textBrush, labelX, drawableCenterY);
                labelX += Math.Max(baseAlphaLabelSize.Width, baseBetaLabelSize.Width);

                graphics.DrawString(_alphaPlusBetaLeftLabelText, _textFont, textBrush, labelX, drawableCenterY - alphaPlusBetaLeftLabelSize.Height);
                graphics.DrawString(_alphaPlusBetaRightLabelText, _textFont, textBrush, labelX, drawableCenterY);
                labelX += Math.Max(alphaPlusBetaLeftLabelSize.Width, alphaPlusBetaRightLabelSize.Width);

                graphics.DrawString(_baseLabelText, _textFont, textBrush, labelX, drawableCenterY - baseLabelSize.Height);
            }

            if (Mode == EmotionBarMode.Artifact)
            {
                using (var artifactBrush = new SolidBrush(Color.FromArgb(80, Color.Red)))
                {
                    graphics.FillRectangle(artifactBrush, 1, barY, fullBarWidth, _drawableSize.Height - barY - 1);
                    var artifactString = "Artifact zone";
                    var artifactStringFont = new Font("Arial", 14);
                    var stringSize = graphics.MeasureString(artifactString, artifactStringFont);
                    var barCenterX = fullBarWidth - fullBarWidth / 2;
                    var barCenterY = barY + (_drawableSize.Height - barY) / 2;
                    graphics.DrawString(artifactString, artifactStringFont, Brushes.Black,
                        barCenterX - stringSize.Width / 2, barCenterY - stringSize.Height / 2);
                }
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

        public BaseEmotionalValue BaseValue
        {
            set
            {
                _baseValue = value;
                UpdateLabels();
            }
        }

        public double AlphaLeft
        {
            set
            {
                _alphaLeftValue = value;
                UpdateLabels();
            }
            get => _alphaLeftValue;
        }

        public double BetaLeft
        {
            set
            {
                _betaLeftValue = value;
                UpdateLabels();
            }
            get => _betaLeftValue;
        }

        public double AlphaRight
        {
            set
            {
                _alphaRightValue = value;
                UpdateLabels();
            }
            get => _alphaRightValue;
        }

        public double BetaRight
        {
            set
            {
                _betaRightValue = value;
                UpdateLabels();
            }
            get => _betaRightValue;
        }

        public double BasePower
        {
            set
            {
                _basePower = value;
                UpdateLabels();
            }
            get => _basePower;
        }

        private void UpdateLabels()
        {
            _baseAlphaLabelText = $"Base Alpha: {Math.Round(_baseValue.Alpha, 2)}";
            _baseBetaLabelText = $"Base Beta: {Math.Round(_baseValue.Beta, 2)}";
            _alphaPlusBetaLeftLabelText = $"Alpha+Beta Left: {Math.Round((_alphaLeftValue + _betaLeftValue)*1e6, 2)} uW";
            _alphaPlusBetaRightLabelText = $"Alpha+Beta Right: {Math.Round((_alphaRightValue + _betaRightValue)*1e6, 2)} uW";
            _baseLabelText = $"Alpha+Beta Base: {Math.Round((_basePower)*1e6, 2)} uW";
        }
    }
}