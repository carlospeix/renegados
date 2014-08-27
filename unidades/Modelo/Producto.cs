using System.Linq;
using System.Collections.Generic;

namespace Unidades.Modelo
{
	public class Producto
	{
		private readonly IList<Unidad> _unidades;

		public Producto()
		{
			_unidades = new List<Unidad>();
		}

		public void AgregarUnidad(Unidad unidad)
		{
			_unidades.Add(unidad);
		}

		public int UnidadesPor(string nombreUnidad)
		{
			if (_unidades.Count == 0)
				return 0;

			if (_unidades.Count >= 1 && _unidades[0].Nombre == nombreUnidad)
				return _unidades[0].Cantidad;

			if (_unidades.Count >= 2 && _unidades[1].Nombre == nombreUnidad)
				return _unidades[1].Cantidad * _unidades[0].Cantidad;

			return 0;
		}
	}
}
