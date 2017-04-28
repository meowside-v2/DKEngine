/*
* (C) 2017 David Knieradl 
*/

using DKEngine.Core.Ext;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Diagnostics;

namespace DKEngine.Core.Components
{
    public class Animator : Behavior, IAnimated
    {
        public TimeSpan CurrentAnimationTime;
        internal Dictionary<string, AnimationNode> Animations;
        private AnimationNode _current;
        //private GameObject _p;

        public int NumberOfPlays { get; private set; } = 0;
        public AnimationNode Current
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
            this.Parent = Parent;

            this.Name = string.Format("{0}_Animator", Parent.Name);

            try
            {
                Engine.LoadingScene.AllComponents.AddSafe(this);
            }
            catch (Exception e)
            {
                Debug.WriteLine("Loading scene is NULL\n\n{0}", e);
            }

            /*if(Parent.Model != null)
            {
                Animations.Add("default", new AnimationNode("default", Parent.Model));
                this.Play("default");
            }*/
        }

        public void AddAnimation(string Name, Material Source)
        {
            Animations.Add(Name, new AnimationNode(Name, Source));
            if(Animations.Count == 1)
            {
                Play(Animations.ElementAt(0).Key);
            }
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
                CurrentAnimationTime = CurrentAnimationTime.Add(new TimeSpan(0, 0, 0, 0, (int)(Engine.DeltaTime * 1000)));

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
