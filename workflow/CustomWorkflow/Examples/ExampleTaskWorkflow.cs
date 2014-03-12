using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CustomWorkflows
{
    public class ExampleTaskWorkflow : TaskWorkflow
    {
        public Task Inicio { get; private set; }
        public Task IngresoSolicitud { get; private set; }
        public Task Carga2a { get; private set; }
        public Task Carga2b { get; private set; }
        public Task Carga3 { get; private set; }
        public Task Completada { get; private set; }
        public Task Rechazada { get; private set; }

        public ExampleTaskWorkflow()
        {
            Descripcion = "Workflow Ejemplo 1";

            Action accion = new Action("Aceptar", (env) => { InitiateTasks(env, IngresoSolicitud); });
            Inicio.Actions.Add(accion);
            System.Action<ExecutionEnvironment> resetParamsCarga2 = (env) => { env.Parameters["N1aceptada"] = false; env.Parameters["N1revisada"] = false; env.Parameters["N2aceptada"] = false; env.Parameters["N2revisada"] = false; };

            accion = new Action("Aceptar", (env) => { resetParamsCarga2(env); InitiateTasks(env, Carga2a); if ((bool?)env.Parameters["N2"] == true) { InitiateTasks(env, Carga2b); };  });
            IngresoSolicitud.Actions.Add(accion);
            accion = new Action("Rechazar", (env) => { InitiateTasks(env, Rechazada); });
            IngresoSolicitud.Actions.Add(accion);

            accion = new Action("Aceptar", (env) => { env.Parameters["N1aceptada"] = true; if ((bool?)env.Parameters["N2"] != true || (bool?)env.Parameters["N2aceptada"] == true) { InitiateTasks(env, Carga3); };  });
            Carga2a.Actions.Add(accion);
            accion = new Action("Revisar", (env) => { env.Parameters["N1revisada"] = true; if ((bool?)env.Parameters["N2"] != true || (bool?)env.Parameters["N2revisada"] == true) { InitiateTasks(env, IngresoSolicitud); };  });
            Carga2a.Actions.Add(accion);

            accion = new Action("Aceptar", (env) => { env.Parameters["N2aceptada"] = true; if ((bool?)env.Parameters["N1aceptada"] == true) { InitiateTasks(env, Carga3); };  });
            Carga2b.Actions.Add(accion);
            accion = new Action("Revisar", (env) => { env.Parameters["N2revisada"] = true; if ((bool?)env.Parameters["N1revisada"] == true) { InitiateTasks(env, IngresoSolicitud); };  });
            Carga2b.Actions.Add(accion);

            accion = new Action("Aceptar", (env) => { InitiateTasks(env, Completada); });
            Carga3.Actions.Add(accion);
            accion = new Action("Revisar", (env) => { resetParamsCarga2(env); if ((bool?)env.Parameters["N2"] == true) { InitiateTasks(env, Carga2a, Carga2b); } else { InitiateTasks(env, Carga2a); } });
            Carga3.Actions.Add(accion);

        }
    }

}
