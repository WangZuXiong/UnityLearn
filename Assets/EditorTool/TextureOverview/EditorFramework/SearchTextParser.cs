using System;
using System.Collections.Generic;
using System.Text;

namespace EditorFramework
{
	// Token: 0x02000068 RID: 104
	public static class SearchTextParser
	{
		// Token: 0x0600026A RID: 618 RVA: 0x0002CBE4 File Offset: 0x0002ADE4
		public static SearchTextParser.Result Parse(string text)
		{
			SearchTextParser.Result result = new SearchTextParser.Result();
			if (string.IsNullOrEmpty(text))
			{
				return result;
			}
			List<SearchTextParser.ResultExpr> namesExpr = result.NamesExpr;
			List<string> types = result.Types;
			List<string> labels = result.Labels;
			StringBuilder stringBuilder = new StringBuilder(64);
			int num = 0;
			SearchTextParser.LogicalOperator logicalOperator = SearchTextParser.LogicalOperator.And;
			bool not = false;
			bool exact = false;
			int num2 = 0;
			while (num2 < text.Length && ++num <= 10000)
			{
				SearchTextParser.SkipWhiteSpace(text, ref num2);
				if (num2 + 1 < text.Length)
				{
					if (text[num2] == '&' && text[num2 + 1] == '&')
					{
						num2 += 2;
						logicalOperator = SearchTextParser.LogicalOperator.And;
						exact = false;
						continue;
					}
					if (text[num2] == '|' && text[num2 + 1] == '|')
					{
						num2 += 2;
						logicalOperator = SearchTextParser.LogicalOperator.Or;
						exact = false;
						continue;
					}
					if (text[num2] == '=' && text[num2 + 1] == '=')
					{
						num2 += 2;
						exact = true;
						continue;
					}
					if (text[num2] == '!')
					{
						num2++;
						not = true;
						continue;
					}
					if (text[num2] == '"' || text[num2] == '„')
					{
						SearchTextParser.GetNextQuotedWord(text, ref num2, stringBuilder);
						if (stringBuilder.Length > 0)
						{
							if (namesExpr.Count > 0 && logicalOperator == SearchTextParser.LogicalOperator.Or)
							{
								namesExpr[namesExpr.Count - 1].Op = SearchTextParser.LogicalOperator.Or;
							}
							namesExpr.Add(new SearchTextParser.ResultExpr
							{
								Op = logicalOperator,
								Text = stringBuilder.ToString().Trim(),
								Not = not,
								Exact = exact
							});
							stringBuilder.Length = 0;
							not = false;
							logicalOperator = SearchTextParser.LogicalOperator.And;
							continue;
						}
						continue;
					}
					else if (text[num2 + 1] == ':')
					{
						if (text[num2] == 't' || text[num2] == 'T')
						{
							num2 += 2;
							SearchTextParser.SkipWhiteSpace(text, ref num2);
							SearchTextParser.GetNextWord(text, ref num2, stringBuilder);
							if (stringBuilder.Length > 0)
							{
								string text2 = stringBuilder.ToString().Trim();
								if (string.Compare(text2, "prefab", StringComparison.OrdinalIgnoreCase) == 0)
								{
									text2 = "GameObject";
								}
								types.Add(text2);
								stringBuilder.Length = 0;
								continue;
							}
						}
						else if (text[num2] == 'l' || text[num2] == 'L')
						{
							num2 += 2;
							SearchTextParser.SkipWhiteSpace(text, ref num2);
							SearchTextParser.GetNextWord(text, ref num2, stringBuilder);
							if (stringBuilder.Length > 0)
							{
								labels.Add(stringBuilder.ToString().Trim());
								stringBuilder.Length = 0;
								continue;
							}
						}
					}
				}
				SearchTextParser.GetNextWord(text, ref num2, stringBuilder);
				if (stringBuilder.Length > 0)
				{
					if (namesExpr.Count > 0 && logicalOperator == SearchTextParser.LogicalOperator.Or)
					{
						namesExpr[namesExpr.Count - 1].Op = SearchTextParser.LogicalOperator.Or;
					}
					namesExpr.Add(new SearchTextParser.ResultExpr
					{
						Op = logicalOperator,
						Text = stringBuilder.ToString().Trim(),
						Not = not,
						Exact = exact
					});
					stringBuilder.Length = 0;
					not = false;
					logicalOperator = SearchTextParser.LogicalOperator.And;
				}
			}
			foreach (SearchTextParser.ResultExpr resultExpr in result.NamesExpr)
			{
				result.Names.Add(resultExpr.Text);
			}
			List<SearchTextParser.ResultExpr> list = new List<SearchTextParser.ResultExpr>();
			foreach (SearchTextParser.ResultExpr resultExpr2 in result.NamesExpr)
			{
				if (resultExpr2.Op == SearchTextParser.LogicalOperator.And)
				{
					list.Add(resultExpr2);
				}
			}
			foreach (SearchTextParser.ResultExpr resultExpr3 in result.NamesExpr)
			{
				if (resultExpr3.Op == SearchTextParser.LogicalOperator.Or)
				{
					list.Add(resultExpr3);
				}
			}
			result.NamesExpr = list;
			return result;
		}

		// Token: 0x0600026B RID: 619 RVA: 0x0002D00C File Offset: 0x0002B20C
		private static void SkipWhiteSpace(string text, ref int index)
		{
			int num = 0;
			while (index < text.Length)
			{
				char c = text[index];
				if (!char.IsWhiteSpace(c))
				{
					return;
				}
				index++;
				if (++num > 10000)
				{
					return;
				}
			}
		}

		// Token: 0x0600026C RID: 620 RVA: 0x0002D04C File Offset: 0x0002B24C
		private static void GetNextWord(string text, ref int index, StringBuilder builder)
		{
			int num = 0;
			while (index < text.Length)
			{
				char c = text[index];
				if (char.IsWhiteSpace(c))
				{
					return;
				}
				builder.Append(c);
				index++;
				if (++num > 10000)
				{
					return;
				}
			}
		}

		// Token: 0x0600026D RID: 621 RVA: 0x0002D094 File Offset: 0x0002B294
		private static void GetNextQuotedWord(string text, ref int index, StringBuilder builder)
		{
			int num = 0;
			index++;
			while (index < text.Length)
			{
				char c = text[index];
				if (c == '"' || c == '“')
				{
					index++;
					return;
				}
				builder.Append(c);
				index++;
				if (++num > 10000)
				{
					return;
				}
			}
		}

		// Token: 0x02000069 RID: 105
		internal enum LogicalOperator
		{
			// Token: 0x040001A2 RID: 418
			And,
			// Token: 0x040001A3 RID: 419
			Or
		}

		// Token: 0x0200006A RID: 106
		internal class ResultExpr
		{
			// Token: 0x040001A4 RID: 420
			public SearchTextParser.LogicalOperator Op;

			// Token: 0x040001A5 RID: 421
			public bool Not;

			// Token: 0x040001A6 RID: 422
			public bool Exact;

			// Token: 0x040001A7 RID: 423
			public string Text;
		}

		// Token: 0x0200006B RID: 107
		public class Result
		{
			// Token: 0x0600026F RID: 623 RVA: 0x0002D0F8 File Offset: 0x0002B2F8
			public bool IsNameMatch(string text)
			{
				if (this.NamesExpr.Count == 0)
				{
					return true;
				}
				if (text == null)
				{
					return false;
				}
				bool flag = false;
				bool flag2 = true;
				for (int i = 0; i < this.NamesExpr.Count; i++)
				{
					SearchTextParser.ResultExpr resultExpr = this.NamesExpr[i];
					if (resultExpr.Op == SearchTextParser.LogicalOperator.And)
					{
						if (resultExpr.Not)
						{
							if (!resultExpr.Exact && text.IndexOf(resultExpr.Text, StringComparison.OrdinalIgnoreCase) != -1)
							{
								return false;
							}
							if (resultExpr.Exact && text.Equals(resultExpr.Text, StringComparison.OrdinalIgnoreCase))
							{
								return false;
							}
						}
						else
						{
							if (!resultExpr.Exact && text.IndexOf(resultExpr.Text, StringComparison.OrdinalIgnoreCase) == -1)
							{
								return false;
							}
							if (resultExpr.Exact && !text.Equals(resultExpr.Text, StringComparison.OrdinalIgnoreCase))
							{
								return false;
							}
						}
					}
					else if (resultExpr.Op == SearchTextParser.LogicalOperator.Or)
					{
						if (resultExpr.Not)
						{
							if (!resultExpr.Exact && text.IndexOf(resultExpr.Text, StringComparison.OrdinalIgnoreCase) == -1)
							{
								flag = true;
							}
							if (resultExpr.Exact && !text.Equals(resultExpr.Text, StringComparison.OrdinalIgnoreCase))
							{
								flag = true;
							}
						}
						else
						{
							if (!resultExpr.Exact && text.IndexOf(resultExpr.Text, StringComparison.OrdinalIgnoreCase) != -1)
							{
								flag = true;
							}
							if (resultExpr.Exact && text.Equals(resultExpr.Text, StringComparison.OrdinalIgnoreCase))
							{
								flag = true;
							}
						}
						flag2 = false;
					}
				}
				return flag || flag2;
			}

			// Token: 0x040001A8 RID: 424
			internal List<SearchTextParser.ResultExpr> NamesExpr = new List<SearchTextParser.ResultExpr>();

			// Token: 0x040001A9 RID: 425
			public List<string> Names = new List<string>();

			// Token: 0x040001AA RID: 426
			public List<string> Types = new List<string>();

			// Token: 0x040001AB RID: 427
			public List<string> Labels = new List<string>();
		}
	}
}
