using System;

namespace Unidades.Modelo
{
	public class Unidad
	{
		private readonly string _nombre;
		private readonly int _cantidad;
		private readonly Unidad _unidadReferencia;

		public Unidad(string nombre, int cantidad)
		{
			_nombre = nombre;
			_cantidad = cantidad;
		}

		public Unidad(string nombre, int cantidad, Unidad unidadReferencia)
		{
			_nombre = nombre;
			_cantidad = cantidad;
			_unidadReferencia = unidadReferencia;
		}

		public string Nombre
		{
			get { return _nombre; }
		}

		public virtual int Cantidad
		{
			get
			{
				if (_unidadReferencia == null)
					return _cantidad;

				return _cantidad * _unidadReferencia.Cantidad;
			}
		}
	}
}
