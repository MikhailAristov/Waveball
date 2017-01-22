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

        public Color[] Palette { get; set; }

        void Start()
        {
            Palette = new Color[5];
        }

        void Update()
        {
            
        }

        public void CreatePaletteWith(Color color)
        {
            
        }


        public void CreatePaletteWith(int r, int g, int b, int a)
        {
            
        }

        public void CreatePaletteWith(int h, int s, int l)
        {
            var hue = h;

            var primarySaturation = s;
            var secondarySaturation = (s >= 30) ? s - 30 : s + 30;

            var realLuminance = l;



        }
    }
}
