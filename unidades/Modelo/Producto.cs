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

		public void AgregarUnidad(string nombreUnidad, int cantidad)
		{
			_unidades.Add(new Unidad(nombreUnidad, cantidad));
		}

		public void AgregarUnidad(string nombreUnidad, int cantidad, string nombreUnidadRef)
		{
			var unidadRef = _unidades.First(u => u.Nombre == nombreUnidadRef);
			_unidades.Add(new Unidad(nombreUnidad, cantidad, unidadRef));
		}

		public int UnidadesPor(string nombreUnidad)
		{
			var unidad = _unidades.First(u => u.Nombre == nombreUnidad);
			return unidad.Cantidad;
		}

		public int UnidadesPorOld(string nombreUnidad)
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
