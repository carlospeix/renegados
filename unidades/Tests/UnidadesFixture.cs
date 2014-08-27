using NUnit.Framework;
using Unidades.Modelo;

namespace Renegados.Tests
{
	[TestFixture]
	public class CasosEspecialesTest
	{
		private Producto _prod;

		[SetUp]
		public void SetUp()
		{
			_prod = new Producto();
			_prod.AgregarUnidad(new Unidad("Unidad", 1));
			_prod.AgregarUnidad(new Unidad("Caja", 10));
		}

		[Test]
		public void UnidadesPor1UnidadDeberiaSer1()
		{
			var unidadesPorUnidad = _prod.UnidadesPor("Unidad");
			Assert.AreEqual(1, unidadesPorUnidad);
		}

		[Test]
		public void UnidadesPor1CajasDeberiaSer10()
		{
			var unidadesEn1Cajas = _prod.UnidadesPor("Caja");
			Assert.AreEqual(10, unidadesEn1Cajas);
		}
	}
}