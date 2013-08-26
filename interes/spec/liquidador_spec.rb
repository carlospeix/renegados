# encoding: utf-8

require File.join(File.dirname(__FILE__),'../model/liquidador')
require File.join(File.dirname(__FILE__),'../model/day_count_basis')

describe Liquidador do

  before(:each) do
    tasas_interes = [
      [ Date.new(2000, 1, 1), Date.new(2000, 1, 2), 2 ],
      [ Date.new(2000, 1, 3), Date.new(2000, 1, 4), 1 ]
    ]
    @liquidador = Liquidador.new(tasas_interes, ActualActual.new)
  end

  it 'debería calcular cero interés con capital no vencido' do
    @liquidador.interes(100, Date.new(2000, 1, 1), Date.new(2000, 1, 1)).should == 0
  end

  it 'debería calcular 4 pesos de interés para un día despues del vencimiento' do
    @liquidador.interes(200, Date.new(2000, 1, 1), Date.new(2000, 1, 2)).should == 4
  end

  it 'debería calcular 6 pesos de interés, 4 para el primer día y 2 para el segundo despues del vencimiento' do
    @liquidador.interes(200, Date.new(2000, 1, 1), Date.new(2000, 1, 3)).should == 6
  end

end
