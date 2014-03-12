using System;
using System.Collections.Generic;
using System.Linq;
using CustomWorkflows;
using NUnit.Framework;

namespace CustomWorkflowsTest
{
	public class Tests
	{
		private ExampleTaskWorkflow _ew;
		private NewTaskEventArgs _lastTaskResult;
		private Guid _instanceId;

		[TestFixtureSetUp]
		public void Setup()
		{
			_ew = new ExampleTaskWorkflow();
			_ew.OnNewTask += OnNewTask;
		}

		[Test]
		public void GetStartTask()
		{
			Assert.IsTrue(_ew.GetStartTask() == _ew.Inicio);
		}

		[Test]
		public void TestSecondTask()
		{
			var parameters = new Dictionary<string, object>
			                 	{
			                 		{"N2", true}
			                 	};
			_instanceId = Guid.NewGuid();
			_ew.DispatchTask(_ew.Inicio, "Aceptar", _instanceId, parameters);
			Assert.AreEqual(_lastTaskResult.Tasks.Single(), _ew.IngresoSolicitud);
		}

		[Test]
		public void TestWorkflow()
		{
			var parameters = new Dictionary<string, object>
			                 	{
			                 		{"N2", true}
			                 	};

			_ew.DispatchTask(_ew.Inicio, "Aceptar", _instanceId, parameters);
			Assert.AreEqual(_lastTaskResult.Tasks.Single(), _ew.IngresoSolicitud);

			_ew.DispatchTask(_ew.IngresoSolicitud, "Aceptar", _instanceId, parameters);
			Assert.AreEqual(_lastTaskResult.Tasks.Single(), _ew.Carga2b);

			_ew.DispatchTask(_ew.Carga2a, "Aceptar", _instanceId, parameters);
			Assert.AreEqual(_lastTaskResult.Tasks.Single(), _ew.Carga2b);

			_ew.DispatchTask(_ew.Carga2b, "Aceptar", _instanceId, parameters);
			Assert.AreEqual(_lastTaskResult.Tasks.Single(), _ew.Carga3);

			_ew.DispatchTask(_ew.Carga3, "Revisar", _instanceId, parameters);
			Assert.AreEqual(_lastTaskResult.Tasks.First(), _ew.Carga2a);
			Assert.AreEqual(_lastTaskResult.Tasks.Last(), _ew.Carga2b);

			_ew.DispatchTask(_ew.Carga2b, "Aceptar", _instanceId, parameters);
			Assert.AreEqual(_lastTaskResult.Tasks.First(), _ew.Carga2a);
			Assert.AreEqual(_lastTaskResult.Tasks.Last(), _ew.Carga2b);

			_ew.DispatchTask(_ew.Carga2a, "Aceptar", _instanceId, parameters);
			Assert.AreEqual(_lastTaskResult.Tasks.Single(), _ew.Carga3);

			_ew.DispatchTask(_ew.Carga3, "Aceptar", _instanceId, parameters);
			Assert.AreEqual(_lastTaskResult.Tasks.Single(), _ew.Completada);
		}

		private void OnNewTask(object sender, NewTaskEventArgs e)
		{
			_lastTaskResult = e;
		}

	}
}