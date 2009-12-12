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
namespace Magnum.RulesEngine.Specs
{
	using Conditions;
	using Consequences;
	using DSL;
	using Events;
	using TestFramework;

	[Scenario]
	public class When_defining_a_rule_using_the_dsl
	{
		[When]
		public void A_fluent_rule_definition_format()
		{
			var declared = Rule.Declare<OrderSubmitted>(rule =>
				{
					rule.When<CustomerIsPreferred>()
						.Exit();

					rule.Always<LogOrderDetails>();

					rule.When<OrderAmountIsOverLimit>()
						.Then<HoldOrderForApproval>();
				});
		}
	}
}