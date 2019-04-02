using Neuro;

namespace EmotionalStates.EmotionsChart
{
    public enum EmotionBarMode
    {
        Empty,
        Wait,
        Data
    }
    public interface IEmotionsChart
    {
        EmotionalState State { set; }
        EmotionBarMode Mode { set; }
    }
}