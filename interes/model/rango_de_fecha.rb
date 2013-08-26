require 'date'



class Date
  def to(date)
    RangoDeFecha.new(self,date)
  end

  def max(date)
    return self if self > date
    return date
  end

  def min(date)
    return self if self < date
    return date
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
    return (@to-@from).to_i+1 if @from<=@to
    return 0
  end
end