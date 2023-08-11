using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Text.RegularExpressions;
using Ionic.Zip;

namespace Ionic
{
	/// <summary>
	///   FileSelector encapsulates logic that selects files from a source - a zip file
	///   or the filesystem - based on a set of criteria.  This class is used internally
	///   by the DotNetZip library, in particular for the AddSelectedFiles() methods.
	///   This class can also be used independently of the zip capability in DotNetZip.
	/// </summary>
	///
	/// <remarks>
	///
	/// <para>
	///   The FileSelector class is used internally by the ZipFile class for selecting
	///   files for inclusion into the ZipFile, when the <see cref="M:Ionic.Zip.ZipFile.AddSelectedFiles(System.String,System.String)" /> method, or one of
	///   its overloads, is called.  It's also used for the <see cref="M:Ionic.Zip.ZipFile.ExtractSelectedEntries(System.String)" /> methods.  Typically, an
	///   application that creates or manipulates Zip archives will not directly
	///   interact with the FileSelector class.
	/// </para>
	///
	/// <para>
	///   Some applications may wish to use the FileSelector class directly, to
	///   select files from disk volumes based on a set of criteria, without creating or
	///   querying Zip archives.  The file selection criteria include: a pattern to
	///   match the filename; the last modified, created, or last accessed time of the
	///   file; the size of the file; and the attributes of the file.
	/// </para>
	///
	/// <para>
	///   Consult the documentation for <see cref="P:Ionic.FileSelector.SelectionCriteria" />
	///   for more information on specifying the selection criteria.
	/// </para>
	///
	/// </remarks>
	public class FileSelector
	{
		private enum ParseState
		{
			Start,
			OpenParen,
			CriterionDone,
			ConjunctionPending,
			Whitespace
		}

		private static class RegexAssertions
		{
			public static readonly string PrecededByOddNumberOfSingleQuotes = "(?<=(?:[^']*'[^']*')*'[^']*)";

			public static readonly string FollowedByOddNumberOfSingleQuotesAndLineEnd = "(?=[^']*'(?:[^']*'[^']*')*[^']*$)";

			public static readonly string PrecededByEvenNumberOfSingleQuotes = "(?<=(?:[^']*'[^']*')*[^']*)";

			public static readonly string FollowedByEvenNumberOfSingleQuotesAndLineEnd = "(?=(?:[^']*'[^']*')*[^']*$)";
		}

		internal SelectionCriterion _Criterion;

		/// <summary>
		///   The string specifying which files to include when retrieving.
		/// </summary>
		/// <remarks>
		///
		/// <para>
		///   Specify the criteria in statements of 3 elements: a noun, an operator,
		///   and a value.  Consider the string "name != *.doc" .  The noun is
		///   "name".  The operator is "!=", implying "Not Equal".  The value is
		///   "*.doc".  That criterion, in English, says "all files with a name that
		///   does not end in the .doc extension."
		/// </para>
		///
		/// <para>
		///   Supported nouns include "name" (or "filename") for the filename;
		///   "atime", "mtime", and "ctime" for last access time, last modfied time,
		///   and created time of the file, respectively; "attributes" (or "attrs")
		///   for the file attributes; "size" (or "length") for the file length
		///   (uncompressed); and "type" for the type of object, either a file or a
		///   directory.  The "attributes", "type", and "name" nouns all support =
		///   and != as operators.  The "size", "atime", "mtime", and "ctime" nouns
		///   support = and !=, and &gt;, &gt;=, &lt;, &lt;= as well.  The times are
		///   taken to be expressed in local time.
		/// </para>
		///
		/// <para>
		///   Specify values for the file attributes as a string with one or more of
		///   the characters H,R,S,A,I,L in any order, implying file attributes of
		///   Hidden, ReadOnly, System, Archive, NotContextIndexed, and ReparsePoint
		///   (symbolic link) respectively.
		/// </para>
		///
		/// <para>
		///   To specify a time, use YYYY-MM-DD-HH:mm:ss or YYYY/MM/DD-HH:mm:ss as
		///   the format.  If you omit the HH:mm:ss portion, it is assumed to be
		///   00:00:00 (midnight).
		/// </para>
		///
		/// <para>
		///   The value for a size criterion is expressed in integer quantities of
		///   bytes, kilobytes (use k or kb after the number), megabytes (m or mb),
		///   or gigabytes (g or gb).
		/// </para>
		///
		/// <para>
		///   The value for a name is a pattern to match against the filename,
		///   potentially including wildcards.  The pattern follows CMD.exe glob
		///   rules: * implies one or more of any character, while ?  implies one
		///   character.  If the name pattern contains any slashes, it is matched to
		///   the entire filename, including the path; otherwise, it is matched
		///   against only the filename without the path.  This means a pattern of
		///   "*\*.*" matches all files one directory level deep, while a pattern of
		///   "*.*" matches all files in all directories.
		/// </para>
		///
		/// <para>
		///   To specify a name pattern that includes spaces, use single quotes
		///   around the pattern.  A pattern of "'* *.*'" will match all files that
		///   have spaces in the filename.  The full criteria string for that would
		///   be "name = '* *.*'" .
		/// </para>
		///
		/// <para>
		///   The value for a type criterion is either F (implying a file) or D
		///   (implying a directory).
		/// </para>
		///
		/// <para>
		///   Some examples:
		/// </para>
		///
		/// <list type="table">
		///   <listheader>
		///     <term>criteria</term>
		///     <description>Files retrieved</description>
		///   </listheader>
		///
		///   <item>
		///     <term>name != *.xls </term>
		///     <description>any file with an extension that is not .xls
		///     </description>
		///   </item>
		///
		///   <item>
		///     <term>name = *.mp3 </term>
		///     <description>any file with a .mp3 extension.
		///     </description>
		///   </item>
		///
		///   <item>
		///     <term>*.mp3</term>
		///     <description>(same as above) any file with a .mp3 extension.
		///     </description>
		///   </item>
		///
		///   <item>
		///     <term>attributes = A </term>
		///     <description>all files whose attributes include the Archive bit.
		///     </description>
		///   </item>
		///
		///   <item>
		///     <term>attributes != H </term>
		///     <description>all files whose attributes do not include the Hidden bit.
		///     </description>
		///   </item>
		///
		///   <item>
		///     <term>mtime &gt; 2009-01-01</term>
		///     <description>all files with a last modified time after January 1st, 2009.
		///     </description>
		///   </item>
		///
		///   <item>
		///     <term>ctime &gt; 2009/01/01-03:00:00</term>
		///     <description>all files with a created time after 3am (local time),
		///     on January 1st, 2009.
		///     </description>
		///   </item>
		///
		///   <item>
		///     <term>size &gt; 2gb</term>
		///     <description>all files whose uncompressed size is greater than 2gb.
		///     </description>
		///   </item>
		///
		///   <item>
		///     <term>type = D</term>
		///     <description>all directories in the filesystem. </description>
		///   </item>
		///
		/// </list>
		///
		/// <para>
		///   You can combine criteria with the conjunctions AND, OR, and XOR. Using
		///   a string like "name = *.txt AND size &gt;= 100k" for the
		///   selectionCriteria retrieves entries whose names end in .txt, and whose
		///   uncompressed size is greater than or equal to 100 kilobytes.
		/// </para>
		///
		/// <para>
		///   For more complex combinations of criteria, you can use parenthesis to
		///   group clauses in the boolean logic.  Absent parenthesis, the
		///   precedence of the criterion atoms is determined by order of
		///   appearance.  Unlike the C# language, the AND conjunction does not take
		///   precendence over the logical OR.  This is important only in strings
		///   that contain 3 or more criterion atoms.  In other words, "name = *.txt
		///   and size &gt; 1000 or attributes = H" implies "((name = *.txt AND size
		///   &gt; 1000) OR attributes = H)" while "attributes = H OR name = *.txt
		///   and size &gt; 1000" evaluates to "((attributes = H OR name = *.txt)
		///   AND size &gt; 1000)".  When in doubt, use parenthesis.
		/// </para>
		///
		/// <para>
		///   Using time properties requires some extra care. If you want to
		///   retrieve all entries that were last updated on 2009 February 14,
		///   specify "mtime &gt;= 2009-02-14 AND mtime &lt; 2009-02-15".  Read this
		///   to say: all files updated after 12:00am on February 14th, until
		///   12:00am on February 15th.  You can use the same bracketing approach to
		///   specify any time period - a year, a month, a week, and so on.
		/// </para>
		///
		/// <para>
		///   The syntax allows one special case: if you provide a string with no
		///   spaces, it is treated as a pattern to match for the filename.
		///   Therefore a string like "*.xls" will be equivalent to specifying "name
		///   = *.xls".  This "shorthand" notation does not work with compound
		///   criteria.
		/// </para>
		///
		/// <para>
		///   There is no logic in this class that insures that the inclusion
		///   criteria are internally consistent.  For example, it's possible to
		///   specify criteria that says the file must have a size of less than 100
		///   bytes, as well as a size that is greater than 1000 bytes.  Obviously
		///   no file will ever satisfy such criteria, but this class does not check
		///   for or detect such inconsistencies.
		/// </para>
		///
		/// </remarks>
		///
		/// <exception cref="T:System.Exception">
		///   Thrown in the setter if the value has an invalid syntax.
		/// </exception>
		public string SelectionCriteria
		{
			get
			{
				if (_Criterion == null)
				{
					return null;
				}
				return _Criterion.ToString();
			}
			set
			{
				if (value == null)
				{
					_Criterion = null;
				}
				else if (value.Trim() == "")
				{
					_Criterion = null;
				}
				else
				{
					_Criterion = _ParseCriterion(value);
				}
			}
		}

		/// <summary>
		///  Indicates whether searches will traverse NTFS reparse points, like Junctions.
		/// </summary>
		public bool TraverseReparsePoints { get; set; }

		/// <summary>
		///   Constructor that allows the caller to specify file selection criteria.
		/// </summary>
		///
		/// <remarks>
		/// <para>
		///   This constructor allows the caller to specify a set of criteria for
		///   selection of files.
		/// </para>
		///
		/// <para>
		///   See <see cref="P:Ionic.FileSelector.SelectionCriteria" /> for a description of
		///   the syntax of the selectionCriteria string.
		/// </para>
		///
		/// <para>
		///   By default the FileSelector will traverse NTFS Reparse Points.  To
		///   change this, use <see cref="M:Ionic.FileSelector.#ctor(System.String,System.Boolean)">FileSelector(String, bool)</see>.
		/// </para>
		/// </remarks>
		///
		/// <param name="selectionCriteria">The criteria for file selection.</param>
		public FileSelector(string selectionCriteria)
			: this(selectionCriteria, true)
		{
		}

		/// <summary>
		///   Constructor that allows the caller to specify file selection criteria.
		/// </summary>
		///
		/// <remarks>
		/// <para>
		///   This constructor allows the caller to specify a set of criteria for
		///   selection of files.
		/// </para>
		///
		/// <para>
		///   See <see cref="P:Ionic.FileSelector.SelectionCriteria" /> for a description of
		///   the syntax of the selectionCriteria string.
		/// </para>
		/// </remarks>
		///
		/// <param name="selectionCriteria">The criteria for file selection.</param>
		/// <param name="traverseDirectoryReparsePoints">
		/// whether to traverse NTFS reparse points (junctions).
		/// </param>
		public FileSelector(string selectionCriteria, bool traverseDirectoryReparsePoints)
		{
			if (!string.IsNullOrEmpty(selectionCriteria))
			{
				_Criterion = _ParseCriterion(selectionCriteria);
			}
			TraverseReparsePoints = traverseDirectoryReparsePoints;
		}

		private static string NormalizeCriteriaExpression(string source)
		{
			string[][] array = new string[11][]
			{
				new string[2] { "([^']*)\\(\\(([^']+)", "$1( ($2" },
				new string[2] { "(.)\\)\\)", "$1) )" },
				new string[2] { "\\(([^'\\f\\n\\r\\t\\v\\x85\\p{Z}])", "( $1" },
				new string[2] { "(\\S)\\)", "$1 )" },
				new string[2] { "^\\)", " )" },
				new string[2] { "(\\S)\\(", "$1 (" },
				new string[2] { "\\)([^'\\f\\n\\r\\t\\v\\x85\\p{Z}])", ") $1" },
				new string[2] { "(=)('[^']*')", "$1 $2" },
				new string[2] { "([^ !><])(>|<|!=|=)", "$1 $2" },
				new string[2] { "(>|<|!=|=)([^ =])", "$1 $2" },
				new string[2] { "/", "\\" }
			};
			string input = source;
			for (int i = 0; i < array.Length; i++)
			{
				string pattern = RegexAssertions.PrecededByEvenNumberOfSingleQuotes + array[i][0] + RegexAssertions.FollowedByEvenNumberOfSingleQuotesAndLineEnd;
				input = Regex.Replace(input, pattern, array[i][1]);
			}
			string pattern2 = "/" + RegexAssertions.FollowedByOddNumberOfSingleQuotesAndLineEnd;
			input = Regex.Replace(input, pattern2, "\\");
			pattern2 = " " + RegexAssertions.FollowedByOddNumberOfSingleQuotesAndLineEnd;
			return Regex.Replace(input, pattern2, "\u0006");
		}

		private static SelectionCriterion _ParseCriterion(string s)
		{
			if (s == null)
			{
				return null;
			}
			s = NormalizeCriteriaExpression(s);
			if (s.IndexOf(" ") == -1)
			{
				s = "name = " + s;
			}
			string[] array = s.Trim().Split(' ', '\t');
			if (array.Length < 3)
			{
				throw new ArgumentException(s);
			}
			SelectionCriterion selectionCriterion = null;
			LogicalConjunction logicalConjunction = LogicalConjunction.NONE;
			Stack<ParseState> stack = new Stack<ParseState>();
			Stack<SelectionCriterion> stack2 = new Stack<SelectionCriterion>();
			stack.Push(ParseState.Start);
			for (int i = 0; i < array.Length; i++)
			{
				string text = array[i].ToLower();
				ParseState parseState;
				switch (text)
				{
				case "and":
				case "xor":
				case "or":
					parseState = stack.Peek();
					if (parseState != ParseState.CriterionDone)
					{
						throw new ArgumentException(string.Join(" ", array, i, array.Length - i));
					}
					if (array.Length <= i + 3)
					{
						throw new ArgumentException(string.Join(" ", array, i, array.Length - i));
					}
					logicalConjunction = (LogicalConjunction)Enum.Parse(typeof(LogicalConjunction), array[i].ToUpper(), true);
					selectionCriterion = new CompoundCriterion
					{
						Left = selectionCriterion,
						Right = null,
						Conjunction = logicalConjunction
					};
					stack.Push(parseState);
					stack.Push(ParseState.ConjunctionPending);
					stack2.Push(selectionCriterion);
					break;
				case "(":
					parseState = stack.Peek();
					if (parseState != 0 && parseState != ParseState.ConjunctionPending && parseState != ParseState.OpenParen)
					{
						throw new ArgumentException(string.Join(" ", array, i, array.Length - i));
					}
					if (array.Length <= i + 4)
					{
						throw new ArgumentException(string.Join(" ", array, i, array.Length - i));
					}
					stack.Push(ParseState.OpenParen);
					break;
				case ")":
					parseState = stack.Pop();
					if (stack.Peek() != ParseState.OpenParen)
					{
						throw new ArgumentException(string.Join(" ", array, i, array.Length - i));
					}
					stack.Pop();
					stack.Push(ParseState.CriterionDone);
					break;
				case "atime":
				case "ctime":
				case "mtime":
				{
					if (array.Length <= i + 2)
					{
						throw new ArgumentException(string.Join(" ", array, i, array.Length - i));
					}
					DateTime value;
					try
					{
						value = DateTime.ParseExact(array[i + 2], "yyyy-MM-dd-HH:mm:ss", null);
					}
					catch (FormatException)
					{
						try
						{
							value = DateTime.ParseExact(array[i + 2], "yyyy/MM/dd-HH:mm:ss", null);
						}
						catch (FormatException)
						{
							try
							{
								value = DateTime.ParseExact(array[i + 2], "yyyy/MM/dd", null);
								goto end_IL_0497;
							}
							catch (FormatException)
							{
								try
								{
									value = DateTime.ParseExact(array[i + 2], "MM/dd/yyyy", null);
									goto end_IL_0497;
								}
								catch (FormatException)
								{
									value = DateTime.ParseExact(array[i + 2], "yyyy-MM-dd", null);
									goto end_IL_0497;
								}
							}
							end_IL_0497:;
						}
					}
					value = DateTime.SpecifyKind(value, DateTimeKind.Local).ToUniversalTime();
					selectionCriterion = new TimeCriterion
					{
						Which = (WhichTime)Enum.Parse(typeof(WhichTime), array[i], true),
						Operator = (ComparisonOperator)EnumUtil.Parse(typeof(ComparisonOperator), array[i + 1]),
						Time = value
					};
					i += 2;
					stack.Push(ParseState.CriterionDone);
					break;
				}
				case "length":
				case "size":
				{
					if (array.Length <= i + 2)
					{
						throw new ArgumentException(string.Join(" ", array, i, array.Length - i));
					}
					long num = 0L;
					string text2 = array[i + 2];
					num = (text2.ToUpper().EndsWith("K") ? (long.Parse(text2.Substring(0, text2.Length - 1)) * 1024) : (text2.ToUpper().EndsWith("KB") ? (long.Parse(text2.Substring(0, text2.Length - 2)) * 1024) : (text2.ToUpper().EndsWith("M") ? (long.Parse(text2.Substring(0, text2.Length - 1)) * 1024 * 1024) : (text2.ToUpper().EndsWith("MB") ? (long.Parse(text2.Substring(0, text2.Length - 2)) * 1024 * 1024) : (text2.ToUpper().EndsWith("G") ? (long.Parse(text2.Substring(0, text2.Length - 1)) * 1024 * 1024 * 1024) : ((!text2.ToUpper().EndsWith("GB")) ? long.Parse(array[i + 2]) : (long.Parse(text2.Substring(0, text2.Length - 2)) * 1024 * 1024 * 1024)))))));
					selectionCriterion = new SizeCriterion
					{
						Size = num,
						Operator = (ComparisonOperator)EnumUtil.Parse(typeof(ComparisonOperator), array[i + 1])
					};
					i += 2;
					stack.Push(ParseState.CriterionDone);
					break;
				}
				case "filename":
				case "name":
				{
					if (array.Length <= i + 2)
					{
						throw new ArgumentException(string.Join(" ", array, i, array.Length - i));
					}
					ComparisonOperator comparisonOperator2 = (ComparisonOperator)EnumUtil.Parse(typeof(ComparisonOperator), array[i + 1]);
					if (comparisonOperator2 != ComparisonOperator.NotEqualTo && comparisonOperator2 != ComparisonOperator.EqualTo)
					{
						throw new ArgumentException(string.Join(" ", array, i, array.Length - i));
					}
					string text3 = array[i + 2];
					if (text3.StartsWith("'") && text3.EndsWith("'"))
					{
						text3 = text3.Substring(1, text3.Length - 2).Replace("\u0006", " ");
					}
					if (Path.DirectorySeparatorChar == '/')
					{
						text3 = text3.Replace('\\', Path.DirectorySeparatorChar);
					}
					selectionCriterion = new NameCriterion
					{
						MatchingFileSpec = text3,
						Operator = comparisonOperator2
					};
					i += 2;
					stack.Push(ParseState.CriterionDone);
					break;
				}
				case "attrs":
				case "attributes":
				case "type":
				{
					if (array.Length <= i + 2)
					{
						throw new ArgumentException(string.Join(" ", array, i, array.Length - i));
					}
					ComparisonOperator comparisonOperator = (ComparisonOperator)EnumUtil.Parse(typeof(ComparisonOperator), array[i + 1]);
					if (comparisonOperator != ComparisonOperator.NotEqualTo && comparisonOperator != ComparisonOperator.EqualTo)
					{
						throw new ArgumentException(string.Join(" ", array, i, array.Length - i));
					}
					selectionCriterion = ((text == "type") ? ((SelectionCriterion)new TypeCriterion
					{
						AttributeString = array[i + 2],
						Operator = comparisonOperator
					}) : ((SelectionCriterion)new AttributesCriterion
					{
						AttributeString = array[i + 2],
						Operator = comparisonOperator
					}));
					i += 2;
					stack.Push(ParseState.CriterionDone);
					break;
				}
				case "":
					stack.Push(ParseState.Whitespace);
					break;
				default:
					throw new ArgumentException("'" + array[i] + "'");
				}
				parseState = stack.Peek();
				if (parseState == ParseState.CriterionDone)
				{
					stack.Pop();
					if (stack.Peek() == ParseState.ConjunctionPending)
					{
						while (stack.Peek() == ParseState.ConjunctionPending)
						{
							CompoundCriterion obj = stack2.Pop() as CompoundCriterion;
							obj.Right = selectionCriterion;
							selectionCriterion = obj;
							stack.Pop();
							parseState = stack.Pop();
							if (parseState != ParseState.CriterionDone)
							{
								throw new ArgumentException("??");
							}
						}
					}
					else
					{
						stack.Push(ParseState.CriterionDone);
					}
				}
				if (parseState == ParseState.Whitespace)
				{
					stack.Pop();
				}
			}
			return selectionCriterion;
		}

		/// <summary>
		/// Returns a string representation of the FileSelector object.
		/// </summary>
		/// <returns>The string representation of the boolean logic statement of the file
		/// selection criteria for this instance. </returns>
		public override string ToString()
		{
			return "FileSelector(" + _Criterion.ToString() + ")";
		}

		private bool Evaluate(string filename)
		{
			return _Criterion.Evaluate(filename);
		}

		[Conditional("SelectorTrace")]
		private void SelectorTrace(string format, params object[] args)
		{
			if (_Criterion != null && _Criterion.Verbose)
			{
				Console.WriteLine(format, args);
			}
		}

		/// <summary>
		///   Returns the names of the files in the specified directory
		///   that fit the selection criteria specified in the FileSelector.
		/// </summary>
		///
		/// <remarks>
		///   This is equivalent to calling <see cref="M:Ionic.FileSelector.SelectFiles(System.String,System.Boolean)" />
		///   with recurseDirectories = false.
		/// </remarks>
		///
		/// <param name="directory">
		///   The name of the directory over which to apply the FileSelector
		///   criteria.
		/// </param>
		///
		/// <returns>
		///   A collection of strings containing fully-qualified pathnames of files
		///   that match the criteria specified in the FileSelector instance.
		/// </returns>
		public ICollection<string> SelectFiles(string directory)
		{
			return SelectFiles(directory, false);
		}

		/// <summary>
		///   Returns the names of the files in the specified directory that fit the
		///   selection criteria specified in the FileSelector, optionally recursing
		///   through subdirectories.
		/// </summary>
		///
		/// <remarks>
		///   This method applies the file selection criteria contained in the
		///   FileSelector to the files contained in the given directory, and
		///   returns the names of files that conform to the criteria.
		/// </remarks>
		///
		/// <param name="directory">
		///   The name of the directory over which to apply the FileSelector
		///   criteria.
		/// </param>
		///
		/// <param name="recurseDirectories">
		///   Whether to recurse through subdirectories when applying the file
		///   selection criteria.
		/// </param>
		///
		/// <returns>
		///   A collection of strings containing fully-qualified pathnames of files
		///   that match the criteria specified in the FileSelector instance.
		/// </returns>
		public ReadOnlyCollection<string> SelectFiles(string directory, bool recurseDirectories)
		{
			if (_Criterion == null)
			{
				throw new ArgumentException("SelectionCriteria has not been set");
			}
			List<string> list = new List<string>();
			try
			{
				if (Directory.Exists(directory))
				{
					string[] files = Directory.GetFiles(directory);
					foreach (string text in files)
					{
						if (Evaluate(text))
						{
							list.Add(text);
						}
					}
					if (recurseDirectories)
					{
						files = Directory.GetDirectories(directory);
						foreach (string text2 in files)
						{
							if (TraverseReparsePoints || (File.GetAttributes(text2) & FileAttributes.ReparsePoint) == 0)
							{
								if (Evaluate(text2))
								{
									list.Add(text2);
								}
								list.AddRange(SelectFiles(text2, recurseDirectories));
							}
						}
					}
				}
			}
			catch (UnauthorizedAccessException)
			{
			}
			catch (IOException)
			{
			}
			return list.AsReadOnly();
		}

		private bool Evaluate(ZipEntry entry)
		{
			return _Criterion.Evaluate(entry);
		}

		/// <summary>
		/// Retrieve the ZipEntry items in the ZipFile that conform to the specified criteria.
		/// </summary>
		/// <remarks>
		///
		/// <para>
		/// This method applies the criteria set in the FileSelector instance (as described in
		/// the <see cref="P:Ionic.FileSelector.SelectionCriteria" />) to the specified ZipFile.  Using this
		/// method, for example, you can retrieve all entries from the given ZipFile that
		/// have filenames ending in .txt.
		/// </para>
		///
		/// <para>
		/// Normally, applications would not call this method directly.  This method is used
		/// by the ZipFile class.
		/// </para>
		///
		/// <para>
		/// Using the appropriate SelectionCriteria, you can retrieve entries based on size,
		/// time, and attributes. See <see cref="P:Ionic.FileSelector.SelectionCriteria" /> for a
		/// description of the syntax of the SelectionCriteria string.
		/// </para>
		///
		/// </remarks>
		///
		/// <param name="zip">The ZipFile from which to retrieve entries.</param>
		///
		/// <returns>a collection of ZipEntry objects that conform to the criteria.</returns>
		public ICollection<ZipEntry> SelectEntries(ZipFile zip)
		{
			if (zip == null)
			{
				throw new ArgumentNullException("zip");
			}
			List<ZipEntry> list = new List<ZipEntry>();
			foreach (ZipEntry item in zip)
			{
				if (Evaluate(item))
				{
					list.Add(item);
				}
			}
			return list;
		}

		/// <summary>
		/// Retrieve the ZipEntry items in the ZipFile that conform to the specified criteria.
		/// </summary>
		/// <remarks>
		///
		/// <para>
		/// This method applies the criteria set in the FileSelector instance (as described in
		/// the <see cref="P:Ionic.FileSelector.SelectionCriteria" />) to the specified ZipFile.  Using this
		/// method, for example, you can retrieve all entries from the given ZipFile that
		/// have filenames ending in .txt.
		/// </para>
		///
		/// <para>
		/// Normally, applications would not call this method directly.  This method is used
		/// by the ZipFile class.
		/// </para>
		///
		/// <para>
		/// This overload allows the selection of ZipEntry instances from the ZipFile to be restricted
		/// to entries contained within a particular directory in the ZipFile.
		/// </para>
		///
		/// <para>
		/// Using the appropriate SelectionCriteria, you can retrieve entries based on size,
		/// time, and attributes. See <see cref="P:Ionic.FileSelector.SelectionCriteria" /> for a
		/// description of the syntax of the SelectionCriteria string.
		/// </para>
		///
		/// </remarks>
		///
		/// <param name="zip">The ZipFile from which to retrieve entries.</param>
		///
		/// <param name="directoryPathInArchive">
		/// the directory in the archive from which to select entries. If null, then
		/// all directories in the archive are used.
		/// </param>
		///
		/// <returns>a collection of ZipEntry objects that conform to the criteria.</returns>
		public ICollection<ZipEntry> SelectEntries(ZipFile zip, string directoryPathInArchive)
		{
			if (zip == null)
			{
				throw new ArgumentNullException("zip");
			}
			List<ZipEntry> list = new List<ZipEntry>();
			string text = directoryPathInArchive?.Replace("/", "\\");
			if (text != null)
			{
				while (text.EndsWith("\\"))
				{
					text = text.Substring(0, text.Length - 1);
				}
			}
			foreach (ZipEntry item in zip)
			{
				if ((directoryPathInArchive == null || Path.GetDirectoryName(item.FileName) == directoryPathInArchive || Path.GetDirectoryName(item.FileName) == text) && Evaluate(item))
				{
					list.Add(item);
				}
			}
			return list;
		}
	}
}
