using DKEngine.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DKEngine.Core.Components;
using System.Diagnostics;
using DKEngine;
using MarIO.Assets.Models;

namespace MarIO.Assets.Scripts
{
    class DelayScript : Script
    {
        TimeSpan Checker;
        Delayer Source;

        public DelayScript(GameObject Parent) : base(Parent)
        {
            Source = (Delayer)Parent;
        }

        protected override void OnColliderEnter(Collider e)
        { }

        protected override void Start()
        {
            Checker = new TimeSpan();
        }

        protected override void Update()
        {
            Checker += new TimeSpan(0, 0, 0, 0, (int)(Engine.DeltaTime * 1000));

            if(Checker > Source?.TimeToWait)
            {
                Source?.CalledAction?.Invoke();
            }
        }
    }
}
