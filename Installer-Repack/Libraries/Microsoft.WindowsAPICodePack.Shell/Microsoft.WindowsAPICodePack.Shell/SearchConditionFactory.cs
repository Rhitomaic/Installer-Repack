using System;
using System.Collections.Generic;
using System.Globalization;
using System.Runtime.InteropServices;
using Microsoft.WindowsAPICodePack.Shell.PropertySystem;
using Microsoft.WindowsAPICodePack.Shell.Resources;
using MS.WindowsAPICodePack.Internal;

namespace Microsoft.WindowsAPICodePack.Shell
{
	public static class SearchConditionFactory
	{
		public static SearchCondition CreateLeafCondition(string propertyName, string value, SearchConditionOperation operation)
		{
			using (PropVariant propVar = new PropVariant(value))
			{
				return CreateLeafCondition(propertyName, propVar, null, operation);
			}
		}

		public static SearchCondition CreateLeafCondition(string propertyName, DateTime value, SearchConditionOperation operation)
		{
			using (PropVariant propVar = new PropVariant(value))
			{
				return CreateLeafCondition(propertyName, propVar, "System.StructuredQuery.CustomProperty.DateTime", operation);
			}
		}

		public static SearchCondition CreateLeafCondition(string propertyName, int value, SearchConditionOperation operation)
		{
			using (PropVariant propVar = new PropVariant(value))
			{
				return CreateLeafCondition(propertyName, propVar, "System.StructuredQuery.CustomProperty.Integer", operation);
			}
		}

		public static SearchCondition CreateLeafCondition(string propertyName, bool value, SearchConditionOperation operation)
		{
			using (PropVariant propVar = new PropVariant(value))
			{
				return CreateLeafCondition(propertyName, propVar, "System.StructuredQuery.CustomProperty.Boolean", operation);
			}
		}

		public static SearchCondition CreateLeafCondition(string propertyName, double value, SearchConditionOperation operation)
		{
			using (PropVariant propVar = new PropVariant(value))
			{
				return CreateLeafCondition(propertyName, propVar, "System.StructuredQuery.CustomProperty.FloatingPoint", operation);
			}
		}

		private static SearchCondition CreateLeafCondition(string propertyName, PropVariant propVar, string valueType, SearchConditionOperation operation)
		{
			IConditionFactory conditionFactory = null;
			SearchCondition result = null;
			try
			{
				conditionFactory = (IConditionFactory)new ConditionFactoryCoClass();
				ICondition ppcResult = null;
				if (string.IsNullOrEmpty(propertyName) || propertyName.ToUpperInvariant() == "SYSTEM.NULL")
				{
					propertyName = null;
				}
				HResult hResult = HResult.Fail;
				hResult = conditionFactory.MakeLeaf(propertyName, operation, valueType, propVar, null, null, null, false, out ppcResult);
				if (!CoreErrorHelper.Succeeded(hResult))
				{
					throw new ShellException(hResult);
				}
				result = new SearchCondition(ppcResult);
			}
			finally
			{
				if (conditionFactory != null)
				{
					Marshal.ReleaseComObject(conditionFactory);
				}
			}
			return result;
		}

		public static SearchCondition CreateLeafCondition(PropertyKey propertyKey, string value, SearchConditionOperation operation)
		{
			PropertySystemNativeMethods.PSGetNameFromPropertyKey(ref propertyKey, out var ppszCanonicalName);
			if (string.IsNullOrEmpty(ppszCanonicalName))
			{
				throw new ArgumentException(LocalizedMessages.SearchConditionFactoryInvalidProperty, "propertyKey");
			}
			return CreateLeafCondition(ppszCanonicalName, value, operation);
		}

		public static SearchCondition CreateLeafCondition(PropertyKey propertyKey, DateTime value, SearchConditionOperation operation)
		{
			PropertySystemNativeMethods.PSGetNameFromPropertyKey(ref propertyKey, out var ppszCanonicalName);
			if (string.IsNullOrEmpty(ppszCanonicalName))
			{
				throw new ArgumentException(LocalizedMessages.SearchConditionFactoryInvalidProperty, "propertyKey");
			}
			return CreateLeafCondition(ppszCanonicalName, value, operation);
		}

		public static SearchCondition CreateLeafCondition(PropertyKey propertyKey, bool value, SearchConditionOperation operation)
		{
			PropertySystemNativeMethods.PSGetNameFromPropertyKey(ref propertyKey, out var ppszCanonicalName);
			if (string.IsNullOrEmpty(ppszCanonicalName))
			{
				throw new ArgumentException(LocalizedMessages.SearchConditionFactoryInvalidProperty, "propertyKey");
			}
			return CreateLeafCondition(ppszCanonicalName, value, operation);
		}

		public static SearchCondition CreateLeafCondition(PropertyKey propertyKey, double value, SearchConditionOperation operation)
		{
			PropertySystemNativeMethods.PSGetNameFromPropertyKey(ref propertyKey, out var ppszCanonicalName);
			if (string.IsNullOrEmpty(ppszCanonicalName))
			{
				throw new ArgumentException(LocalizedMessages.SearchConditionFactoryInvalidProperty, "propertyKey");
			}
			return CreateLeafCondition(ppszCanonicalName, value, operation);
		}

		public static SearchCondition CreateLeafCondition(PropertyKey propertyKey, int value, SearchConditionOperation operation)
		{
			PropertySystemNativeMethods.PSGetNameFromPropertyKey(ref propertyKey, out var ppszCanonicalName);
			if (string.IsNullOrEmpty(ppszCanonicalName))
			{
				throw new ArgumentException(LocalizedMessages.SearchConditionFactoryInvalidProperty, "propertyKey");
			}
			return CreateLeafCondition(ppszCanonicalName, value, operation);
		}

		public static SearchCondition CreateAndOrCondition(SearchConditionType conditionType, bool simplify, params SearchCondition[] conditionNodes)
		{
			IConditionFactory conditionFactory = (IConditionFactory)new ConditionFactoryCoClass();
			ICondition ppcResult = null;
			try
			{
				List<ICondition> list = new List<ICondition>();
				if (conditionNodes != null)
				{
					foreach (SearchCondition searchCondition in conditionNodes)
					{
						list.Add(searchCondition.NativeSearchCondition);
					}
				}
				IEnumUnknown peuSubs = new EnumUnknownClass(list.ToArray());
				HResult result = conditionFactory.MakeAndOr(conditionType, peuSubs, simplify, out ppcResult);
				if (!CoreErrorHelper.Succeeded(result))
				{
					throw new ShellException(result);
				}
			}
			finally
			{
				if (conditionFactory != null)
				{
					Marshal.ReleaseComObject(conditionFactory);
				}
			}
			return new SearchCondition(ppcResult);
		}

		public static SearchCondition CreateNotCondition(SearchCondition conditionToBeNegated, bool simplify)
		{
			if (conditionToBeNegated == null)
			{
				throw new ArgumentNullException("conditionToBeNegated");
			}
			IConditionFactory conditionFactory = (IConditionFactory)new ConditionFactoryCoClass();
			ICondition ppcResult;
			try
			{
				HResult result = conditionFactory.MakeNot(conditionToBeNegated.NativeSearchCondition, simplify, out ppcResult);
				if (!CoreErrorHelper.Succeeded(result))
				{
					throw new ShellException(result);
				}
			}
			finally
			{
				if (conditionFactory != null)
				{
					Marshal.ReleaseComObject(conditionFactory);
				}
			}
			return new SearchCondition(ppcResult);
		}

		public static SearchCondition ParseStructuredQuery(string query)
		{
			return ParseStructuredQuery(query, null);
		}

		public static SearchCondition ParseStructuredQuery(string query, CultureInfo cultureInfo)
		{
			if (string.IsNullOrEmpty(query))
			{
				throw new ArgumentNullException("query");
			}
			IQueryParserManager queryParserManager = (IQueryParserManager)new QueryParserManagerCoClass();
			IQueryParser ppQueryParser = null;
			IQuerySolution ppSolution = null;
			ICondition ppQueryNode = null;
			IEntity ppMainType = null;
			SearchCondition searchCondition = null;
			try
			{
				Guid riid = new Guid("2EBDEE67-3505-43f8-9946-EA44ABC8E5B0");
				HResult result = queryParserManager.CreateLoadedParser("SystemIndex", (ushort)((cultureInfo != null) ? ((ushort)cultureInfo.LCID) : 0), ref riid, out ppQueryParser);
				if (!CoreErrorHelper.Succeeded(result))
				{
					throw new ShellException(result);
				}
				if (ppQueryParser != null)
				{
					using (PropVariant pOptionValue = new PropVariant(true))
					{
						result = ppQueryParser.SetOption(StructuredQuerySingleOption.NaturalSyntax, pOptionValue);
					}
					if (!CoreErrorHelper.Succeeded(result))
					{
						throw new ShellException(result);
					}
					result = ppQueryParser.Parse(query, null, out ppSolution);
					if (!CoreErrorHelper.Succeeded(result))
					{
						throw new ShellException(result);
					}
					if (ppSolution != null)
					{
						result = ppSolution.GetQuery(out ppQueryNode, out ppMainType);
						if (!CoreErrorHelper.Succeeded(result))
						{
							throw new ShellException(result);
						}
					}
				}
				searchCondition = new SearchCondition(ppQueryNode);
				return searchCondition;
			}
			catch
			{
				searchCondition?.Dispose();
				throw;
			}
			finally
			{
				if (queryParserManager != null)
				{
					Marshal.ReleaseComObject(queryParserManager);
				}
				if (ppQueryParser != null)
				{
					Marshal.ReleaseComObject(ppQueryParser);
				}
				if (ppSolution != null)
				{
					Marshal.ReleaseComObject(ppSolution);
				}
				if (ppMainType != null)
				{
					Marshal.ReleaseComObject(ppMainType);
				}
			}
		}
	}
}
