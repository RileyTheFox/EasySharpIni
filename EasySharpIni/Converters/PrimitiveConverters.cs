namespace EasySharpIni.Converters
{
    public class SByteConverter : Converter<sbyte>
    {
        public override string GetDefaultName()
        {
            return "SByte";
        }

        public override sbyte GetDefaultValue()
        {
            return 0;
        }

        public override bool Parse(string arg, out sbyte result)
        {
            return sbyte.TryParse(arg, out result);
        }
    }

    public class ByteConverter : Converter<byte>
    {
        public override string GetDefaultName()
        {
            return "Byte";
        }

        public override byte GetDefaultValue()
        {
            return 0;
        }

        public override bool Parse(string arg, out byte result)
        {
            return byte.TryParse(arg, out result);
        }
    }

    public class ShortConverter : Converter<short>
    {
        public override string GetDefaultName()
        {
            return "Short";
        }

        public override short GetDefaultValue()
        {
            return 0;
        }

        public override bool Parse(string arg, out short result)
        {
            return short.TryParse(arg, out result);
        }
    }

    public class UShortConverter : Converter<ushort>
    {
        public override string GetDefaultName()
        {
            return "UShort";
        }

        public override ushort GetDefaultValue()
        {
            return 0;
        }

        public override bool Parse(string arg, out ushort result)
        {
            return ushort.TryParse(arg, out result);
        }
    }

    public class IntConverter : Converter<int>
    {
        public override string GetDefaultName()
        {
            return "Int";
        }

        public override int GetDefaultValue()
        {
            return 0;
        }

        public override bool Parse(string arg, out int result)
        {
            return int.TryParse(arg, out result);
        }
    }

    public class UIntConverter : Converter<uint>
    {
        public override string GetDefaultName()
        {
            return "UInt";
        }

        public override uint GetDefaultValue()
        {
            return 0;
        }

        public override bool Parse(string arg, out uint result)
        {
            return uint.TryParse(arg, out result);
        }
    }

    public class LongConverter : Converter<long>
    {
        public override string GetDefaultName()
        {
            return "Long";
        }

        public override long GetDefaultValue()
        {
            return 0;
        }

        public override bool Parse(string arg, out long result)
        {
            return long.TryParse(arg, out result);
        }
    }

    public class ULongConverter : Converter<ulong>
    {
        public override string GetDefaultName()
        {
            return "ULong";
        }

        public override ulong GetDefaultValue()
        {
            return 0;
        }

        public override bool Parse(string arg, out ulong result)
        {
            return ulong.TryParse(arg, out result);
        }
    }

    public class FloatConverter : Converter<float>
    {
        public override string GetDefaultName()
        {
            return "Float";
        }

        public override float GetDefaultValue()
        {
            return 0f;
        }

        public override bool Parse(string arg, out float result)
        {
            return float.TryParse(arg, out result);
        }
    }

    public class DoubleConverter : Converter<double>
    {
        public override string GetDefaultName()
        {
            return "Double";
        }

        public override double GetDefaultValue()
        {
            return 0D;
        }

        public override bool Parse(string arg, out double result)
        {
            return double.TryParse(arg, out result);
        }
    }

    public class DecimalConverter : Converter<decimal>
    {
        public override string GetDefaultName()
        {
            return "Decimal";
        }

        public override decimal GetDefaultValue()
        {
            return decimal.Zero;
        }

        public override bool Parse(string arg, out decimal result)
        {
            return decimal.TryParse(arg, out result);
        }
    }
}
