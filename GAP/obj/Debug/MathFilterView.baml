   M S B A M L   `   `   `  ����A  =GAP.BL, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null/$clr-namespace:GAP.BL;assembly=GAP.BLGAP.BL  5   	   > :GAP, Version=1.0.2.2, Culture=neutral, PublicKeyToken=null;"clr-namespace:GAP.ExtendedControlsGAP.ExtendedControls 5   	      GAP.MathFilterView   5      ,BLL$clr-namespace:GAP.BL;assembly=GAP.BL  5   	   +base"clr-namespace:GAP.ExtendedControls  5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	      WindowDialog#��[  5   	     � WindowStartupLocation$  CenterScreen=�5   	     
MathFilter � Title# �  5	   	   $	��620��6.   $	��390��6;    � 	Resources 5
       �� MergedDictionaries �� Source �� 
TargetType �� Property �� Value�� 5   
    5      �� 5      $E ?pack://application:,,,/Style/BaseResource.CustomScrollbars.xaml?�6%   %V   '��       �� 5      " ��6   .�5      ��  � ��$ Center=�66   5      5      .��5      � $��5q�6    � RowDefinitions 5   
   �� 5        AutoHeightRowDefinition#��[ 6   �� 5        MinHeightRowDefinition#��[ 6   �� 5      #��[ 6   �� 5      #��[ 6   �� 5      #��[ 6   �� 5      #��[ 6   �� 5      #��[ 6   �� 5      #��[ 6   �� 5      #��[ 6   �� 5      #��[ 6   �� 5       #��[ 6   �� 5!      #��[ 6   �� 5"      #��[ 6   �� 5#      #��[ 6   �� 5$      #��[ 6   �� 5%      #��[ 6   �� 5&      #��[ 6   �� 5'      #��[ 6   	 � ColumnDefinitions	 5)   
   �� 5*        AutoWidthColumnDefinition#��[ 6   �� 5+        MinWidthColumnDefinition#��[ 6   �� 5,      #��[ 6   .+�5.   
   ��   Project#���  6   �� 5/   
   $
��Left=�6<   $��2��6X   
 �� DisplayMemberPath$

 Name��6�    �� SelectedValuePath$ ID��6�    �� SelectedItem 6   �� *SelectedProject+��6h   �� *
Projects+6�   �� 51   
   $��2��6     Well#���  6!   �� 52   
   $��2��69   $��2��6F   $

 Name��6t   $ ID��6�    6   �� *SelectedWell+��6V   �� *Wells+6�   �� 53   
   $��4��6     Dataset#���  6!   �� 54   
   $��4��6<   $��2��6I   $

 Name��6z   $ ID��6�    6   �� *SelectedDataset+��6Y   �� *
Datasets+6�   �� 56   
   $��6��6    
	 Filter#��� 	 6!   �� 57   
   $��6��6   $��2��6   ��6/   �� *Filter+�� 59   
   $��8��6    
 
NewDataset#��� 
 6!   �� 5:   
   $��8��6   $��2��6   ��6/   �� *CurrentObject.Name+ �� Mode$ TwoWay=� �� UpdateSourceTrigger$ PropertyChanged=��� 5<   
   $��10��6     SystemNotes#���  6"   �� 5=   
   |�.   �� 
IsReadOnly .   TextBoxNotes#��[ 66   $��10��6\   $��2��6j   ��6z   �� *CurrentObject.SystemNotes+$ TwoWay=�$ PropertyChanged=�.��6�    5>        CustomScrollbar �� BasedOn��%�   '��       7  �� 5?      " ��6   8   .�5@      ��  � ��$ 0 0 -3 0q�6/   �� 5A       � ��$ 0q�60   �� 5B       � ��$ 10��6.   5C      5D      .��6!   5E      �� 5G   
   $��12��6     	UserNotes#���  6"   �� 5H   
   #��[ 6   $��12��68   $��2��6F   ��6V   �� *CurrentObject.UserNotes+$ TwoWay=�$ PropertyChanged=�.��6�    5I      ��%�   '��       7  �� 5J      " ��6   8   .�5K      ��  � ��$ 0 0 -3 0q�6/   �� 5L       � ��$ 0q�60   �� 5M       � ��$ 10��6.   5N      5O      .��6!   5P      �� 5R   
     MetroGroupBox#��[ 6   $	��360��6;   $��14��6G   $��0��6U   $��3��6e   ��5S      �� 5T        CurveProperties#���  6   $
��Bold"�6E   .��5V      �� .+�5W      ��   LineStyleProperties#��[ 6   �� 5X        MarkerStyleProperties#��[ 6   5Y      5Z      �� 5\   
     SaveCancelButton#��[ 6   $��16��6C   $��2��6Q   5]      5^      