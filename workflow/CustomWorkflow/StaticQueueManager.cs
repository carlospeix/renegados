using System.Collections.Generic;
using System.Linq;

namespace CustomWorkflows
{
	public class StaticQueueManager : QueueManager
	{
		protected static Dictionary<string, List<object>> Queue = new Dictionary<string, List<object>>();
		
		private Dictionary<string, List<object>> TransactQueue
		{
			get
			{
				var tq = System.Runtime.Remoting.Messaging.CallContext.GetData("StaticQueueManager_transactQueue") as Dictionary<string, List<object>>;
				if (tq == null)
				{
					tq = new Dictionary<string, List<object>>();
					TransactQueue = tq;
					CreateQueue(QueueName);
				}
				return tq;
			}
			set
			{
				System.Runtime.Remoting.Messaging.CallContext.SetData("StaticQueueManager_transactQueue", value);
			}
		}

		private static System.Transactions.Transaction Transaction
		{
			get
			{
				return System.Runtime.Remoting.Messaging.CallContext.GetData("StaticQueueManager_Transaction") as System.Transactions.Transaction;
			}
			set
			{
				System.Runtime.Remoting.Messaging.CallContext.SetData("StaticQueueManager_Transaction", value);
			}
		}

		public StaticQueueManager(string queueName) : base(queueName)
		{
			CreateQueue(queueName);
		}

		protected void CreateQueue(string queueName)
		{
			if (!Queue.ContainsKey(queueName))
			{
				lock (Queue)
				{
					if (!Queue.ContainsKey(queueName))
					{
						Queue.Add(queueName, new List<object>());
					}
				}
			}
			if (!TransactQueue.ContainsKey(queueName))
			{
				var aux = new object();
				lock (aux)
				{
					if (!TransactQueue.ContainsKey(queueName))
					{
						TransactQueue.Add(queueName, new List<object>());
					}
				}
			}
		}

		private void RealAdd(object item)
		{
			Queue[QueueName].Add(item);
			System.Threading.Tasks.Task.Factory.StartNew(DoProcessQueue);
		}

		private void DoProcessQueue()
		{
			var objLock = new object();
			lock (objLock)
			{
				object item = Get();
				SendItemAvailable(item);
			}
		}

		public override void Add(object item)
		{
			if (System.Transactions.Transaction.Current != null)
			{
				if (Transaction == null)
				{
					Transaction = System.Transactions.Transaction.Current;
					Transaction.TransactionCompleted += CurrentTransactionCompleted;
				}
				TransactQueue[QueueName].Add(item);
			}
			else
				RealAdd(item);
		}

		void CurrentTransactionCompleted(object sender, System.Transactions.TransactionEventArgs e)
		{
			if (e.Transaction.TransactionInformation.Status == System.Transactions.TransactionStatus.Committed)
			{
				if (TransactQueue[QueueName] != null)
				{
					foreach (var item in TransactQueue[QueueName])
					{
						RealAdd(item);
					}
				}
			}
			TransactQueue.Clear();
			Transaction = null;
		}

		public override object Get()
		{
			object item = null;
			
			lock (Queue[QueueName])
			{
				if (Queue[QueueName].Count > 0)
				{
					item = Queue[QueueName].First();
				}
			}

			return item;
		}

		public override void Remove(object item)
		{
			Queue[QueueName].Remove(item);
		}

		public override int Count()
		{
			return Queue[QueueName].Count;
		}

		public override void Clear()
		{
			lock (Queue[QueueName])
			{
				if (Queue[QueueName].Count > 0)
				{
					Queue[QueueName].Clear();
				}
			}
		}
	}
}