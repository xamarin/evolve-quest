
using Android.Widget;
using Android.Util;
using Android.Views;
using Android.Content;

namespace EvolveQuest.Droid
{
    public class LockableScrollView : ScrollView
    {
        public bool IsScrollable { get; set; }

        public LockableScrollView(Context context, IAttributeSet attrs, int defStyle)
            : base(context, attrs, defStyle)
        {
        }

        public LockableScrollView(Context context, IAttributeSet attrs)
            : base(context, attrs)
        {
        }

        public LockableScrollView(Context context)
            : base(context)
        {
        }

        public override bool OnTouchEvent(MotionEvent e)
        {
            switch (e.Action)
            {
                case MotionEventActions.Down:
				// if we can scroll pass the event to the superclass
                    if (IsScrollable)
                        return base.OnTouchEvent(e);
				// only continue to handle the touch event if scrolling enabled
                    return IsScrollable; // mScrollable is always false at this point

            }

            return base.OnTouchEvent(e);
        }

        public override bool OnInterceptTouchEvent(MotionEvent ev)
        {
            // Don't do anything with intercepted touch events if 
            // we are not scrollable
            if (!IsScrollable)
                return false;

            return base.OnInterceptTouchEvent(ev);
        }
    }
}

