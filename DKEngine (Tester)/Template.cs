using DKEngine.Core;
using DKEngine.Core.Components;
using DKEngine.Core.UI;
using System.Diagnostics;
using System.Drawing;

namespace DKEngine_Tester
{
    internal sealed class TemplateScript : Script
    {
        private TextBlock DepthMeter;

        public TemplateScript(GameObject Parent)
            : base(Parent)
        { }

        protected override void Start()
        {
            DepthMeter = new TextBlock(this.Parent);
            DepthMeter.Name = "Depth meter";
            DepthMeter.Transform.Position += new Vector3(150, 0, 0);
            DepthMeter.Foreground = Color.BlanchedAlmond;
            DepthMeter.Transform.Dimensions = new Vector3(30, 20, 1);
            DepthMeter.Transform.Scale = new Vector3(1, 1, 1);
            DepthMeter.TextHAlignment = Text.HorizontalAlignment.Right;
            DepthMeter.Background = Color.ForestGreen;
        }

        protected override void Update()
        {
            DepthMeter.Text = string.Format("{0:F2}", Parent.Transform.Position.Y);
        }

        protected override void OnColliderEnter(Collider e)
        {
            Debug.WriteLine("Collided");
        }
    }
}