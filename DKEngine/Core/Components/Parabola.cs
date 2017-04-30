using DKEngine.Core.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DKEngine.Core;
using DKEngine;

namespace DKBasicEngine_1_0.Core.Components
{
    /*
     * ------------------
     * DOES NOT WORK YET
     * ------------------
    */

    [Obsolete]
    public class Parabola : Component
    {
        public TimeSpan Time;
        public float Y;

        private float _accumulated = 0f;
        public float Accumulated
        {
            get
            {
                if (Enabled)
                {
                    if (this.LastUpdated != Engine.LastUpdated)
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

                return _accumulated;
            }
        }

        public bool Enabled = false;

        float Elapsed = 0f;

        float[] ValuesInTime;
        int NumberOfSamples;
        const float SamplesInSecodnd = 1000;

        public Parabola(GameObject Parent)
            : base(Parent)
        { }

        internal override void Init()
        {
            Name = string.Format("{0}_{1}", Parent.Name, nameof(Parabola));

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

            /*NumberOfSamples = (int)Time.TotalMilliseconds;
            ValuesInTime = new float[NumberOfSamples];

            //double midpoint = NumberOfSamples / 2;

            for (int i = 0; i < NumberOfSamples; i++)
            {
                float result = (float)(Math.Sin(i * (Math.PI / SamplesInSecodnd)) * Y);
                ValuesInTime[i] = i > 0 ? (float)(Math.Sin(i * (Math.PI / SamplesInSecodnd)) * Y) - ValuesInTime[i - 1] : 0;
            }

            //Elapsed += Engine.DeltaTime;*/
        }

        public override void Destroy()
        { }
    }
}
