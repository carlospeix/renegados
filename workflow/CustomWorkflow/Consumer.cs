using System;

namespace CustomWorkflows
{
	public class Consumer
	{
		public event EventHandler<EventArgs> OnItemAvailable;

		private readonly QueueManager _queue;

		public Consumer(QueueManager queue)
		{
			_queue = queue;
			_queue.OnItemAvailable += queue_OnItemAvailable;
		}

		void queue_OnItemAvailable(object sender, EventArgs e)
		{
			if (OnItemAvailable != null)
				OnItemAvailable(sender, e);
		}

		public void TerminarProcesarCola()
		{
			while (_queue.Count() > 0)
				System.Threading.Thread.Sleep(1);
		}
	}
}
