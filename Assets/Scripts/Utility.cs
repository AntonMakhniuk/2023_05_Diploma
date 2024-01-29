using ThirdParty.Scripts;
using UnityEngine;
using UnityEngine.UI;

public abstract class Utility
{
    public static T GetRandomEnum<T>()
    {
        System.Array a = System.Enum.GetValues(typeof(T));
        T v = (T)a.GetValue(UnityEngine.Random.Range(0,a.Length));
            
        return v;
    }

    public static void CreateGradientSpriteAndApplyToSlider(Slider slider, Image backgroundImage,
        CustomGradientKey[] colorKeys)
    {
        var sliderRect = slider.GetComponent<RectTransform>().rect;
        
        var gradient = new CustomGradient();

        foreach (var colorKey in colorKeys)
        {
            gradient.AddKey(colorKey);
        }

        Texture2D texture2D = new Texture2D((int) sliderRect.width, (int)sliderRect.height)
        {
            wrapMode = TextureWrapMode.Clamp
        };

        for (int x = 0; x < sliderRect.width; x++)
        {
            float t = Mathf.InverseLerp(0, sliderRect.width - 1, x);

            for (int y = 0; y < sliderRect.height; y++)
            {
                texture2D.SetPixel(x, y, gradient.Evaluate(t));
            }
        }
        
        texture2D.Apply();

        Sprite sprite = Sprite.Create
        (
            texture2D, 
            new Rect(0, 0, sliderRect.width, sliderRect.height), 
            Vector2.one * 0.5f
        );

        backgroundImage.sprite = sprite;
    }
}