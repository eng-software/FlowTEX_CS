using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.NetworkInformation;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using TEX.PressureUnit;
using TEX.Unit;

namespace TEX.FlowUnit
{
    public enum eFlowUnit
    {
        eMM3_S      ,
        eCCM        ,
        eCCH        ,
        eCM3_S      ,
        eCM3_MIN    ,
        eCM3_H      ,
        eML_S       ,
        eML_MIN     ,
        eML_H       ,
        eLPM        ,
        eL_MIN      ,
        eL_H        ,
        eM3_S       ,
        eM3_MIN     ,
        eMBAR_LS    ,
    }

    public class FlowUnits
    {
        public static readonly IDictionary<eFlowUnit, string> Names = new Dictionary<eFlowUnit, string>()
        {       
			{   eFlowUnit.eMM3_S      ,   "Smm³/s"       }, 
            {   eFlowUnit.eCCM        ,   "Sccm"         }, 
            {   eFlowUnit.eCCH        ,   "Scch"         }, 
            {   eFlowUnit.eCM3_S      ,   "Scm³/s"       }, 
            {   eFlowUnit.eCM3_MIN    ,   "Scm³/min"     }, 
            {   eFlowUnit.eCM3_H      ,   "Scm³/h"       }, 
            {   eFlowUnit.eML_S       ,   "SmL/s"        }, 
            {   eFlowUnit.eML_MIN     ,   "SmL/min"      }, 
            {   eFlowUnit.eML_H       ,   "SmL/h"        }, 
            {   eFlowUnit.eLPM        ,   "Slpm"         }, 
            {   eFlowUnit.eL_MIN      ,   "SL/min"       }, 
            {   eFlowUnit.eL_H        ,   "SL/h"         }, 
            {   eFlowUnit.eM3_S       ,   "Sm³/s"        }, 
            {   eFlowUnit.eM3_MIN     ,   "Sm³/min"      }, 
            {   eFlowUnit.eMBAR_LS    ,   "SmbarL/s"     }, 
        };
    }

    public class FlowUnitConverter : IUnitsConverter<eFlowUnit>
    {
        public static readonly IDictionary<eFlowUnit, IUnitConvertion<eFlowUnit>> ConvertionTable = new Dictionary<eFlowUnit, IUnitConvertion<eFlowUnit>>()
        {        
            //Base unit is PA
			{   eFlowUnit.eMM3_S      ,   new UnitConvertion<eFlowUnit>(eFlowUnit.eMM3_S   , () => FlowUnits.Names[eFlowUnit.eMM3_S  ]    , value => value * 16.6666667              , value => value / 16.6666667               )     }, 
            {   eFlowUnit.eCCM        ,   new UnitConvertion<eFlowUnit>(eFlowUnit.eCCM     , () => FlowUnits.Names[eFlowUnit.eCCM    ]    , value => value * 1.0                     , value => value / 1.0                      )     }, 
            {   eFlowUnit.eCCH        ,   new UnitConvertion<eFlowUnit>(eFlowUnit.eCCH     , () => FlowUnits.Names[eFlowUnit.eCCH    ]    , value => value * 60.0                    , value => value / 60.0                     )     }, 
            {   eFlowUnit.eCM3_S      ,   new UnitConvertion<eFlowUnit>(eFlowUnit.eCM3_S   , () => FlowUnits.Names[eFlowUnit.eCM3_S  ]    , value => value * 0.016666667             , value => value / 0.016666667              )     }, 
            {   eFlowUnit.eCM3_MIN    ,   new UnitConvertion<eFlowUnit>(eFlowUnit.eCM3_MIN , () => FlowUnits.Names[eFlowUnit.eCM3_MIN]    , value => value * 1.0                     , value => value / 1.0                      )     }, 
            {   eFlowUnit.eCM3_H      ,   new UnitConvertion<eFlowUnit>(eFlowUnit.eCM3_H   , () => FlowUnits.Names[eFlowUnit.eCM3_H  ]    , value => value * 60.0                    , value => value / 60.0                     )     }, 
            {   eFlowUnit.eML_S       ,   new UnitConvertion<eFlowUnit>(eFlowUnit.eML_S    , () => FlowUnits.Names[eFlowUnit.eML_S   ]    , value => value * 0.016666667             , value => value / 0.016666667              )     }, 
            {   eFlowUnit.eML_MIN     ,   new UnitConvertion<eFlowUnit>(eFlowUnit.eML_MIN  , () => FlowUnits.Names[eFlowUnit.eML_MIN ]    , value => value * 1.0                     , value => value / 1.0                      )     }, 
            {   eFlowUnit.eML_H       ,   new UnitConvertion<eFlowUnit>(eFlowUnit.eML_H    , () => FlowUnits.Names[eFlowUnit.eML_H   ]    , value => value * 60.0                    , value => value / 60.0                     )     }, 
            {   eFlowUnit.eLPM        ,   new UnitConvertion<eFlowUnit>(eFlowUnit.eLPM     , () => FlowUnits.Names[eFlowUnit.eLPM    ]    , value => value * 0.001                   , value => value / 0.001                    )     }, 
            {   eFlowUnit.eL_MIN      ,   new UnitConvertion<eFlowUnit>(eFlowUnit.eL_MIN   , () => FlowUnits.Names[eFlowUnit.eL_MIN  ]    , value => value * 0.001                   , value => value / 0.001                    )     }, 
            {   eFlowUnit.eL_H        ,   new UnitConvertion<eFlowUnit>(eFlowUnit.eL_H     , () => FlowUnits.Names[eFlowUnit.eL_H    ]    , value => value * 0.06                    , value => value / 0.06                     )     }, 
            {   eFlowUnit.eM3_S       ,   new UnitConvertion<eFlowUnit>(eFlowUnit.eM3_S    , () => FlowUnits.Names[eFlowUnit.eM3_S   ]    , value => value * 1.6666667e-8            , value => value / 1.6666667e-8             )     }, 
            {   eFlowUnit.eM3_MIN     ,   new UnitConvertion<eFlowUnit>(eFlowUnit.eM3_MIN  , () => FlowUnits.Names[eFlowUnit.eM3_MIN ]    , value => value * 1.0e-6                  , value => value / 1.0e-6                   )     }, 
            {   eFlowUnit.eMBAR_LS    ,   new UnitConvertion<eFlowUnit>(eFlowUnit.eMBAR_LS , () => FlowUnits.Names[eFlowUnit.eMBAR_LS]    , value => value * 0.016666667             , value => value / 0.016666667              )     }, 
        };

        static public double convertPaToUnit(double valueInPa, eFlowUnit desiredUnit)
        {
            return ConvertionTable[desiredUnit].ConvertFromBase(valueInPa);
        }

        static public double convertUnitToPa(double valueInUnit, eFlowUnit valueUnit)
        {
            return ConvertionTable[valueUnit].ConvertToBase(valueInUnit);
        }

        static public double convert(double sourceValue, eFlowUnit sourceUnit, eFlowUnit desiredUnit)
        {
            return convertPaToUnit(convertUnitToPa(sourceValue, sourceUnit), desiredUnit);
        }

        static public string getUnitString(eFlowUnit unit)
        {
            return ConvertionTable[unit].Name;
        }

        public double ConvertFromBase(double valueInBaseUnit, eFlowUnit desiredUnit)
        {
            return convertPaToUnit(valueInBaseUnit,  desiredUnit);
        }

        public double ConvertToBase(double valueInUnit, eFlowUnit valueUnit)
        {
            return convertUnitToPa(valueInUnit, valueUnit);
        }

        public double Convert(double sourceValue, eFlowUnit sourceUnit, eFlowUnit desiredUnit)
        {
            return convert(sourceValue, sourceUnit, desiredUnit);
        }

        public string UnitName(eFlowUnit unit)
        {
            return ConvertionTable[unit].Name;
        }
    }

    public class FlowUnitResolutionMap : IUnitResolutionMap<eFlowUnit>
    {
        int rangeIdx = 0;

        struct sFlowResolution
        {
            public double range;
            public IDictionary<eFlowUnit, UnitResolution<eFlowUnit>> resolutions;
        };

        List<sFlowResolution> FlowUnitsResolution = new List<sFlowResolution>()
        {
            new sFlowResolution()
            {
                range = 0,  //10 e 20ccm
                resolutions = new Dictionary<eFlowUnit, UnitResolution<eFlowUnit>>()
                {
                    {   eFlowUnit.eMM3_S      ,   new UnitResolution<eFlowUnit>(eFlowUnit.eCCM,  5,  eResolutionIncrementSize.e2 )     },
                    {   eFlowUnit.eCCM        ,   new UnitResolution<eFlowUnit>(eFlowUnit.eCCM,  3,  eResolutionIncrementSize.e1 )     },
                    {   eFlowUnit.eCCH        ,   new UnitResolution<eFlowUnit>(eFlowUnit.eCCM,  2,  eResolutionIncrementSize.e1 )     },
                    {   eFlowUnit.eCM3_S      ,   new UnitResolution<eFlowUnit>(eFlowUnit.eCCM,  5,  eResolutionIncrementSize.e2 )     },
                    {   eFlowUnit.eCM3_MIN    ,   new UnitResolution<eFlowUnit>(eFlowUnit.eCCM,  3,  eResolutionIncrementSize.e1 )     },
                    {   eFlowUnit.eCM3_H      ,   new UnitResolution<eFlowUnit>(eFlowUnit.eCCM,  2,  eResolutionIncrementSize.e1 )     },
                    {   eFlowUnit.eML_S       ,   new UnitResolution<eFlowUnit>(eFlowUnit.eCCM,  5,  eResolutionIncrementSize.e2 )     },
                    {   eFlowUnit.eML_MIN     ,   new UnitResolution<eFlowUnit>(eFlowUnit.eCCM,  3,  eResolutionIncrementSize.e1 )     },
                    {   eFlowUnit.eML_H       ,   new UnitResolution<eFlowUnit>(eFlowUnit.eCCM,  2,  eResolutionIncrementSize.e1 )     },
                    {   eFlowUnit.eLPM        ,   new UnitResolution<eFlowUnit>(eFlowUnit.eCCM,  4,  eResolutionIncrementSize.e1 )     },
                    {   eFlowUnit.eL_MIN      ,   new UnitResolution<eFlowUnit>(eFlowUnit.eCCM,  4,  eResolutionIncrementSize.e1 )     },
                    {   eFlowUnit.eL_H        ,   new UnitResolution<eFlowUnit>(eFlowUnit.eCCM,  5,  eResolutionIncrementSize.e1 )     },
                    {   eFlowUnit.eM3_S       ,   new UnitResolution<eFlowUnit>(eFlowUnit.eCCM,  5,  eResolutionIncrementSize.e2 )     },
                    {   eFlowUnit.eM3_MIN     ,   new UnitResolution<eFlowUnit>(eFlowUnit.eCCM,  5,  eResolutionIncrementSize.e1 )     },
                    {   eFlowUnit.eMBAR_LS    ,   new UnitResolution<eFlowUnit>(eFlowUnit.eCCM,  5,  eResolutionIncrementSize.e2 )     },
                }
            },

            new sFlowResolution()
            {
                range = 50,  //50 a 500ccm
                resolutions = new Dictionary<eFlowUnit, UnitResolution<eFlowUnit>>()
                {     
                    
		            {   eFlowUnit.eMM3_S      ,   new UnitResolution<eFlowUnit>(eFlowUnit.eCCM,  4,  eResolutionIncrementSize.e2 )     },
                    {   eFlowUnit.eCCM        ,   new UnitResolution<eFlowUnit>(eFlowUnit.eCCM,  2,  eResolutionIncrementSize.e1 )     },
                    {   eFlowUnit.eCCH        ,   new UnitResolution<eFlowUnit>(eFlowUnit.eCCM,  1,  eResolutionIncrementSize.e1 )     },
                    {   eFlowUnit.eCM3_S      ,   new UnitResolution<eFlowUnit>(eFlowUnit.eCCM,  4,  eResolutionIncrementSize.e2 )     },
                    {   eFlowUnit.eCM3_MIN    ,   new UnitResolution<eFlowUnit>(eFlowUnit.eCCM,  2,  eResolutionIncrementSize.e1 )     },
                    {   eFlowUnit.eCM3_H      ,   new UnitResolution<eFlowUnit>(eFlowUnit.eCCM,  1,  eResolutionIncrementSize.e1 )     },
                    {   eFlowUnit.eML_S       ,   new UnitResolution<eFlowUnit>(eFlowUnit.eCCM,  4,  eResolutionIncrementSize.e2 )     },
                    {   eFlowUnit.eML_MIN     ,   new UnitResolution<eFlowUnit>(eFlowUnit.eCCM,  2,  eResolutionIncrementSize.e1 )     },
                    {   eFlowUnit.eML_H       ,   new UnitResolution<eFlowUnit>(eFlowUnit.eCCM,  1,  eResolutionIncrementSize.e1 )     },
                    {   eFlowUnit.eLPM        ,   new UnitResolution<eFlowUnit>(eFlowUnit.eCCM,  3,  eResolutionIncrementSize.e1 )     },
                    {   eFlowUnit.eL_MIN      ,   new UnitResolution<eFlowUnit>(eFlowUnit.eCCM,  4,  eResolutionIncrementSize.e1 )     },
                    {   eFlowUnit.eL_H        ,   new UnitResolution<eFlowUnit>(eFlowUnit.eCCM,  4,  eResolutionIncrementSize.e1 )     },
                    {   eFlowUnit.eM3_S       ,   new UnitResolution<eFlowUnit>(eFlowUnit.eCCM,  4,  eResolutionIncrementSize.e2 )     },
                    {   eFlowUnit.eM3_MIN     ,   new UnitResolution<eFlowUnit>(eFlowUnit.eCCM,  4,  eResolutionIncrementSize.e1 )     },
                    {   eFlowUnit.eMBAR_LS    ,   new UnitResolution<eFlowUnit>(eFlowUnit.eCCM,  4,  eResolutionIncrementSize.e2 )     },
                }
            },

            new sFlowResolution()
            {
                range = 10000,  //10,000 a 50,000ccm
                resolutions = new Dictionary<eFlowUnit, UnitResolution<eFlowUnit>>()
                {
                    {   eFlowUnit.eMM3_S      ,   new UnitResolution<eFlowUnit>(eFlowUnit.eCCM,  2,  eResolutionIncrementSize.e2 )     },
                    {   eFlowUnit.eCCM        ,   new UnitResolution<eFlowUnit>(eFlowUnit.eCCM,  0,  eResolutionIncrementSize.e1 )     },
                    {   eFlowUnit.eCCH        ,   new UnitResolution<eFlowUnit>(eFlowUnit.eCCM, -1,  eResolutionIncrementSize.e1 )     },
                    {   eFlowUnit.eCM3_S      ,   new UnitResolution<eFlowUnit>(eFlowUnit.eCCM,  2,  eResolutionIncrementSize.e2 )     },
                    {   eFlowUnit.eCM3_MIN    ,   new UnitResolution<eFlowUnit>(eFlowUnit.eCCM,  0,  eResolutionIncrementSize.e1 )     },
                    {   eFlowUnit.eCM3_H      ,   new UnitResolution<eFlowUnit>(eFlowUnit.eCCM, -1,  eResolutionIncrementSize.e1 )     },
                    {   eFlowUnit.eML_S       ,   new UnitResolution<eFlowUnit>(eFlowUnit.eCCM,  2,  eResolutionIncrementSize.e2 )     },
                    {   eFlowUnit.eML_MIN     ,   new UnitResolution<eFlowUnit>(eFlowUnit.eCCM,  0,  eResolutionIncrementSize.e1 )     },
                    {   eFlowUnit.eML_H       ,   new UnitResolution<eFlowUnit>(eFlowUnit.eCCM, -1,  eResolutionIncrementSize.e1 )     },
                    {   eFlowUnit.eLPM        ,   new UnitResolution<eFlowUnit>(eFlowUnit.eCCM,  1,  eResolutionIncrementSize.e1 )     },
                    {   eFlowUnit.eL_MIN      ,   new UnitResolution<eFlowUnit>(eFlowUnit.eCCM,  2,  eResolutionIncrementSize.e1 )     },
                    {   eFlowUnit.eL_H        ,   new UnitResolution<eFlowUnit>(eFlowUnit.eCCM,  2,  eResolutionIncrementSize.e1 )     },
                    {   eFlowUnit.eM3_S       ,   new UnitResolution<eFlowUnit>(eFlowUnit.eCCM,  2,  eResolutionIncrementSize.e2 )     },
                    {   eFlowUnit.eM3_MIN     ,   new UnitResolution<eFlowUnit>(eFlowUnit.eCCM,  2,  eResolutionIncrementSize.e1 )     },
                    {   eFlowUnit.eMBAR_LS    ,   new UnitResolution<eFlowUnit>(eFlowUnit.eCCM,  2,  eResolutionIncrementSize.e2 )     },
                }
            },

            new sFlowResolution()
            {
                range = 100000,  //100000  a 200000
                resolutions = new Dictionary<eFlowUnit, UnitResolution<eFlowUnit>>()
                {
                    {   eFlowUnit.eMM3_S      ,   new UnitResolution<eFlowUnit>(eFlowUnit.eCCM,  1,  eResolutionIncrementSize.e2 )     },
                    {   eFlowUnit.eCCM        ,   new UnitResolution<eFlowUnit>(eFlowUnit.eCCM, -1,  eResolutionIncrementSize.e1 )     },
                    {   eFlowUnit.eCCH        ,   new UnitResolution<eFlowUnit>(eFlowUnit.eCCM, -2,  eResolutionIncrementSize.e1 )     },
                    {   eFlowUnit.eCM3_S      ,   new UnitResolution<eFlowUnit>(eFlowUnit.eCCM,  1,  eResolutionIncrementSize.e2 )     },
                    {   eFlowUnit.eCM3_MIN    ,   new UnitResolution<eFlowUnit>(eFlowUnit.eCCM, -1,  eResolutionIncrementSize.e1 )     },
                    {   eFlowUnit.eCM3_H      ,   new UnitResolution<eFlowUnit>(eFlowUnit.eCCM, -2,  eResolutionIncrementSize.e1 )     },
                    {   eFlowUnit.eML_S       ,   new UnitResolution<eFlowUnit>(eFlowUnit.eCCM,  1,  eResolutionIncrementSize.e2 )     },
                    {   eFlowUnit.eML_MIN     ,   new UnitResolution<eFlowUnit>(eFlowUnit.eCCM, -1,  eResolutionIncrementSize.e1 )     },
                    {   eFlowUnit.eML_H       ,   new UnitResolution<eFlowUnit>(eFlowUnit.eCCM, -2,  eResolutionIncrementSize.e1 )     },
                    {   eFlowUnit.eLPM        ,   new UnitResolution<eFlowUnit>(eFlowUnit.eCCM,  0,  eResolutionIncrementSize.e1 )     },
                    {   eFlowUnit.eL_MIN      ,   new UnitResolution<eFlowUnit>(eFlowUnit.eCCM,  1,  eResolutionIncrementSize.e1 )     },
                    {   eFlowUnit.eL_H        ,   new UnitResolution<eFlowUnit>(eFlowUnit.eCCM,  1,  eResolutionIncrementSize.e1 )     },
                    {   eFlowUnit.eM3_S       ,   new UnitResolution<eFlowUnit>(eFlowUnit.eCCM,  1,  eResolutionIncrementSize.e2 )     },
                    {   eFlowUnit.eM3_MIN     ,   new UnitResolution<eFlowUnit>(eFlowUnit.eCCM,  1,  eResolutionIncrementSize.e1 )     },
                    {   eFlowUnit.eMBAR_LS    ,   new UnitResolution<eFlowUnit>(eFlowUnit.eCCM,  1,  eResolutionIncrementSize.e2 )     },
                }
            },
        };

        public void SetRange(double range)
        {
            rangeIdx = FlowUnitsResolution.Count - 1;
            for (int i = 1; i < FlowUnitsResolution.Count; i++)
            {
                if (FlowUnitsResolution[i].range > range)
                {
                    rangeIdx = i - 1;
                    break;
                }
            }
        }

        public int GetDecimalPlaces(eFlowUnit unit)
        {
            return FlowUnitsResolution[rangeIdx].resolutions[unit].DecimalPlaces;
        }

        public double GetResolution(eFlowUnit unit)
        {
            return FlowUnitsResolution[rangeIdx].resolutions[unit].Resolution;
        }

        public IUnitResolution<eFlowUnit> GetUnitResolution(eFlowUnit unit)
        {
            return FlowUnitsResolution[rangeIdx].resolutions[unit];
        }
    }

    public class cFlow : MeasurableValue<eFlowUnit>
    {
        public cFlow(cFlow flow) : base(flow)
        {

        }

        public cFlow() : base(new FlowUnitResolutionMap(), new FlowUnitConverter(), eFlowUnit.eCCM, eFlowUnit.eCCM )
        {

        }

        public cFlow(IUnitResolutionMap<eFlowUnit> UnitResolutionMap) : base(UnitResolutionMap, new FlowUnitConverter(), eFlowUnit.eCCM, eFlowUnit.eCCM)
        {

        }

        public cFlow(IUnitsConverter<eFlowUnit> UnitConverter) : base(new FlowUnitResolutionMap(), UnitConverter, eFlowUnit.eCCM, eFlowUnit.eCCM)
        {

        }

        public cFlow(IUnitResolutionMap<eFlowUnit> UnitResolutionMap, IUnitsConverter<eFlowUnit> UnitConverter) : base(UnitResolutionMap, UnitConverter, eFlowUnit.eCCM, eFlowUnit.eCCM)
        {

        }

        public cFlow(IUnitResolutionMap<eFlowUnit> ResolutionMap, IUnitsConverter<eFlowUnit> UnitConverter, eFlowUnit InputUnit, eFlowUnit OutputUnit) : base(ResolutionMap, UnitConverter, InputUnit, OutputUnit)
        {

        }

        public void SetRange(double range)
        {
            FlowUnitResolutionMap  rm = new FlowUnitResolutionMap();
            rm.SetRange(range);
            this.ResolutionMap = rm;
        }

    }

    public class cMeasuredFlow : MeasuredValue<eFlowUnit>
    {
        public cMeasuredFlow() : base()
        {

        }

        public cMeasuredFlow(double value, eFlowUnit unit, cResolution resolution) : base(value, unit, resolution)
        {

        }

        public void Set(cFlow Flow)
        {
            if (Flow != null) 
            {
                Unit = Flow.Unit;
                Value = Flow.Value;
                Resolution.Set(Flow.ResolutionMap.GetUnitResolution(Unit));
            }
        }

        public void Set(cMeasuredFlow Flow)
        {
            if (Flow != null)
            {
                Unit = Flow.Unit;
                Value = Flow.Value;
                Resolution = Flow.Resolution;
            }
        }
    }
}