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
namespace Magnum.Channels.Configuration.Internal
{
	/// <summary>
	/// Configures a channel on an untyped channel
	/// </summary>
	public interface ChannelConfigurator :
		Configurator
	{
		void Configure(ChannelConfiguratorConnection connection);
	}


	/// <summary>
	/// Configures a channel on a typed channel
	/// </summary>
	/// <typeparam name="TChannel"></typeparam>
	public interface ChannelConfigurator<TChannel> :
		Configurator
	{
		void Configure(ChannelConfiguratorConnection<TChannel> connection);
	}
}