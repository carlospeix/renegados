# encoding: utf-8

require File.join(File.dirname(__FILE__),'../model/liquidador')

describe Liquidador do

  before(:each) do
    tasas_interes = [
      [ Date.new(2000, 1, 1), Date.new(2001, 1, 1), 2 ]
    ]
    @liquidador = Liquidador.new(tasas_interes)
  end

  it 'debería calcular cero interés con capital no vencido' do
    @liquidador.interes(100, Date.today, Date.today).should == 0
  end

  it 'debería calcular 4 pesos de interés para un día despues del vencimiento' do
    @liquidador.interes(200, Date.today - 1, Date.today).should == 4
  end

end