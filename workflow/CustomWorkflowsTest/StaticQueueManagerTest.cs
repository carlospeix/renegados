using CustomWorkflows;
using NUnit.Framework;

namespace CustomWorkflowsTest
{
	[TestFixture]
	public class TestStaticQueueManager
	{
		QueueManager _queueManager;

		[SetUp]
		public void Setup()
		{
			_queueManager = new StaticQueueManager("StaticQueueTest");
			_queueManager.Clear();
		}

		[Test]
		public void AgregarItemsACola()
		{
			Assert.AreEqual(0, _queueManager.Count());
			_queueManager.Add("Hola mundo");
			_queueManager.Add("Adios mundo");
			Assert.AreEqual(2, _queueManager.Count());
		}

		[Test]
		public void ObtenerItemsDeCola()
		{
			_queueManager.Add("Hola mundo");
			_queueManager.Add("Adios mundo");

			var item = (string)_queueManager.Get();
			_queueManager.Remove(item);
			Assert.AreEqual("Hola mundo", item);

			item = (string)_queueManager.Get();
			_queueManager.Remove(item);
			Assert.AreEqual("Adios mundo", item);

			Assert.AreEqual(0, _queueManager.Count());
		}

		[Test]
		public void ObtenerYReponerItemsEnCola()
		{
			_queueManager.Add("Hola mundo");
			_queueManager.Add("Adios mundo");

			var item = (string)_queueManager.Get();
			_queueManager.Remove(item);
			Assert.AreEqual("Hola mundo", item);
			_queueManager.Add("Hola mundo");

			item = (string)_queueManager.Get();
			_queueManager.Remove(item);
			Assert.AreEqual("Adios mundo", item);

			Assert.AreEqual(1, _queueManager.Count());
		}

		[Test]
		public void UsarSegundaColaSeparadaPrimeraCola()
		{
			var queueManager2 = new StaticQueueManager("StaticQueueTest2");
			queueManager2.Clear();

			Assert.AreEqual(0, _queueManager.Count());
			_queueManager.Add("Hola mundo");
			_queueManager.Add("Adios mundo");
			Assert.AreEqual(2, _queueManager.Count());

			Assert.AreEqual(0, queueManager2.Count());
			queueManager2.Add("Hola mundo");
			queueManager2.Add("Adios mundo");
			Assert.AreEqual(2, queueManager2.Count());
		}

		[Test]
		public void UsarConTransactionScope()
		{
			using (var ts = new System.Transactions.TransactionScope())
			{
				_queueManager.Add("Hola mundo");
				_queueManager.Add("Adios mundo");
				Assert.AreEqual(0, _queueManager.Count());
				ts.Complete();
			}
			Assert.AreEqual(2, _queueManager.Count());

		}
	}
}