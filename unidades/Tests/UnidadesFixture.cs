using NUnit.Framework;
using Unidades.Modelo;

namespace Unidades.Tests
{
	[TestFixture]
	public class UnidadesFixture
	{
		private Producto _prod;

		[SetUp]
		public void SetUp()
		{
			_prod = new Producto();
			_prod.AgregarUnidad("Botella", 1);
			_prod.AgregarUnidad("Caja", 10, "Botella");
			_prod.AgregarUnidad("Cajón", 50, "Botella");
			_prod.AgregarUnidad("Pallet", 125, "Caja");
		}

		[Test]
		public void UnidadesPor1UnidadDeberiaSer1()
		{
			Assert.AreEqual(1, _prod.UnidadesPor("Botella"));
		}

		[Test]
		public void UnidadesPor1CajasDeberiaSer10()
		{
			Assert.AreEqual(10, _prod.UnidadesPor("Caja"));
		}

		[Test]
		public void UnidadesPor1CajonDeberiaSer50()
		{
			Assert.AreEqual(50, _prod.UnidadesPor("Cajón"));
		}

		[Test]
		public void UnidadesPor1PalletDeberiaSer1250()
		{
			Assert.AreEqual(1250, _prod.UnidadesPor("Pallet"));
		}
}
}