using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MathExpressionParser
{
	class Expression
	{
		public Operator operatorType;
		public float? firstOperand;
		public float? secondOperand;
		public int parenthesisLevel;

		public bool CanDoOperationAlone()
		{
			switch (operatorType)
			{
				case Operator.badValue:
					return false;

				case Operator.none:
					return false;

				case Operator.addition:
					return firstOperand != null && secondOperand != null;

				case Operator.subtraction:
					return firstOperand != null && secondOperand != null;

				case Operator.multiplication:
					return firstOperand != null && secondOperand != null;

				case Operator.division:
					return firstOperand != null && secondOperand != null;

				case Operator.potencies:
					return firstOperand != null && secondOperand != null;

				case Operator.sqrt:
					return firstOperand != null || secondOperand != null;

				default:
					return false;
			}
		}

		public float? PerformOperation()
		{
			switch (operatorType)
			{
				case Operator.addition:
					return firstOperand + secondOperand;

				case Operator.subtraction:
					return firstOperand - secondOperand;

				case Operator.multiplication:
					return firstOperand * secondOperand;

				case Operator.division:
					return firstOperand / secondOperand;

				case Operator.potencies:
					return (float)Math.Pow((double)firstOperand, (double)secondOperand);

				default:
					throw new ArgumentException();
			}
		}

		public float? PerformOperation(float operand, bool first)
		{
			if (first)
			{
				firstOperand = operand;
			}
			else
			{
				secondOperand = operand;
			}

			switch (operatorType)
			{
				case Operator.addition:
					return firstOperand + secondOperand;

				case Operator.subtraction:
					return firstOperand - secondOperand;

				case Operator.multiplication:
					return firstOperand * secondOperand;

				case Operator.division:
					return firstOperand / secondOperand;

				case Operator.potencies:
					return (float)Math.Pow((double)firstOperand, (double)secondOperand);

				case Operator.sqrt:
					return (float)Math.Sqrt((double)firstOperand);

				default:
					throw new ArgumentException();
			}
		}

		//float PerformOperationRecursively()
		//{
		//	float value = firstOperand;

		//	switch (numberOperator)
		//	{
		//		case Operator.addition:
		//			value = firstOperand + secondOperand;
		//			break;

		//		case Operator.subtraction:
		//			value = firstOperand - secondOperand;
		//			break;

		//		case Operator.multiplication:
		//			value = firstOperand * secondOperand;
		//			break;

		//		case Operator.division:
		//			value = firstOperand / secondOperand;
		//			break;

		//		case Operator.potencies:
		//			value = (float)Math.Pow(firstOperand, secondOperand);
		//			break;

		//		default:
		//			throw new ArgumentException();
		//	}

		//	if (nextExpression != null)
		//	{
		//		value = nextExpression.PerformOperationRecursively();
		//	}

		//	return value;
		//}

		public override string ToString()
		{
			string operatorString;

			switch (operatorType)
			{
				case Operator.addition:
					operatorString = "+";
					break;
				case Operator.subtraction:
					operatorString = "-";
					break;
				case Operator.multiplication:
					operatorString = "*";
					break;
				case Operator.division:
					operatorString = "/";
					break;
				case Operator.potencies:
					operatorString = "^";
					break;
				case Operator.sqrt:
					return $"sqrt({firstOperand})";

				
				case Operator.none:
				default:
					throw new Exception();
			}

			return $"{firstOperand} {operatorString} {secondOperand}";
		}
	}
}
