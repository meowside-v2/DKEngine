using System;

namespace DKBasicEngine_1_0
{
    public class Animator : IAnimated
    {
        IGraphics Parent;

        public int MaxBufferImages = 0;
        public int MinBufferImages = 0;

        private bool _wasPlayed = false;
        private AnimationLoop _settings = AnimationLoop.Once;
        public AnimationLoop Settings
        {
            get
            {
                return _settings;
            }
            set
            {
                _settings = value;
                _wasPlayed = false;
            }
        }
        public int AnimationState
        {
            get
            {
                return (int)(CurrentAnimationTime.TotalMilliseconds / Parent.Model.DurationPerFrame % Parent.Model.Frames);
            }
            set { }
        }
        public TimeSpan CurrentAnimationTime { get; private set; } = new TimeSpan(0);

        private int _numberOfPlays = 0;
        public int NumberOfPlays
        {
            get { return _numberOfPlays; }
            set
            {
                if(value >= 0 && value != _numberOfPlays)
                {
                    _numberOfPlays = value;

                    if (Settings == AnimationLoop.Once && value > 0)
                        _wasPlayed = true;
                }
            }
        }

        public Animator(IGraphics Parent)
        {
            this.Parent = Parent;
        }

        internal void Update()
        {
            if (Parent.Model?.Frames > 1 && !_wasPlayed)
            {
                CurrentAnimationTime = CurrentAnimationTime.Add(new TimeSpan(0, 0, 0, 0, (int)(Engine.deltaTime * 1000)));

                if (CurrentAnimationTime.TotalMilliseconds > Parent.Model.Duration)
                {
                    CurrentAnimationTime = CurrentAnimationTime.Subtract(new TimeSpan(0, 0, 0, 0, Parent.Model.Duration));
                    NumberOfPlays++;
                }
            }
        }

        public void Restart()
        {
            _wasPlayed = false;
            CurrentAnimationTime = new TimeSpan(0);
            NumberOfPlays = 0;
        }
    }
}
