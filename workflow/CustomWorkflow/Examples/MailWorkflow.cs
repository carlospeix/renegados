using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CustomWorkflows
{
    public class MailWorkflow : FlowControlTaskWorkflow
    {
        public Task Inicio { get; set; }
        public Task CargarTitulo { get; set; }
        public Task EnviarMail { get; set; }
        public Task RevisarTexto { get; set; }
        public Task Rechazada { get; set; }

        public MailWorkflow()
        {
            Inicio.Actions.Add(new Action("Aceptar", (env) =>
            {
                if (env.BooleanParameter("saltearCT"))
                    InitiateTasks(env, EnviarMail);
                else
                    InitiateTasks(env, CargarTitulo);
            }));
            RevisarTexto.Actions.Add(new Action("Aceptar", (env) => { InitiateTasks(env, CargarTitulo); }));
            RevisarTexto.Actions.Add(new Action("Rechazar", (env) => { InitiateTasks(env, Rechazada); }));
            CargarTitulo.Actions.Add(new Action("Aceptar", (env) => { InitiateTasks(env, EnviarMail); }));
            CargarTitulo.Actions.Add(new Action("Revisar", (env) => { InitiateTasks(env, RevisarTexto); }));
            EnviarMail.Actions.Add(new Action("Aceptar", (env) => { Console.WriteLine("Enviar el mail"); }));
            EnviarMail.Actions.Add(new Action("Revisar", (env) => { InitiateTasks(env, CargarTitulo); }));
        }
    }
}
