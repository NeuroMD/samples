using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Neuro;

namespace Biofeedback.Spectrum
{
    class SpectrumModel
    {
        private readonly IList<SpectrumChannel> _spectrumChannels;

        public double FrequencyStep => _spectrumChannels.Select(x => x.HzPerSpectrumSample).First();
        public float SamplingFrequency => _spectrumChannels.Select(x => x.SamplingFrequency).First();
        public double WindowDuration { get; set; }
        public IList<Spectrum> Spectrums { get; private set; }
        private readonly Action _calculationTask;
        private bool _stopTask = false;

        public SpectrumModel(IList<SpectrumChannel> channels)
        {
            Spectrums = new List<Spectrum>();
            _spectrumChannels = channels;
            _calculationTask = ()=>
            {
                try
                {
                    CalculateSpectrums();
                }
                catch (Exception e) { Console.WriteLine(e.Message);}

                if (!_stopTask) Task.Run(_calculationTask);
            };
            Task.Run(_calculationTask);
        }

        ~SpectrumModel()
        {
            _stopTask = true;
        }

        private void CalculateSpectrums()
        {
            var spectrums = new List<Spectrum>();
            var readLength = (int) (SamplingFrequency * WindowDuration);
            if (readLength <= 0)
                return;
            foreach (var spectrumChannel in _spectrumChannels)
            {
                var offset = spectrumChannel.TotalLength - readLength;
                if (offset < 0)
                {
                    offset = 0;
                    readLength = spectrumChannel.TotalLength;
                }
                spectrums.Add(new Spectrum(spectrumChannel.Info.Name,
                    spectrumChannel.ReadData(offset, readLength)));
            }

            Spectrums = spectrums;
        }
    }
}
