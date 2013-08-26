require File.join(File.dirname(__FILE__),'../model/tasa_de_interes_en_rango')
require File.join(File.dirname(__FILE__),'../model/rango_de_fecha')

class Liquidador
  def initialize(tasas_de_interes_diario,day_count_basis)
    @tasas_por_rango = tasas_de_interes_diario.collect { | tasa_de_interes_diario|
      TasaDeInteresEnRango.new(tasa_de_interes_diario[0].to(tasa_de_interes_diario[1]),
        tasa_de_interes_diario[2])
    }
    @day_count_basis = day_count_basis
  end

  def interes(capital, fecha_de_inicio, fecha_liquidacion)
    rango_de_deuda = (fecha_de_inicio+1).to(fecha_liquidacion)

    intereses_por_rango = @tasas_por_rango.collect { | tasa_en_rango |
      tasa_en_rango.interes_para(rango_de_deuda,@day_count_basis)
    }

    intereses_por_rango.sum {|interes_en_rango | capital * interes_en_rango / 100 }

  end

end

class Array
  def sum(&block)
    self.inject 0 do | total,sumando |
      total + block.call(sumando)
    end
  end
end