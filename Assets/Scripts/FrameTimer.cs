using System;

namespace Miren
{
    [Serializable]
    public struct FrameTimer
    {
        public float Duration;
        private float currentTime;

        public FrameTimer(float duration)
        {
            Duration = duration;
            currentTime = 0;
        }

        public bool Update(float delta)
        {
            currentTime += delta;
            if (currentTime > Duration)
            {
                currentTime = 0;
                return true;
            }
            else
            {
                return false;
            }
        }

        public void Reset()
        {
            currentTime = 0;
        }
    }
}