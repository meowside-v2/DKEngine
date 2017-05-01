using DKEngine.Core.Components;
using DKEngine.Core.UI;
using System;
using System.Diagnostics;

namespace DKEngine.Core.Scripts
{
    internal sealed class TextBoxScript : Script
    {
        private TextBox _Parent;
        private TimeSpan TimeOut = new TimeSpan(0, 0, 0, 0, 50);
        private Stopwatch TimeOutStopwatch = new Stopwatch();

        private short MaxTextLenght = 64;

        public TextBoxScript(TextBox Parent)
            : base(Parent)
        {
            _Parent = Parent;
        }

        protected internal override void Update()
        {
            if (_Parent.IsFocused)
            {
                if (Console.KeyAvailable)
                {
                    if (TimeOut < TimeOutStopwatch.Elapsed)
                        TimeOutStopwatch.Reset();

                    if (TimeOutStopwatch.ElapsedMilliseconds == 0)
                    {
                        char key = Console.ReadKey(true).KeyChar;

                        while (Console.KeyAvailable) Console.ReadKey();

                        if (key == '\b')
                        {
                            if (_Parent.Text.Length > 0)
                            {
                                _Parent.Text = _Parent.Text.Substring(0, _Parent.Text.Length - 1);
                            }

                            TimeOutStopwatch.Start();
                        }
                        else if (_Parent.Text.Length < MaxTextLenght)
                        {
                            _Parent.Text += key;

                            TimeOutStopwatch.Start();
                        }
                    }
                }
                else if (TimeOutStopwatch.IsRunning)
                    TimeOutStopwatch.Reset();
            }
        }

        protected internal override void Start()
        { }

        protected internal override void OnColliderEnter(Collider e)
        { }
    }
}