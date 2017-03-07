﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DKBasicEngine_1_0
{
    internal sealed class TextBoxScript : Script
    {
        TextBox _Parent;
        private TimeSpan TimeOut = new TimeSpan(0, 0, 0, 0, 50);
        private Stopwatch TimeOutStopwatch = new Stopwatch();

        private short MaxTextLenght = 64;

        public TextBoxScript(TextBox Parent)
            : base(Parent)
        {
            _Parent = Parent;
        }

        public override void Update()
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
    }
}