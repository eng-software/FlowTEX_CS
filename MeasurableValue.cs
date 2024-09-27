using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TEX.FixedNumber;

namespace TEX.Unit
{

    public class MeasuredValue<UnitType> where UnitType : Enum
    {
        double _value;
        UnitType _unit;
        cResolution _resolution;

        public MeasuredValue(double value, UnitType unit, cResolution resolution)
        {
            _value = value;
            _unit = unit;
            _resolution = new cResolution(resolution);
        }

        public MeasuredValue() : this(0, (UnitType)Enum.GetValues(typeof(UnitType)).GetValue(0), new cResolution())
        {

        }

        public double Value { get => _value; set => _value = value; }
        public UnitType Unit { get => _unit; set => _unit = value; }
        public cResolution Resolution { get => _resolution; set => _resolution = value; }
        public string ValueString
        {
            get 
            {
                int dp = Resolution.DecimalPlaces;
                if (dp < 0)
                {
                    dp = 0;
                }
                //return cFixedNumber.fix(Value, Resolution.Resolution).ToString(String.Format("N{0}", dp));
                return String.Format("{0:F" + String.Format("{0:0}", dp) + "}", cFixedNumber.fix(Value, Resolution.Resolution));

            }
        }
    }

    public class MeasurableValue<UnitType> where UnitType : Enum, IComparable
    {
        UnitType _currentInputUnit;
        UnitType _currentOutputUnit;
        double _lastInputValue;
        IUnitResolutionMap<UnitType> _ResolutionMap;
        IUnitsConverter<UnitType> _Converter;
        cFixedNumber _Value;
                
        public MeasurableValue(MeasurableValue<UnitType> measurableValue)
        {
            if (measurableValue != null)
            {
                this._currentInputUnit = measurableValue._currentInputUnit;
                this._currentOutputUnit = measurableValue._currentOutputUnit;
                this._lastInputValue = measurableValue._lastInputValue;
                this._ResolutionMap = measurableValue._ResolutionMap;
                this._Converter = measurableValue._Converter;
                this._Value = new cFixedNumber(measurableValue._Value);
            }
        }

        public MeasurableValue(IUnitResolutionMap<UnitType> ResolutionMap, IUnitsConverter<UnitType> Converter, UnitType InputUnit, UnitType OutputUnit)
        {
            _currentInputUnit = InputUnit;
            _currentOutputUnit = OutputUnit;
            _ResolutionMap = ResolutionMap;
            _Converter = Converter;
            _Value = new cFixedNumber(_ResolutionMap.GetResolution(_currentOutputUnit));
        }

        public double Value
        {
            get => _Value.get();
            set
            {
                _lastInputValue = value;
                value = _Converter.Convert(value, _currentInputUnit, _currentOutputUnit);
                _Value.set(value);
            }
        }

        public UnitType Unit
        {
            get => _currentOutputUnit;
            set
            {
                if (_currentOutputUnit.CompareTo(value) != 0)
                {
                    _currentOutputUnit = value;
                    _Value.setResolution(_ResolutionMap.GetResolution(value));
                    _Value.set(_Converter.Convert(_lastInputValue, _currentInputUnit, _currentOutputUnit));
                }
            }
        }

        public UnitType InputUnit
        {
            get => _currentInputUnit;
            set
            {
                if (_currentInputUnit.CompareTo(value) != 0)
                {
                    _lastInputValue = _Converter.Convert(_lastInputValue, _currentInputUnit, value);
                    _currentInputUnit = value;
                    _Value.set(_lastInputValue);
                }
            }
        }

        public string UnitName
        {
            get
            {
                return _Converter.UnitName(_currentOutputUnit);
            }
        }

        public int DecimalPlaces
        {
            get
            {
                int res = _ResolutionMap.GetDecimalPlaces(_currentOutputUnit);
                if (res < 0)
                { res = 0; }
                return res;
            }
        }

        public string ValueString
        {            
            get => String.Format("{0:F" + String.Format("{0:0}", DecimalPlaces) + "}", Value);
        }

        public IUnitResolutionMap<UnitType> ResolutionMap
        {
            get => _ResolutionMap;
            set
            {
                _ResolutionMap = value;
                _Value.setResolution(_ResolutionMap.GetResolution(_currentOutputUnit));
            }
        }

        public IUnitsConverter<UnitType> UnitCoverter
        {
            get => _Converter;
            set
            {
                _Converter = value;
                _Value.set(_Converter.Convert(_lastInputValue, _currentInputUnit, _currentOutputUnit));
            }
        }

    }

}
