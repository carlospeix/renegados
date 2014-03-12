using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace CustomWorkflows
{
	abstract public class TaskWorkflow
	{
		public bool AsyncMode { get; set; }
		public List<Task> Tasks { get; set; }
		public event EventHandler<NewTaskEventArgs> OnNewTask;
		public string Descripcion = "Task Workflow";
		public bool Verbose { get; set; }
		QueueManager _queueManager;
		private readonly Consumer _consumer;

		protected TaskWorkflow(QueueManager queueManager, Consumer consumer)
		{
			_queueManager = queueManager;
			_consumer = consumer;
			_consumer.OnItemAvailable += DoProcessItem;

			Tasks = new List<Task>();
			var properties = GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);
			foreach (var property in properties)
			{
				if (property.PropertyType == typeof(Task))
				{
					var t = new Task
					        	{
					        		Name = property.Name
					        	};
					property.SetValue(this, t, null);
					Tasks.Add(t);
				}
			}
		}

		public void DispatchTask(string taskName, string actionName, Guid instanceId, Dictionary<string, object> parameters)
		{
			DispatchTask(taskName, actionName, instanceId, parameters, AsyncMode);
		}

		public void DispatchTask(string taskName, string actionName, Guid instanceId, Dictionary<string, object> parameters, bool asyncMode)
		{
			DispatchTask(Tasks.Single(x => x.Name == taskName), actionName, instanceId, parameters, asyncMode);
		}

		public void DispatchTask(Task task, string actionName, Guid instanceId, Dictionary<string, object> parameters)
		{
			DispatchTask(task, actionName, instanceId, parameters, AsyncMode);
		}

		public void DispatchTask(Task task, string actionName, Guid instanceId, Dictionary<string, object> parameters, bool asyncMode)
		{
			if (asyncMode)
			{
				var eventTask = new EventTask(task, actionName, instanceId, parameters);
				_queueManager.Add(eventTask);
			}
			else
				ExecuteTask(task, actionName, instanceId, parameters);
		}

		public virtual void InitiateTasks(ExecutionEnvironment env, params Task[] tasks)
		{
			if (OnNewTask != null)
			{
				var e = new NewTaskEventArgs
				{
					ExecEnvironment = env,
					Tasks = tasks
				};
				OnNewTask(this, e);
			}
		}

		public virtual Task GetStartTask()
		{
			return Tasks.First();
		}

		public void TerminarProcesarCola()
		{
			while (_queueManager.Count() > 0)
			{
				System.Threading.Thread.Sleep(1);
			}
		}

		private void DoProcessItem(object sender, EventArgs e)
		{
			var eventTask = (EventTask)sender;
			ExecuteTask(eventTask.Task, eventTask.ActionName, eventTask.InstanceId, eventTask.Parameters);
		}

		protected virtual void ExecuteTask(Task task, string actionName, Guid instanceId, Dictionary<string, object> parameters)
		{
			var env = new ExecutionEnvironment
			          	{
			          		InstanceId = instanceId,
			          		Parameters = parameters,
			          		WorkflowInstance = this,
			          		CurrentTask = Tasks.Single(x => x.Name == task.Name)
			          	};
			env.CurrentAction = env.CurrentTask.Actions.Single(x => x.Name == actionName);

			if (Verbose)
				Console.WriteLine("\nEjecutar Tarea: " + Tasks.Single(x => x.Name == task.Name).Name + ", Accion: " + actionName + ", hora: " + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"));

			env.CurrentAction.ExecuteInstructions(env);
		}

		public override string ToString()
		{
			return Descripcion;
		}
	}

	public class ExecutionEnvironment
	{
		public Dictionary<string, object> Parameters { get; set; }
		public Guid InstanceId { get; set; }
		public TaskWorkflow WorkflowInstance { get; set; }
		public Action CurrentAction { get; set; }
		public Task CurrentTask { get; set; }

		public bool BooleanParameter(string parameterKey)
		{
			if (!Parameters.ContainsKey(parameterKey))
				return false;
			return (bool)Parameters[parameterKey];
		}

		public string StringParameter(string parameterKey)
		{
			if (!Parameters.ContainsKey(parameterKey))
				return null;
			return (string)Parameters[parameterKey];
		}
	}

	[Serializable]
	public class Action
	{
		public string Name { get; private set; }
		public delegate void TaskInstructions(ExecutionEnvironment actionEnvironment);
		public TaskInstructions Instructions;

		public Action(string name, TaskInstructions instructions)
		{
			Name = name;
			Instructions = instructions;
		}

		public void ExecuteInstructions(ExecutionEnvironment executionEnvironment)
		{
			Instructions(executionEnvironment);
		}
	}

	[Serializable]
	public class Task
	{
		public string Description { get; set; }
		public string Name { get; set; }
		public List<Action> Actions { get; set; }

		public Task()
		{
			Actions = new List<Action>();
		}

		public override string ToString()
		{
			return Name;
		}
	}

	[Serializable]
	public class EventTask
	{
		public Task Task { get; set; }
		public string ActionName { get; set; }
		public Guid InstanceId { get; set; }
		public Dictionary<string, object> Parameters { get; set; }

		public EventTask(Task task, string actionName, Guid instanceId, Dictionary<string, object> parameters)
		{
			Task = task;
			ActionName = actionName;
			InstanceId = instanceId;
			Parameters = parameters;
		}
	}

	public class NewTaskEventArgs : EventArgs
	{
		public ExecutionEnvironment ExecEnvironment { get; set; }
		public Task[] Tasks { get; set; }
	}
}