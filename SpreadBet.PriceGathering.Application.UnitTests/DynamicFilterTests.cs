using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using System.Text.RegularExpressions;

namespace SpreadBet.PriceGathering.Application.UnitTests
{
	[TestFixture]
	public class DynamicFilterTests
	{
		[Test]
		public void Evaluate_WithValidExpression_ReturnsTrue()
		{
			var dynamicFilter = new DynamicStockFilter("Stock.Identifier.ToUpper().SubString(0,1) = \"A\"");

			var stock = new SpreadBet.Domain.Stock
			{
				Identifier = "ABC.L"
			};

			var result = dynamicFilter.Evaluate(stock);

			Assert.IsTrue(result);
		}

		[Test]
		public void Evaluate_ExpressionDoesNotContainStockReference_ResolvesStockProperty()
		{
			var dynamicFilter = new DynamicStockFilter("Identifier == \"ABC.L\"");

			var stock = new SpreadBet.Domain.Stock
			{
				Identifier = "ABC.L"
			};

			var result = dynamicFilter.Evaluate(stock);

			Assert.IsTrue(result);
		}

		[Test]
		public void Evaluate_WithRegexInExpression_ReturnsTrue()
		{
			var dynamicFilter = new DynamicStockFilter("Regex.IsMatch(Stock.Identifier, \"(?i)^[ABC]\")");
			var stock = new SpreadBet.Domain.Stock
			{
				Identifier = "ABC.L"
			};

			var result = dynamicFilter.Evaluate(stock);

			Assert.IsTrue(result);
		}

		[Test]
		public void Evaluate_WithRegexInExpression_ReturnsFalse()
		{
			var dynamicFilter = new DynamicStockFilter("Regex.IsMatch(Stock.Identifier, \"(?i)^[ABC]\")");
			var stock = new SpreadBet.Domain.Stock
			{
				Identifier = "XYZ.L"
			};

			var result = dynamicFilter.Evaluate(stock);

			Assert.IsFalse(result);
		}
	}
}
