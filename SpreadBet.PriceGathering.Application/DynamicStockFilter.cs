// -----------------------------------------------------------------------
// <copyright file="DynamicStockFilter.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace SpreadBet.PriceGathering.Application
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using SpreadBet.Domain;
	using Ciloci.Flee;
	using System.Text.RegularExpressions;
	using CuttingEdge.Conditions;

	public class DynamicStockFilter : IPipelineFilter
	{
		private readonly string _expression;

		public DynamicStockFilter(string expression)
		{
			Condition.Requires(expression).IsNotNullOrEmpty();

			// Replace some common c# operators with Flee operators
			var cleanedExpression = expression
										.Replace("&&", " AND ")
										.Replace("==", "=")
										.Replace("!=", "<>");

			this._expression = cleanedExpression;
		}

		public bool Evaluate(Stock stock)
		{
			var expressionContext = new ExpressionContext();

			expressionContext.Imports.AddType(typeof(Regex), "Regex");

			var variables = expressionContext.Variables;
			variables.Add("Stock", stock);

			variables.ResolveVariableType += new EventHandler<ResolveVariableTypeEventArgs>((sender, e) =>
			{
				var propertyInfo = typeof(Stock).GetProperty(e.VariableName);
				if (propertyInfo != null)
				{
					e.VariableType = propertyInfo.PropertyType;
				}
			});

			variables.ResolveVariableValue += new EventHandler<ResolveVariableValueEventArgs>((sender, e) =>
			{
				var propertyInfo = typeof(Stock).GetProperty(e.VariableName);
				if (propertyInfo != null)
				{
					e.VariableValue = Convert.ChangeType(propertyInfo.GetValue(stock, null), e.VariableType);
				}
			});

			var expression = expressionContext.CompileDynamic(this._expression);

			var retVal = (bool)expression.Evaluate();

			return retVal;
		}
	}
}
