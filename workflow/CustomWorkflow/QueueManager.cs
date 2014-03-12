using System;

namespace CustomWorkflows
{
	public abstract class QueueManager
	{
		public event EventHandler<EventArgs> OnItemAvailable;

		public virtual void SendItemAvailable(object item)
		{
			if (item != null)
			{
				if (OnItemAvailable != null)
				{
					OnItemAvailable(item, new EventArgs());
					Remove(item);
				}
			}
		}

		protected string QueueName;

		protected QueueManager(string queueName)
		{
			QueueName = queueName;
		}

		public abstract void Add(object item);

		public abstract object Get();

		public abstract void Remove(object item);

		public abstract int Count();

		public abstract void Clear();
	}
}