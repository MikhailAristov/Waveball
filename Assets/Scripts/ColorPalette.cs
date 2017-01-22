using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts
{
    public class ColorPalette : MonoBehaviour
    {
        private const int MIN_LUMINANCE = 50;
        private const int MID_LUMINANCE = 80;
        private const int MAX_LUMINANCE = 100;

        public List<Color> Palette { get; set; }

        void Start()
        {
            Palette = new List<Color>();
            for (int i = 0; i < 5; i++)
            {
                Palette.Add(new Color());
            }
            Palette.Add(HSLtoRGB(0, 0, 0));
            Palette.Add(HSLtoRGB(0, 0, 0.25f));
            Palette.Add(HSLtoRGB(0, 0, 0.5f));
            Palette.Add(HSLtoRGB(0, 0, 0.75f));
            Palette.Add(HSLtoRGB(0, 0, 1));

        }

        void Update()
        {
            
        }

        //public void CreatePaletteWith(Color color)
        //{
            
        //}


        //public void CreatePaletteWith(int r, int g, int b, float a = 1.0f)
        //{
            
        //}

        public void CreatePaletteWith(float h, float s, float l)
        {
            var hue = h;

            var primarySaturation = s;
            var secondarySaturation = (s >= 30) ? s - 30 : s + 30;

            var realLuminance = l;

            Palette[0] = HSLtoRGB(hue, primarySaturation, realLuminance);
            Palette[1] = HSLtoRGB(hue, primarySaturation, MID_LUMINANCE);
            Palette[2] = HSLtoRGB(hue, primarySaturation, MAX_LUMINANCE);
            Palette[3] = HSLtoRGB(hue, secondarySaturation, MIN_LUMINANCE);
            Palette[4] = HSLtoRGB(hue, secondarySaturation, MAX_LUMINANCE);
           
        }

        private Color HSLtoRGB(float h, float s, float l)
        {
            float r, g, b;

            if (s == 0)
            {
                r = g = b = (int)(l * 255); // achromatic
            }
            else
            {
                var q = l < 0.5f ? l * (1 + s) : l + s - l * s;
                var p = 2 * l - q;
                r = hue2rgb(p, q, h + 1 / 3);
                g = hue2rgb(p, q, h);
                b = hue2rgb(p, q, h - 1 / 3);
            }

            return new Color(r * 255, g * 255, b * 255);
        }

        private float hue2rgb(float p, float q, float t)
        {
            if (t < 0) t += 1;
            else if (t > 1) t -= 1;
            else if (t < 1 / 6) return p + (q - p) * 6 * t;
            else if (t < 1 / 2) return q;
            else if (t < 2 / 3) return p + (q - p) * (2 / 3 - t) * 6;
            return p;
        }
    }
}
