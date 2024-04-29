using UnityEngine;

namespace SpookSuite.Util
{
    internal class RainbowController
    {
        public RainbowController() { r = 255; }

        public float speed = 1;

        public float r, g, b, a = 0;

        public void Update()
        {
            if (r > 0 && b == 0)
            {
                g += speed;
                r -= speed;
            }
            if (g > 0 && r == 0)
            {
                b += speed;
                g -= speed;
            }
            if (b > 0 && g == 0)
            {
                r += speed;
                b -= speed;
            }

            r = Mathf.Clamp(r, 0, 255);
            g = Mathf.Clamp(g, 0, 255);
            b = Mathf.Clamp(b, 0, 255);
        }
        public Color GetColor() => new Color(r / 255, g / 255, b / 255);
        public Color GetColor255() => new Color(r, g, b);
        public RGBAColor GetRGBA() => new RGBAColor(r / 255, g / 255, b / 255, a); //only should be used if u aint setting alpha with the color, otherwise its always 1.
        public RGBAColor GetRGBA255() => new RGBAColor(r, g, b, a);
    }
}
