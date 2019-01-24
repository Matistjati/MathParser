using System;
using System.Collections.Generic;
using System.Linq;

namespace MathExpressionParser
{
	enum Operator
	{
		badValue = 0,
		none = 1,
		addition = 2,
		subtraction = 3,
		multiplication = 4,
		division = 5,
		modulo = 6,
		potencies = 7,
		sqrt = 8,
		sin = 9,
		cos = 10,
		tan = 11,
	}

	class Program
	{
		static readonly int[] operatorMap =
		{
			-1, -1, // badValue and none
			0, 0,   // Addition and subtraction
			1, 1,   // Multiplication and division
			1,      // Modulo
			2, -1,  // Potencies and sqrt
			-1, -1, // sin and cos
			-1,     // tan
		};

		static void Main()
		{
			Console.WriteLine("Enter a mathematical expression");
			string expression = Console.ReadLine();

			float outPut = 0;

			List<Expression> expressions = new List<Expression>();

			int parenthesisLevel = 0;

			float currentNumber = 0;
			Operator currentOperator = Operator.none;


			for (int i = 0; i < expression.Length; i++)
			{
				switch (expression[i])
				{
					case '(':
						parenthesisLevel++;
						break;

					case ')':
						parenthesisLevel--;
						break;

					case '+':
						currentOperator = OperatorCheck(currentOperator, Operator.addition);
						break;

					case '-':
						currentOperator = OperatorCheck(currentOperator, Operator.subtraction);
						break;

					case '*':
						currentOperator = OperatorCheck(currentOperator, Operator.multiplication);
						break;

					case '/':
						currentOperator = OperatorCheck(currentOperator, Operator.division);
						break;

					case '%':
						currentOperator = OperatorCheck(currentOperator, Operator.modulo);
						break;

					case '^':
						currentOperator = OperatorCheck(currentOperator, Operator.potencies);
						break;

					case 's':
					case 'S':
						Operator sOperator = NamedOperatorCheck(ref i, expression, "sqrt");
						if (sOperator == Operator.badValue)
						{
							sOperator = NamedOperatorCheck(ref i, expression, "sin");
							if (sOperator == Operator.badValue)
							{
								break;
							}
						}

						if (sOperator != Operator.badValue)
						{
							expressions.Add(new Expression
							{
								operatorType = sOperator,
								parenthesisLevel = parenthesisLevel,
								firstOperand = null,
								secondOperand = null
							});
						}

						break;

					case 'c':
					case 'C':
						Operator cOperator = NamedOperatorCheck(ref i, expression, "cos");

						if (cOperator != Operator.badValue)
						{
							expressions.Add(new Expression
							{
								operatorType = cOperator,
								parenthesisLevel = parenthesisLevel,
								firstOperand = null,
								secondOperand = null
							});
						}

						break;

					case 't':
					case 'T':
						Operator tOperator = NamedOperatorCheck(ref i, expression, "tan");

						if (tOperator != Operator.badValue)
						{
							expressions.Add(new Expression
							{
								operatorType = tOperator,
								parenthesisLevel = parenthesisLevel,
								firstOperand = null,
								secondOperand = null
							});
						}

						break;

					case '0':
					case '1':
					case '2':
					case '3':
					case '4':
					case '5':
					case '6':
					case '7':
					case '8':
					case '9':
						if (currentNumber != 0 && currentOperator == Operator.none)
						{
							Error("currentnumber was 0 with operator none");
						}

						float localnumber = (i + 1 < expression.Length && expression[i + 1] == '.')
							? GetFloat(ref i, expression)
							: GetInt(ref i, expression);

						if (currentNumber != 0 && currentOperator != Operator.none)
						{
							expressions.Add(new Expression
							{
								operatorType = currentOperator,
								parenthesisLevel = parenthesisLevel,
								firstOperand = currentNumber,
								secondOperand = localnumber
							});

							currentNumber = 0;
							currentOperator = Operator.none;
						}
						else if (currentNumber == 0 && currentOperator != Operator.none)
						{
							expressions.Add(new Expression
							{
								operatorType = currentOperator,
								parenthesisLevel = parenthesisLevel,
								firstOperand = localnumber,
								secondOperand = null
							});

							currentNumber = 0;
							currentOperator = Operator.none;
						}
						else
						{
							currentNumber = localnumber;
						}

						break;

					default:
						break;
				}
			}

			for (int i = 0; i < expressions.Count; i++)
			{
				Expression current = expressions[i];

				if (current.CanDoOperationAlone() && current.operatorType != Operator.sqrt)
				{
					for (int j = 0; j < expressions.Count; j++)
					{
						Expression other = expressions[j];

						if (current.parenthesisLevel != other.parenthesisLevel)
							continue;

						if (operatorMap[(int)current.operatorType] < operatorMap[(int)other.operatorType])
						{
							if (other.firstOperand is null)
							{
								other.firstOperand = current.firstOperand;
								current.secondOperand = null;
							}
							else if (other.secondOperand is null)
							{
								other.secondOperand = current.secondOperand;
								current.secondOperand = null;
							}
						}
					}
				}
			}

			expressions = expressions
			   .OrderByDescending(x => operatorMap[(int)(x.operatorType)])
			   .ToList();

			expressions = expressions
			   .OrderByDescending(x => x.parenthesisLevel)
			   .ToList();

			for (int i = 0; i < expressions.Count; i++)
			{
				Expression current = expressions[i];

				if (current.CanDoOperationAlone())
				{
					outPut += (float)current.PerformOperation();
				}
				else
				{
					if (current.operatorType == Operator.sqrt ||
						current.operatorType == Operator.sin ||
						current.operatorType == Operator.cos ||
						current.operatorType == Operator.tan)
					{
						outPut = (float)current.PerformOperation(outPut, true);
					}
					else if (current.firstOperand is null && current.secondOperand is null)
					{

					}
					else if (current.firstOperand is null)
					{
						outPut = (float)current.PerformOperation(outPut, true);
					}
					else if (current.secondOperand is null)
					{
						outPut = (float)current.PerformOperation(outPut, false);
					}
				}
			}

			Console.WriteLine(outPut);
			Console.ReadLine();
		}

		static Operator OperatorCheck(Operator currentOperator, Operator newOperator)
		{
			if (currentOperator == Operator.none)
			{
				return newOperator;
			}
			else
			{
				Error($"currentOperator isn't none, is {currentOperator}. Newoperator is {newOperator}");
				return Operator.badValue;
			}
		}

		private static float GetInt(ref int index, string expression)
		{
			string number = "";

			int i = index;

			for (; i < expression.Length; i++)
			{
				if (IsNumber(expression[i]))
				{
					number += expression[i];
				}
				else
					break;
			}

			index += i - index - 1;
			return float.Parse(number);
		}

		static readonly Dictionary<string, Operator> stringToOperator = new Dictionary<string, Operator>
		{
			{"SQRT", Operator.sqrt },
			{"SIN", Operator.sin },
			{"COS", Operator.cos },
			{"TAN", Operator.tan },
		};

		static Operator NamedOperatorCheck(ref int index, string expression, string operatorName)
		{
			operatorName = operatorName.ToUpper();

			for (int i = 0; i < operatorName.Length; i++)
			{
				if (char.ToUpper(expression[index + i]) != operatorName[i])
				{
					return Operator.badValue;
				}
			}


			index += operatorName.Length;
			return stringToOperator[operatorName];
		}

		static float GetFloat(ref int index, string expression)
		{
			string number = "";
			int i = index;
			if (IsNumber(expression[i]))
			{
				number += expression[i];
				i++;
			}
			else
			{
				throw new ArgumentException();
			}

			if (expression[i] == '.')
			{
				number += '.';
				i++;
			}
			else
			{
				throw new ArgumentException();
			}

			for (; i < expression.Length; i++)
			{
				if (IsNumber(expression[i]))
				{
					number += expression[i];
				}
				else if (expression[i] == ' ')
				{
					break;
				}
				else
				{
					throw new ArgumentException();
				}
			}

			index += i - index - 1;
			return float.Parse(number);
		}

		static int ToInt(char ch)
		{
			switch (ch)
			{
				case '0':
					return 0;
				case '1':
					return 1;
				case '2':
					return 2;
				case '3':
					return 3;
				case '4':
					return 4;
				case '5':
					return 5;
				case '6':
					return 6;
				case '7':
					return 7;
				case '8':
					return 8;
				case '9':
					return 9;

				default:
					throw new ArgumentException();
			}
		}

		static bool IsNumber(char num)
		{
			switch (num)
			{
				case '0':
				case '1':
				case '2':
				case '3':
				case '4':
				case '5':
				case '6':
				case '7':
				case '8':
				case '9':
					return true;

				default:
					return false;
			}
		}



		static void Error(string message)
		{
			Console.WriteLine(message);
			Console.ReadLine();
			throw new Exception();
		}
	}
}
