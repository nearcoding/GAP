   M S B A M L   `   `   `  ���� A  =GAP.BL, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null/$clr-namespace:GAP.BL;assembly=GAP.BLGAP.BL  @ <GAP, Version=1.0.1.141, Culture=neutral, PublicKeyToken=null;"clr-namespace:GAP.ExtendedControlsGAP.ExtendedControls    GAP.AddEquationView   ,BLL$clr-namespace:GAP.BL;assembly=GAP.BL  +base"clr-namespace:GAP.ExtendedControls  \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml        WindowDialog#��[    � WindowStartupLocation$  CenterScreen=�  AddEquation � Title# �  $	��600��$	��380�� � 	Resources (   "GAP.BL.BooleanToVisibilityConveter  BooleanToVisbilityConverter �� 
TargetType �� Property �� Value�� %@   &	       '��        �� " ��.���  � ��$ Center=�.��� $��5q� � RowDefinitions ��   AutoHeightRowDefinition#��[ ��   MinHeightRowDefinition#��[ �� #��[ �� #��[ �� #��[ �� #��[ �� #��[ �� #��[ �� #��[ �� #��[ �� #��[ �� #��[ �� #��[ �� #��[ �� #��[ �� #��[ �� #��[ �� #��[  � ColumnDefinitions ��   AutoWidthColumnDefinition#��[ ��   MinWidthColumnDefinition#��[ �� #��[ .+���   Project#���  �� $
��Left=�$��2�� �� DisplayMemberPath$
 Name��	 �� SelectedValuePath$	 ID��
 �� SelectedItem
 �� *SelectedProject+���� *
Projects+�� $��2��  Well#���  �� $��2��$��2��$
 Name��$	 ID��
 �� *SelectedWell+���� *Wells+�� $��4�� 	 Dataset#��� 	 �� $��4��$��2��$
 Name��$	 ID��
 �� *SelectedDataset+���� *
Datasets+�� $��6�� 
 Equation#��� 
 �� $��6��$��2�� �� SelectedIndex �� *
Equation+.@��� $��Sum (Datasets)���� $��Subtraction (Datasets)���� $��Division (Datasets)���� $��Multiplication (Datasets)���� $��Log (base number)���� $��8��$��	Parameter���� $��8��$��2��$
 Name��$	 ID��y��� *!ParamVisibility+ �� 	Converter# [  �� Mode$ TwoWay=� �� UpdateSourceTrigger$ PropertyChanged=�
 �� *SelectedDataset2+���� *
Datasets+�� $��8��$��2��y��� *ParamVisibility+# [ $ TwoWay=�$ PropertyChanged=����� *EquationNumber+$ TwoWay=�$ PropertyChanged=��� $��10��  
NewDataset#���  �� $��10��$��2������ *CurrentObject.Name+$ TwoWay=�$ PropertyChanged=��� $��12�� 	 Notes#���  ��   TextBoxNotes#��[ $��12��$��2������ *CurrentObject.Notes+$ TwoWay=�$ PropertyChanged=�.����   MetroGroupBox#��[ $��14��$��0��$��3������   CurveProperties#���  $
��Bold"�.���� .+���   LineStyleProperties#��[ ��   MarkerStyleProperties#��[ ��   SaveCancelButton#��[ $��16��$��2��