using DKBasicEngine_1_0.Properties;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace DKBasicEngine_1_0
{
    public static class Database
    {
        private enum Font
        {
            Num0,
            Num1,
            Num2,
            Num3,
            Num4,
            Num5,
            Num6,
            Num7,
            Num8,
            Num9,
            A,
            AngleBracketLeft,
            AngleBracketRight,
            ArrowToLeft,
            ArrowToRight,
            ArrowToTop,
            B,
            Backslash,
            BraceLeft,
            BraceRight,
            BracketLeft,
            BracketRight,
            C,
            Comma,
            D,
            Dot,
            E,
            Equals,
            ExclamationMark,
            F,
            G,
            H,
            Hashtag,
            I,
            J,
            K,
            L,
            M,
            Minus,
            N,
            O,
            P,
            Percent,
            Q,
            QuestionMark,
            QuotationMarks,
            R,
            S,
            Semicolon,
            Slash,
            StarLarge,
            StarSmall,
            T,
            U,
            Underscore,
            V,
            W,
            X,
            Y,
            Z,
            NumberOfTypes
                
        };

        private static Dictionary<char, Font> font = new Dictionary<char, Font>()
        {
            { '0' , Font.Num0 },
            { '1' , Font.Num1 },
            { '2' , Font.Num2 },
            { '3' , Font.Num3 },
            { '4' , Font.Num4 },
            { '5' , Font.Num5 },
            { '6' , Font.Num6 },
            { '7' , Font.Num7 },
            { '8' , Font.Num8 },
            { '9' , Font.Num9 },
            { 'A' , Font.A },
            { 'B' , Font.B },
            { 'C' , Font.C },
            { 'D' , Font.D },
            { 'E' , Font.E },
            { 'F' , Font.F },
            { 'G' , Font.G },
            { 'H' , Font.H },
            { 'I' , Font.I },
            { 'J' , Font.J },
            { 'K' , Font.K },
            { 'L' , Font.L },
            { 'M' , Font.M },
            { 'N' , Font.N },
            { 'O' , Font.O },
            { 'P' , Font.P },
            { 'Q' , Font.Q },
            { 'R' , Font.R },
            { 'S' , Font.S },
            { 'T' , Font.T },
            { 'U' , Font.U },
            { 'V' , Font.V },
            { 'W' , Font.W },
            { 'X' , Font.X },
            { 'Y' , Font.Y },
            { 'Z' , Font.Z },
            { '-' , Font.Minus },
            { '?' , Font.QuestionMark },
            { '!' , Font.ExclamationMark },
            { '.' , Font.Dot },
            { ',' , Font.Comma },
            { '[' , Font.AngleBracketLeft },
            { ']' , Font.AngleBracketRight },
            { '>' , Font.ArrowToRight },
            { '<' , Font.ArrowToLeft },
            { '^' , Font.ArrowToTop },
            { '{' , Font.BraceLeft },
            { '}' , Font.BraceRight },
            { '(' , Font.BracketLeft },
            { ')' , Font.BracketRight },
            { '=' , Font.Equals },
            { '#' , Font.Hashtag },
            { '%' , Font.Percent },
            { '"' , Font.QuotationMarks },
            { ';' , Font.Semicolon },
            //{ '*' , Font.StarLarge },
            { '*' , Font.StarSmall },
            { '_' , Font.Underscore },
            { '/' , Font.Slash },
            { '\\' , Font.Backslash }
        };

        private static List<Material> letterMaterial = new List<Material>();

        private static void CreateLetterReferences()
        {
            using (BinaryReader br = new BinaryReader(new MemoryStream(Resources.FontFile)))
            {
                int lenght = br.ReadInt32();

                for (int index = 0; index < lenght; index++)
                {
                    byte[] byteArray = br.ReadBytes(br.ReadInt32());

                    using (MemoryStream ms = new MemoryStream(byteArray))
                    {
                        letterMaterial.Add(new Material((Bitmap)Image.FromStream(ms)));
                    }
                }
            }
        }

        private static Dictionary<string, int> GameObjects = new Dictionary<string, int>();

        private static List<Material> GameObjectsMaterial = new List<Material>();

        internal static void InitDatabase()
        {
            AddNewGameObjectMaterial("border", new Material(Resources.border));
            AddNewGameObjectMaterial("splashScreen", new Material(Resources.DKEngine_splash2));

            CreateLetterReferences();
        }

        public static Material GetLetter(this char ch)
        {
            Material retValue = null;

            try
            {
                retValue = letterMaterial[(int)font[Char.ToUpper(ch)]];
            }
            catch
            {
                retValue = letterMaterial[(int)font['?']];
            }

            return retValue;
        }

        public static void AddNewGameObjectMaterial(string ObjectName, Material Object)
        {
            GameObjects.Add(ObjectName, GameObjectsMaterial.Count);
            GameObjectsMaterial.Add(Object);
        }

        public static Material GetGameObjectMaterial(string Key)
        {
            Material retValue = null;

            try
            {
                retValue = GameObjectsMaterial[GameObjects[Key]];
            }
            catch (Exception ex)
            {
                MessageBox.Show("Object not found\n" + ex);
            }

            return retValue;
        }

        public static Material GetGameObjectMaterial(int position)
        {
            Material retValue = null;

            try
            {
                retValue = GameObjectsMaterial[position];
            }
            catch (Exception ex)
            {
                MessageBox.Show("Object not found\n" + ex);
            }

            return retValue;
        }

        public static int GetMaterialDatabasePosition(string Key)
        {
            return GameObjects[Key];
        }

        public static string GetMaterialDatabaseKey(int Position)
        {
            return GameObjects.FirstOrDefault(x => x.Value == Position).Key;
        }
    }
}
