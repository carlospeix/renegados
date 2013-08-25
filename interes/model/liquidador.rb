require File.join(File.dirname(__FILE__),'../model/interes_en_rango_de_fecha')
require File.join(File.dirname(__FILE__),'../model/rango_de_fecha')

class Liquidador
  def initialize(tasas_de_interes_diario)
    @tasas_de_interes_diario = tasas_de_interes_diario.collect { |array|
      InteresEnRangoDeFecha.new(array[0].to(array[1]),array[2]) }
  end

  def interes(capital, fecha_de_inicio, fecha_liquidacion)
    rango_de_deuda = (fecha_de_inicio+1).to(fecha_liquidacion)

    interes_acumulado = 0
    intereses_por_rango_para(rango_de_deuda).each { |interes_de_rango |
      interes_acumulado = interes_acumulado + capital * interes_de_rango / 100 }
    interes_acumulado
  end

  def intereses_por_rango_para(rango_de_deuda)
    @tasas_de_interes_diario.collect { |interes_por_rango|
      interes_por_rango.interes_para(rango_de_deuda) }
  end

end