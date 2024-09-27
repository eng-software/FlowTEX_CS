using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.NetworkInformation;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using TEX.Unit;

namespace TEX.PressureUnit
{
    public enum ePressureUnit
    {
        ePA,
        eHPA,
        eKPA,
        eMPA,
        eBAR,
        eMBAR,
        eMMH2O,
        eINH2O,
        eMMHG,
        eINHG,
        ePSI,
        eKGF,
    }

    public class PressureUnits
    {
        public static readonly IDictionary<ePressureUnit, string> Names = new Dictionary<ePressureUnit, string>()
        {       
			{   ePressureUnit.ePA       ,   "Pa"         }, //Pa     
            {   ePressureUnit.eHPA      ,   "hPa"        }, //HPa
            {   ePressureUnit.eKPA      ,   "KPa"        }, //KPa
            {   ePressureUnit.eMPA      ,   "MPa"        }, //MPa
            {   ePressureUnit.eBAR      ,   "bar"        }, //Bar
            {   ePressureUnit.eMBAR     ,   "mbar"       }, //mBar	
            {   ePressureUnit.eMMH2O    ,   "mmH₂O"      }, //mmH2O
            {   ePressureUnit.eINH2O    ,   "inH₂O"      }, //inH2O	
            {   ePressureUnit.eMMHG     ,   "mmHg"       }, //mmHg
            {   ePressureUnit.eINHG     ,   "inHg"       }, //inHg
            {   ePressureUnit.ePSI      ,   "PSI"        }, //PSI
            {   ePressureUnit.eKGF      ,   "kgf/cm²"    }, //Kgf/cm2
        };
    }


    public class PressureUnitConverter : IUnitsConverter<ePressureUnit>
    {
        public static readonly IDictionary<ePressureUnit, IUnitConvertion<ePressureUnit>> ConvertionTable = new Dictionary<ePressureUnit, IUnitConvertion<ePressureUnit>>()
        {        
            //Base unit is PA
			{   ePressureUnit.ePA       ,   new UnitConvertion<ePressureUnit>(ePressureUnit.ePA    , () => PressureUnits.Names[ePressureUnit.ePA   ]    , value => value * 1.0                     , value => value / 1.0                      )     }, //Pa     
            {   ePressureUnit.eHPA      ,   new UnitConvertion<ePressureUnit>(ePressureUnit.eHPA   , () => PressureUnits.Names[ePressureUnit.eHPA  ]    , value => value * 0.01                    , value => value / 0.01                     )     }, //HPa
            {   ePressureUnit.eKPA      ,   new UnitConvertion<ePressureUnit>(ePressureUnit.eKPA   , () => PressureUnits.Names[ePressureUnit.eKPA  ]    , value => value * 0.001                   , value => value / 0.001                    )     }, //KPa
            {   ePressureUnit.eMPA      ,   new UnitConvertion<ePressureUnit>(ePressureUnit.eMPA   , () => PressureUnits.Names[ePressureUnit.eMPA  ]    , value => value * 0.000001                , value => value / 0.000001                 )     }, //MPa
            {   ePressureUnit.eBAR      ,   new UnitConvertion<ePressureUnit>(ePressureUnit.eBAR   , () => PressureUnits.Names[ePressureUnit.eBAR  ]    , value => value * 0.00001                 , value => value / 0.00001                  )     }, //Bar
            {   ePressureUnit.eMBAR     ,   new UnitConvertion<ePressureUnit>(ePressureUnit.eMBAR  , () => PressureUnits.Names[ePressureUnit.eMBAR ]    , value => value * 0.01                    , value => value / 0.01                     )     }, //mBar	
            {   ePressureUnit.eMMH2O    ,   new UnitConvertion<ePressureUnit>(ePressureUnit.eMMH2O , () => PressureUnits.Names[ePressureUnit.eMMH2O]    , value => value * 0.10197162129779283     , value => value / 0.10197162129779283      )     }, //mmH2O
            {   ePressureUnit.eINH2O    ,   new UnitConvertion<ePressureUnit>(ePressureUnit.eINH2O , () => PressureUnits.Names[ePressureUnit.eINH2O]    , value => value * 0.0040146307866177      , value => value / 0.0040146307866177       )     }, //inH2O	
            {   ePressureUnit.eMMHG     ,   new UnitConvertion<ePressureUnit>(ePressureUnit.eMMHG  , () => PressureUnits.Names[ePressureUnit.eMMHG ]    , value => value * 0.0075006156130264      , value => value / 0.0075006156130264       )     }, //mmHg
            {   ePressureUnit.eINHG     ,   new UnitConvertion<ePressureUnit>(ePressureUnit.eINHG  , () => PressureUnits.Names[ePressureUnit.eINHG ]    , value => value * 0.00029529983071445     , value => value / 0.00029529983071445      )     }, //inHg
            {   ePressureUnit.ePSI      ,   new UnitConvertion<ePressureUnit>(ePressureUnit.ePSI   , () => PressureUnits.Names[ePressureUnit.ePSI  ]    , value => value * 0.00014503773800722     , value => value / 0.00014503773800722      )     }, //PSI
            {   ePressureUnit.eKGF      ,   new UnitConvertion<ePressureUnit>(ePressureUnit.eKGF   , () => PressureUnits.Names[ePressureUnit.eKGF  ]    , value => value * 0.000010197162129779282 , value => value / 0.000010197162129779282  )     }, //Kgf/cm2
        };

        static public double convertPaToUnit(double valueInPa, ePressureUnit desiredUnit)
        {
            return ConvertionTable[desiredUnit].ConvertFromBase(valueInPa);
        }

        static public double convertUnitToPa(double valueInUnit, ePressureUnit valueUnit)
        {
            return ConvertionTable[valueUnit].ConvertToBase(valueInUnit);
        }

        static public double convert(double sourceValue, ePressureUnit sourceUnit, ePressureUnit desiredUnit)
        {
            return convertPaToUnit(convertUnitToPa(sourceValue, sourceUnit), desiredUnit);
        }

        static public string getUnitString(ePressureUnit unit)
        {
            return ConvertionTable[unit].Name;
        }

        public double ConvertFromBase(double valueInBaseUnit, ePressureUnit desiredUnit)
        {
            return convertPaToUnit(valueInBaseUnit,  desiredUnit);
        }

        public double ConvertToBase(double valueInUnit, ePressureUnit valueUnit)
        {
            return convertUnitToPa(valueInUnit, valueUnit);
        }

        public double Convert(double sourceValue, ePressureUnit sourceUnit, ePressureUnit desiredUnit)
        {
            return convert(sourceValue, sourceUnit, desiredUnit);
        }

        public string UnitName(ePressureUnit unit)
        {
            return ConvertionTable[unit].Name;
        }
    }

    public class PressureUnitResolutionMap : IUnitResolutionMap<ePressureUnit>
    {
        public static readonly IDictionary<ePressureUnit, UnitResolution<ePressureUnit>> PressureUnits = new Dictionary<ePressureUnit, UnitResolution<ePressureUnit>>()
        {        
            //Base unit is PA
			{   ePressureUnit.ePA       ,   new UnitResolution<ePressureUnit>(ePressureUnit.ePA, -1,  eResolutionIncrementSize.e1 )     }, //Pa     
            {   ePressureUnit.eHPA      ,   new UnitResolution<ePressureUnit>(ePressureUnit.ePA,  1,  eResolutionIncrementSize.e1 )     }, //HPa
            {   ePressureUnit.eKPA      ,   new UnitResolution<ePressureUnit>(ePressureUnit.ePA,  2,  eResolutionIncrementSize.e1 )     }, //KPa
            {   ePressureUnit.eMPA      ,   new UnitResolution<ePressureUnit>(ePressureUnit.ePA,  4,  eResolutionIncrementSize.e1 )     }, //MPa
            {   ePressureUnit.eBAR      ,   new UnitResolution<ePressureUnit>(ePressureUnit.ePA,  4,  eResolutionIncrementSize.e1 )     }, //Bar
            {   ePressureUnit.eMBAR     ,   new UnitResolution<ePressureUnit>(ePressureUnit.ePA,  1,  eResolutionIncrementSize.e1 )     }, //mBar	
            {   ePressureUnit.eMMH2O    ,   new UnitResolution<ePressureUnit>(ePressureUnit.ePA,  0,  eResolutionIncrementSize.e1 )     }, //mmH2O
            {   ePressureUnit.eINH2O    ,   new UnitResolution<ePressureUnit>(ePressureUnit.ePA,  2,  eResolutionIncrementSize.e1 )     }, //inH2O	
            {   ePressureUnit.eMMHG     ,   new UnitResolution<ePressureUnit>(ePressureUnit.ePA,  2,  eResolutionIncrementSize.e1 )     }, //mmHg
            {   ePressureUnit.eINHG     ,   new UnitResolution<ePressureUnit>(ePressureUnit.ePA,  3,  eResolutionIncrementSize.e1 )     }, //inHg
            {   ePressureUnit.ePSI      ,   new UnitResolution<ePressureUnit>(ePressureUnit.ePA,  3,  eResolutionIncrementSize.e1 )     }, //PSI
            {   ePressureUnit.eKGF      ,   new UnitResolution<ePressureUnit>(ePressureUnit.ePA,  4,  eResolutionIncrementSize.e1 )     }, //Kgf/cm2
        };

        public int GetDecimalPlaces(ePressureUnit unit)
        {
            return PressureUnits[unit].DecimalPlaces;
        }

        public double GetResolution(ePressureUnit unit)
        {
            return PressureUnits[unit].Resolution;
        }

        public IUnitResolution<ePressureUnit> GetUnitResolution(ePressureUnit unit)
        {
            return PressureUnits[unit];
        }

        public void SetRange(double range)
        {

        }
    }

    public class cPressure : MeasurableValue<ePressureUnit>
    {
        public cPressure(cPressure pressure) : base(pressure)
        {

        }

        public cPressure() : base(new PressureUnitResolutionMap(), new PressureUnitConverter(), ePressureUnit.eKPA, ePressureUnit.eKPA )
        {

        }

        public cPressure(IUnitResolutionMap<ePressureUnit> UnitResolutionMap) : base(UnitResolutionMap, new PressureUnitConverter(), ePressureUnit.eKPA, ePressureUnit.eKPA)
        {

        }

        public cPressure(IUnitsConverter<ePressureUnit> UnitConverter) : base(new PressureUnitResolutionMap(), UnitConverter, ePressureUnit.eKPA, ePressureUnit.eKPA)
        {

        }

        public cPressure(IUnitResolutionMap<ePressureUnit> UnitResolutionMap, IUnitsConverter<ePressureUnit> UnitConverter) : base(UnitResolutionMap, UnitConverter, ePressureUnit.eKPA, ePressureUnit.eKPA)
        {

        }

        public cPressure(IUnitResolutionMap<ePressureUnit> ResolutionMap, IUnitsConverter<ePressureUnit> UnitConverter, ePressureUnit InputUnit, ePressureUnit OutputUnit) : base(ResolutionMap, UnitConverter, InputUnit, OutputUnit)
        {

        }
    }

    public class cMeasuredPressure : MeasuredValue<ePressureUnit>
    {
        public cMeasuredPressure() : base()
        {

        }

        public cMeasuredPressure(double value, ePressureUnit unit, cResolution resolution) : base(value, unit, resolution)
        {

        }

        public void Set(cPressure Pressure)
        {
            if (Pressure != null) 
            {
                Unit = Pressure.Unit;
                Value = Pressure.Value;
                Resolution.Set(Pressure.ResolutionMap.GetUnitResolution(Unit));
            }
        }

        public void Set(cMeasuredPressure Pressure)
        {
            if (Pressure != null)
            {
                Unit = Pressure.Unit;
                Value = Pressure.Value;
                Resolution = Pressure.Resolution;
            }
        }

    }
}