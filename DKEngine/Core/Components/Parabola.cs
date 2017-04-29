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
    public class Parabola : Behavior
    {
        public TimeSpan Time;
        public float Y;

        public float Accumulated { get; private set; }

        public bool Enabled = false;

        float Elapsed;

        float[] ValuesInTime;
        int NumberOfSamples;
        const float SamplesInSecodnd = 1000;

        public Parabola(GameObject Parent)
            : base(Parent)
        { }

        

        public override void Destroy()
        { }

        protected override void Initialize()
        {
            throw new NotImplementedException();
        }

        protected internal override void Start()
        {
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
                Accumulated = 0;
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

                for (int i = (int)Elapsed; i <= (int)MaxTime; i++)
                {
                    Accumulated += ValuesInTime[i];
                }

                Elapsed += Engine.DeltaTime;
            }
        }
    }
}
