// Copyright 2007-2010 The Apache Software Foundation.
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
namespace Magnum.Actors.Internal
{
	using System;
	using System.Collections.Generic;
	using Channels;
	using Extensions;
	using Fibers;


	public class BufferedInbox<T> :
		Inbox<T>
	{
		readonly Fiber _fiber;
		readonly List<PendingReceiveImpl<T>> _receivers;
		readonly Scheduler _scheduler;
		readonly IList<T> _waitingMessages;

		bool _disposed;

		public BufferedInbox(Fiber fiber, Scheduler scheduler)
		{
			_fiber = fiber;
			_scheduler = scheduler;

			_receivers = new List<PendingReceiveImpl<T>>();
			_waitingMessages = new List<T>();
		}

		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		public void Send(T message)
		{
			_fiber.Add(() => HandleSend(message));
		}

		public PendingReceive Receive(SelectiveConsumer<T> consumer)
		{
			if (ReceiveWaitingMessage(consumer))
				return null;

			var pending = new PendingReceiveImpl<T>(consumer, x => _receivers.Remove(x));

			_receivers.Add(pending);

			return pending;
		}

		public PendingReceive Receive(SelectiveConsumer<T> consumer, TimeSpan timeout, Action timeoutCallback)
		{
			if (ReceiveWaitingMessage(consumer))
				return null;

			var pending = new PendingReceiveImpl<T>(consumer, timeoutCallback, x => _receivers.Remove(x));

			pending.ScheduleTimeout(x => _scheduler.Schedule(timeout, _fiber, x.Timeout));

			_receivers.Add(pending);

			return pending;
		}

		public PendingReceive Receive(SelectiveConsumer<T> consumer, int timeout, Action timeoutCallback)
		{
			return Receive(consumer, timeout.Milliseconds(), timeoutCallback);
		}

		~BufferedInbox()
		{
			Dispose(false);
		}

		void Dispose(bool disposing)
		{
			if (_disposed)
				return;
			if (disposing)
			{
				_receivers.ToArray().Each(x => x.Cancel());

				_waitingMessages.Clear();
			}

			_disposed = true;
		}

		void HandleSend(T message)
		{
			if (DeliverToWaitingReceiver(message))
				return;

			_waitingMessages.Add(message);
		}

		bool ReceiveWaitingMessage(SelectiveConsumer<T> selectiveConsumer)
		{
			for (int i = 0; i < _waitingMessages.Count; i++)
			{
				Consumer<T> consumer = selectiveConsumer(_waitingMessages[i]);
				if (consumer == null)
					continue;

				consumer(_waitingMessages[i]);

				_waitingMessages.RemoveAt(i);
				return true;
			}

			return false;
		}

		bool DeliverToWaitingReceiver(T message)
		{
			for (int i = 0; i < _receivers.Count; i++)
			{
				PendingReceiveImpl<T> receiver = _receivers[i];

				Consumer<T> consumer = receiver.Accept(message);
				if (consumer == null)
					continue;

				consumer(message);

				return true;
			}

			return false;
		}
	}
}