using System.Collections.Generic;
using NUnit.Framework;
using Unidades.Modelo;

namespace Unidades.Tests
{
	[TestFixture]
	public class ReporteFixture
	{
		private Producto _prod;
		private IList<dynamic> _reporte;

		[SetUp]
		public void SetUp()
		{
			_prod = new Producto();
			_prod.AgregarUnidad("Unidad", 1);
			_prod.AgregarUnidad("Caja", 10, "Unidad");
			_prod.AgregarUnidad("Cajón", 50, "Unidad");
			_prod.AgregarUnidad("Pallet", 125, "Caja");

			_reporte = new List<dynamic>
			           	{
			           		new {Producto = _prod, Cantidad = 90, Presentacion = "Unidad"}, // 90
			           		new {Producto = _prod, Cantidad = 25, Presentacion = "Caja"},   // 250
			           		new {Producto = _prod, Cantidad = 15, Presentacion = "Cajón"},  // 750
			           		new {Producto = _prod, Cantidad = 10, Presentacion = "Pallet"}  // 12500
			           	};
		}

		[Test]
		public void ReporteDeberiaSumar()
		{
			var unidades = 0;

			foreach (var linea in _reporte)
				unidades += linea.Cantidad*linea.Producto.UnidadesPor(linea.Presentacion);

			Assert.AreEqual(90 + 250 + 750 + 12500, unidades);
		}
	}
}