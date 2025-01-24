using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using Microsoft.WindowsAPICodePack.Shell.Resources;

namespace Microsoft.WindowsAPICodePack.Shell.PropertySystem
{
	internal static class ShellPropertyFactory
	{
		private static Dictionary<int, Func<PropertyKey, ShellPropertyDescription, object, IShellProperty>> _storeCache = new Dictionary<int, Func<PropertyKey, ShellPropertyDescription, object, IShellProperty>>();

		public static IShellProperty CreateShellProperty(PropertyKey propKey, ShellObject shellObject)
		{
			return GenericCreateShellProperty(propKey, shellObject);
		}

		public static IShellProperty CreateShellProperty(PropertyKey propKey, IPropertyStore store)
		{
			return GenericCreateShellProperty(propKey, store);
		}

		private static IShellProperty GenericCreateShellProperty<T>(PropertyKey propKey, T thirdArg)
		{
			Type type = ((thirdArg is ShellObject) ? typeof(ShellObject) : typeof(T));
			ShellPropertyDescription propertyDescription = ShellPropertyDescriptionsCache.Cache.GetPropertyDescription(propKey);
			Type type2 = typeof(ShellProperty<>).MakeGenericType(VarEnumToSystemType(propertyDescription.VarEnumType));
			int typeHash = GetTypeHash(type2, type);
			if (!_storeCache.TryGetValue(typeHash, out var value))
			{
				Type[] argTypes = new Type[3]
				{
					typeof(PropertyKey),
					typeof(ShellPropertyDescription),
					type
				};
				value = ExpressConstructor(type2, argTypes);
				_storeCache.Add(typeHash, value);
			}
			return value(propKey, propertyDescription, thirdArg);
		}

		public static Type VarEnumToSystemType(VarEnum VarEnumType)
		{
			switch (VarEnumType)
			{
			case VarEnum.VT_EMPTY:
			case VarEnum.VT_NULL:
				return typeof(object);
			case VarEnum.VT_UI1:
				return typeof(byte?);
			case VarEnum.VT_I2:
				return typeof(short?);
			case VarEnum.VT_UI2:
				return typeof(ushort?);
			case VarEnum.VT_I4:
				return typeof(int?);
			case VarEnum.VT_UI4:
				return typeof(uint?);
			case VarEnum.VT_I8:
				return typeof(long?);
			case VarEnum.VT_UI8:
				return typeof(ulong?);
			case VarEnum.VT_R8:
				return typeof(double?);
			case VarEnum.VT_BOOL:
				return typeof(bool?);
			case VarEnum.VT_FILETIME:
				return typeof(DateTime?);
			case VarEnum.VT_CLSID:
				return typeof(IntPtr?);
			case VarEnum.VT_CF:
				return typeof(IntPtr?);
			case VarEnum.VT_BLOB:
				return typeof(byte[]);
			case VarEnum.VT_LPWSTR:
				return typeof(string);
			case VarEnum.VT_UNKNOWN:
				return typeof(IntPtr?);
			case VarEnum.VT_STREAM:
				return typeof(IStream);
			case (VarEnum)4113:
				return typeof(byte[]);
			case (VarEnum)4098:
				return typeof(short[]);
			case (VarEnum)4114:
				return typeof(ushort[]);
			case (VarEnum)4099:
				return typeof(int[]);
			case (VarEnum)4115:
				return typeof(uint[]);
			case (VarEnum)4116:
				return typeof(long[]);
			case (VarEnum)4117:
				return typeof(ulong[]);
			case (VarEnum)4101:
				return typeof(double[]);
			case (VarEnum)4107:
				return typeof(bool[]);
			case (VarEnum)4160:
				return typeof(DateTime[]);
			case (VarEnum)4168:
				return typeof(IntPtr[]);
			case (VarEnum)4167:
				return typeof(IntPtr[]);
			case (VarEnum)4127:
				return typeof(string[]);
			default:
				return typeof(object);
			}
		}

		private static Func<PropertyKey, ShellPropertyDescription, object, IShellProperty> ExpressConstructor(Type type, Type[] argTypes)
		{
			int typeHash = GetTypeHash(argTypes);
			ConstructorInfo constructorInfo = type.GetConstructors(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic).FirstOrDefault((ConstructorInfo x) => typeHash == GetTypeHash(from a in x.GetParameters()
				select a.ParameterType));
			if (constructorInfo == null)
			{
				throw new ArgumentException(LocalizedMessages.ShellPropertyFactoryConstructorNotFound, "type");
			}
			ParameterExpression parameterExpression = Expression.Parameter(argTypes[0], "propKey");
			ParameterExpression parameterExpression2 = Expression.Parameter(argTypes[1], "desc");
			ParameterExpression parameterExpression3 = Expression.Parameter(typeof(object), "third");
			NewExpression body = Expression.New(constructorInfo, parameterExpression, parameterExpression2, Expression.Convert(parameterExpression3, argTypes[2]));
			return Expression.Lambda<Func<PropertyKey, ShellPropertyDescription, object, IShellProperty>>(body, new ParameterExpression[3] { parameterExpression, parameterExpression2, parameterExpression3 }).Compile();
		}

		private static int GetTypeHash(params Type[] types)
		{
			return GetTypeHash((IEnumerable<Type>)types);
		}

		private static int GetTypeHash(IEnumerable<Type> types)
		{
			int num = 0;
			foreach (Type type in types)
			{
				num = num * 31 + type.GetHashCode();
			}
			return num;
		}
	}
}
