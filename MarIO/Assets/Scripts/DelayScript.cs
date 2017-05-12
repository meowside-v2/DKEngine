using DKEngine;
using DKEngine.Core;
using DKEngine.Core.Components;
using MarIO.Assets.Models;
using System;

namespace MarIO.Assets.Scripts
{
    public class DelayScript : Script
    {
        private TimeSpan Checker;
        private Delayer Source;

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

            if (Checker > Source?.TimeToWait)
            {
                Source?.CalledAction?.Invoke();
            }
        }
    }
}