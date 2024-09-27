using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TEX.Unit
{
    
    public enum eResolutionIncrementSize
    {
        e1,
        e2,
        e5    
    };

    public interface IResolution
    {
        eResolutionIncrementSize IncrementSize { get; }
        int DecimalPlaces { get; }
        double Resolution { get; }
    }

    static public class ResolutionIncrements
    {
        public static readonly IDictionary<eResolutionIncrementSize, double> Sizes = new Dictionary<eResolutionIncrementSize, double>()
        {        
		    {   eResolutionIncrementSize.e1      ,   1.0  },    
            {   eResolutionIncrementSize.e2      ,   2.0  },
            {   eResolutionIncrementSize.e5      ,   5.0  },
        };
    }

    public class cResolution : IResolution
    {
        public eResolutionIncrementSize IncrementSize { get; set; }
        public int DecimalPlaces { get; set; }

        public double Resolution
        {
            get
            {
                return GetResolution(IncrementSize, DecimalPlaces);
            }
        }

        public cResolution()
        {
            IncrementSize = eResolutionIncrementSize.e1;
            DecimalPlaces = 3;
        }

        public cResolution(eResolutionIncrementSize incrementSize, int decimalPlaces)
        {
            IncrementSize = incrementSize;
            DecimalPlaces = decimalPlaces;
        }

        public cResolution(IResolution BasicResolution)
        {
            Set(BasicResolution);
        }

        public void Set(IResolution BasicResolution)
        {
            IncrementSize = BasicResolution.IncrementSize;
            DecimalPlaces = BasicResolution.DecimalPlaces;
        }

        public static double GetResolution(eResolutionIncrementSize incrementSize, int decimalPlaces)
        {
            return ResolutionIncrements.Sizes[incrementSize] / Math.Pow(10, decimalPlaces);
        }
    }

    public interface IUnitResolution<UnitType> : IResolution where UnitType : Enum
    {
        UnitType Unit { get; }
    }

    public class UnitResolution<UnitType> : IUnitResolution<UnitType> where UnitType : Enum
    {
        public UnitType Unit { get; set; }

        public eResolutionIncrementSize IncrementSize { get; set; }

        public int DecimalPlaces { get; set; }

        public double Resolution 
        {
            get => cResolution.GetResolution(IncrementSize, DecimalPlaces);
        }

        public UnitResolution() : this( (UnitType)Enum.GetValues(typeof(UnitType)).GetValue(0), 3, eResolutionIncrementSize.e1)
        {

        }
        
        public UnitResolution(UnitType unit, int decimalPlaces, eResolutionIncrementSize incrementSize)
        {
            Unit = unit;
            IncrementSize = incrementSize;
            DecimalPlaces = decimalPlaces;
        }
    }

    public interface IUnitResolutionMap<UnitType> where UnitType : Enum
    {
        double GetResolution(UnitType unit);
        int GetDecimalPlaces(UnitType unit);
        IUnitResolution<UnitType> GetUnitResolution(UnitType unit);
        void SetRange(double range);
    }
}
