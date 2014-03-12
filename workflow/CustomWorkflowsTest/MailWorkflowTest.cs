using System;
using System.Collections.Generic;
using System.Linq;
using CustomWorkflows;
using NUnit.Framework;

namespace CustomWorkflowsTest
{
	public class InstanciaMail
	{
		public Guid InstanceId { get; set; }
		public string Texto { get; set; }
		public string Titulo { get; set; }
		public string TareaActual { get; set; }
	}

	[TestFixture]
	public class MailWorkflowTest
	{
		MailWorkflow _workflow;
		InstanciaMail _instancia;

		[SetUp]
		public void Setup()
		{
			_workflow = new MailWorkflow();
			_instancia = new InstanciaMail
			             	{
			             		InstanceId = Guid.NewGuid(), 
											Texto = "Hola mundo"
			             	};
		}

		[Test]
		public void DispatchTaskParaCrearInstanciaYDispararLaPrimeraTareaEnFormaSincronica()
		{
			string taskName = null;

			_workflow.OnNewTask += (sender, e) => taskName = e.Tasks.First().Name;

			_workflow.DispatchTask(_workflow.GetStartTask(), "Aceptar", _instancia.InstanceId, new Dictionary<string, object>());

			Assert.AreEqual("CargarTitulo", taskName);
		}

		[Test]
		public void DisparaLaSegundaTarea()
		{
			string taskName = null;
			_workflow.OnNewTask += (sender, e) =>
			                       	{
			                       		if (e.Tasks.First().Name == "CargarTitulo")
			                       		{
			                       			_instancia.Titulo = "Titulo";
			                       			_workflow.DispatchTask(e.Tasks.First(), "Aceptar", _instancia.InstanceId, new Dictionary<string, object>());
			                       		}
			                       		else
			                       			taskName = e.Tasks.First().Name;
			                       	};

			_workflow.DispatchTask(_workflow.GetStartTask(), "Aceptar", _instancia.InstanceId, new Dictionary<string, object>());

			Assert.AreEqual("EnviarMail", taskName);
			Assert.AreEqual("Titulo", _instancia.Titulo);
		}

		[Test]
		public void RechazarLaInstancia()
		{
			string taskName = null;
			_workflow.OnNewTask += (sender, e) =>
			                       	{
			                       		if (e.Tasks.First().Name == "CargarTitulo")
			                       		{
			                       			_workflow.DispatchTask(e.Tasks.First(), "Revisar", _instancia.InstanceId, new Dictionary<string, object>());
			                       		}
			                       		else if (e.Tasks.First().Name == "RevisarTexto")
			                       		{
			                       			_workflow.DispatchTask(e.Tasks.First(), "Rechazar", _instancia.InstanceId, new Dictionary<string, object>());
			                       		}
			                       		else
			                       			taskName = e.Tasks.First().Name;
			                       	};

			_workflow.DispatchTask(_workflow.GetStartTask(), "Aceptar", _instancia.InstanceId, new Dictionary<string, object>());

			Assert.AreEqual("Rechazada", taskName);
		}

		[Test]
		public void SaltearCargaTituloPorParametro()
		{
			string taskName = null;
			_workflow.OnNewTask += (sender, e) =>
			                       	{
			                       		if (e.Tasks.First().Name == "CargarTitulo")
			                       		{
			                       			_workflow.DispatchTask(e.Tasks.First(), "Revisar", _instancia.InstanceId, new Dictionary<string, object>());
			                       		}
			                       		taskName = e.Tasks.First().Name;
			                       	};

			_workflow.DispatchTask(_workflow.GetStartTask(), "Aceptar", _instancia.InstanceId, new Dictionary<string, object>
			                                                                                   	{
			                                                                                   		{ "saltearCT", true }
			                                                                                   	});

			Assert.AreEqual("EnviarMail", taskName);
		}

		[Test]
		public void AsincronicoUnaAccion()
		{
			string taskName = null;
			_workflow.AsyncMode = true;
			_workflow.OnNewTask += (sender, e) =>
			                       	{
			                       		taskName = e.Tasks.First().Name;
			                       	};
			_instancia.TareaActual = "Procesando";

			Assert.AreNotEqual("EnviarMail", taskName);
			_workflow.DispatchTask(_workflow.GetStartTask(), "Aceptar", _instancia.InstanceId, new Dictionary<string, object>
			                                                                                   	{
			                                                                                   		{ "saltearCT", true }
			                                                                                   	});
			System.Threading.Thread.Sleep(100);

			Assert.AreEqual("EnviarMail", taskName);
		}

		[Test]
		public void AsincronicoDosAccionesEnSerie()
		{
			var taskName = new List<string>();
			_workflow.AsyncMode = true;
			_workflow.OnNewTask += (sender, e) =>
			                       	{
			                       		taskName.Add(e.Tasks.First().Name);
			                       		if (e.Tasks.First().Name == "CargarTitulo")
			                       		{
			                       			_workflow.DispatchTask(e.Tasks.First(), "Aceptar", _instancia.InstanceId, new Dictionary<string, object>());
			                       		}
			                       	};
			_instancia.TareaActual = "Procesando";

			_workflow.DispatchTask(_workflow.GetStartTask(), "Aceptar", _instancia.InstanceId, new Dictionary<string, object>
			                                                                                   	{
			                                                                                   		{ "saltearCT", false }
			                                                                                   	});

			System.Threading.Thread.Sleep(100);

			Assert.AreEqual("CargarTitulo", taskName[0]);
			Assert.AreEqual("EnviarMail", taskName[1]);
		}

		[Test]
		public void AsincronicoUnaAccionSincronicaYOtraAsincronica()
		{
			var taskName = new List<string>();
			_workflow.OnNewTask += (sender, e) =>
			                       	{
			                       		taskName.Add(e.Tasks.First().Name);
			                       		if (e.Tasks.First().Name == "CargarTitulo")
			                       		{
			                       			_workflow.DispatchTask(e.Tasks.First(), "Aceptar", _instancia.InstanceId, new Dictionary<string, object>(), true);
			                       		}
			                       	};
			_instancia.TareaActual = "Procesando";

			_workflow.DispatchTask(_workflow.GetStartTask(), "Aceptar", _instancia.InstanceId, new Dictionary<string, object>
			                                                                                   	{
			                                                                                   		{ "saltearCT", false }
			                                                                                   	}, false);

			System.Threading.Thread.Sleep(100);

			Assert.AreEqual("CargarTitulo", taskName[0]);
			Assert.AreEqual("EnviarMail", taskName[1]);
		}

		[Test]
		public void AsincronicoDosAccionesAsincronicasEnSerieDesdeThreadPrincipal()
		{
			_workflow.AsyncMode = true;
			_workflow.OnNewTask += (sender, e) =>
			                       	{
			                       		_instancia.TareaActual = e.Tasks.First().Name;
			                       	};
			_instancia.TareaActual = "Procesando";

			_workflow.DispatchTask(_workflow.GetStartTask(), "Aceptar", _instancia.InstanceId, new Dictionary<string, object>
			                                                                                   	{
			                                                                                   		{ "saltearCT", false }
			                                                                                   	});

			System.Threading.Thread.Sleep(100);

			Assert.AreEqual("CargarTitulo", _instancia.TareaActual);

			_workflow.DispatchTask(_instancia.TareaActual, "Aceptar", _instancia.InstanceId, new Dictionary<string, object>());

			System.Threading.Thread.Sleep(100);

			Assert.AreEqual("EnviarMail", _instancia.TareaActual);
		}

		[Test]
		public void AsincronicoAccionAsincronicaQueTardeEnActualizarYConTransactionScopeRolledBack()
		{
			_workflow.AsyncMode = true;
			_workflow.OnNewTask += (sender, e) =>
			                       	{
			                       		_instancia.TareaActual = e.Tasks.First().Name;
			                       	};
			_instancia.TareaActual = "Procesando";
			using (new System.Transactions.TransactionScope())
			{
				_workflow.DispatchTask(_workflow.GetStartTask(), "Aceptar", _instancia.InstanceId, new Dictionary<string, object>
				                                                                                   	{
				                                                                                   		{ "saltearCT", false }
				                                                                                   	});
			}

			System.Threading.Thread.Sleep(100);

			Assert.AreEqual("Procesando", _instancia.TareaActual);
		}
	}
}