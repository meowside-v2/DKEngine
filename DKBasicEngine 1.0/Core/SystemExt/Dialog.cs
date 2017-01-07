using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DKBasicEngine_1_0.Core.SystemExt
{
    class Dialog : GameObject, ICore, IGraphics
    {

        public TextBlock Caption;
        public readonly List<I3Dimensional> Content = new List<I3Dimensional>();

        public Dialog(string Caption)
        {
            this.Caption = new TextBlock(this);
            this.Caption.Text = Caption;
        }

        public void ChangeContent(params I3Dimensional[] stuff)
        {

        }
    }
}
