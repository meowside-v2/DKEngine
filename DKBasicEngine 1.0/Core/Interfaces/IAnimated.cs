using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DKBasicEngine_1_0
{
    public interface IAnimated
    {
        AnimationLoop Settings { get; set; }
        int NumberOfPlays { get; set; }
        int AnimationState { get; set; }
        TimeSpan CurrentAnimationTime { get; }
    }
    public enum AnimationLoop
    {
        Once,
        Endless
    }
}
