using DKEngine;
using DKEngine.Core;
using DKEngine.Core.Components;
using System;

namespace DKBasicEngine_1_0.Core.Components
{
    /*
     * ------------------
     * DOES NOT WORK YET
     * ------------------
    */

    /*[Obsolete]
    public class Parabola : Behavior
    {
        public TimeSpan Time;
        public float Y;

        private float _accumulated = 0f;
        public float Accumulated { get { return _accumulated; } }

        public bool Enabled = false;

        float Elapsed = 0f;

        float[] ValuesInTime;
        int NumberOfSamples;
        const float SamplesInSecodnd = 1000;

        public Parabola(GameObject Parent)
            : base(Parent)
        {
            Name = string.Format("{0}_{1}", Parent.Name, nameof(Parabola));
        }

        public override void Destroy()
        { }

        protected internal override void Start()
        {
            NumberOfSamples = (int)Time.TotalMilliseconds;
            ValuesInTime = new float[NumberOfSamples];

            float midpoint = NumberOfSamples / 2f;
            float start = -midpoint;
            float lastResult = 0f;

            for (float i = start; i < midpoint; i += 0.1f)
            {
                float index = i + midpoint;
                float result = -(float)(Math.Cos(Math.PI / NumberOfSamples * i) * Y);
                ValuesInTime[(int)index] = index > 0 ? result - lastResult : result;
                lastResult = result;
            }

            NumberOfSamples = (int)Time.TotalMilliseconds;
            ValuesInTime = new float[NumberOfSamples];

            //double midpoint = NumberOfSamples / 2;

            for (int i = 0; i < NumberOfSamples; i++)
            {
                float result = (float)(Math.Sin(i * (Math.PI / SamplesInSecodnd)) * Y);
                ValuesInTime[i] = i > 0 ? (float)(Math.Sin(i * (Math.PI / SamplesInSecodnd)) * Y) - ValuesInTime[i - 1] : 0;
            }

            //Elapsed += Engine.DeltaTime;
        }

        protected internal override void Update()
        {
            if (Enabled)
            {
                float MaxTime = 0;
                float LeftoverTime = (float)((Elapsed + Engine.DeltaTime) - Time.TotalMilliseconds);

                if (LeftoverTime > 0)
                {
                    MaxTime = (float)(Time.TotalMilliseconds - LeftoverTime);
                    Enabled = false;
                }
                else
                {
                    MaxTime = Elapsed + Engine.DeltaTime;
                }

                for (int i = (int)Elapsed; i < (int)MaxTime; i++)
                {
                    _accumulated += ValuesInTime[i];
                }

                Elapsed += Engine.DeltaTime;
            }
        }
    }*/

    [Obsolete]
    public class Parabola : Behavior
    {
        public TimeSpan Time;
        public float Y;

        private float _accumulated = 0f;
        public float Accumulated { get { return _accumulated; } }

        public bool Enabled = false;

        private float Elapsed = 0f;

        private float[] ValuesInTime;
        private int NumberOfSamples;
        private const float SamplesInSecodnd = 1000;
        private const float X1 = 0f;
        public float X2 { get; private set; }

        public Parabola(GameObject Parent)
            : base(Parent)
        {
            Name = string.Format("{0}_{1}", Parent.Name, nameof(Parabola));
        }

        public override void Destroy()
        { }

        protected internal override void Start()
        {
            NumberOfSamples = (int)Time.TotalMilliseconds;
            ValuesInTime = new float[NumberOfSamples];
            float Duration = (float)Time.TotalSeconds;

            float lastResult = 0f;

            for (float i = 0; i < NumberOfSamples; i += 0.1f)
            {
                float constant = i / 1000f;
                float result = ((float)Math.Pow(-constant, 2) - (Duration * constant)) * Y;
                ValuesInTime[(int)i] = result - lastResult;
                lastResult = result;
            }
        }

        protected internal override void Update()
        {
            if (Enabled)
            {
                _accumulated = 0;
                float MaxTime = 0;
                float LeftoverTime = (float)((Elapsed + Engine.DeltaTime) * 1000 - Time.TotalMilliseconds);

                if (LeftoverTime > 0)
                {
                    MaxTime = (float)Time.TotalSeconds;
                    Enabled = false;
                }
                else
                {
                    MaxTime = Elapsed + Engine.DeltaTime;
                }

                int start = (int)(Elapsed * 1000);
                int end = (int)(MaxTime * 1000);
                for (int i = start; i < end; i++)
                {
                    _accumulated += ValuesInTime[i];
                }

                Elapsed += Engine.DeltaTime;
            }
        }
    }
}