using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using UnityEngine;

public class ColorPalette : MonoBehaviour
{
	private const int MIN_LUMINANCE = 50;
	private const int MID_LUMINANCE = 80;
	private const int MAX_LUMINANCE = 100;

	public float H;
    public float S;
    public float L;

	public List<Color> Palette;

	void Awake()
	{
		Palette = new List<Color> (10);
        //for (int i = 0; i < 5; i++)
        //{
        //    Palette.Add(new Color());
        //}
        Palette.Add(new Color((float)6/255, (float)112 /255, (float)127 /255));
        Palette.Add(new Color((float)10 /255, (float)179 /255, (float)204 /255));
        Palette.Add(new Color((float)13 /255, (float)224 /255, (float)225 /255));
        Palette.Add(new Color((float)45 /255, (float)117 /255, (float)227 /255));
        Palette.Add(new Color((float)89 /255, (float)234 /255, (float)255 /255));
        Palette.Add ( HSLtoRGB ( 0, 0, 0 ) );
		Palette.Add ( HSLtoRGB ( 0, 0, 0.25f ) );
		Palette.Add ( HSLtoRGB ( 0, 0, 0.5f ) );
		Palette.Add ( HSLtoRGB ( 0, 0, 0.75f ) );
		Palette.Add ( HSLtoRGB ( 0, 0, 1 ) );

		//CreatePaletteWith ( H, S, L );
	}

	private void CreatePaletteWith(float h, float s, float l)
	{
		var hue = h;

		var primarySaturation = s;
		var secondarySaturation = (s >= 30) ? s - 30 : s + 30;

		var realLuminance = l;

		Palette[0] = HSLtoRGB ( hue, primarySaturation, realLuminance );
		Palette[1] = HSLtoRGB ( hue, primarySaturation, MID_LUMINANCE );
		Palette[2] = HSLtoRGB ( hue, primarySaturation, MAX_LUMINANCE );
		Palette[3] = HSLtoRGB ( hue, secondarySaturation, MIN_LUMINANCE );
		Palette[4] = HSLtoRGB ( hue, secondarySaturation, MAX_LUMINANCE );
	}

	private Color HSLtoRGB(float h, float s, float l)
	{
	    if (s == 0.0f)
	    {
	        return new Color(l, l, l); // achromatic
	    }

	    else
	    {
	        var c = (1 - Math.Abs(2*l - 1))*s;
	        var h1 = (360*h)/60;
	        var x = c*(1 - Math.Abs((h1%2) - 1));
            return GenerateRGB(c, x, h1);
	    }
	}

	private Color GenerateRGB(float c, float x, float h1)
	{
	    var c1 = c/255;
	    var x1 = x/255;
	    if (0 <= h1 && h1 <= 1) return new Color(c1, x1, 0);
	    if (1 <= h1 && h1 <= 2) return new Color(x1, c1, 0);
	    if (2 <= h1 && h1 <= 3) return new Color(0, c1, x1);
	    if (3 <= h1 && h1 <= 4) return new Color(0, x1, c1);
	    if (4 <= h1 && h1 <= 5) return new Color(x1, 0, c1);
	    if (5 <= h1 && h1 < 6) return new Color(c1, 0, x1);
        else return new Color(0, 0, 0);
	}
}
