// Copyright 2007-2008 The Apache Software Foundation.
//  
// Licensed under the Apache License, Version 2.0 (the "License"); you may not use 
// this file except in compliance with the License. You may obtain a copy of the 
// License at 
// 
//     http://www.apache.org/licenses/LICENSE-2.0 
// 
// Unless required by applicable law or agreed to in writing, software distributed 
// under the License is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR 
// CONDITIONS OF ANY KIND, either express or implied. See the License for the 
// specific language governing permissions and limitations under the License.
namespace Magnum.RulesEngine.ExecutionModel
{
	using System.Text;

	public class StringNodeVisitor :
		AbstractModelVisitor<StringNodeVisitor>
	{
		private readonly StringBuilder _output = new StringBuilder();
		private int _depth;

		public string Result
		{
			get { return _output.ToString(); }
		}

		protected override void IncreaseDepth()
		{
			_depth++;
		}

		protected override void DecreaseDepth()
		{
			_depth--;
		}

		protected bool Visit(TypeDispatchNode node)
		{
			Append("MatchType");

			return true;
		}

		protected bool Visit<T>(TypeNode<T> node)
		{
			Append("ConditionTree<{0}>", typeof (T).Name);

			return true;
		}

		protected bool Visit<T>(ConditionNode<T> node)
		{
			Append("Condition<{0}>: {1}", typeof (T).Name, node.Expression);

			return true;
		}

		protected bool Visit<T>(AlphaNode<T> node)
		{
			Append("Alpha<{0}>[{1}]", typeof (T).Name, node.GetHashCode());

			return true;
		}

		protected bool Visit<T>(JoinNode<T> node)
		{
			Append("Junction<{0}>[{1}]", typeof (T).Name, node.GetHashCode());

			return true;
		}

		protected bool Visit<T>(ConstantNode<T> node)
		{
			Append("Constant<{0}>", typeof (T).Name);

			return true;
		}

		protected bool Visit<T>(ActionNode<T> node)
		{
			Append("Action<{0}>: {1}", typeof (T).Name, node.Expression);

			return true;
		}

		private string Pad()
		{
			return new string('\t', _depth);
		}

		private void Append(string format, params object[] args)
		{
			_output.Append(Pad()).AppendFormat(format, args).AppendLine();
		}
	}
}