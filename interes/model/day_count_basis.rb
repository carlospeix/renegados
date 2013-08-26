
class DayCountBasis
  def calcular_dias(rango)
    throw Exception.new("Implementar en subclases")
  end
end

class ActualActual < DayCountBasis
  def calcular_dias(rango)
    rango.cantidad_de_dias
  end
end
