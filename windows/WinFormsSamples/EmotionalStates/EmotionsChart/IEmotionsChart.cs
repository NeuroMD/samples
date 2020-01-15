using Neuro;

namespace EmotionalStates.EmotionsChart
{
    public enum EmotionBarMode
    {
        Empty,
        Wait,
        Data,
        Artifact
    }

    public interface IEmotionsChart
    {
        EmotionalState State { set; }
        BaseEmotionalValue BaseValue { set; }
        double AlphaLeft { set; get; }
        double BetaLeft { set; get; }
        double AlphaRight { set; }
        double BetaRight { set; }
        double BasePower { set; }
        EmotionBarMode Mode { set; }
    }
}