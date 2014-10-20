
using System;

using Android.Content;
using Android.Util;
using Android.Views;
using Android.Graphics;
using Android.Animation;
using Android.Graphics.Drawables;

namespace EvolveQuest.Droid
{
    public enum MapPinPosition
    {
        Left,
        Center,
        Right
    };

    public class QuestMapView : View
    {
        const int BaseRadius = 5;

        Paint mainPaint;
        Paint linePaint;
        Drawable ripple;

        int radius;
        int textMargin;

        MapPinPosition[] pins;

        int[] shiverOffsets;
        ValueAnimator shiverAnimator;
        int shiverIndex;
        float currentShiverInterpolation;

        int[] heightVariants;

        public QuestMapView(Context context)
            : base(context)
        {
            Initialize();
        }

        public QuestMapView(Context context, IAttributeSet attrs)
            : base(context, attrs)
        {
            Initialize();
        }

        public QuestMapView(Context context, IAttributeSet attrs, int defStyle)
            : base(context, attrs, defStyle)
        {
            Initialize();
        }

        public int CurrentPin { get; set; }

        void Initialize()
        {
            var display = Resources.DisplayMetrics;
            // Numbers and dots
            mainPaint = new Paint
            {
                AntiAlias = true,
                Color = Color.White,
                TextSize = TypedValue.ApplyDimension(ComplexUnitType.Sp, 12, display),
            };
            linePaint = new Paint
            {
                AntiAlias = true,
                Color = Color.Argb(0xaa, 0xff, 0xff, 0xff),
            };
            linePaint.SetStyle(Paint.Style.Stroke);
            ripple = Context.Resources.GetDrawable(Resource.Drawable.ripple_background);

            radius = (int)TypedValue.ApplyDimension(ComplexUnitType.Dip, BaseRadius, display);
            textMargin = (int)TypedValue.ApplyDimension(ComplexUnitType.Dip, 10, display);

            heightVariants = new []
            {
                0,
                (int)TypedValue.ApplyDimension(ComplexUnitType.Dip, 16, display),
                -(int)TypedValue.ApplyDimension(ComplexUnitType.Dip, 14, display),
                (int)TypedValue.ApplyDimension(ComplexUnitType.Dip, 9, display),
            };

            shiverOffsets = new []
            {
                (int)TypedValue.ApplyDimension(ComplexUnitType.Dip, 9, display),
                (int)TypedValue.ApplyDimension(ComplexUnitType.Dip, 3, display),
                -(int)TypedValue.ApplyDimension(ComplexUnitType.Dip, 8, display),
                (int)TypedValue.ApplyDimension(ComplexUnitType.Dip, 4, display),
                -(int)TypedValue.ApplyDimension(ComplexUnitType.Dip, 7, display),
                (int)TypedValue.ApplyDimension(ComplexUnitType.Dip, 6, display),
            };
            shiverAnimator = ObjectAnimator.OfFloat(this, "scaleX", 0, 1);
            shiverAnimator.SetInterpolator(new TriangleWave());
            shiverAnimator.SetDuration(8000);
            shiverAnimator.RepeatCount = ValueAnimator.Infinite;
            shiverAnimator.AnimationRepeat += (sender, e) => shiverIndex++;
        }

        public void SetPins(MapPinPosition[] pins)
        {
            this.pins = pins;
            Invalidate();
            if (!shiverAnimator.IsStarted)
                shiverAnimator.Start();
        }

        protected override void OnMeasure(int widthMeasureSpec, int heightMeasureSpec)
        {
            var height = PaddingTop + PaddingBottom;
            var vspace = Resources.DisplayMetrics.HeightPixels / 5;
            heightMeasureSpec = MeasureSpec.MakeMeasureSpec(height + vspace * (pins.Length - 1), MeasureSpecMode.Exactly);

            base.OnMeasure(widthMeasureSpec, heightMeasureSpec);
        }

        public override void Draw(Canvas canvas)
        {
            if (pins == null)
                return;

            var xcoords = new int []
            {
                PaddingLeft,   // Left
                Width / 2,    // Center
                Width - PaddingRight // Right
            };

            var vspace = (Height - PaddingTop - PaddingBottom) / (pins.Length - 1);
            var currY = PaddingTop;

            for (int i = 0; i < pins.Length; i++)
            {
                var pin = pins[i];
                var x = xcoords[(int)pin];
                var y = currY + heightVariants[i % heightVariants.Length];

                // Apply shivering
                x = ApplyShivering(i, x);
                y = ApplyShivering(i + 1, y);

                // First draw joint line if needed
                if (i < pins.Length - 1)
                {
                    var x1 = xcoords[(int)pins[i + 1]];
                    var y1 = currY + vspace + heightVariants[(i + 1) % heightVariants.Length];
                    x1 = ApplyShivering(i + 1, x1);
                    y1 = ApplyShivering(i + 2, y1);
                    canvas.DrawLine(x, y, x1, y1, linePaint);
                }

                // If the point is currently the selected one, draw the ripple current state
                if (CurrentPin == i)
                {
                    var dur = shiverAnimator.Duration / 4;
                    var fraction = (float)(shiverAnimator.CurrentPlayTime % dur) / dur;
                    // Cheap interpolation
                    fraction *= fraction;
                    var rippleRadius = (int)(radius * (5 * fraction + 1)) - 8;
                    var color = Color.Argb((int)Math.Round(0x99 * (1 - fraction)),
                                    0xFF, 0xFF, 0xFF);
                    ((GradientDrawable)ripple).SetColor(color.ToArgb());
                    ripple.SetBounds(x - rippleRadius, y - rippleRadius, x + rippleRadius, y + rippleRadius);
                    ripple.Draw(canvas);
                }

                // Then draw our point
                canvas.DrawCircle(x, y, radius, mainPaint);

                // Finally draw the step number
                var ty = y;
                var tx = x;

                if (pin == MapPinPosition.Right)
                    tx += textMargin;
                else if (pin == MapPinPosition.Left)
                    tx -= textMargin * 2;
                else
                {
                    if (i == 0 || i == pins.Length - 1)
                        ty += (i == 0) ? -textMargin : 2 * textMargin;
                    else
                    {
                        var prev = pins[i - 1];
                        var next = pins[i + 1];

                        if (prev != next)
                            ty -= textMargin;
                        tx += prev == MapPinPosition.Left ? textMargin : -textMargin;
                    }
                }
                canvas.DrawText((i + 1).ToString(), tx, ty, mainPaint);

                currY += vspace;
            }
        }

        int ApplyShivering(int i, int value)
        {
            value += (int)Math.Round(shiverOffsets[(i + shiverIndex) % shiverOffsets.Length] * currentShiverInterpolation);
            return value;
        }

        public override float ScaleX
        {
            get
            {
                return currentShiverInterpolation;
            }
            set
            {
                currentShiverInterpolation = value;
                Invalidate();
            }
        }
    }

    class TriangleWave : Java.Lang.Object, ITimeInterpolator
    {
        public float GetInterpolation(float input)
        {
            var t = input * 2;
            return (float)(2 * Math.Abs(t - Math.Floor(t + 0.5)) * (1 - 2 * Math.Floor(t)));
        }
    }
}

