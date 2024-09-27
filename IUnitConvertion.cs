using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TEX.Unit
{
    public interface IUnitConvertion<UnitType>
    {
        double ConvertFromBase(double value);
        double ConvertToBase(double value);
        string GetUnitName();
        UnitType GetUnit();
        UnitType Unit { get; }
        string Name { get; }
    }

    public class UnitConvertion<UnitType> : IUnitConvertion<UnitType>
    {
        public delegate double FConvertFromBase(double value);
        public delegate double FConvertToBase(double value);
        public delegate UnitType FGetUnit();
        public delegate string FGetUnitName();

        UnitType _unit;
        protected FConvertFromBase DlgConvertFromBase { get; set; }
        protected FConvertToBase DlgConvertToBase { get; set; }
        protected FGetUnit DlgUnit { get; set; }
        protected FGetUnitName DlgUnitName { get; set; }

        public UnitType Unit { get => GetUnit(); }
        public string Name { get => GetUnitName(); }

        public UnitConvertion(UnitType unit, FGetUnitName unitName, FConvertFromBase convertFromBase, FConvertToBase convertToBase)
        {
            DlgConvertFromBase = convertFromBase;
            DlgConvertToBase = convertToBase;
            DlgUnitName = unitName;
            DlgUnit = () => _unit;
        }

        public double ConvertFromBase(double value)
        {
            return DlgConvertFromBase.Invoke(value);
        }

        public double ConvertToBase(double value)
        {
            return DlgConvertToBase.Invoke(value);
        }

        public string GetUnitName()
        {
            return DlgUnitName.Invoke();
        }

        public UnitType GetUnit()
        {
            return DlgUnit.Invoke();
        }
    }

    public interface IUnitsConverter<UnitType>
    {
        double ConvertFromBase(double valueInBaseUnit, UnitType desiredUnit);
        double ConvertToBase(double valueInUnit, UnitType valueUnit);
        double Convert(double sourceValue, UnitType sourceUnit, UnitType desiredUnit);
        string UnitName(UnitType unit);
    }
}
