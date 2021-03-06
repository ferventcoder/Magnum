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
namespace Magnum.Reflection
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Reflection;
	using Extensions;

	public class FastProperties<T>
	{
		private readonly Dictionary<string, FastProperty<T>> _properties = new Dictionary<string, FastProperty<T>>();

		public FastProperties()
		{
			const BindingFlags flags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;

			typeof (T).GetProperties(flags)
				.Each(property => _properties.Add(property.Name, new FastProperty<T>(property, flags)));
		}

		public void Each(T instance, Action<object> action)
		{
			_properties.OrderBy(x => x.Key).Select(x => x.Value).Each(property => action(property.Get(instance)));
		}
	}
}