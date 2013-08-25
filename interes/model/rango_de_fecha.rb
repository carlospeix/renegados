require 'date'

class Date
  def to(end_date)
    RangoDeFecha.new(self,end_date)
  end

  def max(another_date)
    return self if self > another_date
    another_date
  end

  def min(another_date)
    return self if self < another_date
    another_date
  end

end

class RangoDeFecha
  def initialize(from,to)
    @from = from
    @to = to
  end

  def from
    @from
  end

  def to
    @to
  end

  def interseccion(otro_rango)
    @from.max(otro_rango.from).to(@to.min(otro_rango.to))
  end

  def cantidad_de_dias
    cantidad = (@to-@from).to_i + 1
    return cantidad if cantidad > 0
    return 0
  end
end