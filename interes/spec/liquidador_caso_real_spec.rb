# encoding: utf-8

require File.join(File.dirname(__FILE__),'../model/liquidador')
require File.join(File.dirname(__FILE__),'../model/day_count_basis')

describe Liquidador do

  before(:each) do
    tasas_interes_combinadas = [
      [ Date.new(1998, 1, 1), Date.new(1998, 9, 30), 2 ],
      [ Date.new(1998, 10, 1), Date.new(2002, 6, 10), 3 ],
      [ Date.new(2002, 6, 11), Date.new(2003, 1, 31), 4 ],
      [ Date.new(2003, 2, 1), Date.new(2004, 5, 31), 3 ],
      [ Date.new(2004, 6, 1), Date.new(2004, 8, 31), 2 ],
      [ Date.new(2004, 9, 1), Date.new(2006, 6, 30), 1.5 ],
      [ Date.new(2006, 7, 1), Date.new(2010, 12, 31), 2 ],
      [ Date.new(2011, 1, 1), Date.new(9999, 12, 31), 3 ]
    ]
    @liquidador = Liquidador.new(tasas_interes_combinadas,ActualActual.new)
  end

  it 'debería calcular interes combinando varias tasas' do
    @liquidador.interes(1000, Date.new(2000, 1, 1), Date.new(2013, 1, 3)).should == 117490
  end

end