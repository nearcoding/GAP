   M S B A M L   `   `   `  ����A  =GAP.BL, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null/$clr-namespace:GAP.BL;assembly=GAP.BLGAP.BL  5   	   > :GAP, Version=1.0.2.2, Culture=neutral, PublicKeyToken=null;"clr-namespace:GAP.ExtendedControlsGAP.ExtendedControls 5   	      GAP.AddEquationView   5      ,BLL$clr-namespace:GAP.BL;assembly=GAP.BL  5   	   +base"clr-namespace:GAP.ExtendedControls  5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	      WindowDialog#��[  5   	     � WindowStartupLocation$  CenterScreen=�5   	     AddEquation � Title# �  5	   	   $	��600��6/   $	��380��6<    � 	Resources 5
      (   "GAP.BL.BooleanToVisibilityConveter  BooleanToVisbilityConverter �� 
TargetType �� Property �� Value�� 5   
   %s   &	       '��        5      6.   �� 5      " ��6   .�5      ��  � ��$ Center=�66   5      5      .��5      � $��5q�6    � RowDefinitions 5   
   �� 5        AutoHeightRowDefinition#��[ 6   �� 5        MinHeightRowDefinition#��[ 6   �� 5      #��[ 6   �� 5      #��[ 6   �� 5      #��[ 6   �� 5      #��[ 6   �� 5      #��[ 6   �� 5      #��[ 6   �� 5      #��[ 6   �� 5      #��[ 6   �� 5      #��[ 6   �� 5      #��[ 6   �� 5       #��[ 6   �� 5!      #��[ 6   �� 5"      #��[ 6   �� 5#      #��[ 6   �� 5$      #��[ 6   �� 5%      #��[ 6    � ColumnDefinitions 5'   
   �� 5(        AutoWidthColumnDefinition#��[ 6   �� 5)        MinWidthColumnDefinition#��[ 6   �� 5*      #��[ 6   .+�5,   
   ��   Project#���  6   �� 5-   
   $
��Left=�6<   $��2��6X    �� DisplayMemberPath$
 Name��6�   	 �� SelectedValuePath$	 ID��6�   
 �� SelectedItem
 6   �� *SelectedProject+��6h   �� *
Projects+6�   �� 5/   
   $��2��6     Well#���  6!   �� 50   
   $��2��69   $��2��6F   $
 Name��6t   $	 ID��6�   
 6   �� *SelectedWell+��6V   �� *Wells+6�   �� 51   
   $��4��6    	 Dataset#��� 	 6!   �� 52   
   $��4��6<   $��2��6I   $
 Name��6z   $	 ID��6�   
 6   �� *SelectedDataset+��6Y   �� *
Datasets+6�   �� 55   
   $��6��6    
 Equation#��� 
 6!   �� 56   
   $��6��66   $��2��6C    �� SelectedIndex 6   �� *
Equation+.@�57      �� $��Sum (Datasets)��6   �� 58      $��Subtraction (Datasets)��6   �� 59      $��Division (Datasets)��6   �� 5:      $��Multiplication (Datasets)��6   �� 5;      $��Log (base number)��6   5<      �� 5>   
   $��8��6   $��	Parameter��6!   �� 5?   
   $��8��6�   $��2��6�   $
 Name��6  $	 ID��6%  y�6   �� *!ParamVisibility+ �� 	Converter# [  �� Mode$ TwoWay=� �� UpdateSourceTrigger$ PropertyChanged=�
 6�   �� *SelectedDataset2+��6�   �� *
Datasets+6%  �� 5@   
   $��8��6�   $��2��6�   y�6   �� *ParamVisibility+# [ $ TwoWay=�$ PropertyChanged=���6�   �� *EquationNumber+$ TwoWay=�$ PropertyChanged=��� 5B   
   $��10��6     
NewDataset#���  6"   �� 5C   
   $��10��6   $��2��6    ��60   �� *CurrentObject.Name+$ TwoWay=�$ PropertyChanged=��� 5E   
   $��12��6    	 Notes#���  6"   �� 5F   
     TextBoxNotes#��[ 6   $��12��68   $��2��6F   ��6V   �� *CurrentObject.Notes+$ TwoWay=�$ PropertyChanged=�.��6�   5H      �� 5J   
     MetroGroupBox#��[ 6   $��14��6;   $��0��6I   $��3��6Y   ��5K      �� 5L        CurveProperties#���  6   $
��Bold"�6E   .��5N      �� .+�5O      ��   LineStyleProperties#��[ 6   �� 5P        MarkerStyleProperties#��[ 6   5Q      5R      �� 5T   
     SaveCancelButton#��[ 6   $��16��6C   $��2��6Q   5U      5V      