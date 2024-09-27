using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TEX.FixedNumber
{
    public class cFixedNumber
    {
        double resolution;
        double lastOutput;
        double hysteresis;
        double lastInput;

        public cFixedNumber(cFixedNumber fixedNumber)
        {
            this.resolution = fixedNumber.resolution;
            this.lastOutput = fixedNumber.lastOutput;
            this.hysteresis = fixedNumber.hysteresis;
            this.lastInput  = fixedNumber.lastInput;
        }

        public cFixedNumber()
        {
            this.resolution = 0.001;
            this.lastOutput = 0;
            this.hysteresis = 0.25;
        }

        public cFixedNumber(double resolution, float hysteresis)
        {
            this.resolution = resolution;
            this.hysteresis = hysteresis;
            this.lastOutput = 0;
        }

        public cFixedNumber(double resolution)
        {
            this.resolution = resolution;
            this.lastOutput = 0;
            this.hysteresis = 0.25;
        }

        public void setResolution(double value)
        {
            this.resolution = value;
            set(lastInput);
        }

        public void setHysteresis(double value)
        {
            if ((value >= 0) && (value < 0.5))
            {
                hysteresis = value;
                set(lastInput);
            }
        }

        public double set(double value)
        {
            Int64 q;
            bool negative = value < 0;

            if (negative)
            {
                if (value < lastOutput)
                {
                    q = (Int64)(double)((value / resolution) - (0.5 - hysteresis));
                }
                else
                {
                    q = (Int64)(double)((value / resolution) - (0.5 + hysteresis));
                }
            }
            else
            {
                if (value > lastOutput)
                {
                    q = (Int64)(double)((value / resolution) + (0.5 - hysteresis));
                }
                else
                {
                    q = (Int64)(double)((value / resolution) + (0.5 + hysteresis));
                }
            }

            lastOutput = resolution * q;

            return get();
        }

        public double get()
        {
            return lastOutput;
        }

        static public double fix(double value, double resolution)
        {
            Int64 q;            
            q = (Int64)(double)(value / resolution);
            return resolution * q;
        }
    }
}
