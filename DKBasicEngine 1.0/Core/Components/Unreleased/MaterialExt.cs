/*
* (C) 2017 David Knieradl 
*/

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace DKBasicEngine_1_0
{
    /// <summary>
    /// Low-Memory Material, unreleased stuff
    /// </summary>
    public class MaterialExt
    {
        /// <summary>
        /// Source image used for this Material
        /// </summary>
        public readonly Bitmap[] SourceImage = null;

        /// <summary>
        /// Represents scaled length of image in pixels
        /// </summary>
        public readonly int Width = 0;

        /// <summary>
        /// Represents scaled height of image in pixels
        /// </summary>
        public readonly int Height = 0;

        /// <summary>
        /// Number of frames
        /// </summary>
        public readonly int Frames = 1;

        /// <summary>
        /// Total duration of animated image
        /// </summary>
        public readonly int Duration = 1;

        /// <summary>
        /// Duration between two frames of animation
        /// </summary>
        public readonly int DurationPerFrame = 1;

        /// <summary>
        /// Returns true if image is animated
        /// </summary>
        public readonly bool IsAnimated = false;

        /// <summary>
        /// Returns true if image is looped
        /// </summary>
        public readonly bool IsLooped = false;


        //public readonly int BytesPerPixel = -1;
        
        /// <summary>
        /// Loads image and creates new material
        /// </summary>
        /// <param name="source">Source image</param>
        public MaterialExt(Image source)
        {
            if(source != null)
            {
                FrameDimension frameDimension = new FrameDimension(source.FrameDimensionsList[0]);
                Frames = source.GetFrameCount(frameDimension);

                SourceImage = new Bitmap[Frames];
                //SourceImage = source;

                Width = source.Width;
                Height = source.Height;

                if (ImageAnimator.CanAnimate(source))
                {
                    int delay = 0;
                    int this_delay = 0;
                    int index = 0;

                    for (int i = 0; i < Frames; i++)
                    {
                        this_delay = BitConverter.ToInt32(source.GetPropertyItem(20736).Value, index) * 10;
                        delay += (this_delay < 1 ? 33 : this_delay);
                        index += 4;
                    }

                    Duration = delay;
                    DurationPerFrame = Duration / Frames;
                    IsAnimated = true;
                    IsLooped = BitConverter.ToInt16(source.GetPropertyItem(20737).Value, 0) != 1;
                    
                    for(int i = 0; i < Frames; i++)
                    {
                        source.SelectActiveFrame(frameDimension, i);
                        SourceImage[i] = new Bitmap(source);
                    }
                }

                else
                {
                    SourceImage[0] = (Bitmap)source;
                }

                /*switch (source.PixelFormat)
                {
                    case PixelFormat.Format32bppArgb:
                        BytesPerPixel = 4;
                        break;

                    case PixelFormat.Format24bppRgb:
                        BytesPerPixel = 3;
                        break;

                    default:
                        throw new Exception("Unsupported");
                }*/
            }
        }

        /// <summary>
        /// Creates new material with given color and scales it by parent's given scales
        /// </summary>
        /// <param name="clr">Source color</param>
        /// <param name="Parent">I3Dimensional used for material scale</param>
        public MaterialExt(Color clr, GameObject Parent)
        {
            this.Width = (int)Parent.Transform.Dimensions.Width;
            this.Height = (int)Parent.Transform.Dimensions.Height;

            Frames = 1;
            SourceImage = new Bitmap[Frames];
            int BytesPerPixel = 4;

            int size = Width * Height * BytesPerPixel;
            byte[] ImageData = new byte[size];

            for (int index = 0; index < size; index += BytesPerPixel)
            {
                ImageData[index + 3] = clr.A;
                ImageData[index + 2] = clr.R;
                ImageData[index + 1] = clr.G;
                ImageData[index] = clr.B;
            }

            unsafe
            {
                fixed(byte* data = ImageData)
                {
                    using (Bitmap OutFrame = new Bitmap(Width,
                                                       Height,
                                                       Width * BytesPerPixel,
                                                       PixelFormat.Format32bppArgb,
                                                       new IntPtr(data)))
                    {
                        SourceImage[0] = new Bitmap(OutFrame);
                    }
                }
            }
        }

        /*/// <summary>
        /// Returns color of pixel on coordinations
        /// </summary>
        /// <param name="x">Column coordination</param>
        /// <param name="y">Row coordination</param>
        /// <param name="frame">Layer/Frame coordination</param>
        /// <returns></returns>
        public string PixelToString(int x, int y, int frame = 0)
        {
            return string.Format("#{0:X2}{1:X2}{2:X2}{3:X2}", this.colorMapA[frame][y][x],
                                                              this.colorMapR[frame][y][x],
                                                              this.colorMapG[frame][y][x],
                                                              this.colorMapB[frame][y][x]);
        }*/
        
        /// <summary>
        /// Render material into engine image buffer
        /// </summary>
        /// <param name="Parent">I3Dimensional for coordiantions</param>
        public void Render(GameObject Parent, Color? ReColor = null)
        {

            int AnimationState = Parent.Animator != null ? Parent.Animator.AnimationState : 0;
            bool HasShadow = Parent.HasShadow;

            BitmapData ImageData = SourceImage[AnimationState].LockBits(new Rectangle(0, 0, Width, Height), ImageLockMode.ReadOnly, SourceImage[AnimationState].PixelFormat);

            int BytesPerPixel = -1;
            switch (ImageData.PixelFormat)
            {
                case PixelFormat.Format32bppArgb:
                    BytesPerPixel = 4;
                    break;

                case PixelFormat.Format24bppRgb:
                    BytesPerPixel = 3;
                    break;

                default:
                    throw new Exception("Unsupported");
            }

            byte[] data = new byte[ImageData.Stride * ImageData.Height];
            Marshal.Copy(ImageData.Scan0, data, 0, ImageData.Stride * ImageData.Height);

            SourceImage[AnimationState].UnlockBits(ImageData);

            float CamX = Engine.BaseCam != null ? Engine.BaseCam.X : 0;
            float CamY = Engine.BaseCam != null ? Engine.BaseCam.Y : 0;

            int x = (int)(Parent.Transform.Position.X - CamX);
            int y = (int)(Parent.Transform.Position.Y - CamY);

            float RasteredHeight = this.Height * Parent.Transform.Scale.Y;
            float RasteredWidth = this.Width * Parent.Transform.Scale.X;

            float NonRasteredWidthRatio = 1 / Parent.Transform.Scale.X;
            float NonRasteredHeightRatio = 1 / Parent.Transform.Scale.Y;

            float NonRasteredHeight = 0;
            float NonRasteredWidth = 0;
            if (ReColor == null)
            {
                for (int row = 0; row < RasteredHeight; row++)
                {
                    NonRasteredWidth = 0;

                    if (y + row >= Engine.Render.RenderHeight)
                        break;

                    for (int column = 0; column < RasteredWidth; column++)
                    {

                        if (x + column >= Engine.Render.RenderWidth)
                            break;

                        if (IsOnScreen(x + column, y + row))
                        {

                            int offset = (int)(3 * ((y + row) * Engine.Render.RenderWidth + (x + column)));
                            int keyOffset = (int)((y + row) * Engine.Render.RenderWidth + (x + column));

                            int tempColumn = (int)NonRasteredWidth;
                            int tempRow = (int)NonRasteredHeight;

                            int index = BytesPerPixel * (tempRow * Width + tempColumn);
                            
                            if (Engine.Render.imageBufferKey[keyOffset] != 255 && BytesPerPixel == 4 ? data[index + 3] != 0 : true)
                            {
                                Color temp = MixPixel(Engine.Render.imageBufferKey[keyOffset], Engine.Render.imageBuffer[offset + 2], Engine.Render.imageBuffer[offset + 1], Engine.Render.imageBuffer[offset],
                                                        BytesPerPixel == 4 ? data[index + 3] : (byte)255, data[index + 2], data[index + 1], data[index]);

                                Engine.Render.imageBufferKey[keyOffset] = temp.A;

                                Engine.Render.imageBuffer[offset] = temp.B;
                                Engine.Render.imageBuffer[offset + 1] = temp.G;
                                Engine.Render.imageBuffer[offset + 2] = temp.R;
                            }
                        }

                        NonRasteredWidth += NonRasteredWidthRatio;
                    }

                    NonRasteredHeight += NonRasteredHeightRatio;
                }
            }
            else
            {
                Color tempColor = (Color)ReColor;

                for (int row = 0; row < RasteredHeight; row++)
                {
                    NonRasteredWidth = 0;

                    if (y + row >= Engine.Render.RenderHeight)
                        break;

                    for (int column = 0; column < RasteredWidth; column++)
                    {

                        if (x + column >= Engine.Render.RenderWidth)
                            break;

                        if (IsOnScreen(x + column, y + row))
                        {

                            int offset = (int)(3 * ((y + row) * Engine.Render.RenderWidth + (x + column)));
                            int keyOffset = (int)((y + row) * Engine.Render.RenderWidth + (x + column));

                            int tempColumn = (int)NonRasteredWidth;
                            int tempRow = (int)NonRasteredHeight;

                            int index = BytesPerPixel * (tempRow * Width + tempColumn);

                            if (Engine.Render.imageBufferKey[keyOffset] != 255 && BytesPerPixel == 4 ? data[index + 3] != 0 : true)
                            {
                                Color temp = MixPixel(Color.FromArgb(Engine.Render.imageBufferKey[keyOffset], Engine.Render.imageBuffer[offset + 2], Engine.Render.imageBuffer[offset + 1], Engine.Render.imageBuffer[offset]),
                                                        tempColor);

                                Engine.Render.imageBufferKey[keyOffset] = temp.A;

                                Engine.Render.imageBuffer[offset] = temp.B;
                                Engine.Render.imageBuffer[offset + 1] = temp.G;
                                Engine.Render.imageBuffer[offset + 2] = temp.R;
                            }
                        }

                        NonRasteredWidth += NonRasteredWidthRatio;
                    }

                    NonRasteredHeight += NonRasteredHeightRatio;
                }
            }

            if (HasShadow)
            {
                NonRasteredHeight = 0;
                NonRasteredWidth = 0;

                x++;
                y++;

                for (int row = 0; row < RasteredHeight; row++)
                {
                    NonRasteredWidth = 0;

                    if (y + row >= Engine.Render.RenderHeight)
                        break;

                    for (int column = 0; column < RasteredWidth; column++)
                    {
                        if (x + column >= Engine.Render.RenderWidth)
                            break;

                        if (IsOnScreen(x + column, y + row))
                        {
                            int offset = (int)(3 * ((y + row) * Engine.Render.RenderWidth + (x + column)));
                            int keyOffset = (int)((y + row) * Engine.Render.RenderWidth + (x + column));

                            int tempColumn = (int)NonRasteredWidth;
                            int tempRow = (int)NonRasteredHeight;

                            int index = BytesPerPixel * (tempRow * Width + tempColumn);

                            if (Engine.Render.imageBufferKey[keyOffset] != 255 && BytesPerPixel == 4 ? data[index + 3] != 0 : true)
                            {
                                Color temp = MixPixel(Engine.Render.imageBufferKey[keyOffset], Engine.Render.imageBuffer[offset + 2], Engine.Render.imageBuffer[offset + 1], Engine.Render.imageBuffer[offset],
                                                      0xBB, 0x00, 0x00, 0x00);

                                Engine.Render.imageBufferKey[keyOffset] = temp.A;

                                Engine.Render.imageBuffer[offset] = temp.B;
                                Engine.Render.imageBuffer[offset + 1] = temp.G;
                                Engine.Render.imageBuffer[offset + 2] = temp.R;
                            }
                        }

                        NonRasteredWidth += NonRasteredWidthRatio;
                    }

                    NonRasteredHeight += NonRasteredHeightRatio;
                }
            }
        }

        public Color MixPixel(byte topA, byte topR, byte topG, byte topB, byte bottomA, byte bottomR, byte bottomG, byte bottomB)
        {
            if (topA == 0)
                return Color.FromArgb(bottomA, bottomR, bottomG, bottomB);

            if (bottomA == 0)
                return Color.FromArgb(topA, topR, topG, topB);

            float opacityTop = (float)topA / 255;

            byte newA = (byte)(topA + bottomA >= 255 ? 255 : topA + bottomA);
            byte A = (byte)(newA - topA);

            float opacityBottom = (float)A / 255;

            byte R = (byte)(topR * opacityTop + bottomR * opacityBottom);
            byte G = (byte)(topG * opacityTop + bottomG * opacityBottom);
            byte B = (byte)(topB * opacityTop + bottomB * opacityBottom);

            return Color.FromArgb(newA, R, G, B);
        }

        public Color MixPixel(Color top, Color bottom)
        {
            if (top.A == 0)
                return bottom;

            if (bottom.A == 0)
                return top;

            float opacityTop = (float)top.A / 255;

            byte newA = (byte)(top.A + bottom.A >= 255 ? 255 : top.A + bottom.A);
            byte A = (byte)(newA - top.A);

            float opacityBottom = (float)A / 255;

            byte R = (byte)(top.R * opacityTop + bottom.R * opacityBottom);
            byte G = (byte)(top.G * opacityTop + bottom.G * opacityBottom);
            byte B = (byte)(top.B * opacityTop + bottom.B * opacityBottom);

            return Color.FromArgb(newA, R, G, B);
        }

        private bool IsOnScreen(float x, float y)
        {
            return x >= 0 && x < Engine.Render.RenderWidth && y >= 0 && y < Engine.Render.RenderHeight;
        }
    }
}
