using System;
using System.Collections.Generic;
using Android.Content;
using Android.Content.Res;
using Android.Graphics;
using Android.Runtime;
using Android.Util;
using Android.Widget;

namespace EvolveQuest.Droid.Controls
{
    public class RobotoTextView : TextView
    {
        private const int RobotoThin = 0;
        private const int RobotoThinItalic = 1;
        private const int RobotoLight = 2;
        private const int RobotoLightItalic = 3;
        private const int RobotoRegular = 4;
        private const int RobotoItalic = 5;
        private const int RobotoMedium = 6;
        private const int RobotoMediumItalic = 7;
        private const int RobotoBold = 8;
        private const int RobotoBoldItalic = 9;
        private const int RobotoBlack = 10;
        private const int RobotoBlackItalic = 11;
        private const int RobotoCondensed = 12;
        private const int RobotoCondensedItalic = 13;
        private const int RobotoCondensedBold = 14;
        private const int RobotoCondensedBoldItalic = 15;

        private TypefaceStyle style = TypefaceStyle.Normal;

        private static readonly Dictionary<int, Typeface> typefaces = new Dictionary<int, Typeface>(16);

        public RobotoTextView(Context context)
            : base(context)
        {
        }

        protected RobotoTextView(IntPtr javaReference, JniHandleOwnership transfer)
            : base(javaReference, transfer)
        {
        }


        public RobotoTextView(Context context, IAttributeSet attrs)
            : base(context, attrs)
        {
            this.Initialize(context, attrs);
        }

        public RobotoTextView(Context context, IAttributeSet attrs, int defStyle)
            : base(context, attrs, defStyle)
        {
            this.Initialize(context, attrs);
        }

        private void Initialize(Context context, IAttributeSet attrs)
        {


            try
            {
                TypedArray values = context.ObtainStyledAttributes(attrs, Resource.Styleable.RobotoTextView);

                int typefaceValue = values.GetInt(Resource.Styleable.RobotoTextView_typeface, 0);
                values.Recycle();
                var font = this.ObtainTypeface(context, typefaceValue);
                this.SetTypeface(font, this.style);
            }
            catch (Exception ex)
            {

            }

        }

        private Typeface ObtainTypeface(Context context, int typefaceValue)
        {
            try
            {

                Typeface typeface = null;
                if (typefaces.ContainsKey(typefaceValue))
                    typeface = typefaces[typefaceValue];

                if (typeface == null)
                {
                    typeface = this.CreateTypeface(context, typefaceValue);
                    typefaces.Add(typefaceValue, typeface);
                }
                return typeface;
            }
            catch (Exception ex)
            {

            }

            return null;
        }

        private Typeface CreateTypeface(Context context, int typefaceValue)
        {
            try
            {

                Typeface typeface;
                switch (typefaceValue)
                {
                    case RobotoThin:
                        typeface = Typeface.CreateFromAsset(context.Assets, "fonts/Roboto-Thin.ttf");
                        break;
                    case RobotoThinItalic:
                        typeface = Typeface.CreateFromAsset(context.Assets, "fonts/Roboto-ThinItalic.ttf");
                        style = TypefaceStyle.Italic;
                        break;
                    case RobotoLight:
                        typeface = Typeface.CreateFromAsset(context.Assets, "fonts/Roboto-Light.ttf");
                        break;
                    case RobotoLightItalic:
                        typeface = Typeface.CreateFromAsset(context.Assets, "fonts/Roboto-LightItalic.ttf");
                        style = TypefaceStyle.Italic;
                        break;
                    case RobotoRegular:
                        typeface = Typeface.CreateFromAsset(context.Assets, "fonts/Roboto-Regular.ttf");
                        break;
                    case RobotoItalic:
                        typeface = Typeface.CreateFromAsset(context.Assets, "fonts/Roboto-Italic.ttf");
                        style = TypefaceStyle.Italic;
                        break;
                    case RobotoMedium:
                        typeface = Typeface.CreateFromAsset(context.Assets, "fonts/Roboto-Medium.ttf");
                        break;
                    case RobotoMediumItalic:
                        typeface = Typeface.CreateFromAsset(context.Assets, "fonts/Roboto-MediumItalic.ttf");
                        style = TypefaceStyle.Italic;
                        break;
                    case RobotoBold:
                        typeface = Typeface.CreateFromAsset(context.Assets, "fonts/Roboto-Bold.ttf");
                        style = TypefaceStyle.Bold;
                        break;
                    case RobotoBoldItalic:
                        typeface = Typeface.CreateFromAsset(context.Assets, "fonts/Roboto-BoldItalic.ttf");
                        style = TypefaceStyle.BoldItalic;
                        break;
                    case RobotoBlack:
                        typeface = Typeface.CreateFromAsset(context.Assets, "fonts/Roboto-Black.ttf");
                        break;
                    case RobotoBlackItalic:
                        typeface = Typeface.CreateFromAsset(context.Assets, "fonts/Roboto-BlackItalic.ttf");
                        style = TypefaceStyle.Italic;
                        break;
                    case RobotoCondensed:
                        typeface = Typeface.CreateFromAsset(context.Assets, "fonts/Roboto-Condensed.ttf");
                        break;
                    case RobotoCondensedItalic:
                        typeface = Typeface.CreateFromAsset(context.Assets, "fonts/Roboto-CondensedItalic.ttf");
                        style = TypefaceStyle.Italic;
                        break;
                    case RobotoCondensedBold:
                        typeface = Typeface.CreateFromAsset(context.Assets, "fonts/Roboto-BoldCondensed.ttf");
                        style = TypefaceStyle.Bold;
                        break;
                    case RobotoCondensedBoldItalic:
                        typeface = Typeface.CreateFromAsset(context.Assets, "fonts/Roboto-BoldCondensedItalic.ttf");
                        style = TypefaceStyle.BoldItalic;
                        break;
                    default:
                        throw new ArgumentException("Unknown typeface attribute value " + typefaceValue);
                }
                return typeface;

            }
            catch (Exception)
            {
            }

            return null;
        }
    }
}