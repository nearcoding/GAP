   M S B A M L   `   `   `  ����>  :GAP, Version=1.0.2.2, Culture=neutral, PublicKeyToken=null;"clr-namespace:GAP.ExtendedControlsGAP.ExtendedControls  5   	   *    $GAP.AverageAndExactOrCloserValueView   5      \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   3base"clr-namespace:GAP.ExtendedControls     5   	   X TXceed.Wpf.Toolkit, Version=2.4.0.0, Culture=neutral, PublicKeyToken=3e4669d2f30244f44xctk)http://schemas.xceed.com/wpf/xaml/toolkit  5   
      ImportAverageData  � Title#  �   5   	   $��239.2��65   $	��340��6D     WindowDialog#��[ 5   
    � WindowStartupLocation$ CenterScreen=�5   
   .��5	      �  � ColumnDefinitions 5
   
   �� 5        MaxWidthColumnDefinition#��[ 6   �� 5        AutoWidthColumnDefinition#��[ 6   �� 5        MinWidthColumnDefinition#��[ 6   �� 5      #��[ 6   �� 5      #��[ 6    � RowDefinitions 5   
   �� 5        MaxHeightRowDefinition#��[ 6   �� 5        AutoHeightRowDefinition#��[ 6   �� 5        MinHeightRowDefinition#��[ 6   �� 5      #��[ 6   �� 5      #��[ 6   �� 5      #��[ 6   �� 5      #��[ 6   �� 5      #��[ 6   �� 5      #��[ 6   �� 5      #��[ 6   �� 5      #��[ 6   �� 5      #��[ 6   �� 5      #��[ 6   .+�5    
   ��   InitialDepth#���  6   $��1��6:   $��1��6G   �� 5!   
    	 
FinalDepth#��� 	 6   $��3��68   $��1��6E   �� 5"   
    
 Step#��� 
 6   $��5��62   $��1��6?   %  Xceed.Wpf.Toolkit.DecimalUpDown  5$   
   -   5        $��txtInitialDepth��5$   ;   $��1��6R   $��3��6_   ,  &Xceed.Wpf.Toolkit.Primitives.InputBase   Text 6   �� *InitialDepth+6_     5%   
   $��3��69   $��3��6F    6   �� *
FinalDepth+6F   %  Xceed.Wpf.Toolkit.IntegerUpDown  5&   
   -   5        $��txtStep��5&   d   $��5��6s   $��3��6�    6   �� *Step+ �� Mode$ TwoWay=� �� UpdateSourceTrigger$ PropertyChanged=�6�   	� 5(   
   -   5        $��rbArit��5(        AritmeticalAverage#���  6$   $
��Left=�5)      $��7��61   $��3��6>    d� 	IsChecked 5(   U   �� *IsArithmeticAverageChecked+$ TwoWay=�$ PropertyChanged=�5)   Q   	� 5+   
   -   5        $��rbExact��5+        ExactOrCloserValue#���  6%   $
��Left=�5,      $��9��61   $��3��6>    5+   T   �� *'%IsArithmeticExactOrCloserValueChecked+$ TwoWay=�$ PropertyChanged=�5,   Q   �� 5.   
   $��11��6   $��3��6(   $��2��68   $��0 0 10 0q�6L     SaveCancelButton#��[ 6^   5/      50      