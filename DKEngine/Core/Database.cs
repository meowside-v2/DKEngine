using DKEngine.Core.Components;
using DKEngine.Properties;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Resources;

namespace DKEngine.Core
{
    /// <summary>
    /// DKEngine library database holding all loaded materials, scenes, etc.
    /// </summary>
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
            Colon,
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
            { ':' , Font.Colon },
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
            { '☼' , Font.StarLarge },
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

        private static Dictionary<string, Material> CachedMaterials = new Dictionary<string, Material>();
        private static Dictionary<string, Scene> CachedScenes = new Dictionary<string, Scene>();

        internal static void InitDatabase()
        {
            AddNewGameObjectMaterial("border", new Material(Resources.border));
            AddNewGameObjectMaterial("splashScreen", new Material(Resources.DKEngine_splash2));

            CreateLetterReferences();
        }

        internal static Scene GetScene(string Key)
        {
            Scene retValue = null;

            try
            {
                retValue = CachedScenes[Key];
            }
            catch
            { }

            return retValue;
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
            try
            {
                if (Object != null)
                {
                    CachedMaterials.Add(ObjectName, Object);
                }
                else
                    throw new Exception("Material is null\n" + Object.ToString());
            }
            catch (Exception e)
            {
                Debug.WriteLine("Object not found\n" + e);
            }
        }

        public static Material GetGameObjectMaterial(string Key)
        {
            Material retValue = null;

            try
            {
                retValue = CachedMaterials[Key];
            }
            catch (Exception e)
            {
                Debug.WriteLine("Object not found\n" + e);
            }

            return retValue;
        }

        public static Material GetGameObjectMaterial(int Position)
        {
            Material retValue = null;

            try
            {
                retValue = CachedMaterials.ElementAtOrDefault(Position).Value;
            }
            catch (Exception e)
            {
                Debug.WriteLine("Object not found\n" + e);
            }

            return retValue;
        }

        public static string GetMaterialDatabaseKey(int Position)
        {
            return CachedMaterials.ElementAtOrDefault(Position).Key; //.FirstOrDefault(x => x.Value == Position).Key;
        }

        public static void LoadResources(ResourceSet source)
        {
            foreach (DictionaryEntry entry in source)
            {
                if (entry.Value is Image)
                {
                    AddNewGameObjectMaterial((string)entry.Key, new Material((Bitmap)entry.Value));
                }
            }
        }

        internal static void RewriteWorld(string Name)
        {
            try
            {
                Scene tmp = (Scene)Activator.CreateInstance(CachedScenes[Name].GetType());
                Scene ToBeDestroyed = CachedScenes[Name];

                foreach (var pair in ToBeDestroyed.AllComponents)
                    pair.Value.Destroy();

                int ComponentCount = ToBeDestroyed.AllBehaviors.Count;
                for (int i = ComponentCount - 1; i >= 0; i--)
                    ToBeDestroyed.AllBehaviors[i].Destroy();

                tmp.Init();
                CachedScenes[Name] = tmp;
            }
            catch
            { }
        }

        internal static void AddScene(Scene Source)
        {
            try
            {
                CachedScenes.Add(Source.Name, Source);
            }
            catch { }
        }
    }
}