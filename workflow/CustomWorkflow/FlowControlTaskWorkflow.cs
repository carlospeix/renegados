using System;
using System.Collections.Generic;

namespace CustomWorkflows
{
	public abstract class FlowControlTaskWorkflow : TaskWorkflow
	{
		protected FlowControlTaskWorkflow(QueueManager queueManager, Consumer consumer)
			: base(queueManager, consumer)
		{
		}

		protected override void ExecuteTask(Task task, string actionName, Guid instanceId, Dictionary<string, object> parameters)
		{
			if (!parameters.ContainsKey(task.Name))
				parameters.Add(task.Name, null);
			parameters[task.Name] = actionName;
			try
			{
				base.ExecuteTask(task, actionName, instanceId, parameters);
			}
			catch (Exception)
			{
				parameters[task.Name] = null;
				throw;
			}
		}

		public override void InitiateTasks(ExecutionEnvironment env, params Task[] tasks)
		{
			foreach (var task in tasks)
			{
				if (!env.Parameters.ContainsKey(task.Name))
					env.Parameters.Add(task.Name, null);
				else
					env.Parameters[task.Name] = null;
			}
			base.InitiateTasks(env, tasks);
		}

	}

	public abstract class BranchFlowControl
	{
		public List<Task> Tasks = new List<Task>();
		public abstract bool Evaluate(ExecutionEnvironment env);
		public List<BranchFlowControl> InvalidatesFlows { get; set; }
		public abstract bool CheckJoinStarted(ExecutionEnvironment env);

		protected BranchFlowControl()
		{
			InvalidatesFlows = new List<BranchFlowControl>();
		}
	}

	public class SynchronizationControlPattern : BranchFlowControl
	{
		public override bool Evaluate(ExecutionEnvironment env)
		{
			foreach (BranchFlowControl flow in InvalidatesFlows)
			{
				if (flow.CheckJoinStarted(env))
					throw new InvalidatedFlowException("No se puede enviar la accion");
			}

			bool ret = true;
			string action = null;
			bool seteado = false;
			foreach (var task in Tasks)
			{
				if (!env.Parameters.ContainsKey(task.Name))
				{
					ret = false;
					break;
				}
				if (!seteado)
				{
					seteado = true;
					action = (string)env.Parameters[task.Name];
				}
				else if ((string)env.Parameters[task.Name] != action)
				{
					ret = false;
					break;
				}
			}
			return ret;
		}

		public override bool CheckJoinStarted(ExecutionEnvironment env)
		{
			foreach (var task in Tasks)
			{
				if (env.Parameters.ContainsKey(task.Name) && !string.IsNullOrEmpty((string)env.Parameters[task.Name]) && (string)env.Parameters[task.Name] != env.CurrentAction.Name)
				{
					return true;
				}
			}
			return false;
		}
	}

	public class SimpleMergeControlPattern : BranchFlowControl
	{

		public override bool Evaluate(ExecutionEnvironment env)
		{
			foreach (BranchFlowControl flow in InvalidatesFlows)
			{
				if (flow.CheckJoinStarted(env))
					throw new InvalidatedFlowException("No se puede enviar la accion");
			}

			bool ret = true;
			bool seteado = false;
			foreach (var task in Tasks)
			{
				if (env.Parameters.ContainsKey(task.Name) && !string.IsNullOrEmpty((string)env.Parameters[task.Name]))
				{
					if (seteado)
					{
						ret = false;
						break;
					}

					seteado = true;
				}
			}
			return ret;
		}

		public override bool CheckJoinStarted(ExecutionEnvironment env)
		{
			foreach (var task in Tasks)
			{
				if (env.Parameters.ContainsKey(task.Name) && !string.IsNullOrEmpty((string)env.Parameters[task.Name]) && (string)env.Parameters[task.Name] != env.CurrentAction.Name)
				{
					return true;
				}
			}
			return false;
		}
	}

	public class InvalidatedFlowException : Exception
	{
		public InvalidatedFlowException() { }
		public InvalidatedFlowException(string message) : base(message) { }
		public InvalidatedFlowException(string message, Exception innerException) : base(message, innerException) { }
	}
}