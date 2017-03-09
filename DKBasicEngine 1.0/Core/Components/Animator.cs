/*
* (C) 2017 David Knieradl 
*/

using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace DKEngine.Core.Components
{
    public class Animator : Behavior, IAnimated
    {
        public TimeSpan CurrentAnimationTime;
        public Dictionary<string, AnimationNode> Animations;
        private AnimationNode _current;
        private GameObject _p;

        public int NumberOfPlays { get; private set; } = 0;
        internal AnimationNode Current
        {
            get { return _current; }
            set
            {
                if(value != _current)
                {
                    _current = value;
                    Parent.Model = _current.Animation;
                    NumberOfPlays = 0;
                    CurrentAnimationTime = new TimeSpan(0);
                }
            }
        }
        public int AnimationState
        {
            get
            {
                return (int)(CurrentAnimationTime.TotalMilliseconds / Parent.Model.DurationPerFrame % Parent.Model.Frames);
            }
        }
       
        public Animator(GameObject Parent)
            :base(Parent)
        {
            this.CurrentAnimationTime = new TimeSpan(0);
            this.Animations           = new Dictionary<string, AnimationNode>();
            _p = Parent;
            /*if(Parent.Model != null)
            {
                Animations.Add("default", new AnimationNode("default", Parent.Model));
                this.Play("default");
            }*/
        }

        public void Play(string AnimationName)
        {
            if (AnimationName != Current?.Name)
            {
                AnimationNode Result;

                try
                {
                    Result = Animations[AnimationName];
                }
                catch (Exception e)
                {
                    Debug.WriteLine("Animation \"{0}\"not found\n{1}", AnimationName, e);
                    return;
                }

                Current = Result;
            }
        }

        protected internal override void Update()
        {
            if (Parent.Model?.Frames > 1)
            {
                CurrentAnimationTime = CurrentAnimationTime.Add(new TimeSpan(0, 0, 0, 0, (int)(Engine.deltaTime * 1000)));

                if (CurrentAnimationTime.TotalMilliseconds > Parent.Model.Duration)
                {
                    CurrentAnimationTime = CurrentAnimationTime.Subtract(new TimeSpan(0, 0, 0, 0, Parent.Model.Duration));
                    NumberOfPlays++;
                }
            }
        }

        protected internal override void Start()
        { }

        public override void Destroy()
        {
            Engine.UpdateEvent -= UpdateHandle;
            
            Parent = null;
            UpdateHandle = null;
        }
    }
}
