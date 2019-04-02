using System;
using System.Drawing;
using Neuro;

namespace EmotionalStates.IndexChart
{
    internal enum EegIndexChartMode
    {
        Signal,
        Waiting,
        Empty
    }

    internal interface IEegIndexChart
    {
        Size DrawableSize { get; }
        EegIndexValues[] IndicesData { set; }
        EegIndexChartMode Mode { get; set; }
        event EventHandler<Size> SizeChanged;
    }
}
