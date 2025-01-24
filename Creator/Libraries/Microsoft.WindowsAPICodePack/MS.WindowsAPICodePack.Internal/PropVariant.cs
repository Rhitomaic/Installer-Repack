using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using Microsoft.WindowsAPICodePack.Resources;
using Microsoft.WindowsAPICodePack.Shell.PropertySystem;

namespace MS.WindowsAPICodePack.Internal
{
	[StructLayout(LayoutKind.Explicit)]
	public sealed class PropVariant : IDisposable
	{
		private static Dictionary<Type, Action<PropVariant, Array, uint>> _vectorActions = null;

		private static Dictionary<Type, Func<object, PropVariant>> _cache = new Dictionary<Type, Func<object, PropVariant>>();

		private static object _padlock = new object();

		[FieldOffset(0)]
		private decimal _decimal;

		[FieldOffset(0)]
		private ushort _valueType;

		[FieldOffset(12)]
		private IntPtr _ptr2;

		[FieldOffset(8)]
		private IntPtr _ptr;

		[FieldOffset(8)]
		private int _int32;

		[FieldOffset(8)]
		private uint _uint32;

		[FieldOffset(8)]
		private byte _byte;

		[FieldOffset(8)]
		private sbyte _sbyte;

		[FieldOffset(8)]
		private short _short;

		[FieldOffset(8)]
		private ushort _ushort;

		[FieldOffset(8)]
		private long _long;

		[FieldOffset(8)]
		private ulong _ulong;

		[FieldOffset(8)]
		private double _double;

		[FieldOffset(8)]
		private float _float;

		public VarEnum VarType
		{
			get
			{
				return (VarEnum)_valueType;
			}
			set
			{
				_valueType = (ushort)value;
			}
		}

		public bool IsNullOrEmpty => _valueType == 0 || _valueType == 1;

		public object Value
		{
			get
			{
				switch ((VarEnum)_valueType)
				{
				case VarEnum.VT_I1:
					return _sbyte;
				case VarEnum.VT_UI1:
					return _byte;
				case VarEnum.VT_I2:
					return _short;
				case VarEnum.VT_UI2:
					return _ushort;
				case VarEnum.VT_I4:
				case VarEnum.VT_INT:
					return _int32;
				case VarEnum.VT_UI4:
				case VarEnum.VT_UINT:
					return _uint32;
				case VarEnum.VT_I8:
					return _long;
				case VarEnum.VT_UI8:
					return _ulong;
				case VarEnum.VT_R4:
					return _float;
				case VarEnum.VT_R8:
					return _double;
				case VarEnum.VT_BOOL:
					return _int32 == -1;
				case VarEnum.VT_ERROR:
					return _long;
				case VarEnum.VT_CY:
					return _decimal;
				case VarEnum.VT_DATE:
					return DateTime.FromOADate(_double);
				case VarEnum.VT_FILETIME:
					return DateTime.FromFileTime(_long);
				case VarEnum.VT_BSTR:
					return Marshal.PtrToStringBSTR(_ptr);
				case VarEnum.VT_BLOB:
					return GetBlobData();
				case VarEnum.VT_LPSTR:
					return Marshal.PtrToStringAnsi(_ptr);
				case VarEnum.VT_LPWSTR:
					return Marshal.PtrToStringUni(_ptr);
				case VarEnum.VT_UNKNOWN:
					return Marshal.GetObjectForIUnknown(_ptr);
				case VarEnum.VT_DISPATCH:
					return Marshal.GetObjectForIUnknown(_ptr);
				case VarEnum.VT_DECIMAL:
					return _decimal;
				case (VarEnum)8205:
					return CrackSingleDimSafeArray(_ptr);
				case (VarEnum)4127:
					return GetVector<string>();
				case (VarEnum)4098:
					return GetVector<short>();
				case (VarEnum)4114:
					return GetVector<ushort>();
				case (VarEnum)4099:
					return GetVector<int>();
				case (VarEnum)4115:
					return GetVector<uint>();
				case (VarEnum)4116:
					return GetVector<long>();
				case (VarEnum)4117:
					return GetVector<ulong>();
				case (VarEnum)4100:
					return GetVector<float>();
				case (VarEnum)4101:
					return GetVector<double>();
				case (VarEnum)4107:
					return GetVector<bool>();
				case (VarEnum)4160:
					return GetVector<DateTime>();
				case (VarEnum)4110:
					return GetVector<decimal>();
				default:
					return null;
				}
			}
		}

		private static Dictionary<Type, Action<PropVariant, Array, uint>> GenerateVectorActions()
		{
			Dictionary<Type, Action<PropVariant, Array, uint>> dictionary = new Dictionary<Type, Action<PropVariant, Array, uint>>();
			dictionary.Add(typeof(short), delegate(PropVariant pv, Array array, uint i)
			{
				PropVariantNativeMethods.PropVariantGetInt16Elem(pv, i, out var pnVal7);
				array.SetValue(pnVal7, i);
			});
			dictionary.Add(typeof(ushort), delegate(PropVariant pv, Array array, uint i)
			{
				PropVariantNativeMethods.PropVariantGetUInt16Elem(pv, i, out var pnVal6);
				array.SetValue(pnVal6, i);
			});
			dictionary.Add(typeof(int), delegate(PropVariant pv, Array array, uint i)
			{
				PropVariantNativeMethods.PropVariantGetInt32Elem(pv, i, out var pnVal5);
				array.SetValue(pnVal5, i);
			});
			dictionary.Add(typeof(uint), delegate(PropVariant pv, Array array, uint i)
			{
				PropVariantNativeMethods.PropVariantGetUInt32Elem(pv, i, out var pnVal4);
				array.SetValue(pnVal4, i);
			});
			dictionary.Add(typeof(long), delegate(PropVariant pv, Array array, uint i)
			{
				PropVariantNativeMethods.PropVariantGetInt64Elem(pv, i, out var pnVal3);
				array.SetValue(pnVal3, i);
			});
			dictionary.Add(typeof(ulong), delegate(PropVariant pv, Array array, uint i)
			{
				PropVariantNativeMethods.PropVariantGetUInt64Elem(pv, i, out var pnVal2);
				array.SetValue(pnVal2, i);
			});
			dictionary.Add(typeof(DateTime), delegate(PropVariant pv, Array array, uint i)
			{
				PropVariantNativeMethods.PropVariantGetFileTimeElem(pv, i, out var pftVal);
				long fileTimeAsLong = GetFileTimeAsLong(ref pftVal);
				array.SetValue(DateTime.FromFileTime(fileTimeAsLong), i);
			});
			dictionary.Add(typeof(bool), delegate(PropVariant pv, Array array, uint i)
			{
				PropVariantNativeMethods.PropVariantGetBooleanElem(pv, i, out var pfVal);
				array.SetValue(pfVal, i);
			});
			dictionary.Add(typeof(double), delegate(PropVariant pv, Array array, uint i)
			{
				PropVariantNativeMethods.PropVariantGetDoubleElem(pv, i, out var pnVal);
				array.SetValue(pnVal, i);
			});
			dictionary.Add(typeof(float), delegate(PropVariant pv, Array array, uint i)
			{
				float[] array3 = new float[1];
				Marshal.Copy(pv._ptr2, array3, (int)i, 1);
				array.SetValue(array3[0], (int)i);
			});
			dictionary.Add(typeof(decimal), delegate(PropVariant pv, Array array, uint i)
			{
				int[] array2 = new int[4];
				for (int j = 0; j < array2.Length; j++)
				{
					array2[j] = Marshal.ReadInt32(pv._ptr2, (int)(i * 16) + j * 4);
				}
				array.SetValue(new decimal(array2), i);
			});
			dictionary.Add(typeof(string), delegate(PropVariant pv, Array array, uint i)
			{
				string ppszVal = string.Empty;
				PropVariantNativeMethods.PropVariantGetStringElem(pv, i, ref ppszVal);
				array.SetValue(ppszVal, i);
			});
			return dictionary;
		}

		public static PropVariant FromObject(object value)
		{
			if (value == null)
			{
				return new PropVariant();
			}
			Func<object, PropVariant> dynamicConstructor = GetDynamicConstructor(value.GetType());
			return dynamicConstructor(value);
		}

		private static Func<object, PropVariant> GetDynamicConstructor(Type type)
		{
			lock (_padlock)
			{
				if (!_cache.TryGetValue(type, out var value))
				{
					ConstructorInfo constructor = typeof(PropVariant).GetConstructor(new Type[1] { type });
					if (constructor == null)
					{
						throw new ArgumentException(LocalizedMessages.PropVariantTypeNotSupported);
					}
					ParameterExpression parameterExpression = Expression.Parameter(typeof(object), "arg");
					NewExpression body = Expression.New(constructor, Expression.Convert(parameterExpression, type));
					value = Expression.Lambda<Func<object, PropVariant>>(body, new ParameterExpression[1] { parameterExpression }).Compile();
					_cache.Add(type, value);
				}
				return value;
			}
		}

		public PropVariant()
		{
		}

		public PropVariant(string value)
		{
			if (value == null)
			{
				throw new ArgumentException(LocalizedMessages.PropVariantNullString, "value");
			}
			_valueType = 31;
			_ptr = Marshal.StringToCoTaskMemUni(value);
		}

		public PropVariant(string[] value)
		{
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			PropVariantNativeMethods.InitPropVariantFromStringVector(value, (uint)value.Length, this);
		}

		public PropVariant(bool[] value)
		{
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			PropVariantNativeMethods.InitPropVariantFromBooleanVector(value, (uint)value.Length, this);
		}

		public PropVariant(short[] value)
		{
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			PropVariantNativeMethods.InitPropVariantFromInt16Vector(value, (uint)value.Length, this);
		}

		public PropVariant(ushort[] value)
		{
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			PropVariantNativeMethods.InitPropVariantFromUInt16Vector(value, (uint)value.Length, this);
		}

		public PropVariant(int[] value)
		{
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			PropVariantNativeMethods.InitPropVariantFromInt32Vector(value, (uint)value.Length, this);
		}

		public PropVariant(uint[] value)
		{
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			PropVariantNativeMethods.InitPropVariantFromUInt32Vector(value, (uint)value.Length, this);
		}

		public PropVariant(long[] value)
		{
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			PropVariantNativeMethods.InitPropVariantFromInt64Vector(value, (uint)value.Length, this);
		}

		public PropVariant(ulong[] value)
		{
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			PropVariantNativeMethods.InitPropVariantFromUInt64Vector(value, (uint)value.Length, this);
		}

		public PropVariant(double[] value)
		{
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			PropVariantNativeMethods.InitPropVariantFromDoubleVector(value, (uint)value.Length, this);
		}

		public PropVariant(DateTime[] value)
		{
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			System.Runtime.InteropServices.ComTypes.FILETIME[] array = new System.Runtime.InteropServices.ComTypes.FILETIME[value.Length];
			for (int i = 0; i < value.Length; i++)
			{
				ref System.Runtime.InteropServices.ComTypes.FILETIME reference = ref array[i];
				reference = DateTimeToFileTime(value[i]);
			}
			PropVariantNativeMethods.InitPropVariantFromFileTimeVector(array, (uint)array.Length, this);
		}

		public PropVariant(bool value)
		{
			_valueType = 11;
			_int32 = (value ? (-1) : 0);
		}

		public PropVariant(DateTime value)
		{
			_valueType = 64;
			System.Runtime.InteropServices.ComTypes.FILETIME pftIn = DateTimeToFileTime(value);
			PropVariantNativeMethods.InitPropVariantFromFileTime(ref pftIn, this);
		}

		public PropVariant(byte value)
		{
			_valueType = 17;
			_byte = value;
		}

		public PropVariant(sbyte value)
		{
			_valueType = 16;
			_sbyte = value;
		}

		public PropVariant(short value)
		{
			_valueType = 2;
			_short = value;
		}

		public PropVariant(ushort value)
		{
			_valueType = 18;
			_ushort = value;
		}

		public PropVariant(int value)
		{
			_valueType = 3;
			_int32 = value;
		}

		public PropVariant(uint value)
		{
			_valueType = 19;
			_uint32 = value;
		}

		public PropVariant(decimal value)
		{
			_decimal = value;
			_valueType = 14;
		}

		public PropVariant(decimal[] value)
		{
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			_valueType = 4110;
			_int32 = value.Length;
			_ptr2 = Marshal.AllocCoTaskMem(value.Length * 16);
			for (int i = 0; i < value.Length; i++)
			{
				int[] bits = decimal.GetBits(value[i]);
				Marshal.Copy(bits, 0, _ptr2, bits.Length);
			}
		}

		public PropVariant(float value)
		{
			_valueType = 4;
			_float = value;
		}

		public PropVariant(float[] value)
		{
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			_valueType = 4100;
			_int32 = value.Length;
			_ptr2 = Marshal.AllocCoTaskMem(value.Length * 4);
			Marshal.Copy(value, 0, _ptr2, value.Length);
		}

		public PropVariant(long value)
		{
			_long = value;
			_valueType = 20;
		}

		public PropVariant(ulong value)
		{
			_valueType = 21;
			_ulong = value;
		}

		public PropVariant(double value)
		{
			_valueType = 5;
			_double = value;
		}

		internal void SetIUnknown(object value)
		{
			_valueType = 13;
			_ptr = Marshal.GetIUnknownForObject(value);
		}

		internal void SetSafeArray(Array array)
		{
			if (array == null)
			{
				throw new ArgumentNullException("array");
			}
			IntPtr intPtr = PropVariantNativeMethods.SafeArrayCreateVector(13, 0, (uint)array.Length);
			IntPtr ptr = PropVariantNativeMethods.SafeArrayAccessData(intPtr);
			try
			{
				for (int i = 0; i < array.Length; i++)
				{
					object value = array.GetValue(i);
					IntPtr val = ((value != null) ? Marshal.GetIUnknownForObject(value) : IntPtr.Zero);
					Marshal.WriteIntPtr(ptr, i * IntPtr.Size, val);
				}
			}
			finally
			{
				PropVariantNativeMethods.SafeArrayUnaccessData(intPtr);
			}
			_valueType = 8205;
			_ptr = intPtr;
		}

		private static long GetFileTimeAsLong(ref System.Runtime.InteropServices.ComTypes.FILETIME val)
		{
			return ((long)val.dwHighDateTime << 32) + val.dwLowDateTime;
		}

		private static System.Runtime.InteropServices.ComTypes.FILETIME DateTimeToFileTime(DateTime value)
		{
			long num = value.ToFileTime();
			System.Runtime.InteropServices.ComTypes.FILETIME result = default(System.Runtime.InteropServices.ComTypes.FILETIME);
			result.dwLowDateTime = (int)(num & 0xFFFFFFFFu);
			result.dwHighDateTime = (int)(num >> 32);
			return result;
		}

		private object GetBlobData()
		{
			byte[] array = new byte[_int32];
			IntPtr ptr = _ptr2;
			Marshal.Copy(ptr, array, 0, _int32);
			return array;
		}

		private Array GetVector<T>()
		{
			int num = PropVariantNativeMethods.PropVariantGetElementCount(this);
			if (num <= 0)
			{
				return null;
			}
			lock (_padlock)
			{
				if (_vectorActions == null)
				{
					_vectorActions = GenerateVectorActions();
				}
			}
			if (!_vectorActions.TryGetValue(typeof(T), out var value))
			{
				throw new InvalidCastException(LocalizedMessages.PropVariantUnsupportedType);
			}
			Array array = new T[num];
			for (uint num2 = 0u; num2 < num; num2++)
			{
				value(this, array, num2);
			}
			return array;
		}

		private static Array CrackSingleDimSafeArray(IntPtr psa)
		{
			uint num = PropVariantNativeMethods.SafeArrayGetDim(psa);
			if (num != 1)
			{
				throw new ArgumentException(LocalizedMessages.PropVariantMultiDimArray, "psa");
			}
			int num2 = PropVariantNativeMethods.SafeArrayGetLBound(psa, 1u);
			int num3 = PropVariantNativeMethods.SafeArrayGetUBound(psa, 1u);
			int num4 = num3 - num2 + 1;
			object[] array = new object[num4];
			for (int i = num2; i <= num3; i++)
			{
				array[i] = PropVariantNativeMethods.SafeArrayGetElement(psa, ref i);
			}
			return array;
		}

		public void Dispose()
		{
			PropVariantNativeMethods.PropVariantClear(this);
			GC.SuppressFinalize(this);
		}

		~PropVariant()
		{
			Dispose();
		}

		public override string ToString()
		{
			return string.Format(CultureInfo.InvariantCulture, "{0}: {1}", Value, VarType.ToString());
		}
	}
}
