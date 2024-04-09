using System.Collections;
using System.Collections.Generic;
using ThirdParty.Scripts;
using UnityEngine;
using UnityEngine.UI;

namespace Miscellaneous
{
    public static class Utility
    {
        public static T GetRandomEnum<T>()
        {
            System.Array a = System.Enum.GetValues(typeof(T));
            T v = (T)a.GetValue(Random.Range(0,a.Length));
            
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
    
        public static IEnumerator LerpFloat(float startValue, float endValue, 
            AnimationCurve speedCurve, float lengthInSeconds)
        {
            float startTime = Time.time;
            float elapsedTime = 0f;
            float newValue;
            
            while (elapsedTime < lengthInSeconds)
            {
                float t = elapsedTime / lengthInSeconds;

                newValue = Mathf.Lerp(startValue, endValue, speedCurve.Evaluate(t));

                elapsedTime = Time.time - startTime;

                yield return newValue;
            }

            newValue = endValue;

            yield return newValue;
        }
        
        public static IEnumerator LerpVector3(Vector3 startValue, Vector3 endValue, 
            AnimationCurve speedCurve, float lengthInSeconds)
        {
            float startTime = Time.time;
            float elapsedTime = 0f;
            Vector3 newValue;
            
            while (elapsedTime < lengthInSeconds)
            {
                float t = elapsedTime / lengthInSeconds;

                newValue = Vector3.Lerp(startValue, endValue, speedCurve.Evaluate(t));

                elapsedTime = Time.time - startTime;

                yield return newValue;
            }

            newValue = endValue;

            yield return newValue;
        }
        
        public static IList<T> Swap<T>(this IList<T> list, int indexA, int indexB)
        {
            (list[indexA], list[indexB]) = (list[indexB], list[indexA]);
            
            return list;
        }
        
        public static IList<T> Swap<T>(this IList<T> list, T objA, T objB)
        {
            (list[list.IndexOf(objA)], list[list.IndexOf(objB)]) 
                = (list[list.IndexOf(objB)], list[list.IndexOf(objA)]);
            
            return list;
        }
    }
}